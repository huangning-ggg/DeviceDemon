using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace DeviceConnect
{
    /// <summary>使用TcpClient作为客户端与主机（被控制设备）进行通信
    /// 
    /// </summary>
    public class TCPClientHelper
    {
        private TcpClient client;
        private BinaryReader br;
        private BinaryWriter bw;

        public bool isConnect;

        public string Connect(string IP, string Port)
        {
            try
            {
                int port = 0;
                bool rtl;

                IPAddress address;
                rtl = IPAddress.TryParse(IP, out address);
                if (rtl == false)
                    throw new Exception(string.Format("输入的IP地址：{0} 格式不正确！！！", IP));

                rtl = int.TryParse(Port, out port);
                if (rtl == false)
                    throw new Exception(string.Format("输入的端口：{0} 格式不正确！！！", port));

                IPEndPoint EndPoint = new IPEndPoint(address, port);

                client = new TcpClient(EndPoint);
                Task.Run(() => TcpRecessive());

                isConnect = true;

                return "";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string Disconnect()
        {
            try
            {
                if (client == null) return "";
                if (client.Connected == true)
                    client.Close();
                isConnect = false;
                return "";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string SendMsg(string str)
        {
            try
            {
                NetworkStream ns = client.GetStream();
                bw = new BinaryWriter(ns);
                bw.Write(str);
                return "";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string ReadMsg(string str,int timeout=5)
        {
            try
            {
                SendMsg(str);
                DateTime st = DateTime.Now;
                string msg = "";
                while (true)
                {
                    if (ReceiveMsg.Length > 0)
                    {
                        msg = ReceiveMsg;
                        ReceiveMsg = "";
                        return msg;
                    }
                    else
                    {
                        if (st.Subtract(DateTime.Now).TotalSeconds > timeout)
                        {
                            throw new Exception(string.Format("{0} 秒内未接收到响应！！！",timeout));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string ReceiveMsg;
        private void TcpRecessive()
        {
            byte[] array = new byte[256];
            try
            {
                NetworkStream ns = client.GetStream();
                br = new BinaryReader(ns);
                while (true)
                {
                    ReceiveMsg = br.ReadString();
                }
            }
            catch (Exception)
            {
            }
            this.client.Close();
        }
    }
}
