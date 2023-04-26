using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceConnect
{
    public class TCPIPDriverTemp
    {
        #region 声明、属性、字段
        LAN_VISA32 tcpip;

        public string errorMsg;

        public bool isConnect;

        string address;

        #endregion

        #region 构造构析及基本函数
        public TCPIPDriverTemp()//构造函数
        {

        }

        ~TCPIPDriverTemp()//构析函数
        {
            try
            {
                if (isConnect == true) tcpip.Close();
            }
            catch { }
        }

        public bool Connect(string address)
        {
            try
            {
                this.address = address;
                tcpip = new LAN_VISA32(address);
                tcpip.Open();
                isConnect = true;
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0}连接异常\r\n{1}", address, ex.Message);
                isConnect = false;
                return false;
            }
        }

        public bool Disconnect()
        {
            try
            {
                tcpip.Close();
                isConnect = false;
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0}断开异常\r\n{1}", address, ex.Message);
                isConnect = false;
                return false;
            }
        }

        object portLock = new object();//通道锁，同一时间只能执行一次读或写
        public void SendMsg(string str)
        {
            lock (portLock)
            {
                if (isConnect == false)
                    throw new Exception(string.Format("地址：{0} 已断开", address));
                tcpip.WriteLine(str);
            }
        }

        public string ReadMsg(string str)
        {
            lock (portLock)
            {
                if (isConnect == false)
                    throw new Exception(string.Format("地址：{0} 已断开", address));
                tcpip.WriteLine(str);
                return tcpip.ReadLine();
            }
        }
        #endregion

        #region 功能函数
        public bool SetConfig(string id)
        {
            try
            {
                string deviceName = ReadMsg("*IDN?");
                if (deviceName.Contains(id) == false) throw new Exception(string.Format("返回ID：{0} 不包含：{1}", deviceName, id));
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0} 函数：SetConfig()\r\n{1}", address, ex.Message);
                return false;
            }
        }
        /*指令控制*/
        //1、只发类型
        public bool Instruction_Send(string str)
        {
            try
            {
                SendMsg(str);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0} 函数：Instruction_Send()\r\n{1}", address, ex.Message);
                return false;
            }
        }

        //2、回读类型
        public bool Instruction_Read(string str, ref string read)
        {
            try
            {
                read = ReadMsg(str);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0} 函数：Instruction_Read()\r\n{1}", address, ex.Message);
                return false;
            }
        }
        #endregion
    }
}
