using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceConnect
{
    public class DAQM_4203 : PortDriverTemp
    {
        //Connect(baurd=9600)
        public string GetAllCHVal(ref double[] Temperatures)
        {
            try
            {
                Temperatures = new double[8];
                byte[] send = new byte[] { 0x01, 0x03, 0x00, 0x00, 0x00, 0x08 };
                byte[] crc = PortFunction.ModBusCRC16(send);
                send = send.Concat(crc).ToArray();
                byte[] read = ReadBytesDelay(send);
                if (read.Length != 21) throw new Exception("读取所有通道返回数据长度不为21 ！！！");
                for (byte ch = 0; ch < 8; ch++)
                {
                    byte[] temp = read.Skip(3 + 2 * ch).Take(2).Reverse().ToArray();
                    Temperatures[ch] = BitConverter.ToUInt16(temp, 0) / 100.0;
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>CH:0-7
        /// 
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="Temperature"></param>
        /// <returns></returns>
        public string GetCHVal(UInt16 ch, ref double Temperature)
        {
            try
            {
                byte[] send = new byte[] { 0x01, 0x03, 0x00, 0x00, 0x00, 0x01 };
                byte[] chbyte = BitConverter.GetBytes(ch);
                send[2] = chbyte[1]; send[3] = chbyte[0];
                byte[] crc = PortFunction.ModBusCRC16(send);
                send = send.Concat(crc).ToArray();
                byte[] read = ReadBytesDelay(send);
                if (read.Length != 7) throw new Exception("读取所有通道返回数据长度不为7 ！！！");
                byte[] temp = read.Skip(3).Take(2).Reverse().ToArray();
                Temperature = BitConverter.ToUInt16(temp, 0) / 100.0;
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
