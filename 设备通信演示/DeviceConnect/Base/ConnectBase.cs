using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceConnect
{
    public interface ComConnectBase
    {
        bool isConnect { get; set; }
        
        
        bool Connect(string com,int baurdRate=115200);
        bool Disconnect();
        void SendMsg(string str);
        string ReadMsg(string str);
    }
}
