using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DeviceConnect;

namespace Demons
{
    public partial class UCDemon4 : UserControl
    {
        HTOven myHT = new HTOven();
        TT5166_TCP myTT = new TT5166_TCP();
        public UCDemon4()
        {
            InitializeComponent();
        }

        private void UCDemon3_Load(object sender, EventArgs e)
        {
            ConvertSta(OvenType.HT, false);
            ConvertSta(OvenType.TT, false);
        }

        private void btDemo1_Click(object sender, EventArgs e)
        {
            DDMVal val = new DDMVal();
            bool rtl;
            val.Runsta = true;
            rtl = myHT.SetTemperature(148);
            rtl = myHT.GetCurTemperature(ref val.Temperature);
            
            double set = 0;
            rtl = myHT.GetSetTemperature(ref set);
        }

        private void btDemo2_Click(object sender, EventArgs e)
        {

        }

        private void btDemo3_Click(object sender, EventArgs e)
        {

        }

        private void btDemo4_Click(object sender, EventArgs e)
        {

        }

        private void btDemo5_Click(object sender, EventArgs e)
        {

        }

        #region 台通烤箱
        private void btTTCon_Click(object sender, EventArgs e)
        {
            bool rtl = myTT.Connect(tbIP.Text, tbPort.Text);
            if (rtl == true)
            {
                ConvertSta(OvenType.TT, true);

            }
            else
            {
                MessageBox.Show(myTT.errorMsg);
            }
        }

        private void btTTClose_Click(object sender, EventArgs e)
        {
            bool rtl = myTT.Disconnect();
            ConvertSta(OvenType.TT, false);
        }

        private async void btTTStart_Click(object sender, EventArgs e)
        {
            btTTStart.Enabled = false;
            await Task.Run(() => TTStartTask());
            btTTStart.Enabled = true;
        }

