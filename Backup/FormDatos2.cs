using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIM
{
    public partial class FormDatos2 : Form
    {
        public FormDatos2()
        {
            InitializeComponent();
            label1.Text = "ZZZ";
        }

        /*private void FormDatos2_Shown(object sender, EventArgs e)
        {
            label1.Text = "VVV";
        
        }*/




        private void FormDatos2_Load(object sender, EventArgs e)
        {
            label1.Text = "VVV";
        }
    }
}
