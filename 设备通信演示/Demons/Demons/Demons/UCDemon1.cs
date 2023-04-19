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
    public partial class UCDemon1 : UserControl
    {
        public UCDemon1()
        {
            InitializeComponent();
        }

        private void UCDemon1_Load(object sender, EventArgs e)
        {
            InitCtl();
        }

        public void InitCtl()
        {
            PortInit();
        }

        public ConnectCtl port;
        PortDriverTemp myPort = new PortDriverTemp();
        public void PortInit()
        {
            port = new ConnectCtl { connect = this.btConPort, close = this.btClosePort, send = this.btSendPort, read = this.btReadPort };
            ConvertPortSta(false);
            GetPorts();
        }

        private void btConPort_Click(object sender, EventArgs e)
        {
            string com = cbComPort.Text;
            int baurd = 0;
            bool rtl;
            rtl = int.TryParse(cbRate.Text, out baurd);
            if (rtl == false) return;
            rtl = myPort.Connect(com, baurd);
            if (rtl == true)
            {
                ConvertPortSta(true);
                PortSendShow(string.Format("端口：{0} 波特率：{1} 连接成功", com, baurd));
            }
            else
            {
                ConvertPortSta(false);
                PortSendShow(string.Format("端口：{0} 波特率：{1} 连接失败！！！", com, baurd));
                PortSendShow(string.Format("{0}", myPort.errorMsg));
                MessageBox.Show(myPort.errorMsg);
            }
        }

        private void btClosePort_Click(object sender, EventArgs e)
        {
            bool rtl;
            rtl = myPort.Disconnect();
            if (rtl == false)
            {
                PortSendShow(string.Format("断开失败！！！"));
                MessageBox.Show(myPort.errorMsg);
            }
            else PortSendShow(string.Format("断开成功"));
            ConvertPortSta(false);
        }

        private void btSendPort_Click(object sender, EventArgs e)
        {
            bool rtl;
            string msg = cbSendMsgPort.Text;
            PortSendShow(msg);
            rtl = myPort.Instruction_Send(msg);
            if (rtl == false) PortSendShow(myPort.errorMsg);
            else PortSendShow("OK");
        }

        private void btReadPort_Click(object sender, EventArgs e)
        {
            bool rtl;
            string msg = cbReadMsgPort.Text;
            PortReadShow(msg);
            string read = "";
            rtl = myPort.Instruction_Read(msg, ref read);
            if (rtl == false) PortReadShow(myPort.errorMsg);
            else PortReadShow(read);
        }

        private void btSearchPort_Click(object sender, EventArgs e)
        {
            GetPorts();
        }

        public void GetPorts()
        {
            cbComPort.Items.Clear();
            string[] coms = System.IO.Ports.SerialPort.GetPortNames();
            cbComPort.Items.AddRange(coms);
        }

        public void PortSendShow(string str)
        {
            LBShow(lbPortSend, str);
        }

        public void PortReadShow(string str)
        {
            LBShow(lbPortRead, str);
        }
        public void ConvertPortSta(bool sta)
        {
            CTLFunc.ConvertConSta(port, sta);
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

        private void cbReadMsgPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool rtl;
            string msg = "*IDN?\r\n";
            PortReadShow(msg);
            string read = "";
            rtl = myPort.Instruction_Read(msg, ref read);
            if (rtl == false) PortReadShow(myPort.errorMsg);
            else PortReadShow(read);
        }

        /// <summary>
        /// CRC16_Modbus效验
        /// </summary>
        /// <param name="byteData">要进行计算的字节数组</param>
        /// <returns>计算后的数组</returns>
        public byte[] ModBusCRC16(byte[] byteData)
        {
            byte[] CRC = new byte[2];

            UInt16 wCrc = 0xFFFF;
            for (int i = 0; i < byteData.Length; i++)
            {
                wCrc ^= Convert.ToUInt16(byteData[i]);
                for (int j = 0; j < 8; j++)
                {
                    if ((wCrc & 0x0001) == 1)
                    {
                        wCrc >>= 1;
                        wCrc ^= 0xA001;//异或多项式
                    }
                    else
                    {
                        wCrc >>= 1;
                    }
                }
            }

            CRC[1] = (byte)((wCrc & 0xFF00) >> 8);//高位在后
            CRC[0] = (byte)(wCrc & 0x00FF);       //低位在前
            return CRC;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //byte[] rtl = ModBusCRC16(Str2ArrayShow(cbSendMsgPort.Text));
            //PortSendShow(Array2StrShow(rtl));

            //try
            //{
            //    string msg = myPort.ReadMsg(new byte[] { 0x1B, 0x31, 0x0A, 0x0D });
            //    LBShow(lbPortRead, msg);
            //}
            //catch (Exception ex)
            //{
            //    LBShow(lbPortRead, ex.Message);
            //}     
            HTOven ht = new HTOven();
            ht.SetTemperature(85);
        }

        public byte[] Str2ArrayShow(string str)
        {
            int length = str.Length;
            int count = (int)Math.Ceiling(length / 2.0);
            List<byte> vals = new List<byte>();
            for (int i = 0; i < count; i++)
            {
                byte val = Convert.ToByte(str.Substring(2 * i, 2), 16);
                vals.Add(val);
            }
            return vals.ToArray();
        }

        public string Array2StrShow(byte[] cmd)
        {
            StringBuilder ReadString = new StringBuilder();
            foreach (byte data in cmd)
            {
                ReadString.Append(data.ToString("X2").ToUpper() + " ");
            }
            return ReadString.ToString();
        }
    }
}
