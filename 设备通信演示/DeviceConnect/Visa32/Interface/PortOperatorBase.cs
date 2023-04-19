using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeviceConnect
{
    public abstract class PortOperatorBase : IPortOperator
    {
        public string Address { private set; get; }

        public PortOperatorBase(string address)
        {
            this.Address = address;
            Timeout = 2000;
            IsPortOpen = false;
        }

        public PortOperatorBase()
        {
            // TODO: Complete member initialization
        }

        public int Timeout { set; get; }

        public event EventHandler<PortEventArgs> PortOpenning;

        public event EventHandler<PortEventArgs> PortClosing;

        protected virtual void OnPortOpenning(PortEventArgs e)
        {
            if (PortOpenning != null)
                PortOpenning.Invoke(this, e);
        }

        protected virtual void OnPortClosing(PortEventArgs e)
        {
            if (PortClosing != null)
                PortClosing.Invoke(this, e);
        }

        int result;
        int sesn;
        protected int vi;

        protected int VI { private set; get; }

        public bool IsPortOpen { private set; get; }
        public virtual void Open()
        {
            PortEventArgs e = new PortEventArgs(Address);
            OnPortOpenning(e);
            if (!e.Cancel)
            {
                result = VISA32.viOpenDefaultRM(out sesn);
                result = VISA32.viOpen(sesn, Address, 0, Timeout, out vi);
                VI = vi;
                PortUltility.ThrowIfResultExcepiton(result);
                this.IsPortOpen = true;
            }
        }

        public virtual void Close()
        {
            PortEventArgs e = new PortEventArgs(Address);
            OnPortClosing(e);
            if (!e.Cancel)
            {
                result = VISA32.viClose(vi);
                PortUltility.ThrowIfResultExcepiton(result);
                this.IsPortOpen = false;
            }
        }

        public virtual void Write(string command)
        {
            int retCount = 0;
            byte[] commandBytes = Encoding.ASCII.GetBytes(command);
            result = VISA32.viWrite(vi, commandBytes, commandBytes.Length, out  retCount);
            PortUltility.ThrowIfResultExcepiton(result);
        }

        public virtual void WriteLine(string command)
        {
            Write(string.Format("{0}\n", command));
        }

        public const int READ_BUFFER_COUNT = 1024;

        public virtual string Read()
        {
            int retCount = 0;
            byte[] resultBytes = new byte[READ_BUFFER_COUNT];
            result = VISA32.viRead(vi, resultBytes, READ_BUFFER_COUNT, out  retCount);
            PortUltility.ThrowIfResultExcepiton(result);
            return retCount != 0 ? Encoding.ASCII.GetString(resultBytes.Take(retCount).ToArray()) : null;
        }

        public virtual string ReadLine()
        {
            string result = Read();
            return result.EndsWith("\n") ? result.TrimEnd(new char[] { '\n' }) : result;
        }
    }
}
