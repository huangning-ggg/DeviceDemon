using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demons
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            SwitchForm(items.demon1);
        }

        #region 界面
        UCDemon1 uc1 = new UCDemon1();
        private void btDemon1_Click(object sender, EventArgs e)
        {
            SwitchForm(items.demon1);
        }
        UCDemon2 uc2 = new UCDemon2();
        private void btDemon2_Click(object sender, EventArgs e)
        {
            SwitchForm(items.demon2);
        }
        UCDemon3 uc3 = new UCDemon3();
        private void btDemon3_Click(object sender, EventArgs e)
        {
            SwitchForm(items.demon3);
        }
        UCDemon4 uc4 = new UCDemon4();
        private void btDemon4_Click(object sender, EventArgs e)
        {
            SwitchForm(items.demon4);
        }
        UCDemon5 uc5 = new UCDemon5();
        private void btDemon5_Click(object sender, EventArgs e)
        {
            SwitchForm(items.demon5);
        }

        private void SwitchForm(items item)
        {
            UserControl uc = new UserControl();
            Point Location = new Point();
            if (item == items.demon1)
            {
                uc = uc1;
                Location = new Point { X = btDemon1.Location.X - 10, Y = btDemon1.Location.Y };
            }
            else if (item == items.demon2)
            {
                uc = uc2;
                Location = new Point { X = btDemon2.Location.X - 10, Y = btDemon2.Location.Y };
            }
            else if (item == items.demon3)
            {
                uc = uc3;
                Location = new Point { X = btDemon3.Location.X - 10, Y = btDemon3.Location.Y };
            }
            else if (item == items.demon4)
            {
                uc = uc4;
                Location = new Point { X = btDemon4.Location.X - 10, Y = btDemon4.Location.Y };
            }
            else if (item == items.demon5)
            {
                uc = uc5;
                Location = new Point { X = btDemon5.Location.X - 10, Y = btDemon5.Location.Y };
            }
            panelBar.Location = Location;
            panelMain.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            panelMain.Controls.Add(uc);

        }

        enum items
        {
            demon1,
            demon2,
            demon3,
            demon4,
            demon5
        }
        #endregion

        
    }



    public static class CTLFunc
    {
        public static void ConvertConSta(ConnectCtl cc,bool sta)
        {
            if (cc.connect != null && cc.close != null)
            {
                if (cc.connect.InvokeRequired == true)
                {
                    cc.connect.Invoke((Action)delegate
                    {
                        cc.connect.Enabled = !sta;
                        cc.close.Enabled = sta;
                        cc.send.Enabled = sta;
                        cc.read.Enabled = sta;
                    });
                }
                else
                {
                    cc.connect.Enabled = !sta;
                    cc.close.Enabled = sta;
                    cc.send.Enabled = sta;
                    cc.read.Enabled = sta;
                }
            }
        }
    }

    public class ConnectCtl
    {
        public Button connect;
        public Button close;
        public Button send;
        public Button read;
    }
}
