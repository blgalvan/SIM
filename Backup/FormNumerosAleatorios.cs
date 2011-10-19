using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.DataVisualization.Charting.Utilities;
using System.Diagnostics;

namespace SIM
{
    public partial class FormNumerosAleatorios : Form
    {
        public FormNumerosAleatorios()
        {
            InitializeComponent();
            //Aseguramos que utiliza la configuración española para numeros decimales
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-ES");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random r = new Random(DateTime.Now.Millisecond);
            double aleatorio = 0;

            chart1.Series.Clear();
            chart1.ChartAreas.Clear();

            
            textBox5.Enabled = true;
            textBox5.Text = "";

            
            try
            {


            //Captura de indicaciones del usuario
            int numero_aleatorios = Convert.ToInt16(textBox10.Text);

            //Indicar el tamaño máximo de la barra de seguimiento
            progressBar1.Maximum = numero_aleatorios;
            progressBar1.Value = 0;


            //Captura de datos sobre Ley de Funcionamiento
            string Ley_Funcionamiento = comboBox1.Text;
            double parametro1 = Convert.ToDouble(textBox1.Text);
            double parametro2 = Convert.ToDouble(textBox2.Text);
            double Minimo_Admisible = Convert.ToDouble(textBox3.Text);
            double Maximo_Admisible = Convert.ToDouble(textBox4.Text);

          //Control de los valores maximos y minimos introducidos

          if(ChequeoTextboxNumeosAleatorios(comboBox1, textBox1, textBox2, textBox3, textBox4)==0)
          //lanzamos la simulación
           {
            chart1.ChartAreas.Add("ChartArea1");
            chart1.Series.Add("Datos");            
            
            for (int i = 1; i <= numero_aleatorios; i++)
            {
                //Generar los numeros aleatorios
                if (Ley_Funcionamiento == "Uniforme") aleatorio = GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme(Minimo_Admisible, Maximo_Admisible, r);
                if (Ley_Funcionamiento == "Exponencial") aleatorio = GeneradoresDeAleatorios.Generador_Aleatorio_Exponencial(parametro1, 1/parametro2, Minimo_Admisible, Maximo_Admisible, r);
                if (Ley_Funcionamiento == "Weibull") aleatorio = GeneradoresDeAleatorios.Generador_Aleatorio_Weibull_2P(parametro1, parametro2, Minimo_Admisible, Maximo_Admisible, r);
                if (Ley_Funcionamiento == "Normal") aleatorio = GeneradoresDeAleatorios.Generador_Aleatorio_Normal(parametro1, parametro2, Minimo_Admisible, Maximo_Admisible, r);

                chart1.Series["Datos"].Points.AddY(aleatorio);
                
                //Presentar los resultados numéricos en el TextBox de pantalla
                textBox5.Text += Convert.ToString(i) + "     " + Convert.ToString(aleatorio) + "\r\n";


                //Incrementar la barra indicadora
                progressBar1.Increment(1);

            }

            
            HistogramChartHelper histo = new HistogramChartHelper();
            
            // Show the percent frequency on the right Y axis.
            histo.ShowPercentOnSecondaryYAxis = true;

            // Specify number of segment intervals
            histo.SegmentIntervalNumber = 10;

            // Or you can specify the exact length of the interval
            // histogramHelper.SegmentIntervalWidth = 15;

            // Create histogram series    
            histo.CreateHistogram(chart1, "Datos", "Histogram");

           }
            }
            catch
            {
                MessageBox.Show("Se ha producido un error en los datos de entrada", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Ninguna Ley")
            {
                textBox3.Text = "";
                textBox4.Text = "";
                textBox1.Text = "";
                textBox2.Text = "";
                textBox10.Text = "";
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox10.Enabled = false;
                label2.Text = "Parámetro 1";
                label3.Text = "Parámetro 2";
            }

            if (comboBox1.Text == "Exponencial")
            {
                textBox3.Text = "";
                textBox4.Text = "";
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox10.Enabled = true;
                label2.Text = "Gamma";
                label3.Text = "Lambda";

            }

            if (comboBox1.Text == "Weibull")
            {
                textBox3.Text = "";
                textBox4.Text = "";
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox10.Enabled = true;
                label2.Text = "Beta";
                label3.Text = "Eta";
            }

            if (comboBox1.Text == "Normal")
            {
                textBox3.Text = "";
                textBox4.Text = "";
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox10.Enabled = true;
                label2.Text = "Media";
                label3.Text = "Desviación Típica";
            }

            if (comboBox1.Text == "Uniforme")
            {

                textBox3.Text = "";
                textBox4.Text = "";
                textBox1.Text = "0";
                textBox2.Text = "0";
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox10.Enabled = true;
                label2.Text = "";
                label3.Text = "";
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Borrar todo los campos de datos
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox10.Text = "";
            

            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox10.Enabled = false;
            

            comboBox1.Text = "";
            

            label2.Text = "";
            label3.Text = "";
            

            //Limpiar la gráfica
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();

            //POner a cero la barra de progreso de los calculos
            progressBar1.Value = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
            comboBox1.Text = "Weibull";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox10.Enabled = true;
            textBox1.Text = "2,1";
            textBox2.Text = "550,5";
            textBox3.Text = "100";
            textBox4.Text = "1500";
            textBox10.Text = "50";
            label2.Text = "Beta";
            label3.Text = "Eta";

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /*** Eventos para el formateo de datos ****/
        private void NumerosConComa(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;
            Debug.Assert(tb != null, "sender no es de tipo textBox");
            if (tb != null)
            {
                FormateoDatos.numOcoma(e, tb);
            }
        }
        private void NumerosSinComa(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;
            Debug.Assert(tb != null, "sender no es de tipo textBox");
            if (tb != null)
            {
                FormateoDatos.soloNum(e, tb);
            }
        }
 
        //Control de los valores maximos y minimos introducidos
        private int ChequeoTextboxNumeosAleatorios(ComboBox CBOX1,TextBox TBOX1, TextBox TBOX2, TextBox TBOX3, TextBox TBOX4)
        {
            int valor = 0;
            /******************************************************   FUNCIONAMIENTO   *******************************************************/
            switch (CBOX1.Text)
            {
                case "Exponencial":
                    {
                        if (Convert.ToDouble(TBOX1.Text) > 100000)
                        {
                            MessageBox.Show("Valor de Gamma elevado (<=500000)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if (Convert.ToDouble(TBOX2.Text) > 1)
                        {
                            MessageBox.Show("Valor de Lambda elevado (<=1)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }

                        else if ((Convert.ToDouble(TBOX3.Text) > 1 / Convert.ToDouble(TBOX2.Text)) || (Convert.ToDouble(TBOX4.Text) < 1 / Convert.ToDouble(TBOX2.Text)))
                        {
                            MessageBox.Show("Valor de Lambda erróneo: 1/Lambda ha de estar dentro del rango [Mínimo Admisible, Maximo Admisible]", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }

                        else if ((Convert.ToDouble(TBOX4.Text) - Convert.ToDouble(TBOX3.Text)) < 0)
                        {
                            MessageBox.Show("Valores maximos y minimos admisibles erroneos ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if ((Convert.ToDouble(TBOX4.Text) > 500000) || Convert.ToDouble(TBOX3.Text) > 500000)
                        {
                            MessageBox.Show("Valor maximo o minimo admisible elevado (<=500.000)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                    }

                    break;
                case "Weibull":
                    {
                        if (Convert.ToDouble(TBOX1.Text) > 10)
                        {
                            MessageBox.Show("Valor de Beta elevado (<=10)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if (Convert.ToDouble(TBOX2.Text) > 500000)
                        {
                            MessageBox.Show("Valor de Eta elevado (<=500000)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if (Convert.ToDouble(TBOX3.Text) > Convert.ToDouble(TBOX2.Text))
                        {
                            MessageBox.Show("Valor de Eta menor que el minimo admisible ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if (Convert.ToDouble(TBOX4.Text) < Convert.ToDouble(TBOX2.Text))
                        {
                            MessageBox.Show("Valor de Eta mayor que el maximo admisible ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }

                        else if ((Convert.ToDouble(TBOX4.Text) - Convert.ToDouble(TBOX3.Text)) < 0)
                        {
                            MessageBox.Show("Valores maximos y minimos admisibles erroneos ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if ((Convert.ToDouble(TBOX4.Text) > 500000) || Convert.ToDouble(TBOX3.Text) > 500000)
                        {
                            MessageBox.Show("Valor maximo o minimo admisible elevado (<=500.000)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                    }
                    break;
                case "Normal":
                    {
                        if (Convert.ToDouble(TBOX1.Text) > 500000)
                        {
                            MessageBox.Show("Valor de la Media elevado (<=500000)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if (Convert.ToDouble(TBOX2.Text) > Convert.ToDouble(Convert.ToDouble(TBOX1.Text) / 2))
                        {
                            MessageBox.Show("Valor de la desviacion tipica no puede ser mayor que Media/2", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if (Convert.ToDouble(TBOX3.Text) > Convert.ToDouble(TBOX1.Text))
                        {
                            MessageBox.Show("Valor de la Media menor que el minimo admisible ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if (Convert.ToDouble(TBOX4.Text) < Convert.ToDouble(TBOX1.Text))
                        {
                            MessageBox.Show("Valor de la Media mayor que el maximo admisible ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }

                        else if ((Convert.ToDouble(TBOX4.Text) - Convert.ToDouble(TBOX3.Text)) < 0)
                        {
                            MessageBox.Show("Valores maximos y minimos admisibles erroneos ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if ((Convert.ToDouble(TBOX4.Text) > 500000) || Convert.ToDouble(TBOX3.Text) > 500000)
                        {
                            MessageBox.Show("Valor maximo o minimo admisible elevado (<=500.000)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                    }
                    break;
                case "Uniforme":
                    {
                        if ((Convert.ToDouble(TBOX4.Text) - Convert.ToDouble(TBOX3.Text)) < 0)
                        {
                            MessageBox.Show("Valores maximos y minimos admisibles erroneos ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if ((Convert.ToDouble(TBOX4.Text) > 500000) || Convert.ToDouble(TBOX3.Text) > 500000)
                        {
                            MessageBox.Show("Valor maximo o minimo admisible elevado (<=500.000)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                    }
                    break;
            }

            return valor;
        }


    }
}
