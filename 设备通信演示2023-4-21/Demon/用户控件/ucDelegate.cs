using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demon.用户控件
{
    public delegate string PortConnect(string port,int baurd);

    public delegate bool PortClose();

    public delegate string[] PortSerach();
}
