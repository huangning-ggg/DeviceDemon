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
    public partial class UCDemon2 : UserControl
    {
        public UCDemon2()
        {
            InitializeComponent();
        }

        private void UCDemon2_Load(object sender, EventArgs e)
        {
            InitCtl();
        }

        public void InitCtl()
        {
            TCPInit();
        }

        public ConnectCtl TCP;
        TCPDriverTemp myTCP = new TCPDriverTemp();
        public void TCPInit()
        {
            TCP = new ConnectCtl { connect = this.btCon, close = this.btClose, send = this.btSend, read = this.btRead };
            ConvertSta(false);
        }

        private void btCon_Click(object sender, EventArgs e)
        {
            string ip = tbIP.Text;
            string port = tbPort.Text;
            bool rtl;

            rtl = myTCP.Connect(ip, port);
            if (rtl == true)
            {
                ConvertSta(true);
                SendShow(string.Format("IP：{0} PORT：{1} 连接成功", ip, port));
            }
            else
            {
                ConvertSta(false);
                SendShow(string.Format("IP：{0} PORT：{1} 连接失败！！！", ip, port));
                SendShow(string.Format("{0}", myTCP.errorMsg));
                MessageBox.Show(myTCP.errorMsg);
            }
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            bool rtl;
            rtl = myTCP.Disconnect();
            if (rtl == false)
            {
                SendShow(string.Format("断开失败！！！"));
                MessageBox.Show(myTCP.errorMsg);
            }
            else SendShow(string.Format("断开成功"));
            ConvertSta(false);
        }

        private void btSend_Click(object sender, EventArgs e)
        {
            bool rtl;
            string msg = cbSendMsg.Text;
            SendShow(msg);
            rtl = myTCP.Instruction_Send(msg);
            if (rtl == false) SendShow(myTCP.errorMsg);
            else SendShow("OK");
        }

        private void btRead_Click(object sender, EventArgs e)
        {
            bool rtl;
            string msg = cbReadMsg.Text;
            ReadShow(msg);
            string read = "";
            rtl = myTCP.Instruction_Read(msg, ref read);
            if (rtl == false) SendShow(myTCP.errorMsg);
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
            CTLFunc.ConvertConSta(TCP, sta);
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
