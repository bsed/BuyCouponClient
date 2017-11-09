using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CouponClient
{
    public partial class Main : Form
    {
        private FrmLogin frmLogin;

        private Models.BuyUserInfo userinfo;

        private bool enableRun = false;

        private bool EnableRun
        {
            get
            {
                return enableRun;
            }
            set
            {
                enableRun = value;
                colMoGuJie.EnableRun = colMoGuJie.EnableRun = enableRun;
            }
        }



        public Main()
        {
            try
            {
                var ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                InitializeComponent();
                CefSettings settings = new CefSettings();
                settings.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36";
                Cef.Initialize(settings);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void Main_Load(object sender, EventArgs e)
        {

            Hide();
            frmLogin = new FrmLogin();
            frmLogin.OnLoginSuccess += FrmLogin_OnLoginSuccess;
            frmLogin.FormClosing += FrmLogin_FormClosing;
            frmLogin.ShowDialog();
            tab.SelectedTab = tabJd;
            //tab.TabPages.Remove(tabJd);
            MonitorNetworkSpeed.NetworkSpeedChanged += MonitorNetworkSpeed_NetworkSpeedChanged;

        }



        private void FrmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Close();
            }

        }

        private void FrmLogin_OnLoginSuccess(Models.BuyUserInfo user)
        {
            EnableRun = true;
            userinfo = user;

            frmLogin.Hide();
            Action show = () =>
            {
                Show();
            };
            Invoke(show);

            Action initColTaobao = () =>
            {
                colTaobao.EnableRun = EnableRun;
                colTaobao.UserInfo = user;
                colTaobao.InitLoad();
                colTaobao.OnStateChange += OnStateChange;
            };

            Invoke(initColTaobao);

            Action initColMoGuJie = () =>
            {
                colMoGuJie.EnableRun = EnableRun;
                colMoGuJie.UserInfo = user;
                colMoGuJie.InitLoad();
                colMoGuJie.OnStateChange += OnStateChange;
            };

            Invoke(initColMoGuJie);


            Action initColJd = () =>
            {

                colJd.EnableRun = EnableRun;
                colJd.UserInfo = user;
                colJd.InitLoad();
                colJd.OnStateChange += OnStateChange;
            };
            Invoke(initColJd);
        }



        #region 事件处理
        private void OnStateChange(Enums.StateLogType type, string message)
        {
            //Action temp = () =>
            //{
            //    tab.SelectedTab = tabLog;
            //};
            //tab.Invoke(temp);
            WriteLog(message);
        }

        #endregion

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            colMoGuJie.Dispose();
            colTaobao.Dispose();
            Cef.Shutdown();
            Dispose();
        }


        public void WriteLog(string content)
        {
            Action set = () =>
            {
                txtLog.AppendText($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {content}\r\n");
            };
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(set);
            }
            else
            {
                set();
            }

        }

        /// <summary>
        /// 检测看有没有多点登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            if (EnableRun)
            {
                var api = new Api.BuyApi("CheckClientCode", "Account", new { ID = userinfo.ID, Code = userinfo.Code });
                var result = api.CreateRequestReturnBuyResult<object>();
                if (result.State != "Success")
                {
                    EnableRun = false;
                    frmLogin.Show();
                    MessageBox.Show("检测到在有你的帐号在别的地方登陆");
                }
            }
        }

        private void menuLoginOut_Click(object sender, EventArgs e)
        {
            Config.ConfigSetting.Password = "";
            Config.ConfigSetting = Config.ConfigSetting;
            this.Hide();

            frmLogin.ShowDialog();
            EnableRun = false;
            frmLogin.Logout();
        }

        private void menuClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MonitorNetworkSpeed_NetworkSpeedChanged(float sent, float received)
        {
            string strSent, strReceived;
            if (sent / Math.Pow(1024, 2) > 1)
            {
                strSent = $"{Math.Round(sent / Math.Pow(1024, 2), 2)}Mb/s";
            }
            else
            {
                strSent = $"{Math.Round(sent / Math.Pow(1024, 1), 2)}Kb/s";
            }
            if (received / Math.Pow(1024, 2) > 1)
            {
                strReceived = $"{Math.Round(received / Math.Pow(1024, 2), 2)}Mb/s";
            }
            else
            {
                strReceived = $"{Math.Round(received / Math.Pow(1024, 1), 2)}Kb/s";
            }
            SetSpeed($"{strReceived}↓ {strSent}↑ ");
        }


        private void SetSpeed(string txt)
        {
            Action set = () =>
            {
                lblSpeed.Text = txt;
            };
            if (stauts.InvokeRequired)
            {
                stauts.Invoke(set);
            }
            else
            {
                set();
            }

        }

        private void menuHelpPage_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://client.caijimao.cn/help");
        }
    }


}
