using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demon.用户控件
{
    public partial class ucPortInfoShow : UserControl
    {
        public PortSerach portSerach;
        public PortConnect portConnect;
        public PortClose portClose;

        public ucPortInfoShow()
        {
            InitializeComponent();
        }

        private void ucPortInfoShow_Load(object sender, EventArgs e)
        {
            ConvertConnectSta(false);
        }

        private void btSearchPort_Click(object sender, EventArgs e)
        {
            string[] ports = portSerach();
            this.cbComPort.Items.Clear();
            this.cbComPort.Items.AddRange(ports);
            if (this.cbComPort.Items.Count > 0) this.cbComPort.SelectedIndex = 0;
        }

        private void btConPort_Click(object sender, EventArgs e)
        {
            try
            {
                string port = cbComPort.Text;
                int baurd = Convert.ToInt32(cbRate.Text);
                string msg = portConnect(port,baurd);
                if (msg != "") throw new Exception(msg);
                ConvertConnectSta(true);
            }
            catch (Exception ex)
            {
                ConvertConnectSta(false);
                MessageBox.Show(ex.Message);
            }
        }

        private void btClosePort_Click(object sender, EventArgs e)
        {
            try
            {
                portClose();
                ConvertConnectSta(false);
            }
            catch (Exception ex)
            {
                ConvertConnectSta(false);
                MessageBox.Show(ex.Message);
            }
        }

        void ConvertConnectSta(bool sta)
        {
            if (sta == true)
            {
                cbComPort.Enabled = false;
                cbRate.Enabled = false;
                btSearchPort.Enabled = false;
                btConPort.Enabled = false;
                btClosePort.Enabled = true;
            }
            else
            {
                cbComPort.Enabled = true;
                cbRate.Enabled = true;
                btSearchPort.Enabled = true;
                btConPort.Enabled = true;
                btClosePort.Enabled = false;
            }
        }
    }
}
