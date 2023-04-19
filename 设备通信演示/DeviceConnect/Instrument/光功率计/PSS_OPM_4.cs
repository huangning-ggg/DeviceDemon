using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceConnect
{
    public class PSS_OPM_4 : ComConnectBase
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
                    if (idn.Trim().Length == 0 || (idn.Contains("PSS,OPM-4") == false))
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

        /// <summary>光功率计补偿
        /// 
        /// </summary>
        public OPMOffset opmOffset = new OPMOffset();

        public double TxPowerNOOffset;
        object opmlock = new object();
        /// <summary>不带补偿的读取光功率函数
        /// 0-3
        /// </summary>
        /// <param name="power"></param>
        /// <returns></returns>
        public bool ReadPower(ref double power, int OPMchannel)
        {
            try
            {
                lock (opmlock)
                {
                    string rtl = this.ReadMsg(string.Format("Read:Power Channel{0}\r", OPMchannel + 1));
                    rtl = rtl.Replace(string.Format("Channel{0}", OPMchannel + 1), "");
                    power = Convert.ToDouble(rtl.Trim());
                    power = dBm2uW(power);
                    power = Math.Round(power, 2);
                    return true;
                }
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
                TxPowers[i] = this.ReadPower(ref power, i) ? power : -99;
            }
            return TxPowers;
        }

        /// <summary>带补偿的读取光功率函数
        /// 
        /// </summary>
        /// <param name="power"></param>
        /// <param name="OPMchannel">光功率计通道(0|1|2|3)</param>
        /// <param name="PartChannel">模块通道(0|1|2|3)</param>
        /// <returns></returns>
        public bool ReadPower(ref double power, int OPMchannel, int PartChannel)
        {
            try
            {
                string rtl = this.ReadMsg(string.Format("Read:Power Channel{0}\r", OPMchannel + 1));
                rtl = rtl.Replace(string.Format("Channel{0}", OPMchannel + 1), "");
                power = Convert.ToDouble(rtl.Trim());
                power = dBm2uW(power);
                power = Math.Round(power, 2);
                TxPowerNOOffset = power;
                if (PartChannel < 0 || PartChannel >= 4)
                { }
                else power = power + this.opmOffset.GetOffset(OPMchannel)[PartChannel];
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0} 函数：{1}\r\n{2}", address, "ReadPowerOffset()", ex.Message);
                return false;
            }
        }

        private double dBm2uW(double dBm)
        {
            return Math.Pow(10, dBm / 10) * 1000;
        }

    }

    public class OPMOffset
    {
        public OPMOffset()
        {
            OPM1 = new double[4];
            OPM2 = new double[4];
            OPM3 = new double[4];
            OPM4 = new double[4];
        }
        public bool isCheck1;
        public bool isCheck2;
        public bool isCheck3;
        public bool isCheck4;

        public double[] OPM1;
        public double[] OPM2;
        public double[] OPM3;
        public double[] OPM4;

        /// <summary>OPMchannel:(0|1|2|3)
        /// 
        /// </summary>
        /// <param name="OPMchannel"></param>
        /// <returns></returns>
        public double[] GetOffset(int OPMchannel)
        {
            if (OPMchannel == 0) return this.OPM1;
            else if (OPMchannel == 1) return this.OPM2;
            else if (OPMchannel == 2) return this.OPM3;
            else if (OPMchannel == 3) return this.OPM4;
            else return new double[4];
        }

        /// <summary>OPMchannel:(0|1|2|3)
        /// 
        /// </summary>
        /// <param name="OPMchannel"></param>
        /// <param name="offset"></param>
        public void SetOffset(int OPMchannel, double[] offset)
        {
            if (OPMchannel == 0) this.OPM1 = offset;
            else if (OPMchannel == 1) this.OPM2 = offset;
            else if (OPMchannel == 2) this.OPM3 = offset;
            else if (OPMchannel == 3) this.OPM4 = offset;
        }

        /// <summary>OPMchannel:(0|1|2|3)
        /// 
        /// </summary>
        /// <param name="OPMchannel"></param>
        /// <param name="ischeck"></param>
        public void SetCheckSta(int OPMchannel, bool ischeck)
        {
            if (OPMchannel == 0) this.isCheck1 = ischeck;
            else if (OPMchannel == 1) this.isCheck2 = ischeck;
            else if (OPMchannel == 2) this.isCheck3 = ischeck;
            else if (OPMchannel == 3) this.isCheck4 = ischeck;
        }

        /// <summary>OPMchannel:(0|1|2|3)
        /// 
        /// </summary>
        /// <param name="OPMchannel"></param>
        /// <returns></returns>
        public bool GetCheckSta(int OPMchannel)
        {
            if (OPMchannel == 0) return this.isCheck1;
            else if (OPMchannel == 1) return this.isCheck2;
            else if (OPMchannel == 2) return this.isCheck3;
            else if (OPMchannel == 3) return this.isCheck4;
            else return false;
        }

    }
}
