using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;

namespace SIM
{
    public partial class Form5 : Form
    {

        public List<PointF> Datos_a_representar;
        
        public Form5()
        {
            InitializeComponent();
            //Aseguramos que utiliza la configuración española para numeros decimales
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-ES");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox1.Text == "Ninguna Ley")
            {
                textBox3.Text = "";
                textBox4.Text = "";
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                textBox1.Enabled = false;
                textBox2.Enabled = false;
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
                label2.Text = "";
                label3.Text = "";
            }

        }


        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text == "Ninguna Ley")
            {
                textBox6.Text = "";
                textBox7.Text = "";
                textBox8.Text = "";
                textBox9.Text = "";
                textBox6.Enabled = false;
                textBox7.Enabled = false;
                textBox8.Enabled = false;
                textBox9.Enabled = false;
                label7.Text = "Parámetro 1";
                label8.Text = "Parámetro 2";
            }

            if (comboBox2.Text == "Exponencial")
            {
                textBox6.Text = "";
                textBox7.Text = "";
                textBox8.Text = "";
                textBox9.Text = "";
                textBox6.Enabled = true;
                textBox7.Enabled = true;
                textBox8.Enabled = true;
                textBox9.Enabled = true;
                label7.Text = "Gamma";
                label8.Text = "Lambda";
            }


            if (comboBox2.Text == "Weibull")
            {
                textBox6.Text = "";
                textBox7.Text = "";
                textBox8.Text = "";
                textBox9.Text = "";
                textBox6.Enabled = true;
                textBox7.Enabled = true;
                textBox8.Enabled = true;
                textBox9.Enabled = true;
                label7.Text = "Beta";
                label8.Text = "Eta";
            }

            if (comboBox2.Text == "Normal")
            {
                textBox6.Text = "";
                textBox7.Text = "";
                textBox8.Text = "";
                textBox9.Text = "";
                textBox6.Enabled = true;
                textBox7.Enabled = true;
                textBox8.Enabled = true;
                textBox9.Enabled = true;
                label7.Text = "Media";
                label8.Text = "Desviación Típica";
            }

