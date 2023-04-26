using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Text.RegularExpressions;

namespace DeviceConnect
{
    public class TECDriver
    {
        private SerialPort MyCom = new SerialPort();    //com接口

        public bool isConnect;

        public bool isDDMOpen;

        public string errorMsg;

        public double CurTempVal;

        public double SetTempVal;

        public bool isOK;

        /// <summary>构析函数
        /// 
        /// </summary>
        ~TECDriver()
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
                    string str = MyCom.ReadLine();
                    if (str.Contains("OK") == true)
                    {
                        this.isOK = true;
                    }
                    else
                    {
                        string pattern = @"[0-9\.\-]*";
                        string PV = str.Substring(str.IndexOf("PV") + 2);
                        string SV = str.Substring(str.IndexOf("SV") + 2);
                        string val1 = Regex.Match(PV, pattern).Value;
                        string val2 = Regex.Match(SV, pattern).Value;
                        this.CurTempVal = val1 == "" ? -99 : Convert.ToDouble(val1);
                        this.SetTempVal = val2 == "" ? -99 : Convert.ToDouble(val2);
                    }
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

        public bool DDMOpen()
        {
            try
            {
                string cmd = string.Format("SO\r\n");
                MyCom.Write(cmd);
                int count = 0;
                while (true)
                {
                    System.Threading.Thread.Sleep(10);
                    if (this.isOK == true)
                    {
                        this.isDDMOpen = true;
                        this.isOK = false;
                        break;
                    }
                    else
                    {
                        count++;
                        if (count == 100) throw new Exception("未获取开启DDM响应！");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = ex.Message;
                return false;
            }
        }

        public bool DDMClose()
        {
            try
            {
                string cmd = string.Format("SC\r\n");
                MyCom.Write(cmd);
                int count = 0;
                while (true)
                {
                    System.Threading.Thread.Sleep(10);
                    if (this.isOK == true)
                    {
                        this.isDDMOpen = false;
                        this.isOK = false;
                        break;
                    }
                    else
                    {
                        count++;
                        if (count == 100) throw new Exception("未获取关闭DDM响应！");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = ex.Message;
                return false;
            }
        }

        public bool SetTemperature(double val)
        {
            try
            {
                string cmd = string.Format("S1{0}", Math.Round(val, 1));
                MyCom.Write(cmd);
                DDMOpen();
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
