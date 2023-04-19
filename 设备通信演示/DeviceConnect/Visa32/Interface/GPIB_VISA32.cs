using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeviceConnect
{
    public class GPIB_VISA32 : PortOperatorBase, IPortType
    {
        public GPIB_VISA32(string address)
            : base(address)
        {
            if (!address.ToUpper().Contains("GPIB"))
                throw new ArgumentException("该地址不含GPIB字样");
        }
        public PortType PortType
        {
            get { return PortType.GPIB; }
        }

    }
}
