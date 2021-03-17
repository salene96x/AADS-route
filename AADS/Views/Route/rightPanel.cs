using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AADS.Views.Route
{
    public partial class rightPanel : UserControl
    {
        public string colorCheck;
        public string test;
        
        public rightPanel()
        {
            InitializeComponent();
    }
        public void setValueTextBox1(string value)
        {
            textBox1.Text = value;
        }
        public void setValueTextBox2(string value)
        {
            textBox2.Text = value;

        }
        public void setValueTextBox3(string value)
        {
            textBox3.Text = value;
        }
        public void setValueTextBox4(string value)
        {
            textBox4.Text = value;
        }
        public void setValueTextBox5(string value)
        {
            textBox5.Text = value;
        }
        private void rdbLineRoute_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbLineRouteAir.Checked)
            {
                this.colorCheck = "Red";
            }
            else if (rdbLineRouteL.Checked)
            {
                this.colorCheck = "Brown";
            }
            else
            {
                this.colorCheck = "Deepblue";
            }
        }
    }
}
