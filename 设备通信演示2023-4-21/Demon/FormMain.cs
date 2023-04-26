using DeviceConnect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demon
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            PortInit();
        }

        #region 串口
        PortDriverTemp portDriverTemp = new PortDriverTemp();
        void PortInit()
        {
            //显示委托
            portS1.portSerach += new 用户控件.PortSerach(GetAllPorts);
            portS1.portConnect += new 用户控件.PortConnect(PortConnect);
            portS1.portClose += new 用户控件.PortClose(PortClose);
        }

        public string[] GetAllPorts()
        {
            return System.IO.Ports.SerialPort.GetPortNames();
        }

        public string PortConnect(string port, int baurd)
        {
            bool rtl = portDriverTemp.Connect(port, baurd);
            if (rtl == false) return portDriverTemp.errorMsg;
            return "";
        }

        public bool PortClose()
        {
            return portDriverTemp.Disconnect();
        }

        private void btPortSend_Click(object sender, EventArgs e)
        {
            try
            {
                string cmd = cbPortCmd.Text.Trim();
                cmd = string.Format("{0}{1}", cmd, cbAddLine.Checked ? "\r\n" : "");
                if (cmd == "") throw new Exception("输入命令不能为空！！！");

                if (cbString.Checked == true)
                {
                    portDriverTemp.SendMsg(cmd);
                }
                else
                {
                    byte[] byteCMD = PortFunction.Str2ArrayShow(cmd);
                    if (cbCRC.Checked == true)
                    {
                        byte[] crc = PortFunction.ModBusCRC16(byteCMD);
                        byteCMD = byteCMD.Concat(crc).ToArray();
                    }
                    portDriverTemp.SendByte(byteCMD);
                }
                PortSendAdd(cmd);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                PortSendAdd(ex.Message);
            }
        }

        private void btPortRead_Click(object sender, EventArgs e)
        {
            try
            {
                string cmd = cbPortCmd.Text.Trim().Replace(" ","");
                string ret = "";
                cmd = string.Format("{0}{1}", cmd, cbAddLine.Checked ? "\r\n" : "");
                if (cmd == "") throw new Exception("输入命令不能为空！！！");
                PortReadAdd(cmd);
                if (rbPortReadLine.Checked == true)
                {
                    if (cbString.Checked == true)
                    {
                        ret=portDriverTemp.ReadLine(cmd);
                    }
                    else
                    {
                        byte[] byteCMD = PortFunction.Str2ArrayShow(cmd);
                        if (cbCRC.Checked == true)
                        {
                            byte[] crc = PortFunction.ModBusCRC16(byteCMD);
                            byteCMD = byteCMD.Concat(crc).ToArray();
                        }
                        byte[] rByte= portDriverTemp.ReadBytes(byteCMD);
                        ret = PortFunction.Array2StrShow(rByte);
                    }
                    
                }
                if (rbPortReadTo.Checked == true)
                {
                    if (cbString.Checked == true)
                    {
                        ret = portDriverTemp.ReadLine(cmd);
                    }
                    else
                    {
                        byte[] byteCMD = PortFunction.Str2ArrayShow(cmd);
                        if (cbCRC.Checked == true)
                        {
                            byte[] crc = PortFunction.ModBusCRC16(byteCMD);
                            byteCMD = byteCMD.Concat(crc).ToArray();
                        }
                        byte[] rByte = portDriverTemp.ReadBytes(byteCMD);
                        ret = PortFunction.Array2StrShow(rByte);
                    }
                }
                if (rbPortReadDelay.Checked == true)
                {
                    if (cbString.Checked == true)
                    {
                        ret = portDriverTemp.ReadDelay(cmd);
                    }
                    else
                    {
                        byte[] byteCMD = PortFunction.Str2ArrayShow(cmd);
                        if (cbCRC.Checked == true)
                        {
                            byte[] crc = PortFunction.ModBusCRC16(byteCMD);
                            byteCMD = byteCMD.Concat(crc).ToArray();
                        }
                        byte[] rByte = portDriverTemp.ReadBytesDelay(byteCMD);
                        ret = PortFunction.Array2StrShow(rByte);
                    }
                }
                PortReadAdd(ret);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                PortReadAdd(ex.Message);
            }
        }

        public void PortSendAdd(string msg)
        {
            this.Invoke((Action)delegate
            {
                lbPortSend.Items.Add(string.Format("{0}:{1}",DateTime.Now.ToString(),msg));
                lbPortSend.TopIndex = lbPortSend.Items.Count - 1;
            });
        }

        public void PortReadAdd(string msg)
        {
            this.Invoke((Action)delegate
            {
                lbPortRead.Items.Add(string.Format("{0}:{1}", DateTime.Now.ToString(), msg));
                lbPortRead.TopIndex = lbPortSend.Items.Count - 1;
            });
        }
        #endregion

        private void rbPortReadDelay_CheckedChanged(object sender, EventArgs e)
        {
            DAQM_4203 t1 = new DAQM_4203();
            bool rtl= t1.Connect("COM9",9600);
            double val = 0;
            string msg = t1.GetCHVal(1, ref val);
        }
    }
}