        public void TTStartTask()
        {
            try
            {
                if (myTT.isConnect == false) return;
                UInt16 Group;
                Group = Convert.ToUInt16(tbGroup.Text);
                bool rtl;
                //检测台通烤箱是否处于开启状态
                bool sta = true;
                rtl = myTT.GetRunSta(ref sta);
                if (rtl == false) throw new Exception(myTT.errorMsg);
                if (sta == true) throw new Exception(string.Format("台通烤箱处于开启状态，请选择其他烤箱！！！"));
                //获取当前烤箱使用程式组
                int group = 0;
                rtl = myTT.GetCurGroup(ref group);
                if (rtl == false) throw new Exception(myTT.errorMsg);
                //选用指定程式组并开启烤箱
                if (group != Group)
                {
                    System.Threading.Thread.Sleep(200);
                    myTT.SetWorkSta(Mode.Program);
                    System.Threading.Thread.Sleep(400);
                    myTT.ChoiseRunGroup(Group);
                    System.Threading.Thread.Sleep(200);
                    myTT.LoadRunGroup(Group);
                    System.Threading.Thread.Sleep(15000);
                    myTT.SetOvenOutSta(Oven.ON);
                    System.Threading.Thread.Sleep(200);
                }
                else
                {
                    System.Threading.Thread.Sleep(200);
                    myTT.SetWorkSta(Mode.Program);
                    System.Threading.Thread.Sleep(400);
                    myTT.ChoiseRunGroup(Group);
                    System.Threading.Thread.Sleep(200);
                    myTT.SetOvenOutSta(Oven.ON);
                    System.Threading.Thread.Sleep(200);
                }
                //开启后检测烤箱状态是否开启
                System.Threading.Thread.Sleep(2000);
                int offCount = 0;
                int errorCount = 0;
                while (true)
                {
                    rtl = myTT.GetRunSta(ref sta);
                    if (rtl == false)
                    {
                        if (sta == false)
                        {
                            offCount++;
                            if (offCount >= 10) throw new Exception("程控开启烤箱后，连续10次检测到烤箱未正常开启！");
                        }
                        else break;
                    }
                    else
                    {
                        errorCount++;
                        if (errorCount >= 10) throw new Exception("程控开启烤箱后，连续10次未能获取烤箱状态！");
                    }
                    System.Threading.Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btTTStop_Click(object sender, EventArgs e)
        {
            btTTStart.Enabled = false;
            await Task.Run(() => TTStopTask());
            btTTStart.Enabled = true;                        
        }

        private void TTStopTask()
        {
            if (myTT.isConnect == false) return;
            UInt16 Group;
            Group = Convert.ToUInt16(tbGroup.Text);
        }

        private void btSetTempProtect_Click(object sender, EventArgs e)
        {
            if (myTT.isConnect == false) return;
            Int16 temp;
            bool rtl;
            temp = Convert.ToInt16(tbTempProtect.Text);
            rtl = myTT.SetTempProtect(temp);
            if (rtl == false) MessageBox.Show(myHT.errorMsg);
        }

        private async void timerTT_Tick(object sender, EventArgs e)
        {
            if (myTT.isConnect == false) return;
            if (cbTTDDM.Checked == false) return;
            timerTT.Stop();
            DDMVal val = await Task.Run(() => GetTTDDMVal());
            btTTSta.Text = val.Runsta ? "OPEN" : "CLOSE";
            tbTTTemperature.Text = val.Temperature.ToString();
            timerTT.Start();
        }


        public DDMVal GetTTDDMVal()
        {
            DDMVal val = new DDMVal();
            bool rtl;
            rtl = myTT.GetRunSta(ref val.Runsta);
            rtl = myTT.GetCurTemperature(ref val.Temperature);
            return val;
        }
        #endregion

        #region 海拓烤箱
        private void btSearchPort_Click(object sender, EventArgs e)
        {
            GetPorts();
        }

        public void GetPorts()
        {
            cbComPort.Items.Clear();
            string[] coms = System.IO.Ports.SerialPort.GetPortNames();
            cbComPort.Items.AddRange(coms);
        }

        private void btConPort_Click(object sender, EventArgs e)
        {
            bool rtl;
            rtl = myHT.Connect(cbComPort.Text);
            if (rtl == true)
            {
                ConvertSta(OvenType.HT, true);

            }
            else
            {
                MessageBox.Show(myHT.errorMsg);
            }
        }

        private void btClosePort_Click(object sender, EventArgs e)
        {
            bool rtl;
            rtl = myHT.Disconnect();
            ConvertSta(OvenType.HT, false);
        }

        private void btHTStart_Click(object sender, EventArgs e)
        {
            if (myHT.isConnect == false) return;
            UInt16 temp;
            bool rtl;
            temp = Convert.ToUInt16(tbHTTemp.Text);
            rtl= myHT.SetTemperature(temp);
            if (rtl == false) MessageBox.Show(myHT.errorMsg);
        }

        private void btHTStop_Click(object sender, EventArgs e)
        {
            if (myHT.isConnect == false) return;
            bool rtl;
            rtl = myHT.SetTemperature(25);
            if (rtl == false) MessageBox.Show(myHT.errorMsg);
        }

        


        private async void timerHT_Tick(object sender, EventArgs e)
        {
            if (myHT.isConnect == false) return;
            if (cbHTDDM.Checked == false) return;
            timerHT.Stop();
            DDMVal val = await Task.Run(() => GetHTDDMVal());
            btHTSta.Text = val.Runsta ? "OPEN" : "CLOSE";
            tbHTTemperature.Text = val.Temperature.ToString();
            timerHT.Start();
        }

        public DDMVal GetHTDDMVal()
        {
            DDMVal val = new DDMVal();
            bool rtl;
            val.Runsta=true;
            rtl = myHT.GetCurTemperature(ref val.Temperature);
            return val;
        }
        #endregion

        #region


        #endregion

        public void ConvertSta(OvenType ot, bool sta)
        {
            if (ot == OvenType.HT)
            {
                cbComPort.Enabled = !sta;
                btSearchPort.Enabled = !sta;
                btConPort.Enabled = !sta;
                btClosePort.Enabled = sta;
            }
            else if (ot == OvenType.TT)
            {
                tbIP.Enabled = !sta;
                tbPort.Enabled = !sta;
                btTTCon.Enabled = !sta;
                btTTClose.Enabled = sta;
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        
    }

    public class DDMVal
    {


        public bool Runsta;

        public double Temperature;


    }

    public enum OvenType
    {
        TT,
        HT
    }
}
