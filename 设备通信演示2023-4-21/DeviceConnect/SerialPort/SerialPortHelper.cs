using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace DeviceConnect
{
    /// <summary>
    /// using:
    /// using System.IO.Ports;
    /// 开始：
    ///       1、提供获取所有可用串口函数 ObtainAllPort() 返回为字符串数组。
    ///       2、连接串口 ComConnect(PortName,9600)
    /// 通信过程：
    ///       1、发送指令（）
    ///       ComWriteString(string cmd)   以字符串格式发送
    ///       2、读取指令（发送读取命令后读取数据）
    ///       ComReadString(string cmd, ref string readData)   发送字符串指令，返回字符串数据
    /// 结束：关闭串口 ComClose()
    /// </summary>
    /// <summary>以设置速率最大速度进行通信
    /// 
    /// </summary>
    public class SerialPortHelper
    {
        private SerialPort MyCom = new SerialPort();

        public string errorMsgBase { set; get; }

        public System.IO.Ports.Parity parity;//奇偶校验位

        public System.IO.Ports.StopBits stopBits;//停止位

        public int dataBits;//数据位

        public int Delay = 200;

        public string ReadTo;

        /// <summary>获取所有串口名称
        /// 
        /// </summary>
        /// <param name="Ports"></param>
        /// <returns></returns>
        public bool ObtainAllPort(ref string[] Ports)
        {
            try
            {
                Ports = SerialPort.GetPortNames();
            }
            catch (Exception e)
            {
                this.errorMsgBase = e.Message;
                return false;
            }
            return true;
        }

        /// <summary>连接输入的串口
        /// 只开放端口名称，波特率部分参数设置，如有需要根据需求添加
        /// </summary>
        /// <param name="PortName"></param>
        /// <returns></returns>
        public bool ComConnect(string PortName, int baurdrate)
        {
            MyCom.PortName = PortName;  //com口名字
            MyCom.Parity = this.parity; //校验位0位
            MyCom.StopBits = this.stopBits;  //停止位1位
            MyCom.BaudRate = baurdrate;
            MyCom.DataBits = this.dataBits;
            MyCom.ReadTimeout = 3000;//单位毫秒
            MyCom.WriteTimeout = 3000;//单位毫秒
            try
            {
                MyCom.Open();//打开串口
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsgBase = ex.Message;
                return false;
            }
        }

        /// <summary>关闭串口
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ComClose()
        {
            try
            {
                if (MyCom.IsOpen)
                {
                    MyCom.Close();
                    return true;
                }
                else throw new Exception("该串口未开启，不可关闭！");
            }
            catch (Exception ex)
            {
                this.errorMsgBase = ex.Message;
                return false;
            }
        }

        public SerialPortHelper()
        {
            ReadTo = Array2Str(new byte[] { 0x0A, 0x0D });
        }


        /// <summary>构析函数
        /// 
        /// </summary>
        ~SerialPortHelper()
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

        #region 串口字符串读写函数

        /// <summary>以字符串的形式发送数据
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public void ComWriteString(string cmd)
        {
            try
            {
                MyCom.Write(cmd);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("发送指令：{0}错误，\r\n{1}", cmd, e.Message));
            }
        }

        /// <summary>默认发送字符串指令，返回字符串数据
        /// 返回字符串结尾需要有换行符
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="readData"></param>
        /// <returns></returns>
        public string ComReadString(string cmd)
        {
            try
            {
                MyCom.Write(cmd);
                return MyCom.ReadLine();
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("发送指令：{0}错误，\r\n{1}", cmd, e.Message));
            }
        }

        /// <summary>默认发送字符串指令，返回字符串数据
        /// 返回字符串结尾需要有换行符
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="readData"></param>
        /// <returns></returns>
        public string ComReadStringTo(string cmd, string ReadTo = "\r\n")
        {
            try
            {
                MyCom.Write(cmd);
                return MyCom.ReadTo(ReadTo);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("发送指令：{0}错误，\r\n{1}", cmd, e.Message));
            }
        }

        /// <summary>默认发送字符串指令，返回字符串数据
        /// 返回字符串结尾需要有换行符
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="readData"></param>
        /// <returns></returns>
        public string ComReadStringDelay(string cmd, int delay = 200)
        {
            try
            {
                MyCom.Write(cmd);
                System.Threading.Thread.Sleep(delay);
                return MyCom.ReadExisting();
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("发送指令：{0}错误，\r\n{1}", cmd, e.Message));
            }
        }

        #endregion

        #region 串口数组读写函数

        /// <summary>以16进制数发送数据,发送的数据是数组
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public void ComWriteHex(byte[] cmd)
        {
            try
            {
                MyCom.Write(cmd, 0, cmd.Length);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("发送指令错误，\r\n{0}", e.Message));
            }
        }

        /// <summary>默认发送数组指令，返回数组数据
        /// 返回的数组中有固定的结束符，默认为0x0a,0x0d,既回车换行符。可自行定义“ReadTo”决定
        /// 数组读取没有结束符读写，使用字符串读取后转换为数组代替。
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public byte[] ComReadHex(byte[] cmd, string ReadTo = "\r\n")
        {
            try
            {
                MyCom.Write(cmd, 0, cmd.Length);
                string rtl = MyCom.ReadTo(ReadTo);
                rtl += ReadTo;
                return this.Str2Array(rtl);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("发送读取指令错误，\r\n{0}", e.Message));
            }
        }

        /// <summary>发送数据后，延时读取
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public byte[] ComReadHexDelay(byte[] cmd, int delay = 200)
        {
            try
            {
                MyCom.DiscardInBuffer();
                MyCom.Write(cmd, 0, cmd.Length);
                System.Threading.Thread.Sleep(delay);
                List<byte> RTL = new List<byte>();
                while (MyCom.BytesToRead > 0)
                {
                    byte val = (byte)MyCom.ReadByte();
                    RTL.Add(val);
                }
                return RTL.ToArray();
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("发送读取指令错误，\r\n{0}", e.Message));
            }
        }

        #endregion

        #region 交叉传收

        /// <summary>发送数组，返回字符串
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public string ReadExisting(byte[] cmd)
        {
            try
            {
                MyCom.Write(cmd, 0, cmd.Length);
                System.Threading.Thread.Sleep(Delay);
                return MyCom.ReadExisting();
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("发送读取指令错误，\r\n{0}", e.Message));
            }
        }

        #endregion

        #region 命令、接收数据转换：字符串与数组间相互转换
        //字符串转为数组       
        public byte[] Str2Array(string str)
        {
            return System.Text.Encoding.Default.GetBytes(str);
        }
        //数组转为字符串
        public string Array2Str(byte[] cmd)
        {
            return System.Text.Encoding.Default.GetString(cmd);
        }
        #endregion

        #region 字符串与16进制数组相互转换
        //将数组转换成16进制字符串显示，在需要场景将数组以16进制字符串的形式显示出来
        public string Array2StrShow(byte[] cmd)
        {
            StringBuilder ReadString = new StringBuilder();
            foreach (byte data in cmd)
            {
                ReadString.Append(data.ToString("X2").ToUpper());
            }
            return ReadString.ToString();
        }

        /// <summary>字符串每两个字符，转换为数组
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public byte[] Str2ArrayShow(string str)
        {
            int length = str.Length;
            int count = (int)Math.Ceiling(length / 2.0);
            List<byte> vals = new List<byte>();
            for (int i = 0; i < count; i++)
            {
                byte val = Convert.ToByte(str.Substring(2 * i, 2), 16);
                vals.Add(val);
            }
            return vals.ToArray();
        }
        #endregion

        #region CRC校验

        /// <summary>计算校验位
        /// 逻辑和字节累加校验
        /// </summary>
        /// <param name="dataArr"></param>
        /// <returns></returns>
        public byte getChkSum(byte[] dataArr)
        {
            int checksum = 0;
            foreach (byte b in dataArr)
                checksum += b;

            checksum = checksum & 255;
            return Convert.ToByte(checksum);
        }

        /// <summary>CRC16_Modbus校验
        /// 
        /// </summary>
        /// <param name="byteData">要进行计算的字节数组</param>
        /// <returns>计算后的数组</returns>
        public byte[] ModBusCRC16(byte[] byteData)
        {
            byte[] CRC = new byte[2];

            UInt16 wCrc = 0xFFFF;
            for (int i = 0; i < byteData.Length; i++)
            {
                wCrc ^= Convert.ToUInt16(byteData[i]);
                for (int j = 0; j < 8; j++)
                {
                    if ((wCrc & 0x0001) == 1)
                    {
                        wCrc >>= 1;
                        wCrc ^= 0xA001;//异或多项式
                    }
                    else
                    {
                        wCrc >>= 1;
                    }
                }
            }

            CRC[1] = (byte)((wCrc & 0xFF00) >> 8);//高位在后
            CRC[0] = (byte)(wCrc & 0x00FF);       //低位在前
            return CRC;

        }
        #endregion
    }

    public static class PortFunction
    {
        #region 命令、接收数据转换：字符串与数组间相互转换
        //字符串转为数组       
        public static byte[] Str2Array(string str)
        {
            return System.Text.Encoding.Default.GetBytes(str);
        }
        //数组转为字符串
        public static string Array2Str(byte[] cmd)
        {
            return System.Text.Encoding.Default.GetString(cmd);
        }
        #endregion

        #region 字符串与16进制数组相互转换
        //将数组转换成16进制字符串显示，在需要场景将数组以16进制字符串的形式显示出来
        public static string Array2StrShow(byte[] cmd)
        {
            StringBuilder ReadString = new StringBuilder();
            foreach (byte data in cmd)
            {
                ReadString.Append(data.ToString("X2").ToUpper());
            }
            return ReadString.ToString();
        }

        /// <summary>字符串每两个字符，转换为数组
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] Str2ArrayShow(string str)
        {
            int length = str.Length;
            int count = (int)Math.Ceiling(length / 2.0);
            List<byte> vals = new List<byte>();
            for (int i = 0; i < count; i++)
            {
                byte val = Convert.ToByte(str.Substring(2 * i, 2), 16);
                vals.Add(val);
            }
            return vals.ToArray();
        }
        #endregion

        #region CRC校验

        /// <summary>计算校验位
        /// 逻辑和字节累加校验
        /// </summary>
        /// <param name="dataArr"></param>
        /// <returns></returns>
        public static byte getChkSum(byte[] dataArr)
        {
            int checksum = 0;
            foreach (byte b in dataArr)
                checksum += b;

            checksum = checksum & 255;
            return Convert.ToByte(checksum);
        }

        /// <summary>CRC16_Modbus校验
        /// 
        /// </summary>
        /// <param name="byteData">要进行计算的字节数组</param>
        /// <returns>计算后的数组</returns>
        public static byte[] ModBusCRC16(byte[] byteData)
        {
            byte[] CRC = new byte[2];

            UInt16 wCrc = 0xFFFF;
            for (int i = 0; i < byteData.Length; i++)
            {
                wCrc ^= Convert.ToUInt16(byteData[i]);
                for (int j = 0; j < 8; j++)
                {
                    if ((wCrc & 0x0001) == 1)
                    {
                        wCrc >>= 1;
                        wCrc ^= 0xA001;//异或多项式
                    }
                    else
                    {
                        wCrc >>= 1;
                    }
                }
            }

            CRC[1] = (byte)((wCrc & 0xFF00) >> 8);//高位在后
            CRC[0] = (byte)(wCrc & 0x00FF);       //低位在前
            return CRC;

        }
        #endregion
    }
}
