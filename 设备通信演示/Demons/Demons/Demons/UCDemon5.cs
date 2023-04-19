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
    public partial class UCDemon5 : UserControl
    {
        public UCDemon5()
        {
            InitializeComponent();
        }

        private void UCDemon3_Load(object sender, EventArgs e)
        {

        }

        private void btDemo1_Click(object sender, EventArgs e)
        {
            try
            {
                PSS_AOS test = new PSS_AOS();
                bool bo = test.Connect("COM3");
                if (bo == false) throw new Exception(test.errorMsg);
                string str = test.CloseAll();
                if (str != "") throw new Exception(str);

                str = test.OpenAll();
                if (str != "") throw new Exception(str);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

        #region


        #endregion

        #region


        #endregion

        #region


        #endregion
    }
}
