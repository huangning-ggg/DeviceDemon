using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace DeviceConnect
{
    public class PSS_OPS1X401 : ComConnectBase
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
                    string idn = this.ReadLine("*IDN?\r\n");
                    if (idn.Trim().Length == 0 || (idn.Contains("PSS,OPS1X401") == false))
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

        public void SendMsg(string str)
        {
            if (isConnect == false)
                throw new Exception(string.Format("地址：{0} 已断开", address));
            port.ComWriteString(str);
        }

        public string ReadLine(string str)
        {
            if (isConnect == false)
                throw new Exception(string.Format("地址：{0} 已断开", address));
            return port.ComReadString(str);

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
        public string Instruction_Read(string str)
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

        /// <summary>_channel:1-4
        /// 
        /// </summary>
        /// <param name="_channel"></param>
        /// <param name="enableStatus"></param>
        /// <returns></returns>
        public bool SetOptSwitchEnableStatus(int _channel, bool enableStatus)
        {
            bool bResult = false;
            string TxCmd = "";
            string RxBuffer = "";

            TxCmd = string.Format("Configure:WorkChannel {0}\n", _channel);

            RxBuffer = this.ReadLine(TxCmd); //"Config:1:OK!\n"
            if (RxBuffer.ToUpper().Contains("OK"))
            {
                bResult = true;
            }

            TxCmd = enableStatus ? "Configure:Switch1OnOff 1\n" : "Configure:Switch1OnOff 0\n";

            RxBuffer = this.ReadLine(TxCmd); //"Config:OFF:OK!\n"
            if (RxBuffer.ToUpper().Contains("OK"))
            {
                bResult &= true;
            }

            if (bResult == false) throw new Exception("PSS_OPS1X401光开关切换失败！");
            return bResult;
        }
    }
}
