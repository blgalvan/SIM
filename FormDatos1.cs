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
    public partial class FormDatos1 : Form
    {
        public string TituloDelFormulario;

        public string Rotulo_Parametro1;
        public string Rotulo_Parametro2;
        public string Rotulo_Parametro3;
        public string Rotulo_Parametro4;
        public string Rotulo_Parametro5;
        public string Rotulo_Parametro6;

        public double parametro1 = -9999999;
        public double parametro2 = -9999999;
        public double parametro3 = -9999999;
        public double parametro4 = -9999999;
        public double parametro5 = -9999999;
        public double parametro6 = -9999999;


        public string TipoDeDistribucion;
        
        public FormDatos1()
        {
            InitializeComponent();
            //Aseguramos que utiliza la configuración española para numeros decimales
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-ES");          
        }
     
        private void button1_Click(object sender, EventArgs e)
        {
            if ((Rotulo_Parametro1 != "") & (textBox1.Text != "")) parametro1 = Convert.ToDouble(textBox1.Text);
            if ((Rotulo_Parametro2 != "") & (textBox2.Text != "")) parametro2 = Convert.ToDouble(textBox2.Text);
            if ((Rotulo_Parametro3 != "") & (textBox3.Text != "")) parametro3 = Convert.ToDouble(textBox3.Text);
            if ((Rotulo_Parametro4 != "") & (textBox4.Text != "")) parametro4 = Convert.ToDouble(textBox4.Text);
            if ((Rotulo_Parametro5 != "") & (textBox5.Text != "")) parametro5 = Convert.ToDouble(textBox5.Text);
            if ((Rotulo_Parametro6 != "") & (textBox6.Text != "")) parametro6 = Convert.ToDouble(textBox6.Text);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FormDatos1_Load(object sender, EventArgs e)
        {
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            textBox4.Visible = false;
            textBox5.Visible = false;
            textBox6.Visible = false;

            if (TituloDelFormulario != "")
            {
                label6.Visible = true;
                label6.Text = TituloDelFormulario;
            }

            if (Rotulo_Parametro1 != "")
            {
                label1.Visible = true;
                label1.Text = Rotulo_Parametro1;
                textBox1.Visible = true;
                textBox1.Text = "";
            }

            if (Rotulo_Parametro2 != "")
            {
                label2.Visible = true;
                label2.Text = Rotulo_Parametro2;
                textBox2.Visible = true;
                textBox2.Text = "";
            }

            if (Rotulo_Parametro3 != "")
            {
                label3.Visible = true;
                label3.Text = Rotulo_Parametro3;
                textBox3.Visible = true;
                textBox3.Text = "";
            }

            if (Rotulo_Parametro4 != "")
            {
                label4.Visible = true;
                label4.Text = Rotulo_Parametro4;
                textBox4.Visible = true;
                textBox4.Text = "";
            }

            if (Rotulo_Parametro5 != "")
            {
                label5.Visible = true;
                label5.Text = Rotulo_Parametro5;
                textBox5.Visible = true;
                textBox5.Text = "";
            }

            if (Rotulo_Parametro6 != "")
            {
                label7.Visible = true;
                label7.Text = Rotulo_Parametro6;
                textBox6.Visible = true;
                textBox6.Text = "";
            }
        }
    }
}
