using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceConnect
{
    public class TT5166_TCP_Socket : TCPSocketTemp
    {
        public string errorMsg;
        #region 设备驱动函数
        //设置烤箱开启\关闭
        public bool SetOvenOutSta(Oven sta)
        {
            try
            {
                byte[] Sendinfo;
                if (sta == Oven.ON) Sendinfo = TCPDataFunction.GetByteCommand_05(0x0000, true);//00000000000600050000FF00;
                else Sendinfo = TCPDataFunction.GetByteCommand_05(0x0001, true);//00000000000600050001FF00;
                Send(Sendinfo);
                //开关设备，重连网络
                base.ReConnect();
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("Address:{0} 函数：{1}\r\n{2}", base.IP, "SetOvenOutSta()", ex.Message);
                return false;
            }
        }

        //设置工作模式，定值、程控
        public bool SetWorkSta(Mode mode)
        {
            try
            {
                byte[] Sendinfo;
                if (mode == Mode.Constant) Sendinfo = TCPDataFunction.GetByteCommand_05(0x0004, false);//"000000000006000500040000";
                else Sendinfo = TCPDataFunction.GetByteCommand_05(0x0004, true);// "00000000000600050004FF00";
                Send(Sendinfo);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("Address:{0} 函数：{1}\r\n{2}", base.IP, "SetWorkSta()", ex.Message);
                return false;
            }
        }

        //设置超温保护值
        public bool SetTempProtect(UInt16 temp)
        {
            try
            {
                byte[] Sendinfo;
                byte[] wByte = BitConverter.GetBytes((UInt16)(temp * 10));
                wByte = wByte.Reverse().ToArray();
                Sendinfo = TCPDataFunction.GetByteCommand_06(0x00B2, wByte);//"000000000006000600B2" + Sendinfo;
                Send(Sendinfo);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("Address:{0} 函数：{1}\r\n{2}", base.IP, "SetTempProtect()", ex.Message);
                return false;
            }
        }

        public bool GetCurTemperature(ref double temperature)
        {
            try
            {
                double a = 100.0;
                byte[] Sendinfo = TCPDataFunction.GetByteCommand_03(0x0000, 1); //"000000000006000300000001";
                byte[] rData = new byte[0];
                Read(Sendinfo, ref rData);//Replace(" ", "").Substring(18, 4);
                byte[] temp = rData.Skip(9).Take(2).Reverse().ToArray();
                temperature = BitConverter.ToUInt16(temp, 0) / a;
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("Address:{0} 函数：{1}\r\n{2}", base.IP, "GetCurTemperature()", ex.Message);
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
                byte[] Sendinfo = TCPDataFunction.GetByteCommand_03(0x0018, 1); //"000000000006000300180001";
                byte[] rData = new byte[0];
                Read(Sendinfo, ref rData);
                byte[] temp = rData.Skip(9).Take(2).Reverse().ToArray();
                UInt16 flag = BitConverter.ToUInt16(temp, 0);
                sta = (flag & 0x0001) == 1;
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("Address:{0} 函数：{1}\r\n{2}", base.IP, "GetRunSta()", ex.Message);
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
                byte[] Sendinfo = TCPDataFunction.GetByteCommand_03(0x0019, 1); //"000000000006000300190001";
                byte[] rData = new byte[0];
                Read(Sendinfo, ref rData);
                byte[] temp = rData.Skip(9).Take(2).Reverse().ToArray();
                UInt16 flag = BitConverter.ToUInt16(temp, 0);
                sta = (flag & 0x0001) == 1;
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("Address:{0} 函数：{1}\r\n{2}", base.IP, "GetWorkSta()", ex.Message);
                return false;
            }
        }

        public bool GetCurGroup(ref int num)
        {
            try
            {
                byte[] Sendinfo = TCPDataFunction.GetByteCommand_03(0x0021, 1);
                byte[] rData = new byte[0];
                Read(Sendinfo, ref rData);
                byte[] temp = rData.Skip(9).Take(2).Reverse().ToArray();
                UInt16 flag = BitConverter.ToUInt16(temp, 0);
                num = flag;
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("Address:{0} 函数：{1}\r\n{2}", base.IP, "GetCurGroup()", ex.Message);
                return false;
            }
        }

        public bool ChoiseGroup(UInt16 Group)
        {
            try
            {
                byte[] wByte = BitConverter.GetBytes(Group);
                wByte = wByte.Reverse().ToArray();
                byte[] Sendinfo = TCPDataFunction.GetByteCommand_06(0x0050, wByte);
                Send(Sendinfo);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("Address:{0} 函数：{1}\r\n{2}", base.IP, "ChoiseGroup()", ex.Message);
                return false;
            }
        }

        //选择需要运行的程式组
        public bool ChoiseRunGroup(UInt16 Group)
        {
            try
            {
                byte[] wByte = BitConverter.GetBytes(Group);
                wByte = wByte.Reverse().ToArray();
                byte[] Sendinfo = TCPDataFunction.GetByteCommand_06(0x0021, wByte);
                Send(Sendinfo);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("Address:{0} 函数：{1}\r\n{2}", base.IP, "ChoiseRunGroup()", ex.Message);
                return false;
            }
        }
        //加载程式组编号数据
        public bool LoadRunGroup(UInt16 Group)
        {
            try
            {
                byte[] wByte = BitConverter.GetBytes(Group);
                wByte = wByte.Reverse().ToArray();
                byte[] Sendinfo = TCPDataFunction.GetByteCommand_06(0x0063, wByte);
                Send(Sendinfo);
                return true;
            }
            catch (Exception ex)
            {
                this.errorMsg = string.Format("Address:{0} 函数：{1}\r\n{2}", base.IP, "LoadRunGroup()", ex.Message);
                return false;
            }
        }



        #endregion

    }
}
