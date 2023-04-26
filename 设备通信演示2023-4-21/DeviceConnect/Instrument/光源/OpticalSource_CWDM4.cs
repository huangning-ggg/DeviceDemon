using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceConnect
{
    public class OpticalSource_CWDM4
    {
        #region 声明、属性、字段
        RS232_VISA32 rs232;

        public string errorMsg;

        public bool isConnect;

        string address;

        int BaurdRate;

        public System.IO.Ports.Parity parity;//奇偶校验位

        public System.IO.Ports.StopBits stopBits;//停止位

        public int dataBits;//数据位
        #endregion

        #region 构造构析及基本函数
        public OpticalSource_CWDM4()//构造函数
        {
            parity = System.IO.Ports.Parity.None;
            stopBits = System.IO.Ports.StopBits.One;
            dataBits = 8;
            Channel = 4;
        }

        ~OpticalSource_CWDM4()//构析函数
        {
            try
            {
                if (isConnect == true) rs232.Close();
            }
            catch { }
        }

        public bool Connect(string com, int baurdRate = 115200)
        {
            try
            {
                this.address = com;
                this.BaurdRate = baurdRate;
                rs232 = new RS232_VISA32(com, baurdRate, parity, stopBits, 8);
                rs232.Open();
                isConnect = true;
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0}（波特率：{1}）连接异常\r\n{2}", address, BaurdRate, ex.Message);
                isConnect = false;
                return false;
            }
        }

        public bool Disconnect()
        {
            try
            {
                rs232.Close();
                isConnect = false;
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0}（波特率：{1}）断开异常\r\n{2}", address, BaurdRate, ex.Message);
                isConnect = false;
                return false;
            }
        }

        object portLock = new object();//通道锁，同一时间只能执行一次读或写
        public void SendMsg(string str)
        {
            lock (portLock)
            {
                if (isConnect == false)
                    throw new Exception(string.Format("地址：{0} 已断开", address));
                rs232.WriteLine(str);
            }
        }

        public string ReadMsg(string str)
        {
            lock (portLock)
            {
                if (isConnect == false)
                    throw new Exception(string.Format("地址：{0} 已断开", address));
                rs232.WriteLine(str);
                return rs232.ReadLine();
            }
        }


        #endregion

        #region 功能函数
        public bool SetConfig()
        {
            try
            {
                string deviceName = ReadMsg("*IDN?");

                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0} 函数：{1}\r\n{2}", address, "SetConfig()", ex.Message);
                return false;
            }
        }
        /*指令控制*/
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
                this.errorMsg = string.Format("地址：{0} 函数：{1}\r\n{2}", address, "Instruction_Send()", ex.Message);
                return false;
            }
        }

        //2、回读类型
        public bool Instruction_Read(string str)
        {
            try
            {
                string deviceName = ReadMsg(str);

                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0} 函数：{1}\r\n{2}", address, "Instruction_Read()", ex.Message);
                return false;
            }
        }
        #endregion

        byte Channel;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel">1-4</param>
        /// <returns></returns>
        public bool OpenOPTSource(byte channel)
        {
            try
            {
                if (this.isConnect == false) throw new Exception("设备未连接！");
                if (channel > this.Channel) throw new Exception("通道设置错误，正确的通道为1-4");
                string cmd = string.Format("OUTP:SW{0}:ON", channel);
                string rtl = ReadMsg(cmd);
                string str = rtl.TrimEnd(new char[] { '\r' });
                if (str != string.Format("SW{0}_ON", channel)) throw new Exception(string.Format("通道{0}开启失败！", channel));
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0} 函数：{1}\r\n{2}", address, "OpenOPTSource()", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel">1-4</param>
        /// <returns></returns>
        public bool CloseOPTSource(byte channel)
        {
            try
            {
                if (this.isConnect == false) throw new Exception("设备未连接！");
                if (channel > this.Channel) throw new Exception("通道设置错误，正确的通道为1-4");
                string cmd = string.Format("OUTP:SW{0}:OFF", channel);
                string rtl = ReadMsg(cmd);
                string str = rtl.TrimEnd(new char[] { '\r' });
                if (str != string.Format("SW{0}_OFF", channel)) throw new Exception(string.Format("通道{0}关闭失败！", channel));
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0} 函数：{1}\r\n{2}", address, "CloseOPTSource()", ex.Message);
                return false;
            }
        }

        /// <summary>打开1-4通道光路
        /// 
        /// </summary>
        /// <returns></returns>
        public bool OpenAll()
        {
            string msg = "";
            for (byte i = 1; i < 5; i++)
            {
                msg += this.OpenOPTSource(i) ? "" : this.errorMsg + "\r\n";
                System.Threading.Thread.Sleep(100);
            }
            if (msg.Length == 0) return true;
            else { this.errorMsg = msg; return false; }
        }

        /// <summary>关闭1-4通道光路
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CloseAll()
        {
            string msg = "";
            for (byte i = 1; i < 5; i++)
            {
                msg += this.CloseOPTSource(i) ? "" : this.errorMsg + "\r\n";
                System.Threading.Thread.Sleep(100);
            }
            if (msg.Length == 0) return true;
            else { this.errorMsg = msg; return false; }
        }

        /// <summary>单独开启或关闭单独某个通道
        /// 
        /// </summary>
        /// <param name="channel">1-4</param>
        /// <returns></returns>
        public bool SetOutOnly(byte channel, bool sta)
        {
            bool rtl = true;
            if (sta == true)
            {
                rtl = CloseAll();
                rtl &= OpenOPTSource(channel);
            }
            else
            {
                rtl = OpenAll();
                rtl &= CloseOPTSource(channel);
            }
            return rtl;
        }
    }
}
