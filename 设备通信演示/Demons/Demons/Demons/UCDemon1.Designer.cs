namespace Demons
{
    partial class UCDemon1
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCDemon1));
            this.label1 = new System.Windows.Forms.Label();
            this.tlpCOM = new System.Windows.Forms.TableLayoutPanel();
            this.lbPortSend = new System.Windows.Forms.ListBox();
            this.lbPortRead = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btReadPort = new System.Windows.Forms.Button();
            this.btSendPort = new System.Windows.Forms.Button();
            this.cbReadMsgPort = new System.Windows.Forms.ComboBox();
            this.cbSendMsgPort = new System.Windows.Forms.ComboBox();
            this.cbRate = new System.Windows.Forms.ComboBox();
            this.cbComPort = new System.Windows.Forms.ComboBox();
            this.btConPort = new System.Windows.Forms.Button();
            this.btSearchPort = new System.Windows.Forms.Button();
            this.btClosePort = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tlpCOM.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "demonstration1";
            // 
            // tlpCOM
            // 
            this.tlpCOM.ColumnCount = 2;
            this.tlpCOM.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCOM.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCOM.Controls.Add(this.lbPortSend, 0, 1);
            this.tlpCOM.Controls.Add(this.lbPortRead, 1, 1);
            this.tlpCOM.Controls.Add(this.panel1, 0, 0);
            this.tlpCOM.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tlpCOM.Location = new System.Drawing.Point(0, 15);
            this.tlpCOM.Name = "tlpCOM";
            this.tlpCOM.RowCount = 2;
            this.tlpCOM.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tlpCOM.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpCOM.Size = new System.Drawing.Size(777, 434);
            this.tlpCOM.TabIndex = 0;
            // 
            // lbPortSend
            // 
            this.lbPortSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPortSend.FormattingEnabled = true;
            this.lbPortSend.HorizontalScrollbar = true;
            this.lbPortSend.ItemHeight = 12;
            this.lbPortSend.Location = new System.Drawing.Point(3, 83);
            this.lbPortSend.Name = "lbPortSend";
            this.lbPortSend.Size = new System.Drawing.Size(382, 348);
            this.lbPortSend.TabIndex = 0;
            // 
            // lbPortRead
            // 
            this.lbPortRead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPortRead.FormattingEnabled = true;
            this.lbPortRead.HorizontalScrollbar = true;
            this.lbPortRead.ItemHeight = 12;
            this.lbPortRead.Location = new System.Drawing.Point(391, 83);
            this.lbPortRead.Name = "lbPortRead";
            this.lbPortRead.Size = new System.Drawing.Size(383, 348);
            this.lbPortRead.TabIndex = 0;
            // 
            // panel1
            // 
            this.tlpCOM.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.btReadPort);
            this.panel1.Controls.Add(this.btSendPort);
            this.panel1.Controls.Add(this.cbReadMsgPort);
            this.panel1.Controls.Add(this.cbSendMsgPort);
            this.panel1.Controls.Add(this.cbRate);
            this.panel1.Controls.Add(this.cbComPort);
            this.panel1.Controls.Add(this.btConPort);
            this.panel1.Controls.Add(this.btSearchPort);
            this.panel1.Controls.Add(this.btClosePort);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(771, 74);
            this.panel1.TabIndex = 1;
            // 
            // btReadPort
            // 
            this.btReadPort.Location = new System.Drawing.Point(657, 39);
            this.btReadPort.Name = "btReadPort";
            this.btReadPort.Size = new System.Drawing.Size(62, 28);
            this.btReadPort.TabIndex = 27;
            this.btReadPort.Text = "读取";
            this.btReadPort.UseVisualStyleBackColor = true;
            this.btReadPort.Click += new System.EventHandler(this.btReadPort_Click);
            // 
            // btSendPort
            // 
            this.btSendPort.Location = new System.Drawing.Point(657, 6);
            this.btSendPort.Name = "btSendPort";
            this.btSendPort.Size = new System.Drawing.Size(62, 28);
            this.btSendPort.TabIndex = 27;
            this.btSendPort.Text = "发送";
            this.btSendPort.UseVisualStyleBackColor = true;
            this.btSendPort.Click += new System.EventHandler(this.btSendPort_Click);
            // 
            // cbReadMsgPort
            // 
            this.cbReadMsgPort.FormattingEnabled = true;
            this.cbReadMsgPort.Items.AddRange(new object[] {
            "*IDN?",
            "*TST?",
            "*RST",
            "*CLS",
            "*ESE",
            "*ESE?",
            "*ESR?",
            "*OPC",
            "*OPC?",
            "*PSC",
            "*PSC?",
            "*SRE",
            "*SRE?",
            "*STB?",
            "*SAV",
            "*RCL",
            "*TRG"});
            this.cbReadMsgPort.Location = new System.Drawing.Point(224, 41);
            this.cbReadMsgPort.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbReadMsgPort.Name = "cbReadMsgPort";
            this.cbReadMsgPort.Size = new System.Drawing.Size(427, 20);
            this.cbReadMsgPort.TabIndex = 26;
            this.cbReadMsgPort.SelectedIndexChanged += new System.EventHandler(this.cbReadMsgPort_SelectedIndexChanged);
            // 
            // cbSendMsgPort
            // 
            this.cbSendMsgPort.FormattingEnabled = true;
            this.cbSendMsgPort.Location = new System.Drawing.Point(224, 8);
            this.cbSendMsgPort.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbSendMsgPort.Name = "cbSendMsgPort";
            this.cbSendMsgPort.Size = new System.Drawing.Size(427, 20);
            this.cbSendMsgPort.TabIndex = 26;
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
            this.cbRate.Location = new System.Drawing.Point(117, 8);
            this.cbRate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbRate.Name = "cbRate";
            this.cbRate.Size = new System.Drawing.Size(91, 20);
            this.cbRate.TabIndex = 26;
            this.cbRate.Text = "115200";
            // 
            // cbComPort
            // 
            this.cbComPort.FormattingEnabled = true;
            this.cbComPort.Location = new System.Drawing.Point(16, 8);
            this.cbComPort.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbComPort.Name = "cbComPort";
            this.cbComPort.Size = new System.Drawing.Size(95, 20);
            this.cbComPort.TabIndex = 26;
            // 
            // btConPort
            // 
            this.btConPort.Image = ((System.Drawing.Image)(resources.GetObject("btConPort.Image")));
            this.btConPort.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btConPort.Location = new System.Drawing.Point(82, 39);
            this.btConPort.Name = "btConPort";
            this.btConPort.Size = new System.Drawing.Size(60, 28);
            this.btConPort.TabIndex = 13;
            this.btConPort.Text = "连接";
            this.btConPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btConPort.UseVisualStyleBackColor = true;
            this.btConPort.Click += new System.EventHandler(this.btConPort_Click);
            // 
            // btSearchPort
            // 
            this.btSearchPort.Image = ((System.Drawing.Image)(resources.GetObject("btSearchPort.Image")));
            this.btSearchPort.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btSearchPort.Location = new System.Drawing.Point(16, 39);
            this.btSearchPort.Name = "btSearchPort";
            this.btSearchPort.Size = new System.Drawing.Size(60, 28);
            this.btSearchPort.TabIndex = 12;
            this.btSearchPort.Text = "搜索";
            this.btSearchPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btSearchPort.UseVisualStyleBackColor = true;
            this.btSearchPort.Click += new System.EventHandler(this.btSearchPort_Click);
            // 
            // btClosePort
            // 
            this.btClosePort.Image = ((System.Drawing.Image)(resources.GetObject("btClosePort.Image")));
            this.btClosePort.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btClosePort.Location = new System.Drawing.Point(148, 39);
            this.btClosePort.Name = "btClosePort";
            this.btClosePort.Size = new System.Drawing.Size(60, 28);
            this.btClosePort.TabIndex = 11;
            this.btClosePort.Text = "断开";
            this.btClosePort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btClosePort.UseVisualStyleBackColor = true;
            this.btClosePort.Click += new System.EventHandler(this.btClosePort_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(730, 47);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(25, 20);
            this.button1.TabIndex = 28;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // UCDemon1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpCOM);
            this.Controls.Add(this.label1);
            this.Name = "UCDemon1";
            this.Size = new System.Drawing.Size(777, 449);
            this.Load += new System.EventHandler(this.UCDemon1_Load);
            this.tlpCOM.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tlpCOM;
        private System.Windows.Forms.ListBox lbPortSend;
        private System.Windows.Forms.ListBox lbPortRead;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btReadPort;
        private System.Windows.Forms.Button btSendPort;
        private System.Windows.Forms.ComboBox cbComPort;
        private System.Windows.Forms.Button btConPort;
        private System.Windows.Forms.Button btSearchPort;
        private System.Windows.Forms.Button btClosePort;
        private System.Windows.Forms.ComboBox cbReadMsgPort;
        private System.Windows.Forms.ComboBox cbSendMsgPort;
        private System.Windows.Forms.ComboBox cbRate;
        private System.Windows.Forms.Button button1;
    }
}
