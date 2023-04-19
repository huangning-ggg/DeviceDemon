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
    public class PSS_AOS
    {
        #region 声明、属性、字段
        SerialPortHelper sp = new SerialPortHelper();

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
                sp.parity = System.IO.Ports.Parity.None;
                sp.stopBits = System.IO.Ports.StopBits.One;
                sp.dataBits = 8;
                //sp.ReadTo = "";
                this.address = com;
                this.BaurdRate = baurdRate;

                if (sp.ComConnect(com, baurdRate) == false)
                {
                    this.errorMsg = string.Format("地址：{0}（波特率：{1}）连接异常\r\n{2}", address, BaurdRate, sp.errorMsgBase);
                    isConnect = false;
                    return false;
                }
                else
                {
                    isConnect = true;
                    string deviceName = ReadMsg("*IDN?\r\n");
                    if (deviceName.Contains("PSS,AOS") == false)
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
            if (sp.ComClose() == false)
            {
                this.errorMsg = string.Format("地址：{0}（波特率：{1}）断开异常\r\n{2}", address, BaurdRate, sp.errorMsgBase);
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
                sp.ComWriteString(str);
            }
        }

        public string ReadMsg(string str)
        {
            lock (portLock)
            {
                if (isConnect == false)
                    throw new Exception(string.Format("地址：{0} 已断开", address));
                return sp.ComReadString(str);
            }
        }

        public string ReadMsg(byte[] cmd)
        {
            lock (portLock)
            {
                if (isConnect == false)
                    throw new Exception(string.Format("地址：{0} 已断开", address));
                sp.Delay = 500;
                return sp.ReadExisting(cmd);
            }
        }

        public void SendByte(byte[] cmd)
        {
            lock (portLock)
            {
                if (isConnect == false)
                    throw new Exception(string.Format("地址：{0} 已断开", address));
                sp.ComWriteHex(cmd);
            }
        }

        public byte[] ReadBytes(byte[] cmd)
        {
            lock (portLock)
            {
                if (isConnect == false)
                    throw new Exception(string.Format("地址：{0} 已断开", address));
                sp.Delay = 200;
                return sp.ComReadHexDelay(cmd);
            }
        }
        #endregion

        #region 功能函数
        public bool SetConfig(string id)
        {
            try
            {
                string deviceName = ReadMsg("*IDN?");
                if (deviceName.Contains(id) == false) throw new Exception(string.Format("返回ID：{0} 不包含：{1}", deviceName, id));
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0} 函数：SetConfig()\r\n{1}", address, ex.Message);
                return false;
            }
        }

        /* 发送接收指令
         * 字符串发送读取        
         * base.ComWriteString(cmd)
         * string recessive= base.ComReadString(cmd)
         * 数组发送读取
         * base.ComWriteHex(cmd)
         * byte[] recessive= base.ComReadHex(cmd)
         */
        //1、只发类型
        public bool Instruction_Send(string str)
        {
            try
            {
                SendMsg(str);

                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0} 函数：Instruction_Send()\r\n{1}", address, ex.Message);
                return false;
            }
        }

        //2、回读类型
        public bool Instruction_Read(string str, ref string read)
        {
            try
            {
                read = ReadMsg(str);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0} 函数：Instruction_Read()\r\n{1}", address, ex.Message);
                return false;
            }
        }
        #endregion

        /// <summary>
        /// ch:1234
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public string ChoiseChannel(byte ch)
        {
            try
            {
                string str = string.Format("Configure:Setwave LD{0}\n", ch);
                SendMsg(str);
                System.Threading.Thread.Sleep(100);
                str = string.Format("Configure:Getwave?\n");
                string rtl = ReadMsg(str);
                if (rtl != string.Format("LD{0}", ch)) throw new Exception(string.Format("选择通道{0} 失败！！！", ch));
                return "";
            }
            catch (Exception ex)
            {
                return string.Format("地址：{0} 函数：ChoiseChannel()\r\n{1}", address, ex.Message);
            }
        }

        public string Open(byte ch)
        {
            try
            {
                string rtl = ChoiseChannel(ch);
                if (rtl != "") throw new Exception(rtl);
                string str = string.Format("Configure:SetStatus ON\n");
                SendMsg(str);
                return "";
            }
            catch (Exception ex)
            {
                return string.Format("地址：{0} 函数：Open()\r\n{1}", address, ex.Message);
            }
        }

        public string Close(byte ch)
        {
            try
            {
                string rtl = ChoiseChannel(ch);
                if (rtl != "") throw new Exception(rtl);
                string str = string.Format("Configure:SetStatus OFF\n");
                SendMsg(str);
                return "";
            }
            catch (Exception ex)
            {
                return string.Format("地址：{0} 函数：Close()\r\n{1}", address, ex.Message);
            }
        }

        public string OpenAll()
        {
            try
            {
                for (byte ch = 1; ch < 5; ch++)
                {
                    string rtl = Open(ch);
                    if (rtl != "") throw new Exception(rtl);
                    System.Threading.Thread.Sleep(100);
                }
                return "";
            }
            catch (Exception ex)
            {
                return string.Format("地址：{0} 函数：OpenAll()\r\n{1}", address, ex.Message);
            }
        }

        public string CloseAll()
        {
            try
            {
                for (byte ch = 1; ch < 5; ch++)
                {
                    string rtl = Close(ch);
                    if (rtl != "") throw new Exception(rtl);
                    System.Threading.Thread.Sleep(100);
                }
                return "";
            }
            catch (Exception ex)
            {
                return string.Format("地址：{0} 函数：CloseAll()\r\n{1}", address, ex.Message);
            }
        }

    }
}
