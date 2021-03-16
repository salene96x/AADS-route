using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AADS.Views.ReportManagement
{
    public partial class main : Form
    {
        private string query;
        private string[] specificTable = new string[] {"trackHostile", "egmStatus"};
        public main(string tableName)
        {
            InitializeComponent();
            lblName.Text = tableName;
            for (int i = 0; i < specificTable.Length; i++)
            {
                if (specificTable[i].Equals("trackHostile"))
                {
                    query = "SELECT * FROM track WHERE type = Hostile";
                }
                else
                {
                    query = "SELECT status FROM engagement";
                }
            }
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
