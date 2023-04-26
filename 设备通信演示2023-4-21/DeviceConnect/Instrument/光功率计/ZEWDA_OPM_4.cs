using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceConnect
{
    public class ZEWDA_OPM_4
    {
        #region base
        SerialPortHelper port = new SerialPortHelper();

        public string errorMsg;

        public bool isConnect { set; get; }

        string address;

        int BaurdRate;

        public bool Connect(string com, int baurdRate = 115200)
        {
            try
            {
                port.parity = System.IO.Ports.Parity.None;
                port.stopBits = System.IO.Ports.StopBits.One;
                port.dataBits = 8;
                this.address = com;
                this.BaurdRate = baurdRate;
                if (port.ComConnect(com, baurdRate) == false)
                {
                    this.errorMsg = string.Format("地址：{0}（波特率：{1}）连接异常\r\n{2}", address, BaurdRate, port.errorMsgBase);
                    isConnect = false;
                    return false;
                }
                else
                {
                    isConnect = true;
                    this.SendMsg("*IDN?\r\n");
                    string idn = this.ReadMsg("*IDN?\r\n");
                    if (idn.Trim().Length == 0 || (idn.Contains("ZLPMA") == false))
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
            if (port.ComClose() == false)
            {
                this.errorMsg = string.Format("地址：{0}（波特率：{1}）断开异常\r\n{2}", address, BaurdRate, port.errorMsgBase);
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
                port.ComWriteString(str);
            }
        }

        public string ReadMsg(string str)
        {
            lock (portLock)
            {
                if (isConnect == false)
                    throw new Exception(string.Format("地址：{0} 已断开", address));
                return port.ComReadString(str);
            }
        }
        #endregion

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
                string test = ReadMsg(str);
                return test;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("地址：{0} 函数：{1}\r\n{2}", address, "Instruction_Read()", ex.Message));
            }
        }

        /// <summary>不带补偿的读取光功率函数
        /// OPMchannel:1-4
        /// </summary>
        /// <param name="power"></param>
        /// <param name="OPMchannel"></param>
        /// <returns></returns>
        public bool ReadPower(int OPMchannel, ref double power)
        {
            try
            {
                string rtl = this.ReadMsg(string.Format("SOUR{0}:POW?\r", OPMchannel));
                rtl = rtl.Replace(string.Format("Channel{0}", OPMchannel), "");
                power = Convert.ToDouble(rtl.Trim());
                power = dBm2uW(power);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0} 函数：{1}\r\n{2}", address, "ReadPower()", ex.Message);
                return false;
            }
        }

        public double[] GetAllPower()
        {
            double[] TxPowers = new double[4];
            double power = 0;
            for (byte i = 0; i < 4; i++)
            {
                TxPowers[i] = this.ReadPower(i, ref power) ? power : -99;
            }
            return TxPowers;
        }

        private double dBm2uW(double dBm)
        {
            return Math.Pow(10, dBm / 10) * 1000;
        }
    }
}