            if (comboBox2.Text == "Uniforme")
            {
                textBox6.Text = "0";
                textBox7.Text = "0";
                textBox8.Text = "";
                textBox9.Text = "";
                textBox6.Enabled = false;
                textBox7.Enabled = false;
                textBox8.Enabled = true;
                textBox9.Enabled = true;
                label7.Text = "";
                label8.Text = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            double Disponibilidad_una_simulacion=9999999;
            Random r = new Random(DateTime.Now.Millisecond);

            textBox5.Enabled = true;
            textBox5.Text = "";
            try
            {
            //Captura de indicaciones del usuario
            double Tiempo_A_Simular = Convert.ToDouble(textBox10.Text);
            int numero_repeticiones = Convert.ToInt16(textBox11.Text);
           
            //Captura de datos sobre Ley de Funcionamiento
            string Ley_Funcionamiento = comboBox1.Text;
            double parametro1_func = Convert.ToDouble(textBox1.Text);
            double parametro2_func = Convert.ToDouble(textBox2.Text);
            double Minimo_Admisible_Funcionando = Convert.ToDouble(textBox3.Text);
            double Maximo_Admisible_Funcionando = Convert.ToDouble(textBox4.Text);

            //Captura de datos sobre Ley de Parada
            string Ley_Parada = comboBox2.Text;
            double parametro1_paro = Convert.ToDouble(textBox6.Text);
            double parametro2_paro = Convert.ToDouble(textBox7.Text);
            double Minimo_Admisible_Parado = Convert.ToDouble(textBox8.Text);
            double Maximo_Admisible_Parado = Convert.ToDouble(textBox9.Text);

            //Indicar el tamaño máximo de la barra de seguimiento
            progressBar1.Maximum = numero_repeticiones;
            progressBar1.Value = 0;

            double Disponibilidad_Media = 0;
            double Disponibilidad_Acumulada = 0;
            double Disponibilidad_Minima_Encontrada = 999999;
            double Disponibilidad_Maxima_Encontrada = -999999;
            
            
            //Comprobamos que se han introducido correctamente los valores maximos y minimos, y lanzamos la simulacion
            
                 if (ChequeoTextboxDisponibilidadVariasLeyes(comboBox1, comboBox2, textBox1, textBox2, textBox3, textBox4, textBox6, textBox7, textBox8, textBox9, textBox10) == 0)
            
            
            {

            //Limpiar la gráfica
            chart1.Series.Clear();

            chart1.Series.Add("Datos");
            chart1.Series["Datos"].ChartType = SeriesChartType.Line;

            // Enable range selection and zooming end user interface
            chart1.ChartAreas["ChartArea1"].CursorX.IsUserEnabled = true;
            chart1.ChartAreas["ChartArea1"].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.IsPositionedInside = true;
            chart1.ChartAreas["ChartArea1"].CursorY.IsUserEnabled = true;
            chart1.ChartAreas["ChartArea1"].CursorY.IsUserSelectionEnabled = true;
            chart1.ChartAreas["ChartArea1"].AxisY.ScaleView.Zoomable = true;
            chart1.ChartAreas["ChartArea1"].AxisY.ScrollBar.IsPositionedInside = true;


            //Poner rótulos apropiados en los ejes de la gráfica
            chart1.ChartAreas["ChartArea1"].AxisX.Title = "Número de Repeticiones";
            chart1.ChartAreas["ChartArea1"].AxisY.Title = "Disponibilidad Media";

            //Poner los valores máximo, mínimo y redondeo a los valores del eje X
            chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
            chart1.ChartAreas["ChartArea1"].AxisX.Maximum = numero_repeticiones;
            chart1.ChartAreas["ChartArea1"].AxisX.RoundAxisValues();
            chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "F3";

            //Poner redondeo a los valores del eje Y
            chart1.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();
            chart1.ChartAreas["ChartArea1"].AxisY.LabelStyle.Format = "F3";



           
           //BUCLE QUE REALIZA LAS REPETICIONES DE LAS SIMULACIONES
            for (int i = 1; i <= numero_repeticiones; i++)
            {

                string linea = "";

                //Calculo de la Disponibilidad mediante una simulacion
                Disponibilidad_una_simulacion = Simuladores_Monte_Carlo.Simulador1_Disponibilidad(Tiempo_A_Simular, Ley_Funcionamiento, parametro1_func,
                                        parametro2_func, Minimo_Admisible_Funcionando,
                                        Maximo_Admisible_Funcionando, Ley_Parada, parametro1_paro,
                                        parametro2_paro, Minimo_Admisible_Parado, Maximo_Admisible_Parado, r);



                Disponibilidad_Acumulada += Disponibilidad_una_simulacion;
                Disponibilidad_Media = Disponibilidad_Acumulada / i;

                //Incluir en gráfica de la simulacion
                chart1.Series["Datos"].Points.AddXY(i, Disponibilidad_Media);

                //Almacenar las Disponibilidades mínima y máxima encontradas para construir la gráfica correctamente
                if (Disponibilidad_Media < Disponibilidad_Minima_Encontrada) Disponibilidad_Minima_Encontrada = Disponibilidad_Media;
                if (Disponibilidad_Media > Disponibilidad_Maxima_Encontrada) Disponibilidad_Maxima_Encontrada = Disponibilidad_Media;

                //Actualizar valores máximo y mínimo de la Disponibilidad en el eje Y de la gráfica
                chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Disponibilidad_Minima_Encontrada;
                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Disponibilidad_Maxima_Encontrada;

                //Incrementar la barra indicadora
                progressBar1.Increment(1);

                linea = string.Format("{0,15}", i.ToString("F0")) + "  " +  "Disponibilidad media= " + string.Format("{0,15}", Disponibilidad_Media.ToString("F4")) + "\r\n ";
                //linea = Convert.ToString(i) + ", " + Convert.ToString(Disponibilidad_una_simulacion) + Convert.ToString(Disponibilidad_Media);
                textBox5.Text += linea;
            }

              }
            }
            catch
            {
                MessageBox.Show("Se ha producido un error en los datos de entrada", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //evento que muestra informacion de la recta al pasar por encima el raton
            this.chart1.GetToolTipText += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs>(ChartControlGraph.Chart1_GetToolTipText);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            //Borrar todo los campos de datos
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";  
            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";

            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            textBox8.Enabled = false;
            textBox9.Enabled = false; 
           
            comboBox1.Text = "";
            comboBox2.Text = "";

            label2.Text = "";
            label3.Text = "";
            label7.Text = "";
            label8.Text = "";

            //Limpiar la gráfica
            chart1.Series.Clear();

            //POner a cero la barra de progreso de los calculos
            progressBar1.Value = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Funcionamiento
            comboBox1.Text = "Weibull";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox1.Text = "2,1";
            textBox2.Text = "550,5";
            textBox3.Text = "100";
            textBox4.Text = "1500";
            label2.Text = "Beta";
            label3.Text = "Eta";

            //Parada-fallo
            comboBox2.Text = "Normal";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox6.Enabled = true;
            textBox7.Enabled = true;
            textBox8.Enabled = true;
            textBox9.Enabled = true;
            textBox6.Text = "80,7";
            textBox7.Text = "10,2";
            textBox8.Text = "40";
            textBox9.Text = "200";
            label7.Text = "Media";
            label8.Text = "Desviación Típica";

            //Tiempo a simular y repeticiones de la simulación
            textBox10.Text = "10000";
            textBox11.Text = "100";

        }

        private void label12_Click(object sender, EventArgs e)
        {

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
        private int ChequeoTextboxDisponibilidadVariasLeyes(ComboBox CBOX1, ComboBox CBOX2, TextBox TBOX1, TextBox TBOX2, TextBox TBOX3, TextBox TBOX4
        , TextBox TBOX5, TextBox TBOX6, TextBox TBOX7, TextBox TBOX8, TextBox TBOX18)
        {
            int valor = 0;
            /******************************************************   FUNCIONAMIENTO   *******************************************************/
            switch (CBOX1.Text)
            {
                case "Exponencial":
                    {
                        if (Convert.ToDouble(TBOX1.Text) > 100000)
                        {
                            MessageBox.Show("Valor de Gamma elevado en FUNCIONAMIENTO (<=500000)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if ((Convert.ToDouble(TBOX3.Text) > 1 / Convert.ToDouble(TBOX2.Text)) || (Convert.ToDouble(TBOX4.Text) < 1 / Convert.ToDouble(TBOX2.Text)))
                        {
                            MessageBox.Show("Valor de Lambda erróneo en FUNCIONAMIENTO: 1/Lambda ha de estar dentro del rango [Mínimo Admisible, Maximo Admisible]", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }

                        else if ((Convert.ToDouble(TBOX4.Text) - Convert.ToDouble(TBOX3.Text)) < 0)
                        {
                            MessageBox.Show("Valores maximos y minimos admisibles erroneos en FUNCIONAMIENTO ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if ((Convert.ToDouble(TBOX4.Text) > 500000) || Convert.ToDouble(TBOX3.Text) > 500000)
                        {
                            MessageBox.Show("Valor maximo o minimo admisible elevado en FUNCIONAMIENTO (<=500.000)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                    }

                    break;
                case "Weibull":
                    {
                        if (Convert.ToDouble(TBOX1.Text) > 10)
                        {
                            MessageBox.Show("Valor de Beta elevado en FUNCIONAMIENTO (<=10)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if (Convert.ToDouble(TBOX2.Text) > 500000)
                        {
                            MessageBox.Show("Valor de Eta elevado en FUNCIONAMIENTO (<=500000)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if (Convert.ToDouble(TBOX3.Text) > Convert.ToDouble(TBOX2.Text))
                        {
                            MessageBox.Show("Valor de Eta menor que el minimo admisible en FUNCIONAMIENTO ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if (Convert.ToDouble(TBOX4.Text) < Convert.ToDouble(TBOX2.Text))
                        {
                            MessageBox.Show("Valor de Eta mayor que el maximo admisible en FUNCIONAMIENTO ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }

                        else if ((Convert.ToDouble(TBOX4.Text) - Convert.ToDouble(TBOX3.Text)) < 0)
                        {
                            MessageBox.Show("Valores maximos y minimos admisibles erroneos en FUNCIONAMIENTO ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if ((Convert.ToDouble(TBOX4.Text) > 500000) || Convert.ToDouble(TBOX3.Text) > 500000)
                        {
                            MessageBox.Show("Valor maximo o minimo admisible elevado en FUNCIONAMIENTO (<=500.000)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                    }
                    break;
                case "Normal":
                    {
                        if (Convert.ToDouble(TBOX1.Text) > 500000)
                        {
                            MessageBox.Show("Valor de la Media elevado en FUNCIONAMIENTO (<=500000)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if (Convert.ToDouble(TBOX2.Text) > Convert.ToDouble(Convert.ToDouble(TBOX1.Text) / 2))
                        {
                            MessageBox.Show("Valor de la desviacion tipica no puede ser mayor que Media/2 en FUNCIONAMIENTO", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if (Convert.ToDouble(TBOX3.Text) > Convert.ToDouble(TBOX1.Text))
                        {
                            MessageBox.Show("Valor de la Media menor que el minimo admisible en FUNCIONAMIENTO", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if (Convert.ToDouble(TBOX4.Text) < Convert.ToDouble(TBOX1.Text))
                        {
                            MessageBox.Show("Valor de la Media mayor que el maximo admisible en FUNCIONAMIENTO", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }

                        else if ((Convert.ToDouble(TBOX4.Text) - Convert.ToDouble(TBOX3.Text)) < 0)
                        {
                            MessageBox.Show("Valores maximos y minimos admisibles erroneos en FUNCIONAMIENTO", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if ((Convert.ToDouble(TBOX4.Text) > 500000) || Convert.ToDouble(TBOX3.Text) > 500000)
                        {
                            MessageBox.Show("Valor maximo o minimo admisible elevado en FUNCIONAMIENTO (<=500.000)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                    }
                    break;
                case "Uniforme":
                    {
                        if ((Convert.ToDouble(TBOX4.Text) - Convert.ToDouble(TBOX3.Text)) < 0)
                        {
                            MessageBox.Show("Valores maximos y minimos admisibles erroneos en FUNCIONAMIENTO", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if ((Convert.ToDouble(TBOX4.Text) > 500000) || Convert.ToDouble(TBOX3.Text) > 500000)
                        {
                            MessageBox.Show("Valor maximo o minimo admisible elevado en FUNCIONAMIENTO (<=500.000)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                    }
                    break;
            }
            /******************************************************   FALLO   *******************************************************/

            switch (CBOX2.Text)
            {
                case "Exponencial":
                    {
                        if (Convert.ToDouble(TBOX5.Text) > 100000)
                        {
                            MessageBox.Show("Valor de Gamma elevado en FALLO (<=500000)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if (Convert.ToDouble(TBOX6.Text) > 1)
                        {
                            MessageBox.Show("Valor de Lambda elevado en FALLO (<=1)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }

                        else if ((Convert.ToDouble(TBOX7.Text) > 1 / Convert.ToDouble(TBOX6.Text)) || (Convert.ToDouble(TBOX8.Text) < 1 / Convert.ToDouble(TBOX6.Text)))
                        {
                            MessageBox.Show("Valor de Lambda erróneo en FALLO: 1/Lambda ha de estar dentro del rango [Mínimo Admisible, Maximo Admisible]", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }

                        else if ((Convert.ToDouble(TBOX8.Text) - Convert.ToDouble(TBOX7.Text)) < 0)
                        {
                            MessageBox.Show("Valores maximos y minimos admisibles erroneos en FALLO ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if ((Convert.ToDouble(TBOX8.Text) > 500000) || Convert.ToDouble(TBOX7.Text) > 500000)
                        {
                            MessageBox.Show("Valor maximo o minimo admisible elevado en FALLO (<=500.000)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                    }

                    break;
                case "Weibull2P":
                    {
                        if (Convert.ToDouble(TBOX5.Text) > 10)
                        {
                            MessageBox.Show("Valor de Beta elevado en FALLO (<=10)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if (Convert.ToDouble(TBOX6.Text) > 500000)
                        {
                            MessageBox.Show("Valor de Eta elevado en FALLO (<=500000)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if (Convert.ToDouble(TBOX7.Text) > Convert.ToDouble(TBOX6.Text))
                        {
                            MessageBox.Show("Valor de Eta menor que el minimo admisible en FALLO ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if (Convert.ToDouble(TBOX8.Text) < Convert.ToDouble(TBOX6.Text))
                        {
                            MessageBox.Show("Valor de Eta mayor que el maximo admisible en FALLO ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }

                        else if ((Convert.ToDouble(TBOX8.Text) - Convert.ToDouble(TBOX7.Text)) < 0)
                        {
                            MessageBox.Show("Valores maximos y minimos admisibles erroneos en FALLO ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if ((Convert.ToDouble(TBOX8.Text) > 500000) || Convert.ToDouble(TBOX7.Text) > 500000)
                        {
                            MessageBox.Show("Valor maximo o minimo admisible elevado en FALLO (<=500.000)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                    }
                    break;
                case "Normal":
                    {
                        if (Convert.ToDouble(TBOX5.Text) > 500000)
                        {
                            MessageBox.Show("Valor de la Media elevado en FALLO (<=500000)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if (Convert.ToDouble(TBOX6.Text) > Convert.ToDouble(Convert.ToDouble(TBOX5.Text) / 2))
                        {
                            MessageBox.Show("Valor de la desviacion tipica no puede ser mayor que Media/2 en FALLO", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if (Convert.ToDouble(TBOX7.Text) > Convert.ToDouble(TBOX5.Text))
                        {
                            MessageBox.Show("Valor de la Media menor que el minimo admisible en FALLO", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if (Convert.ToDouble(TBOX8.Text) < Convert.ToDouble(TBOX5.Text))
                        {
                            MessageBox.Show("Valor de la Media mayor que el maximo admisible en FALLO", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }

                        else if ((Convert.ToDouble(TBOX8.Text) - Convert.ToDouble(TBOX7.Text)) < 0)
                        {
                            MessageBox.Show("Valores maximos y minimos admisibles erroneos en FALLO", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if ((Convert.ToDouble(TBOX8.Text) > 500000) || Convert.ToDouble(TBOX7.Text) > 500000)
                        {
                            MessageBox.Show("Valor maximo o minimo admisible elevado en FALLO (<=500.000)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                    }
                    break;
                case "Uniforme":
                    {
                        if ((Convert.ToDouble(TBOX8.Text) - Convert.ToDouble(TBOX7.Text)) < 0)
                        {
                            MessageBox.Show("Valores maximos y minimos admisibles erroneos en FALLO", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if ((Convert.ToDouble(TBOX8.Text) > 500000) || Convert.ToDouble(TBOX7.Text) > 500000)
                        {
                            MessageBox.Show("Valor maximo o minimo admisible elevado en FALLO (<=500.000)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                    }
                    break;

            }
            if (Convert.ToDouble(TBOX18.Text) > 31536000)
            {
                MessageBox.Show("Valor del tiempo a simular elevado (<=31536000)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                valor = 1;
            }
            return valor;
        }



      }
}
