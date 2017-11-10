using System;
using System.Windows.Forms;
using CefSharp;
using CouponClient.AppStart.Extensions;
using CefSharp.WinForms;
using System.IO;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using CsQuery;
using System.Net;
namespace CouponClient
{
    public partial class ColTaoBao : UserControl
    {
        public Models.BuyUserInfo UserInfo { get; set; }

        public ChromiumWebBrowser chrome;

        public Models.DownloadHandler downloadHandler;


        public Models.BuyUserInfo dlProxy;

        #region 淘宝地址
        /// <summary>
        /// 登录地址
        /// </summary>
        private const string LOGIN_URL = "https://login.taobao.com/member/login.jhtml?style=mini&newMini2=true&from=alimama&redirectURL=http%3A%2F%2Flogin.taobao.com%2Fmember%2Ftaobaoke%2Flogin.htm%3Fis_login%3d1&full_redirect=true&disableQuickLogin=true";

        /// <summary>
        /// 阿里妈妈首页，登录成功后自动跳这个页面
        /// </summary>
        private const string ALIMAMA_HOMEPAGE_URL = "https://www.alimama.com/index.htm";

        /// <summary>
        /// 优惠券的下载页面
        /// </summary>
        private const string COUPON_DOWNLOAD_URL = "https://pub.alimama.com/myunion.htm?spm=a219t.7900221/1.1998910419.dbb742793.21214865Jx8f2X#!/promo/self/items";
        #endregion


        public ColTaoBao()
        {
            InitializeComponent();

        }

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
            chrome = new ChromiumWebBrowser(url);
            chrome.FrameLoadEnd += Chrome_FrameLoadEnd;
            chrome.AddressChanged += Chrome_AddressChanged;
            chrome.Dock = DockStyle.Fill;
            chrome.DownloadHandler = downloadHandler;
            plChrome.Controls.Add(chrome);
        }

