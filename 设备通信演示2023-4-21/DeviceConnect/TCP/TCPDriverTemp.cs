using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DeviceConnect
{
    public class TCPDriverTemp
    {
        #region 声明、属性、字段
        TCPHelper tcp = new TCPHelper();
        /// <summary>错误信息提示
        /// 
        /// </summary>
        public string errorMsg;

        /// <summary>是否连接
        /// 
        /// </summary>
        public bool isConnect;

        public string IP;

        public string Port;
        #endregion

        #region 构造构析及基本函数
        /// <summary>连接
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool Connect(string ip, string port)
        {
            try
            {
                this.IP = ip;
                this.Port = port;
                tcp.ConnectServer(ip, port);
                this.isConnect = true;
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = ex.Message;
                return false;
            }
        }

        /// <summary>断开
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Disconnect()
        {
            try
            {
                if (this.isConnect == false) throw new Exception("设备未连接！");
                tcp.DisConnectServer();
                this.isConnect = false;
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = ex.Message;
                this.isConnect = false;
                return false;
            }
        }

        object tcpLock = new object();//通道锁，同一时间只能执行一次读或写
        /// <summary>只发送数据
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public void SendMsg(string msg)
        {
            lock (tcpLock)
            {
                if (this.isConnect == false) throw new Exception("设备未连接！");
                tcp.SendMsg(msg);
            }
        }

        /// <summary>发送并接收数据
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string ReadMsg(string msg)
        {
            lock (tcpLock)
            {
                string data;
                if (this.isConnect == false) throw new Exception("设备未连接！");
                tcp.SendMsg(msg);
                data = tcp.ReceiveMsg().Replace(" ", "");
                return data;
            }
        }
        #endregion

        #region 功能函数
        public bool SetConfig(string id)
        {
            try
            {
                string deviceName = ReadMsg("*IDN?");
                if (deviceName.Contains(id) == false) throw new Exception(string.Format("返回ID：{0} 不包含：{1}", deviceName, id));
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("IP:{0} Port:{1} 函数：SetConfig()\r\n{2}", IP, Port, ex.Message);
                return false;
            }
        }

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
                this.errorMsg = string.Format("IP:{0} Port:{1} 函数：Instruction_Send()\r\n{2}", IP, Port, ex.Message);
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
                this.errorMsg = string.Format("IP:{0} Port:{1} 函数：Instruction_Read()\r\n{2}", IP, Port, ex.Message);
                return false;
            }
        }
        #endregion
    }
}
