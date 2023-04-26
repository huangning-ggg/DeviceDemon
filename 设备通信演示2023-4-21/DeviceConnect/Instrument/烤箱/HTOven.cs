using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace DeviceConnect
{
    /// <summary>海拓温湿度箱
    /// 注意：必需添加引用空间 using System.IO.Ports;
    /// </summary>
    public class HTOven : ComConnectBase
    {
        #region 声明、属性、字段
        SerialPortHelper sp = new SerialPortHelper();

        public string errorMsg { set; get; }

        public bool isConnect { set; get; }

        public string address;

        int BaurdRate;
        #endregion

        #region 构造构析及基本函数
        public bool Connect(string com, int baurdRate = 9600)
        {
            try
            {
                sp.parity = System.IO.Ports.Parity.None;
                sp.stopBits = System.IO.Ports.StopBits.Two;
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
                    //string deviceName = ReadMsg("*IDN?\r\n");
                    //if (deviceName.Contains("xxx") == false)
                    //{
                    //    Disconnect();
                    //    return false;
                    //}
                    //else
                    //{
                    //    isConnect = true;
                    //    return true;
                    //}
                    return true;
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

        public string ReadLine(string str)
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
                return sp.ComReadHexDelay(cmd);
            }
        }
        #endregion

        #region 功能函数
        public bool SetConfig(string id)
        {
            try
            {
                string deviceName = ReadLine("*IDN?");
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
                read = ReadLine(str);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0} 函数：Instruction_Read()\r\n{1}", address, ex.Message);
                return false;
            }
        }
        #endregion

        public bool SetTemperature(UInt16 Tem)
        {
            try
            {
                List<byte> temp = new List<byte>();
                temp.AddRange(new byte[] { 0x01, 0x06, 0x00, 0x00 });
                byte[] b = BitConverter.GetBytes(Tem);
                temp.AddRange(b.Reverse());
                byte[] crc = sp.ModBusCRC16(temp.ToArray());
                temp.AddRange(crc);
                byte[] rtl = this.ReadBytes(temp.ToArray());
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0} 函数：SetTemperature()\r\n{1}", address, ex.Message);
                return false;
            }
        }

        public bool GetSetTemperature(ref double Tem)
        {
            try
            {
                List<byte> temp = new List<byte>();
                temp.AddRange(new byte[] { 0x01, 0x03, 0x00, 0x00, 0x00, 0x01, 0x84, 0x0a });
                byte[] rtl = this.ReadBytes(temp.ToArray());
                if (rtl.Length != 7) throw new Exception(string.Format("返回数组长度：{0} 不为7", rtl.Length));
                Tem = rtl[3] * 256 + rtl[4];
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0} 函数：GetSetTemperature()\r\n{1}", address, ex.Message);
                return false;
            }
        }

        public bool GetCurTemperature(ref double Tem)
        {
            try
            {
                List<byte> temp = new List<byte>();
                temp.AddRange(new byte[] { 0x01, 0x03, 0x00, 0xC0, 0x00, 0x01, 0x84, 0x36 });
                byte[] rtl = this.ReadBytes(temp.ToArray());
                if (rtl.Length != 7) throw new Exception(string.Format("返回数组长度：{0} 不为7", rtl.Length));
                Tem = rtl[3] * 256 + rtl[4];
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0} 函数：GetCurTemperature()\r\n{1}", address, ex.Message);
                return false;
            }
        }
    }
}
