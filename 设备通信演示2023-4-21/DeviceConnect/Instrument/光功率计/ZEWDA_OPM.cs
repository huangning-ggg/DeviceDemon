using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceConnect
{
    public class ZEWDA_OPM
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
                    string deviceName = ReadMsg("*IDN?\r\n");
                    if (deviceName.Contains("ZHPMA") == false)
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

        public string ReadMsg(string str)
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
                string test = ReadMsg(str);
                return test;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("地址：{0} 函数：{1}\r\n{2}", address, "Instruction_Read()", ex.Message));
            }
        }
        #endregion
        #endregion

        /// <summary>光功率计补偿
        /// 
        /// </summary>
        public double[] OPMOffset;

        public double TxPowerNOOffset;

        /// <summary>不带补偿的读取光功率函数
        /// 
        /// </summary>
        /// <param name="power"></param>
        /// <returns></returns>
        public bool ReadPower(ref double power)
        {
            try
            {
                string cmd = string.Format("SOUR:CH{0}:POW?\r\n", 1);
                string value = this.ReadMsg(cmd);
                double coeff = 1;
                if (value.Contains("nW") == true)
                { value = value.Replace("nW", ""); coeff = 0.001; }
                else if (value.Contains("uW") == true)
                { value = value.Replace("uW", ""); coeff = 1; }
                else if (value.Contains("mW") == true)
                { value = value.Replace("mW", ""); coeff = 1000; }
                power = Convert.ToSingle(value == "" ? "-1" : value);
                power = power * coeff;
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0} 函数：{1}\r\n{2}", address, "ReadPower()", ex.Message);
                return false;
            }
        }
       
        /// <summary>带补偿的读取光功率函数
        /// 
        /// </summary>
        /// <param name="power"></param>
        /// <param name="channel">通道表示的是模块的1-4通道，通道1、2、3、4 ->channel选值范围(0|1|2|3)</param>
        /// <returns></returns>
        public bool ReadPower(ref double power, int channel)
        {
            try
            {
                string cmd = string.Format("SOUR:CH{0}:POW?\r\n", 1);
                string value = this.ReadMsg(cmd);
                double coeff = 1;
                if (value.Contains("nW") == true)
                { value = value.Replace("nW", ""); coeff = 0.001; }
                else if (value.Contains("uW") == true)
                { value = value.Replace("uW", ""); coeff = 1; }
                else if (value.Contains("mW") == true)
                { value = value.Replace("mW", ""); coeff = 1000; }
                power = Convert.ToSingle(value == "" ? "-1" : value);
                power = power * coeff + OPMOffset[channel];
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0} 函数：{1}\r\n{2}", address, "ReadPower()", ex.Message);
                return false;
            }
        }

        /// <summary>设置光功率计读取单位
        /// 
        /// </summary>
        /// <param name="isdBm">true:dbm,false:mW</param>
        /// <returns></returns>
        public bool ConvertuWordBm(bool isdBm)
        {
            try
            {
                string unit = isdBm ? "dbm" : "mW";
                string cmd = string.Format("SOUR:UNIT {0}", unit);
                this.SendMsg(cmd);
                System.Threading.Thread.Sleep(200);
                cmd = "SOUR:UNIT?";
                string rtl = this.ReadMsg(cmd);
                rtl = rtl.TrimEnd(new char[] { '\r' });
                if (rtl != unit)
                    throw new Exception(string.Format("读取众旺达单位：{0} 与设置单位：{1} 不一致！", rtl, unit));
                return true;
            }
            catch (Exception e)
            {
                errorMsg = string.Format("ConvertuWordBm() error！\r\n{0}", e.Message);
                return false;
            }
            finally
            {

            }
        }
    }
}
