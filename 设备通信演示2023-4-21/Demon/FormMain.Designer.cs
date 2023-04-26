
namespace Demon
{
    partial class FormMain
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
            this.tcMain = new Sunny.UI.UITabControlMenu();
            this.tpPort = new System.Windows.Forms.TabPage();
            this.tpTCP = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbPortSend = new System.Windows.Forms.ListBox();
            this.lbPortRead = new System.Windows.Forms.ListBox();
            this.cbPortCmd = new System.Windows.Forms.ComboBox();
            this.btPortSend = new System.Windows.Forms.Button();
            this.btPortRead = new System.Windows.Forms.Button();
            this.cbString = new System.Windows.Forms.CheckBox();
            this.cbAddLine = new System.Windows.Forms.CheckBox();
            this.rbPortReadLine = new System.Windows.Forms.RadioButton();
            this.rbPortReadTo = new System.Windows.Forms.RadioButton();
            this.rbPortReadDelay = new System.Windows.Forms.RadioButton();
            this.cbPortReadTo = new System.Windows.Forms.ComboBox();
            this.cbCRC = new System.Windows.Forms.CheckBox();
            this.portS1 = new Demon.用户控件.ucPortInfoShow();
            this.tcMain.SuspendLayout();
            this.tpPort.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcMain
            // 
            this.tcMain.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tcMain.Controls.Add(this.tpPort);
            this.tcMain.Controls.Add(this.tpTCP);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tcMain.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tcMain.ItemSize = new System.Drawing.Size(40, 120);
            this.tcMain.Location = new System.Drawing.Point(0, 0);
            this.tcMain.Multiline = true;
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(858, 525);
            this.tcMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tcMain.TabIndex = 0;
            this.tcMain.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // tpPort
            // 
            this.tpPort.Controls.Add(this.btPortRead);
            this.tpPort.Controls.Add(this.btPortSend);
            this.tpPort.Controls.Add(this.cbPortCmd);
            this.tpPort.Controls.Add(this.lbPortRead);
            this.tpPort.Controls.Add(this.lbPortSend);
            this.tpPort.Controls.Add(this.groupBox1);
            this.tpPort.Controls.Add(this.portS1);
            this.tpPort.Location = new System.Drawing.Point(121, 0);
            this.tpPort.Name = "tpPort";
            this.tpPort.Size = new System.Drawing.Size(737, 525);
            this.tpPort.TabIndex = 0;
            this.tpPort.Text = "串口";
            this.tpPort.UseVisualStyleBackColor = true;
            // 
            // tpTCP
            // 
            this.tpTCP.Location = new System.Drawing.Point(121, 0);
            this.tpTCP.Name = "tpTCP";
            this.tpTCP.Size = new System.Drawing.Size(737, 525);
            this.tpTCP.TabIndex = 1;
            this.tpTCP.Text = "TCPIP";
            this.tpTCP.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbPortReadDelay);
            this.groupBox1.Controls.Add(this.rbPortReadTo);
            this.groupBox1.Controls.Add(this.cbPortReadTo);
            this.groupBox1.Controls.Add(this.rbPortReadLine);
            this.groupBox1.Controls.Add(this.cbCRC);
            this.groupBox1.Controls.Add(this.cbAddLine);
            this.groupBox1.Controls.Add(this.cbString);
            this.groupBox1.Location = new System.Drawing.Point(249, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(476, 106);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "串口设置";
            // 
            // lbPortSend
            // 
            this.lbPortSend.FormattingEnabled = true;
            this.lbPortSend.HorizontalScrollbar = true;
            this.lbPortSend.ItemHeight = 21;
            this.lbPortSend.Location = new System.Drawing.Point(14, 172);
            this.lbPortSend.Name = "lbPortSend";
            this.lbPortSend.Size = new System.Drawing.Size(338, 340);
            this.lbPortSend.TabIndex = 2;
            // 
            // lbPortRead
            // 
            this.lbPortRead.FormattingEnabled = true;
            this.lbPortRead.HorizontalScrollbar = true;
            this.lbPortRead.ItemHeight = 21;
            this.lbPortRead.Location = new System.Drawing.Point(371, 172);
            this.lbPortRead.Name = "lbPortRead";
            this.lbPortRead.Size = new System.Drawing.Size(354, 340);
            this.lbPortRead.TabIndex = 2;
            // 
            // cbPortCmd
            // 
            this.cbPortCmd.FormattingEnabled = true;
            this.cbPortCmd.Items.AddRange(new object[] {
            "*IDN?"});
            this.cbPortCmd.Location = new System.Drawing.Point(14, 125);
            this.cbPortCmd.Name = "cbPortCmd";
            this.cbPortCmd.Size = new System.Drawing.Size(305, 29);
            this.cbPortCmd.TabIndex = 3;
            // 
            // btPortSend
            // 
            this.btPortSend.Location = new System.Drawing.Point(338, 125);
            this.btPortSend.Name = "btPortSend";
            this.btPortSend.Size = new System.Drawing.Size(70, 29);
            this.btPortSend.TabIndex = 4;
            this.btPortSend.Text = "发送";
            this.btPortSend.UseVisualStyleBackColor = true;
            this.btPortSend.Click += new System.EventHandler(this.btPortSend_Click);
            // 
            // btPortRead
            // 
            this.btPortRead.Location = new System.Drawing.Point(434, 125);
            this.btPortRead.Name = "btPortRead";
            this.btPortRead.Size = new System.Drawing.Size(70, 29);
            this.btPortRead.TabIndex = 4;
            this.btPortRead.Text = "读取";
            this.btPortRead.UseVisualStyleBackColor = true;
            this.btPortRead.Click += new System.EventHandler(this.btPortRead_Click);
            // 
            // cbString
            // 
            this.cbString.AutoSize = true;
            this.cbString.Checked = true;
            this.cbString.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbString.Location = new System.Drawing.Point(17, 28);
            this.cbString.Name = "cbString";
            this.cbString.Size = new System.Drawing.Size(127, 25);
            this.cbString.TabIndex = 0;
            this.cbString.Text = "字符串√\\数组";
            this.cbString.UseVisualStyleBackColor = true;
            // 
            // cbAddLine
            // 
            this.cbAddLine.AutoSize = true;
            this.cbAddLine.Location = new System.Drawing.Point(17, 67);
            this.cbAddLine.Name = "cbAddLine";
            this.cbAddLine.Size = new System.Drawing.Size(109, 25);
            this.cbAddLine.TabIndex = 0;
            this.cbAddLine.Text = "添加换行符";
            this.cbAddLine.UseVisualStyleBackColor = true;
            // 
            // rbPortReadLine
            // 
            this.rbPortReadLine.AutoSize = true;
            this.rbPortReadLine.Checked = true;
            this.rbPortReadLine.Location = new System.Drawing.Point(258, 16);
            this.rbPortReadLine.Name = "rbPortReadLine";
            this.rbPortReadLine.Size = new System.Drawing.Size(92, 25);
            this.rbPortReadLine.TabIndex = 1;
            this.rbPortReadLine.TabStop = true;
            this.rbPortReadLine.Text = "读取到行";
            this.rbPortReadLine.UseVisualStyleBackColor = true;
            // 
            // rbPortReadTo
            // 
            this.rbPortReadTo.AutoSize = true;
            this.rbPortReadTo.Location = new System.Drawing.Point(258, 45);
            this.rbPortReadTo.Name = "rbPortReadTo";
            this.rbPortReadTo.Size = new System.Drawing.Size(92, 25);
            this.rbPortReadTo.TabIndex = 1;
            this.rbPortReadTo.Text = "读取指定";
            this.rbPortReadTo.UseVisualStyleBackColor = true;
            // 
            // rbPortReadDelay
            // 
            this.rbPortReadDelay.AutoSize = true;
            this.rbPortReadDelay.Location = new System.Drawing.Point(258, 74);
            this.rbPortReadDelay.Name = "rbPortReadDelay";
            this.rbPortReadDelay.Size = new System.Drawing.Size(92, 25);
            this.rbPortReadDelay.TabIndex = 1;
            this.rbPortReadDelay.Text = "延时读取";
            this.rbPortReadDelay.UseVisualStyleBackColor = true;
            this.rbPortReadDelay.CheckedChanged += new System.EventHandler(this.rbPortReadDelay_CheckedChanged);
            // 
            // cbPortReadTo
            // 
            this.cbPortReadTo.FormattingEnabled = true;
            this.cbPortReadTo.Items.AddRange(new object[] {
            "*IDN?"});
            this.cbPortReadTo.Location = new System.Drawing.Point(382, 41);
            this.cbPortReadTo.Name = "cbPortReadTo";
            this.cbPortReadTo.Size = new System.Drawing.Size(86, 29);
            this.cbPortReadTo.TabIndex = 3;
            // 
            // cbCRC
            // 
            this.cbCRC.AutoSize = true;
            this.cbCRC.Location = new System.Drawing.Point(143, 28);
            this.cbCRC.Name = "cbCRC";
            this.cbCRC.Size = new System.Drawing.Size(61, 25);
            this.cbCRC.TabIndex = 0;
            this.cbCRC.Text = "CRC";
            this.cbCRC.UseVisualStyleBackColor = true;
            // 
            // portS1
            // 
            this.portS1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.portS1.Location = new System.Drawing.Point(14, 12);
            this.portS1.MaximumSize = new System.Drawing.Size(223, 93);
            this.portS1.MinimumSize = new System.Drawing.Size(223, 93);
            this.portS1.Name = "portS1";
            this.portS1.Size = new System.Drawing.Size(223, 93);
            this.portS1.TabIndex = 0;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(858, 525);
            this.Controls.Add(this.tcMain);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Demon";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.tcMain.ResumeLayout(false);
            this.tpPort.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UITabControlMenu tcMain;
        private System.Windows.Forms.TabPage tpPort;
        private System.Windows.Forms.TabPage tpTCP;
        private 用户控件.ucPortInfoShow portS1;
        private System.Windows.Forms.Button btPortRead;
        private System.Windows.Forms.Button btPortSend;
        private System.Windows.Forms.ComboBox cbPortCmd;
        private System.Windows.Forms.ListBox lbPortRead;
        private System.Windows.Forms.ListBox lbPortSend;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbAddLine;
        private System.Windows.Forms.CheckBox cbString;
        private System.Windows.Forms.RadioButton rbPortReadDelay;
        private System.Windows.Forms.RadioButton rbPortReadTo;
        private System.Windows.Forms.ComboBox cbPortReadTo;
        private System.Windows.Forms.RadioButton rbPortReadLine;
        private System.Windows.Forms.CheckBox cbCRC;
    }
}

