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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //public static double Generador_Aleatorio_Uniforme(double minimo_admisible, double maximo_admisible, Random r)
        //{
           // double aleatorio_uniforme;
            //aleatorio_uniforme = minimo_admisible + (maximo_admisible - minimo_admisible) * r.NextDouble();
        //
        //    return aleatorio_uniforme;
        //}



        private void lección1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Form frm = new Form2();
            //frm.Show();
        }

        private void introducciónToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void disponibilidadUnElementoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new FormNumerosAleatorios();
            frm.Show();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cicloFuncionaFallaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new Form2();
            frm.Show();
        }

        private void disponibilidadConVariasLeyesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new Form5();
            frm.Show();
        }

        private void cicloFuncionaFallaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form frm = new FormFuncionaFalla();
            frm.Show();
        }

        private void regresionLinealSimpleToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Form frm = new Formajuste();
            frm.Show();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void cicloFuncionaFallaAmpliadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new FormFuncionaFallaAmpliado();
            frm.Show();
        }
    }
}
