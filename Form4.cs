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
    public partial class Form4 : Form
    {
        //Datos de entrada para la representación
        public List<PointF> Datos_a_dibujar;
        public string rotulo_eje_x;
        public string rotulo_eje_y;
        public string rotulo_primer_dato;
        public string rotulo_segundo_dato;
        public double primer_dato;
        public double segundo_dato;

        //variable para indicar a la grafica si toma los valores de CosteRecuperacion.(Solo se aplica para Disponibilidad)
        //public bool representar_CosteRecuperacion;
        
        
        public Form4()
        {
            InitializeComponent();
            //Aseguramos que utiliza la configuración española para numeros decimales
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-ES");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form4_Shown(object sender, EventArgs e)
        {

            //Limpiar la gráfica
            chart1.Series.Clear();

            //limpiamos, ocultamos y desabilitamos por defecto label1, label2, textbox1 y textbox2
            label1.Text = "";
            label1.Visible = false;
            label2.Text = "";
            label2.Visible = false;
            textBox1.Text = "";
            textBox1.Enabled = false;
            textBox1.Visible = false;
            textBox2.Text = "";
            textBox2.Enabled = false;
            textBox2.Visible = false;
            
            //Se crea la Serie "Datos" en el chart1
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

            //Poner título apropiado en el fomulario de la grafica
            
            
            //Poner rótulos apropiados en los ejes de la gráfica
            chart1.ChartAreas["ChartArea1"].AxisX.Title = rotulo_eje_x;
            chart1.ChartAreas["ChartArea1"].AxisY.Title = rotulo_eje_y;


            //Búsqueda de los valores máximo y mínimo de los datos a representar, que se usan para definir los ejes de la gráfica
            //NOTA: SEGURAMENTE QUE EXISTE UNA UTILIDAD PREPROGRAMADA PARA ENCONTRAR EL MAXIMO Y EL MINIMO EN UNA LISTA  
            float Minima_Y_Encontrada = 9999999;
            float Maxima_Y_Encontrada = -9999999;
            float Minima_X_Encontrada = 9999999;
            float Maxima_X_Encontrada = -9999999;
            for (int i = 0; i <= Datos_a_dibujar.Count - 1; i++)
            {   
                if (Datos_a_dibujar[i].Y < Minima_Y_Encontrada) Minima_Y_Encontrada = Datos_a_dibujar[i].Y;
                if (Datos_a_dibujar[i].Y > Maxima_Y_Encontrada) Maxima_Y_Encontrada = Datos_a_dibujar[i].Y;
                if (Datos_a_dibujar[i].X < Minima_X_Encontrada) Minima_X_Encontrada = Datos_a_dibujar[i].X;
                if (Datos_a_dibujar[i].X > Maxima_X_Encontrada) Maxima_X_Encontrada = Datos_a_dibujar[i].X;
            }

            //Cargar en la serie Datos de chart1 los miembros de la lista
            for (int i = 0; i <= Datos_a_dibujar.Count - 1; i++)
            {
                chart1.Series["Datos"].Points.AddXY(Datos_a_dibujar[i].X, Datos_a_dibujar[i].Y);
            }


            //Actualizar valores máximo y mínimo de la Disponibilidad en el eje Y de la gráfica
            chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Minima_Y_Encontrada;
            chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Maxima_Y_Encontrada;
            chart1.ChartAreas["ChartArea1"].AxisX.Minimum = Minima_X_Encontrada;
            chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Maxima_X_Encontrada;

            //Redondeo a los valores del eje X y del eje Y            
            chart1.ChartAreas["ChartArea1"].AxisX.RoundAxisValues();
            chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "F2";
            chart1.ChartAreas["ChartArea1"].AxisY.RoundAxisValues();
            chart1.ChartAreas["ChartArea1"].AxisY.LabelStyle.Format = "F6";


            //Representar el primer dato si su rotulo no está vacio
            if (rotulo_primer_dato != "")
            {
                textBox1.Visible = true;
                label1.Visible = true;
                label1.Text = rotulo_primer_dato;
                textBox1.Text = Convert.ToString(primer_dato);
            }

            //Representar el segundo dato si su rotulo no está vacio
            if (rotulo_segundo_dato != "")
            {
                textBox2.Visible = true;
                label2.Visible = true;
                label2.Text = rotulo_segundo_dato;
                textBox2.Text = Convert.ToString(segundo_dato);
            }
                        
            //evento que muestra informacion de la recta al pasar por encima el raton
            this.chart1.GetToolTipText += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs>(ChartControlGraph.Chart1_GetToolTipText);
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
