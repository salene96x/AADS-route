using GMap.NET.WindowsForms.Markers;
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
        public List<string> points = new List<string>();
        private static int counter = 1;
        public bool resetCheck = false;
        public bool addClick = true;
        public List<GMarkerGoogle> arr = new List<GMarkerGoogle>();
        public rightPanel()
        {
            InitializeComponent();
            if (rdbLineRouteL.Checked)
            {
                this.colorCheck = "Brown";
            }
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
        public void setListBox()
        {
            listBox1.Items.Add("Point " + counter.ToString() + "= " + points[counter-1]);
            counter++;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
        }
        public void reset(bool check)
        {
            if (check)
            {
                points.Clear();
                listBox1.Items.Clear();
                counter = 0;
            }
        }

        private void btnMark_Click(object sender, EventArgs e)
        {
            addClick = true;
        }

        private void btnResetMarker_Click(object sender, EventArgs e)
        {
            resetCheck = true;
        }

        private void btnStopMarking_Click(object sender, EventArgs e)
        {
            addClick = false;
        }
    }
}