        private void Chrome_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            SetAddress(e.Address);
            if (e.Address.Contains(ALIMAMA_HOMEPAGE_URL))
            {
                OnStateChange?.Invoke(Enums.StateLogType.TaoBaoSignSuccess, "登录淘宝成功");
                chrome.Load(COUPON_DOWNLOAD_URL);
            }
        }

        public void InitLoad()
        {
            InitChrome();

        }

        private void DownloadHandler_OnDownloadStart(IBrowser browser, DownloadItem downloadItem)
        {
            OnStateChange?.Invoke(Enums.StateLogType.TaoBaoCouponDownloadStart, $"代理{dlProxy.PhoneNumber}的{downloadItem.SuggestedFileName} 下载开始");
        }

        private void DownloadHandler_OnDownloadComplete(DownloadItem downloadItem, string path)
        {
            string fileName = new FileInfo(path).Name;
            OnStateChange?.Invoke(Enums.StateLogType.TaoBaoCouponDownloadStart, $"代理{dlProxy.PhoneNumber}的{fileName} 下载完成");

            new Task(() =>
            {
                UploadTaobao(path);
            }).Start();



        }

        private void DownloadHandler_OnDownloading(IBrowser browser, DownloadItem downloadItem)
        {

        }

        private void btnDebug_Click(object sender, EventArgs e)
        {
            chrome.ShowDevTools();
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

        private void Chrome_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            var url = e.Url;
            try
            {
                string path = $"{System.Environment.CurrentDirectory}\\jquery-3.2.1.min.js";
                if (File.Exists(path))
                {
                    chrome.ExecuteScriptAsync(File.ReadAllText($"{System.Environment.CurrentDirectory}\\jquery-3.2.1.min.js"));
                }
                else
                {
                    OnStateChange.Invoke(Enums.StateLogType.TaoBaoCouponUploadFail, $"jquey不存在");
                }
            }
            catch (Exception ex)
            {
                OnStateChange.Invoke(Enums.StateLogType.TaoBaoCouponUploadFail, $"jquey加载失败{ex.Message}");
            }
            if (url.Contains(COUPON_DOWNLOAD_URL))
            {
                Download();
            }
        }

        public async void Download()
        {
            if (!EnableRun)
            {
                return;
            }
            try
            {
                //获取自己和所有二级代理，今天采集的淘宝券
                var proxys = Bll.BuyApis.GetProxyCouponCount(UserInfo.ID, Enums.Platform.TaoBao)
                    .Where(s => !string.IsNullOrWhiteSpace(s.PhoneNumber)
                        && s.Count < 1000)
                    .ToList();
                if (proxys.Count > 0)
                {
                    await chrome.Wait(".list-desc", 10000, async () =>
                     {
                         chrome.ExecuteScriptAsync("$('.list-desc:last').click()");
                         await chrome.Wait(".dialog-overlay button[mx-click=submit]", 10000, async () =>
                            {
                                var html = await chrome.GetSourceAsync();
                                var doc = CQ.CreateDocument(html);
                                var list = doc.Select("#J_zones_dropdown .dropdown-list li span");
                                var proxy = proxys[0];
                                //获取联盟广告位
                                var advs = list.Select(s => WebUtility.HtmlDecode(s.InnerText)).ToList();
                                var adv = advs.FirstOrDefault(s => s == proxy.PhoneNumber);
                                dlProxy = new Models.BuyUserInfo { ID = proxy.UserID, PhoneNumber = proxy.PhoneNumber };
                                if (adv == null)//广告位里没有该手机号添加一个
                                {
                                    chrome.ExecuteScriptAsync($"$('#J_newzone_radio').click();$('#J_new_zone').val('{proxy.PhoneNumber}');");
                                }
                                else//广告位里有选择一个
                                {
                                    var i = advs.IndexOf(adv);
                                    chrome.ExecuteScriptAsync($"$('#J_zones_dropdown .dropdown-hd').click();$('#J_zones_dropdown .dropdown-list li:nth-child({i})').click();");
                                }
                                downloadHandler.Set(prefix: $"{proxy.PhoneNumber}_");
                                chrome.ExecuteScriptAsync("$('.dialog-overlay button[mx-click=submit]').click();");
                            });

                     });
                }
                else
                {
                    OnStateChange?.Invoke(Enums.StateLogType.TaoBaoCouponAddDbComplated, $"未检测到有新淘宝商品");
                    Task task = new Task(() =>
                    {
                        System.Threading.Thread.Sleep(5 * 60 * 1000);
                        chrome.Load(COUPON_DOWNLOAD_URL);
                    });
                    task.Start();
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

        private void ColBrowser_Load(object sender, EventArgs e)
        {

        }



        public void UploadTaobao(string path)
        {
            var fileInfo = new FileInfo(path);
            var fileName = fileInfo.Name;
            string fileUrl = null;
            try
            {
                OnStateChange?.Invoke(Enums.StateLogType.TaoBaoCouponUploadStart, $"代理{dlProxy.PhoneNumber}的{fileName}开始上传");
                Dictionary<string, string> files = new Dictionary<string, string>();
                files.Add("file", path);
                var uploadFileApi = new Api.BuyUploadApi(files);
                fileUrl = uploadFileApi.CreateRequestReturnUrls()[0];
                OnStateChange?.Invoke(Enums.StateLogType.TaoBaoCouponUploadComplated, $"代理{dlProxy.PhoneNumber}的{fileName}上传完成");
                try
                {

                    OnStateChange(Enums.StateLogType.TaoBaoCouponAddDbStart, $"代理{dlProxy.PhoneNumber}的{fileName}开始处理");
                    var api = new Api.BuyApi("Import", "Taobao", new
                    {
                        UserID = dlProxy.ID,
                        Url = fileUrl
                    });

                    var result = api.CreateRequestReturnBuyResult<object>();
                    if (result.State == "Success")
                    {
                        OnStateChange?.Invoke(Enums.StateLogType.TaoBaoCouponAddDbComplated, $"代理{dlProxy.PhoneNumber}的{fileName}处理完成");
                    }
                    else
                    {
                        OnStateChange?.Invoke(Enums.StateLogType.TaoBaoCouponAddDbFail, $"代理{dlProxy.PhoneNumber}的{fileName}处理失败");
                    }
                }
                catch (Exception ex)
                {
                    OnStateChange?.Invoke(Enums.StateLogType.TaoBaoCouponDownloadFail, $"{fileName}处理失败");
                    SystemBase.WriteLog($"提交失败 {ex.Message}", "UploadTaobao");
                }
            }
            catch (Exception ex)
            {
                OnStateChange?.Invoke(Enums.StateLogType.TaoBaoCouponUploadFail, $"代理{dlProxy.PhoneNumber}的{fileName}上传失败");
                SystemBase.WriteLog($"上传失败 {ex.Message}", "UploadTaobao");
            }
            finally
            {
                try
                {
                    fileInfo.Delete();
                }
                catch (Exception ex)
                {
                    OnStateChange?.Invoke(Enums.StateLogType.TaoBaoCouponDeleteTemp, $"代理{dlProxy.PhoneNumber}的{fileName}缓存文件删除失败");
                }
            }
            Task task = new Task(() =>
            {
                chrome.Load(COUPON_DOWNLOAD_URL);
            });
            task.Start();
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
