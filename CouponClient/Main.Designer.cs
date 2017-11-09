namespace CouponClient
{
    partial class Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.tab = new System.Windows.Forms.TabControl();
            this.tabTaoBao = new System.Windows.Forms.TabPage();
            this.colTaobao = new CouponClient.ColTaoBao();
            this.tabMoGuJie = new System.Windows.Forms.TabPage();
            this.colMoGuJie = new CouponClient.ColMoGuJie();
            this.tabJd = new System.Windows.Forms.TabPage();
            this.tabLog = new System.Windows.Forms.TabPage();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuAccount = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLoginOut = new System.Windows.Forms.ToolStripMenuItem();
            this.menuClose = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelpPage = new System.Windows.Forms.ToolStripMenuItem();
            this.stauts = new System.Windows.Forms.StatusStrip();
            this.lblSpeed = new System.Windows.Forms.ToolStripStatusLabel();
            this.colJd = new CouponClient.ColJd();
            this.tab.SuspendLayout();
            this.tabTaoBao.SuspendLayout();
            this.tabMoGuJie.SuspendLayout();
            this.tabJd.SuspendLayout();
            this.tabLog.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.stauts.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab
            // 
            this.tab.Controls.Add(this.tabTaoBao);
            this.tab.Controls.Add(this.tabMoGuJie);
            this.tab.Controls.Add(this.tabJd);
            this.tab.Controls.Add(this.tabLog);
            this.tab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab.Location = new System.Drawing.Point(0, 25);
            this.tab.Name = "tab";
            this.tab.SelectedIndex = 0;
            this.tab.Size = new System.Drawing.Size(777, 436);
            this.tab.TabIndex = 1;
            // 
            // tabTaoBao
            // 
            this.tabTaoBao.Controls.Add(this.colTaobao);
            this.tabTaoBao.Location = new System.Drawing.Point(4, 22);
            this.tabTaoBao.Name = "tabTaoBao";
            this.tabTaoBao.Padding = new System.Windows.Forms.Padding(3);
            this.tabTaoBao.Size = new System.Drawing.Size(769, 410);
            this.tabTaoBao.TabIndex = 0;
            this.tabTaoBao.Text = "淘宝联盟";
            this.tabTaoBao.UseVisualStyleBackColor = true;
            // 
            // colTaobao
            // 
            this.colTaobao.Dock = System.Windows.Forms.DockStyle.Fill;
            this.colTaobao.EnableRun = false;
            this.colTaobao.Location = new System.Drawing.Point(3, 3);
            this.colTaobao.Name = "colTaobao";
            this.colTaobao.Size = new System.Drawing.Size(763, 404);
            this.colTaobao.TabIndex = 0;
            this.colTaobao.UserInfo = null;
            // 
            // tabMoGuJie
            // 
            this.tabMoGuJie.Controls.Add(this.colMoGuJie);
            this.tabMoGuJie.Location = new System.Drawing.Point(4, 22);
            this.tabMoGuJie.Name = "tabMoGuJie";
            this.tabMoGuJie.Padding = new System.Windows.Forms.Padding(3);
            this.tabMoGuJie.Size = new System.Drawing.Size(769, 410);
            this.tabMoGuJie.TabIndex = 2;
            this.tabMoGuJie.Text = "蘑菇街";
            this.tabMoGuJie.UseVisualStyleBackColor = true;
            // 
            // colMoGuJie
            // 
            this.colMoGuJie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.colMoGuJie.EnableRun = false;
            this.colMoGuJie.Location = new System.Drawing.Point(3, 3);
            this.colMoGuJie.Name = "colMoGuJie";
            this.colMoGuJie.Size = new System.Drawing.Size(763, 404);
            this.colMoGuJie.TabIndex = 0;
            this.colMoGuJie.UserInfo = null;
            // 
            // tabJd
            // 
            this.tabJd.Controls.Add(this.colJd);
            this.tabJd.Location = new System.Drawing.Point(4, 22);
            this.tabJd.Name = "tabJd";
            this.tabJd.Padding = new System.Windows.Forms.Padding(3);
            this.tabJd.Size = new System.Drawing.Size(769, 410);
            this.tabJd.TabIndex = 3;
            this.tabJd.Text = "京东";
            this.tabJd.UseVisualStyleBackColor = true;
            // 
            // tabLog
            // 
            this.tabLog.Controls.Add(this.txtLog);
            this.tabLog.Location = new System.Drawing.Point(4, 22);
            this.tabLog.Name = "tabLog";
            this.tabLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabLog.Size = new System.Drawing.Size(769, 410);
            this.tabLog.TabIndex = 1;
            this.tabLog.Text = "日志";
            this.tabLog.UseVisualStyleBackColor = true;
            // 
            // txtLog
            // 
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Location = new System.Drawing.Point(3, 3);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(763, 404);
            this.txtLog.TabIndex = 0;
            // 
            // timer
            // 
            this.timer.Interval = 10000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuAccount,
            this.menuHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(777, 25);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuAccount
            // 
            this.menuAccount.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuLoginOut,
            this.menuClose});
            this.menuAccount.Name = "menuAccount";
            this.menuAccount.Size = new System.Drawing.Size(44, 21);
            this.menuAccount.Text = "帐号";
            // 
            // menuLoginOut
            // 
            this.menuLoginOut.Name = "menuLoginOut";
            this.menuLoginOut.Size = new System.Drawing.Size(100, 22);
            this.menuLoginOut.Text = "注销";
            this.menuLoginOut.Click += new System.EventHandler(this.menuLoginOut_Click);
            // 
            // menuClose
            // 
            this.menuClose.Name = "menuClose";
            this.menuClose.Size = new System.Drawing.Size(100, 22);
            this.menuClose.Text = "退出";
            this.menuClose.Click += new System.EventHandler(this.menuClose_Click);
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuHelpPage});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(44, 21);
            this.menuHelp.Text = "帮助";
            // 
            // menuHelpPage
            // 
            this.menuHelpPage.Name = "menuHelpPage";
            this.menuHelpPage.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.menuHelpPage.Size = new System.Drawing.Size(145, 22);
            this.menuHelpPage.Text = "帮助文档";
            this.menuHelpPage.Click += new System.EventHandler(this.menuHelpPage_Click);
            // 
            // stauts
            // 
            this.stauts.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblSpeed});
            this.stauts.Location = new System.Drawing.Point(0, 461);
            this.stauts.Name = "stauts";
            this.stauts.Size = new System.Drawing.Size(777, 22);
            this.stauts.TabIndex = 3;
            this.stauts.Text = "statusStrip1";
            // 
            // lblSpeed
            // 
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(32, 17);
            this.lblSpeed.Text = "测速";
            // 
            // colJd1
            // 
            this.colJd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.colJd.EnableRun = false;
            this.colJd.Location = new System.Drawing.Point(3, 3);
            this.colJd.Name = "colJd1";
            this.colJd.Size = new System.Drawing.Size(763, 404);
            this.colJd.TabIndex = 0;
            this.colJd.UserInfo = null;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 483);
            this.Controls.Add(this.tab);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.stauts);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "客户端";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Main_Load);
            this.tab.ResumeLayout(false);
            this.tabTaoBao.ResumeLayout(false);
            this.tabMoGuJie.ResumeLayout(false);
            this.tabJd.ResumeLayout(false);
            this.tabLog.ResumeLayout(false);
            this.tabLog.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.stauts.ResumeLayout(false);
            this.stauts.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TabControl tab;
        private System.Windows.Forms.TabPage tabLog;
        private System.Windows.Forms.TabPage tabTaoBao;
        private ColTaoBao colTaobao;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuAccount;
        private System.Windows.Forms.ToolStripMenuItem menuLoginOut;
        private System.Windows.Forms.ToolStripMenuItem menuClose;
        private System.Windows.Forms.TabPage tabMoGuJie;
        private ColMoGuJie colMoGuJie;
        private System.Windows.Forms.StatusStrip stauts;
        private System.Windows.Forms.ToolStripStatusLabel lblSpeed;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuHelpPage;
        private System.Windows.Forms.TabPage tabJd;
        private ColJd colJd;
    }
}

