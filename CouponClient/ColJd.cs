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

        private void JsDialogHandler_OnJSDialogShow(string message)
        {
            chrome.ExecuteScriptAsync("setTimeout(function () { $('#getcode-btn').click() }, 1000);");
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F12)
            {
                chrome.ShowDevTools();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public List<Models.Coupon> TempCoupon { get; set; } = new List<Models.Coupon>();

        public void InitLoad()
        {
            InitChrome();
            ProxyUsers = Bll.BuyApis.GetProxyCouponCount(UserInfo.ID, Enums.Platform.JD)
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
                chrome.Load(cid.Url);
            }
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
                var model = (from log in cidLinks
                             from u in ProxyUsers
                             select new Models.JdUserCidUrl
                             {
                                 Cid = log.Cid,
                                 Url = log.Url,
                                 UserID = u.UserID
                             }).OrderBy(s => s.UserID)
                       .ThenBy(s => s.Cid)
                       .ToList();
                var loaded = (from m in model
                              from h in hadLoad
                              where m.UserID == h.UserID && m.Cid == h.Cid
                              select m).ToList();
                model = model.Except(loaded).ToList();
                return model;

            }
        }





        private void Chrome_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            var url = e.Url.ToLower();
            if (url.Contains(GOTOADV_URL))
            {
                //防止加载过快问题
                //chrome.ExecuteScriptAsync("window.alert = function (message) { if (message == '下载速度太快，请稍候再试！') { setTimeout(function () { $('#getcode-btn').click() }, 1000) } }");
                //chrome.ExecuteScriptAsync("window.alert = function (message) { if (message == '下载速度太快，请稍后再试！') { setTimeout(function () { $('body').css('color', 'red') }, 1000); } else { $('body').css('color', 'green'); $('body').append(message); } }");
                chrome.ExecuteScriptAsync("$('#ifile')[0].contentWindow.alert = function (message) { console.log(message) };");
                //chrome.ExecuteScriptAsync("alert('下载速度太快，请稍候再试！')");
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
            var cid = NoLoadCids.FirstOrDefault().Cid;
            var newCoupon = Bll.Jd.GetCouponsFromExcel(noload.UserID, cid, html, path);
            TempCoupon.AddRange(newCoupon);
            if (Page.IsLast)//该类目的最后一页
            {
                //记录该
                cidLogs.Add(new Models.JdCidLog { Cid = cid, CreateDateTime = DateTime.Now, UserID = noload.UserID });
                var next = NoLoadCids.FirstOrDefault()?.Url;
                if (!string.IsNullOrWhiteSpace(next))
                {
                    chrome.Load(next);
                }
                else //没有下一个了CidUrl,该代理已经上传完成
                {
                    string phoneNumber = noload.PhoneNumber;
                    Bll.Jd.Import(TempCoupon, (state) =>
                    {
                        switch (state)
                        {
                            case Enums.StateLogType.JdCouponAddDbComplated:
                                OnStateChange(state, $"代理 {phoneNumber} JD导入完成");
                                break;
                            case Enums.StateLogType.JdCouponAddDbFail:
                                OnStateChange(state, $"代理 {phoneNumber} JD导入失败");
                                break;
                            case Enums.StateLogType.JdCouponAddDbStart:
                                OnStateChange(state, $"代理 {phoneNumber} JD导入开始");
                                break;
                            case Enums.StateLogType.JdCouponUploadComplated:
                                OnStateChange(state, $"代理 {phoneNumber} JD上传开始");
                                break;
                            case Enums.StateLogType.JdCouponUploadFail:
                                OnStateChange(state, $"代理 {phoneNumber} JD上传失败");
                                break;
                            case Enums.StateLogType.JdCouponUploadStart:
                                OnStateChange(state, $"代理 {phoneNumber} JD上传开始");
                                break;
                            default:
                                break;
                        }
                    });
                    //导入完成后清空
                    TempCoupon = new List<Models.Coupon>();
                    if (NoLoadCids.Count == 0)
                    {
                        OnStateChange(Enums.StateLogType.JdCouponAddedDb, "已经采集完成");
                    }
                }
            }
            else
            {
                //回避下载过快问题

                chrome.ExecuteScriptAsync($"pageClick({Page.Number + 1})");
            }
            //Thread.Sleep(5000);
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

                               downloadHandler.Set(prefix: $"{proxy.PhoneNumber}_{NoLoadCids.FirstOrDefault().Cid}_", suffix: $"_{Page.Number}_{Page.Total}");
                               chrome.ExecuteScriptAsync("$('#getcode-btn').click()");
                           });


                    }
                    catch (Exception ex)
                    {
                        OnStateChange?.Invoke(Enums.StateLogType.JdCouponColumnError, "找不到广告位");
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
                chrome.Load(txtAddress.Text);
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            chrome.Load(LOGIN_URL);
        }
    }
}
