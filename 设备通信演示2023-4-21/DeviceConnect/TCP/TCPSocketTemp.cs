using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace DeviceConnect
{
    public class TCPSocketTemp : TCPSocketHelper
    {

        #region 声明、属性、字段
        /// <summary>是否连接
        /// 
        /// </summary>
        public bool isConnect;

        public string IP;

        public int Port;
        #endregion


        #region 构造构析及基本函数
        public string Connect(string ip, int port)
        {
            try
            {
                IP = ip;
                Port = port;
                string msg = base.ConnectServer(ip, port);
                if (msg != "") throw new Exception(msg);
                isConnect = true;
                return "";
            }
            catch (Exception ex)
            {
                isConnect = false;
                return ex.Message;
            }
        }

        public string Disconnect()
        {
            try
            {
                isConnect = false;
                string msg = base.DisConnectServer();
                if (msg != "") throw new Exception(msg);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        object tcpLock = new object();//通道锁，同一时间只能执行一次读或写
        public void Send(byte[] cmd)
        {
            try
            {
                Monitor.Enter(tcpLock);
                string msg = base.SendMsg(cmd);
                if (msg != "")
                {
                    if (base.isBreak == true)//断开，重连一次再发送
                    {
                        msg = ReConnect();
                        if (msg != "") throw new Exception(msg);
                        msg = base.SendMsg(cmd);
                        if (msg != "") throw new Exception(msg);
                    }
                    else
                    {
                        throw new Exception(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception( ex.Message);
            }
            finally 
            {
                Monitor.Exit(tcpLock);
            }
        }

        public void Read(byte[] cmd,ref byte[] readBuf)
        {
            try
            {
                Monitor.Enter(tcpLock);
                //先发送数据，再接收数据
                string msg = base.SendMsg(cmd);
                if (msg != "")
                {
                    if (base.isBreak == true)//断开，重连一次再发送
                    {
                        msg = ReConnect();
                        if (msg != "") throw new Exception(msg);
                        msg = base.SendMsg(cmd);
                        if (msg != "") throw new Exception(msg);
                    }
                    else
                    {
                        throw new Exception(msg);
                    }
                }
                //接收数据
                msg = base.ReceiveMsg(ref readBuf);
                if (msg != "")
                {
                    if (base.isBreak == true)//断开，重连一次再发送
                    {
                        msg = ReConnect();
                        if (msg != "") throw new Exception(msg);
                        msg = base.ReceiveMsg(ref readBuf);
                        if (msg != "") throw new Exception(msg);
                    }
                    else
                    {
                        throw new Exception(msg);
                    }
                }
                if (msg != "") throw new Exception(msg);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                Monitor.Exit(tcpLock);
            }
        }

        public string ReConnect()
        {
            try
            {
                Disconnect();
                if (Connect(this.IP, this.Port) != "") throw new Exception("断开重连失败！！！");
                return "";
            }
            catch (Exception ex)
            {
                return "断开重连失败！！！";
            }
        }
        #endregion


    }
}
