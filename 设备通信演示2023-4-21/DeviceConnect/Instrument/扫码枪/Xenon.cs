using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Text.RegularExpressions;

namespace DeviceConnect
{
    public class Xenon
    {

        private SerialPort MyCom = new SerialPort();    //com接口

        public bool isConnect;

        public string errorMsg;

        private string pCode="";

        public string Code
        {
            set { ReadOnce = false; pCode = value; }
            get { ReadOnce = true; return pCode; }
        }

        /// <summary>true：已经读取了一次；false：尚未读取
        /// 
        /// </summary>
        public bool ReadOnce;

        /// <summary>构析函数
        /// 
        /// </summary>
        ~Xenon()
        {
            try
            {
                if (MyCom.IsOpen == true)
                    MyCom.Close();
            }
            catch
            {
            }
        }

        /// <summary>连接输入的串口
        /// 
        /// </summary>
        /// <param name="PortName"></param>
        /// <returns></returns>
        public bool Connect(string PortName)
        {
            MyCom.PortName = PortName;  //com口名字
            MyCom.Parity = Parity.None; //校验位0位
            MyCom.StopBits = StopBits.One;  //停止位1位
            MyCom.BaudRate = 9600;
            MyCom.DataBits = 8;
            MyCom.ReadBufferSize = 1024;
            MyCom.ReadTimeout = 3000;//单位毫秒
            MyCom.WriteTimeout = 3000;//单位毫秒
            try
            {
                MyCom.ReceivedBytesThreshold = 1;//串口输入缓冲区获得新数据后，将检查缓冲区内已有的字节数，大于等于ReceivedBytesThreshold就会触发DataReceived事件
                MyCom.DataReceived += port_DataReceived; //注册串口监听事件
                MyCom.Open();//打开串口
                this.isConnect = true;
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = ex.Message;
                this.isConnect = false;
                return false;
            }
        }

        /// <summary>关闭串口
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Disconnect()
        {
            try
            {
                if (MyCom.IsOpen)
                {
                    MyCom.Close();
                }
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

        /// <summary>接收串口数据事件
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void port_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            lock (this)
            {
                try
                {
                    string str = MyCom.ReadTo("\r");
                    Code = str;
                }
                catch (Exception ex)
                {
                    this.errorMsg = ex.Message;
                }

            }
        }

        private bool TEMP()
        {
            try
            {


                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = ex.Message;
                return false;
            }
        }
    }
}
