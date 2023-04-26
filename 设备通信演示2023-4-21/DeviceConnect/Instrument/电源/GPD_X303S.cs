using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace DeviceConnect
{
    /// <summary>串口类驱动范例
    /// 注意：必需添加引用空间 using System.IO.Ports;
    /// </summary>
    public class GPD_X303S : ComConnectBase
    {
        #region Template
        #region 声明、属性、字段
        SerialPortHelper rs232 = new SerialPortHelper();

        public string errorMsg;

        public bool isConnect { set; get; }

        string address;

        int BaurdRate;
        #endregion

        #region 构造构析及基本函数
        public bool Connect(string com, int baurdRate = 115200)
        {
            try
            {
                rs232.parity = System.IO.Ports.Parity.None;
                rs232.stopBits = System.IO.Ports.StopBits.One;
                rs232.dataBits = 8;
                this.address = com;
                this.BaurdRate = baurdRate;

                if (rs232.ComConnect(com, baurdRate) == false)
                {
                    this.errorMsg = string.Format("地址：{0}（波特率：{1}）连接异常\r\n{2}", address, BaurdRate, rs232.errorMsgBase);
                    isConnect = false;
                    return false;
                }
                else
                {
                    isConnect = true;
                    string deviceName = ReadLine("*IDN?\r\n");
                    if (deviceName.Contains("xxx") == false)
                    {
                        Disconnect();
                        return false;
                    }
                    else
                    {
                        isConnect = true;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0}（波特率：{1}）连接异常\r\n{2}", address, BaurdRate, ex.Message);
                Disconnect();
                return false;
            }
        }

        public bool Disconnect()
        {
            if (rs232.ComClose() == false)
            {
                this.errorMsg = string.Format("地址：{0}（波特率：{1}）断开异常\r\n{2}", address, BaurdRate, rs232.errorMsgBase);
                isConnect = false;
                return false;
            }
            else { isConnect = false; return true; }
        }

        object portLock = new object();//通道锁，同一时间只能执行一次读或写
        public void SendMsg(string str)
        {
            lock (portLock)
            {
                if (isConnect == false)
                    throw new Exception(string.Format("地址：{0} 已断开", address));
                rs232.ComWriteString(str);
            }
        }

        public string ReadLine(string str)
        {
            lock (portLock)
            {
                if (isConnect == false)
                    throw new Exception(string.Format("地址：{0} 已断开", address));
                return rs232.ComReadString(str);
            }
        }
        #endregion

        #region 功能函数
        /* 发送接收指令
         * 字符串发送读取        
         * base.ComWriteString(cmd)
         * string recessive= base.ComReadString(cmd)
         * 数组发送读取
         * base.ComWriteHex(cmd)
         * byte[] recessive= base.ComReadHex(cmd)
         */
        /*指令控制*/
        //1、只发类型
        private bool Instruction_Send(string str)
        {
            try
            {
                SendMsg(str);

                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0} 函数：{1}\r\n{2}", address, "Instruction_Send()", ex.Message);
                return false;
            }
        }

        //2、回读类型
        private string Instruction_Read(string str)
        {
            try
            {
                string test = ReadLine(str);
                return test;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("地址：{0} 函数：{1}\r\n{2}", address, "Instruction_Read()", ex.Message));
            }
        }
        #endregion
        #endregion

        public bool SetOutput(bool sta)
        {
            try
            {
                string msg = sta ? "OUT1\r\n" : "OUT0\r\n";
                SendMsg(msg);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0} 函数：{1}\r\n{2}", address, "SetOutput()", ex.Message);
                return false;
            }
        }

        public bool SetVoltage(byte ch,double voltage)
        {
            try
            {
                string msg = string.Format("VSET{0}:{1}\r\n",ch,voltage.ToString("0.000"));
                SendMsg(msg);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0} 函数：SetVoltage()\r\n{1}", address, ex.Message);
                return false;
            }
        }

        public bool GetVoltage(byte ch, ref double voltage)
        {
            try
            {
                string msg = string.Format("VOUT{0}?\r\n", ch);
                string rece= ReadLine(msg);
                voltage = Convert.ToDouble(rece);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0} 函数：GetVoltage()\r\n{1}", address, ex.Message);
                return false;
            }
        }

        public bool SetCurrent(byte ch, double Current)
        {
            try
            {
                string msg = string.Format("ISET{0}:{1}\r\n", ch, Current.ToString("0.000"));
                SendMsg(msg);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0} 函数：SetCurrent()\r\n{1}", address, ex.Message);
                return false;
            }
        }

        public bool GetCurrent(byte ch, ref double Current)
        {
            try
            {
                string msg = string.Format("IOUT{0}?\r\n", ch);
                string rece = ReadLine(msg);
                Current = Convert.ToDouble(rece);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0} 函数：GetCurrent()\r\n{1}", address, ex.Message);
                return false;
            }
        }
    }
}
