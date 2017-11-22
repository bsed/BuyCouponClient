using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp.WinForms;
using CefSharp;
using CouponClient.AppStart.Extensions;
using CsQuery;
using System.IO;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace CouponClient
{
    public partial class ColJd : UserControl
    {
        public ColJd()
        {
            InitializeComponent();
        }

        public Models.BuyUserInfo UserInfo { get; set; }

        public ChromiumWebBrowser chrome;

        public Models.DownloadHandler downloadHandler;

        public Models.JsDialogHandler jsDialogHandler;

        public Models.BuyUserInfo dlProxy;

        private Models.JdCidUrl[] cidLinks = Bll.Jd.AllCids;


        private ObservableCollection<Models.JdCidLog> cidLogs = Bll.Jd.Logs;

        private List<Models.ProxyCouponCount> ProxyUsers;
        #region 京东地址
        /// <summary>
        /// 联盟登录页面
        /// </summary>
        private const string LOGIN_URL = "https://media.jd.com/";

        /// <summary>
        /// 联盟首页
        /// </summary>
        private const string HOME_PAGE_URL = "https://media.jd.com/index/overview";

        /// <summary>
        /// 专享价商品页面
        /// </summary>
        private const string GOTOADV_URL = "https://media.jd.com/gotoadv/pickupdate";
        #endregion

        #region 事件委托定义
        public delegate void StateChangeEventHandler(Enums.StateLogType type, string message);

        public event StateChangeEventHandler OnStateChange;
        #endregion

        private bool _enableRun = false;

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

        public void InitChrome()
        {
            downloadHandler = new Models.DownloadHandler();
            downloadHandler.OnDownloadStart += DownloadHandler_OnDownloadStart;
            downloadHandler.OnDownloading += DownloadHandler_OnDownloading;
            downloadHandler.OnDownloadComplete += DownloadHandler_OnDownloadComplete;
            var url = LOGIN_URL;
            jsDialogHandler = new Models.JsDialogHandler();
            jsDialogHandler.OnJSDialogShow += JsDialogHandler_OnJSDialogShow;
            chrome = new ChromiumWebBrowser(url);
            chrome.FrameLoadEnd += Chrome_FrameLoadEnd;
            chrome.AddressChanged += Chrome_AddressChanged;
            chrome.Dock = DockStyle.Fill;
            chrome.DownloadHandler = downloadHandler;
            chrome.JsDialogHandler = jsDialogHandler;
            plChrome.Controls.Add(chrome);
        }

        /// <summary>
        /// 拦截警告
        /// </summary>
        /// <param name="message"></param>
        private void JsDialogHandler_OnJSDialogShow(string message)
        {
            //拦截下载速度过快
            if (message == "下载速度太快，请稍后再试！")
            {
                chrome.ExecuteScriptAsync("setTimeout(function () { $('#getcode-btn').click() }, 1000);");
            }

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F12)
            {
                chrome.ShowDevTools();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public List<Models.Coupon> TempCoupon { get { return Bll.Jd.TempCoupons.Data; } }

        public void InitLoad()
        {
            InitChrome();
            ProxyUsers = Bll.Buy.GetProxyCouponCount(UserInfo.ID, Enums.Platform.JD)
                    .Where(s => !string.IsNullOrWhiteSpace(s.PhoneNumber)
                        && s.Count < 1000)
                    .ToList();
        }

        private void ColJd_Load(object sender, EventArgs e)
        {

        }

        private void Chrome_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            SetAddress(e.Address);
            if (e.Address.Contains(HOME_PAGE_URL))
            {
                OnStateChange?.Invoke(Enums.StateLogType.TaoBaoSignSuccess, "登录京东成功");

                if (NoLoadCids.Count == 0)
                {
                    OnStateChange?.Invoke(Enums.StateLogType.JdCouponAddedDb, "暂无新数据库");
                }
                var cid = NoLoadCids.FirstOrDefault();

                LoadAddress(cid.CidUrls.FirstOrDefault().Url);
            }
        }

        private void LoadAddress(string url)
        {
            chrome.ExecuteScriptAsync($"location='{url}'");
        }

        /// <summary>
        /// 还没加载的分类
        /// </summary>
        private List<Models.JdUserCidUrl> NoLoadCids
        {
            get
            {
                var hadLoad = cidLogs
                      .Where(s => s.CreateDateTime > DateTime.Now.AddDays(-1))
                      .ToList();
                var model = ProxyUsers.Select(s => new Models.JdUserCidUrl
                {
                    UserID = s.UserID,
                    PhoneNumber = s.PhoneNumber,
                    CidUrls = cidLinks.Where(x => !hadLoad.Any(y => y.UserID == s.UserID && y.Cid == x.Cid)).ToArray()

                }).ToList();
                //var model = (from log in cidLinks
                //             from u in ProxyUsers
                //             select new Models.JdUserCidUrl
                //             {
                //                 Cid = log.Cid,
                //                 Url = log.Url,
                //                 UserID = u.UserID
                //             }).OrderBy(s => s.UserID)
                //       .ThenBy(s => s.Cid)
                //       .ToList();
                //var loaded = (from m in model
                //              from h in hadLoad
                //              where m.UserID == h.UserID && m.Cid == h.Cid
                //              select m).ToList();
                //model = model.Except(loaded).ToList();
                return model;

            }
        }

        private void Chrome_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            var url = e.Url.ToLower();
            if (url.Contains(GOTOADV_URL))
            {
                Download();

            }
        }

        private async void DownloadHandler_OnDownloadComplete(DownloadItem downloadItem, string path)
        {
            //当前代理
            var noload = NoLoadCids.FirstOrDefault();
            var fileInfo = new FileInfo(path);
            OnStateChange?.Invoke(Enums.StateLogType.JdCouponDownloadComplated, $"Jd{fileInfo.Name}下载完成");
            var html = await chrome.GetSourceAsync();
            var cid = noload.CidUrls.FirstOrDefault();
            var newCoupon = Bll.Jd.GetCouponsFromExcel(noload.UserID, cid, html, path);
            //缓存到本地的json文件
            Bll.Jd.TempCoupons.Update(newCoupon);
            if (Page.IsLast)//该类目的最后一页
            {
                //记录该
                cidLogs.Add(new Models.JdCidLog { Cid = cid.Cid, CreateDateTime = DateTime.Now, UserID = noload.UserID });
                //导入
                string phoneNumber = noload.PhoneNumber;
                Bll.Jd.Import(TempCoupon, (state) =>
                {
                    switch (state)
                    {
                        case Enums.StateLogType.JdCouponAddDbComplated:
                            OnStateChange(state, $"代理 {phoneNumber} 分类 {cid.Cid} 导入完成");
                            break;
                        case Enums.StateLogType.JdCouponAddDbFail:
                            OnStateChange(state, $"代理 {phoneNumber} 分类 {cid.Cid} 导入失败");
                            break;
                        case Enums.StateLogType.JdCouponAddDbStart:
                            OnStateChange(state, $"代理 {phoneNumber} 分类 {cid.Cid} 导入开始");
                            break;
                        case Enums.StateLogType.JdCouponUploadComplated:
                            OnStateChange(state, $"代理 {phoneNumber} 分类 {cid.Cid} 上传开始");
                            break;
                        case Enums.StateLogType.JdCouponUploadFail:
                            OnStateChange(state, $"代理 {phoneNumber} 分类 {cid.Cid} 上传失败");
                            break;
                        case Enums.StateLogType.JdCouponUploadStart:
                            OnStateChange(state, $"代理 {phoneNumber} 分类 {cid.Cid} 上传开始");
                            break;
                        default:
                            break;
                    }
                });
                //导入完成后清空缓存
                Bll.Jd.TempCoupons.Clear();
                if (noload == null)
                {
                    OnStateChange(Enums.StateLogType.JdCouponAddedDb, $"全部代理已经采集完成");
                }
                else
                {
                    var next = NoLoadCids.FirstOrDefault()?.CidUrls.FirstOrDefault().Url;
                    if (!string.IsNullOrWhiteSpace(next))
                    {
                        LoadAddress(next);
                        //chrome.Load(next);
                    }
                }
            }
            else
            {
                chrome.ExecuteScriptAsync($"pageClick({Page.Number + 1})");
            }
            fileInfo.Delete();
        }

        private void DownloadHandler_OnDownloading(IBrowser browser, DownloadItem downloadItem)
        {
            //throw new NotImplementedException();
        }

        private void DownloadHandler_OnDownloadStart(IBrowser browser, DownloadItem downloadItem)
        {
            OnStateChange?.Invoke(Enums.StateLogType.JdCouponDownloadStart, $"Jd{downloadItem.SuggestedFileName}下载中");
        }


        private void toolStrip1_Resize(object sender, EventArgs e)
        {
            txtAddress.Width = toolStrip1.Bounds.Right - btnReload.Bounds.Right - 10;
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

        public Models.JdPage Page { get; set; } = new Models.JdPage();

        public async void Download()
        {
            if (!EnableRun)
            {
                return;
            }
            try
            {
                //获取自己和所有二级代理，今天采集的京东
                var proxys = ProxyUsers;
                if (proxys.Count > 0)
                {
                    var scrPage = await chrome.EvaluateScriptAsync("(function (){ return $('.page-num').text()})()");
                    var strPage = scrPage.Result.ToString().SplitToArray<int>('/');
                    Page.Number = strPage[0];
                    Page.Total = strPage[1];

                    var proxy = proxys.FirstOrDefault();

                    chrome.ExecuteScriptAsync("$('#bitch-checkbox').click()");
                    chrome.ExecuteScriptAsync("$('.btn-bitch').click()");
                    Thread.Sleep(100);
                    chrome.ExecuteScriptAsync("$('#adtType_4').click()");
                    try
                    {

                        await chrome.Wait("#spaceName option:nth-child(2)", 10000, async () =>
                           {

                               chrome.ExecuteScriptAsync("$('#protocol_1,#channel_2').click()");
                               var html = await chrome.GetSourceAsync();
                               var doc = CQ.CreateDocument(html);
                               var spaceNames = doc.Select("#spaceName option")
                                   .Select(s => WebUtility.HtmlDecode(s.InnerText).Trim())
                                   .ToList();


                               var index = spaceNames.IndexOf(proxy.PhoneNumber);
                               if (index > 0)
                               {
                                   chrome.ExecuteScriptAsync($"$('#spaceName option:nth-child({index + 1})').prop('selected',true);");
                               }
                               else
                               {
                                   chrome.ExecuteScriptAsync($"$('#kind_2').click();$('#positionName').val('{proxy.PhoneNumber}');");
                               }

                               downloadHandler.Set(prefix: $"{proxy.PhoneNumber}_{NoLoadCids.FirstOrDefault().CidUrls.FirstOrDefault().Cid}_", suffix: $"_{Page.Number}_{Page.Total}");
                               chrome.ExecuteScriptAsync("$('#getcode-btn').click()");
                           });


                    }
                    catch (Exception ex)
                    {
                        OnStateChange?.Invoke(Enums.StateLogType.JdCouponColumnError, "找不到广告位");
                        LoadAddress(chrome.Address);
                    }



                }
                else
                {
                    //OnStateChange?.Invoke(Enums.StateLogType.TaoBaoCouponAddDbComplated, $"未检测到有新淘宝商品");
                    //Task task = new Task(() =>
                    //{
                    //    System.Threading.Thread.Sleep(5 * 60 * 1000);
                    //    chrome.Load(COUPON_DOWNLOAD_URL);
                    //});
                    //task.Start();
                }
            }
            catch (Exception ex)
            {
                SystemBase.WriteLog($"加载错误{ex.Message}", "error");
            }

        }

        private void txtAddress_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadAddress(txtAddress.Text);
                //chrome.Load(txtAddress.Text);
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            chrome.Load(LOGIN_URL);
        }
    }
}
