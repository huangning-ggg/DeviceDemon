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
    public class PortDriverTemp : ComConnectBase
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
        #endregion

        #region 字符串读写
        object portLock = new object();//通道锁，同一时间只能执行一次读或写
        /// <summary>写入字符串
        /// 
        /// </summary>
        /// <param name="str"></param>
        public void SendMsg(string str)
        {
            lock (portLock)
            {
                if (isConnect == false)
                    throw new Exception(string.Format("地址：{0} 已断开", address));
                sp.ComWriteString(str);
            }
        }

        /// <summary>读取字符串，返回结束符为换行符
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string ReadLine(string str)
        {
            lock (portLock)
            {
                if (isConnect == false)
                    throw new Exception(string.Format("地址：{0} 已断开", address));
                return sp.ComReadString(str);
            }
        }

        /// <summary>读取字符串，返回结束符为指定符号，默认情况下，等同于ReadLine
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="ReadTo"></param>
        /// <returns></returns>
        public string ReadTo(string str, string ReadTo = "\r\n")
        {
            lock (portLock)
            {
                if (isConnect == false)
                    throw new Exception(string.Format("地址：{0} 已断开", address));

                return sp.ComReadStringTo(str, ReadTo);
            }
        }

        /// <summary>读取字符串，延时一定时间后，读取接收缓存中所有的数据
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="ReadTo"></param>
        /// <returns></returns>
        public string ReadDelay(string str, int delay = 200)
        {
            lock (portLock)
            {
                if (isConnect == false)
                    throw new Exception(string.Format("地址：{0} 已断开", address));

                return sp.ComReadStringDelay(str, delay);
            }
        }
        #endregion

        #region 数组读写
        /// <summary>发送数组数据
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        public void SendByte(byte[] cmd)
        {
            lock (portLock)
            {
                if (isConnect == false)
                    throw new Exception(string.Format("地址：{0} 已断开", address));
                sp.ComWriteHex(cmd);
            }
        }

        /// <summary>读取数组数据，发送类型为数组，默认结束符为换行符
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="ReadTo"></param>
        /// <returns></returns>
        public byte[] ReadBytes(byte[] cmd, string ReadTo = "\r\n")
        {
            lock (portLock)
            {
                if (isConnect == false)
                    throw new Exception(string.Format("地址：{0} 已断开", address));
                return sp.ComReadHex(cmd, ReadTo);
            }
        }

        /// <summary>读取数组数据，发送类型为数组，默认结束符为换行符
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="ReadTo"></param>
        /// <returns></returns>
        public byte[] ReadBytesDelay(byte[] cmd, int delay = 200)
        {
            lock (portLock)
            {
                if (isConnect == false)
                    throw new Exception(string.Format("地址：{0} 已断开", address));
                sp.Delay = 200;
                return sp.ComReadHexDelay(cmd, delay);
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
    }
}
