using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using CouponClient.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace CouponClient
{
    public partial class ColMoGuJie : UserControl
    {
        public ColMoGuJie()
        {
            InitializeComponent();
        }

        public ChromiumWebBrowser chrome;


        public MoGuJie.Method mgj = new MoGuJie.Method();

        private const string LOGIN_URL = "http://www.immlm.cn/MoGuJie/Login?state=~/MoGuJie/LoginSuccess";

        public void InitChrome()
        {
            chrome = new ChromiumWebBrowser(LOGIN_URL);
            chrome.FrameLoadEnd += Chrome_FrameLoadEnd;
            chrome.AddressChanged += Chrome_AddressChanged;
            chrome.Dock = DockStyle.Fill;
            this.Controls.Add(chrome);
        }

        private void Chrome_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            SetAddress(e.Address);
        }

        public void InitLoad()
        {
            InitChrome();

        }

        public bool _enableRun;

        public Models.BuyUserInfo UserInfo { get; set; }

        /// <summary>
        /// 是否要运行
        /// </summary>
        public bool EnableRun
        {
            get
            {
                return _enableRun;
            }
            set
            {
                _enableRun = value;
            }

        }

        private async void Chrome_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            var url = e.Url.ToLower();
            if (!url.Contains("https://oauth.mogujie.com/authorize") && url.Contains("/mogujie/loginsuccess"))
            {
                var cookieManager = CefSharp.Cef.GetGlobalCookieManager();
                var html = await chrome.GetSourceAsync();
                var query = CsQuery.CQ.CreateDocument(html);
                mgj.Token = new MoGuJie.Token
                {
                    AccessToken = query.Find("#hidAccessToken").Val<string>(),
                    RefreshToken = query.Find("#hidRefreshToken").Val<string>(),
                    UserID = query.Find("#hidUserID").Val<string>(),
                };
                //CookieVisitor visitor = new CookieVisitor();
                //visitor.SendCookie += visitor_SendCookie;
                //cookieManager.VisitAllCookies(visitor);
                new Task(() => { LoadCoupon(); }).Start();

            }
        }

        public void LoadType(bool reload)
        {
            Action<string, object> saveResult = (name, obj) =>
             {
                 var p = $"{Config.RuningPath}\\temp\\{name}.json";
                 System.IO.File.WriteAllText(p, JToken.FromObject(obj).ToString(Formatting.Indented));
             };
            Func<string, string> readResult = name =>
            {
                var p = $"{Config.RuningPath}\\temp\\{name}.json";
                return System.IO.File.ReadAllText(p);
            };
            var d = new System.IO.DirectoryInfo($"{Config.RuningPath}\\temp");
            if (!d.Exists)
            {
                d.Create();
            }
            List<MoGuJie.Cid2> r = new List<MoGuJie.Cid2>();
            if (reload)
            {
                r = mgj.GetAllCategory2();
                var r2 = new List<MoGuJie.Cid2>();
                foreach (var item in r)
                {
                    var id = item.ID;
                    r2.AddRange(mgj.GetAllCategory2(id));
                }
                var r3 = new List<MoGuJie.Cid2>();
                foreach (var item in r2)
                {
                    var id = item.ID;
                    r3.AddRange(mgj.GetAllCategory2(id));
                }

                r.AddRange(r2);
                r.AddRange(r3);
                foreach (var item in r)
                {
                    var id = mgj.DecryptID(item.ID);
                    item.DID = id;
                }
                saveResult("cid", r);
            }
            else
            {
                r = JsonConvert.DeserializeObject<List<MoGuJie.Cid2>>(readResult("cid"));
            }
            var dNames = r.GroupBy(s => s.Name)
                  .Select(s => new
                  {
                      Name = s.Key,
                      Count = s.Count()
                  }).Where(s => s.Count > 1)
                  .Select(s => s.Name).ToList();
            foreach (var item in dNames)
            {
                var rTemp = r.Where(s => s.Name == item).ToList();
                foreach (var temp in rTemp)
                {
                    var pName = r.FirstOrDefault(s => s.ID == temp.ParentId)?.Name;
                    pName = pName == null ? "" : $"{pName}+";
                    temp.Name = $"{pName}{temp.Name}";
                }
            }
            foreach (var item in r)
            {
                item.Count = mgj.GetItemList(cid: item.DID).Total;
            }
            saveResult("temp2", r);


        }

        public void LoadCoupon()
        {
            OnStateChange?.Invoke(Enums.StateLogType.MoGuJieGetCouponStart, "开始加载蘑菇街优惠券");
            var cids = MoGuJie.Method.AllCategory;
            var pageSize = 100;
            var models = new List<Coupon>();
            var checkCount = new Api.BuyApi("Count", "MoGuJie", new { UserID = UserInfo.ID }, "GET").CreateRequestReturnBuyResult<JToken>();
            bool onlyFirstPage = checkCount.Result["Count"].Value<int>() > 10000;
            foreach (var cid in cids)
            {
                if (!EnableRun)
                {
                    return;
                }

                try
                {
                    //获取第一页
                    var result = mgj.GetItemList(pageNo: 1, pageSize: pageSize, cid: cid.DID);
                    Action<IEnumerable<Coupon>> set = items =>
                    {
                        //设置导入没有的字段
                        foreach (var item in items)
                        {
                            item.ProductType = cid.Name;
                            item.TypeID = cid.TypeID;
                            item.UserID = UserInfo.ID;
                        }
                    };

                    //已经初始化过了，就不往下加载了
                    if (result.Total > 0)
                    {
                        set(result.Items);
                        models.AddRange(result.Items);
                        //获取
                        if (!onlyFirstPage)
                        {
                            for (int i = 2; i <= result.TotalPage; i++)
                            {
                                var result2 = mgj.GetItemList(pageNo: i, pageSize: pageSize, cid: cid.DID);
                                set(result2.Items);
                                models.AddRange(result2.Items);
                            }

                        }

                    }
                }
                catch (Exception ex)
                {
                    SystemBase.WriteLog(ex.Message, "MoGuJie");
                }
            }


            OnStateChange?.Invoke(Enums.StateLogType.MoGuJieGetCouponComplated, "加载蘑菇街优惠券加载完成");
            var dirInfo = new System.IO.DirectoryInfo($"{Config.RuningPath}\\temp");

            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            var path = $"{Config.RuningPath}\\temp\\蘑菇街_{DateTime.Now:yyyyMMddHHmm}.json";
            var fileInfo = new System.IO.FileInfo(path);
            System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(models));

            try
            {
                OnStateChange?.Invoke(Enums.StateLogType.MoGuJieCouponAddDbStart, "蘑菇街提交开始");
                var files = new Dictionary<string, string>();
                files.Add("file", path);

                var upload = new Api.BuyUploadApi(files).CreateRequestReturnUrls();
                var update = new Api.BuyApi("ImportItems", "MoGuJie", new { Url = upload[0] }).CreateRequestReturnBuyResult<object>();
                if (update.State == "Success")
                {
                    OnStateChange?.Invoke(Enums.StateLogType.MoGuJieCouponAddDbComplated, "蘑菇街提交完成");
                    try
                    {
                        System.IO.File.Delete(path);
                    }
                    catch (Exception)
                    {
                        
                    }
                   
                }
                else
                {
                    OnStateChange?.Invoke(Enums.StateLogType.MoGuJieCouponAddDbError, update.Message);
                }
            }
            catch (Exception ex)
            {
                OnStateChange?.Invoke(Enums.StateLogType.MoGuJieCouponAddDbError, $"蘑菇街提交有误");
            }
            finally
            {
                try
                {
                    System.IO.File.Delete(path);
                }
                catch (Exception ex)
                {
                    OnStateChange?.Invoke(Enums.StateLogType.MoGuJieCouponDeleteTemp, $"蘑菇街提交有误");
                }

            }

            var task = new Task(() =>
            {
                Thread.Sleep(5 * 60 * 1000);
                mgj.RefeashToken();
                LoadCoupon();
            });
            task.Start();
        }


        private void visitor_SendCookie(Cookie obj)
        {
            if (obj.Name == "MGJToken")
            {
                var token = JsonConvert.DeserializeObject<MoGuJie.Token>(obj.Value);
                mgj.Token = token;
                OnStateChange?.Invoke(Enums.StateLogType.MoGuJieSignSuccess, "蘑菇街授权成功");
                new Task(() => { LoadCoupon(); }).Start();

            }
        }

        public void SetAddress(string address)
        {
            if (address.ToLower() == "about:blank")
            {
                return;
            }
            Action set = () =>
            {
                txtAddress.Text = address;
            };
            if (toolStrip1.InvokeRequired)
            {
                toolStrip1.Invoke(set);
            }
            else
            {
                set();
            }

        }

        #region 委托事件
        public delegate void StateChangeEventHandler(Enums.StateLogType type, string message);

        public event StateChangeEventHandler OnStateChange;
        #endregion

        private void txtAddress_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode== Keys.Enter)
            {
                chrome.Load(txtAddress.Text);
            }
           
        }

        private void toolStrip1_Resize(object sender, EventArgs e)
        {
            txtAddress.Width = toolStrip1.Bounds.Right - btnReload.Bounds.Right - 10;
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            chrome.Load(LOGIN_URL);
        }
    }
}
