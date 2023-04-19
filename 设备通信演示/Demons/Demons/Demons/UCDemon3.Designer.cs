namespace Demons
{
    partial class UCDemon3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCDemon3));
            this.label1 = new System.Windows.Forms.Label();
            this.tlpCOM = new System.Windows.Forms.TableLayoutPanel();
            this.lbSend = new System.Windows.Forms.ListBox();
            this.lbRead = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.btRead = new System.Windows.Forms.Button();
            this.btSend = new System.Windows.Forms.Button();
            this.cbReadMsg = new System.Windows.Forms.ComboBox();
            this.cbSendMsg = new System.Windows.Forms.ComboBox();
            this.btCon = new System.Windows.Forms.Button();
            this.btClose = new System.Windows.Forms.Button();
            this.btSearchPort = new System.Windows.Forms.Button();
            this.tbAddress = new System.Windows.Forms.ComboBox();
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
            this.label1.TabIndex = 4;
            this.label1.Text = "demonstration3";
            // 
            // tlpCOM
            // 
            this.tlpCOM.ColumnCount = 2;
            this.tlpCOM.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCOM.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCOM.Controls.Add(this.lbSend, 0, 1);
            this.tlpCOM.Controls.Add(this.lbRead, 1, 1);
            this.tlpCOM.Controls.Add(this.panel1, 0, 0);
            this.tlpCOM.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tlpCOM.Location = new System.Drawing.Point(0, 15);
            this.tlpCOM.Name = "tlpCOM";
            this.tlpCOM.RowCount = 2;
            this.tlpCOM.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tlpCOM.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpCOM.Size = new System.Drawing.Size(777, 434);
            this.tlpCOM.TabIndex = 5;
            // 
            // lbSend
            // 
            this.lbSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbSend.FormattingEnabled = true;
            this.lbSend.ItemHeight = 12;
            this.lbSend.Location = new System.Drawing.Point(3, 83);
            this.lbSend.Name = "lbSend";
            this.lbSend.Size = new System.Drawing.Size(382, 348);
            this.lbSend.TabIndex = 0;
            // 
            // lbRead
            // 
            this.lbRead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbRead.FormattingEnabled = true;
            this.lbRead.ItemHeight = 12;
            this.lbRead.Location = new System.Drawing.Point(391, 83);
            this.lbRead.Name = "lbRead";
            this.lbRead.Size = new System.Drawing.Size(383, 348);
            this.lbRead.TabIndex = 0;
            // 
            // panel1
            // 
            this.tlpCOM.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.tbAddress);
            this.panel1.Controls.Add(this.btSearchPort);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btRead);
            this.panel1.Controls.Add(this.btSend);
            this.panel1.Controls.Add(this.cbReadMsg);
            this.panel1.Controls.Add(this.cbSendMsg);
            this.panel1.Controls.Add(this.btCon);
            this.panel1.Controls.Add(this.btClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(771, 74);
            this.panel1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 29;
            this.label2.Text = "addr:";
            // 
            // btRead
            // 
            this.btRead.Location = new System.Drawing.Point(657, 39);
            this.btRead.Name = "btRead";
            this.btRead.Size = new System.Drawing.Size(62, 28);
            this.btRead.TabIndex = 27;
            this.btRead.Text = "读取";
            this.btRead.UseVisualStyleBackColor = true;
            this.btRead.Click += new System.EventHandler(this.btRead_Click);
            // 
            // btSend
            // 
            this.btSend.Location = new System.Drawing.Point(657, 6);
            this.btSend.Name = "btSend";
            this.btSend.Size = new System.Drawing.Size(62, 28);
            this.btSend.TabIndex = 27;
            this.btSend.Text = "发送";
            this.btSend.UseVisualStyleBackColor = true;
            this.btSend.Click += new System.EventHandler(this.btSend_Click);
            // 
            // cbReadMsg
            // 
            this.cbReadMsg.FormattingEnabled = true;
            this.cbReadMsg.Items.AddRange(new object[] {
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
            this.cbReadMsg.Location = new System.Drawing.Point(224, 41);
            this.cbReadMsg.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbReadMsg.Name = "cbReadMsg";
            this.cbReadMsg.Size = new System.Drawing.Size(427, 20);
            this.cbReadMsg.TabIndex = 26;
            // 
            // cbSendMsg
            // 
            this.cbSendMsg.FormattingEnabled = true;
            this.cbSendMsg.Location = new System.Drawing.Point(224, 8);
            this.cbSendMsg.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbSendMsg.Name = "cbSendMsg";
            this.cbSendMsg.Size = new System.Drawing.Size(427, 20);
            this.cbSendMsg.TabIndex = 26;
            // 
            // btCon
            // 
            this.btCon.Image = ((System.Drawing.Image)(resources.GetObject("btCon.Image")));
            this.btCon.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btCon.Location = new System.Drawing.Point(80, 39);
            this.btCon.Name = "btCon";
            this.btCon.Size = new System.Drawing.Size(59, 28);
            this.btCon.TabIndex = 13;
            this.btCon.Text = "连接";
            this.btCon.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btCon.UseVisualStyleBackColor = true;
            this.btCon.Click += new System.EventHandler(this.btCon_Click);
            // 
            // btClose
            // 
            this.btClose.Image = ((System.Drawing.Image)(resources.GetObject("btClose.Image")));
            this.btClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btClose.Location = new System.Drawing.Point(151, 39);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(59, 28);
            this.btClose.TabIndex = 11;
            this.btClose.Text = "断开";
            this.btClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // btSearchPort
            // 
            this.btSearchPort.Image = ((System.Drawing.Image)(resources.GetObject("btSearchPort.Image")));
            this.btSearchPort.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btSearchPort.Location = new System.Drawing.Point(8, 39);
            this.btSearchPort.Name = "btSearchPort";
            this.btSearchPort.Size = new System.Drawing.Size(60, 28);
            this.btSearchPort.TabIndex = 30;
            this.btSearchPort.Text = "搜索";
            this.btSearchPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btSearchPort.UseVisualStyleBackColor = true;
            this.btSearchPort.Click += new System.EventHandler(this.btSearchPort_Click);
            // 
            // tbAddress
            // 
            this.tbAddress.FormattingEnabled = true;
            this.tbAddress.Location = new System.Drawing.Point(47, 13);
            this.tbAddress.Name = "tbAddress";
            this.tbAddress.Size = new System.Drawing.Size(92, 20);
            this.tbAddress.TabIndex = 31;
            // 
            // UCDemon3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpCOM);
            this.Controls.Add(this.label1);
            this.Name = "UCDemon3";
            this.Size = new System.Drawing.Size(777, 449);
            this.Load += new System.EventHandler(this.UCDemon3_Load);
            this.tlpCOM.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tlpCOM;
        private System.Windows.Forms.ListBox lbSend;
        private System.Windows.Forms.ListBox lbRead;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btRead;
        private System.Windows.Forms.Button btSend;
        private System.Windows.Forms.ComboBox cbReadMsg;
        private System.Windows.Forms.ComboBox cbSendMsg;
        private System.Windows.Forms.Button btCon;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.Button btSearchPort;
        private System.Windows.Forms.ComboBox tbAddress;
    }
}
