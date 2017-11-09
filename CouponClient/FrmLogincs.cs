using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CouponClient
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        public delegate void LoginSuccessHandler(Models.BuyUserInfo user);

        public event LoginSuccessHandler OnLoginSuccess;

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Action login = () =>
            {
                Action<bool> setBtn = enabled =>
                {
                    btnLogin.Enabled = enabled;
                };
                try
                {
                    btnLogin.Invoke(setBtn, false);

                    Login(txtUserName.Text, txtPassword.Text);
                }
                catch (Exception ex)
                {
                    SetErrorMessage(ex.Message);
                    btnLogin.Invoke(setBtn, true);
                }
            };
            Task tskLogin = new Task(login);
            tskLogin.Start();

        }

        public void Logout()
        {
            btnLogin.Enabled = true;
            txtPassword.Text = "";
            ckbRemember.Checked = false;
        }

        private void Login(string userName, string password)
        {
            try
            {
                var user = Bll.BuyApis.Login(userName, password);
                var config = Config.ConfigSetting;
                config.UserName = userName;
                OnLoginSuccess(user);
                if (ckbRemember.Checked)
                {
                    config.Password = Security.Encrypt(password, userName);
                }
                Config.ConfigSetting = config;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetErrorMessage(string message)
        {
            Action set = () =>
            {
                lblError.Text = message;
            };
            if (lblError.InvokeRequired)
            {
                lblError.Invoke(set);
            }
            else
            {
                set();
            }
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            try
            {
                //MessageBox.Show(Config.RuningPath);
                lblError.Text = "";
                string username = Config.ConfigSetting.UserName;
                txtUserName.Text = username;
                if (!string.IsNullOrWhiteSpace(Config.ConfigSetting.Password))
                {
                    string password = Security.Decrypt(Config.ConfigSetting.Password, username);

                    txtPassword.Text = password;
                    ckbRemember.Checked = true;
                    btnLogin.PerformClick();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public new void Show()
        {
            Action show = () =>
            {
                base.Show();
            };
            Invoke(show);
        }

        public new void Hide()
        {
            Action hide = () =>
            {
                base.Hide();
            };
            Invoke(hide);
        }
    }
}
