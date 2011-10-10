using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace SIM
{
    public partial class FormFuncionaFalla : Form
    {

        //public double DiponibilidadOperacionalFinal;
        //public double CosteRecuperacionFinal;

        
        public FormFuncionaFalla()
        {
            InitializeComponent();
            //Aseguramos que utiliza la configuración española para numeros decimales
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-ES");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {


            Random r = new Random(DateTime.Now.Millisecond);
            textBox5.Enabled = true;
            textBox5.Text = "";

            double TiempoFuncionandoAcumulado = 0;
            double TiempoParadoAcumulado = 0;
            double t = 0;
            double Disponibilidad;

            double X1 = 0;
            double Y1 = 0;
            double X2 = 0;
            double Y2 = 0;


            //Crear e inicializar la lista que contendrá los puntos de la gráfica
            List<PointF> Datos_a_representar = new List<PointF>();

            try
            {
                //Captura de indicaciones del usuario
                double Tiempo_A_Simular = Convert.ToDouble(textBox10.Text);


                //Captura de datos sobre Ley de Funcionamiento ------------------------------------------------------
                string ley_func = comboBox1.Text;
                double ley_func_param1 = 0;
                double ley_func_param2 = 0;
                double MinimoFuncionando = 0;
                double MaximoFuncionando = 0;

                if (comboBox1.Text == "Exponencial" || comboBox1.Text == "Weibull2P")
                {
                    ley_func_param1 = Convert.ToDouble(textBox1.Text);
                    ley_func_param2 = Convert.ToDouble(textBox2.Text);
                    MinimoFuncionando = Convert.ToDouble(textBox3.Text);
                    MaximoFuncionando = Convert.ToDouble(textBox4.Text);
                }

                if (comboBox1.Text == "Uniforme")
                {
                    MinimoFuncionando = Convert.ToDouble(textBox3.Text);
                    MaximoFuncionando = Convert.ToDouble(textBox4.Text);
                }

                //pregunta que debo poner aqui para que no ejecute
                if (comboBox1.Text == "Ninguna Ley") return;

                //Fin de la captura de datos sobre ley de funcionamiento


                //Captura de datos sobre Ley de Parada ------------------------------------------------------------
                string ley_paro = comboBox2.Text;
                double ley_paro_param1 = 0;
                double ley_paro_param2 = 0;
                double MinimoParado = 0;
                double MaximoParado = 0;

                if (comboBox2.Text == "Exponencial" || comboBox2.Text == "Normal")
                {
                    ley_paro_param1 = Convert.ToDouble(textBox6.Text);
                    ley_paro_param2 = Convert.ToDouble(textBox7.Text);
                    MinimoParado = Convert.ToDouble(textBox8.Text);
                    MaximoParado = Convert.ToDouble(textBox9.Text);
                }

                if (comboBox2.Text == "Weibull2P")
                {
                    ley_paro_param1 = Convert.ToDouble(textBox6.Text);
                    ley_paro_param2 = Convert.ToDouble(textBox7.Text);
                    MinimoParado = Convert.ToDouble(textBox8.Text);
                    MaximoParado = Convert.ToDouble(textBox9.Text);
                }

                if (comboBox2.Text == "Uniforme")
                {
                    MinimoParado = Convert.ToDouble(textBox8.Text);
                    MaximoParado = Convert.ToDouble(textBox9.Text);
                }

                //pregunta que debo poner aqui para que no ejecute
                if (comboBox2.Text == "Ninguna Ley") return;

                //Fin de la captura de datos sobre ley de parada



                //Captura de indicaciones sobre recuperacion ------------------------------------------------------
                string ley_recu = comboBox3.Text;
                double ley_recu_param1 = 0;
                double ley_recu_param2 = 0;
                double MinimoRecup = 0;
                double MaximoRecup = 0;

                if (comboBox3.Text == "Exponencial" || comboBox3.Text == "Weibull2P")
                {
                    ley_recu_param1 = Convert.ToDouble(textBox12.Text);
                    ley_recu_param2 = Convert.ToDouble(textBox13.Text);
                    MinimoRecup = Convert.ToDouble(textBox14.Text);
                    MaximoRecup = Convert.ToDouble(textBox15.Text);
                }

                if (comboBox3.Text == "Línea recta")
                {
                    X1 = Convert.ToDouble(textBox12.Text);
                    Y1 = Convert.ToDouble(textBox13.Text);
                    X2 = Convert.ToDouble(textBox14.Text);
                    Y2 = Convert.ToDouble(textBox15.Text);
                }

                //pregunta que debo poner aqui para que no ejecute
                //if (comboBox3.Text == "Ninguna Ley") return;

                //Fin de la captura de datos sobre recuperacion



                //Captura de indicaciones sobre costes ------------------------------------------------------------
                double CosteMantenimientoHr = 0;
                double CostePerdidaProduccionHr = 0;
                double CosteMantenimietoCadaIntervencion = 0;
                double XX1 = 0;
                double YY1 = 0;
                double XX2 = 0;
                double YY2 = 0;
                string ley_coste = "";
                double CosteEsteMantenimiento = 0;
                double CosteEstaPerdidaDeProduccion = 0;

                if (comboBox4.Text == "Fijo por tiempo")
                {
                    ley_coste = "Fijo por tiempo";
                    CosteMantenimientoHr = Convert.ToDouble(textBox16.Text);
                    CostePerdidaProduccionHr = Convert.ToDouble(textBox17.Text);
                }

                if (comboBox4.Text == "Fijo por intervención")
                {
                    ley_coste = "Fijo por intervención";
                    CosteMantenimietoCadaIntervencion = Convert.ToDouble(textBox16.Text);
                    CostePerdidaProduccionHr = Convert.ToDouble(textBox17.Text);
                }

                if (comboBox4.Text == "Lineal creciente")
                {
                    ley_coste = "Lineal creciente";
                    XX1 = Convert.ToDouble(textBox16.Text);
                    YY1 = Convert.ToDouble(textBox17.Text);
                    XX2 = Convert.ToDouble(textBox18.Text);
                    YY2 = Convert.ToDouble(textBox19.Text);
                    CostePerdidaProduccionHr = Convert.ToDouble(textBox20.Text);
                }

                //Fin de la captura de datos sobre costes



                //Captura de indicaciones sobre estrategia de mantenimiento ---------------------------------------
                //Fin de la captura de datos sobre estrategia de mantenimiento 

                //Estimar el tamaño máximo de la barra de seguimiento
                progressBar1.Maximum = Convert.ToInt32((MaximoFuncionando - MinimoFuncionando) / 2 + (MaximoParado - MinimoParado) / 2);
                progressBar1.Value = 0;

                //Borrar el contenido del textbox de salida de resultados
                textBox5.Text = ""; ;


                TiempoFuncionandoAcumulado = 0;
                TiempoParadoAcumulado = 0;
                double TiempoTranscurrido = 0;
                double minimoY = 0;
                double maximoY = 1;
                double TiempoParcialFuncionando;
                double TiempoParcialFallado;
                double PuntoFinalBajadaDisponibilidad = 0;
                double DisponibilidadMinimaAdmisible = 0.0;

                double CosteTotalMantenimiento = 0;
                double CosteTotalPerdidaProduccion = 0;
                double CosteAcumuladoRecuperacion = 0;
                double CosteMedioRecuperacion = 0;

                int ContadorCiclosFuncionaPara = 0;
                double AcumuladorTiemposEntreFallos = 0;
                double ContadorTiempoDisponibilidadPositiva;
                double MTBF = 0;
                double MTTR = 0;
                double IntensidadFallos = 0;
                double TiempoDelCiclo = 0;

                //Aseguramos que los datos introducidos son correctos
                if(ChequeoTextboxCicloFuncionamieontoParada(comboBox1,comboBox2,comboBox3,comboBox4,textBox1,textBox2,textBox3,textBox4
                ,textBox6,textBox7,textBox8,textBox9,textBox12,textBox13,textBox14,textBox15,textBox16,textBox17,textBox18
                ,textBox19,textBox20,textBox10)==0)
                {//Lanzamos la simulación

                //BUCLE QUE REALIZA la SIMULACIÓN
                do
                {
                    //Incrementar el contador de ciclos funciona-para
                    ContadorCiclosFuncionaPara += 1;

                    //Incrementar la barra indicadora
                    progressBar1.Increment(1);

                    //Puntos inicial y final de la subida a funcionamiento
                    Datos_a_representar.Add(new PointF((float)TiempoTranscurrido, (float)minimoY));
                    Datos_a_representar.Add(new PointF((float)TiempoTranscurrido, (float)maximoY));

                    //Generar tiempo funcionando, controlar su validez y acumularlo
                    if (ley_func == "Uniforme") t = GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme(MinimoFuncionando, MaximoFuncionando, r);
                    if (ley_func == "Exponencial") t = GeneradoresDeAleatorios.Generador_Aleatorio_Exponencial(ley_func_param1, 1 / ley_func_param2, MinimoFuncionando, MaximoFuncionando, r);
                    if (ley_func == "Weibull2P") t = GeneradoresDeAleatorios.Generador_Aleatorio_Weibull_2P(ley_func_param1, ley_func_param2, MinimoFuncionando, MaximoFuncionando, r);
                    if (ley_func == "Normal") t = GeneradoresDeAleatorios.Generador_Aleatorio_Normal(ley_func_param1, ley_func_param2, MinimoFuncionando, MaximoFuncionando, r);

                    //bajada de disponibilidad durante el periodo de funcionamiento, solo Exponencial y Weubull tienen sentido
                    //Para las otras leyes se opta por no bajar la disponibilidad
                    if (ley_func == "Exponencial") PuntoFinalBajadaDisponibilidad = Math.Exp(-ley_func_param2 * t) - 1 + maximoY;
                    if (ley_func == "Weibull2P") PuntoFinalBajadaDisponibilidad = Math.Exp(-Math.Pow(t / ley_func_param2, ley_func_param1)) - 1 + maximoY;
                    if (ley_func == "Uniforme" || ley_func == "Normal") PuntoFinalBajadaDisponibilidad = maximoY;

                    //Controlar la validez de t ==> encontrar el t que hace cero la disponibilidad (si procede)
                    ContadorTiempoDisponibilidadPositiva = 0;
                    if (PuntoFinalBajadaDisponibilidad <= DisponibilidadMinimaAdmisible)
                    {
                        double DisponibilidadInstantanea = -1000;
                        for (int j = 1; j <= t; j++)
                        {
                            if (ley_func == "Exponencial") DisponibilidadInstantanea = Math.Exp(-ley_func_param2 * j) - 1 + maximoY;
                            if (ley_func == "Weibull2P") DisponibilidadInstantanea = Math.Exp(-Math.Pow(j / ley_func_param2, ley_func_param1)) - 1 + maximoY;
                            if (DisponibilidadInstantanea <= DisponibilidadMinimaAdmisible) break;
                            ContadorTiempoDisponibilidadPositiva += 1;
                        }
                        PuntoFinalBajadaDisponibilidad = DisponibilidadMinimaAdmisible;
                        t = ContadorTiempoDisponibilidadPositiva;
                    }


                    //Acumular tiempos funcionando
                    TiempoFuncionandoAcumulado += t;
                    TiempoParcialFuncionando = t;

                    //Acumular el tiempo de funcionamiento al tiempo total simulado o "tiempo transcurrido"
                    TiempoTranscurrido += t;


                    //Dibujar puntos intermedios para mejorar el aspecto de la gráfica
                    if (PuntoFinalBajadaDisponibilidad >= 0)
                    {
                        double TiempoIntermedio = 0;
                        double maxY_new = 0;
                        if (ley_func == "Exponencial" || ley_func == "Weibull2P")
                        {
                            for (int j = 1; j <= 9; j++)
                            {
                                //TiempoIntermedio = (TiempoTranscurrido - t) + j * (t / 10);
                                TiempoIntermedio = j * (t / 10);

                                if (ley_func == "Exponencial") maxY_new = Math.Exp(-ley_func_param2 * TiempoIntermedio) - 1 + maximoY;
                                if (ley_func == "Weibull2P") maxY_new = Math.Exp(-Math.Pow(TiempoIntermedio / ley_func_param2, ley_func_param1)) - 1 + maximoY;
                                Datos_a_representar.Add(new PointF((float)(TiempoTranscurrido - t + TiempoIntermedio), (float)maxY_new));
                            }
                        }
                    }
                    //Dibujar Punto final de la bajada de Disponibilidad (inicial de la bajada al fallo) (depende de la lay de Confiabilidad y no del maximoY)
                    Datos_a_representar.Add(new PointF((float)TiempoTranscurrido, (float)PuntoFinalBajadaDisponibilidad));

                    //Punto final de la bajada a fallo
                    Datos_a_representar.Add(new PointF((float)TiempoTranscurrido, (float)minimoY));

                    //Generar tiempo parado y acumularlo
                    if (ley_paro == "Uniforme") t = GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme(MinimoParado, MaximoParado, r);
                    if (ley_paro == "Exponencial") t = GeneradoresDeAleatorios.Generador_Aleatorio_Exponencial(ley_paro_param1, 1 / ley_paro_param2, MinimoParado, MaximoParado, r);
                    if (ley_paro == "Weibull2P") t = GeneradoresDeAleatorios.Generador_Aleatorio_Weibull_2P(ley_paro_param1, ley_paro_param2, MinimoParado, MaximoParado, r);
                    if (ley_paro == "Normal") t = GeneradoresDeAleatorios.Generador_Aleatorio_Normal(ley_paro_param1, ley_paro_param2, MinimoParado, MaximoParado, r);
                    TiempoParadoAcumulado += t;
                    TiempoParcialFallado = t;

                    //Actualizacion del tiempo
                    TiempoTranscurrido += t;

                    //Calcular el tiempo del ciclo (tiempo que dura el ciclo)
                    TiempoDelCiclo = TiempoParcialFallado + TiempoParcialFuncionando;

                    //Establecer el valor de la máxima recuperacion posible dado por la Ley de Recuperación
                    if (ley_recu == "Siempre a Nuevo (GAN)" || ley_recu == "Ninguna Ley") maximoY = 1;
                    if (ley_recu == "Según tiempo (BAO)" && ley_func == "Exponencial") maximoY = Math.Exp(-ley_func_param2 * (TiempoTranscurrido - ley_func_param1));
                    if (ley_recu == "Según tiempo (BAO)" && ley_func == "Weibull2P") maximoY = Math.Exp(-Math.Pow(TiempoTranscurrido / ley_func_param2, ley_func_param1));
                    if (ley_recu == "Exponencial") maximoY = Math.Exp(-ley_recu_param2 * (TiempoTranscurrido - ley_recu_param1));
                    if (ley_recu == "Weibull2P") maximoY = Math.Exp(-Math.Pow(TiempoTranscurrido / ley_recu_param2, ley_recu_param1));
                    if (ley_recu == "Línea recta") maximoY = Y1 + (Y2 - Y1) * (TiempoTranscurrido - X1) / (X2 - X1);
                    if(maximoY > 1 )maximoY = 1;


                    //Romper si la recuperacion maxima es menor o igual que la DisponibilidadMinimaAdmisible
                    //if (maximoY <= DisponibilidadMinimaAdmisible) break;

                    //Generar los costes de la recuperacion y de pérdida de producción
                    if (ley_coste == "Fijo por tiempo")
                    {
                        CosteEsteMantenimiento = CosteMantenimientoHr * t;
                        CosteEstaPerdidaDeProduccion = CostePerdidaProduccionHr * t;
                    }

                    if (ley_coste == "Fijo por intervención")
                    {
                        CosteEsteMantenimiento = CosteMantenimietoCadaIntervencion;
                        CosteEstaPerdidaDeProduccion = CostePerdidaProduccionHr * t;
                    }

                    if (ley_coste == "Lineal creciente")
                    {
                        CosteEsteMantenimiento = (YY1 + (YY2 - YY1) * (TiempoTranscurrido - XX1) / (XX2 - XX1)) * t;
                        CosteEstaPerdidaDeProduccion = CostePerdidaProduccionHr * t;
                    }

                    //Acumular costes derivados del paro/fallo
                    CosteTotalMantenimiento += CosteEsteMantenimiento;
                    CosteTotalPerdidaProduccion += CosteEstaPerdidaDeProduccion;

                    //CALCULO DE INDICADORES (SACAR A GRAFICAS TODOS LOS INDICADORES)
                    //---------------------------------------------------------------

                    //Calcular Disponibilidad
                    Disponibilidad = TiempoFuncionandoAcumulado / (TiempoFuncionandoAcumulado + TiempoParadoAcumulado);
                    
                    //Indicador Tiempo Medio Entre Fallos (MTBF)
                    AcumuladorTiemposEntreFallos += TiempoParcialFallado + TiempoParcialFuncionando;
                    MTBF = AcumuladorTiemposEntreFallos / ContadorCiclosFuncionaPara;

                    //Indicador Tiempo Medio de Reparacion (MTTR)
                    MTTR = TiempoParadoAcumulado / ContadorCiclosFuncionaPara;

                    //Indicador Intensidad de Fallos (IntensidadFallos)
                    IntensidadFallos = ContadorCiclosFuncionaPara / TiempoTranscurrido;

                    //Indicador Coste Acumulado de Recuperacion (Mto+Perdida de produccion)
                    CosteAcumuladoRecuperacion = CosteTotalMantenimiento + CosteTotalPerdidaProduccion;

                    //Indicador Coste Medio de Recuperacion (CosteMedioRecuperacion)
                    CosteMedioRecuperacion = (CosteEsteMantenimiento + CosteEstaPerdidaDeProduccion) / ContadorCiclosFuncionaPara;

                    //FIN DEL CALCULO DE INDICADORES
                    //-------------------------------


                    //Presentar los resultados numéricos en el TextBox de pantalla
                    textBox5.Text += " Ciclo número: " + ContadorCiclosFuncionaPara.ToString("0.") + "\r\n";
                    textBox5.Text += " ---------------------------------------------------------------------------------------------------------------------------------------------------" + "\r\n";
                    textBox5.Text += " t funcionando este ciclo = " + TiempoParcialFuncionando.ToString("0.##") + "      t parado este ciclo = " + TiempoParcialFallado.ToString("0.##") + "      t del ciclo = " + TiempoDelCiclo.ToString("0.##") + "\r\n";
                    textBox5.Text += " Tiempo Medio Entre Fallos (MTBF)= " + MTBF.ToString("0.##") + "\r\n";
                    textBox5.Text += " Tiempo total simulado = " + TiempoTranscurrido.ToString("0.##") + "       Tiempo total funcionando =  " + TiempoFuncionandoAcumulado.ToString("0.##") + "\r\n";
                    textBox5.Text += " Disponibilidad Maxima Alcanzada en el Ciclo = " + maximoY.ToString("0.######") + "\r\n";
                    textBox5.Text += " Disponibilidad Operacional= " + Disponibilidad.ToString("0.######") + "\r\n";
                    textBox5.Text += " Intensidad de Fallos = " + IntensidadFallos.ToString("0.##########") + "\r\n";
                    textBox5.Text += " Coste Mto este ciclo = " + CosteEsteMantenimiento.ToString("0.##") + "     Coste Perdida de producción este ciclo = " + CosteEstaPerdidaDeProduccion.ToString("0.##") + "\r\n";
                    textBox5.Text += " Coste acumulado Recuperación (Mto+PérdidaProd) = " + CosteAcumuladoRecuperacion.ToString("0.##") + "\r\n";
                    textBox5.Text += " Coste Medio de Recuperacion = " + CosteMedioRecuperacion.ToString("0.##") + "\r\n";
                    textBox5.Text += " Tiempo Medio de Reparacion (MTTR) = " + MTTR.ToString("0.##") + "\r\n";
                    textBox5.Text += "  " + "\r\n";
                    textBox5.Text += "  " + "\r\n";

                } while (TiempoFuncionandoAcumulado + TiempoParadoAcumulado <= Tiempo_A_Simular);


                //Terminar de rellanar la barra de progreso de los calculos
                progressBar1.Value = progressBar1.Maximum;
                    
                //linea = Formato(t,4,12) +" ";
                //linea = string.Format("{0,15}", t.ToString("F4")) + " ";                 
                //linea += string.Format("{0,15}", t.ToString("F4")) + " ";                  
                //linea += TiempoFuncionandoAcumulado.ToString("F4") + " ";
                //linea += string.Format("{0,15}", TiempoFuncionandoAcumulado.ToString("F4")) + " ";
                //linea += Convert.ToString(TiempoParadoAcumulado) + " ";
                //linea += string.Format("{0,15}", TiempoParadoAcumulado.ToString("F4")) + " ";                    
                //linea += Convert.ToString(TiempoFuncionandoAcumulado + TiempoParadoAcumulado)+ " ";
                //linea += string.Format("{0,15}", (TiempoFuncionandoAcumulado + TiempoParadoAcumulado).ToString("F4")) + " ";           

                //Lanzar formulario flotante con la gráfica
                Form4 frm = new Form4();
                frm.Datos_a_dibujar = Datos_a_representar;
                frm.rotulo_eje_x = "tiempo de simulacion";
                frm.rotulo_eje_y = "Disponibilidad Operacional";
                frm.rotulo_primer_dato = "Disponibilidad";
                frm.primer_dato = Disponibilidad;
                frm.rotulo_segundo_dato = "Coste";
                frm.segundo_dato = CosteTotalMantenimiento + CosteTotalPerdidaProduccion;
                frm.Show();

            }//del if
            }
            catch
            {
                MessageBox.Show("Se ha producido un error en los datos de entrada", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Funcionamiento
            comboBox1.Text = "Exponencial";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox1.Text = "0";
            textBox2.Text = "0,01";
            textBox3.Text = "10";
            textBox4.Text = "200";
            label11.Text = "Gamma";
            label3.Text = "Lambda";

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
            textBox6.Text = "30";
            textBox7.Text = "4";
            textBox8.Text = "5";
            textBox9.Text = "50";
            label7.Text = "Media";
            label8.Text = "Desv. Típica";

            //Tiempo a simular y repeticiones de la simulación
            textBox10.Text = "1000";


            //Recuperacion
            comboBox3.Text = "Exponencial";
            textBox12.Text = "0";
            textBox13.Text = "0,0001";
            textBox14.Text = "1000";
            textBox15.Text = "100000";
            textBox12.Enabled = true;
            textBox13.Enabled = true;
            textBox14.Enabled = true;
            textBox15.Enabled = true;
            label14.Text = "Gamma";
            label15.Text = "Lambda";
            label18.Text = "Mínimo Admisible";
            label19.Text = "Máximo Admisible";


            //Costes
            comboBox4.Text = "Fijo por tiempo";
            textBox16.Text = "100";
            textBox17.Text = "30";
            textBox18.Text = "";
            textBox19.Text = "";
            textBox20.Text = "";
            textBox16.Enabled = true;
            textBox17.Enabled = true;
            textBox18.Enabled = false;
            textBox19.Enabled = false;
            textBox20.Enabled = false;
            label21.Text = "Coste Mto/hr";
            label22.Text = "Coste prod/hr";
            label23.Text = "Parámetro 3";
            label24.Text = "Parámetro 4";
            label25.Text = "Parámetro 5";


        }

        //Entradas de usuario sobre la Ley de Funcionamiento
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Ninguna Ley")
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                label11.Text = "Parámetro 1";
                label3.Text = "Parámetro 2";
            }

            if (comboBox1.Text == "Exponencial")
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                label11.Text = "Gamma";
                label3.Text = "Lambda";
            }

            if (comboBox1.Text == "Weibull2P")
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                label11.Text = "Beta";
                label3.Text = "Eta";
            }


            if (comboBox1.Text == "Uniforme")
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                label11.Text = "Parámetro 1";
                label3.Text = "Parámetro 2";
            }
        }



        //Entradas de usuario sobre la Ley de Parada
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

            if (comboBox2.Text == "Weibull2P")
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
                label8.Text = "Desv. Típica";
            }

            if (comboBox2.Text == "Uniforme")
            {
                textBox6.Text = "";
                textBox7.Text = "";
                textBox8.Text = "";
                textBox9.Text = "";
                textBox6.Enabled = false;
                textBox7.Enabled = false;
                textBox8.Enabled = true;
                textBox9.Enabled = true;
                label7.Text = "Parámetro 1";
                label8.Text = "Parámetro 2";
            }
        }

        //Captura de indicaciones de usuario sobre ley de recuperacion
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.Text == "Ninguna Ley")
            {
                textBox12.Text = "";
                textBox13.Text = "";
                textBox14.Text = "";
                textBox15.Text = "";
                textBox12.Enabled = false;
                textBox13.Enabled = false;
                textBox14.Enabled = false;
                textBox15.Enabled = false;
                label14.Text = "Parámetro 1";
                label15.Text = "Parámetro 2";
                label18.Text = "Mínimo Admisible";
                label19.Text = "Máximo Admisible";

            }

            if (comboBox3.Text == "Siempre a Nuevo (GAN)")
            {
                textBox12.Text = "";
                textBox13.Text = "";
                textBox14.Text = "";
                textBox15.Text = "";
                textBox12.Enabled = false;
                textBox13.Enabled = false;
                textBox14.Enabled = false;
                textBox15.Enabled = false;
                label14.Text = "Parámetro 1";
                label15.Text = "Parámetro 2";
                label18.Text = "Mínimo Admisible";
                label19.Text = "Máximo Admisible";

            }

            if (comboBox3.Text == "Según tiempo (BAO)")
            {
                textBox12.Text = "";
                textBox13.Text = "";
                textBox14.Text = "";
                textBox15.Text = "";
                textBox12.Enabled = false;
                textBox13.Enabled = false;
                textBox14.Enabled = false;
                textBox15.Enabled = false;
                label14.Text = "Parámetro 1";
                label15.Text = "Parámetro 2";
                label18.Text = "Mínimo Admisible";
                label19.Text = "Máximo Admisible";
            }

            if (comboBox3.Text == "Línea recta")
            {
                textBox12.Text = "";
                textBox13.Text = "";
                textBox14.Text = "";
                textBox15.Text = "";
                textBox12.Enabled = true;
                textBox13.Enabled = true;
                textBox14.Enabled = true;
                textBox15.Enabled = true;
                label14.Text = "Inicial tiempo";
                label15.Text = "Inicial Disp.";
                label18.Text = "Final tiempo";
                label19.Text = "Final Disp.";
            }

            if (comboBox3.Text == "Exponencial")
            {
                textBox12.Text = "";
                textBox13.Text = "";
                textBox14.Text = "";
                textBox15.Text = "";
                textBox12.Enabled = true;
                textBox13.Enabled = true;
                textBox14.Enabled = true;
                textBox15.Enabled = true;
                label14.Text = "Gamma";
                label15.Text = "Lambda";
                label18.Text = "Mínimo Admisible";
                label19.Text = "Máximo Admisible";
            }

            if (comboBox3.Text == "Weibull2P")
            {
                textBox12.Text = "";
                textBox13.Text = "";
                textBox14.Text = "";
                textBox15.Text = "";
                textBox12.Enabled = true;
                textBox13.Enabled = true;
                textBox14.Enabled = true;
                textBox15.Enabled = true;
                label14.Text = "Beta";
                label15.Text = "Eta";
                label18.Text = "Mínimo Admisible";
                label19.Text = "Máximo Admisible";
            }


        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.Text == "Ninguna Ley")
            {
                textBox16.Text = "";
                textBox17.Text = "";
                textBox18.Text = "";
                textBox19.Text = "";
                textBox20.Text = "";
                textBox16.Enabled = false;
                textBox17.Enabled = false;
                textBox18.Enabled = false;
                textBox19.Enabled = false;
                textBox20.Enabled = false;
                label21.Text = "Parámetro 1";
                label22.Text = "Parámetro 2";
                label23.Text = "Parámetro 3";
                label24.Text = "Parámetro 4";
                label25.Text = "Parámetro 5";
            }

            if (comboBox4.Text == "Fijo por tiempo")
            {
                textBox16.Text = "";
                textBox17.Text = "";
                textBox18.Text = "";
                textBox19.Text = "";
                textBox20.Text = "";
                textBox16.Enabled = true;
                textBox17.Enabled = true;
                textBox18.Enabled = false;
                textBox19.Enabled = false;
                textBox20.Enabled = false;
                label21.Text = "Coste Mto/hr";
                label22.Text = "Coste prod/hr";
                label23.Text = "Parámetro 3";
                label24.Text = "Parámetro 4";
                label25.Text = "Parámetro 5";
            }

            if (comboBox4.Text == "Fijo por intervención")
            {
                textBox16.Text = "";
                textBox17.Text = "";
                textBox18.Text = "";
                textBox19.Text = "";
                textBox20.Text = "";
                textBox16.Enabled = true;
                textBox17.Enabled = true;
                textBox18.Enabled = false;
                textBox19.Enabled = false;
                textBox20.Enabled = false;
                label21.Text = "Coste interv.";
                label22.Text = "Coste prod/hr";
                label23.Text = "Parámetro 3";
                label24.Text = "Parámetro 4";
                label25.Text = "Parámetro 5";
            }


            if (comboBox4.Text == "Lineal creciente")
            {
                textBox16.Text = "";
                textBox17.Text = "";
                textBox18.Text = "";
                textBox19.Text = "";
                textBox20.Text = "";
                textBox16.Enabled = true;
                textBox17.Enabled = true;
                textBox18.Enabled = true;
                textBox19.Enabled = true;
                textBox20.Enabled = true;
                label21.Text = "Inicio: tiempo";
                label22.Text = "Inicio: costeMto/hr";
                label23.Text = "Fin: tiempo";
                label24.Text = "Fin: costeMto/hr";
                label25.Text = "Coste prod/hr";
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //TextBox de resultados
            textBox5.Text = "";
            textBox5.Enabled = false;
            
            //Funcionamiento
            comboBox1.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            label11.Text = "Parámetro 1";
            label3.Text = "Parámetro 2";

            //Parada-fallo
            comboBox2.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            textBox8.Enabled = false;
            textBox9.Enabled = false;
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            label7.Text = "Parámetro 1";
            label8.Text = "Parámetro 2";

            //Tiempo a simular y repeticiones de la simulación
            textBox10.Text = "";


            //Recuperacion
            comboBox3.Text = "";
            textBox12.Text = "";
            textBox13.Text = "";
            textBox14.Text = "";
            textBox15.Text = "";
            textBox12.Enabled = false;
            textBox13.Enabled = false;
            textBox14.Enabled = false;
            textBox15.Enabled = false;
            label14.Text = "Parámetro 1";
            label15.Text = "Parámetro 2";
            label18.Text = "Mínimo Admisible";
            label19.Text = "Máximo Admisible";


            //Costes
            comboBox4.Text = "";
            textBox16.Text = "";
            textBox17.Text = "";
            textBox18.Text = "";
            textBox19.Text = "";
            textBox20.Text = "";
            textBox16.Enabled = false;
            textBox17.Enabled = false;
            textBox18.Enabled = false;
            textBox19.Enabled = false;
            textBox20.Enabled = false;
            label21.Text = "Parámetro 1";
            label22.Text = "Parámetro 2";
            label23.Text = "Parámetro 3";
            label24.Text = "Parámetro 4";
            label25.Text = "Parámetro 5";

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
        private int ChequeoTextboxCicloFuncionamieontoParada(ComboBox CBOX1, ComboBox CBOX2, ComboBox CBOX3, ComboBox CBOX4, TextBox TBOX1, TextBox TBOX2, TextBox TBOX3, TextBox TBOX4
        , TextBox TBOX5, TextBox TBOX6, TextBox TBOX7, TextBox TBOX8, TextBox TBOX9, TextBox TBOX10, TextBox TBOX11, TextBox TBOX12, TextBox TBOX13
        , TextBox TBOX14, TextBox TBOX15, TextBox TBOX16, TextBox TBOX17, TextBox TBOX18)
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
                        else if (Convert.ToDouble(TBOX2.Text) > 1)
                        {
                            MessageBox.Show("Valor de Lambda elevado en FUNCIONAMIENTO (<=1)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                case "Weibull2P":
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

            /******************************************************   FALLO   *************************************************************/
            switch (CBOX2.Text)
            {
                case "Exponencial":
                    {
                        if (Convert.ToDouble(TBOX5.Text) > 500000)
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
                            MessageBox.Show("Valores maximos y minimos admisibles erroneos en FALLO", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if ((Convert.ToDouble(TBOX8.Text) > 500000) || Convert.ToDouble(TBOX7.Text) > 500000)
                        {
                            MessageBox.Show("Valor maximo o minimo admisible elevado en FALLO  (<=500.000)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            }

            /****************************************************   RECUPERACIÓN   *************************************************/
            switch (CBOX3.Text)
            {
                case "Siempre a Nuevo (GAN)":
                    {

                    }
                    break;
                case "Según tiempo (BAO)":
                    {

                    }
                    break;
                case "Línea recta":
                    {

                    }
                    break;

                case "Exponencial":
                    {
                        if (Convert.ToDouble(TBOX9.Text) > 500000)
                        {
                            MessageBox.Show("Valor de Gamma elevado en RECUPERACIÓN (<=500000)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if (Convert.ToDouble(TBOX10.Text) > 1)
                        {
                            MessageBox.Show("Valor de Lambda elevado en RECUPERACIÓN (<=1)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if ((Convert.ToDouble(TBOX11.Text) > 1 / Convert.ToDouble(TBOX10.Text)) || (Convert.ToDouble(TBOX12.Text) < 1 / Convert.ToDouble(TBOX10.Text)))
                        {
                            MessageBox.Show("Valor de Lambda erróneo en RECUPERACIÓN: 1/Lambda ha de estar dentro del rango [Mínimo Admisible, Maximo Admisible]", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }

                        else if ((Convert.ToDouble(TBOX12.Text) - Convert.ToDouble(TBOX11.Text)) < 0)
                        {
                            MessageBox.Show("Valores maximos y minimos admisibles erroneos en RECUPERACIÓN ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if ((Convert.ToDouble(TBOX12.Text) > 500000) || Convert.ToDouble(TBOX11.Text) > 500000)
                        {
                            MessageBox.Show("Valor maximo o minimo admisible elevado en RECUPERACIÓN (<=500.000)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                    }

                    break;
                case "Weibull2P":
                    {
                        if (Convert.ToDouble(TBOX9.Text) > 10)
                        {
                            MessageBox.Show("Valor de Beta elevado en RECUPERACIÓN (<=10)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if (Convert.ToDouble(TBOX10.Text) > 500000)
                        {
                            MessageBox.Show("Valor de Eta elevado en RECUPERACIÓN (<=500000)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if (Convert.ToDouble(TBOX11.Text) > Convert.ToDouble(TBOX10.Text))
                        {
                            MessageBox.Show("Valor de Eta menor que el minimo admisible en RECUPERACIÓN ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if (Convert.ToDouble(TBOX12.Text) < Convert.ToDouble(TBOX10.Text))
                        {
                            MessageBox.Show("Valor de Eta mayor que el maximo admisible en RECUPERACIÓN ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }

                        else if ((Convert.ToDouble(TBOX12.Text) - Convert.ToDouble(TBOX11.Text)) < 0)
                        {
                            MessageBox.Show("Valores maximos y minimos admisibles erroneos en RECUPERACIÓN ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                        else if ((Convert.ToDouble(TBOX12.Text) > 500000) || Convert.ToDouble(TBOX11.Text) > 500000)
                        {
                            MessageBox.Show("Valor maximo o minimo admisible elevado en RECUPERACIÓN (<=500.000)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            valor = 1;
                        }
                    }
                    break;
            }

            /****************************************************   COSTE   ********************************************************/
            switch (CBOX4.Text)
            {
                case "Fijo por tiempo":
                    {

                    }
                    break;
                case "Fijo por intervención":
                    {

                    }
                    break;
                case "Lineal creciente":
                    {

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
