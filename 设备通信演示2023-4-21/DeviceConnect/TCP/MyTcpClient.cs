using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace DeviceConnect
{
    public class MyTcpClient
    {
        public string errmsg = "";

        bool conn = false;
        Socket clientSocket = null;

        public bool Conn { get { return conn; } }
        //public MyTcpClient(string ip, int port)
        //{
        //    clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        //    IPAddress address = IPAddress.Parse(ip.Trim());
        //    IPEndPoint endPoint = new IPEndPoint(address, port);

        //    try
        //    {
        //        clientSocket.Connect(endPoint);
        //    }
        //    catch (Exception ex)
        //    {
        //        errmsg = ex.Message;
        //        return;
        //    }

        //    conn = true;
        //}

        public void ConnectServer(string Connect_IP, int Connect_Port)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress address = IPAddress.Parse(Connect_IP.Trim());
            IPEndPoint endPoint = new IPEndPoint(address, Connect_Port);

            try
            {
                clientSocket.Connect(endPoint);
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return;
            }

            conn = true;
        }

        public void DisConnectServer()
        {
            if (conn)
            {
                clientSocket.Close();
                conn = false;
            }
        }
        public int SendBuff(byte[] buff)
        {
            if (!conn)
                return 1;

            clientSocket.Send(buff);
            return 0;
        }

        public int RecvBuff(out byte[] buff)
        {
            buff = new byte[0];

            if (!conn)
                return 1;

            List<byte> totalBuff = new List<byte>();
            int totalLen = 0;
            do
            {
                byte[] RxBuff = new byte[1024];
                totalLen += clientSocket.Receive(RxBuff);
                totalBuff.AddRange(RxBuff);

            } while (0 != clientSocket.Available);

            totalBuff.RemoveRange(totalLen, totalBuff.Count - totalLen);
            buff = totalBuff.ToArray();

            return 0;
        }
    }
}
