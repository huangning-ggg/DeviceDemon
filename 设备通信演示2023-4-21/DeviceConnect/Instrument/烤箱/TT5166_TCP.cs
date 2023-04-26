using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceConnect
{
    public class TT5166_TCP
    {
        #region base
        TCPHelper tcp = new TCPHelper();
        /// <summary>错误信息提示
        /// 
        /// </summary>
        public string errorMsg;

        /// <summary>是否连接
        /// 
        /// </summary>
        public bool isConnect;

        public string IP;

        public string Port;

        /// <summary>连接
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool Connect(string ip, string port)
        {
            try
            {
                this.IP = ip;
                this.Port = port;
                tcp.ConnectServer(ip, port);
                this.isConnect = true;
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = ex.Message;
                return false;
            }
        }

        /// <summary>断开
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Disconnect()
        {
            try
            {
                if (this.isConnect == false) throw new Exception("设备未连接！");
                tcp.DisConnectServer();
                this.isConnect = false;
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = ex.Message;
                this.isConnect = false;
                return false;
            }
        }

        object tcpLock = new object();//通道锁，同一时间只能执行一次读或写
        /// <summary>只发送数据
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public void SendMsg(string msg)
        {
            lock (tcpLock)
            {
                if (this.isConnect == false) throw new Exception("设备未连接！");
                tcp.SendMsg(msg);
            }
        }

        /// <summary>发送并接收数据
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string ReadMsg(string msg)
        {
            lock (tcpLock)
            {
                string data;
                if (this.isConnect == false) throw new Exception("设备未连接！");
                tcp.SendMsg(msg);
                data = tcp.ReceiveMsg().Replace(" ", "");
                return data;
            }
        }
        #endregion

        //设置烤箱开启\关闭
        public bool SetOvenOutSta(Oven sta)
        {
            try
            {
                string Sendinfo = "";
                if (sta == Oven.ON) Sendinfo = "00000000000600050000FF00";
                else Sendinfo = "00000000000600050001FF00";
                SendMsg(Sendinfo);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("IP:{0} Port:{1} 函数：{2}\r\n{3}", IP, Port, "OvenOutSta()", ex.Message);
                return false;
            }
        }

        //设置定值温度大小，注意设定值写入之后需要设备启动，数据写入才会生效。
        public bool ConstantTemSet(Int16 Tem)
        {
            try
            {
                string Sendinfo = ((Int16)(Tem * 10)).ToString("X4");
                Sendinfo = "00000000000600060026" + Sendinfo;
                SendMsg(Sendinfo);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("IP:{0} Port:{1} 函数：{2}\r\n{3}", IP, Port, "ConstantTemSet()", ex.Message);
                return false;
            }
        }

        //设置定值湿度，注意设定值写入之后需要设备启动，数据写入才会生效。
        public bool ConstantHumidSet(Int16 Humid)
        {
            try
            {
                string Sendinfo = ((Int16)(Humid * 100)).ToString("X4");
                Sendinfo = "00000000000600060027" + Sendinfo;
                SendMsg(Sendinfo);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("IP:{0} Port:{1} 函数：{2}\r\n{3}", IP, Port, "ConstantHumidSet()", ex.Message);
                return false;
            }
        }

        //设置工作模式，定值、程控
        public bool SetWorkSta(Mode mode)
        {
            try
            {
                string Sendinfo;
                if (mode == Mode.Constant) Sendinfo = "000000000006000500040000";
                else Sendinfo = "00000000000600050004FF00";
                SendMsg(Sendinfo);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("IP:{0} Port:{1} 函数：{2}\r\n{3}", IP, Port, "ConvertMode()", ex.Message);
                return false;
            }
        }

        //设置超温保护值
        public bool SetTempProtect(Int16 temp)
        {
            try
            {
                string Sendinfo = ((Int16)(temp * 10)).ToString("X4");
                Sendinfo = "000000000006000600B2" + Sendinfo;
                SendMsg(Sendinfo);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("IP:{0} Port:{1} 函数：{2}\r\n{3}", IP, Port, "SetTempProtect()", ex.Message);
                return false;
            }
        }

        //获取剩余程控时间
        public bool GetTimeLeft(ref int left)
        {
            try
            {
                Int32 hour, minute;
                string dataStr = ReadMsg("000000000006000300060002");//读取小时H
                hour = Convert.ToInt32(dataStr.Substring(18, 4), 16);
                dataStr = ReadMsg("000000000006000300080002");//读取分钟M
                minute = Convert.ToInt32(dataStr.Substring(18, 4), 16);
                left = hour * 60 + minute;
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("IP:{0} Port:{1} 函数：{2}\r\n{3}", IP, Port, "GetTimeLeft()", ex.Message);
                return false;
            }
        }

        public bool GetCurTemperature(ref double temperature)
        {
            try
            {
                double a = 100.0;
                string Sendinfo = "000000000006000300000001";
                string Data;
                Data = ReadMsg(Sendinfo).Replace(" ", "").Substring(18, 4);
                temperature = Convert.ToInt16(Data, 16) / a;
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("IP:{0} Port:{1} 函数：{2}\r\n{3}", IP, Port, "GetTorH()", ex.Message);
                return false;
            }
        }

        //获取定值温湿度，设置温湿度，环境温湿度
        public bool GetTorH(Get get, ref double data)
        {
            try
            {
                double a = 1;
                string Sendinfo = "";
                string Data;
                switch (get)
                {
                    case Get.ConTemSet://获取定值温度/10
                        a = 10.0;
                        Sendinfo = "000000000006000300260001";
                        break;
                    case Get.ConHumSet://获取定值湿度/100
                        a = 100.0;
                        Sendinfo = "000000000006000300270001";
                        break;
                    case Get.Humid_PV://获取环境湿度/100
                        a = 100;
                        Sendinfo = "000000000006000300030001";
                        break;
                    case Get.Humid_SV://获取设定湿度/10
                        a = 10.0;
                        Sendinfo = "000000000006000300040001";
                        break;
                    case Get.Temp1_PV://获取环境温度/100
                        a = 100.0;
                        Sendinfo = "000000000006000300000001";
                        break;
                    case Get.Temp_SV://获取设定温度/10
                        a = 10.0;
                        Sendinfo = "000000000006000300010001";
                        break;
                }
                Data = ReadMsg(Sendinfo).Replace(" ", "").Substring(18, 4);
                data = Convert.ToInt16(Data, 16) / a;
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("IP:{0} Port:{1} 函数：{2}\r\n{3}", IP, Port, "GetTorH()", ex.Message);
                return false;
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
                string Sendinfo = "000000000006000300180001";
                string data = ReadMsg(Sendinfo);
                data = data.Substring(18, 4);
                sta = (Convert.ToUInt16(data, 16) & 0x0001) == 1;
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("IP:{0} Port:{1} 函数：{2}\r\n{3}", IP, Port, "RunSta()", ex.Message);
                return false;
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
                string Sendinfo = "000000000006000300190001";
                string data = ReadMsg(Sendinfo);
                data = data.Substring(18, 4);
                sta = (Int16.Parse(data) & 0x0001) == 1;
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("IP:{0} Port:{1} 函数：{2}\r\n{3}", IP, Port, "WorkSta()", ex.Message);
                return false;
            }
        }        

        #region 选择程式组编号
        public bool GetCurGroup(ref int num)
        {
            try
            {
                string Sendinfo;
                Sendinfo = "000000000006000300210001";
                string data = ReadMsg(Sendinfo);
                data = data.Substring(18, 4);
                num = Convert.ToInt32(data);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("IP:{0} Port:{1} 函数：{2}\r\n{3}", IP, Port, "GetCurGroup()", ex.Message);
                return false;
            }
        }

        public bool ChoiseGroup(UInt16 Group)
        {
            try
            {
                string Sendinfo;
                Sendinfo = "00000000000600060050" + Group.ToString("X4");
                SendMsg(Sendinfo);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("IP:{0} Port:{1} 函数：{2}\r\n{3}", IP, Port, "ChoiseGroup()", ex.Message);
                return false;
            }
        }
        //选择编辑首页
        public bool EditFirstPage()
        {
            try
            {
                string Sendinfo = "0000000000060005001AFF00";
                SendMsg(Sendinfo);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("IP:{0} Port:{1} 函数：{2}\r\n{3}", IP, Port, "EditFirstPage()", ex.Message);
                return false;
            }
        }
        //向下翻页
        public bool PageDown()
        {
            try
            {
                string Sendinfo = "00000000000600050007FF00";
                SendMsg(Sendinfo);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("IP:{0} Port:{1} 函数：{2}\r\n{3}", IP, Port, "PageDown()", ex.Message);
                return false;
            }
        }
        //向上翻页
        public bool PageUp()
        {
            try
            {
                string Sendinfo = "00000000000600050008FF00";
                SendMsg(Sendinfo);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("IP:{0} Port:{1} 函数：{2}\r\n{3}", IP, Port, "PageUp()", ex.Message);
                return false;
            }
        }
        //设置段参数，在调用该函数之前，应选取对应页，段号在1-5之间，不能是其他的段号
        public bool SetSectionPara(String SectionNum, SectionData sectiondata)
        {
            try
            {
                UInt16 DataAddress = 44;
                switch (SectionNum)
                {
                    case "0001":
                        DataAddress += 7 * 0;
                        break;
                    case "0002":
                        DataAddress += 7 * 1;
                        break;
                    case "0003":
                        DataAddress += 7 * 2;
                        break;
                    case "0004":
                        DataAddress += 7 * 3;
                        break;
                    case "0005":
                        DataAddress += 7 * 4;
                        break;
                    //default:
                    //    sectiondata=null;
                    //    return "error input";
                }
                string Sendinfo = "0000000000060006" + DataAddress.ToString("X4") + ((Int16)(sectiondata.Temperature * 10)).ToString("X4");//温度设置
                SendMsg(Sendinfo);
                Sendinfo = "0000000000060006" + (DataAddress + 1).ToString("X4") + ((UInt16)(sectiondata.Humid * 100)).ToString("X4");//湿度设置
                SendMsg(Sendinfo);
                Sendinfo = "0000000000060006" + (DataAddress + 2).ToString("X4") + sectiondata.Hour.ToString("X4");//小时
                SendMsg(Sendinfo);
                Sendinfo = "0000000000060006" + (DataAddress + 3).ToString("X4") + sectiondata.Minute.ToString("X4");//分钟
                SendMsg(Sendinfo);
                Sendinfo = "0000000000060006" + (DataAddress + 6).ToString("X4") + sectiondata.StandByMode;//待机模式
                SendMsg(Sendinfo);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("IP:{0} Port:{1} 函数：{2}\r\n{3}", IP, Port, "SetSectionPara()", ex.Message);
                return false;
            }
        }

        //读取设置的段参数,在调用该函数之前，应选取对应页，段号在1-5之间，不能是其他的段号
        public bool ObtainSectionSet(String SectionNum, ref SectionData sectiondata)
        {
            try
            {
                UInt16 DataAddress = 44;
                switch (SectionNum)
                {
                    case "1":
                        DataAddress += 7 * 0;
                        break;
                    case "2":
                        DataAddress += 7 * 1;
                        break;
                    case "3":
                        DataAddress += 7 * 2;
                        break;
                    case "4":
                        DataAddress += 7 * 3;
                        break;
                    case "5":
                        DataAddress += 7 * 4;
                        break;
                    //default:
                    //    sectiondata=null;
                    //    return "error input";
                }
                string Sendinfo = "0000000000060003" + DataAddress.ToString("X4") + "000E";
                string Data = ReadMsg(Sendinfo).Replace(" ", "");

                sectiondata.Temperature = Convert.ToInt16(Data.Substring(0, 4), 16);
                sectiondata.Humid = Convert.ToUInt16(Data.Substring(4, 4), 16);
                sectiondata.Hour = Convert.ToUInt16(Data.Substring(8, 4), 16);
                sectiondata.Minute = Convert.ToUInt16(Data.Substring(12, 4), 16);
                sectiondata.StandByMode = Data.Substring(24, 4);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("IP:{0} Port:{1} 函数：{2}\r\n{3}", IP, Port, "ObtainSectionSet()", ex.Message);
                return false;
            }

        }
        //保存设置
        public bool SaveSet()
        {
            try
            {
                string Sendinfo = "00000000000600050009FF00";
                SendMsg(Sendinfo);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("IP:{0} Port:{1} 函数：{2}\r\n{3}", IP, Port, "SaveSet()", ex.Message);
                return false;
            }
        }
        //选择需要运行的程式组
        public bool ChoiseRunGroup(UInt16 Group)
        {
            try
            {
                string Sendinfo;
                Sendinfo = "00000000000600060021" + Group.ToString("X4");
                SendMsg(Sendinfo);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("IP:{0} Port:{1} 函数：{2}\r\n{3}", IP, Port, "ChoiseRunGroup()", ex.Message);
                return false;
            }
        }
        //加载程式组编号数据
        public bool LoadRunGroup(UInt16 Group)
        {
            try
            {
                string Sendinfo;
                Sendinfo = "00000000000600060063" + Group.ToString("X4");
                SendMsg(Sendinfo);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("IP:{0} Port:{1} 函数：{2}\r\n{3}", IP, Port, "LoadRunGroup()", ex.Message);
                return false;
            }
        }
        //运行设备
        //OvenOutSta();

        #endregion
    }
}
