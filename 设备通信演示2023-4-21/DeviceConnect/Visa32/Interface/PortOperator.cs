using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace DeviceConnect
{
    public enum FlowControl
    {
        None, XOnXOff, RtsCts, DtrDst
    }

    public interface IPortOperator
    {
        void Open();
        void Close();
        void Write(string command);
        string Read();
    }

    public interface IPortType
    {
        PortType PortType { get; }
    }
}

