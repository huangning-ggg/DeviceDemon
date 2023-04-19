using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DeviceConnect;

namespace Demons
{
    public partial class UCDemon3 : UserControl
    {
        public UCDemon3()
        {
            InitializeComponent();
        }

        private void UCDemon3_Load(object sender, EventArgs e)
        {

        }

        public void InitCtl()
        {
            TCPInit();
        }

        public ConnectCtl GPIB;
        GPIBDriverTemp myGPIB = new GPIBDriverTemp();
        public void TCPInit()
        {
            GPIB = new ConnectCtl { connect = this.btCon, close = this.btClose, send = this.btSend, read = this.btRead };
            ConvertSta(false);
        }


        private void btSearchPort_Click(object sender, EventArgs e)
        {
            tbAddress.Items.Clear();
            string[] gpibs = PortUltility.FindAddresses(PortType.GPIB);
            tbAddress.Items.AddRange(gpibs);
        }      

        private void btCon_Click(object sender, EventArgs e)
        {
            string addr = tbAddress.Text;
            bool rtl;

            rtl = myGPIB.Connect(addr);
            if (rtl == true)
            {
                ConvertSta(true);
                SendShow(string.Format("Address：{0} 连接成功", addr));
            }
            else
            {
                ConvertSta(false);
                SendShow(string.Format("Address：{0} 连接失败！！！", addr));
                SendShow(string.Format("{0}", myGPIB.errorMsg));
                MessageBox.Show(myGPIB.errorMsg);
            }
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            bool rtl;
            rtl = myGPIB.Disconnect();
            if (rtl == false)
            {
                SendShow(string.Format("断开失败！！！"));
                MessageBox.Show(myGPIB.errorMsg);
            }
            else SendShow(string.Format("断开成功"));
            ConvertSta(false);
        }

        private void btSend_Click(object sender, EventArgs e)
        {
            bool rtl;
            string msg = cbSendMsg.Text;
            SendShow(msg);
            rtl = myGPIB.Instruction_Send(msg);
            if (rtl == false) SendShow(myGPIB.errorMsg);
            else SendShow("OK");
        }

        private void btRead_Click(object sender, EventArgs e)
        {
            bool rtl;
            string msg = cbReadMsg.Text;
            ReadShow(msg);
            string read = "";
            rtl = myGPIB.Instruction_Read(msg, ref read);
            if (rtl == false) SendShow(myGPIB.errorMsg);
            else SendShow(read);
        }

        public void SendShow(string str)
        {
            LBShow(lbSend, str);
        }

        public void ReadShow(string str)
        {
            LBShow(lbRead, str);
        }
        public void ConvertSta(bool sta)
        {
            CTLFunc.ConvertConSta(GPIB, sta);
        }

        public void LBShow(ListBox lb, string msg)
        {
            if (lb.InvokeRequired == true)
            {
                lb.Invoke((Action)delegate
                {
                    lb.Items.Add(string.Format("{0}:{1}", DateTime.Now, msg));
                    lb.TopIndex = lb.Items.Count - 1;
                });
            }
            else
            {
                lb.Items.Add(string.Format("{0}:{1}", DateTime.Now, msg));
                lb.TopIndex = lb.Items.Count - 1;
            }
        }
  
    }
}
