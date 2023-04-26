using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceConnect
{
    public class PSS_OPM_S
    {
        #region Template
        #region 声明、属性、字段
        SerialPortHelper rs232 = new SerialPortHelper();

        public string errorMsg;

        public bool isConnect;

        string address;

        int BaurdRate;

        public System.IO.Ports.Parity parity;//奇偶校验位

        public System.IO.Ports.StopBits stopBits;//停止位

        public int dataBits;//数据位
        #endregion

        #region 构造构析及基本函数
        public PSS_OPM_S()//构造函数
        {
            parity = System.IO.Ports.Parity.None;
            stopBits = System.IO.Ports.StopBits.One;
            dataBits = 8;

            this.OPMOffset = new double[4] { 0, 0, 0, 0 };
        }

        ~PSS_OPM_S()//构析函数
        {
            try
            {
                if (isConnect == true) rs232.ComClose();
            }
            catch { }
        }

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
                    if (deviceName.Contains("OPM-S") == false)
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
            try
            {
                rs232.ComClose();
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
        private bool Instruction_Read(string str)
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
        #endregion

        /// <summary>光功率计补偿
        /// 
        /// </summary>
        public double[] OPMOffset;

        /// <summary>不带补偿的读取光功率函数
        /// 
        /// </summary>
        /// <param name="power"></param>
        /// <returns></returns>
        public bool ReadPower(ref double power)
        {
            try
            {
                string rtl = ReadMsg("Read:Power Channel1\r\n");
                power = Math.Round(Convert.ToDouble(rtl) * 1000, 2);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0} 函数：{1}\r\n{2}", address, "ReadPower()", ex.Message);
                return false;
            }
        }

        public double TxPowerNOOffset;

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
                string rtl = ReadMsg("Read:Power Channel1\r\n");
                power = Math.Round(power = Convert.ToDouble(rtl) * 1000, 2);
                TxPowerNOOffset = power;
                if (channel < 0 || channel >= 4)
                {
                    
                }
                else
                    power += this.OPMOffset[channel];
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0} 函数：{1}\r\n{2}", address, "ReadPower()", ex.Message);
                return false;
            }
        }
    }
}
