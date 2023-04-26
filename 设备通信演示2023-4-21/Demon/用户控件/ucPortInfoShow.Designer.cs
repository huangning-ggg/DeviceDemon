
namespace Demon.用户控件
{
    partial class ucPortInfoShow
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucPortInfoShow));
            this.cbRate = new System.Windows.Forms.ComboBox();
            this.cbComPort = new System.Windows.Forms.ComboBox();
            this.btConPort = new System.Windows.Forms.Button();
            this.btSearchPort = new System.Windows.Forms.Button();
            this.btClosePort = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbRate
            // 
            this.cbRate.FormattingEnabled = true;
            this.cbRate.Items.AddRange(new object[] {
            "256000",
            "128000",
            "115200",
            "57600",
            "56000",
            "43000",
            "38400",
            "28800",
            "19200",
            "9600",
            "4800",
            "2400",
            "1200",
            "600",
            "300",
            "110"});
            this.cbRate.Location = new System.Drawing.Point(120, 15);
            this.cbRate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbRate.Name = "cbRate";
            this.cbRate.Size = new System.Drawing.Size(91, 20);
            this.cbRate.TabIndex = 30;
            this.cbRate.Text = "115200";
            // 
            // cbComPort
            // 
            this.cbComPort.FormattingEnabled = true;
            this.cbComPort.Location = new System.Drawing.Point(16, 15);
            this.cbComPort.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbComPort.Name = "cbComPort";
            this.cbComPort.Size = new System.Drawing.Size(95, 20);
            this.cbComPort.TabIndex = 31;
            // 
            // btConPort
            // 
            this.btConPort.Image = ((System.Drawing.Image)(resources.GetObject("btConPort.Image")));
            this.btConPort.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btConPort.Location = new System.Drawing.Point(82, 50);
            this.btConPort.Name = "btConPort";
            this.btConPort.Size = new System.Drawing.Size(60, 28);
            this.btConPort.TabIndex = 29;
            this.btConPort.Text = "连接";
            this.btConPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btConPort.UseVisualStyleBackColor = true;
            this.btConPort.Click += new System.EventHandler(this.btConPort_Click);
            // 
            // btSearchPort
            // 
            this.btSearchPort.Image = ((System.Drawing.Image)(resources.GetObject("btSearchPort.Image")));
            this.btSearchPort.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btSearchPort.Location = new System.Drawing.Point(16, 50);
            this.btSearchPort.Name = "btSearchPort";
            this.btSearchPort.Size = new System.Drawing.Size(60, 28);
            this.btSearchPort.TabIndex = 28;
            this.btSearchPort.Text = "搜索";
            this.btSearchPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btSearchPort.UseVisualStyleBackColor = true;
            this.btSearchPort.Click += new System.EventHandler(this.btSearchPort_Click);
            // 
            // btClosePort
            // 
            this.btClosePort.Image = ((System.Drawing.Image)(resources.GetObject("btClosePort.Image")));
            this.btClosePort.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btClosePort.Location = new System.Drawing.Point(151, 50);
            this.btClosePort.Name = "btClosePort";
            this.btClosePort.Size = new System.Drawing.Size(60, 28);
            this.btClosePort.TabIndex = 27;
            this.btClosePort.Text = "断开";
            this.btClosePort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btClosePort.UseVisualStyleBackColor = true;
            this.btClosePort.Click += new System.EventHandler(this.btClosePort_Click);
            // 
            // ucPortInfoShow
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.cbRate);
            this.Controls.Add(this.cbComPort);
            this.Controls.Add(this.btConPort);
            this.Controls.Add(this.btSearchPort);
            this.Controls.Add(this.btClosePort);
            this.MaximumSize = new System.Drawing.Size(223, 93);
            this.MinimumSize = new System.Drawing.Size(223, 93);
            this.Name = "ucPortInfoShow";
            this.Size = new System.Drawing.Size(221, 91);
            this.Load += new System.EventHandler(this.ucPortInfoShow_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbRate;
        private System.Windows.Forms.ComboBox cbComPort;
        private System.Windows.Forms.Button btConPort;
        private System.Windows.Forms.Button btSearchPort;
        private System.Windows.Forms.Button btClosePort;
    }
}
