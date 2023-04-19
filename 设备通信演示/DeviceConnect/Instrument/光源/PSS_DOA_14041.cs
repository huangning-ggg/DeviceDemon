﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceConnect
{
    /// <summary>该设备驱动作为光源使用
    /// 
    /// </summary>
    public class PSS_DOA_14041_OPS : IOPS
    {
        #region base
        public PSS_DOA_14041_OPS()
        {
            opsCheck = new OPSSetAtt();
        }


        SerialPortHelper port = new SerialPortHelper();

        public string errorMsg { set; get; }

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
                    string deviceName = ReadMsg("*IDN?\r\n");
                    if (deviceName.ToUpper().Contains("PSS,DOA14041") == false)
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
                string test = ReadMsg(str);
                return test;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("地址：{0} 函数：{1}\r\n{2}", address, "Instruction_Read()", ex.Message));
            }
        }




        byte Channel = 4;

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
                bool rtl = this.SetATT(channel, 0);
                if (rtl == false) throw new Exception(string.Format("通道{0}开启失败！", channel));
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
                bool rtl = this.SetATT(channel, 40);
                if (rtl == false) throw new Exception(string.Format("通道{0}开启失败！", channel));
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
        /// channel:1-4
        /// </summary>
        /// <param name="channel"></param>
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

        /// <summary>光衰中设置的值为负数，为保持与光源设置格式一致，在内部转为负数
        /// 
        /// </summary>
        /// <param name="channel">1-4</param>
        /// <param name="ATT"></param>
        /// <returns></returns>
        public bool SetATT(byte channel, double ATT)
        {
            try
            {
                ATT *= -1;
                ATT = Math.Round(ATT, 2);
                if (this.isConnect == false) throw new Exception("设备未连接！");
                if (channel > this.Channel) throw new Exception("通道设置错误，正确的通道为1-4");
                string cmd = string.Format("Configure:Atten Channel{0} {1}\n", channel, ATT);
                string rtl = ReadMsg(cmd);
                System.Threading.Thread.Sleep(200);                
                double getatt =0;
                if (this.GetATT(channel, ref getatt) == false) throw new Exception(string.Format("读取通道{0} 衰减值失败！", channel));
                if (getatt != ATT*-1) throw new Exception(string.Format("读取通道{0} 衰减值：{1} 与设置值：{2}不一致", channel, getatt, ATT));
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0} 函数：{1}\r\n{2}", address, "SetATT()", ex.Message);
                return false;
            }
        }

        /// <summary>channel:1-4
        /// 
        /// </summary>
        /// <param name="channel">1-4</param>
        /// <param name="power"></param>
        /// <returns></returns>
        public bool GetATT(byte channel, ref double ATT)
        {
            try
            {
                if (this.isConnect == false) throw new Exception("设备未连接！");
                if (channel > this.Channel) throw new Exception("通道设置错误，正确的通道为1-4");
                string cmd = string.Format("Configure:Atten? Channel{0}\r", channel);
                string rtl = ReadMsg(cmd);
                rtl = rtl.Replace("\r", "");
                ATT = Convert.ToDouble(rtl);
                ATT *= -1;
                ATT = Math.Round(ATT, 2);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0} 函数：{1}\r\n{2}", address, "GetATT()", ex.Message);
                return false;
            }
        }

        public OPSSetAtt opsCheck { set; get; }

        /// <summary>设置4个通道的衰减值
        /// 
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        public bool SetATTFour(int station)
        {
            bool rtl = SetATT(1, opsCheck.GetATTVal(station)[0]);
            System.Threading.Thread.Sleep(200);
            rtl &= SetATT(2, opsCheck.GetATTVal(station)[1]);
            System.Threading.Thread.Sleep(200);
            rtl &= SetATT(3, opsCheck.GetATTVal(station)[2]);
            System.Threading.Thread.Sleep(200);
            rtl &= SetATT(4, opsCheck.GetATTVal(station)[3]);
            System.Threading.Thread.Sleep(200);
            return rtl;
        }

        /// <summary>设置四个通道指定衰减值
        /// 
        /// </summary>
        /// <param name="Atts"></param>
        /// <returns></returns>
        public bool SetATTFour(double[] Atts)
        {
            bool rtl = SetATT(1, Atts[0]);
            System.Threading.Thread.Sleep(200);
            rtl &= SetATT(2, Atts[1]);
            System.Threading.Thread.Sleep(200);
            rtl &= SetATT(3, Atts[2]);
            System.Threading.Thread.Sleep(200);
            rtl &= SetATT(4, Atts[3]);
            System.Threading.Thread.Sleep(200);
            return rtl;
        }

        private double uW2dBm(uint uw)
        {
            return Math.Round(Math.Log10(uw / 1000.0) * 10, 2);
        }

        private double dBm2uW(double dBm)
        {
            return Math.Round(Math.Pow(10, dBm / 10) * 1000);
        }
    }
}
