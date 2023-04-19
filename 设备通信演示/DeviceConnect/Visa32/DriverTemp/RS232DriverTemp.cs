using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeviceConnect;

namespace DeviceConnect
{
    public class RS232DriverTemp : ComConnectBase
    {
        #region 声明、属性、字段
        RS232_VISA32 rs232;

        public string errorMsg;

        public bool isConnect { set; get; }

        string address;

        int BaurdRate;

        public System.IO.Ports.Parity parity;//奇偶校验位

        public System.IO.Ports.StopBits stopBits;//停止位

        public int dataBits;//数据位
        #endregion

        #region 构造构析及基本函数
        public RS232DriverTemp()//构造函数
        {
            parity = System.IO.Ports.Parity.None;
            stopBits = System.IO.Ports.StopBits.One;
            dataBits = 8;
        }

        ~RS232DriverTemp()//构析函数
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
                this.errorMsg = string.Format("地址：{0} 函数：SetConfig()\r\n{1}", address, ex.Message);
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
    }
}
