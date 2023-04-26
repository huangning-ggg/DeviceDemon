using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace DeviceConnect
{
    public class TT5166_Port
    {
        #region 声明、属性、字段
        SerialPortHelper sp = new SerialPortHelper();

        public string errorMsg;

        public bool isConnect { set; get; }

        string address;

        int BaurdRate;
        #endregion

        #region 构造构析及基本函数
        public bool Connect(string com, int baurdRate = 38400)
        {
            try
            {
                sp.parity = System.IO.Ports.Parity.Even;
                sp.stopBits = System.IO.Ports.StopBits.One;
                sp.dataBits = 8;
                this.address = com;
                this.BaurdRate = baurdRate;

                if (sp.ComConnect(com, baurdRate) == false)
                {
                    this.errorMsg = string.Format("地址：{0}（波特率：{1}）连接异常\r\n{2}", address, BaurdRate, sp.errorMsgBase);
                    isConnect = false;
                    return false;
                }
                else
                {
                    isConnect = true;
                    //string deviceName = ReadMsg("*IDN?\r\n");
                    //if (deviceName.Contains("xxx") == false)
                    //{
                    //    Disconnect();
                    //    return false;
                    //}
                    //else
                    //{
                    //    isConnect = true;
                    //    return true;
                    //}
                    return true;
                }
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("地址：{0}（波特率：{1}）连接异常\r\n{2}", address, BaurdRate, ex.Message);
                Disconnect();
                return false;
            }
        }

        public bool Disconnect()
        {
            if (sp.ComClose() == false)
            {
                this.errorMsg = string.Format("地址：{0}（波特率：{1}）断开异常\r\n{2}", address, BaurdRate, sp.errorMsgBase);
                isConnect = false;
                return false;
            }
            else { isConnect = false; return true; }
        }

        object portLock = new object();//通道锁，同一时间只能执行一次读或写
        public void SendMsg(string str)
        {
            lock (portLock)
            {
                if (isConnect == false)
                    throw new Exception(string.Format("地址：{0} 已断开", address));
                sp.ComWriteString(str);
            }
        }

        public string ReadMsg(string str)
        {
            lock (portLock)
            {
                if (isConnect == false)
                    throw new Exception(string.Format("地址：{0} 已断开", address));
                return sp.ComReadString(str).Replace(" ","");
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

        /* 发送接收指令
         * 字符串发送读取        
         * base.ComWriteString(cmd)
         * string recessive= base.ComReadString(cmd)
         * 数组发送读取
         * base.ComWriteHex(cmd)
         * byte[] recessive= base.ComReadHex(cmd)
         */
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

        //设置烤箱开启\关闭
        public bool SetOvenOutSta(Oven sta)
        {
            try
            {
                string Sendinfo = "";
                if (sta == Oven.ON) Sendinfo = "00050000FF008DEB";
                else Sendinfo = "00050001FF00DC2B";
                SendMsg(Sendinfo);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("OvenOutSta()\r\n{0}", ex.Message);
                return false;
            }
            finally
            {

            }
        }

        //设置工作模式，定值、程控
        public bool SetWorkSta(Mode mode)
        {
            try
            {
                string Sendinfo = "";
                if (mode == Mode.Constant) Sendinfo = "0005000400008DDA";
                else Sendinfo = "00050004FF00CC2A";
                SendMsg(Sendinfo);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("ConvertMode()\r\n{0}", ex.Message);
                return false;
            }
            finally
            {

            }
        }

        //获取剩余程控时间
        public bool GetTimeLeft(ref int left)
        {
            try
            {
                Int32 hour, minute;
                string dataStr = ReadMsg("00030006000225DB");//读取小时H
                hour = Convert.ToInt32(dataStr.Substring(6, 4), 16);
                dataStr = ReadMsg("0003000800024418");//读取分钟M
                minute = Convert.ToInt32(dataStr.Substring(18, 4), 16);
                left = hour * 60 + minute;
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("GetTimeLeft()\r\n{0}", ex.Message);
                return false;
            }
            finally
            {

            }
        }

        /// <summary>获取设备运行状态
        /// sta:false停止，false运行
        /// </summary>
        /// <param name="sta"></param>
        /// <returns></returns>
        public bool GetRunSta(ref bool sta)
        {
            try
            {
                string Sendinfo = "00030018000105DC";
                string data = ReadMsg(Sendinfo);
                data = data.Substring(18, 4);
                sta = (Convert.ToUInt16(data, 16) & 0x0001) == 1;
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("GetTimeLeft()\r\n{0}", ex.Message);
                return false;
            }
            finally
            {

            }
        }

        /// <summary>获取设备工作状态
        /// sta:false定值，false程式
        /// </summary>
        /// <param name="sta"></param>
        /// <returns></returns>
        public bool GetWorkSta(ref bool sta)
        {
            try
            {
                string Sendinfo = "000300190001541C";
                string data = ReadMsg(Sendinfo);
                data = data.Substring(18, 4);
                sta = (Int16.Parse(data) & 0x0001) == 1;
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("GetTimeLeft()\r\n{0}", ex.Message);
                return false;
            }
            finally
            {

            }
        }

        //获取当前使用的程式组
        public bool GetCurGroup(ref int num)
        {
            try
            {
                string Sendinfo;
                Sendinfo = "000300210001D5D1";
                string data = ReadMsg(Sendinfo);
                data = data.Substring(18, 4);
                num = Convert.ToInt32(data);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("ChoiseRunGroup()\r\n{0}", ex.Message);
                return false;
            }
            finally
            {

            }
        }

        //选择需要运行的程式组
        public bool ChoiseRunGroup(UInt16 Group)
        {
            try
            {
                string Sendinfo;
                Sendinfo = "00060021" + Group.ToString("X4");
                Sendinfo = sp.Array2Str(sp.ModBusCRC16(sp.Str2ArrayShow(Sendinfo)));
                SendMsg(Sendinfo);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("ChoiseRunGroup()\r\n{0}", ex.Message);
                return false;
            }
            finally
            {

            }
        }
        //加载程式组编号数据
        public bool LoadRunGroup(UInt16 Group)
        {
            try
            {
                string Sendinfo;
                Sendinfo = "00060063" + Group.ToString("X4");
                Sendinfo = sp.Array2Str(sp.ModBusCRC16(sp.Str2ArrayShow(Sendinfo)));
                SendMsg(Sendinfo);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("LoadRunGroup()\r\n{0}", ex.Message);
                return false;
            }
            finally
            {

            }
        }
    }
}
