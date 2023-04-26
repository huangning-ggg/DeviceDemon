using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Windows.Forms;
namespace DeviceConnect
{
    class MyUdpClient
    {
        Socket m_Sock;
        Thread t;
        public delegate void FunShowRecv(string str);
        private FunShowRecv ShowRecv;
        public MyUdpClient(FunShowRecv fun) 
        {
            ShowRecv = fun;

            m_Sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            m_Sock.Bind(new IPEndPoint(IPAddress.Any, 0));//绑定端口号和IP

            m_Sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);

            t = new Thread(ReciveMsg);//开启接收消息线程
            t.Start();
        }

        
        /// <summary>
        /// 向特定ip的主机的端口发送数据报
        /// </summary>
        public void SendMsg()
        {
            string msg = "HiOpTest!";
            EndPoint point;
#if false
            //point = new IPEndPoint(IPAddress.Broadcast, 8);
            //point = new IPEndPoint(IPAddress.Parse("192.168.4.255"), 8);
            point = new IPEndPoint(IPAddress.Parse("192.168.4.133"), 8);
            m_Sock.SendTo(Encoding.UTF8.GetBytes(msg), point);
            //point = new IPEndPoint(IPAddress.Parse("192.168.1.142"), 912);
            point = new IPEndPoint(IPAddress.Broadcast, 912);
            m_Sock.SendTo(Encoding.UTF8.GetBytes(msg), point);
#else
            string ip;
            int a, b;
            
            for (a = 2; a < 255; a++)
            {
                for (b = 0; b < 255; b++)
                {
                    ip = "192.168." + a.ToString() + "." + b.ToString();
                    point = new IPEndPoint(IPAddress.Parse(ip), 8);                    
                    m_Sock.SendTo(Encoding.UTF8.GetBytes(msg), point);
                }
            }

#endif
        }
        /// <summary>
        /// 接收发送给本机ip对应端口号的数据报
        /// </summary>
        void ReciveMsg()
        {
            while (true)
            {
                EndPoint point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号
                byte[] buffer = new byte[1024];
                int length = m_Sock.ReceiveFrom(buffer, ref point);//接收数据报
                string message = Encoding.UTF8.GetString(buffer, 0, length);

                //ShowRecv?.Invoke(message);
            }
        }
    }
}
