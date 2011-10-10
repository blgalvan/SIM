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
    public partial class Form2 : Form
    {

        //public double[] equis;
        //public double[] ies;
        //public int contador;
        //public double Tiempo_A_Simular;
        public List<PointF> Datos_a_representar;
        public double DiponibilidadOperacionalFinal;
        public double CosteRecuperacionFinal;
        
        
        public Form2()
        {
            InitializeComponent();
            //Aseguramos que utiliza la configuración española para numeros decimales
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-ES");
        }

        
        private string Formato(double numero, int numdecimales, int numdigitos)
        {
            string cadena = numero.ToString("F" + numdecimales);
            cadena = new String(' ', numdigitos-cadena.Length) + cadena;
            return cadena;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            double X, Y;
            Random r = new Random(DateTime.Now.Millisecond);

            
            double TiempoFuncionandoAcumulado = 0;
            double TiempoParadoAcumulado = 0;
            double t;
            double Disponibilidad;
            double Disponibilidad_Media = 0;
            double Disponibilidad_Acumulada = 0;
            textBox3.Text = "";

            List<PointF> Datos_a_representar = new List<PointF>();

            try
            {
            //Captura de indicaciones del usuario
            double Tiempo_A_Simular = Convert.ToDouble(textBox1.Text);
            int numero_repeticiones = Convert.ToInt16(textBox2.Text);
            double MinimoFuncionando = Convert.ToDouble(textBox7.Text);
            double MaximoFuncionando = Convert.ToDouble(textBox6.Text);
            double MinimoParado = Convert.ToDouble(textBox5.Text);
            double MaximoParado = Convert.ToDouble(textBox4.Text);
            double Disponibilidad_Minima_Encontrada = 1;
            double Disponibilidad_Maxima_Encontrada = 0;
            
           // Comprobamos que el usuario introduce correctamente los valores maximos y minimos
              if (((MaximoFuncionando - MinimoFuncionando) < 0) || ((MaximoParado - MinimoParado) < 0))
               {
                   MessageBox.Show("Valores maximos y minimos erroneos", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
               }
              else if (((MaximoFuncionando > Tiempo_A_Simular) || (MinimoFuncionando > Tiempo_A_Simular)))
              {
                  MessageBox.Show("Valor del tiempo de funcionamiento mayor al tiempo a simular", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
              }
              else if (((MaximoParado > Tiempo_A_Simular) || (MinimoParado > Tiempo_A_Simular)))
              {
                  MessageBox.Show("Valor del tiempo de parada mayor al tiempo a simular", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
              }
              else if (Tiempo_A_Simular > 31536000)
              {
                  MessageBox.Show("Valor del tiempo a simular elevado", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
              }
           else//lanzamos la simulacion
           {

            //Indicar el tamaño máximo de la barra de seguimiento
            progressBar1.Maximum = numero_repeticiones;
            progressBar1.Value = 0;

            //Limpiar la gráfica
            chart1.Series.Clear();
            
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
            chart1.ChartAreas["ChartArea1"].AxisX.Title = "Tiempo";
            chart1.ChartAreas["ChartArea1"].AxisY.Title = "Disponibilidad";

            //Poner los valores máximo, mínimo y redondeo a los valores del eje X
            chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
            chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Tiempo_A_Simular;
            chart1.ChartAreas["ChartArea1"].AxisX.RoundAxisValues();
            chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format="F3";

            //Poner redondeo a los valores del eje Y
            chart1.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();
            chart1.ChartAreas["ChartArea1"].AxisY.LabelStyle.Format = "F3";


            //Crear fichero de resultados
            System.IO.StreamWriter archivo = new System.IO.StreamWriter("leccion2.txt");
            string linea;

                   
            //BUCLE QUE REALIZA LAS REPETICIONES DE LAS SIMULACIONES
            for (int i = 1; i <= numero_repeticiones; i++)
            {

                TiempoFuncionandoAcumulado = 0;
                TiempoParadoAcumulado = 0;

                linea = "SIMULACION NUMERO : " + Convert.ToString(i);
                archivo.WriteLine(linea);
                linea = "-------------------------------- ";
                archivo.WriteLine(linea);
                linea = "t_funcionando      t_parado        t_total_func       t_total_parado       t_total       Disponibilidad ";
                archivo.WriteLine(linea);
                 
                chart1.Series.Add("Series" + Convert.ToString(i));
                chart1.Series["Series" + Convert.ToString(i)].ChartType = SeriesChartType.Line;

                
                //BUCLE QUE REALIZA CADA SIMULACIÓN
                do
                {
                    //Generar tiempo funcionando y acumularlo
                    t = GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme(MinimoFuncionando, MaximoFuncionando, r);
                    //linea = Formato(t,4,12) +" ";
                    linea = string.Format("{0,15}", t.ToString("F4")) + " ";
                    TiempoFuncionandoAcumulado += t;

                    //Generar tiempo parado y acumularlo
                    t = GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme(MinimoParado, MaximoParado, r);
                    linea += string.Format("{0,15}", t.ToString("F4")) + " ";
                    TiempoParadoAcumulado += t;

                    //guardar tiempos acumulados tambien
                    //linea += TiempoFuncionandoAcumulado.ToString("F4") + " ";
                    linea += string.Format("{0,15}", TiempoFuncionandoAcumulado.ToString("F4")) + " ";
                    //linea += Convert.ToString(TiempoParadoAcumulado) + " ";
                    linea += string.Format("{0,15}", TiempoParadoAcumulado.ToString("F4")) + " ";

                    //Calcular Disponibilidad
                    Disponibilidad = TiempoFuncionandoAcumulado / (TiempoFuncionandoAcumulado + TiempoParadoAcumulado);
                    //linea += Convert.ToString(TiempoFuncionandoAcumulado + TiempoParadoAcumulado)+ " ";
                    linea += string.Format("{0,15}", (TiempoFuncionandoAcumulado + TiempoParadoAcumulado).ToString("F4")) + " ";

                    //Escribir los cálculos en archivo
                    //linea += Convert.ToString(Disponibilidad);
                    linea += string.Format("{0,15}", Disponibilidad.ToString("F4")) + " ";
                    archivo.WriteLine(linea);

                    //Incluir en gráfica de la simulacion
                    X = TiempoFuncionandoAcumulado + TiempoParadoAcumulado;
                    Y = Disponibilidad;
                    chart1.Series["Series" + Convert.ToString(i)].Points.AddXY(X, Y);


                    if (i == 1)
                    {
                        //Añadir a la lista de datos la nueva pareja de valores
                        Datos_a_representar.Add(new PointF((float)X, (float)Y));
                    }
                    

                    //Almacenar las Disponibilidades mínima y máxima encontradas para construir la gráfica correctamente
                    if (Disponibilidad < Disponibilidad_Minima_Encontrada) Disponibilidad_Minima_Encontrada = Disponibilidad;
                    if (Disponibilidad > Disponibilidad_Maxima_Encontrada) Disponibilidad_Maxima_Encontrada = Disponibilidad;
                     

                } while (TiempoFuncionandoAcumulado + TiempoParadoAcumulado <= Tiempo_A_Simular);

                //Actualizar valores máximo y mínimo de la Disponibilidad en el eje Y de la gráfica
                chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Disponibilidad_Minima_Encontrada;
                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Disponibilidad_Maxima_Encontrada;
                
                //Calcular la disponibilidad media según avanzan las repeticiones
                Disponibilidad_Acumulada += Disponibilidad;
                Disponibilidad_Media = Disponibilidad_Acumulada / i;

                //Presentar los resultados numéricos en el TextBox de pantalla
                textBox3.Text += Convert.ToString(i) + ", Disponibilidad Operacional media= " + Convert.ToString(Disponibilidad_Media)+"\r\n";

                //Incrementar la barra indicadora
                progressBar1.Increment(1);

                //Escribir los cálculos en archivo
                //linea = "Disponibilidad Operacional media= " + Convert.ToString(Disponibilidad_Media);
                linea = "Disponibilidad Operacional media= " + string.Format("{0,15}", Disponibilidad_Media.ToString("F4")) + " "; ;
                archivo.WriteLine(linea);
                linea = "                                ";
                archivo.WriteLine(linea);

            }

            //Cerrar archivo de resultados
            archivo.Close();


            Form4 frm = new Form4();
            frm.Datos_a_dibujar = Datos_a_representar;
            frm.rotulo_eje_x = "tiempo de simulacion";
            frm.rotulo_eje_y = "Disponibilidad operacional media";
            frm.rotulo_primer_dato = "Disponibilidad Operacional";
            frm.primer_dato = Disponibilidad_Media;
            frm.Show();

           }
            }
            catch
            {
                MessageBox.Show("Se ha producido un error en los datos de entrada", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //evento que muestra informacion de la recta al pasar por encima el raton
            this.chart1.GetToolTipText += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs>(ChartControlGraph.Chart1_GetToolTipText);
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Limpiar los TextBox
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";

            //Limpiar la gráfica
            chart1.Series.Clear();

            //POner a cero la barra de progreso de los calculos
            progressBar1.Value = 0;
            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "12000";
            textBox2.Text = "30";
            textBox3.Text = "";
            textBox4.Text = "40";
            textBox5.Text = "7";
            textBox6.Text = "100";
            textBox7.Text = "30";
        }

        private void chart1_Click(object sender, EventArgs e)
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
    }
}


 