using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace DeviceConnect
{
    /// <summary>
    /// using:
    /// using System.Net;
    /// using System.Net.Sockets;
    /// using System.Threading;
    /// </summary>
    public class TCPHelper
    {
        /// <summary>
        /// 客户端模式，连接目标服务器
        /// </summary>
        /// <param name="Connect_IP">目标ID</param>
        /// <param name="Connect_Port">目标端口</param>
        /// <returns></returns>
        public void ConnectServer(string Connect_IP, string Connect_Port)
        {
            try
            {
                int port = 0;
                IPAddress address;
                bool flag = IPAddress.TryParse(Connect_IP, out address);
                bool flag2 = int.TryParse(Connect_Port, out port);
                if (!flag)
                {
                    throw new Exception("请输入正确的IP地址：" + Connect_IP);
                }
                if (!flag2)
                {
                    throw new Exception("请输入正确的端口：" + Connect_Port);
                }
                IPEndPoint remoteEP = new IPEndPoint(address, port);
                this._tcpServerClient = new TcpClient();
                this._tcpServerClient.Connect(remoteEP);
                this._stream = this._tcpServerClient.GetStream();
                this._workType = 1;
                this._thread_SendListen = new Thread(new ThreadStart(this.TcpSendListen));
                this._thread_SendListen.Start();
            }
            catch (Exception ex)
            {
                throw new Exception("连接服务器出现错误：" + ex.Message + "\n请检查服务器端是否开启！");
            }
        }
        /// <summary>
        /// 断开与目标服务器的连接
        /// </summary>
        public void DisConnectServer()
        {
            if (this._tcpServerClient != null && this._tcpServerClient.Connected)
            {
                this._tcpServerClient.Close();
            }
            this._workType = -1;
        }
        /// <summary>
        /// 发送消息到服务器端
        /// </summary>
        /// <param name="SendMsg">发送内容</param>
        public void SendMsg(string SendMsg)
        {
            try
            {
                byte[] sendBytes = GetSendBytes(SendMsg);
                //if (this._workType == 0)
                //{
                //    NetworkStream stream = this._tcpClient.GetStream();
                //    stream.Write(sendBytes, 0, sendBytes.Length);
                //    return;
                //}
                this._stream.Write(sendBytes, 0, sendBytes.Length);
            }
            catch (Exception ex)
            {
                throw new Exception("发送消息出错:" + ex.Message);
            }
        }
        /// <summary>
        /// 接收服务器端消息
        /// </summary>
        /// <param name="Timeout">超时时间(单位：秒)</param>
        /// <returns></returns>
        public string ReceiveMsg(int Timeout = 5)
        {
            DateTime outTime = DateTime.Now;
            try
            {
                while (true)
                {
                    if (sReceiveBuilder.Length > 0) //接收到字节
                    {
                        //string sReceiveMsg = Convert.ToString(sReceiveBuilder);
                        string sReceiveMsg = this.StringToHexString(sReceiveBuilder.ToString());
                        sReceiveBuilder.Clear();//清除接收缓存
                        return sReceiveMsg;
                    }
                    DateTime currTime = DateTime.Now;
                    if (DateDiff(DateInterval.Second, outTime, currTime) > Timeout) //时间限制，超时强制退出循环
                    {
                        throw new Exception("接收消息出现超时!未在限制时间:" + Timeout + "秒内接收到服务器端的消息，请检查服务器端运行是否正常!");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("接收消息出错:" + ex.Message);
            }
        }
        /******************内部私有变量和函数*******************/
        private TcpClient _tcpServerClient;
        private int _workType = -1;
        private NetworkStream _stream;
        private Thread _thread_SendListen;
        //private TcpClient _tcpClient;
        StringBuilder sReceiveBuilder = new StringBuilder(); //接收缓存
        private byte[] GetSendBytes(string SendMsg, bool SendHex = true)
        {
            byte[] result;
            if (!SendHex)
            {
                result = Encoding.Default.GetBytes(SendMsg);
            }
            else
            {
                result = this.HexStringToHexByte(SendMsg);
            }
            return result;
        }
        private void TcpSendListen()
        {
            bool ReceiveHex = false;
            byte[] array = new byte[256];
            try
            {
                int num;
                while ((num = this._stream.Read(array, 0, array.Length)) != 0)
                {
                    byte[] array2 = new byte[num];
                    for (int i = 0; i < num; i++)
                    {
                        array2[i] = array[i];
                    }
                    if (ReceiveHex)
                    {
                        sReceiveBuilder.Append(BitConverter.ToString(array2));
                        sReceiveBuilder.Replace("-", " ");
                    }
                    else
                    {
                        sReceiveBuilder.Append(Encoding.Default.GetString(array2));
                    }
                }
            }
            catch (Exception)
            {
            }
            this._tcpServerClient.Close();
        }
        private enum DateInterval
        {
            Second, Minute, Hour, Day, Week, Month, Quarter, Year
        }

        #region 转换
        private long DateDiff(DateInterval Interval, System.DateTime StartDate, System.DateTime EndDate)
        {
            long lngDateDiffValue = 0;
            System.TimeSpan TS = new System.TimeSpan(EndDate.Ticks - StartDate.Ticks);
            switch (Interval)
            {
                case DateInterval.Second:
                    lngDateDiffValue = (long)TS.TotalSeconds;
                    break;
                case DateInterval.Minute:
                    lngDateDiffValue = (long)TS.TotalMinutes;
                    break;
                case DateInterval.Hour:
                    lngDateDiffValue = (long)TS.TotalHours;
                    break;
                case DateInterval.Day:
                    lngDateDiffValue = (long)TS.Days;
                    break;
                case DateInterval.Week:
                    lngDateDiffValue = (long)(TS.Days / 7);
                    break;
                case DateInterval.Month:
                    lngDateDiffValue = (long)(TS.Days / 30);
                    break;
                case DateInterval.Quarter:
                    lngDateDiffValue = (long)((TS.Days / 30) / 3);
                    break;
                case DateInterval.Year:
                    lngDateDiffValue = (long)(TS.Days / 365);
                    break;
            }
            return (lngDateDiffValue);
        }
        /// <summary>字符串转换为16进制字符
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public string StringToHexString(string s)
        {
            Encoding encode = UTF8Encoding.Default;
            byte[] b = encode.GetBytes(s);//按照指定编码将string编程字节数组
            string result = string.Empty;
            for (int i = 0; i < b.Length; i++)//逐字节变为16进制字符
            {
                result += b[i].ToString("X2").ToUpper() + " ";
            }
            return result;
        }
        /// <summary>16进制字符转换为字符串
        /// 
        /// </summary>
        /// <param name="hs"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public string HexStringToString(string hs)
        {
            Encoding encode = UTF8Encoding.Default;
            string str = hs.Replace(" ", "");
            string strTemp = "";
            byte[] b = new byte[str.Length / 2];
            for (int i = 0; i < str.Length / 2; i++)
            {
                strTemp = str.Substring(i * 2, 2);
                b[i] = Convert.ToByte(strTemp, 16);
            }
            //按照指定编码将字节数组变为字符串
            return encode.GetString(b);
        }
        /// <summary>16进制字符转换为数组
        /// 
        /// </summary>
        /// <param name="hs"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public byte[] HexStringToHexByte(string hs)
        {
            string str = hs.Replace(" ", "");
            string strTemp = "";
            byte[] b = new byte[str.Length / 2];
            for (int i = 0; i < str.Length / 2; i++)
            {
                strTemp = str.Substring(i * 2, 2);
                b[i] = Convert.ToByte(strTemp, 16);
            }
            //按照指定编码将字节数组变为字符串
            return b;
        }
        #endregion
    }
}
