namespace Demons
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelItems = new System.Windows.Forms.Panel();
            this.panelBar = new System.Windows.Forms.Panel();
            this.btDemon5 = new System.Windows.Forms.Button();
            this.btDemon4 = new System.Windows.Forms.Button();
            this.btDemon3 = new System.Windows.Forms.Button();
            this.btDemon2 = new System.Windows.Forms.Button();
            this.btDemon1 = new System.Windows.Forms.Button();
            this.panelMenu = new System.Windows.Forms.Panel();
            this.tlpMain.SuspendLayout();
            this.panelItems.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 151F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.panelMain, 1, 1);
            this.tlpMain.Controls.Add(this.panelItems, 0, 1);
            this.tlpMain.Controls.Add(this.panelMenu, 0, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 2;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5969F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 87.4031F));
            this.tlpMain.Size = new System.Drawing.Size(954, 516);
            this.tlpMain.TabIndex = 0;
            // 
            // panelMain
            // 
            this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(152, 66);
            this.panelMain.Margin = new System.Windows.Forms.Padding(1);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(801, 449);
            this.panelMain.TabIndex = 0;
            // 
            // panelItems
            // 
            this.panelItems.AllowDrop = true;
            this.panelItems.AutoScroll = true;
            this.panelItems.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelItems.Controls.Add(this.panelBar);
            this.panelItems.Controls.Add(this.btDemon5);
            this.panelItems.Controls.Add(this.btDemon4);
            this.panelItems.Controls.Add(this.btDemon3);
            this.panelItems.Controls.Add(this.btDemon2);
            this.panelItems.Controls.Add(this.btDemon1);
            this.panelItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelItems.Location = new System.Drawing.Point(1, 66);
            this.panelItems.Margin = new System.Windows.Forms.Padding(1);
            this.panelItems.Name = "panelItems";
            this.panelItems.Size = new System.Drawing.Size(149, 449);
            this.panelItems.TabIndex = 0;
            // 
            // panelBar
            // 
            this.panelBar.BackColor = System.Drawing.Color.Red;
            this.panelBar.Location = new System.Drawing.Point(1, 3);
            this.panelBar.Name = "panelBar";
            this.panelBar.Size = new System.Drawing.Size(10, 45);
            this.panelBar.TabIndex = 0;
            // 
            // btDemon5
            // 
            this.btDemon5.BackColor = System.Drawing.Color.Green;
            this.btDemon5.FlatAppearance.BorderSize = 0;
            this.btDemon5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btDemon5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btDemon5.Location = new System.Drawing.Point(11, 207);
            this.btDemon5.Name = "btDemon5";
            this.btDemon5.Size = new System.Drawing.Size(114, 45);
            this.btDemon5.TabIndex = 0;
            this.btDemon5.Text = "演示5";
            this.btDemon5.UseVisualStyleBackColor = false;
            this.btDemon5.Click += new System.EventHandler(this.btDemon5_Click);
            // 
            // btDemon4
            // 
            this.btDemon4.BackColor = System.Drawing.Color.Green;
            this.btDemon4.FlatAppearance.BorderSize = 0;
            this.btDemon4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btDemon4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btDemon4.Location = new System.Drawing.Point(11, 156);
            this.btDemon4.Name = "btDemon4";
            this.btDemon4.Size = new System.Drawing.Size(114, 45);
            this.btDemon4.TabIndex = 0;
            this.btDemon4.Text = "演示4";
            this.btDemon4.UseVisualStyleBackColor = false;
            this.btDemon4.Click += new System.EventHandler(this.btDemon4_Click);
            // 
            // btDemon3
            // 
            this.btDemon3.BackColor = System.Drawing.Color.Green;
            this.btDemon3.FlatAppearance.BorderSize = 0;
            this.btDemon3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btDemon3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btDemon3.Location = new System.Drawing.Point(11, 105);
            this.btDemon3.Name = "btDemon3";
            this.btDemon3.Size = new System.Drawing.Size(114, 45);
            this.btDemon3.TabIndex = 0;
            this.btDemon3.Text = "GPIB演示";
            this.btDemon3.UseVisualStyleBackColor = false;
            this.btDemon3.Click += new System.EventHandler(this.btDemon3_Click);
            // 
            // btDemon2
            // 
            this.btDemon2.BackColor = System.Drawing.Color.Green;
            this.btDemon2.FlatAppearance.BorderSize = 0;
            this.btDemon2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btDemon2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btDemon2.Location = new System.Drawing.Point(11, 54);
            this.btDemon2.Name = "btDemon2";
            this.btDemon2.Size = new System.Drawing.Size(114, 45);
            this.btDemon2.TabIndex = 0;
            this.btDemon2.Text = "TCP\\IP演示";
            this.btDemon2.UseVisualStyleBackColor = false;
            this.btDemon2.Click += new System.EventHandler(this.btDemon2_Click);
            // 
            // btDemon1
            // 
            this.btDemon1.BackColor = System.Drawing.Color.Green;
            this.btDemon1.FlatAppearance.BorderSize = 0;
            this.btDemon1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btDemon1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btDemon1.Location = new System.Drawing.Point(11, 3);
            this.btDemon1.Name = "btDemon1";
            this.btDemon1.Size = new System.Drawing.Size(114, 45);
            this.btDemon1.TabIndex = 0;
            this.btDemon1.Text = "串口演示";
            this.btDemon1.UseVisualStyleBackColor = false;
            this.btDemon1.Click += new System.EventHandler(this.btDemon1_Click);
            // 
            // panelMenu
            // 
            this.panelMenu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tlpMain.SetColumnSpan(this.panelMenu, 2);
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMenu.Location = new System.Drawing.Point(1, 1);
            this.panelMenu.Margin = new System.Windows.Forms.Padding(1);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(952, 63);
            this.panelMenu.TabIndex = 0;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(954, 516);
            this.Controls.Add(this.tlpMain);
            this.Name = "FormMain";
            this.Text = "演示";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.tlpMain.ResumeLayout(false);
            this.panelItems.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelItems;
        private System.Windows.Forms.Button btDemon1;
        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.Panel panelBar;
        private System.Windows.Forms.Button btDemon5;
        private System.Windows.Forms.Button btDemon4;
        private System.Windows.Forms.Button btDemon3;
        private System.Windows.Forms.Button btDemon2;

    }
}

