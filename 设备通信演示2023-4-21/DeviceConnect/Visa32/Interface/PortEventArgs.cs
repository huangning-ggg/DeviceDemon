using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeviceConnect
{

    public class PortEventArgs : EventArgs
    {
        public string Address { private set; get; }
        public bool Cancel { set; get; }
        public PortEventArgs(string address)
        {
            Address = address;
        }
    }
}
