using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceConnect
{
    public enum PortType
    {
        RS232, USB, GPIB, LAN, None
    }

    public class PortUltility
    {
        private static string ToStringFromPortType(PortType portType)
        {
            switch (portType)
            {
                case PortType.USB: return "USB";
                case PortType.GPIB: return "GPIB";
                case PortType.LAN: return "TCPIP";
                case PortType.None: return "";
                case PortType.RS232:
                default: return "ASRL";
            }
        }

        public static string[] FindAddresses(PortType portType)
        {
            int sesn = 0;
            int retCount = 0;
            int vi = 0;
            List<string> list = new List<string>();
            int result = VISA32.viOpenDefaultRM(out sesn);
            StringBuilder desc = new StringBuilder();
            result = VISA32.viFindRsrc(sesn, string.Format("{0}?*INSTR",ToStringFromPortType(portType)), out vi, out retCount, desc);
            ThrowIfResultExcepiton(result);
            for (int i = 0; i < retCount; i++)
            {
                list.Add(desc.ToString());
                if (i != retCount - 1)
                {
                    result = VISA32.viFindNext(vi, desc);
                    ThrowIfResultExcepiton(result);
                }
            }
            return list.ToArray();
        }

        public static string[] FindAddresses()
        {
            return FindAddresses(PortType.None);
        }

        public static void ThrowIfResultExcepiton(int result)
        {
            if (result != 0 && result != VISA32.VI_ERROR_RSRC_NFOUND)
                throw new ResultException(string.Format("无效的结果编号：{0}", result));
        }
    }

    class ResultException : Exception
    {
        public ResultException(string message) : base(message) { }
    }
}
