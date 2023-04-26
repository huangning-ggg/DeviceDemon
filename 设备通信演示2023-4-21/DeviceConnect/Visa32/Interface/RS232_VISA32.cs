using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace DeviceConnect
{
    public class RS232_VISA32 : PortOperatorBase, IPortType
    {
        public int BaudRate { private set; get; }

        public Parity Parity { private set; get; }

        public StopBits StopBits { private set; get; }

        public int DataBits { private set; get; }

        public PortType PortType
        {
            get { return PortType.RS232; }
        }

        public FlowControl FlowControl { set; get; } 

        public RS232_VISA32(string address, int baudRate, Parity parity, StopBits stopBits, int dataBits) : base(address)
        {
            if (!address.ToUpper().Contains("ASRL")) throw new ArgumentException("该地址不含ASRL字样");
            BaudRate = baudRate;
            Parity = parity;
            if (stopBits == StopBits.None) throw new NotSupportedException(string.Format("不支持停止位为：{0}",stopBits.ToString()));
            StopBits = stopBits;
            if (dataBits < 5 || dataBits > 8) throw new NotSupportedException(string.Format("不支持数据位为：{0}",dataBits.ToString()));
            DataBits = dataBits;
            FlowControl = FlowControl.None;
        }

        public override void Open()
        {
            base.Open();
            int result = 0;
            result = VISA32.viSetAttribute(VI, VISA32.VI_ATTR_ASRL_BAUD, BaudRate);
            PortUltility.ThrowIfResultExcepiton(result);
            switch (Parity)
            {
                case Parity.None:
                    result = VISA32.viSetAttribute(VI, VISA32.VI_ATTR_ASRL_PARITY, VISA32.VI_ASRL_PAR_NONE); break;
                case Parity.Odd:
                    result = VISA32.viSetAttribute(VI, VISA32.VI_ATTR_ASRL_PARITY, VISA32.VI_ASRL_PAR_ODD); break;
                case Parity.Even:
                    result = VISA32.viSetAttribute(VI, VISA32.VI_ATTR_ASRL_PARITY, VISA32.VI_ASRL_PAR_EVEN); break;
                case Parity.Mark:
                    result = VISA32.viSetAttribute(VI, VISA32.VI_ATTR_ASRL_PARITY, VISA32.VI_ASRL_PAR_MARK); break;
                case Parity.Space:
                    result = VISA32.viSetAttribute(VI, VISA32.VI_ATTR_ASRL_PARITY, VISA32.VI_ASRL_PAR_SPACE); break;
            }
            PortUltility.ThrowIfResultExcepiton(result);
            switch (StopBits)
            {
                case StopBits.One:
                    result = VISA32.viSetAttribute(VI, VISA32.VI_ATTR_ASRL_STOP_BITS, VISA32.VI_ASRL_STOP_ONE); break;
                case StopBits.OnePointFive:
                    result = VISA32.viSetAttribute(VI, VISA32.VI_ATTR_ASRL_STOP_BITS, VISA32.VI_ASRL_STOP_ONE5); break;
                case StopBits.Two:
                    result = VISA32.viSetAttribute(VI, VISA32.VI_ATTR_ASRL_STOP_BITS, VISA32.VI_ASRL_STOP_TWO); break;
            }
            PortUltility.ThrowIfResultExcepiton(result);
            result = VISA32.viSetAttribute(VI, VISA32.VI_ATTR_ASRL_DATA_BITS, DataBits);
            PortUltility.ThrowIfResultExcepiton(result);
            switch (FlowControl)
            {
                case FlowControl.None:
                    result = VISA32.viSetAttribute(VI, VISA32.VI_ATTR_ASRL_FLOW_CNTRL, VISA32.VI_ASRL_FLOW_NONE); break;
                case FlowControl.XOnXOff:
                    result = VISA32.viSetAttribute(VI, VISA32.VI_ATTR_ASRL_FLOW_CNTRL, VISA32.VI_ASRL_FLOW_XON_XOFF); break;
                case FlowControl.RtsCts:
                    result = VISA32.viSetAttribute(VI, VISA32.VI_ATTR_ASRL_FLOW_CNTRL, VISA32.VI_ASRL_FLOW_RTS_CTS); break;
                case FlowControl.DtrDst:
                    result = VISA32.viSetAttribute(VI, VISA32.VI_ATTR_ASRL_FLOW_CNTRL, VISA32.VI_ASRL_FLOW_DTR_DSR); break;
            }
            PortUltility.ThrowIfResultExcepiton(result);
        }
    }
}
