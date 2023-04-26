using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceConnect
{
    public class DMC1000B
    {
        public string errorMsg { set; get; }

        public bool isDMC1000BOpen { set; get; }

        public bool DMC1000BOpen()
        {
            try
            {
                int nCard = Dmc1000.d1000_board_init();//控制卡初始化
                if (nCard <= 0) throw new Exception("未找到控制卡!, 警告");
                else
                {
                    this.isDMC1000BOpen = true;
                    return true;
                }
            }
            catch (Exception e)
            {
                errorMsg = string.Format("DMC1000B打开失败！\r\n{0}", e.Message);
                this.isDMC1000BOpen = false;
                return false;
            }
        }

        public bool DMC1000BClose()
        {
            try
            {
                if (this.isDMC1000BOpen == false) return true;
                int nCard = Dmc1000.d1000_board_close();//控制卡初始化
                if (nCard <= 0) throw new Exception("关闭失败！");
                else
                {
                    this.isDMC1000BOpen = false;
                    return true;
                }
            }
            catch (Exception e)
            {
                errorMsg = string.Format("DMC1000B关闭失败！\r\n{0}", e.Message);
                return false;
            }
        }

        ~DMC1000B()
        {
            try
            {
                if (isDMC1000BOpen == true) Dmc1000.d1000_board_close();
            }
            catch
            { }
        }

        /// <summary>mode:根据驱动器工作模式设定
        /// 脉冲∕方向模式(0：上升沿有效；1：下降沿有效；)
        /// 双脉冲模式（2：上升沿有效；3：下降沿有效；）
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public bool DMC1000BInit(int mode)
        {
            try
            {
                if (isDMC1000BOpen == false) throw new Exception("DMC1000B未成功连接！");
                if (mode > 4 || mode < 1) throw new Exception("DMC1000B输出脉冲模式为1、2、3、4");
                bool rtlx = Dmc1000.d1000_set_pls_outmode(0, mode) <= 0;
                bool rtly = Dmc1000.d1000_set_pls_outmode(1, mode) <= 0;
                bool rtlz = Dmc1000.d1000_set_pls_outmode(2, mode) <= 0;
                this.SetOutBit(1, true);
                this.SetOutBit(2, true);
                this.SetOutBit(3, true);
                this.SetOutBit(4, true);
                this.SetOutBit(8, false);
                string msg = rtlx ? "x轴设置脉冲输出模式失败！ " : "";
                msg += rtly ? "y轴设置脉冲输出模式失败！ " : "";
                msg += rtlz ? "z轴设置脉冲输出模式失败！" : "";
                if (msg.Length > 0) throw new Exception(msg);
                else return true;
            }
            catch (Exception e)
            {
                errorMsg = string.Format("DMC1000B设置输出脉冲模式失败！\r\n{0}", e.Message);
                return false;
            }
        }

        private object xmove = new object();

        private object ymove = new object();

        private object zmove = new object();

        /// <summary>单轴移动
        /// 
        /// </summary>
        /// <param name="distant"></param>
        /// <param name="sm"></param>
        public void move_line(int axis, int distant, MovePara mp, SpeedMode sm = SpeedMode.T)
        {
            if (sm == SpeedMode.T) Dmc1000.d1000_start_t_move(axis, distant, mp.StrVel, mp.MaxVel, mp.Tacc);
            else if (sm == SpeedMode.S) Dmc1000.d1000_start_s_move(axis, distant, mp.StrVel, mp.MaxVel, mp.Tacc);            
        }

        /// <summary>单轴绝对移动
        /// 
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="distant"></param>
        /// <param name="mp"></param>
        /// <param name="sm"></param>
        public void move_lineA(int axis, int distant, MovePara mp, SpeedMode sm = SpeedMode.T)
        {
            if (sm == SpeedMode.T) Dmc1000.d1000_start_ta_move(axis, distant, mp.StrVel, mp.MaxVel, mp.Tacc);
            else if (sm == SpeedMode.S) Dmc1000.d1000_start_sa_move(axis, distant, mp.StrVel, mp.MaxVel, mp.Tacc);
        }
        /// <summary>多轴联动相对移动
        /// 
        /// </summary>
        /// <param name="AxisArray"></param>
        /// <param name="DistArray"></param>
        /// <param name="mp"></param>
        public void MultiMoveLine(int[] AxisArray, int[] DistArray, MovePara mp)
        {
            Dmc1000.d1000_start_t_line(AxisArray.Length, AxisArray, DistArray, mp.StrVel, mp.MaxVel, mp.Tacc);
        }

        /// <summary>多轴联动绝对移动
        /// 
        /// </summary>
        /// <param name="AxisArray"></param>
        /// <param name="DistArray"></param>
        /// <param name="mp"></param>
        public void MultiMoveLineA(int[] AxisArray, int[] DistArray, MovePara mp)
        {
            Dmc1000.d1000_start_ta_line(AxisArray.Length, AxisArray, DistArray, mp.StrVel, mp.MaxVel, mp.Tacc);
        }

        public void CheckDone(int[] AxisArray)
        {
            foreach (int axis in AxisArray)
            {
                while (Dmc1000.d1000_check_done(axis) == 0) { System.Threading.Thread.Sleep(1); }
            }
        }

        //true:运动中，false:停止
        public bool CheckDone(int Axis)
        {
            return Dmc1000.d1000_check_done(Axis) == 0;
        }

        public void SetOriginalPoint(int Axis)
        {
            Dmc1000.d1000_set_command_pos(Axis, 0);
        }

        public void SetOutBit(int BitNo, bool Enable)
        {
            int BitData = Enable ? 1 : 0;
            Dmc1000.d1000_out_bit(BitNo, BitData);
        }

        public void Stop()
        {
            if (Dmc1000.d1000_check_done(0) == 0 //在运行
             || Dmc1000.d1000_check_done(1) == 0
             || Dmc1000.d1000_check_done(2) == 0
             || Dmc1000.d1000_check_done(3) == 0)
            {//当减速时间为0时，为急停效果
                Dmc1000.d1000_decel_stop(0);
                Dmc1000.d1000_decel_stop(1);
                Dmc1000.d1000_decel_stop(2);
                Dmc1000.d1000_decel_stop(3);
                return;
            }
        }

        public void Set_Pos(int axis, double pos)
        {
            Dmc1000.d1000_set_command_pos(axis, pos);
        }

        public double Get_Pos(int axis)
        {
            return Dmc1000.d1000_get_command_pos(axis);
        }

        public void MoveHome(int[] axis, MovePara mp)
        {
            foreach (int axi in axis)
            {
                Dmc1000.d1000_home_move(axi, mp.StrVel, mp.MaxVel, mp.Tacc);
            }
        }
    }
    public struct MovePara
    {
        public int StrVel;
        public int MaxVel;
        public double Tacc;
    }

    public enum SpeedMode
    {
        T, S
    }
}
