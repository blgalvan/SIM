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
    public partial class FormFuncionaFallaAmpliado : Form
    {

        public double DiponibilidadOperacionalFinal;
        public double CosteRecuperacionFinal;

        public string rotulo_primer_dato;
        public string rotulo_segundo_dato;
        public double primer_dato;
        public double segundo_dato;

        public static string TituloDelFormulario;

        Dictionary<string, double> parametros = new Dictionary<string, double>();
        Dictionary<string, string> nombres = new Dictionary<string, string>();
        Dictionary<string, double> resultados = new Dictionary<string, double>();
        Dictionary<double, double> programa_mto_preventivo = new Dictionary<double, double>();

        /// <summary>
        /// Este Diccionario contiene los terminos a eliminar de cualquiera de los diccionarios
        /// el primer sting contiene el nombre del diccionario
        /// el segundo string contiene el término a eliminar
        /// </summary>
        Dictionary<string, string> terminos_a_eliminar_en_diccionarios = new Dictionary<string, string>();

        //Crear e inicializar la lista que contendrá los puntos de cada gráfica   
        List<PointF> Lista_Disponibilidad = new List<PointF>();
        List<PointF> Lista_MTBF = new List<PointF>();
        List<PointF> Lista_MTTR = new List<PointF>();
        List<PointF> Lista_IntensidadFallos = new List<PointF>();
        List<PointF> Lista_CosteAcumuladoRecuperacion = new List<PointF>();
        List<PointF> Lista_CosteMedioRecuperacion = new List<PointF>();
        List<PointF> Lista_RatioCorrectivo_vs_total = new List<PointF>();
        List<PointF> Lista_RatioPreventivo_vs_total = new List<PointF>();
        List<PointF> Lista_MTTR_SinLog = new List<PointF>();
        List<PointF> Lista_RatioMTTR_vs_MTTR_SinLog = new List<PointF>();
        List<PointF> Lista_Numero_de_Fallos = new List<PointF>();
        List<PointF> Lista_Numero_de_Preventivos = new List<PointF>();

        public FormFuncionaFallaAmpliado()
        {
            InitializeComponent();
            //Aseguramos que utiliza la configuración española para numeros decimales
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-ES");
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Este método permite eliminar términos contenidos en los diccionarios "nombres" y "parametros"
        /// para ello usa el diccionario denominado "terminos_a_eliminar_en_diccionarios" que contiene
        /// parejas <string1,string2>.
        /// 
        /// "string2" indica del nombre del diccionario pudiera encontrarse el contenido a eliminar, en
        /// este caso "string2" solo puede contener "nombres" o "parametros" que son los nombres de los
        /// dos diccionarios que podría ser necesario limpiar
        /// 
        /// "string1" indica la clave (key) que identifica al termino a eliminar
        /// 
        /// NOTA: la clave indicada en "string1" pudiera no estar en el diccionario
        /// NOTA: seria ideal poder generalizar el los diccionarios, puede que para ello sea necesario
        /// usar a su vez un "diccionario de nombres de diccionarios", pero eso se ha dejado para una
        /// siguiente versión
        /// </summary>
        private void limpiar_diccionarios()
        {
            string auxi1;
            string auxi2;
            foreach (string key in terminos_a_eliminar_en_diccionarios.Keys)
            {
                auxi1 = key; //Extrae el nombre del término a eliminar
                auxi2 = terminos_a_eliminar_en_diccionarios[key];  //Extrae la key que indica si el nombre del diccionario sobre el que se actuará 
               
                switch (auxi2)
                {
                    case  "nombres":
                        nombres.Remove(auxi1);
                        break;

                    case "parametros":
                        parametros.Remove(auxi1);
                        break;
                }   
            }

            //limpiar el diccionario de terminos a eliminar pues ya han sido eliminados
            terminos_a_eliminar_en_diccionarios.Clear();

        }
        
        //SIMULAR
        private void button4_Click(object sender, EventArgs e)
        {
            //Inicializar el Generador de Aleatorios con semilla-reloj
            Random r = new Random(DateTime.Now.Millisecond);

            //Declaraciones de variables necesarias
            double TiempoFuncionandoAcumulado = 0;
            double TiempoParadoAcumulado = 0;
            double t = 0;
            double Disponibilidad;
            double TiempoTranscurrido = 0;
            double minimoY = 0;
            double maximoY = 1;
            double TiempoParcialFuncionando;
            double TiempoParadoParcial;
            double TiempoPreventivoAcumulado = 0;
            double TiempoCorrectivoAcumulado = 0;
            double PuntoFinalBajadaDisponibilidad = 0;
            double DisponibilidadMinimaAdmisible = 0;
            double CosteTotalMantenimiento = 0;
            double CosteTotalPerdidaProduccion = 0;
            double CosteAcumuladoRecuperacion = 0;
            double CosteMedioRecuperacion = 0;
            double CosteEsteMantenimiento = 0;
            double CosteEstaPerdidaDeProduccion = 0;
            int ContadorCiclosFuncionaPara = 0;
            double AcumuladorTiemposEntreFallos = 0;
            double ContadorTiempoDisponibilidadPositiva;
            double Numero_de_Preventivos = 0;
            double Numero_de_Correctivos = 0;
            double MTBF = 0;
            double MTTR = 0;
            double RatioMTTR_vs_MTTR_SinLog = 0;
            double MTTR_SinLog = 0;
            double TiempoDeLogistica = 0;
            double IntensidadFallos = 0;
            double TiempoDelCiclo = 0;
            double Tiempo_A_Simular = 0;
            double t_paro_recon=0;
            double t_paro_diag=0;
            double t_paro_prep=0;
            double t_paro_desm=0;
            double t_paro_repa=0;
            double t_paro_ensam=0;
            double t_paro_verif=0;
            double t_paro_serv=0;
            string clave;
            double coste_recon = 0;
            double coste_diag=0;
            double coste_prep=0;
            double coste_desm=0;
            double coste_repa=0;
            double coste_ensam=0;
            double coste_verif=0;
            double coste_serv=0;
            double coste_prod_recon=0;
            double coste_prod_diag=0;
            double coste_prod_prep=0;
            double coste_prod_desm=0;
            double coste_prod_repa=0;
            double coste_prod_ensam=0;
            double coste_prod_verif=0;
            double coste_prod_serv=0;

            double Ratio_Correctivo_vs_Total = 0;
            double Ratio_Preventivo_vs_Total = 0;
            double TiempoHastaSiguientePreventivo = 0;
            double EficienciaMto=100;

            double rnd1;

            string tipo_mto_este_ciclo; //indica en cada ciclo si se va arealizar un "Correctivo" o un "Preventivo"

            //Limpiar las listas que contendrán los puntos de cada gráfica   
            Lista_Disponibilidad.Clear();
            Lista_MTBF.Clear();
            Lista_MTTR.Clear();
            Lista_IntensidadFallos.Clear();
            Lista_CosteAcumuladoRecuperacion.Clear();
            Lista_CosteMedioRecuperacion.Clear();
            Lista_RatioCorrectivo_vs_total.Clear();
            Lista_RatioPreventivo_vs_total.Clear();
            Lista_MTTR_SinLog.Clear();
            Lista_RatioMTTR_vs_MTTR_SinLog.Clear();
            Lista_Numero_de_Fallos.Clear();
            Lista_Numero_de_Preventivos.Clear();
            
            //Añadir un primer punto ficticio aleatorio a todas las listas para que no de error el graficador si todos los valores a grafica son iguales
            rnd1 = GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme(0.0001, 0.001, r);
            Lista_Disponibilidad.Add(new PointF((float)0.0, (float)rnd1));

            rnd1 = GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme(0.0001, 0.001, r);
            Lista_MTBF.Add(new PointF((float)0.0, (float)rnd1));

            rnd1 = GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme(0.0001, 0.001, r);
            Lista_MTTR.Add(new PointF((float)0.0, (float)rnd1));

            rnd1 = GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme(0.0001, 0.001, r);
            Lista_IntensidadFallos.Add(new PointF((float)0.0, (float)rnd1));

            rnd1 = GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme(0.0001, 0.001, r);
            Lista_CosteAcumuladoRecuperacion.Add(new PointF((float)0.0, (float)rnd1));

            rnd1 = GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme(0.0001, 0.001, r);
            Lista_CosteMedioRecuperacion.Add(new PointF((float)0.0, (float)rnd1));

            rnd1=GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme(0.0001, 0.001, r);
            Lista_RatioCorrectivo_vs_total.Add(new PointF((float)0.0, (float)rnd1));

            rnd1 = GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme(0.0001, 0.001, r);
            Lista_RatioPreventivo_vs_total.Add(new PointF((float)0.0, (float)rnd1));

            rnd1 = GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme(0.0001, 0.001, r);
            Lista_MTTR_SinLog.Add(new PointF((float)0.0, (float)rnd1));

            rnd1 = GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme(0.0001, 0.001, r);
            Lista_RatioMTTR_vs_MTTR_SinLog.Add(new PointF((float)0.0, (float)rnd1));

            rnd1 = GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme(0.0001, 0.001, r);
            Lista_Numero_de_Fallos.Add(new PointF((float)0.0, (float)rnd1));

            rnd1 = GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme(0.0001, 0.001, r);
            Lista_Numero_de_Preventivos.Add(new PointF((float)0.0, (float)rnd1));

            //Limpiar Diccionarios de "resultados" y de "programa de mantenimiento preventivo" ya que son salidas del proceso de simulación
            resultados.Clear();
            programa_mto_preventivo.Clear();

            //Captura del tiempo a simular indicado por el usuario
            double auxi9 = Convert.ToDouble(textBox10.Text);
            if (auxi9 >0 ) Tiempo_A_Simular = auxi9;

            //Fijar el tamaño máximo de la barra de seguimiento y ponerla a cero
            progressBar1.Maximum=100;
            progressBar1.Value = 0;

            //Borrar el contenido del textbox de salida de resultados y encabezarlo con los datos de partida
            textBox5.Enabled = true;
            textBox5.Text = "";
            textBox5.Text += "\r\n";
            textBox5.Text += "\r\n" + "------------------- DATOS DE PARTIDA -------------------------";
            textBox5.Text += "\r\n";
            foreach (string key in nombres.Keys)
            {
                textBox5.Text += "\r\n" + key + " = " + nombres[key].ToString();
            }
            textBox5.Text += "\r\n";
            foreach (string key in parametros.Keys)
            {
                textBox5.Text += "\r\n" + key + " = " + parametros[key].ToString();
            }
            textBox5.Text += "\r\n" + "Tiempo a Simular = " + Tiempo_A_Simular.ToString();
            textBox5.Text += "\r\n";
            textBox5.Text += "\r\n" + "-------------- FIN DE LOS DATOS DE PARTIDA -------------------";
            textBox5.Text += "\r\n";
            textBox5.Enabled = false;

            //Deshabilitar el combobox de ver gráficas hasta que finalicen los cálculos
            DeshabilitarComboBoxVerGraficas();

            //Poner aqui el umbral de disponibilidad si esta activado el preventivo por disponibilidad
            if (nombres.ContainsKey("preventivo") && nombres.ContainsKey("tipo_de_preventivo"))
            {
                if (nombres["preventivo"] == "Activado" && nombres["tipo_de_preventivo"] == "Por Disponibilidad")
                {
                    DisponibilidadMinimaAdmisible = parametros["disponibilidad_minima_admisible"] / 100;
                }

                if (nombres["preventivo"] == "Activado" && nombres["tipo_de_preventivo"] == "Fijo por tiempo")
                {
                    TiempoHastaSiguientePreventivo = parametros["tiempo_entre_preventivos"];
                }
            }

            //GRAN BUCLE QUE REALIZA la SIMULACIÓN
            do
            {
                 //Por defecto se supone que el mantenimiento a realizar en el ciclo actual es "Correctivo" 
                tipo_mto_este_ciclo = "Correctivo";
                
                 //Incrementar el contador de ciclos funciona-para
                 ContadorCiclosFuncionaPara += 1;

                 //A)GENERAR TIEMPO DE FUNCIONAMIENTO, TRATARLO Y ACUMULARLO
                 //Puntos inicial y final de la subida a funcionamiento
                 Lista_Disponibilidad.Add(new PointF((float)TiempoTranscurrido, (float)minimoY));
                 Lista_Disponibilidad.Add(new PointF((float)TiempoTranscurrido, (float)maximoY));

                 //Generar tiempo funcionando, controlar su validez y acumularlo
                 if (nombres["ley_func"] == "Uniforme") t = GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme(parametros["Minimo_func"], parametros["Maximo_func"], r);
                 if (nombres["ley_func"] == "Exponencial") t = GeneradoresDeAleatorios.Generador_Aleatorio_Exponencial(parametros["ley_func_param1"], 1 / parametros["ley_func_param2"], parametros["Minimo_func"], parametros["Maximo_func"], r);
                 if (nombres["ley_func"] == "Weibull2P") t = GeneradoresDeAleatorios.Generador_Aleatorio_Weibull_2P(parametros["ley_func_param1"], parametros["ley_func_param2"], parametros["Minimo_func"], parametros["Maximo_func"], r);
                 if (nombres["ley_func"] == "Normal") t = GeneradoresDeAleatorios.Generador_Aleatorio_Normal(parametros["ley_func_param1"], parametros["ley_func_param2"], parametros["Minimo_func"], parametros["Maximo_func"], r);

                 //En caso de estar activado el Mto Preventivo fijo por tiempo controlar si el tiempo de funcionamiento de este ciclo excede al tiempo restante para el siguiente Mto preventivo
                 if (nombres.ContainsKey("preventivo") && nombres.ContainsKey("tipo_de_preventivo"))
                 {
                     if (nombres["preventivo"] == "Activado" && nombres["tipo_de_preventivo"] == "Fijo por tiempo" && TiempoHastaSiguientePreventivo < t)
                     {
                         t = TiempoHastaSiguientePreventivo;
                         tipo_mto_este_ciclo = "Preventivo";
                         TiempoHastaSiguientePreventivo = parametros["tiempo_entre_preventivos"];
                     }
                 }
                
                 //bajada de disponibilidad durante el periodo de funcionamiento, solo Exponencial y Weubull tienen sentido
                 //Para las otras leyes se opta por no bajar la disponibilidad
                 if (nombres["ley_func"] == "Exponencial") PuntoFinalBajadaDisponibilidad = Math.Exp(-parametros["ley_func_param2"] * t) - 1 + maximoY;
                 if (nombres["ley_func"] == "Weibull2P") PuntoFinalBajadaDisponibilidad = Math.Exp(-Math.Pow(t / parametros["ley_func_param2"], parametros["ley_func_param1"])) - 1 + maximoY;
                 if (nombres["ley_func"] == "Uniforme" || nombres["ley_func"] == "Normal") PuntoFinalBajadaDisponibilidad = maximoY;

                 //Controlar la validez de t ==> encontrar el t que hace cero la disponibilidad (si procede)
                 ContadorTiempoDisponibilidadPositiva = 0;
                 if (PuntoFinalBajadaDisponibilidad <= DisponibilidadMinimaAdmisible)
                 {
                      double DisponibilidadInstantanea = -1000;
                      for (int j = 1; j <= t; j++)
                      {
                          if (nombres["ley_func"] == "Exponencial") DisponibilidadInstantanea = Math.Exp(-parametros["ley_func_param2"] * j) - 1 + maximoY;
                          if (nombres["ley_func"] == "Weibull2P") DisponibilidadInstantanea = Math.Exp(-Math.Pow(j / parametros["ley_func_param2"], parametros["ley_func_param1"])) - 1 + maximoY;
                           if (DisponibilidadInstantanea <= DisponibilidadMinimaAdmisible) break;
                           ContadorTiempoDisponibilidadPositiva += 1;
                      }
                           PuntoFinalBajadaDisponibilidad = DisponibilidadMinimaAdmisible;
                           t = ContadorTiempoDisponibilidadPositiva;
                           if (nombres.ContainsKey("preventivo") && nombres.ContainsKey("tipo_de_preventivo"))
                           {
                               if (nombres["preventivo"] == "Activado" && nombres["tipo_de_preventivo"] == "Por Disponibilidad") tipo_mto_este_ciclo = "Preventivo";
                           }
                  }

                  //Acumular tiempos funcionando
                  TiempoFuncionandoAcumulado += t;
                  TiempoParcialFuncionando = t;

                  //Acumular el tiempo de funcionamiento al tiempo total simulado o "tiempo transcurrido"
                  TiempoTranscurrido += t;

                  //Si el mantenimiento en este ciclo es correctivo pero al preventivo Fijo por Tiempo está activado, decrementar el tiempo restante hasta el siguiente preventivo
                  if (nombres.ContainsKey("preventivo") && nombres.ContainsKey("tipo_de_preventivo"))
                  {
                      if (tipo_mto_este_ciclo == "Correctivo" && nombres["preventivo"] == "Activado" && nombres["tipo_de_preventivo"] == "Fijo por tiempo") TiempoHastaSiguientePreventivo -= t;
                  }


                  //Dibujar puntos intermedios para mejorar el aspecto de la gráfica
                  if (PuntoFinalBajadaDisponibilidad >= 0)
                  {
                      double TiempoIntermedio = 0;
                      double maxY_new = 0;
                      if (nombres["ley_func"] == "Exponencial" || nombres["ley_func"] == "Weibull2P")
                      {
                           for (int j = 1; j <= 9; j++)
                           {
                                //TiempoIntermedio = (TiempoTranscurrido - t) + j * (t / 10);
                                TiempoIntermedio = j * (t / 10);

                                if (nombres["ley_func"] == "Exponencial") maxY_new = Math.Exp(-parametros["ley_func_param2"] * TiempoIntermedio) - 1 + maximoY;
                                if (nombres["ley_func"] == "Weibull2P") maxY_new = Math.Exp(-Math.Pow(TiempoIntermedio / parametros["ley_func_param2"], parametros["ley_func_param1"])) - 1 + maximoY;
                                Lista_Disponibilidad.Add(new PointF((float)(TiempoTranscurrido - t + TiempoIntermedio), (float)maxY_new));
                            }
                       }
                   }
                
                   //Dibujar Punto final de la bajada de Disponibilidad (inicial de la bajada al fallo) (depende de la lay de Confiabilidad y no del maximoY)
                   Lista_Disponibilidad.Add(new PointF((float)TiempoTranscurrido, (float)PuntoFinalBajadaDisponibilidad));

                   //Punto final de la bajada a fallo
                   Lista_Disponibilidad.Add(new PointF((float)TiempoTranscurrido, (float)minimoY));                
                

                   //B)GENERAR TIEMPO DE FALLO/PARO, TRATARLO, ALMACENARLO Y ACUMULARLO
                   //------------------------------------------------------------------
                   
                   //B.1.-Generar tiempo de fallo/paro en el caso en que no exista desglose de tiempos de fallos/paradas
                   if (nombres["ley_paro"] != "Desglose de Fallos")
                   {
                       if (nombres["ley_paro"] == "Uniforme") t = GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme(parametros["Minimo_paro"], parametros["Maximo_paro"], r);
                       if (nombres["ley_paro"] == "Exponencial") t = GeneradoresDeAleatorios.Generador_Aleatorio_Exponencial(parametros["ley_paro_param1"], 1 / parametros["ley_paro_param2"], parametros["Minimo_paro"], parametros["Maximo_paro"], r);
                       if (nombres["ley_paro"] == "Weibull2P") t = GeneradoresDeAleatorios.Generador_Aleatorio_Weibull_2P(parametros["ley_paro_param1"], parametros["ley_paro_param2"], parametros["Minimo_paro"], parametros["Maximo_paro"], r);
                       if (nombres["ley_paro"] == "Normal") t = GeneradoresDeAleatorios.Generador_Aleatorio_Normal(parametros["ley_paro_param1"], parametros["ley_paro_param2"], parametros["Minimo_paro"], parametros["Maximo_paro"], r);
                   }
                   
                   //B.2.-Generar tiempo de fallo/paro en el caso en que exista desglose de tiempos
                   if (nombres["ley_paro"] == "Desglose de Fallos")
                   {                           
                       //Reconocimiento 
                       clave = "recon";
                       t_paro_recon = GenerarValor(TiempoTranscurrido, "ley_paro_" + clave, "ley_paro_" + clave + "_param1", "ley_paro_" + clave + "_param2", "Maximo_paro_" + clave, "Minimo_paro_" + clave, "X1_paro_" + clave, "Y1_paro_" + clave, "X2_paro_" + clave, "Y2_paro_" + clave, 1, r);

                       //Diagnostico 
                       clave = "diag";
                       t_paro_diag = GenerarValor(TiempoTranscurrido, "ley_paro_"+clave, "ley_paro_"+clave+"_param1", "ley_paro_"+clave+"_param2", "Maximo_paro_"+clave, "Minimo_paro_"+clave, "X1_paro_"+clave, "Y1_paro_"+clave, "X2_paro_"+clave, "Y2_paro_"+clave, 1, r);

                       //Preparacion 
                       clave = "prep";
                       t_paro_prep = GenerarValor(TiempoTranscurrido, "ley_paro_" + clave, "ley_paro_" + clave + "_param1", "ley_paro_" + clave + "_param2", "Maximo_paro_" + clave, "Minimo_paro_" + clave, "X1_paro_" + clave, "Y1_paro_" + clave, "X2_paro_" + clave, "Y2_paro_" + clave, 1, r);

                       //Desmantelamiento 
                       clave = "desm";
                       t_paro_desm = GenerarValor(TiempoTranscurrido, "ley_paro_" + clave, "ley_paro_" + clave + "_param1", "ley_paro_" + clave + "_param2", "Maximo_paro_" + clave, "Minimo_paro_" + clave, "X1_paro_" + clave, "Y1_paro_" + clave, "X2_paro_" + clave, "Y2_paro_" + clave, 1, r);

                       //Reparacion 
                       clave = "repa";
                       t_paro_repa = GenerarValor(TiempoTranscurrido, "ley_paro_" + clave, "ley_paro_" + clave + "_param1", "ley_paro_" + clave + "_param2", "Maximo_paro_" + clave, "Minimo_paro_" + clave, "X1_paro_" + clave, "Y1_paro_" + clave, "X2_paro_" + clave, "Y2_paro_" + clave, 1, r);

                       //Ensamblaje 
                       clave = "ensam";
                       t_paro_ensam = GenerarValor(TiempoTranscurrido, "ley_paro_" + clave, "ley_paro_" + clave + "_param1", "ley_paro_" + clave + "_param2", "Maximo_paro_" + clave, "Minimo_paro_" + clave, "X1_paro_" + clave, "Y1_paro_" + clave, "X2_paro_" + clave, "Y2_paro_" + clave, 1, r);

                       //Verificacion 
                       clave = "verif";
                       t_paro_verif = GenerarValor(TiempoTranscurrido, "ley_paro_" + clave, "ley_paro_" + clave + "_param1", "ley_paro_" + clave + "_param2", "Maximo_paro_" + clave, "Minimo_paro_" + clave, "X1_paro_" + clave, "Y1_paro_" + clave, "X2_paro_" + clave, "Y2_paro_" + clave, 1, r);

                       //Puesta en servicio 
                       clave = "serv";
                       t_paro_serv = GenerarValor(TiempoTranscurrido, "ley_paro_" + clave, "ley_paro_" + clave + "_param1", "ley_paro_" + clave + "_param2", "Maximo_paro_" + clave, "Minimo_paro_" + clave, "X1_paro_" + clave, "Y1_paro_" + clave, "X2_paro_" + clave, "Y2_paro_" + clave, 1, r);
                                              
                       //Se calcula ahora el tiempo fallado/parado como la suma de los tiempos del desglose
                       t = t_paro_recon + t_paro_diag + t_paro_prep + t_paro_desm + t_paro_repa + t_paro_ensam + t_paro_verif + t_paro_serv;

                       //Se calcula ahora el tiempo de Logistica identificado aqui como el tiempo de preparacion
                       TiempoDeLogistica = t_paro_prep;
                   }

                   //B.3.-Decidir si el Mantenimiento es "Correctivo" ó "Preventivo" en función de t (de fall/paro) y datos de entrada
                   //Si está activado el Preventivo por Disponibilidad no corresponde hacer nada aqui
                   if (nombres.ContainsKey("preventivo") && nombres.ContainsKey("tipo_de_preventivo"))
                   {
                       if (tipo_mto_este_ciclo == "Correctivo" && nombres["preventivo"] == "Activado" && nombres["tipo_de_preventivo"] == "Fijo por tiempo" && TiempoHastaSiguientePreventivo < t)
                       {
                           tipo_mto_este_ciclo = "Preventivo";
                           TiempoHastaSiguientePreventivo = parametros["tiempo_entre_preventivos"];
                       }
                   }
 
                   //B.4.-Corregir el "Tiempo hasta el siguiente preventivo" en caso de que el mto de este ciclo sea correctivo pero esté activado el preventivo "Fijo por Tiempo"
                   if (nombres.ContainsKey("preventivo") && nombres.ContainsKey("tipo_de_preventivo"))
                   {
                       if (tipo_mto_este_ciclo == "Correctivo" && nombres["preventivo"] == "Activado" && nombres["tipo_de_preventivo"] == "Fijo por tiempo" && TiempoHastaSiguientePreventivo > t) TiempoHastaSiguientePreventivo -= t;
                   }

                   //B.5.-Corregir los tiempos de fallo/parada en caso de que se este aplicando Preventivo sin desglose de tiempos
                   if (nombres.ContainsKey("ley_paro"))
                   {
                       if (tipo_mto_este_ciclo == "Preventivo" && nombres["ley_paro"] != "Desglose de Fallos" && parametros.ContainsKey("paro_Reduccion_si_Preventivo")) t = (100 - parametros["paro_Reduccion_si_Preventivo"]) * t / 100;
                   }                   
                   //OJO FALTA POR ARREGLAR QUE ESTE CARGADO EL parametros["paro_Reduccion_si_Preventivo"] EN EL CASO DE NO DESGLOSE DEL TIEMPO DE PARO, EL FORMULARIO NO LO PREGUNTA
                   
                   //B.6.-Corregir los tiempos de fallo/parada en caso de que se este aplicando Preventivo con desglose de tiempos
                   if (tipo_mto_este_ciclo == "Preventivo" && nombres["ley_paro"] == "Desglose de Fallos")
                   {
                       if (parametros.ContainsKey("paro_recon_Reduccion_si_Preventivo")) t_paro_recon = (100 - parametros["paro_recon_Reduccion_si_Preventivo"]) * t_paro_recon / 100;
                       if (parametros.ContainsKey("paro_diag_Reduccion_si_Preventivo")) t_paro_diag = (100 - parametros["paro_diag_Reduccion_si_Preventivo"]) * t_paro_diag / 100;
                       if (parametros.ContainsKey("paro_prep_Reduccion_si_Preventivo")) t_paro_prep = (100 - parametros["paro_prep_Reduccion_si_Preventivo"]) * t_paro_prep / 100;
                       if (parametros.ContainsKey("paro_desm_Reduccion_si_Preventivo")) t_paro_desm = (100 - parametros["paro_desm_Reduccion_si_Preventivo"]) * t_paro_desm / 100;
                       if (parametros.ContainsKey("paro_repa_Reduccion_si_Preventivo")) t_paro_repa = (100 - parametros["paro_repa_Reduccion_si_Preventivo"]) * t_paro_repa / 100;
                       if (parametros.ContainsKey("paro_ensam_Reduccion_si_Preventivo")) t_paro_ensam = (100 - parametros["paro_ensam_Reduccion_si_Preventivo"]) * t_paro_ensam / 100;
                       if (parametros.ContainsKey("paro_verif_Reduccion_si_Preventivo")) t_paro_verif = (100 - parametros["paro_verif_Reduccion_si_Preventivo"]) * t_paro_verif / 100;
                       if (parametros.ContainsKey("paro_serv_Reduccion_si_Preventivo")) t_paro_serv = (100 - parametros["paro_serv_Reduccion_si_Preventivo"]) * t_paro_serv / 100;

                       //Se recalcula ahora el tiempo fallado/parado como la suma de los tiempos del desglose modificados
                       t = t_paro_recon + t_paro_diag + t_paro_prep + t_paro_desm + t_paro_repa + t_paro_ensam + t_paro_verif + t_paro_serv;
                   }

                   //B.7.-Acumular tiempos en las diferentes variables
                   //Acumular y guaradar          
                   TiempoParadoAcumulado += t;
                   TiempoParadoParcial = t;

                   //Actualizacion del tiempo total transcurrido en la simulación
                   TiempoTranscurrido += t;

                   //Calcular el tiempo del ciclo (tiempo que dura el ciclo)
                   TiempoDelCiclo = TiempoParadoParcial + TiempoParcialFuncionando;

                   //Acumular tiempo de Mantenimiento a Correctivo o a Preventivo
                   if (tipo_mto_este_ciclo == "Preventivo") TiempoPreventivoAcumulado += t;
                   if (tipo_mto_este_ciclo == "Correctivo") TiempoCorrectivoAcumulado += t;
                  
                   
                
                   //C)ESTABLECER EL VALOR DE LA RECUPERACIÓN EN CASO DE QUE PROCEDA, TRATARLO Y USARLO
                   //Establecer el valor de la máxima recuperacion posible dado por la Ley de Recuperación
                   if (nombres.ContainsKey("ley_recu"))
                   {
                       if (nombres["ley_recu"] == "Siempre a Nuevo (GAN)" || nombres["ley_recu"] == "Ninguna Ley") maximoY = 1;
                       if (nombres["ley_recu"] == "Según tiempo (BAO)" && nombres["ley_func"] == "Exponencial") maximoY = Math.Exp(-parametros["ley_func_param2"] * (TiempoTranscurrido - parametros["ley_func_param1"]));
                       if (nombres["ley_recu"] == "Según tiempo (BAO)" && nombres["ley_func"] == "Weibull2P") maximoY = Math.Exp(-Math.Pow(TiempoTranscurrido / parametros["ley_func_param2"], parametros["ley_func_param1"]));
                       if (nombres["ley_recu"] == "Exponencial") maximoY = Math.Exp(-parametros["ley_recu_param2"] * (TiempoTranscurrido - parametros["ley_recu_param1"]));
                       if (nombres["ley_recu"] == "Weibull2P") maximoY = Math.Exp(-Math.Pow(TiempoTranscurrido / parametros["ley_recu_param2"], parametros["ley_recu_param1"]));
                       if (nombres["ley_recu"] == "Línea recta") maximoY = parametros["Y1_recu"] + (parametros["Y2_recu"] - parametros["Y1_recu"]) * (TiempoTranscurrido - parametros["X1_recu"]) / (parametros["X2_recu"] - parametros["X1_recu"]);
                       if (maximoY > 1) maximoY = 1;
                   }
                     
                   //Modificar el valor de la maxima recuperación si se usa el "% de eficiencia del Mantenimiento"
                   EficienciaMto = 100;
                   if (nombres.ContainsKey("ley_eficiencia_mto"))
                   {
                       if (nombres["ley_eficiencia_mto"] != "Ninguna Ley")
                       {
                           //A)Determinación del parametro "EficienciaMto"
                           if (nombres["ley_eficiencia_mto"] == "Fijo") EficienciaMto = parametros["ley_eficiencia_mto_param1"];
                           if (nombres["ley_eficiencia_mto"] == "Uniforme") EficienciaMto = GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme(parametros["Minimo_eficiencia_mto"], parametros["Maximo_eficiencia_mto"], r);
                           if (nombres["ley_eficiencia_mto"] == "Exponencial") EficienciaMto = GeneradoresDeAleatorios.Generador_Aleatorio_Exponencial(parametros["ley_eficiencia_mto_param1"], 1 / parametros["ley_eficiencia_mto_param2"], parametros["Minimo_eficiencia_mto"], parametros["Maximo_eficiencia_mto"], r);
                           if (nombres["ley_eficiencia_mto"] == "Weibull2P") EficienciaMto = GeneradoresDeAleatorios.Generador_Aleatorio_Weibull_2P(parametros["ley_eficiencia_mto_param1"], parametros["ley_eficiencia_mto_param2"], parametros["Minimo_eficiencia_mto"], parametros["Maximo_eficiencia_mto"], r);
                           if (nombres["ley_eficiencia_mto"] == "Línea recta") EficienciaMto = parametros["Y1_eficiencia_mto"] + (parametros["Y2_eficiencia_mto"] - parametros["Y1_eficiencia_mto"]) * (TiempoTranscurrido - parametros["X1_eficiencia_mto"]) / (parametros["X2_eficiencia_mto"] - parametros["X1_eficiencia_mto"]);
                           if (nombres["ley_eficiencia_mto"] == "Normal") EficienciaMto = GeneradoresDeAleatorios.Generador_Aleatorio_Normal(parametros["ley_eficiencia_mto_param1"], parametros["ley_eficiencia_mto_param2"], parametros["Minimo_eficiencia_mto"], parametros["Maximo_eficiencia_mto"], r);

                           //B)Modificación de la variable que contiene el maximo de recuperación 
                           if (EficienciaMto > 0 && EficienciaMto <= 100) maximoY = maximoY * EficienciaMto / 100;
                       }
                   }
                

                   //Generar los costes de la recuperacion y de pérdida de producción
                   if (nombres.ContainsKey("ley_coste"))
                   {
                       if (nombres["ley_coste"] == "Fijo por tiempo")
                       {
                           CosteEsteMantenimiento = parametros["ley_coste_param1"] * t;

                           //Si en lugar de un Mto correctivo se está realizando un Mto Preventivo entonces corregir el coste de Mantenimiento
                           if (tipo_mto_este_ciclo == "Preventivo" && parametros.ContainsKey("coste_Reduccion_si_Preventivo")) CosteEsteMantenimiento = (100 - parametros["coste_Reduccion_si_Preventivo"]) * CosteEsteMantenimiento / 100;

                           CosteEstaPerdidaDeProduccion = parametros["ley_coste_param2"] * t;
                       }

                       if (nombres["ley_coste"] == "Fijo por intervención")
                       {
                           CosteEsteMantenimiento = parametros["ley_coste_param1"];

                           //Si en lugar de un Mto correctivo se está realizando un Mto Preventivo entonces corregir el coste de Mantenimiento 
                           if (tipo_mto_este_ciclo == "Preventivo" && parametros.ContainsKey("coste_Reduccion_si_Preventivo")) CosteEsteMantenimiento = (100 - parametros["coste_Reduccion_si_Preventivo"]) * CosteEsteMantenimiento / 100;

                           CosteEstaPerdidaDeProduccion = parametros["ley_coste_param2"] * t;
                       }

                       if (nombres["ley_coste"] == "Weibull2P")
                       {
                           CosteEsteMantenimiento = GeneradoresDeAleatorios.Generador_Aleatorio_Weibull_2P(parametros["ley_coste_param1"], parametros["ley_coste_param2"], parametros["Minimo_coste"], parametros["Maximo_coste"], r) * t;

                           //Si en lugar de un Mto correctivo se está realizando un Mto Preventivo entonces corregir el coste de Mantenimiento
                           if (tipo_mto_este_ciclo == "Preventivo" && parametros.ContainsKey("coste_Reduccion_si_Preventivo")) CosteEsteMantenimiento = (100 - parametros["coste_Reduccion_si_Preventivo"]) * CosteEsteMantenimiento / 100;

                           CosteEstaPerdidaDeProduccion = parametros["coste_Perdida_Prod_por_Ud_tiempo"] * t;
                       }

                       if (nombres["ley_coste"] == "Normal")
                       {
                           CosteEsteMantenimiento = GeneradoresDeAleatorios.Generador_Aleatorio_Normal(parametros["ley_coste_param1"], parametros["ley_coste_param2"], parametros["Minimo_coste"], parametros["Maximo_coste"], r) * t;

                           //Si en lugar de un Mto correctivo se está realizando un Mto Preventivo entonces corregir el coste de Mantenimiento
                           if (tipo_mto_este_ciclo == "Preventivo" && parametros.ContainsKey("coste_Reduccion_si_Preventivo")) CosteEsteMantenimiento = (100 - parametros["coste_Reduccion_si_Preventivo"]) * CosteEsteMantenimiento / 100;

                           CosteEstaPerdidaDeProduccion = parametros["coste_Perdida_Prod_por_Ud_tiempo"] * t;
                       }


                       if (nombres["ley_coste"] == "Lineal creciente")
                       {
                           CosteEsteMantenimiento = (parametros["Y1_coste"] + (parametros["Y2_coste"] - parametros["Y1_coste"]) * (TiempoTranscurrido - parametros["X1_coste"]) / (parametros["X2_coste"] - parametros["X1_coste"])) * t;

                           //Si en lugar de un Mto correctivo se está realizando un Mto Preventivo entonces corregir el coste de Mantenimiento 
                           if (tipo_mto_este_ciclo == "Preventivo" && parametros.ContainsKey("coste_Reduccion_si_Preventivo")) CosteEsteMantenimiento = (100 - parametros["coste_Reduccion_si_Preventivo"]) * CosteEsteMantenimiento / 100;

                           CosteEstaPerdidaDeProduccion = parametros["coste_Perdida_Prod_por_Ud_tiempo"] * t;
                       }

                       if (nombres["ley_coste"] == "Desglose de Costes")
                       {
                           //Reconocimiento 
                           clave = "recon";
                           coste_recon = GenerarValor(TiempoTranscurrido, "ley_coste_" + clave, "ley_coste_" + clave + "_param1", "ley_coste_" + clave + "_param2", "Maximo_coste_" + clave, "Minimo_coste_" + clave, "X1_coste_" + clave, "Y1_coste_" + clave, "X2_coste_" + clave, "Y2_coste_" + clave, t_paro_recon, r);

                           //Diagnostico 
                           clave = "diag";
                           coste_diag = GenerarValor(TiempoTranscurrido, "ley_coste_" + clave, "ley_coste_" + clave + "_param1", "ley_coste_" + clave + "_param2", "Maximo_coste_" + clave, "Minimo_coste_" + clave, "X1_coste_" + clave, "Y1_coste_" + clave, "X2_coste_" + clave, "Y2_coste_" + clave, t_paro_diag, r);

                           //Preparacion 
                           clave = "prep";
                           coste_prep = GenerarValor(TiempoTranscurrido, "ley_coste_" + clave, "ley_coste_" + clave + "_param1", "ley_coste_" + clave + "_param2", "Maximo_coste_" + clave, "Minimo_coste_" + clave, "X1_coste_" + clave, "Y1_coste_" + clave, "X2_coste_" + clave, "Y2_coste_" + clave, t_paro_prep, r);

                           //Desmantelamiento
                           clave = "desm";
                           coste_desm = GenerarValor(TiempoTranscurrido, "ley_coste_" + clave, "ley_coste_" + clave + "_param1", "ley_coste_" + clave + "_param2", "Maximo_coste_" + clave, "Minimo_coste_" + clave, "X1_coste_" + clave, "Y1_coste_" + clave, "X2_coste_" + clave, "Y2_coste_" + clave, t_paro_desm, r);

                           //Reparacion 
                           clave = "repa";
                           coste_repa = GenerarValor(TiempoTranscurrido, "ley_coste_" + clave, "ley_coste_" + clave + "_param1", "ley_coste_" + clave + "_param2", "Maximo_coste_" + clave, "Minimo_coste_" + clave, "X1_coste_" + clave, "Y1_coste_" + clave, "X2_coste_" + clave, "Y2_coste_" + clave, t_paro_repa, r);

                           //Ensamblaje 
                           clave = "ensam";
                           coste_ensam = GenerarValor(TiempoTranscurrido, "ley_coste_" + clave, "ley_coste_" + clave + "_param1", "ley_coste_" + clave + "_param2", "Maximo_coste_" + clave, "Minimo_coste_" + clave, "X1_coste_" + clave, "Y1_coste_" + clave, "X2_coste_" + clave, "Y2_coste_" + clave, t_paro_ensam, r);

                           //Verificacion 
                           clave = "verif";
                           coste_verif = GenerarValor(TiempoTranscurrido, "ley_coste_" + clave, "ley_coste_" + clave + "_param1", "ley_coste_" + clave + "_param2", "Maximo_coste_" + clave, "Minimo_coste_" + clave, "X1_coste_" + clave, "Y1_coste_" + clave, "X2_coste_" + clave, "Y2_coste_" + clave, t_paro_verif, r);

                           //Puesta en servicio 
                           clave = "serv";
                           coste_serv = GenerarValor(TiempoTranscurrido, "ley_coste_" + clave, "ley_coste_" + clave + "_param1", "ley_coste_" + clave + "_param2", "Maximo_coste_" + clave, "Minimo_coste_" + clave, "X1_coste_" + clave, "Y1_coste_" + clave, "X2_coste_" + clave, "Y2_coste_" + clave, t_paro_serv, r);

                           //Corrección si se está realizando Mantenimiento Preventivo
                           if (tipo_mto_este_ciclo == "Preventivo")
                           {
                               if (parametros.ContainsKey("coste_recon_Reduccion_si_Preventivo")) coste_recon = (100 - parametros["coste_recon_Reduccion_si_Preventivo"]) * coste_recon / 100;
                               if (parametros.ContainsKey("coste_diag_Reduccion_si_Preventivo")) coste_diag = (100 - parametros["coste_diag_Reduccion_si_Preventivo"]) * coste_diag / 100;
                               if (parametros.ContainsKey("coste_prep_Reduccion_si_Preventivo")) coste_prep = (100 - parametros["coste_prep_Reduccion_si_Preventivo"]) * coste_prep / 100;
                               if (parametros.ContainsKey("coste_desm_Reduccion_si_Preventivo")) coste_desm = (100 - parametros["coste_desm_Reduccion_si_Preventivo"]) * coste_desm / 100;
                               if (parametros.ContainsKey("coste_repa_Reduccion_si_Preventivo")) coste_repa = (100 - parametros["coste_repa_Reduccion_si_Preventivo"]) * coste_repa / 100;
                               if (parametros.ContainsKey("coste_ensam_Reduccion_si_Preventivo")) coste_ensam = (100 - parametros["coste_ensam_Reduccion_si_Preventivo"]) * coste_ensam / 100;
                               if (parametros.ContainsKey("coste_verif_Reduccion_si_Preventivo")) coste_verif = (100 - parametros["coste_verif_Reduccion_si_Preventivo"]) * coste_verif / 100;
                               if (parametros.ContainsKey("coste_serv_Reduccion_si_Preventivo")) coste_serv = (100 - parametros["coste_serv_Reduccion_si_Preventivo"]) * coste_serv / 100;
                           }

                           //Se calcula ahora el coste de parada como la suma de los costes del desglose
                           CosteEsteMantenimiento = coste_recon + coste_diag + coste_prep + coste_desm + coste_repa + coste_ensam + coste_verif + coste_serv;


                           //Calculo de los costes desglosados de perdida de producción
                           if (parametros.ContainsKey("coste_recon_Perdida_Prod_por_Ud_tiempo")) coste_prod_recon = parametros["coste_recon_Perdida_Prod_por_Ud_tiempo"] * t_paro_recon;
                           if (parametros.ContainsKey("coste_diag_Perdida_Prod_por_Ud_tiempo")) coste_prod_diag = parametros["coste_diag_Perdida_Prod_por_Ud_tiempo"] * t_paro_diag;
                           if (parametros.ContainsKey("coste_prep_Perdida_Prod_por_Ud_tiempo")) coste_prod_prep = parametros["coste_prep_Perdida_Prod_por_Ud_tiempo"] * t_paro_prep;
                           if (parametros.ContainsKey("coste_desm_Perdida_Prod_por_Ud_tiempo")) coste_prod_desm = parametros["coste_desm_Perdida_Prod_por_Ud_tiempo"] * t_paro_desm;
                           if (parametros.ContainsKey("coste_repa_Perdida_Prod_por_Ud_tiempo")) coste_prod_repa = parametros["coste_repa_Perdida_Prod_por_Ud_tiempo"] * t_paro_repa;
                           if (parametros.ContainsKey("coste_ensam_Perdida_Prod_por_Ud_tiempo")) coste_prod_ensam = parametros["coste_ensam_Perdida_Prod_por_Ud_tiempo"] * t_paro_ensam;
                           if (parametros.ContainsKey("coste_verif_Perdida_Prod_por_Ud_tiempo")) coste_prod_verif = parametros["coste_verif_Perdida_Prod_por_Ud_tiempo"] * t_paro_verif;
                           if (parametros.ContainsKey("coste_serv_Perdida_Prod_por_Ud_tiempo")) coste_prod_serv = parametros["coste_serv_Perdida_Prod_por_Ud_tiempo"] * t_paro_serv;

                           //Calculo del coste total de perdida de produccion como suma de los costes desglosados de perdida de producción
                           CosteEstaPerdidaDeProduccion = coste_prod_recon + coste_prod_diag + coste_prod_prep + coste_prod_desm + coste_prod_repa + coste_prod_ensam + coste_prod_verif + coste_prod_serv;
                       }

                       //Acumular costes derivados del paro/fallo
                       CosteTotalMantenimiento += CosteEsteMantenimiento;
                       CosteTotalPerdidaProduccion += CosteEstaPerdidaDeProduccion;
                   }

                   //Almacenar programa de Mantenimiento preventivo si procede
                   if (tipo_mto_este_ciclo == "Preventivo") programa_mto_preventivo[TiempoTranscurrido] = t;

                   //Incrementar los contadores de Preentivo o correctivo según proceda
                   if (tipo_mto_este_ciclo == "Correctivo") Numero_de_Correctivos += 1;
                   if (tipo_mto_este_ciclo == "Preventivo") Numero_de_Preventivos += 1;

                   //CALCULO DE INDICADORES (SACAR A GRAFICAS TODOS LOS INDICADORES)
                   //---------------------------------------------------------------

                   //Calcular Disponibilidad
                   Disponibilidad = TiempoFuncionandoAcumulado / (TiempoFuncionandoAcumulado + TiempoParadoAcumulado);

                   //Indicador Tiempo Medio Entre Fallos (MTBF)
                   AcumuladorTiemposEntreFallos += TiempoParadoParcial + TiempoParcialFuncionando;
                   MTBF = AcumuladorTiemposEntreFallos / ContadorCiclosFuncionaPara;
                   Lista_MTBF.Add(new PointF((float)TiempoTranscurrido, (float)MTBF));

                   //Indicador "Numero de Fallos"
                   Lista_Numero_de_Fallos.Add(new PointF((float)TiempoTranscurrido, (float)Numero_de_Correctivos));

                   //Indicador "Numero de Preventivos"
                   
                   Lista_Numero_de_Preventivos.Add(new PointF((float)TiempoTranscurrido, (float)Numero_de_Preventivos));

                   //Indicador Tiempo Medio de Reparacion (MTTR)
                   MTTR = TiempoParadoAcumulado / ContadorCiclosFuncionaPara;
                   Lista_MTTR.Add(new PointF((float)TiempoTranscurrido, (float)MTTR));

                   //Indicador Tiempo Medio de Reparacion sin tiempo de Logística (MTTR)
                   MTTR_SinLog = (TiempoParadoAcumulado-TiempoDeLogistica) / ContadorCiclosFuncionaPara;
                   Lista_MTTR_SinLog.Add(new PointF((float)TiempoTranscurrido, (float)MTTR_SinLog));

                   //Indicador  RatioMTTR_vs_MTTR_SinLog
                   RatioMTTR_vs_MTTR_SinLog = MTTR / MTTR_SinLog;
                   Lista_RatioMTTR_vs_MTTR_SinLog.Add(new PointF((float)TiempoTranscurrido, (float)RatioMTTR_vs_MTTR_SinLog));                
                
                   //Indicador Intensidad de Fallos (IntensidadFallos)
                   IntensidadFallos = ContadorCiclosFuncionaPara / TiempoTranscurrido;
                   Lista_IntensidadFallos.Add(new PointF((float)TiempoTranscurrido, (float)IntensidadFallos));

                   //Indicador Coste Acumulado de Recuperacion (Mto+Perdida de produccion)
                   CosteAcumuladoRecuperacion = CosteTotalMantenimiento + CosteTotalPerdidaProduccion;
                   Lista_CosteAcumuladoRecuperacion.Add(new PointF((float)TiempoTranscurrido, (float)CosteAcumuladoRecuperacion));

                   //Indicador Coste Medio de Recuperacion (CosteMedioRecuperacion)
                   CosteMedioRecuperacion = (CosteEsteMantenimiento + CosteEstaPerdidaDeProduccion) / ContadorCiclosFuncionaPara;
                   Lista_CosteMedioRecuperacion.Add(new PointF((float)TiempoTranscurrido, (float)CosteMedioRecuperacion));
                   
                   //Indicador Ratio Correctivo vs tiempo Total de reparación/recuperación
                   Ratio_Correctivo_vs_Total = TiempoCorrectivoAcumulado / TiempoParadoAcumulado;
                   Lista_RatioCorrectivo_vs_total.Add(new PointF((float)TiempoTranscurrido, (float)Ratio_Correctivo_vs_Total));
                   
                   //Indicador ratio Preventivo vs tiempo Total de reparación/recuperación
                   Ratio_Preventivo_vs_Total = TiempoPreventivoAcumulado / TiempoParadoAcumulado;
                   Lista_RatioPreventivo_vs_total.Add(new PointF((float)TiempoTranscurrido, (float)Ratio_Preventivo_vs_Total));
                
                   //FIN DEL CALCULO DE INDICADORES
                   //-------------------------------


                   //Presentar los resultados numéricos en el TextBox de pantalla
                   textBox5.Enabled = true;
                   textBox5.Text += "  " + "\r\n";
                   textBox5.Text += " Ciclo número: " + ContadorCiclosFuncionaPara.ToString("0.") + "\r\n";
                   textBox5.Text += " -------------------------------------------------------------------------------------------------------------------------------------------------" + "\r\n";
                   textBox5.Text += " t funcionando este ciclo = " + TiempoParcialFuncionando.ToString("0.##") + "\r\n";
                   textBox5.Text += " t parado/fallado este ciclo = " + TiempoParadoParcial.ToString("0.##") + "\r\n"; 
                   textBox5.Text += " t total del ciclo = " + TiempoDelCiclo.ToString("0.##") + "\r\n";
                   if (nombres["ley_paro"] == "Desglose de Fallos")
                   {
                       textBox5.Text += "Desglose de Tiempos de Parada/Fallo" + "\r\n";
                       textBox5.Text += "........................................................." + "\r\n";
                       textBox5.Text += " Tiempo de Reconocimiento          = " + t_paro_recon.ToString("0.##") + "\r\n";
                       textBox5.Text += " Tiempo de Diagnóstico             = " + t_paro_diag.ToString("0.##") + "\r\n";
                       textBox5.Text += " Tiempo de Preparación             = " + t_paro_prep.ToString("0.##") + "\r\n";
                       textBox5.Text += " Tiempo de Desmantelamiento        = " + t_paro_desm.ToString("0.##") + "\r\n";
                       textBox5.Text += " Tiempo de Recuperación/Reparación = " + t_paro_repa.ToString("0.##") + "\r\n";
                       textBox5.Text += " Tiempo de Ensamblaje              = " + t_paro_ensam.ToString("0.##") + "\r\n";
                       textBox5.Text += " Tiempo de Verificación            = " + t_paro_verif.ToString("0.##") + "\r\n";
                       textBox5.Text += " Tiempo de Puesta en servicio      = " + t_paro_serv.ToString("0.##") + "\r\n";
                   }
                   textBox5.Text += " Tiempo Medio Entre Fallos (MTBF)= " + MTBF.ToString("0.##") + "\r\n";
                   textBox5.Text += " Tiempo total simulado = " + TiempoTranscurrido.ToString("0.##") + "\r\n";
                   textBox5.Text += " Tiempo total funcionando =  " + TiempoFuncionandoAcumulado.ToString("0.##") + "\r\n";
                   textBox5.Text += " Disponibilidad Maxima Alcanzada en el Ciclo = " + maximoY.ToString("0.######") + "\r\n";
                   textBox5.Text += " Disponibilidad Operacional= " + Disponibilidad.ToString("0.######") + "\r\n";
                   textBox5.Text += " Intensidad de Fallos = " + IntensidadFallos.ToString("0.##########") + "\r\n";
                   textBox5.Text += " Tipo de Mto este ciclo = " + tipo_mto_este_ciclo + "\r\n";
                   textBox5.Text += " Eficiencia del Mto este ciclo = " + EficienciaMto.ToString("0.##") + "\r\n";
                   textBox5.Text += " Tiempo Medio de Reparacion/Recuperación (MTTR) = " + MTTR.ToString("0.##") + "\r\n";                
                   textBox5.Text += " Tiempo Acumulado en Mto. Correctivo = " + TiempoCorrectivoAcumulado.ToString("0.##") + "\r\n";
                   textBox5.Text += " Tiempo Acumulado en Mto. Preventivo = = " + TiempoPreventivoAcumulado.ToString("0.##") + "\r\n";
                   textBox5.Text += " Tiempo Acumulado en Mto. (Preventivo + Correctivo) = = " + TiempoParadoAcumulado.ToString("0.##") + "\r\n";
                   textBox5.Text += " Ratio t_Correctivo/t_Total_Mto = " + Ratio_Correctivo_vs_Total.ToString("0.##") + "\r\n";
                   textBox5.Text += " Ratio t_Preventivo/t_Total_Mto = " + Ratio_Preventivo_vs_Total.ToString("0.##") + "\r\n";
                   textBox5.Text += " Coste Mto este ciclo = " + CosteEsteMantenimiento.ToString("0.##") + "\r\n";
                   textBox5.Text += " Coste acumulado Recuperación (Mto+PérdidaProd) = " + CosteAcumuladoRecuperacion.ToString("0.##") + "\r\n";
                   textBox5.Text += " Coste Medio de Recuperacion = " + CosteMedioRecuperacion.ToString("0.##") + "\r\n";
                   if (nombres.ContainsKey("ley_coste"))
                   {
                       if (nombres["ley_coste"] == "Desglose de Costes")
                       {
                           textBox5.Text += "Desglose de Costes de Parada/Fallo" + "\r\n";
                           textBox5.Text += "........................................................." + "\r\n";
                           textBox5.Text += " Coste de Reconocimiento          = " + coste_recon.ToString("0.##") + "\r\n";
                           textBox5.Text += " Coste de Diagnóstico             = " + coste_diag.ToString("0.##") + "\r\n";
                           textBox5.Text += " Coste de Preparación             = " + coste_prep.ToString("0.##") + "\r\n";
                           textBox5.Text += " Coste de Desmantelamiento        = " + coste_desm.ToString("0.##") + "\r\n";
                           textBox5.Text += " Coste de Recuperación/Reparación = " + coste_repa.ToString("0.##") + "\r\n";
                           textBox5.Text += " Coste de Ensamblaje              = " + coste_ensam.ToString("0.##") + "\r\n";
                           textBox5.Text += " Coste de Verificación            = " + coste_verif.ToString("0.##") + "\r\n";
                           textBox5.Text += " Coste de Puesta en servicio      = " + coste_serv.ToString("0.##") + "\r\n";
                       }
                       textBox5.Text += " Coste Perdida de producción este ciclo = " + CosteEstaPerdidaDeProduccion.ToString("0.##") + "\r\n";
                       if (nombres["ley_coste"] == "Desglose de Costes")
                       {
                           textBox5.Text += "Desglose de Costes Producción Perdida durante la Parada/Fallo" + "\r\n";
                           textBox5.Text += "........................................................................" + "\r\n";
                           textBox5.Text += " Coste de Producción Perdida en Reconocimiento          = " + coste_prod_recon.ToString("0.##") + "\r\n";
                           textBox5.Text += " Coste de Producción Perdida en Diagnóstico             = " + coste_prod_diag.ToString("0.##") + "\r\n";
                           textBox5.Text += " Coste de Producción Perdida en Preparación             = " + coste_prod_prep.ToString("0.##") + "\r\n";
                           textBox5.Text += " Coste de Producción Perdida en Desmantelamiento        = " + coste_prod_desm.ToString("0.##") + "\r\n";
                           textBox5.Text += " Coste de Producción Perdida en Recuperación/Reparación = " + coste_prod_repa.ToString("0.##") + "\r\n";
                           textBox5.Text += " Coste de Producción Perdida en Ensamblaje              = " + coste_prod_ensam.ToString("0.##") + "\r\n";
                           textBox5.Text += " Coste de Producción Perdida en Verificación            = " + coste_prod_verif.ToString("0.##") + "\r\n";
                           textBox5.Text += " Coste de Producción Perdida en Puesta en servicio      = " + coste_prod_serv.ToString("0.##") + "\r\n";
                       }
                   }

                   if (nombres.ContainsKey("preventivo") && nombres.ContainsKey("tipo_de_preventivo"))
                   {
                       if (nombres["preventivo"] == "Activado" && nombres["tipo_de_preventivo"] == "Fijo por tiempo")
                       {
                           textBox5.Text += " Tiempo hasta el siguiente preventivo   = " + TiempoHastaSiguientePreventivo.ToString("0.##") + "\r\n";
                       }
                   }

                   textBox5.Text += "  " + "\r\n";
                   textBox5.Text += "  " + "\r\n";

                   //Estimar el tamaño a rellenar de la barra de calculos y rellenarlo
                   double PorcentajeDeSimulacionRealizadoEnEsteCiclo = (TiempoDelCiclo/Tiempo_A_Simular)*100;
                   int IncrementoBarraDeCalculo = Convert.ToInt32(PorcentajeDeSimulacionRealizadoEnEsteCiclo);   
                   progressBar1.Increment(IncrementoBarraDeCalculo);
             
              } while (TiempoFuncionandoAcumulado + TiempoParadoAcumulado <= Tiempo_A_Simular); //Fin del gran bucle de simulación



              //Guardar Resultados de la Simulación
              resultados["Disponibilidad"] = Disponibilidad;
              resultados["MTBF"] = MTBF;
              resultados["MTTR"] = MTTR;
              resultados["Numero_de_Fallos"] = Numero_de_Correctivos;
              resultados["IntensidadFallos"] = IntensidadFallos;
              resultados["Numero de Ciclos funciona_para simulados = "] = ContadorCiclosFuncionaPara;
              resultados["Numero_de_Correctivos"] = Numero_de_Correctivos;
              resultados["Numero_de_Preventivos"] = Numero_de_Preventivos;
              resultados["Tiempo_Empleado_en_Mto_Correctivo"] = TiempoCorrectivoAcumulado;
              resultados["Tiempo_Empleado_en_Mto_Preventivo"] = TiempoPreventivoAcumulado;
              resultados["Tiempo Total Acumulado en Mto.(Preventivo + Correctivo)"] = TiempoParadoAcumulado;
              resultados["MTTR_SinLog"] = MTTR_SinLog;
              resultados["ratio MTTR/MTTR_sinlog"] = RatioMTTR_vs_MTTR_SinLog;
              resultados["Ratio t_Correctivo/t_Total_Mto"] = Ratio_Correctivo_vs_Total;
              resultados["Ratio t_Preventivo/t_Total_Mto"] = Ratio_Preventivo_vs_Total;
              resultados["CosteTotalMantenimiento"] = CosteTotalMantenimiento;
              resultados["CosteTotalPerdidaProduccion"] = CosteTotalPerdidaProduccion;
              resultados["Coste Medio de Recuperacion"] = CosteMedioRecuperacion;

              textBox5.Text += "RESULTADOS FINALES DE LA SIMULACIÓN" + "\r\n";
              textBox5.Text += "--------------------------------------------------------" + "\r\n";
              foreach(string key in resultados.Keys)
              {
                  textBox5.Text += key + " = " + resultados[key].ToString("0.##########") + "\r\n";
              }
        
              //Imprimir el programa de Mantenimiento Preventivo si procede
              if (programa_mto_preventivo.Count > 0)
              {
                  textBox5.Text += "Programa de Mantenimiento Preventivo" + "\r\n";
                  textBox5.Text += "...................................." + "\r\n";
                  foreach (double key in programa_mto_preventivo.Keys)
                  {
                      textBox5.Text += "\r\n" + "Mto. Preventivo en el tiempo = " + key.ToString("0.##") + " con duración = " + programa_mto_preventivo[key].ToString("0.##");
                  }
              }
            
              //Habilitar el ComboBox de Ver Gráficas
              HabilitarComboBoxVerGraficas();

              //Terminar de rellenar la barra de progreso de los calculos
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


        }



        /// <summary>
        /// Este método genera un valor en función de lo que venga especificado en la string "tipo_ley"
        /// El diccionario global "parametros" del tipo <string,double> contiene los valores asociados a cada nombre de variable de entrada
        /// si "tipo_ley" contiene "Ninguna Ley" se genera un cero
        /// si "tipo_ley" contiene "Fijo" se genera el valor almacenado en "parametros[param1]"
        /// si "tipo_ley" contiene "Uniforme" se genera un aleatorio uniforme entre "parametros[minimo]" y "parametros[maximo]"
        /// si "tipo_ley" contiene "Exponencial" se genera un aleatorio Exponencial con gamma=parametros[param1]y mu= 1 / parametros[param2], cuyo valor debe estar entre parametros[minimo] y parametros[maximo]        
        /// si "tipo_ley" contiene "Weibull2P" se genera un aleatorio Weibull de dos parametros con beta=parametros[param1] y eta= parametros[param2], cuyo valor debe estar entre parametros[minimo] y parametros[maximo]
        /// si "tipo_ley" contiene "Normal" se genera un aleatorio Normal con media=parametros[param1] y desv._tipica=parametros[param2], cuyo valor debe estar entre parametros[minimo] y parametros[maximo]
        /// si "tipo_ley" contiene "línea recta" se genera un valor no aleatorio obtenido de la ecuacion de la recta que pasa por los puntos A(parametros[X1],(parametros[Y1]) y B(parametros[X2],(parametros[Y2])
        /// si "tipo_ley" contiene cualquier otra cadena de caracteres se genera u valor cero a la salida
        /// </summary>
        /// <param name="r"> contiene aleatorio</param>
        /// <param name="variable"> contiene el tiempo total transcurrido en la simulacion actual, o valor asimilable al eje de X</param>
        /// <param name="tipo_ley"> contiene el tipo de ley, por ejemplo: ley_paro_recon</param>
        /// <param name="nombre_ley"> contiene el nombre de una distribucion o forma de generar, por ejemplo: Uniforme</param>
        /// <param name="param1"> el primer parametro o el único</param>
        /// <param name="param2"> contiene el segundo parametro</param>
        /// <param name="minimo"> contiene el limite minimo admisible para el valor a generar</param>
        /// <param name="maximo"> contiene el limite maximo admisible para el valor a generar</param>
        /// <param name="X1"> la x del primer punto de la recta a usar</param>
        /// <param name="Y1"> la y del primer punto de la recta a usar</param>
        /// <param name="X2"> la x del segundo punto de la recta a usar</param>
        /// <param name="Y2"> la y del segundo punto de la recta a usar</param>
        /// <returns></returns>
        private double GenerarValor(double variable, string tipo_ley, string param1, string param2, string maximo, string minimo, string X1, string Y1, string X2, string Y2, double tiempo, Random r )
        {
            double valor_generado = 0;
            string auxi1;
            if (nombres.ContainsKey(tipo_ley))
            {
                auxi1 = nombres[tipo_ley];
                if (auxi1 == "Ninguna Ley") valor_generado = 0;
                if (auxi1 == "Fijo por tiempo") valor_generado = parametros[param1]*tiempo;
                if (auxi1 == "Fijo por intervención") valor_generado = parametros[param1];
                if (auxi1 == "Fijo") valor_generado = parametros[param1];
                if (auxi1 == "Uniforme") valor_generado = GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme(parametros[minimo], parametros[maximo], r) * tiempo;
                if (auxi1 == "Exponencial") valor_generado = GeneradoresDeAleatorios.Generador_Aleatorio_Exponencial(parametros[param1], 1 / parametros[param2], parametros[minimo], parametros[maximo], r) * tiempo;
                if (auxi1 == "Weibull2P") valor_generado = GeneradoresDeAleatorios.Generador_Aleatorio_Weibull_2P(parametros[param1], parametros[param2], parametros[minimo], parametros[maximo], r) * tiempo;
                if (auxi1 == "Normal") valor_generado = GeneradoresDeAleatorios.Generador_Aleatorio_Normal(parametros[param1], parametros[param2], parametros[minimo], parametros[maximo], r) * tiempo;
                if (auxi1 == "Línea recta") valor_generado = parametros[Y1] + (parametros[Y2] - parametros[Y1]) * (variable - parametros[X1]) / (parametros[X2] - parametros[X1]) * tiempo;
            }

            return valor_generado;
        }


        //CARGAR EJEMPLO
        private void button_Ejemplo_Click(object sender, EventArgs e)
        {         
            nombres["ley_func"] = "Exponencial";
            nombres["ley_paro"] = "Normal";
            nombres["ley_recu"] = "Siempre a Nuevo (GAN)";
            nombres["ley_coste"] = "Ninguna Ley";
            nombres["preventivo"] = "No activado";
            nombres["ley_eficiencia_mto"] = "Ninguna Ley";

            parametros["ley_func_param1"] = 0;
            parametros["ley_func_param2"] = 0.001;
            parametros["Minimo_func"] = 100;
            parametros["Maximo_func"] = 10000;
            parametros["ley_paro_param1"] = 200;
            parametros["ley_paro_param2"] = 20;
            parametros["Minimo_paro"] = 100;
            parametros["Maximo_paro"] = 10000;
            parametros["paro_Reduccion_si_Preventivo"] = 20;

            textBox11.Enabled = true;
            textBox10.Text = "20000";

            foreach (string key in nombres.Keys)
            {
                textBox11.Text += "\r\n" + key + " = " + nombres[key].ToString();
            }

            textBox11.Text += "\r\n";

            foreach (string key in parametros.Keys)
            {
                textBox11.Text += "\r\n" + key + " = " + parametros[key].ToString();
            }           
        }

        //RESET DE DATOS
        private void button_Reset_Click(object sender, EventArgs e)
        {
            //TextBox de resultados
            textBox5.Text = "";
            textBox5.Enabled = false;

            //TextBox de valores seleccionados
            textBox11.Text = "";
            textBox11.Enabled = false;
            
            //Funcionamiento
            comboBox1.Text = "";
            comboBox1.Enabled = true;

            //Parada-fallo
            comboBox2.Text = "";
            comboBox2.Enabled = true;

            //Recuperacion
            comboBox3.Text = "";
            comboBox3.Enabled = true;

            //Costes
            comboBox4.Text = "";
            comboBox4.Enabled = true;

            //Desglose Fallos
            comboBox_T_Rec.Text = "";
            comboBox_T_Rec.Enabled = false;

            //Tiempo a simular y repeticiones de la simulación
            textBox10.Text = "";
            textBox10.Enabled = true;
            button4.Enabled = true;
            progressBar1.Value = 0;

            //cargar_ejemplo = false;
            //Desabilitamos las opciones de desglose de Fallo y Coste
            DeshabilitarComboBoxDesgloseTiempoFallo();
            DeshabilitarComboBoxDesgloseCoste();

            //Limpiar los diccionarios
            parametros.Clear();
            nombres.Clear();

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

        //EVENTO PARA VER LOS PARAMETROS ESCOGIDOS 
        private void button_Parametros_Click(object sender, EventArgs e)
        {

            textBox11.Enabled = true;
            foreach (string key in nombres.Keys)
            {
                textBox11.Text += "\r\n" + "nombres[¡"+ key + "¡] = " + "¡" + nombres[key].ToString()+ "¡" + ";";
            } 

            textBox11.Text += "\r\n";
            foreach (string key in parametros.Keys)
            {
                textBox11.Text += "\r\n" + "parametros[¡" + key + "¡] = " + parametros[key].ToString("0.#######") + ";";
            }

        }

        private void DeshabilitarComboBoxVerGraficas()
        {
            comboBox7.Enabled = false;
        }

        private void HabilitarComboBoxVerGraficas()
        {
            comboBox7.Enabled = true;
        }



        private void DeshabilitarComboBoxDesgloseTiempoFallo()
        {
            comboBox_T_Rec.Text = ""; comboBox_T_Rec.Enabled = false;
            comboBox_T_Diag.Text = ""; comboBox_T_Diag.Enabled = false;
            comboBox_T_Prep.Text = ""; comboBox_T_Prep.Enabled = false;
            comboBox_T_Desm.Text = ""; comboBox_T_Desm.Enabled = false;
            comboBox_T_Rep.Text = ""; comboBox_T_Rep.Enabled = false;
            comboBox_T_Ensam.Text = ""; comboBox_T_Ensam.Enabled = false;
            comboBox_T_Verif.Text = ""; comboBox_T_Verif.Enabled = false;
            comboBox_T_Serv.Text = ""; comboBox_T_Serv.Enabled = false;
        }

        private void HacerInvisibleDesgloseTiempoFallo()
        {
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
            label14.Visible = false;
            comboBox_T_Rec.Visible = false;
            comboBox_T_Diag.Visible = false;
            comboBox_T_Prep.Visible = false;
            comboBox_T_Desm.Visible = false;
            comboBox_T_Rep.Visible = false;
            comboBox_T_Ensam.Visible = false;
            comboBox_T_Verif.Visible = false;
            comboBox_T_Serv.Visible = false;
        }

        private void HacerVisibleDesgloseTiempoFallo()
        {
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            label7.Visible = true;
            label8.Visible = true;
            label9.Visible = true;
            label10.Visible = true;
            label11.Visible = true;
            label14.Visible = true;
            comboBox_T_Rec.Visible = true;
            comboBox_T_Diag.Visible = true;
            comboBox_T_Prep.Visible = true;
            comboBox_T_Desm.Visible = true;
            comboBox_T_Rep.Visible = true;
            comboBox_T_Ensam.Visible = true;
            comboBox_T_Verif.Visible = true;
            comboBox_T_Serv.Visible = true;
        }

        private void DeshabilitarComboBoxDesgloseCoste()
        {
            comboBox_C_Rec.Text = ""; comboBox_C_Rec.Enabled = false;
            comboBox_C_Diag.Text = ""; comboBox_C_Diag.Enabled = false;
            comboBox_C_Prep.Text = ""; comboBox_C_Prep.Enabled = false;
            comboBox_C_Desm.Text = ""; comboBox_C_Desm.Enabled = false;
            comboBox_C_Rep.Text = ""; comboBox_C_Rep.Enabled = false;
            comboBox_C_Ensam.Text = ""; comboBox_C_Ensam.Enabled = false;
            comboBox_C_Verif.Text = ""; comboBox_C_Verif.Enabled = false;
            comboBox_C_Serv.Text = ""; comboBox_C_Serv.Enabled = false;
        }

        private void HacerInvisibleDesgloseCoste()
        {
            label27.Visible = false;
            label26.Visible = false;
            label25.Visible = false;
            label24.Visible = false;
            label23.Visible = false;
            label22.Visible = false;
            label21.Visible = false;
            label19.Visible = false;
            label18.Visible = false;
            comboBox_C_Rec.Visible = false;
            comboBox_C_Diag.Visible = false;
            comboBox_C_Prep.Visible = false;
            comboBox_C_Desm.Visible = false;
            comboBox_C_Rep.Visible = false;
            comboBox_C_Ensam.Visible = false;
            comboBox_C_Verif.Visible = false;
            comboBox_C_Serv.Visible = false;
        }

        private void HacerVisibleDesgloseCoste()
        {
            label27.Visible = true;
            label26.Visible = true;
            label25.Visible = true;
            label24.Visible = true;
            label23.Visible = true;
            label22.Visible = true;
            label21.Visible = true;
            label19.Visible = true;
            label18.Visible = true;
            comboBox_C_Rec.Visible = true;
            comboBox_C_Diag.Visible = true;
            comboBox_C_Prep.Visible = true;
            comboBox_C_Desm.Visible = true;
            comboBox_C_Rep.Visible = true;
            comboBox_C_Ensam.Visible = true;
            comboBox_C_Verif.Visible = true;
            comboBox_C_Serv.Visible = true;
        }

        private void HabilitarComboBoxDesgloseTiempoFallo()
        {
            comboBox_T_Rec.Text = ""; comboBox_T_Rec.Enabled = true;
            comboBox_T_Diag.Text = ""; comboBox_T_Diag.Enabled = true;
            comboBox_T_Prep.Text = ""; comboBox_T_Prep.Enabled = true;
            comboBox_T_Desm.Text = ""; comboBox_T_Desm.Enabled = true;
            comboBox_T_Rep.Text = ""; comboBox_T_Rep.Enabled = true;
            comboBox_T_Ensam.Text = ""; comboBox_T_Ensam.Enabled = true;
            comboBox_T_Verif.Text = ""; comboBox_T_Verif.Enabled = true;
            comboBox_T_Serv.Text = ""; comboBox_T_Serv.Enabled = true;
        }
       
        private void HabilitarComboBoxDesgloseCoste()
        {
            comboBox_C_Rec.Text = ""; comboBox_C_Rec.Enabled = true;
            comboBox_C_Diag.Text = ""; comboBox_C_Diag.Enabled = true;
            comboBox_C_Prep.Text = ""; comboBox_C_Prep.Enabled = true;
            comboBox_C_Desm.Text = ""; comboBox_C_Desm.Enabled = true;
            comboBox_C_Rep.Text = ""; comboBox_C_Rep.Enabled = true;
            comboBox_C_Ensam.Text = ""; comboBox_C_Ensam.Enabled = true;
            comboBox_C_Verif.Text = ""; comboBox_C_Verif.Enabled = true;
            comboBox_C_Serv.Text = ""; comboBox_C_Serv.Enabled = true;
        }




        private void SolicitarDatos(FormDatos1 frm, string Ambito_Op, string Ley, string titulo_del_Formulario, string Rotulo_del_Parametro1,
                                    string Rotulo_del_Parametro2, string Rotulo_del_Parametro3,
                                    string Rotulo_del_Parametro4, string Rotulo_del_Parametro5,
                                    string Nombre_Parametro1, string Nombre_Parametro2,
                                    string Nombre_Parametro3, string Nombre_Parametro4, string Nombre_Parametro5, string Rotulo_del_Parametro6, string Nombre_Parametro6)
        {
            //Dar contenidos a las variables del formulario de captura de indicaciones de usuario
            frm.TituloDelFormulario = titulo_del_Formulario;
            frm.Text = Ambito_Op;
            frm.Rotulo_Parametro1 = Rotulo_del_Parametro1;
            frm.Rotulo_Parametro2 = Rotulo_del_Parametro2;
            frm.Rotulo_Parametro3 = Rotulo_del_Parametro3;
            frm.Rotulo_Parametro4 = Rotulo_del_Parametro4;
            frm.Rotulo_Parametro5 = Rotulo_del_Parametro5;
            frm.Rotulo_Parametro6 = Rotulo_del_Parametro6;

            //Llamar al formulario de captura de indicaciones de usuario
            frm.ShowDialog();

            //dar valores a las variables de este formulario en base a las indicaciones de usuario
            if (frm.DialogResult == DialogResult.OK)
            {
                limpiar_diccionarios();
                
                if (Rotulo_del_Parametro1 != "") parametros[Nombre_Parametro1] = Convert.ToDouble(frm.parametro1);
                if (Rotulo_del_Parametro2 != "") parametros[Nombre_Parametro2] = Convert.ToDouble(frm.parametro2);
                if (Rotulo_del_Parametro3 != "") parametros[Nombre_Parametro3] = Convert.ToDouble(frm.parametro3);
                if (Rotulo_del_Parametro4 != "") parametros[Nombre_Parametro4] = Convert.ToDouble(frm.parametro4);
                if (Rotulo_del_Parametro5 != "") parametros[Nombre_Parametro5] = Convert.ToDouble(frm.parametro5);
                if (Rotulo_del_Parametro6 != "") parametros[Nombre_Parametro6] = Convert.ToDouble(frm.parametro6);


                //Alimentar el Textbox de las opciones elegidas por el usuario
                if ((Rotulo_del_Parametro1 != "") || (Rotulo_del_Parametro2 != "") || (Rotulo_del_Parametro3 != "") || (Rotulo_del_Parametro4 != "") || (Rotulo_del_Parametro5 != "") || (Rotulo_del_Parametro6 != ""))
                {
                    textBox11.Enabled = true;
                    textBox11.Text += "\r\n" + "\b " + Ambito_Op + titulo_del_Formulario + "\r\n";
                    if (Rotulo_del_Parametro1 != "") textBox11.Text += "   " + Rotulo_del_Parametro1 + "= " + Convert.ToString(parametros[Nombre_Parametro1]) + "\r\n";
                    if (Rotulo_del_Parametro2 != "") textBox11.Text += "   " + Rotulo_del_Parametro2 + "= " + Convert.ToString(parametros[Nombre_Parametro2]) + "\r\n";
                    if (Rotulo_del_Parametro3 != "") textBox11.Text += "   " + Rotulo_del_Parametro3 + "= " + Convert.ToString(parametros[Nombre_Parametro3]) + "\r\n";
                    if (Rotulo_del_Parametro4 != "") textBox11.Text += "   " + Rotulo_del_Parametro4 + "= " + Convert.ToString(parametros[Nombre_Parametro4]) + "\r\n";
                    if (Rotulo_del_Parametro5 != "") textBox11.Text += "   " + Rotulo_del_Parametro5 + "= " + Convert.ToString(parametros[Nombre_Parametro5]) + "\r\n";
                    if (Rotulo_del_Parametro6 != "") textBox11.Text += "   " + Rotulo_del_Parametro6 + "= " + Convert.ToString(parametros[Nombre_Parametro6]) + "\r\n";
                    textBox11.Text += "             " + "\r\n";
                }

            }
        }

        private void ElegirLeySolicitarDatos(string Ambito, string palabra_clave, string sufijo2, string nombre_de_ley, string nombre_de_campo_y_combo_Box)
        {

                terminos_a_eliminar_en_diccionarios[sufijo2 + "_Mantenimiento_Ud_Tiempo"] = "parametros";
                terminos_a_eliminar_en_diccionarios[sufijo2 + "_Perdida_Prod_por_Ud_tiempo"] = "parametros";
                terminos_a_eliminar_en_diccionarios[sufijo2 + "_MantenimientoCadaIntervencion"]="parametros";
                terminos_a_eliminar_en_diccionarios["X1_" + sufijo2]="parametros";
                terminos_a_eliminar_en_diccionarios["Y1_" + sufijo2]="parametros";
                terminos_a_eliminar_en_diccionarios["X2_" + sufijo2]="parametros";
                terminos_a_eliminar_en_diccionarios["Y2_" + sufijo2]="parametros";
                terminos_a_eliminar_en_diccionarios["ley_" + sufijo2 + "_param1"]="parametros";
                terminos_a_eliminar_en_diccionarios["ley_" + sufijo2 + "_param2"]="parametros";
                terminos_a_eliminar_en_diccionarios["Minimo_" + sufijo2]="parametros";
                terminos_a_eliminar_en_diccionarios["Maximo_" + sufijo2] = "parametros";               
                terminos_a_eliminar_en_diccionarios["ley_" + sufijo2]="parametros";
                //terminos_a_eliminar_en_diccionarios["ley_coste_" + sufijo2 + "_param1"]="parametros";
                //terminos_a_eliminar_en_diccionarios["ley_coste_" + sufijo2 + "_param2"]="parametros";
                //terminos_a_eliminar_en_diccionarios["Minimo_" + sufijo2]="parametros";
                //terminos_a_eliminar_en_diccionarios["Maximo_" + sufijo2] = "parametros";


                
            
            FormDatos1 frm = new FormDatos1();

            if (nombre_de_campo_y_combo_Box == "Ninguna Ley")
            {
                nombres[nombre_de_ley] = nombre_de_campo_y_combo_Box;
                limpiar_diccionarios();
            }

            if (nombre_de_campo_y_combo_Box == "Siempre a Nuevo (GAN)")
            {
                nombres[nombre_de_ley] = nombre_de_campo_y_combo_Box;
                limpiar_diccionarios();
            }

            if (nombre_de_campo_y_combo_Box == "Según tiempo (BAO)")
            {
                nombres[nombre_de_ley] = nombre_de_campo_y_combo_Box;
                limpiar_diccionarios();
            }
                        
            if (nombre_de_campo_y_combo_Box == "Desglose de Costes")
            {
                limpiar_diccionarios();
                nombres[nombre_de_ley] = nombre_de_campo_y_combo_Box;
                
                //Se hacen visibles y habilitan las opciones de desglose de Coste
                HacerVisibleDesgloseCoste();
                HabilitarComboBoxDesgloseCoste();
            }

            if (nombre_de_campo_y_combo_Box == "Desglose de Fallos")
            {
                limpiar_diccionarios();
                nombres[nombre_de_ley] = nombre_de_campo_y_combo_Box;

                //Se habilitan las opciones de desglose de Fallo
                HacerVisibleDesgloseTiempoFallo();
                HabilitarComboBoxDesgloseTiempoFallo();
            }
                       
            //el siguiente if es solo aplicable a captura de datos de coste
            if (nombre_de_campo_y_combo_Box == "Fijo por tiempo")
            {
                //limpiar_diccionarios();
                nombres[nombre_de_ley] = nombre_de_campo_y_combo_Box;
                SolicitarDatos(frm, Ambito, nombres[nombre_de_ley], "Ley de Coste Fijo por tiempo ", "Coste Mto./Ud_tiempo", "Coste_Perdida_Prod_por_Ud_tiempo", "",
                                    "", "","ley_" + sufijo2 +"_param1", "ley_" + sufijo2 + "_param2",
                                    "", "", "","% reducción si Preventivo",sufijo2 +"_Reduccion_si_Preventivo");
            }

            //el siguiente if es solo aplicable a captura de datos de coste
            if (nombre_de_campo_y_combo_Box == "Fijo por intervención")
            {
                //limpiar_diccionarios();
                nombres[nombre_de_ley] = nombre_de_campo_y_combo_Box;
                SolicitarDatos(frm, Ambito, nombres[nombre_de_ley], "Ley de Coste Fijo cada Intervención ", "Coste cada Intervención", "Coste_Perdida_Prod_por_Ud_tiempo", "",
                    "", "", "ley_" + sufijo2 + "_param1", "ley_" + sufijo2 + "_param2",
                    "", "", "", "% reducción si Preventivo", sufijo2 + "_Reduccion_si_Preventivo");
            }           
            
            if (nombre_de_campo_y_combo_Box == "Fijo")
            {
                //limpiar_diccionarios();
                nombres[nombre_de_ley] = nombre_de_campo_y_combo_Box;
                string auxi1="";
                string auxi2="";
                if (Ambito == "COSTE DESGLOSADO: " || Ambito == "COSTE SIN DESGLOSE: ")
                {
                    auxi1 = "Coste Perdida_Prod / Ud_tiempo";
                    auxi2 = "_Perdida_Prod_por_Ud_tiempo";
                }
                
                string auxi3="";
                string auxi4="";
                if (Ambito == "COSTE DESGLOSADO: " || Ambito == "COSTE SIN DESGLOSE: " || Ambito == "FALLO/PARADA: " || Ambito == "DESGLOSE DE FALLO/PARADA: ")
                {
                    auxi3 = "% reducción si Preventivo";
                    auxi4 = sufijo2 + "_Reduccion_si_Preventivo";
                }

                string auxi5 = "Tiempo de ";
                if (Ambito == "EFICIENCIA DEL MANTENIMIENTO: ") auxi5 = " "; ;

              
                SolicitarDatos(frm, Ambito, nombres[nombre_de_ley], "Ley Fija de " + palabra_clave, auxi5 + palabra_clave, "", "",
                                    "", auxi1, "ley_" + sufijo2 + "_param1", "",
                                    "", "", sufijo2 + auxi2, auxi3, auxi4);
            }

            if (nombre_de_campo_y_combo_Box == "Uniforme")
            {
                //limpiar_diccionarios();
                nombres[nombre_de_ley] = nombre_de_campo_y_combo_Box;
                string auxi1 = "";
                string auxi2 = "";
                if (Ambito == "COSTE DESGLOSADO: " || Ambito == "COSTE SIN DESGLOSE: ")
                {
                    auxi1 = "Coste Perdida_Prod / Ud_tiempo";
                    auxi2 = "_Perdida_Prod_por_Ud_tiempo";
                }


                string auxi3 = "";
                string auxi4 = "";
                if (Ambito == "COSTE DESGLOSADO: " || Ambito == "COSTE SIN DESGLOSE: " || Ambito == "FALLO/PARADA: " || Ambito == "DESGLOSE DE FALLO/PARADA: ")
                {
                    auxi3 = "% reducción si Preventivo";
                    auxi4 = sufijo2 + "_Reduccion_si_Preventivo";
                }



                SolicitarDatos(frm, Ambito, nombres[nombre_de_ley], "Ley Uniforme de " + palabra_clave, "", "", "Mínimo Admisible",
                                    "Máximo Admisible", auxi1, "", "",
                                    "Minimo_" + sufijo2, "Maximo_" + sufijo2, sufijo2 + auxi2, auxi3, auxi4);
            }

            if (nombre_de_campo_y_combo_Box == "Línea recta")
            {
                //limpiar_diccionarios();
                nombres[nombre_de_ley] = nombre_de_campo_y_combo_Box;
                string auxi1 = "";
                string auxi2 = "";
                if (Ambito == "COSTE DESGLOSADO: " || Ambito == "COSTE SIN DESGLOSE: ")
                {
                    auxi1 = "Coste Perdida_Prod / Ud_tiempo";
                    auxi2 = "_Perdida_Prod_por_Ud_tiempo";
                }


                string auxi3 = "";
                string auxi4 = "";
                if (Ambito == "COSTE DESGLOSADO: " || Ambito == "COSTE SIN DESGLOSE: " || Ambito == "FALLO/PARADA: " || Ambito == "DESGLOSE DE FALLO/PARADA: ")
                {
                    auxi3 = "% reducción si Preventivo";
                    auxi4 = sufijo2 + "_Reduccion_si_Preventivo";
                }



                SolicitarDatos(frm, Ambito, nombres[nombre_de_ley], "Ley Lineal de " + palabra_clave, "Inicial T", "Inicial t " + palabra_clave, "Final T",
                                    "Final t " + palabra_clave, auxi1, "X1_" + sufijo2, "Y1_" + sufijo2,
                                     "X2_" + sufijo2, "Y2_" + sufijo2, sufijo2 + auxi2, auxi3, auxi4);
            }

            if (nombre_de_campo_y_combo_Box == "Exponencial")
            {
                //limpiar_diccionarios();
                nombres[nombre_de_ley] = nombre_de_campo_y_combo_Box;
                string auxi1 = "";
                string auxi2 = "";
                if (Ambito == "COSTE DESGLOSADO: " || Ambito == "COSTE SIN DESGLOSE: ")
                {
                    auxi1 = "Coste Perdida_Prod / Ud_tiempo";
                    auxi2 = "_Perdida_Prod_por_Ud_tiempo";
                }


                string auxi3 = "";
                string auxi4 = "";
                if (Ambito == "COSTE DESGLOSADO: " || Ambito == "COSTE SIN DESGLOSE: " || Ambito == "FALLO/PARADA: " || Ambito == "DESGLOSE DE FALLO/PARADA: ")
                {
                    auxi3 = "% reducción si Preventivo";
                    auxi4 = sufijo2 + "_Reduccion_si_Preventivo";
                }



                SolicitarDatos(frm, Ambito, nombres[nombre_de_ley], "Ley Exponencial de " + palabra_clave, "Gamma", "Lambda", "Mínimo Admisible",
                                    "Máximo Admisible", auxi1, "ley_" + sufijo2 + "_param1", "ley_" + sufijo2 + "_param2",
                                    "Minimo_" + sufijo2, "Maximo_" + sufijo2, sufijo2 + auxi2, auxi3, auxi4);
            }

            if (nombre_de_campo_y_combo_Box == "Weibull2P")
            {
                //limpiar_diccionarios();
                nombres[nombre_de_ley] = nombre_de_campo_y_combo_Box;
                string auxi1 = "";
                string auxi2 = "";
                if (Ambito == "COSTE DESGLOSADO: " || Ambito == "COSTE SIN DESGLOSE: ")
                {
                    auxi1 = "Coste Perdida_Prod / Ud_tiempo";
                    auxi2 = "_Perdida_Prod_por_Ud_tiempo";
                }


                string auxi3 = "";
                string auxi4 = "";
                if (Ambito == "COSTE DESGLOSADO: " || Ambito == "COSTE SIN DESGLOSE: " || Ambito == "FALLO/PARADA: " || Ambito == "DESGLOSE DE FALLO/PARADA: ")
                {
                    auxi3 = "% reducción si Preventivo";
                    auxi4 = sufijo2 + "_Reduccion_si_Preventivo";
                }



                SolicitarDatos(frm, Ambito, nombres[nombre_de_ley], "Ley Weibull2P de " + palabra_clave, "Beta", "Eta", "Mínimo Admisible",
                                    "Máximo Admisible", auxi1, "ley_" + sufijo2 + "_param1", "ley_" + sufijo2 + "_param2",
                                    "Minimo_" + sufijo2, "Maximo_" + sufijo2, sufijo2 + auxi2, auxi3, auxi4);
            }

            if (nombre_de_campo_y_combo_Box == "Normal")
            {
                //limpiar_diccionarios();
                nombres[nombre_de_ley] = nombre_de_campo_y_combo_Box;
                string auxi1="";
                string auxi2="";
                if (Ambito == "COSTE DESGLOSADO: " || Ambito == "COSTE SIN DESGLOSE: ")
                {
                    auxi1 = "Coste Perdida_Prod / Ud_tiempo";
                    auxi2 = "_Perdida_Prod_por_Ud_tiempo";
                }


                string auxi3 = "";
                string auxi4 = "";
                if (Ambito == "COSTE DESGLOSADO: " || Ambito == "COSTE SIN DESGLOSE: " || Ambito == "FALLO/PARADA: "  || Ambito == "DESGLOSE DE FALLO/PARADA: ")
                {
                    auxi3 = "% reducción si Preventivo";
                    auxi4 = sufijo2 + "_Reduccion_si_Preventivo";
                }



                SolicitarDatos(frm, Ambito, nombres[nombre_de_ley], "Ley Normal de " + palabra_clave, "Valor Medio", "Desviación Típica", "Mínimo Admisible",
                                    "Máximo Admisible", auxi1, "ley_" + sufijo2 + "_param1", "ley_" + sufijo2 + "_param2",
                                    "Minimo_" + sufijo2, "Maximo_" + sufijo2, sufijo2 + auxi2, auxi3, auxi4);               
            }
        }


        //Entradas de usuario sobre la Ley de Funcionamiento
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Ambito = "FUNCIONAMIENTO: ";
            string palabra_clave = "Funcionamiento";
            string sufijo2 = "func";
            string nombre_de_ley = "ley_func";
            string nombre_de_campo_y_combo_Box = comboBox1.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box);
        }

        //Entradas de usuario sobre la Ley de Parada
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            HacerInvisibleDesgloseTiempoFallo();
            DeshabilitarComboBoxDesgloseTiempoFallo();

            string Ambito = "FALLO/PARADA: ";
            string palabra_clave = "Fallo_Parada";
            string sufijo2 = "paro";
            string nombre_de_ley = "ley_paro";
            string nombre_de_campo_y_combo_Box = comboBox2.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box);
        }

        //Captura de indicaciones de usuario sobre ley de reparación/recuperacion
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Ambito = "REPARACIÓN/RECUPERACIÓN: ";
            string palabra_clave = "Recuperación";
            string sufijo2 = "recu";
            string nombre_de_ley = "ley_recu";
            string nombre_de_campo_y_combo_Box = comboBox3.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box);
        }

        //Captura de indicaciones de usuario sobre ley de COSTES
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            //HacerInvisibleDesgloseCoste()
            HacerInvisibleDesgloseCoste();
            DeshabilitarComboBoxDesgloseCoste();

            string Ambito = "COSTE SIN DESGLOSE: ";
            string palabra_clave = "Coste_sin_desglose";
            string sufijo2 = "coste";
            string nombre_de_ley = "ley_coste";
            string nombre_de_campo_y_combo_Box = comboBox4.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box);
        }
       
        //SELECCION DE LAS OPCIONES DE DESGLOSE DE TIEMPO DE FALLO/PARADA
        private void comboBox_T_Rec_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string Ambito = "PREPARACION: ";
            string Ambito = "DESGLOSE DE FALLO/PARADA: ";
            string palabra_clave = "Reconocimiento";
            string sufijo2 = "paro_recon";
            string nombre_de_ley = "ley_paro_recon";
            string nombre_de_campo_y_combo_Box = comboBox_T_Rec.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box);
        }

        private void comboBox_T_Diag_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string Ambito = "DIAGNOSTICO: ";
            string Ambito = "DESGLOSE DE FALLO/PARADA: ";
            string palabra_clave = "Diagnostico";
            string sufijo2 = "paro_diag";
            string nombre_de_ley = "ley_paro_diag";
            string nombre_de_campo_y_combo_Box = comboBox_T_Diag.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box);
        }

        private void comboBox_T_Prep_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string Ambito = "PREPARACIÓN: ";
            string Ambito = "DESGLOSE DE FALLO/PARADA: ";
            string palabra_clave = "Preparacion";
            string sufijo2 = "paro_prep";
            string nombre_de_ley = "ley_paro_prep";
            string nombre_de_campo_y_combo_Box = comboBox_T_Prep.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box);
        }

        private void comboBox_T_Desm_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string Ambito = "DESMANTENLAMIENTO: ";
            string Ambito = "DESGLOSE DE FALLO/PARADA: ";
            string palabra_clave = "Desmantelamiento";
            string sufijo2 = "paro_desm";
            string nombre_de_ley = "ley_paro_desm";
            string nombre_de_campo_y_combo_Box = comboBox_T_Desm.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box);
        }

        private void comboBox_T_Rep_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string Ambito = "REPARACIÓN: ";
            string Ambito = "DESGLOSE DE FALLO/PARADA: ";
            string palabra_clave = "Reparacion";
            string sufijo2 = "paro_repa";
            string nombre_de_ley = "ley_paro_repa";
            string nombre_de_campo_y_combo_Box = comboBox_T_Rep.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box);         
        }

        private void comboBox_T_Ensam_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string Ambito = "ENSAMBLAJE: ";
            string Ambito = "DESGLOSE DE FALLO/PARADA: ";
            string palabra_clave = "Ensamblaje";
            string sufijo2 = "paro_ensam";
            string nombre_de_ley = "ley_paro_ensam";
            string nombre_de_campo_y_combo_Box = comboBox_T_Ensam.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box);         
        }

        private void comboBox_T_Verif_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string Ambito = "VERIFICACIÓN: ";
            string Ambito = "DESGLOSE DE FALLO/PARADA: ";
            string palabra_clave = "Verificacion";
            string sufijo2 = "paro_verif";
            string nombre_de_ley = "ley_paro_verif";
            string nombre_de_campo_y_combo_Box = comboBox_T_Verif.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box);            
        }

        private void comboBox_T_Serv_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string Ambito = "PUESTA EN SERVICIO: ";
            string Ambito = "DESGLOSE DE FALLO/PARADA: ";
            string palabra_clave = "Puesta en Servicio";
            string sufijo2 = "paro_serv";
            string nombre_de_ley = "ley_paro_serv";
            string nombre_de_campo_y_combo_Box = comboBox_T_Serv.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box);       
        }


        
        //SELECCION DE LAS OPCIONES DE DESGLOSE DE COSTE
        private void comboBox_C_Rec_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string Ambito = "COSTE DE RECONOCIMIENTO: ";
            string Ambito = "COSTE DESGLOSADO: ";
            string palabra_clave = "Coste de Reconocimiento";
            string sufijo2 = "coste_recon";
            string nombre_de_ley = "ley_coste_recon";
            string nombre_de_campo_y_combo_Box = comboBox_C_Rec.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box);            
        }

        private void comboBox_C_Diag_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string Ambito = "COSTE DE DIAGNÓSTICO: ";
            string Ambito = "COSTE DESGLOSADO: ";
            string palabra_clave = "Coste de Diagnostico";
            string sufijo2 = "coste_diag";
            string nombre_de_ley = "ley_coste_diag";
            string nombre_de_campo_y_combo_Box = comboBox_C_Diag.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box);      
        }

        private void comboBox_C_Prep_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string Ambito = "COSTE DE PREPARACIÓN: ";
            string Ambito = "COSTE DESGLOSADO: ";
            string palabra_clave = "Coste de Preparacion";
            string sufijo2 = "coste_prep";
            string nombre_de_ley = "ley_coste_prep";
            string nombre_de_campo_y_combo_Box = comboBox_C_Prep.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box);
        }

        private void comboBox_C_Desm_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string Ambito = "COSTE DE DESMANTELAMIENTO: ";
            string Ambito = "COSTE DESGLOSADO: ";
            string palabra_clave = "Coste de Desmantelamiento";
            string sufijo2 = "coste_desm";
            string nombre_de_ley = "ley_coste_desm";
            string nombre_de_campo_y_combo_Box = comboBox_C_Desm.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box);            
        }

        private void comboBox_C_Rep_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string Ambito = "COSTE DE REPARACIÓN/REEMPLAZO: ";
            string Ambito = "COSTE DESGLOSADO: ";
            string palabra_clave = "Coste de Reparacion";
            string sufijo2 = "coste_repa";
            string nombre_de_ley = "ley_coste_repa";
            string nombre_de_campo_y_combo_Box = comboBox_C_Rep.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box);           
        }

        private void comboBox_C_Ensam_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string Ambito = "COSTE DE ENSAMBLAJE: ";
            string Ambito = "COSTE DESGLOSADO: ";
            string palabra_clave = "Coste de Ensamblaje";
            string sufijo2 = "coste_ensam";
            string nombre_de_ley = "ley_coste_ensam";
            string nombre_de_campo_y_combo_Box = comboBox_C_Ensam.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box);          
        }

        private void comboBox_C_Verif_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string Ambito = "COSTE DE VERIFICACIÓN: ";
            string Ambito = "COSTE DESGLOSADO: ";
            string palabra_clave = "Coste de Verificacion";
            string sufijo2 = "coste_verif";
            string nombre_de_ley = "ley_coste_verif";
            string nombre_de_campo_y_combo_Box = comboBox_C_Verif.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box);           
        }

        private void comboBox_C_Serv_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string Ambito = "COSTE DE PUESTA EN SERVICIO: ";
            string Ambito = "COSTE DESGLOSADO: ";
            string palabra_clave = "Coste de Puesta_en_servicio";
            string sufijo2 = "coste_serv";
            string nombre_de_ley = "ley_coste_serv";
            string nombre_de_campo_y_combo_Box = comboBox_C_Serv.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box);
        }

        private void FormFuncionaFallaAmpliado_Load(object sender, EventArgs e)
        {
            //Hacemos Invisibles las opciones de desglose de Fallo y Coste
            HacerInvisibleDesgloseTiempoFallo();
            HacerInvisibleDesgloseCoste();

            //Desabilitamos las opciones de desglose de Fallo y Coste
            DeshabilitarComboBoxDesgloseTiempoFallo();
            DeshabilitarComboBoxDesgloseCoste();

            //Deshabilitar el combobox de Ver Graficas
            DeshabilitarComboBoxVerGraficas();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox11.Text = "";
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox5.Text == "No activado")
            {
                nombres["preventivo"] = "No activado";
                nombres.Remove("tipo_de_preventivo");
                parametros.Remove("tiempo_entre_preventivos");
                parametros.Remove("disponibilidad_minima_admisible");
            }

            if (comboBox5.Text == "Fijo por tiempo")
            {
                FormDatos1 frm = new FormDatos1();
                nombres["preventivo"] = "Activado";
                nombres["tipo_de_preventivo"] = "Fijo por tiempo";
                parametros.Remove("disponibilidad_minima_admisible");
                SolicitarDatos(frm, "PREVENTIVO", "", "MANTENIMIENTO PREVENTIVO FIJO POR TIEMPO", "Tiempo entre Preventivos (Udes. de Tiempo)",
                                    "", "","", "","tiempo_entre_preventivos", "","", "", "", "", "");
            }

            if (comboBox5.Text == "Por Disponibilidad")
            {
                
                //parametros["disponibilidad_minima_admisible"]=100;
                
                FormDatos1 frm = new FormDatos1();
                nombres["preventivo"] = "Activado";
                nombres["tipo_de_preventivo"] = "Por Disponibilidad";
                parametros.Remove("tiempo_entre_preventivos");
                SolicitarDatos(frm, "PREVENTIVO", "", "MANTENIMIENTO POR DISPONIBILIDAD", "Disponibilidad Mínima Admisible (%)",
                                    "", "", "", "", "disponibilidad_minima_admisible", "", "", "", "", "", "");
               
            }

        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Ambito = "EFICIENCIA DEL MANTENIMIENTO: ";
            string palabra_clave = "% de Eficiencia del Mto.";
            string sufijo2 = "eficiencia_mto";
            string nombre_de_ley = "ley_eficiencia_mto";
            string nombre_de_campo_y_combo_Box = comboBox6.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box);           

        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox7.Text == "Disponibilidad")
            {
                //Gráfica de Disponibilidad
                if (Lista_Disponibilidad.Count()>1)
                {
                Form4 frm1 = new Form4();
                frm1.Datos_a_dibujar = Lista_Disponibilidad;
                frm1.Text = "SIM: Evolución de la Disponibilidad Operacional";
                frm1.rotulo_eje_x = "tiempo de simulacion";
                frm1.rotulo_eje_y = "Disponibilidad Operacional";
                frm1.rotulo_primer_dato = "Disponibilidad Operacional";
                frm1.primer_dato = resultados["Disponibilidad"];
                frm1.rotulo_segundo_dato = "";
                frm1.segundo_dato = 0;
                frm1.Show();
                }
            }

            if (comboBox7.Text == "MTBF")
            {
                //Gráfica del MTBF
                if (Lista_MTBF.Count() > 1)
                {
                    Form4 frm2 = new Form4();
                    frm2.Datos_a_dibujar = Lista_MTBF;
                    frm2.Text = "SIM: Evolución del Tiempo Medio Entre Fallos (MTBF)";
                    frm2.rotulo_eje_x = "tiempo de simulacion";
                    frm2.rotulo_eje_y = "MTBF";
                    frm2.rotulo_primer_dato = "Tiempo Medio Entre Fallos";
                    frm2.primer_dato = resultados["MTBF"];
                    frm2.rotulo_segundo_dato = "";
                    frm2.segundo_dato = 0;
                    frm2.Show();
                }
            }

            if (comboBox7.Text == "Número de Fallos")
            {
                //Gráfica del Número de Fallos
                if (Lista_Numero_de_Fallos.Count() > 1)
                {
                    Form4 frm2 = new Form4();
                    frm2.Datos_a_dibujar = Lista_Numero_de_Fallos;
                    frm2.Text = "SIM: Evolución del Número de Fallos";
                    frm2.rotulo_eje_x = "tiempo de simulacion";
                    frm2.rotulo_eje_y = "Nº de Fallos";
                    frm2.rotulo_primer_dato = "Número de Fallos";
                    frm2.primer_dato = resultados["Numero_de_Fallos"];
                    frm2.rotulo_segundo_dato = "";
                    frm2.segundo_dato = 0;
                    frm2.Show();
                }
            }

            if (comboBox7.Text == "Número de Preventivos")
            {
                //Gráfica del Número de Preventivos
                if (Lista_Numero_de_Preventivos.Count() > 1)
                {
                    Form4 frm2 = new Form4();
                    frm2.Datos_a_dibujar = Lista_Numero_de_Preventivos;
                    frm2.Text = "SIM: Evolución del Número de Mantenimientos Preventivos";
                    frm2.rotulo_eje_x = "tiempo de simulacion";
                    frm2.rotulo_eje_y = "Nº de Preventivos";
                    frm2.rotulo_primer_dato = "Número de Preventivos";
                    frm2.primer_dato = resultados["Numero_de_Preventivos"];
                    frm2.rotulo_segundo_dato = "";
                    frm2.segundo_dato = 0;
                    frm2.Show();
                }
            }

            if (comboBox7.Text == "MTTR")
            {
                //Gráfica del MTTR
                if (Lista_MTTR.Count() > 1)
                {
                    Form4 frm3 = new Form4();
                    frm3.Datos_a_dibujar = Lista_MTTR;
                    frm3.Text = "SIM: Evolución del Tiempo Medio de Recuperación/Reparación (MTTR)";
                    frm3.rotulo_eje_x = "tiempo de simulacion";
                    frm3.rotulo_eje_y = "MTTR";
                    frm3.rotulo_primer_dato = "Tiempo Medio Recuperación";
                    frm3.primer_dato = resultados["MTTR"];
                    frm3.rotulo_segundo_dato = "";
                    frm3.segundo_dato = 0;
                    frm3.Show();
                }
            }

            if (comboBox7.Text == "MTTR sin logística")
            {
                //Gráfica de Disponibilidad
                if (Lista_MTTR_SinLog.Count() > 1)
                {
                    Form4 frm1 = new Form4();
                    frm1.Datos_a_dibujar = Lista_MTTR_SinLog;
                    frm1.Text = "SIM: Evolución del MTTR sin tiempos de Logística";
                    frm1.rotulo_eje_x = "tiempo de simulacion";
                    frm1.rotulo_eje_y = "MTTR sin logística";
                    frm1.rotulo_primer_dato = "MTTR sin logística";
                    frm1.primer_dato = resultados["MTTR_SinLog"];
                    frm1.rotulo_segundo_dato = "";
                    frm1.segundo_dato = 0;
                    frm1.Show();
                }
            }


            if (comboBox7.Text == "Ratio MTTR/MTTR_sinlog")
            {
                //Gráfica de Disponibilidad
                if (Lista_RatioMTTR_vs_MTTR_SinLog.Count() > 1)
                {
                    Form4 frm1 = new Form4();
                    frm1.Datos_a_dibujar = Lista_RatioMTTR_vs_MTTR_SinLog;
                    frm1.Text = "SIM: Evolución del ratio MTTR/MTTR_sinlog";
                    frm1.rotulo_eje_x = "tiempo de simulacion";
                    frm1.rotulo_eje_y = "MTTR/MTTR_sinlog";
                    frm1.rotulo_primer_dato = "ratio MTTR/MTTR_sinlog";
                    frm1.primer_dato = resultados["ratio MTTR/MTTR_sinlog"];
                    frm1.rotulo_segundo_dato = "";
                    frm1.segundo_dato = 0;
                    frm1.Show();
                }
            }
                


            if (comboBox7.Text == "Intensidad de Fallos")
            {
                //Grafica de la Intensidad de Fallos
                if (Lista_IntensidadFallos.Count() > 1)
                {
                    Form4 frm4 = new Form4();
                    frm4.Datos_a_dibujar = Lista_IntensidadFallos;
                    frm4.Text = "SIM: Evolución de la Intensidad de Fallos";
                    frm4.rotulo_eje_x = "tiempo de simulacion";
                    frm4.rotulo_eje_y = "Intensidad de Fallos";
                    frm4.rotulo_primer_dato = "Intensidad de Fallos";
                    frm4.primer_dato = resultados["IntensidadFallos"];
                    frm4.rotulo_segundo_dato = "";
                    frm4.segundo_dato = 0;
                    frm4.Show();
                }
  
            }
            

            if (comboBox7.Text == "Ratio t_Correctivo/t_total_Mto")
            {
                if (Lista_RatioCorrectivo_vs_total.Count() > 1)
                {
                    Form4 frm5 = new Form4();
                    frm5.Datos_a_dibujar = Lista_RatioCorrectivo_vs_total;
                    frm5.Text = "SIM: Ratio Tiempo_en_Correctivo/Tiempo_Total_Mto";
                    frm5.rotulo_eje_x = "tiempo de simulacion";
                    frm5.rotulo_eje_y = "t_Corr/t_total";
                    frm5.rotulo_primer_dato = "Ratio t_Correctivo/t_Total_Mto";
                    frm5.primer_dato = resultados["Ratio t_Correctivo/t_Total_Mto"];
                    frm5.rotulo_segundo_dato = "";
                    frm5.segundo_dato = 0;
                    frm5.Show();
                }
            }

            if (comboBox7.Text == "Ratio t_Preventivo/t_total_Mto")
            {
                //Modificar variables para Coste Acumulado de Recuperacion
                if (Lista_RatioPreventivo_vs_total.Count() > 1)
                {
                    Form4 frm5 = new Form4();
                    frm5.Datos_a_dibujar = Lista_RatioPreventivo_vs_total;
                    frm5.Text = "SIM: Ratio Tiempo_en_Preventivo/Tiempo_Total_Mto";
                    frm5.rotulo_eje_x = "tiempo de simulacion";
                    frm5.rotulo_eje_y = "t_Prev/t_total";
                    frm5.rotulo_primer_dato = "Ratio t_Preventivo/t_Total_Mto";
                    frm5.primer_dato = resultados["Ratio t_Preventivo/t_Total_Mto"];
                    frm5.rotulo_segundo_dato = "";
                    frm5.segundo_dato = 0;
                    frm5.Show();
                }
            }

            if (comboBox7.Text == "Coste Acumulado Rec/Rep")
            {
                //Modificar variables para Coste Acumulado de Recuperacion
                if (Lista_CosteAcumuladoRecuperacion.Count() > 1)
                {
                    Form4 frm5 = new Form4();
                    frm5.Datos_a_dibujar = Lista_CosteAcumuladoRecuperacion;
                    frm5.Text = "SIM: Evolución del Coste Acumulado de Recuparación/Reparación (Mto+Perdida de produccion)";
                    frm5.rotulo_eje_x = "tiempo de simulacion";
                    frm5.rotulo_eje_y = "Coste Acumulado de Recuperacion (Mto+Perdida de produccion)";
                    frm5.rotulo_primer_dato = "Coste Total de Recuperación";
                    frm5.primer_dato = resultados["CosteTotalMantenimiento"] + resultados["CosteTotalPerdidaProduccion"];
                    frm5.rotulo_segundo_dato = "";
                    frm5.segundo_dato = 0;
                    frm5.Show();
                }
            }


            if (comboBox7.Text == "Coste Medio de Recuperacion")
            {                
                //Modificar variables para Coste Medio de Recuperacion
                if (Lista_CosteMedioRecuperacion.Count() > 1)
                {
                    Form4 frm6 = new Form4();
                    frm6.Datos_a_dibujar = Lista_CosteMedioRecuperacion;
                    frm6.rotulo_eje_x = "tiempo de simulacion";
                    frm6.rotulo_eje_y = "Coste Medio de Recuperacion";
                    frm6.rotulo_primer_dato = "Coste Medio de Recuperacion";
                    frm6.primer_dato = resultados["Coste Medio de Recuperacion"];
                    frm6.rotulo_segundo_dato = "";
                    frm6.segundo_dato = 0;
                    frm6.Show();
                }
            }
            
          
            if (comboBox7.Text == "     ")
            {

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            nombres["ley_func"] = "Exponencial";
            nombres["ley_paro"] = "Desglose de Fallos";
            nombres["ley_recu"] = "Siempre a Nuevo (GAN)";
            nombres["ley_coste"] = "Ninguna Ley";
            nombres["preventivo"] = "No activado";
            nombres["ley_eficiencia_mto"] = "Ninguna Ley";
            nombres["ley_paro_recon"] = "Fijo";
            nombres["ley_paro_diag"] = "Uniforme";
            nombres["ley_paro_prep"] = "Exponencial";
            nombres["ley_paro_desm"] = "Weibull2P";
            nombres["ley_paro_repa"] = "Normal";
            nombres["ley_paro_ensam"] = "Línea recta";
            nombres["ley_paro_verif"] = "Fijo";
            nombres["ley_paro_serv"] = "Uniforme";

            parametros["ley_func_param1"] = 0;
            parametros["ley_func_param2"] = 0.001;
            parametros["Minimo_func"] = 100;
            parametros["Maximo_func"] = 10000;
            parametros["Maximo_paro_diag"] = 40;
            parametros["Minimo_paro_diag"] = 20;
            parametros["paro_recon_Reduccion_si_Preventivo"] = 50;
            parametros["ley_paro_recon_param1"] = 30;
            parametros["paro_Reduccion_si_Preventivo"] = 20;
            parametros["paro_diag_Reduccion_si_Preventivo"] = 50;
            parametros["ley_paro_prep_param1"] = 10;
            parametros["ley_paro_prep_param2"] = 0.01;
            parametros["Minimo_paro_prep"] = 10;
            parametros["Maximo_paro_prep"] = 1000;
            parametros["paro_prep_Reduccion_si_Preventivo"] = 50;
            parametros["ley_paro_desm_param1"] = 1.5;
            parametros["ley_paro_desm_param2"] = 400;
            parametros["Minimo_paro_desm"] = 50;
            parametros["Maximo_paro_desm"] = 700;
            parametros["paro_desm_Reduccion_si_Preventivo"] = 50;
            parametros["ley_paro_repa_param1"] = 30;
            parametros["ley_paro_repa_param2"] = 3;
            parametros["Minimo_paro_repa"] = 15;
            parametros["Maximo_paro_repa"] = 45;
            parametros["paro_repa_Reduccion_si_Preventivo"] = 50;
            parametros["X1_paro_ensam"] = 0;
            parametros["Y1_paro_ensam"] = 20;
            parametros["X2_paro_ensam"] = 60000;
            parametros["Y2_paro_ensam"] = 50;
            parametros["paro_ensam_Reduccion_si_Preventivo"] = 50;
            parametros["ley_paro_verif_param1"] = 30;
            parametros["paro_verif_Reduccion_si_Preventivo"] = 50;
            parametros["Minimo_paro_serv"] = 20;
            parametros["Maximo_paro_serv"] = 40;
            parametros["paro_serv_Reduccion_si_Preventivo"] = 50;

            textBox11.Enabled = true;

            textBox10.Text = "20000";

            foreach (string key in nombres.Keys)
            {
                textBox11.Text += "\r\n" + key + " = " + nombres[key].ToString();
            }

            textBox11.Text += "\r\n";

            foreach (string key in parametros.Keys)
            {
                textBox11.Text += "\r\n" + key + " = " + parametros[key].ToString();
            }  
        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox8.Text == "Caso1")
            {
                nombres["ley_func"] = "Exponencial";
                nombres["ley_paro"] = "Uniforme";
                nombres["ley_recu"] = "Siempre a Nuevo (GAN)";

                parametros["ley_func_param1"] = 0;
                parametros["ley_func_param2"] = 0.0001246;
                parametros["Minimo_func"] = 2588;
                parametros["Maximo_func"] = 400000;
                parametros["Maximo_paro"] = 136;
                parametros["Minimo_paro"] = 1;

                textBox11.Enabled = true;
                textBox10.Text = "400000";
            }

            if (comboBox8.Text == "Caso2")
            {
                
                nombres["ley_func"] = "Exponencial";
                nombres["ley_paro"] = "Weibull2P";
                nombres["ley_recu"] = "Siempre a Nuevo (GAN)";

                parametros["ley_func_param1"] = 0;
                parametros["ley_func_param2"] = 0.0001246;
                parametros["Minimo_func"] = 2588;
                parametros["Maximo_func"] = 400000;
                parametros["ley_paro_param1"] = 0.588;
                parametros["ley_paro_param2"] = 15.073;
                parametros["Minimo_paro"] = 1;
                parametros["Maximo_paro"] = 136;

                textBox11.Enabled = true;
                textBox10.Text = "400000";

            }

            if (comboBox8.Text == "Caso3")
            {
                
                nombres["ley_func"] = "Exponencial";
                nombres["ley_paro"] = "Weibull2P";
                nombres["ley_recu"] = "Siempre a Nuevo (GAN)";

                parametros["Maximo_func"] = 400000;
                parametros["Minimo_func"] = 2588;
                parametros["ley_func_param2"] = 0.0001526;
                parametros["ley_func_param1"] = 0;
                parametros["ley_paro_param1"] = 0.588;
                parametros["ley_paro_param2"] = 15.073;
                parametros["Minimo_paro"] = 1;
                parametros["Maximo_paro"] = 136;

                textBox11.Enabled = true;
                textBox10.Text = "400000";
            }

            if (comboBox8.Text == "Caso4")
            {
                nombres["ley_func"] = "Exponencial";
                nombres["ley_paro"] = "Weibull2P";
                nombres["ley_recu"] = "Según tiempo (BAO)";

                parametros["ley_func_param1"] = 0;
                parametros["ley_func_param2"] = 0.0001526;
                parametros["Minimo_func"] = 2588;
                parametros["Maximo_func"] = 400000;
                parametros["ley_paro_param1"] = 0.588;
                parametros["ley_paro_param2"] = 15.073;
                parametros["Minimo_paro"] = 1;
                parametros["Maximo_paro"] = 136;

                textBox11.Enabled = true;
                textBox10.Text = "20000";
            }

            if (comboBox8.Text == "Caso5")
            {
                
                nombres["ley_func"] = "Exponencial";
                nombres["ley_paro"] = "Weibull2P";
                nombres["ley_recu"] = "Línea recta";

                parametros["ley_func_param1"] = 0;
                parametros["ley_func_param2"] = 0.0001526;
                parametros["Minimo_func"] = 2588;
                parametros["Maximo_func"] = 400000;
                parametros["ley_paro_param1"] = 0.588;
                parametros["ley_paro_param2"] = 15.073;
                parametros["Minimo_paro"] = 1;
                parametros["Maximo_paro"] = 136;
                parametros["Y2_recu"] = 0.6;
                parametros["X2_recu"] = 87600;
                parametros["Y1_recu"] = 1;
                parametros["X1_recu"] = 0;

                textBox11.Enabled = true;
                textBox10.Text = "87600";
            }

            if (comboBox8.Text == "Caso6")
            {
                
                nombres["ley_func"] = "Exponencial";
                nombres["ley_paro"] = "Weibull2P";
                nombres["ley_recu"] = "Línea recta";
                nombres["ley_eficiencia_mto"] = "Uniforme";

                parametros["ley_func_param1"] = 0;
                parametros["ley_func_param2"] = 0.0001526;
                parametros["Minimo_func"] = 2588;
                parametros["Maximo_func"] = 400000;
                parametros["ley_paro_param1"] = 0.588;
                parametros["ley_paro_param2"] = 15.073;
                parametros["Minimo_paro"] = 1;
                parametros["Maximo_paro"] = 136;
                parametros["Y2_recu"] = 0.6;
                parametros["X2_recu"] = 87600;
                parametros["Y1_recu"] = 1;
                parametros["X1_recu"] = 0;
                parametros["Minimo_eficiencia_mto"] = 90;
                parametros["Maximo_eficiencia_mto"] = 100;

                textBox11.Enabled = true;
                textBox10.Text = "87600";
            }

            if (comboBox8.Text == "Caso7")
            {
                
                nombres["ley_func"] = "Exponencial";
                nombres["ley_paro"] = "Weibull2P";
                nombres["ley_recu"] = "Línea recta";
                nombres["ley_eficiencia_mto"] = "Uniforme";
                nombres["preventivo"] = "Activado";
                nombres["tipo_de_preventivo"] = "Fijo por tiempo";

                parametros["ley_func_param1"] = 0;
                parametros["ley_func_param2"] = 0.0001526;
                parametros["Minimo_func"] = 2588;
                parametros["Maximo_func"] = 400000;
                parametros["ley_paro_param1"] = 0.588;
                parametros["ley_paro_param2"] = 15.073;
                parametros["Minimo_paro"] = 1;
                parametros["Maximo_paro"] = 136;
                parametros["Y2_recu"] = 0.6;
                parametros["X2_recu"] = 87600;
                parametros["Y1_recu"] = 1;
                parametros["X1_recu"] = 0;
                parametros["Minimo_eficiencia_mto"] = 90;
                parametros["Maximo_eficiencia_mto"] = 100;
                parametros["tiempo_entre_preventivos"] = 8760;

                textBox11.Enabled = true;
                textBox10.Text = "87600";

            }

            if (comboBox8.Text == "Caso8")
            {
                nombres["ley_func"] = "Exponencial";
                nombres["ley_paro"] = "Weibull2P";
                nombres["ley_recu"] = "Línea recta";
                nombres["ley_eficiencia_mto"] = "Uniforme";
                nombres["preventivo"] = "Activado";
                nombres["tipo_de_preventivo"] = "Fijo por tiempo";

                parametros["ley_func_param1"] = 0;
                parametros["ley_func_param2"] = 0.0001526;
                parametros["Minimo_func"] = 2588;
                parametros["Maximo_func"] = 400000;
                parametros["Maximo_paro"] = 136;
                parametros["Minimo_paro"] = 1;
                parametros["ley_paro_param2"] = 15.073;
                parametros["ley_paro_param1"] = 0.588;
                parametros["Y2_recu"] = 0.6;
                parametros["X2_recu"] = 87600;
                parametros["Y1_recu"] = 1;
                parametros["X1_recu"] = 0;
                parametros["Minimo_eficiencia_mto"] = 90;
                parametros["Maximo_eficiencia_mto"] = 100;
                parametros["tiempo_entre_preventivos"] = 8760;
                parametros["paro_Reduccion_si_Preventivo"] = 50;

                textBox11.Enabled = true;
                textBox10.Text = "87600";
            }


            if (comboBox8.Text == "Caso9")
            {
                nombres["ley_func"] = "Exponencial";
                nombres["ley_paro"] = "Weibull2P";
                nombres["ley_recu"] = "Línea recta";
                nombres["ley_eficiencia_mto"] = "Uniforme";
                nombres["preventivo"] = "Activado";
                nombres["tipo_de_preventivo"] = "Por Disponibilidad";

                parametros["ley_func_param1"] = 0;
                parametros["ley_func_param2"] = 0.0001526;
                parametros["Minimo_func"] = 2588;
                parametros["Maximo_func"] = 400000;
                parametros["Maximo_paro"] = 136;
                parametros["Minimo_paro"] = 1;
                parametros["ley_paro_param2"] = 15.073;
                parametros["ley_paro_param1"] = 0.588;
                parametros["Y2_recu"] = 0.6;
                parametros["X2_recu"] = 87600;
                parametros["Y1_recu"] = 1;
                parametros["X1_recu"] = 0;
                parametros["Minimo_eficiencia_mto"] = 90;
                parametros["Maximo_eficiencia_mto"] = 100;
                parametros["disponibilidad_minima_admisible"] = 30;
                parametros["paro_Reduccion_si_Preventivo"] = 50;

                textBox11.Enabled = true;
                textBox10.Text = "87600";
            }

            if (comboBox8.Text == "Caso10")
            {
                nombres["ley_func"] = "Exponencial";
                nombres["ley_paro"] = "Desglose de Fallos";
                nombres["ley_recu"] = "Línea recta";
                nombres["ley_eficiencia_mto"] = "Uniforme";
                nombres["preventivo"] = "Activado";
                nombres["tipo_de_preventivo"] = "Por Disponibilidad";
                nombres["ley_paro_prep"] = "Uniforme";
                nombres["ley_paro_repa"] = "Uniforme";
                nombres["ley_coste"] = "Desglose de Costes";
                nombres["ley_coste_prep"] = "Fijo";
                nombres["ley_coste_repa"] = "Fijo";

                parametros["ley_func_param1"] = 0;
                parametros["ley_func_param2"] = 0.0001526;
                parametros["Minimo_func"] = 2588;
                parametros["Maximo_func"] = 400000;
                parametros["Minimo_paro_prep"] = 5;
                parametros["Maximo_paro_prep"] = 10;
                parametros["paro_prep_Reduccion_si_Preventivo"] = 50;
                parametros["Maximo_paro_repa"] = 25;
                parametros["Y2_recu"] = 0.6;
                parametros["X2_recu"] = 87600;
                parametros["Y1_recu"] = 1;
                parametros["X1_recu"] = 0;
                parametros["Minimo_eficiencia_mto"] = 90;
                parametros["Maximo_eficiencia_mto"] = 100;
                parametros["disponibilidad_minima_admisible"] = 30;
                parametros["paro_Reduccion_si_Preventivo"] = 50;
                parametros["Minimo_paro_repa"] = 15;
                parametros["paro_repa_Reduccion_si_Preventivo"] = 30;
                parametros["ley_coste_prep_param1"] = 16;
                parametros["coste_prep_Perdida_Prod_por_Ud_tiempo"] = 5;
                parametros["coste_prep_Reduccion_si_Preventivo"] = 50;
                parametros["ley_coste_repa_param1"] = 16;
                parametros["coste_repa_Perdida_Prod_por_Ud_tiempo"] = 5;
                parametros["coste_repa_Reduccion_si_Preventivo"] = 30;

                textBox11.Enabled = true;
                textBox10.Text = "87600";
            }


            if (comboBox8.Text == "Caso11")
            {
                textBox11.Enabled = true;
                textBox10.Text = "20000";

                nombres["ley_func"] = "Exponencial";
                nombres["ley_paro"] = "Normal";
                nombres["ley_recu"] = "Siempre a Nuevo (GAN)";
                nombres["ley_coste"] = "Ninguna Ley";
                nombres["preventivo"] = "No activado";
                nombres["ley_eficiencia_mto"] = "Ninguna Ley";

                parametros["ley_func_param1"] = 0;
                parametros["ley_func_param2"] = 0.001;
                parametros["Minimo_func"] = 100;
                parametros["Maximo_func"] = 10000;
                parametros["ley_paro_param1"] = 200;
                parametros["ley_paro_param2"] = 20;
                parametros["Minimo_paro"] = 100;
                parametros["Maximo_paro"] = 10000;
                parametros["paro_Reduccion_si_Preventivo"] = 20;

                foreach (string key in nombres.Keys)
                {
                    textBox11.Text += "\r\n" + key + " = " + nombres[key].ToString();
                }

                textBox11.Text += "\r\n";

                foreach (string key in parametros.Keys)
                {
                    textBox11.Text += "\r\n" + key + " = " + parametros[key].ToString();
                }  

            }

            if (comboBox8.Text == "Caso12")
            {
                textBox11.Enabled = true;
                textBox10.Text = "50000";

                nombres["ley_func"] = "Exponencial";
                nombres["ley_paro"] = "Desglose de Fallos";
                nombres["ley_recu"] = "Siempre a Nuevo (GAN)";
                nombres["ley_coste"] = "Ninguna Ley";
                nombres["preventivo"] = "No activado";
                nombres["ley_eficiencia_mto"] = "Ninguna Ley";
                nombres["ley_paro_recon"] = "Fijo";
                nombres["ley_paro_diag"] = "Uniforme";
                nombres["ley_paro_prep"] = "Exponencial";
                nombres["ley_paro_desm"] = "Weibull2P";
                nombres["ley_paro_repa"] = "Normal";
                nombres["ley_paro_ensam"] = "Línea recta";
                nombres["ley_paro_verif"] = "Fijo";
                nombres["ley_paro_serv"] = "Uniforme";

                parametros["ley_func_param1"] = 0;
                parametros["ley_func_param2"] = 0.001;
                parametros["Minimo_func"] = 100;
                parametros["Maximo_func"] = 10000;
                parametros["Maximo_paro_diag"] = 40;
                parametros["Minimo_paro_diag"] = 20;
                parametros["paro_recon_Reduccion_si_Preventivo"] = 50;
                parametros["ley_paro_recon_param1"] = 30;
                parametros["paro_Reduccion_si_Preventivo"] = 20;
                parametros["paro_diag_Reduccion_si_Preventivo"] = 50;
                parametros["ley_paro_prep_param1"] = 10;
                parametros["ley_paro_prep_param2"] = 0.01;
                parametros["Minimo_paro_prep"] = 10;
                parametros["Maximo_paro_prep"] = 1000;
                parametros["paro_prep_Reduccion_si_Preventivo"] = 50;
                parametros["ley_paro_desm_param1"] = 1.5;
                parametros["ley_paro_desm_param2"] = 400;
                parametros["Minimo_paro_desm"] = 50;
                parametros["Maximo_paro_desm"] = 700;
                parametros["paro_desm_Reduccion_si_Preventivo"] = 50;
                parametros["ley_paro_repa_param1"] = 30;
                parametros["ley_paro_repa_param2"] = 3;
                parametros["Minimo_paro_repa"] = 15;
                parametros["Maximo_paro_repa"] = 45;
                parametros["paro_repa_Reduccion_si_Preventivo"] = 50;
                parametros["X1_paro_ensam"] = 0;
                parametros["Y1_paro_ensam"] = 20;
                parametros["X2_paro_ensam"] = 60000;
                parametros["Y2_paro_ensam"] = 50;
                parametros["paro_ensam_Reduccion_si_Preventivo"] = 50;
                parametros["ley_paro_verif_param1"] = 30;
                parametros["paro_verif_Reduccion_si_Preventivo"] = 50;
                parametros["Minimo_paro_serv"] = 20;
                parametros["Maximo_paro_serv"] = 40;
                parametros["paro_serv_Reduccion_si_Preventivo"] = 50;

                foreach (string key in nombres.Keys)
                {
                    textBox11.Text += "\r\n" + key + " = " + nombres[key].ToString();
                }

                textBox11.Text += "\r\n";

                foreach (string key in parametros.Keys)
                {
                    textBox11.Text += "\r\n" + key + " = " + parametros[key].ToString();
                }  

            }

        }        
 
    }
}

