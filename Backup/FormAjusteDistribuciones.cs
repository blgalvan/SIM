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
    public partial class Formajuste : Form
    {
        public Formajuste()
        {
            InitializeComponent();
            //Aseguramos que utiliza la configuración española para numeros decimales
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-ES");
        }

        private void FormRegLinSim_Load(object sender, EventArgs e)
        {

        }


        private void EscribirResultadosEnTextBox1(string NombreAjuste, string NombreParam1, string NombreParam2, string NombreParam3, double cc, double param1, double param2,
                                        double param3, double cd, double var, double desvtip)
        {
            //presentacion de los resultados en el textbox1
            textBox1.Text += " " + "\r\n";
            textBox1.Text += "Resultados del ajuste " + NombreAjuste + "\r\n";
            textBox1.Text += "------------------------------------------------------------------  " + "\r\n";
            textBox1.Text += "Coeficiente de Correlación        : " + Convert.ToString(cc) + "\r\n";
            textBox1.Text += NombreParam1 + "                   : " + Convert.ToString(param1) + "\r\n";
            textBox1.Text += NombreParam2 + "                   : " + Convert.ToString(param2) + "\r\n";
            if (NombreAjuste == "weibull 3P  W(,delta,beta,eta)         ") textBox1.Text += NombreParam3 + "                   : " + Convert.ToString(param3) + "\r\n";
            textBox1.Text += "Coeficiente de Determinación     : " + Convert.ToString(cd) + "\r\n";
            textBox1.Text += "Varianza de la Estimación         : " + Convert.ToString(var) + "\r\n";
            textBox1.Text += "Desviación Típica Estimación     : " + Convert.ToString(desvtip) + "\r\n";

        }

        private void EscribirDatosEnTextBox1(int NumDatos, double[] xx, double[] yy)
        {

            //Presentar datos y resultados en el textBox1";
            //textBox1.Text = "";
            textBox1.Text += "Número de Parejas de Datos   :" + Convert.ToString(NumDatos) + "\r\n";
            textBox1.Text += "Valores de Parejas de Datos  :" + "\r\n";
            textBox1.Text += "-------------------------------------------  " + "\r\n";
            for (int i = 1; i <= NumDatos; i++)
            {
                textBox1.Text += Convert.ToString(xx[i]) + "     " + Convert.ToString(yy[i]) + "\r\n";
            }

        }



        private void button1_Click(object sender, EventArgs e)
        {
            int N = grid.Rows.Count-1; 
            double[] y = new double[grid.Rows.Count+1];
            double[] Logy = new double[grid.Rows.Count + 1];
            double[] LogLogy = new double[grid.Rows.Count + 1];
            double[] x = new double[grid.Rows.Count+1];
            double[] Logx = new double[grid.Rows.Count + 1];
            double[] ResultadosAjusteLineal = new double[10];
            string NombreAjuste, NombreParam1, NombreParam2, NombreParam3;
            textBox1.Text = "";

            
            try
            {
            for (int i = 1; i < grid.Rows.Count; i++)
            {
                if (grid.Rows[i-1].Cells[0].Value != null)
                {
                    x[i] = double.Parse(grid.Rows[i-1].Cells[0].Value.ToString());
                    y[i] = double.Parse(grid.Rows[i-1].Cells[1].Value.ToString());
                    Logx[i] = Math.Log(double.Parse(grid.Rows[i - 1].Cells[0].Value.ToString()));
                    Logy[i] = Math.Log(double.Parse(grid.Rows[i - 1].Cells[1].Value.ToString()));
                    LogLogy[i] = Math.Log(-Math.Log(double.Parse(grid.Rows[i - 1].Cells[1].Value.ToString())));
                }
            }


            //Presentar datos en el textBox1";
                if (N != 0) EscribirDatosEnTextBox1(N, x, y);
                else textBox1.Text = "";

            //--A)AJUSTE LINEAL
            //---------------------------------------------------------------------------------------------
            
            //Llamada al metodo "RegresionLineal()" que reliza el ajuste lineal que está en la clase "Estadistica.cs"
            Estadistica.RegresionLineal(N, x, y, ref ResultadosAjusteLineal);

            //Presentacion de resultados en el textbox1
            NombreAjuste = "ajuste lineal Y = A + Bx             ";
            NombreParam1 = "A   ";
            NombreParam2 = "B   ";
            NombreParam3 = "No existe";
                if (N != 0) EscribirResultadosEnTextBox1(NombreAjuste, NombreParam1, NombreParam2, NombreParam3, ResultadosAjusteLineal[0], ResultadosAjusteLineal[1],
                                   ResultadosAjusteLineal[2], 0, ResultadosAjusteLineal[3], ResultadosAjusteLineal[4], ResultadosAjusteLineal[5]);
                else textBox1.Text = "";

            /*
            //--B)AJUSTE EXPONENCIAL
            //------------------------------------------------------------------------------------------------
            //Llamada al metodo "RegresionLineal()" que reliza el ajuste lineal que está en la clase "Estadistica.cs"
            Estadistica.RegresionLineal(N, x, Logy, ref ResultadosAjusteLineal);

            //Presentacion de resultados en el textbox1
            NombreAjuste = "ajuste exponencial y = AExp(Bx)        ";
            NombreParam1 = "A   ";
            NombreParam2 = "B   ";
            NombreParam3 = "No existe";
                 if (N != 0) EscribirResultadosEnTextBox1(NombreAjuste, NombreParam1, NombreParam2, NombreParam3, ResultadosAjusteLineal[0], Math.Exp(ResultadosAjusteLineal[1]),
                                   ResultadosAjusteLineal[2], 0, ResultadosAjusteLineal[3], ResultadosAjusteLineal[4], ResultadosAjusteLineal[5]);

                else textBox1.Text = "";
              
             */

               //B-1)AJUSTE EXPONENCIAL Y=Exp(A(t-B))
                 Estadistica.RegresionLineal(N, x, Logy, ref ResultadosAjusteLineal);
                 NombreAjuste = "ajuste exponencial y = Exp(B(x-A))        ";
                 NombreParam1 = "A   ";
                 NombreParam2 = "B   ";
                 NombreParam3 = "No existe";
                 if (N != 0) EscribirResultadosEnTextBox1(NombreAjuste, NombreParam1, NombreParam2, NombreParam3, ResultadosAjusteLineal[0],-ResultadosAjusteLineal[1]/ResultadosAjusteLineal[2],
                                  ResultadosAjusteLineal[2], 0, ResultadosAjusteLineal[3], ResultadosAjusteLineal[4], ResultadosAjusteLineal[5]);

                 else textBox1.Text = "";


            /*
            //--C)AJUSTE POTENCIAL
            //----------------------------------------------------------------------------------------------
            //Llamada al metodo "RegresionLineal()" que reliza el ajuste lineal que está en la clase "Estadistica.cs"
            Estadistica.RegresionLineal(N, Logx, Logy, ref ResultadosAjusteLineal);

            //Presentacion de resultados en el textbox1
            NombreAjuste = "ajuste potencial y = Ax^B         ";
            NombreParam1 = "A   ";
            NombreParam2 = "B   ";
            NombreParam3 = "No existe";
                if (N != 0) EscribirResultadosEnTextBox1(NombreAjuste, NombreParam1, NombreParam2, NombreParam3, ResultadosAjusteLineal[0], Math.Exp(ResultadosAjusteLineal[1]),
                                   ResultadosAjusteLineal[2], 0, ResultadosAjusteLineal[3], ResultadosAjusteLineal[4], ResultadosAjusteLineal[5]);
                else textBox1.Text = "";
            */

            //--D)AJUSTE WEIBULL 2P
            //-------------------------------------------------------------------------------------------------
            //Llamada al metodo "RegresionLineal()" que reliza el ajuste lineal que está en la clase "Estadistica.cs"
            Estadistica.RegresionLineal(N, Logx, LogLogy, ref ResultadosAjusteLineal);

            //Presentacion de resultados en el textbox1
            NombreAjuste = "ajuste weibull 2P  W(beta,Eta)      ";
            NombreParam1 = "Eta ";
            NombreParam2 = "Beta";
            NombreParam3 = "No existe";
                if (N != 0) EscribirResultadosEnTextBox1(NombreAjuste, NombreParam1, NombreParam2, NombreParam3, ResultadosAjusteLineal[0], Math.Exp(-ResultadosAjusteLineal[1] / ResultadosAjusteLineal[2]),
                                   ResultadosAjusteLineal[2], 0, ResultadosAjusteLineal[3], ResultadosAjusteLineal[4], ResultadosAjusteLineal[5]);

                else textBox1.Text = "";


            /*
            //--E)AJUSTE WEIBULL 3P
            //--------------------------------------------------------------------------------------------------------
            //E1) Crea e inicializar variables necesarias para la búsqueda
            double cc_del_mejor = 0;
            double eta_del_mejor = 0;
            double beta_del_mejor = 0;
            double delta_del_mejor = 0;
            double cd_del_mejor = 0;
            double var_del_mejor = 0;
            double desvtip_del_mejor = 0;
            Random r = new Random();
            int num_ensayos = 2000;
            double delta=0;

            double max = x[1]-x[1]/1000;
            double min = - max;
            double aleat_auxi;

            //E2) bucle que realiza la búsqueda del óptimo coeficiente de determinación y los parámetros asociados a la Weibull
            for(int j=1;j<=num_ensayos;j++)
            {
                //Generar un nuevo valor de delta
                if (j < num_ensayos / 2)
                {
                    //fase de exploracion del espacio de busqueda
                    delta = GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme(min, max, r);
                }
                else 
                {
                    //fase de explotacion de lo aprendido
                    aleat_auxi = r.NextDouble();
                    if (aleat_auxi <= 0.33) delta = GeneradoresDeAleatorios.Generador_Aleatorio_Normal(delta, delta / 10, min, max, r);
                    if (aleat_auxi > 0.33 && aleat_auxi <= 0.66) delta = GeneradoresDeAleatorios.Generador_Aleatorio_Normal(delta, delta / 100, min, max, r);
                    if (aleat_auxi > 0.66) delta = GeneradoresDeAleatorios.Generador_Aleatorio_Normal(delta, delta / 1000, min, max, r);
                }

                //Restar delta a todos los x y tomr logaritmos neperianos
                for (int i = 1; i < grid.Rows.Count; i++)
                {
                    Logx[i] = Math.Log(double.Parse(grid.Rows[i - 1].Cells[0].Value.ToString())-delta);
                }

                //Llamada al metodo "RegresionLineal()" que reliza el ajuste lineal que está en la clase "Estadistica.cs"
                Estadistica.RegresionLineal(N, Logx, LogLogy, ref ResultadosAjusteLineal);
                
                //Guardar el mejor resultado encontrado hasta el momento
                if (ResultadosAjusteLineal[3] > cd_del_mejor)
                {
                    cc_del_mejor = ResultadosAjusteLineal[0];
                    eta_del_mejor = Math.Exp(-ResultadosAjusteLineal[1] / ResultadosAjusteLineal[2]);
                    beta_del_mejor = ResultadosAjusteLineal[2];
                    cd_del_mejor = ResultadosAjusteLineal[3];
                    var_del_mejor = ResultadosAjusteLineal[4];
                    desvtip_del_mejor = ResultadosAjusteLineal[5];
                    delta_del_mejor = delta;
                }
            }
            
            //E3) presentacion de los resultados en el textbox1
            NombreAjuste = "weibull 3P  W(,delta,beta,eta)         ";
            NombreParam1 = "Eta ";
            NombreParam2 = "Beta";
            NombreParam3 = "Delta";
                if (N != 0) EscribirResultadosEnTextBox1(NombreAjuste, NombreParam1, NombreParam2, NombreParam3, cc_del_mejor, eta_del_mejor, beta_del_mejor,
                                            delta_del_mejor, cd_del_mejor, var_del_mejor, desvtip_del_mejor);
                else textBox1.Text = "";
            */

            }
            catch
            {
                MessageBox.Show("Se ha producido un error en los datos de entrada", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Text = "";
            }

        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
                int row = grid.Rows.Add();
                grid.Rows[row].Cells[0].Value = 4;
                grid.Rows[row].Cells[1].Value = 0.9601;

                row = grid.Rows.Add();
                grid.Rows[row].Cells[0].Value = 14;
                grid.Rows[row].Cells[1].Value = 0.8007;

                row = grid.Rows.Add();
                grid.Rows[row].Cells[0].Value = 39;
                grid.Rows[row].Cells[1].Value = 0.4500;

                row = grid.Rows.Add();
                grid.Rows[row].Cells[0].Value = 71;
                grid.Rows[row].Cells[1].Value = 0.2614;

                row = grid.Rows.Add();
                grid.Rows[row].Cells[0].Value = 99;
                grid.Rows[row].Cells[1].Value = 0.045;
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            grid.Rows.Clear();
            textBox1.Text = "";
        }
        /********** Formateo de los datos de la celda del grid *************/

        private void dataGridDatos_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Donde el numero "4" es el numero de tu columna
            //if (grid.CurrentCell.ColumnIndex == 4)
            //{
            if (e.KeyChar == (char)Keys.Back || char.IsNumber(e.KeyChar) || e.KeyChar == 44)

                e.Handled = false;
            else
                e.Handled = true;
            //}
        }

        private void dataGridDatos_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //if (grid.CurrentCell.ColumnIndex == 4)
            //{
            TextBox txt = e.Control as TextBox;
            if (txt != null)
            {
                txt.KeyPress -= new KeyPressEventHandler(dataGridDatos_KeyPress);
                txt.KeyPress += new KeyPressEventHandler(dataGridDatos_KeyPress);
            }
            //}
        }
    }
}
