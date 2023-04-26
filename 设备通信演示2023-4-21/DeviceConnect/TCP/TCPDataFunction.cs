using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceConnect
{
    public static class TCPDataFunction
    {
        #region Modbus TCP组合
        /// <summary>读取指令
        /// 
        /// </summary>
        /// <param name="rh"></param>
        /// <param name="fc"></param>
        /// <param name="sAddress"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] GetByteCommand_03(UInt16 sAddress, UInt16 length, RPHead rh = RPHead.P6)
        {
            List<byte> command = new List<byte>();
            //报文
            byte[] reportHead = new byte[6] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x06 };
            if (rh == RPHead.P6) reportHead = new byte[6] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x06 };
            command.AddRange(reportHead);

            //功能代码
            byte[] funCode = new byte[2] { 0x00, 0x03 };
            command.AddRange(funCode);
            //操作地址
            byte[] address = BitConverter.GetBytes(sAddress);
            address= address.Reverse().ToArray();
            command.AddRange(address);
            //操作长度
            byte[] leng = BitConverter.GetBytes(length);
            command.AddRange(leng.Reverse());

            return command.ToArray();
        }

        /// <summary>线圈指令
        /// 
        /// </summary>
        /// <param name="rh"></param>
        /// <param name="sAddress"></param>
        /// <returns></returns>
        public static byte[] GetByteCommand_05(UInt16 sAddress, bool sta, RPHead rh = RPHead.P6)
        {
            List<byte> command = new List<byte>();
            //报文
            byte[] reportHead = new byte[6] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x06 };
            if (rh == RPHead.P6) reportHead = new byte[6] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x06 };
            command.AddRange(reportHead);

            //功能代码
            byte[] funCode = new byte[2] { 0x00, 0x05 };
            command.AddRange(funCode);
            //操作地址
            byte[] address = BitConverter.GetBytes(sAddress);
            address = address.Reverse().ToArray();
            command.AddRange(address);
            //操作
            if (sta == true)
                command.AddRange(new byte[] { 0xFF, 0x00 });
            else
                command.AddRange(new byte[] { 0x00, 0x00 });

            return command.ToArray();
        }

        /// <summary>写入指令
        /// 
        /// </summary>
        /// <param name="rh"></param>
        /// <param name="sAddress"></param>
        /// <param name="wData"></param>
        /// <returns></returns>
        public static byte[] GetByteCommand_06(UInt16 sAddress, byte[] wData, RPHead rh = RPHead.P6)
        {
            List<byte> command = new List<byte>();
            //报文
            byte[] reportHead = new byte[6] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x06 };
            if (rh == RPHead.P6) reportHead = new byte[6] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x06 };
            command.AddRange(reportHead);

            //功能代码
            byte[] funCode = new byte[2] { 0x00, 0x06 };
            command.AddRange(funCode);
            //操作地址
            byte[] address = BitConverter.GetBytes(sAddress);
            address = address.Reverse().ToArray();
            command.AddRange(address);
            //写入
            command.AddRange(wData);

            return command.ToArray();
        }

        #endregion

        #region 转换
        /// <summary>字符串转换为16进制字符
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string StringToHexString(string s)
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
        public static string HexStringToString(string hs)
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
        public static byte[] HexStringToHexByte(string hs)
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

        public static string HexToHexString(byte[] hex)
        {
            string msg = "";
            foreach (byte byt in hex)
            {
                msg += byt.ToString("X2");
            }

            return msg;
        }
        #endregion
    }

    public enum RPHead
    {
        P6
    }
}
