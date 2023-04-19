using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeviceConnect
{
    public class USB_VISA32 : PortOperatorBase, IPortType
    {
        public USB_VISA32(string address) : base(address)
        {
            if (!address.ToUpper().Contains("USB"))
                throw new ArgumentException("该地址不含USB字样");
        }
        public PortType PortType
        {
            get { return PortType.USB; }
        }
    }
}
