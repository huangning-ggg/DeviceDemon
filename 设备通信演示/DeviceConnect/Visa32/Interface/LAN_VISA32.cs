using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeviceConnect
{
    public class LAN_VISA32 : PortOperatorBase, IPortType
    {
        public LAN_VISA32(string address) : base(address)
        {
            if (!address.ToUpper().Contains("TCPIP"))
                throw new ArgumentException("该地址不含TCPIP字样");
        }
        public PortType PortType
        {
            get {return PortType.LAN; }
        }
    }
}
