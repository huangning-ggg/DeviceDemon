using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace DeviceConnect
{
    public class TCPSocketHelper
    {
        /// <summary>主机断开连接时，将该值置位。
        /// 
        /// </summary>
        public bool isBreak = false;
        Socket socket;
        public string ConnectServer(string Connect_IP, int Connect_Port)
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.SendTimeout = 3000;
                socket.ReceiveTimeout = 3000;
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 3000);
                socket.Connect(new IPEndPoint(IPAddress.Parse(Connect_IP), Connect_Port));
                isBreak = false;
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string DisConnectServer()
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string SendMsg(byte[] cmd)
        {
            try
            {
                socket.Send(cmd);
                return "";
            }
            catch (SocketException ex)
            {
                isBreak = true;
                return ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string ReceiveMsg(ref byte[] readBuf)
        {
            try
            {
                readBuf = new byte[0];
                List<byte> totalBuff = new List<byte>();
                int totalLen = 0;
                do
                {
                    byte[] RxBuff = new byte[1024];
                    totalLen += socket.Receive(RxBuff);
                    totalBuff.AddRange(RxBuff);

                } while (0 != socket.Available);

                totalBuff.RemoveRange(totalLen, totalBuff.Count - totalLen);
                readBuf = totalBuff.ToArray();
                return "";
            }
            catch (SocketException ex)
            {
                isBreak = true;
                return ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
