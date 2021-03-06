﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;

namespace SIM
{
    public partial class FormFuncionaFallaAmpliado : Form
    {

        /** Attributes **/

        private Hashtable _parameters = new Hashtable();
        private Hashtable _widgets;
        private Hashtable _defaultValues;
        private Hashtable _subformButtons;
        private bool _silentMode;

        Dictionary<string, double> resultados = new Dictionary<string, double>();
        Dictionary<double, double> programa_mto_preventivo = new Dictionary<double, double>();

        /* Listas que contendrá los puntos de cada gráfica */
   
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


        /* Constructor */
        public FormFuncionaFallaAmpliado()
        {
            InitializeComponent();
            //Aseguramos que utiliza la configuración española para numeros decimales
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-ES");
            //Inicializamos tabla de widgets
            
            _widgets = new Hashtable { 
                {"ley_func", comboBox1},
                {"ley_paro", comboBox2},
                {"ley_recu", comboBox3},
                {"ley_coste", comboBox4},
                {"preventivo", comboBox5},
                {"ley_eficiencia_mto", comboBox6},
                {"ley_paro_recon", comboBox_T_Rec},
                {"ley_paro_diag", comboBox_T_Diag},
                {"ley_paro_prep", comboBox_T_Prep},
                {"ley_paro_desm", comboBox_T_Desm},
                {"ley_paro_repa", comboBox_T_Rep},
                {"ley_paro_ensam", comboBox_T_Ensam},
                {"ley_paro_verif", comboBox_T_Verif},
                {"ley_paro_serv", comboBox_T_Serv},
                {"ley_coste_recon", comboBox_C_Rec},
                {"ley_coste_diag", comboBox_C_Diag},
                {"ley_coste_prep", comboBox_C_Prep},
                {"ley_coste_desm", comboBox_C_Desm},
                {"ley_coste_repa", comboBox_C_Rep},
                {"ley_coste_ensam", comboBox_C_Ensam},
                {"ley_coste_verif", comboBox_C_Verif},
                {"ley_coste_serv", comboBox_C_Serv},
                {"grafica", comboBox7},
                {"caso", comboBox8}
            };

            _defaultValues = new Hashtable {
                {"ley_func", "Ninguna Ley"},
                {"ley_paro", "Ninguna Ley"},
                {"ley_recu", "Ninguna Ley"},
                {"ley_coste", "Ninguna Ley"},
                {"preventivo", "No activado"},
                {"ley_eficiencia_mto", "Ninguna Ley"},                  
                {"ley_paro_recon", "Ninguna Ley"},
                {"ley_paro_diag", "Ninguna Ley"},
                {"ley_paro_prep", "Ninguna Ley"},
                {"ley_paro_desm", "Ninguna Ley"},
                {"ley_paro_repa", "Ninguna Ley"},
                {"ley_paro_ensam", "Ninguna Ley"},
                {"ley_paro_verif", "Ninguna Ley"},
                {"ley_paro_serv", "Ninguna Ley"},
                {"ley_coste_recon", "Ninguna Ley"},
                {"ley_coste_diag", "Ninguna Ley"},
                {"ley_coste_prep", "Ninguna Ley"},
                {"ley_coste_desm", "Ninguna Ley"},
                {"ley_coste_repa", "Ninguna Ley"},
                {"ley_coste_ensam", "Ninguna Ley"},
                {"ley_coste_verif", "Ninguna Ley"},
                {"ley_coste_serv", "Ninguna Ley"}
            };

            _subformButtons = new Hashtable { 
                {"ley_func", buttonLeyfunc},
                {"ley_paro", buttonLeyparo},
                {"ley_recu", buttonLeyrecu},
                {"ley_coste", buttonLeycoste},
                {"preventivo", buttonPreventivo},
                {"ley_eficiencia_mto", buttonLeyeficienciamto},
                {"ley_paro_recon", buttonLeyparorecon},
                {"ley_paro_diag", buttonLeyparodiag},
                {"ley_paro_prep", buttonLeyparoprep},
                {"ley_paro_desm", buttonLeyparodesm},
                {"ley_paro_repa", buttonLeyparorepa},
                {"ley_paro_ensam", buttonLeyparoensam},
                {"ley_paro_verif", buttonLeyparoverif},
                {"ley_paro_serv", buttonLeyparoserv},
                {"ley_coste_recon", buttonLeycosterecon},
                {"ley_coste_diag", buttonLeycostediag},
                {"ley_coste_prep", buttonLeycosteprep},
                {"ley_coste_desm", buttonLeycostedesm},
                {"ley_coste_repa", buttonLeycosterepa},
                {"ley_coste_ensam", buttonLeycosteensam},
                {"ley_coste_verif", buttonLeycosteverif},
                {"ley_coste_serv", buttonLeycosteserv}
            };
        }

        /* Load */
        private void FormFuncionaFallaAmpliado_Load(object sender, EventArgs e)
        {
            reset();
        }

        /* Silent mode */

        private void StartSilentMode()
        {
            SilentMode = true;
        }

        private void StopSilentMode()
        {
            SilentMode = false;
        }

        private bool SilentMode
        {
            get
            {
                return this._silentMode;
            }
            set
            {
                this._silentMode = value;
            }
        }

        /* Default values */

        private void DefaultParameters()
        {
            RemoveParameters(ParameterNames());
            AddParameters(_defaultValues);
        }

        /* Reset */

        private void reset()
        {
            StartSilentMode();
            DefaultParameters();
            StopSilentMode();

            // TextBox de resultados
            textBox5.Text = "";
            textBox5.Enabled = false;

            // Log
            textBox11.Text = "";
            textBox11.Enabled = false;

            // Tiempo a simular y repeticiones de la simulación
            textBox10.Text = "";
            textBox10.Enabled = true;
            button4.Enabled = true;

            // Progressbar y selección de gráfica
            progressBar1.Visible = false;
            ResetComboBox7();

            // Selección de casos predefinidos
            comboBox8.Text = "Casos prácticos predefinidos";
        }

        /* Reset selección de gráfica */

        private void ResetComboBox7()
        {
            comboBox7.Visible = false;
            comboBox7.Text = "Elegir gráfico";
        }

        /* Tratamiento de los parámetros */

        private List<String> ParameterNames()
        {
            List<String> pnames = new List<String> { };
            foreach (string name in _parameters.Keys)
            {
                pnames.Add(name);
            }
            return pnames;
        }

        private void AddParameters(Hashtable parameters)
        {
            foreach (DictionaryEntry parameter in parameters)
            {
                _parameters.Remove(parameter.Key);
                _parameters.Add(parameter.Key, parameter.Value);
                if (_widgets.ContainsKey(parameter.Key))
                    SetWidgetValue(_widgets[parameter.Key], (string)parameter.Value);
            }
        }

        private void RemoveParameters(List<String> parameters)
        {
            foreach (string parameter in parameters)
            {
                _parameters.Remove(parameter);
                if (_widgets.ContainsKey(parameter))
                    SetWidgetValue(_widgets[parameter], "");
            }
        }

        private void SetWidgetValue(Object widget, string value)
        {
            Type type = widget.GetType();
            switch (type.ToString())
            {
                case "System.Windows.Forms.ComboBox":
                    ComboBox comboObj = (ComboBox)widget;
                    comboObj.Text = value;
                    break;
                case "System.Windows.Forms.TextBox":
                    TextBox textObj = (TextBox)widget;
                    textObj.Text = value;
                    break;
            }
        }


        /** Buttons **/

        /* Botón cerrar */
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        /* Botón reset */
        private void button_Reset_Click(object sender, EventArgs e)
        {
            reset();
        }

        /* Botón simular */
        private void button4_Click(object sender, EventArgs e)
        {
            Simular();
        }

        /* Botón log */ 
        private void button_Parametros_Click(object sender, EventArgs e)
        {

            textBox11.Enabled = true;
            foreach (string key in _parameters.Keys)
            {
                textBox11.Text += "\r\n" + "_parameters[" + key + "] = " + " " + _parameters[key].ToString() + ";";
            }
        }
        
        /* Botón limpiar log */
        private void button3_Click(object sender, EventArgs e)
        {
            textBox11.Text = "";
        }


        /* Subform buttons */

        private void buttonLeyfunc_Click(object sender, EventArgs e)
        {
            comboBox1Actions(updateParams: false);
        }

        private void buttonLeyparo_Click(object sender, EventArgs e)
        {
            comboBox2Actions(updateParams: false);
        }

        private void buttonLeyrecu_Click(object sender, EventArgs e)
        {
            comboBox3Actions(updateParams: false);
        }

        private void buttonLeycoste_Click(object sender, EventArgs e)
        {
            comboBox4Actions(updateParams: false);
        }

        private void buttonPreventivo_Click(object sender, EventArgs e)
        {
            comboBox5Actions(updateParams: false);
        }

        private void buttonLeyeficienciamto_Click(object sender, EventArgs e)
        {
            comboBox6Actions(updateParams: false);
        }

        /* Subform buttons (desgloce tiempos) */

        private void buttonLeyparorecon_Click(object sender, EventArgs e)
        {
            comboBox_T_RecActions(updateParams: false);
        }

        private void buttonLeyparodiag_Click(object sender, EventArgs e)
        {
            comboBox_T_DiagActions(updateParams: false);
        }

        private void buttonLeyparoprep_Click(object sender, EventArgs e)
        {
            comboBox_T_PrepActions(updateParams: false);
        }

        private void buttonLeyparodesm_Click(object sender, EventArgs e)
        {
            comboBox_T_DesmActions(updateParams: false);
        }

        private void buttonLeyparorepa_Click(object sender, EventArgs e)
        {
            comboBox_T_RepActions(updateParams: false);
        }

        private void buttonLeyparoensam_Click(object sender, EventArgs e)
        {
            comboBox_T_EnsamActions(updateParams: false);
        }

        private void buttonLeyparoverif_Click(object sender, EventArgs e)
        {
            comboBox_T_VerifActions(updateParams: false);
        }

        private void buttonLeyparoserv_Click(object sender, EventArgs e)
        {
            comboBox_T_ServActions(updateParams: false);
        }

        /* Subform buttons (desgloce costes)*/

        private void buttonLeycosterecon_Click(object sender, EventArgs e)
        {
            comboBox_C_RecActions(updateParams: false);
        }

        private void buttonLeycostediag_Click(object sender, EventArgs e)
        {
            comboBox_C_DiagActions(updateParams: false);
        }

        private void buttonLeycosteprep_Click(object sender, EventArgs e)
        {
            comboBox_C_PrepActions(updateParams: false);
        }

        private void buttonLeycostedesm_Click(object sender, EventArgs e)
        {
            comboBox_C_DesmActions(updateParams: false);
        }

        private void buttonLeycosterepa_Click(object sender, EventArgs e)
        {
            comboBox_C_RepActions(updateParams: false);
        }

        private void buttonLeycosteensam_Click(object sender, EventArgs e)
        {
            comboBox_C_EnsamActions(updateParams: false);
        }

        private void buttonLeycosteverif_Click(object sender, EventArgs e)
        {
            comboBox_C_VerifActions(updateParams: false);
        }

        private void buttonLeycosteserv_Click(object sender, EventArgs e)
        {
            comboBox_C_ServActions(updateParams: false);
        }


        /** ComboBoxes **/

        /* Comboboxes del formulario principal */

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1Actions();
        }

        private void comboBox1Actions(bool updateParams = true)
        {
            string Ambito = "FUNCIONAMIENTO: ";
            string palabra_clave = "Funcionamiento";
            string sufijo2 = "func";
            string nombre_de_ley = "ley_func";
            string nombre_de_campo_y_combo_Box = comboBox1.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box, updateParams);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2Actions();
        }

        private void comboBox2Actions(bool updateParams = true)
        {           
            DeshabilitarComboBoxDesgloseTiempoFallo();

            string Ambito = "FALLO/PARADA: ";
            string palabra_clave = "Fallo_Parada";
            string sufijo2 = "paro";
            string nombre_de_ley = "ley_paro";
            string nombre_de_campo_y_combo_Box = comboBox2.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box, updateParams);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3Actions();
        }

        private void comboBox3Actions(bool updateParams = true)
        {
            string Ambito = "REPARACIÓN/RECUPERACIÓN: ";
            string palabra_clave = "Recuperación";
            string sufijo2 = "recu";
            string nombre_de_ley = "ley_recu";
            string nombre_de_campo_y_combo_Box = comboBox3.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box, updateParams);
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox4Actions();
        }

        private void comboBox4Actions(bool updateParams = true)
        {
         
            DeshabilitarComboBoxDesgloseCoste();

            string Ambito = "COSTE SIN DESGLOSE: ";
            string palabra_clave = "Coste_sin_desglose";
            string sufijo2 = "coste";
            string nombre_de_ley = "ley_coste";
            string nombre_de_campo_y_combo_Box = comboBox4.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box, updateParams);
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox5Actions();
        }

        private void comboBox5Actions(bool updateParams = true)
        {
            string Ambito = "";
            string palabra_clave = "";
            string sufijo2 = "";
            string nombre_de_ley = "preventivo";
            string nombre_de_campo_y_combo_Box = comboBox5.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box, updateParams);
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox6Actions();
        }

        private void comboBox6Actions(bool updateParams = true)
        {
            string Ambito = "EFICIENCIA DEL MANTENIMIENTO: ";
            string palabra_clave = "% de Eficiencia del Mto.";
            string sufijo2 = "eficiencia_mto";
            string nombre_de_ley = "ley_eficiencia_mto";
            string nombre_de_campo_y_combo_Box = comboBox6.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box, updateParams);
        }

        /* ComboBoxes desgloce de tiempos */

        private void comboBox_T_Rec_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox_T_RecActions();   
        }

        private void comboBox_T_RecActions(bool updateParams = true)
        {
            string Ambito = "DESGLOSE DE FALLO/PARADA: ";
            string palabra_clave = "Reconocimiento";
            string sufijo2 = "paro_recon";
            string nombre_de_ley = "ley_paro_recon";
            string nombre_de_campo_y_combo_Box = comboBox_T_Rec.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box, updateParams);        
        }

        private void comboBox_T_Diag_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox_T_DiagActions();
        }

        private void comboBox_T_DiagActions(bool updateParams = true)
        { 
            string Ambito = "DESGLOSE DE FALLO/PARADA: ";
            string palabra_clave = "Diagnostico";
            string sufijo2 = "paro_diag";
            string nombre_de_ley = "ley_paro_diag";
            string nombre_de_campo_y_combo_Box = comboBox_T_Diag.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box, updateParams);
        }

        private void comboBox_T_Prep_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox_T_PrepActions();
        }

        private void comboBox_T_PrepActions(bool updateParams = true)
        {
            string Ambito = "DESGLOSE DE FALLO/PARADA: ";
            string palabra_clave = "Preparacion";
            string sufijo2 = "paro_prep";
            string nombre_de_ley = "ley_paro_prep";
            string nombre_de_campo_y_combo_Box = comboBox_T_Prep.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box, updateParams);        
        }

        private void comboBox_T_Desm_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox_T_DesmActions();
        }

        private void comboBox_T_DesmActions(bool updateParams = true)
        {
            string Ambito = "DESGLOSE DE FALLO/PARADA: ";
            string palabra_clave = "Desmantelamiento";
            string sufijo2 = "paro_desm";
            string nombre_de_ley = "ley_paro_desm";
            string nombre_de_campo_y_combo_Box = comboBox_T_Desm.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box, updateParams);        
        }

        private void comboBox_T_Rep_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox_T_RepActions();
        }
        
        private void comboBox_T_RepActions(bool updateParams = true)
        {
            string Ambito = "DESGLOSE DE FALLO/PARADA: ";
            string palabra_clave = "Reparacion";
            string sufijo2 = "paro_repa";
            string nombre_de_ley = "ley_paro_repa";
            string nombre_de_campo_y_combo_Box = comboBox_T_Rep.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box, updateParams);
        }
       
        private void comboBox_T_Ensam_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox_T_EnsamActions();
        }
        
        private void comboBox_T_EnsamActions(bool updateParams = true)
        {
            string Ambito = "DESGLOSE DE FALLO/PARADA: ";
            string palabra_clave = "Ensamblaje";
            string sufijo2 = "paro_ensam";
            string nombre_de_ley = "ley_paro_ensam";
            string nombre_de_campo_y_combo_Box = comboBox_T_Ensam.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box, updateParams);
        }

        private void comboBox_T_Verif_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox_T_VerifActions();
        }

        private void comboBox_T_VerifActions(bool updateParams = true)
        {
            string Ambito = "DESGLOSE DE FALLO/PARADA: ";
            string palabra_clave = "Verificacion";
            string sufijo2 = "paro_verif";
            string nombre_de_ley = "ley_paro_verif";
            string nombre_de_campo_y_combo_Box = comboBox_T_Verif.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box, updateParams); 
        }

        private void comboBox_T_Serv_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox_T_ServActions();       
        }
        
        private void comboBox_T_ServActions(bool updateParams = true)
        {
            string Ambito = "DESGLOSE DE FALLO/PARADA: ";
            string palabra_clave = "Puesta en Servicio";
            string sufijo2 = "paro_serv";
            string nombre_de_ley = "ley_paro_serv";
            string nombre_de_campo_y_combo_Box = comboBox_T_Serv.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box, updateParams);
        }
        
        /* ComboBoxes desgloce de costes */

        private void comboBox_C_Rec_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox_C_RecActions();
        }

        private void comboBox_C_RecActions(bool updateParams = true)
        {
            string Ambito = "COSTE DESGLOSADO: ";
            string palabra_clave = "Coste de Reconocimiento";
            string sufijo2 = "coste_recon";
            string nombre_de_ley = "ley_coste_recon";
            string nombre_de_campo_y_combo_Box = comboBox_C_Rec.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box, updateParams);
        }

        private void comboBox_C_Diag_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox_C_DiagActions();     
        }

        private void comboBox_C_DiagActions(bool updateParams = true)
        {
            string Ambito = "COSTE DESGLOSADO: ";
            string palabra_clave = "Coste de Diagnostico";
            string sufijo2 = "coste_diag";
            string nombre_de_ley = "ley_coste_diag";
            string nombre_de_campo_y_combo_Box = comboBox_C_Diag.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box, updateParams); 
        }

        private void comboBox_C_Prep_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox_C_PrepActions();
        }

        private void comboBox_C_PrepActions(bool updateParams = true)
        {
            string Ambito = "COSTE DESGLOSADO: ";
            string palabra_clave = "Coste de Preparacion";
            string sufijo2 = "coste_prep";
            string nombre_de_ley = "ley_coste_prep";
            string nombre_de_campo_y_combo_Box = comboBox_C_Prep.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box, updateParams);
        }

        private void comboBox_C_Desm_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox_C_DesmActions();            
        }

        private void comboBox_C_DesmActions(bool updateParams = true)
        {
            string Ambito = "COSTE DESGLOSADO: ";
            string palabra_clave = "Coste de Desmantelamiento";
            string sufijo2 = "coste_desm";
            string nombre_de_ley = "ley_coste_desm";
            string nombre_de_campo_y_combo_Box = comboBox_C_Desm.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box, updateParams);
        }

        private void comboBox_C_Rep_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox_C_RepActions();           
        }

        private void comboBox_C_RepActions(bool updateParams = true)
        {
            string Ambito = "COSTE DESGLOSADO: ";
            string palabra_clave = "Coste de Reparacion";
            string sufijo2 = "coste_repa";
            string nombre_de_ley = "ley_coste_repa";
            string nombre_de_campo_y_combo_Box = comboBox_C_Rep.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box, updateParams);
        }

        private void comboBox_C_Ensam_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox_C_EnsamActions();         
        }

        private void comboBox_C_EnsamActions(bool updateParams = true)
        {
            string Ambito = "COSTE DESGLOSADO: ";
            string palabra_clave = "Coste de Ensamblaje";
            string sufijo2 = "coste_ensam";
            string nombre_de_ley = "ley_coste_ensam";
            string nombre_de_campo_y_combo_Box = comboBox_C_Ensam.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box, updateParams); 
        }

        private void comboBox_C_Verif_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox_C_VerifActions();          
        }

        private void comboBox_C_VerifActions(bool updateParams = true)
        {
            string Ambito = "COSTE DESGLOSADO: ";
            string palabra_clave = "Coste de Verificacion";
            string sufijo2 = "coste_verif";
            string nombre_de_ley = "ley_coste_verif";
            string nombre_de_campo_y_combo_Box = comboBox_C_Verif.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box, updateParams); 
        }

        private void comboBox_C_Serv_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox_C_ServActions();
        }

        private void comboBox_C_ServActions(bool updateParams = true)
        {
            string Ambito = "COSTE DESGLOSADO: ";
            string palabra_clave = "Coste de Puesta_en_servicio";
            string sufijo2 = "coste_serv";
            string nombre_de_ley = "ley_coste_serv";
            string nombre_de_campo_y_combo_Box = comboBox_C_Serv.Text;
            ElegirLeySolicitarDatos(Ambito, palabra_clave, sufijo2, nombre_de_ley, nombre_de_campo_y_combo_Box, updateParams);
        }
        
        /* Selección de gráfica */

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox7.Text)
            {
                case "Disponibilidad":
                    //Gráfica de Disponibilidad
                    if (Lista_Disponibilidad.Count() > 1)
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
                    break;

                case "MTBF":
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
                    break;

                case "Número de Fallos":
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
                    break;

                case "Número de Preventivos":
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
                    break;

                case "MTTR":
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
                    break;

                case "MTTR sin logística":
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
                    break;

                case "Ratio MTTR/MTTR_sinlog":
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
                    break;

                case "Intensidad de Fallos":
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
                    break;

                case "Ratio t_Correctivo/t_total_Mto":
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
                    break;

                case "Ratio t_Preventivo/t_total_Mto":
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
                    break;

                case "Coste Acumulado Rec/Rep":
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
                    break;

                case "Coste Medio de Recuperacion":
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
                    break;
            }
        }

        /* Selección de casos predefinidos */

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            Hashtable parameters = new Hashtable { };
            if (comboBox8.Text == "Caso1")
            {
                parameters = new Hashtable {
                    {"ley_func", "Exponencial"},
                    {"ley_paro", "Uniforme"},                
                    {"ley_recu", "Siempre a Nuevo (GAN)"},
                    {"ley_func_param1", 0.0},
                    {"ley_func_param2", 0.0001246},
                    {"Minimo_func", 2588.0},
                    {"Maximo_func", 400000.0},
                    {"Maximo_paro", 136.0},
                    {"Minimo_paro", 1.0}
                };

                textBox11.Enabled = true;
                textBox10.Text = "400000";
            }

            if (comboBox8.Text == "Caso2")
            {
                parameters = new Hashtable {   
                    {"ley_func", "Exponencial"},
                    {"ley_paro", "Weibull2P"},
                    {"ley_recu", "Siempre a Nuevo (GAN)"},
                    {"ley_func_param1", 0.0},
                    {"ley_func_param2", 0.0001246},
                    {"Minimo_func", 2588.0},
                    {"Maximo_func", 400000.0},
                    {"ley_paro_param1", 0.588},
                    {"ley_paro_param2", 15.073},
                    {"Minimo_paro", 1.0},
                    {"Maximo_paro", 136.0}
                };

                textBox11.Enabled = true;
                textBox10.Text = "400000";

            }

            if (comboBox8.Text == "Caso3")
            {
                parameters = new Hashtable {
                    {"ley_func", "Exponencial"},
                    {"ley_paro", "Weibull2P"},
                    {"ley_recu", "Siempre a Nuevo (GAN)"},
                    {"Maximo_func", 400000.0},
                    {"Minimo_func", 2588.0},
                    {"ley_func_param2", 0.0001526},
                    {"ley_func_param1", 0.0},
                    {"ley_paro_param1", 0.588},
                    {"ley_paro_param2", 15.073},
                    {"Minimo_paro", 1.0},
                    {"Maximo_paro", 136.0}
                };

                textBox11.Enabled = true;
                textBox10.Text = "400000";
            }

            if (comboBox8.Text == "Caso4")
            {
                parameters = new Hashtable {
                    {"ley_func", "Exponencial"},
                    {"ley_paro", "Weibull2P"},
                    {"ley_recu", "Según tiempo (BAO)"},    
                    {"ley_func_param1", 0.0},
                    {"ley_func_param2", 0.0001526},
                    {"Minimo_func", 2588.0},
                    {"Maximo_func", 400000.0},
                    {"ley_paro_param1", 0.588},
                    {"ley_paro_param2", 15.073},
                    {"Minimo_paro", 1.0},
                    {"Maximo_paro", 136.0},
                };

                textBox11.Enabled = true;
                textBox10.Text = "20000";
            }

            if (comboBox8.Text == "Caso5")
            {
                parameters = new Hashtable {
                    {"ley_func", "Exponencial"},
                    {"ley_paro", "Weibull2P"},
                    {"ley_recu", "Línea recta"},
                    {"ley_func_param1", 0.0},
                    {"ley_func_param2", 0.0001526},
                    {"Minimo_func", 2588.0},
                    {"Maximo_func", 400000.0},
                    {"ley_paro_param1", 0.588},
                    {"ley_paro_param2", 15.073},
                    {"Minimo_paro", 1.0},
                    {"Maximo_paro", 136.0},
                    {"Y2_recu", 0.6},
                    {"X2_recu", 87600.0},
                    {"Y1_recu", 1.0},
                    {"X1_recu", 0.0},
                };

                textBox11.Enabled = true;
                textBox10.Text = "87600";
            }

            if (comboBox8.Text == "Caso6")
            {
                parameters = new Hashtable {
                    {"ley_func", "Exponencial"},
                    {"ley_paro", "Weibull2P"},
                    {"ley_recu", "Línea recta"},
                    {"ley_eficiencia_mto", "Uniforme"},
                    {"ley_func_param1", 0.0},
                    {"ley_func_param2", 0.0001526},
                    {"Minimo_func", 2588.0},
                    {"Maximo_func", 400000.0},
                    {"ley_paro_param1", 0.588},
                    {"ley_paro_param2", 15.073},
                    {"Minimo_paro", 1.0},
                    {"Maximo_paro", 136.0},
                    {"Y2_recu", 0.6},
                    {"X2_recu", 87600.0},
                    {"Y1_recu", 1.0},
                    {"X1_recu", 0.0},
                    {"Minimo_eficiencia_mto", 90.0},
                    {"Maximo_eficiencia_mto", 100.0},
                };

                textBox11.Enabled = true;
                textBox10.Text = "87600";
            }

            if (comboBox8.Text == "Caso7")
            {
                parameters = new Hashtable {
                    {"ley_func", "Exponencial"},
                    {"ley_paro", "Weibull2P"},
                    {"ley_recu", "Línea recta"},
                    {"ley_eficiencia_mto", "Uniforme"},
                    {"preventivo", "Activado"},
                    {"tipo_de_preventivo", "Fijo por tiempo"},
                    {"ley_func_param1", 0.0},
                    {"ley_func_param2", 0.0001526},
                    {"Minimo_func", 2588.0},
                    {"Maximo_func", 400000.0},
                    {"ley_paro_param1", 0.588},
                    {"ley_paro_param2", 15.073},
                    {"Minimo_paro", 1.0},
                    {"Maximo_paro", 136.0},
                    {"Y2_recu", 0.6},
                    {"X2_recu", 87600.0},
                    {"Y1_recu", 1.0},
                    {"X1_recu", 0.0},
                    {"Minimo_eficiencia_mto", 90.0},
                    {"Maximo_eficiencia_mto", 100.0},
                    {"tiempo_entre_preventivos", 8760.0},
                };

                textBox11.Enabled = true;
                textBox10.Text = "87600";

            }

            if (comboBox8.Text == "Caso8")
            {
                parameters = new Hashtable {
                    {"ley_func", "Exponencial"},
                    {"ley_paro", "Weibull2P"},
                    {"ley_recu", "Línea recta"},
                    {"ley_eficiencia_mto", "Uniforme"},
                    {"preventivo", "Activado"},
                    {"tipo_de_preventivo", "Fijo por tiempo"},
                    {"ley_func_param1", 0.0},
                    {"ley_func_param2", 0.0001526},
                    {"Minimo_func", 2588.0},
                    {"Maximo_func", 400000.0},
                    {"Maximo_paro", 136.0},
                    {"Minimo_paro", 1.0},
                    {"ley_paro_param2", 15.073},
                    {"ley_paro_param1", 0.588},
                    {"Y2_recu", 0.6},
                    {"X2_recu", 87600.0},
                    {"Y1_recu", 1.0},
                    {"X1_recu", 0.0},
                    {"Minimo_eficiencia_mto", 90.0},
                    {"Maximo_eficiencia_mto", 100.0},
                    {"tiempo_entre_preventivos", 8760.0},
                    {"paro_Reduccion_si_Preventivo", 50.0},
                };

                textBox11.Enabled = true;
                textBox10.Text = "87600";
            }


            if (comboBox8.Text == "Caso9")
            {
                parameters = new Hashtable {
                    {"ley_func", "Exponencial"},
                    {"ley_paro", "Weibull2P"},
                    {"ley_recu", "Línea recta"},
                    {"ley_eficiencia_mto", "Uniforme"},
                    {"preventivo", "Activado"},
                    {"tipo_de_preventivo", "Por Disponibilidad"},
                    {"ley_func_param1", 0.0},
                    {"ley_func_param2", 0.0001526},
                    {"Minimo_func", 2588.0},
                    {"Maximo_func", 400000.0},
                    {"Maximo_paro", 136.0},
                    {"Minimo_paro", 1.0},
                    {"ley_paro_param2", 15.073},
                    {"ley_paro_param1", 0.588},
                    {"Y2_recu", 0.6},
                    {"X2_recu", 87600.0},
                    {"Y1_recu", 1.0},
                    {"X1_recu", 0.0},
                    {"Minimo_eficiencia_mto", 90.0},
                    {"Maximo_eficiencia_mto", 100.0},
                    {"disponibilidad_minima_admisible", 30.0},
                    {"paro_Reduccion_si_Preventivo", 50.0},
                };

                textBox11.Enabled = true;
                textBox10.Text = "87600";
            }

            if (comboBox8.Text == "Caso10")
            {
                parameters = new Hashtable {
                    {"ley_func", "Exponencial"},
                    {"ley_paro", "Desglose de Fallos"},
                    {"ley_recu", "Línea recta"},
                    {"ley_eficiencia_mto", "Uniforme"},
                    {"preventivo", "Activado"},
                    {"tipo_de_preventivo", "Por Disponibilidad"},
                    {"ley_paro_prep", "Uniforme"},
                    {"ley_paro_repa", "Uniforme"},
                    {"ley_coste", "Desglose de Costes"},
                    {"ley_coste_prep", "Fijo"},
                    {"ley_coste_repa", "Fijo"},
                    {"ley_func_param1", 0.0},
                    {"ley_func_param2", 0.0001526},
                    {"Minimo_func", 2588.0},
                    {"Maximo_func", 400000.0},
                    {"Minimo_paro_prep", 5.0},
                    {"Maximo_paro_prep", 10.0},
                    {"paro_prep_Reduccion_si_Preventivo", 50.0},
                    {"Maximo_paro_repa", 25.0},
                    {"Y2_recu", 0.6},
                    {"X2_recu", 87600.0},
                    {"Y1_recu", 1.0},
                    {"X1_recu", 0.0},
                    {"Minimo_eficiencia_mto", 90.0},
                    {"Maximo_eficiencia_mto", 100.0},
                    {"disponibilidad_minima_admisible", 30.0},
                    {"paro_Reduccion_si_Preventivo", 50.0},
                    {"Minimo_paro_repa", 15.0},
                    {"paro_repa_Reduccion_si_Preventivo", 30.0},
                    {"ley_coste_prep_param1", 16.0},
                    {"coste_prep_Perdida_Prod_por_Ud_tiempo", 5.0},
                    {"coste_prep_Reduccion_si_Preventivo", 50.0},
                    {"ley_coste_repa_param1", 16.0},
                    {"coste_repa_Perdida_Prod_por_Ud_tiempo", 5.0},
                    {"coste_repa_Reduccion_si_Preventivo", 30.0},
                };

                textBox11.Enabled = true;
                textBox10.Text = "87600";
            }


            if (comboBox8.Text == "Caso11")
            {
                textBox11.Enabled = true;
                textBox10.Text = "20000";

                parameters = new Hashtable {
                    {"ley_func", "Exponencial"},
                    {"ley_paro", "Normal"},
                    {"ley_recu", "Siempre a Nuevo (GAN)"},
                    {"ley_coste", "Ninguna Ley"},
                    {"preventivo", "No activado"},
                    {"ley_eficiencia_mto", "Ninguna Ley"},
                    {"ley_func_param1", 0.0},
                    {"ley_func_param2", 0.001},
                    {"Minimo_func", 100.0},
                    {"Maximo_func", 10000.0},
                    {"ley_paro_param1", 200.0},
                    {"ley_paro_param2", 20.0},
                    {"Minimo_paro", 100.0},
                    {"Maximo_paro", 10000.0},
                    {"paro_Reduccion_si_Preventivo", 20.0},
                };

                /*foreach (string key in nombres.Keys)
                {
                    textBox11.Text += "\r\n" + key + " = " + nombres[key].ToString();
                }

                textBox11.Text += "\r\n";

                foreach (string key in parametros.Keys)
                {
                    textBox11.Text += "\r\n" + key + " = " + parametros[key].ToString();
                }*/

                foreach (string key in _parameters.Keys)
                {
                    textBox11.Text += "\r\n" + key + " = " + _parameters[key].ToString();
                }

            }

            if (comboBox8.Text == "Caso12")
            {
                textBox11.Enabled = true;
                textBox10.Text = "50000";

                parameters = new Hashtable {
                    {"ley_func", "Exponencial"},
                    {"ley_paro", "Desglose de Fallos"},
                    {"ley_recu", "Siempre a Nuevo (GAN)"},
                    {"ley_coste", "Ninguna Ley"},
                    {"preventivo", "No activado"},
                    {"ley_eficiencia_mto", "Ninguna Ley"},
                    {"ley_paro_recon", "Fijo"},
                    {"ley_paro_diag", "Uniforme"},
                    {"ley_paro_prep", "Exponencial"},
                    {"ley_paro_desm", "Weibull2P"},
                    {"ley_paro_repa", "Normal"},
                    {"ley_paro_ensam", "Línea recta"},
                    {"ley_paro_verif", "Fijo"},
                    {"ley_paro_serv", "Uniforme"},
                    {"ley_func_param1", 0.0},
                    {"ley_func_param2", 0.001},
                    {"Minimo_func", 100.0},
                    {"Maximo_func", 10000.0},
                    {"Maximo_paro_diag", 40.0},
                    {"Minimo_paro_diag", 20.0},
                    {"paro_recon_Reduccion_si_Preventivo", 50.0},
                    {"ley_paro_recon_param1", 30.0},
                    {"paro_Reduccion_si_Preventivo", 20.0},
                    {"paro_diag_Reduccion_si_Preventivo", 50.0},
                    {"ley_paro_prep_param1", 10.0},
                    {"ley_paro_prep_param2", 0.01},
                    {"Minimo_paro_prep", 10.0},
                    {"Maximo_paro_prep", 1000.0},
                    {"paro_prep_Reduccion_si_Preventivo", 50.0},
                    {"ley_paro_desm_param1", 1.5},
                    {"ley_paro_desm_param2", 400.0},
                    {"Minimo_paro_desm", 50.0},
                    {"Maximo_paro_desm", 700.0},
                    {"paro_desm_Reduccion_si_Preventivo", 50.0},
                    {"ley_paro_repa_param1", 30.0},
                    {"ley_paro_repa_param2", 3.0},
                    {"Minimo_paro_repa", 15.0},
                    {"Maximo_paro_repa", 45.0},
                    {"paro_repa_Reduccion_si_Preventivo", 50.0},
                    {"X1_paro_ensam", 0.0},
                    {"Y1_paro_ensam", 20.0},
                    {"X2_paro_ensam", 60000.0},
                    {"Y2_paro_ensam", 50.0},
                    {"paro_ensam_Reduccion_si_Preventivo", 50.0},
                    {"ley_paro_verif_param1", 30.0},
                    {"paro_verif_Reduccion_si_Preventivo", 50.0},
                    {"Minimo_paro_serv", 20.0},
                    {"Maximo_paro_serv", 40.0},
                    {"paro_serv_Reduccion_si_Preventivo", 50.0},
                };

                /*foreach (string key in nombres.Keys)
                {
                    textBox11.Text += "\r\n" + key + " = " + nombres[key].ToString();
                }

                textBox11.Text += "\r\n";

                foreach (string key in parametros.Keys)
                {
                    textBox11.Text += "\r\n" + key + " = " + parametros[key].ToString();
                }*/
                foreach (string key in _parameters.Keys)
                {
                    textBox11.Text += "\r\n" + key + " = " + _parameters[key].ToString();
                }
            }
            StartSilentMode();
            DefaultParameters();
            AddParameters(parameters);
            StopSilentMode();
        }


        /* Acciones sobre widgets (mostrar/ocultar) */

        private void DeshabilitarComboBoxDesgloseTiempoFallo()
        {
            groupBoxDesgloceTiempos.Enabled = false;

            List<String> widgets = new List<String> {
                "ley_paro_recon",
                "ley_paro_diag",
                "ley_paro_prep",
                "ley_paro_desm",
                "ley_paro_repa",
                "ley_paro_ensam",
                "ley_paro_verif",
                "ley_paro_serv"
            };

            foreach (String wname in widgets)
            {
                SetWidgetValue(_widgets[wname], (string)_defaultValues[wname]);
            }
        }

        private void HabilitarComboBoxDesgloseTiempoFallo()
        {
            groupBoxDesgloceTiempos.Enabled = true;
        }

        private void DeshabilitarComboBoxDesgloseCoste()
        {
            groupBoxDesgloceCostes.Enabled = false;

            List<String> widgets = new List<String> {
                "ley_coste_recon",
                "ley_coste_diag",
                "ley_coste_prep",
                "ley_coste_desm",
                "ley_coste_repa",
                "ley_coste_ensam",
                "ley_coste_verif",
                "ley_coste_serv"
            };

            foreach (String wname in widgets)
            {
                SetWidgetValue(_widgets[wname], (string)_defaultValues[wname]);
            }
        }

        private void HabilitarComboBoxDesgloseCoste()
        {
            groupBoxDesgloceCostes.Enabled = true;
        }

        private void DeshabilitarComboBoxVerGraficas()
        {
            comboBox7.Enabled = false;
        }

        private void HabilitarComboBoxVerGraficas()
        {
            comboBox7.Enabled = true;
        }


        /* Tratamiento de eventos */

        private void ElegirLeySolicitarDatos(string Ambito, string palabra_clave, string sufijo2, string nombre_de_ley, string nombre_de_campo_y_combo_Box, bool updateParams = true)
        {
            List<String> parametersToRemove;
            Hashtable parametersToAdd = new Hashtable { };
            Hashtable subform = new Hashtable { };
            Hashtable subformFields = new Hashtable { };
    
            // Parámetros que se van a solicitar en los subformularios
            parametersToRemove = new List<String> {
                sufijo2 + "_Mantenimiento_Ud_Tiempo",
                sufijo2 + "_Perdida_Prod_por_Ud_tiempo",
                sufijo2 + "_MantenimientoCadaIntervencion",
                sufijo2 + "_Reduccion_si_Preventivo",
                "X1_" + sufijo2,
                "Y1_" + sufijo2,
                "X2_" + sufijo2,
                "Y2_" + sufijo2,
                "ley_" + sufijo2 + "_param1", 
                "ley_" + sufijo2 + "_param2",
                "Minimo_" + sufijo2,
                "Maximo_" + sufijo2,
                "ley_" + sufijo2
            };

            parametersToAdd.Add(nombre_de_ley, nombre_de_campo_y_combo_Box);

            List<String> auxi = new List<String> {"","","","","" };

            switch (nombre_de_campo_y_combo_Box)
            {
                case "Ninguna Ley":
                case "Siempre a Nuevo (GAN)":        
                case "Según tiempo (BAO)":
                    break;

                case "No activado":
                    if (nombre_de_ley == "preventivo")
                    {
                        parametersToRemove.Add("tipo_de_preventivo");
                        parametersToRemove.Add("tiempo_entre_preventivo");
                        parametersToRemove.Add("disponibilidad_minima_admisible");
                    }
                    break;

                case "Por Disponibilidad":
                    parametersToRemove.Add("tiempo_entre_preventivos");
                    parametersToRemove.Add("tipo_de_preventivo");
                    parametersToAdd.Remove(nombre_de_ley);
                    parametersToAdd.Add(nombre_de_ley, "Activado");
                    parametersToAdd.Add("tipo_de_preventivo", nombre_de_campo_y_combo_Box);

                    subformFields = new Hashtable {
                        {"disponibilidad_minima_admisible", "Disponibilidad Mínima Admisible (%)"}
                    };

                    subform = new Hashtable {
                        {"title", "MANTENIMIENTO POR DISPONIBILIDAD"},
                        {"ambito", "PREVENTIVO"},
                        {"ley", ""},
                        {"fields", subformFields}
                    };
                    break;

                case "Desglose de Costes":                    
                    //Se habilitan las opciones de desglose de Coste
                    HabilitarComboBoxDesgloseCoste();
                    break;

                case "Desglose de Fallos":                    
                    //Se habilitan las opciones de desglose de Fallo
                    HabilitarComboBoxDesgloseTiempoFallo();
                    break;

                //el siguiente if es solo aplicable a captura de datos de coste
                case "Fijo por tiempo":
                    if (nombre_de_ley == "preventivo")
                    {
                        parametersToRemove.Add("disponibilidad_minima_admisible");
                        parametersToAdd.Remove(nombre_de_ley);
                        parametersToAdd.Add(nombre_de_ley, "Activado");
                        parametersToAdd.Add("tipo_de_preventivo", nombre_de_campo_y_combo_Box);

                        subformFields = new Hashtable {
                            {"Tiempo entre Preventivos (Udes. de Tiempo)", "tiempo_entre_preventivos"}
                        };

                        subform = new Hashtable {
                            {"title", "MANTENIMIENTO PREVENTIVO FIJO POR TIEMPO"},
                            {"ambito", "PREVENTIVO"},
                            {"ley", ""},
                            {"fields", subformFields}
                        };
                    }
                    else
                    {
                        subformFields = new Hashtable {
                            {"ley_" + sufijo2 + "_param1", "Coste Mto./Ud_tiempo"}, 
                            {"ley_" + sufijo2 + "_param2", "Coste_Perdida_Prod_por_Ud_tiempo"},
                            {sufijo2 + "_Reduccion_si_Preventivo", "% reducción si Preventivo"}
                        };

                        subform = new Hashtable {
                            {"title", "Ley de Coste Fijo por tiempo "},
                            {"ambito", Ambito},
                            {"ley", (string)_parameters[nombre_de_ley]},
                            {"fields", subformFields}
                        };
                    }
                    break;

                //el siguiente if es solo aplicable a captura de datos de coste
                case "Fijo por intervención":                    
                    subformFields = new Hashtable {
                        {"ley_" + sufijo2 + "_param1", "Coste cada Intervención"}, 
                        {"ley_" + sufijo2 + "_param2", "Coste_Perdida_Prod_por_Ud_tiempo"},
                        {sufijo2 + "_Reduccion_si_Preventivo", "% reducción si Preventivo"}
                    };

                    subform = new Hashtable {
                        {"title", "Ley de Coste Fijo cada Intervención "},
                        {"ambito", Ambito},
                        {"ley", (string)_parameters[nombre_de_ley]},
                        {"fields", subformFields}
                    };
                    break;

                case "Fijo":
                    if (Ambito == "COSTE DESGLOSADO: " || Ambito == "COSTE SIN DESGLOSE: ")
                    {
                        auxi[0] = "Coste Perdida_Prod / Ud_tiempo";
                        auxi[1] = "_Perdida_Prod_por_Ud_tiempo";
                        subformFields.Add(sufijo2 + auxi[1], auxi[0]);
                    }

                    if (Ambito == "COSTE DESGLOSADO: " || Ambito == "COSTE SIN DESGLOSE: " || Ambito == "FALLO/PARADA: " || Ambito == "DESGLOSE DE FALLO/PARADA: ")
                    {
                        auxi[2] = "% reducción si Preventivo";
                        auxi[3] = sufijo2 + "_Reduccion_si_Preventivo";
                        subformFields.Add(auxi[3], auxi[2]);
                    }

                    auxi[4] = Ambito == "EFICIENCIA DEL MANTENIMIENTO: " ? " " : "Tiempo de ";
                    subformFields.Add("ley_" + sufijo2 + "_param1", auxi[4] + palabra_clave);
                    
                    subform = new Hashtable {
                        {"title", "Ley Fija de " + palabra_clave},
                        {"ambito", Ambito},
                        {"ley", (string)_parameters[nombre_de_ley]},
                        {"fields", subformFields}
                    };
                    break;

                case "Uniforme":
                    subformFields = new Hashtable {
                        {"Minimo_" + sufijo2, "Mínimo Admisible"},
                        {"Maximo_" + sufijo2, "Máximo Admisible"}
                    };

                    if (Ambito == "COSTE DESGLOSADO: " || Ambito == "COSTE SIN DESGLOSE: ")
                    {
                        auxi[0] = "Coste Perdida_Prod / Ud_tiempo";
                        auxi[1] = "_Perdida_Prod_por_Ud_tiempo";
                        subformFields.Add(sufijo2 + auxi[1], auxi[0]);
                    }

                    if (Ambito == "COSTE DESGLOSADO: " || Ambito == "COSTE SIN DESGLOSE: " || Ambito == "FALLO/PARADA: " || Ambito == "DESGLOSE DE FALLO/PARADA: ")
                    {
                        auxi[2] = "% reducción si Preventivo";
                        auxi[3] = sufijo2 + "_Reduccion_si_Preventivo";
                        subformFields.Add(auxi[3], auxi[2]);
                    }

                    subform = new Hashtable {
                        {"title", "Ley Uniforme de " + palabra_clave},
                        {"ambito", Ambito},
                        {"ley", (string)_parameters[nombre_de_ley]},
                        {"fields", subformFields}
                    };
                    break;

                case "Línea recta":
                    subformFields = new Hashtable {
                        {"X1_" + sufijo2, "Inicial T"}, 
                        {"Y1_" + sufijo2, "Inicial t " + palabra_clave}, 
                        {"X2_" + sufijo2, "Final T"},
                        {"Y2_" + sufijo2, "Final t " + palabra_clave}
                    };

                    if (Ambito == "COSTE DESGLOSADO: " || Ambito == "COSTE SIN DESGLOSE: ")
                    {
                        auxi[0] = "Coste Perdida_Prod / Ud_tiempo";
                        auxi[1] = "_Perdida_Prod_por_Ud_tiempo";
                        subformFields.Add(sufijo2 + auxi[1], auxi[0]);
                    }

                    if (Ambito == "COSTE DESGLOSADO: " || Ambito == "COSTE SIN DESGLOSE: " || Ambito == "FALLO/PARADA: " || Ambito == "DESGLOSE DE FALLO/PARADA: ")
                    {
                        auxi[2] = "% reducción si Preventivo";
                        auxi[3] = sufijo2 + "_Reduccion_si_Preventivo";
                        subformFields.Add(auxi[3], auxi[2]);
                    }

                    subform = new Hashtable {
                        {"title", "Ley Lineal de " + palabra_clave},
                        {"ambito", Ambito},
                        {"ley", (string)_parameters[nombre_de_ley]},
                        {"fields", subformFields}
                    };
                    break;

                case "Exponencial":
                    subformFields = new Hashtable {
                        {"ley_" + sufijo2 + "_param1", "Gamma"},
                        { "ley_" + sufijo2 + "_param2", "Lambda"}, 
                        {"Minimo_" + sufijo2, "Mínimo Admisible"},
                        {"Maximo_" + sufijo2, "Máximo Admisible"}
                    };

                    if (Ambito == "COSTE DESGLOSADO: " || Ambito == "COSTE SIN DESGLOSE: ")
                    {
                        auxi[0] = "Coste Perdida_Prod / Ud_tiempo";
                        auxi[1] = "_Perdida_Prod_por_Ud_tiempo";
                        subformFields.Add(sufijo2 + auxi[1], auxi[0]);
                    }

                    if (Ambito == "COSTE DESGLOSADO: " || Ambito == "COSTE SIN DESGLOSE: " || Ambito == "FALLO/PARADA: " || Ambito == "DESGLOSE DE FALLO/PARADA: ")
                    {
                        auxi[2] = "% reducción si Preventivo";
                        auxi[3] = sufijo2 + "_Reduccion_si_Preventivo";
                        subformFields.Add(auxi[3], auxi[2]);
                    }

                    subform = new Hashtable {
                        {"title", "Ley Exponencial de " + palabra_clave},
                        {"ambito", Ambito},
                        {"ley", (string)_parameters[nombre_de_ley]},
                        {"fields", subformFields}
                    };
                    break;

                case "Weibull2P":
                    subformFields = new Hashtable {
                        {"ley_" + sufijo2 + "_param1", "Beta"}, 
                        {"ley_" + sufijo2 + "_param2", "Eta"}, 
                        {"Minimo_" + sufijo2, "Mínimo Admisible"},
                        {"Maximo_" + sufijo2, "Máximo Admisible"}
                    };
                    
                    if (Ambito == "COSTE DESGLOSADO: " || Ambito == "COSTE SIN DESGLOSE: ")
                    {
                        auxi[0] = "Coste Perdida_Prod / Ud_tiempo";
                        auxi[1] = "_Perdida_Prod_por_Ud_tiempo";
                        subformFields.Add(sufijo2 + auxi[1], auxi[0]);
                    }

                    if (Ambito == "COSTE DESGLOSADO: " || Ambito == "COSTE SIN DESGLOSE: " || Ambito == "FALLO/PARADA: " || Ambito == "DESGLOSE DE FALLO/PARADA: ")
                    {
                        auxi[2] = "% reducción si Preventivo";
                        auxi[3] = sufijo2 + "_Reduccion_si_Preventivo";
                        subformFields.Add(auxi[3], auxi[2]);
                    }

                    subform = new Hashtable {
                        {"title", "Ley Weibull2P de " + palabra_clave},
                        {"ambito", Ambito},
                        {"ley", (string)_parameters[nombre_de_ley]},
                        {"fields", subformFields}
                    };
                    break;

                case "Normal":
                    subformFields = new Hashtable {
                        {"ley_" + sufijo2 + "_param1", "Valor Medio"}, 
                        {"ley_" + sufijo2 + "_param2", "Desviación Típica"}, 
                        {"Minimo_" + sufijo2, "Mínimo Admisible"},
                        {"Maximo_" + sufijo2, "Máximo Admisible"}
                    };
                    
                    if (Ambito == "COSTE DESGLOSADO: " || Ambito == "COSTE SIN DESGLOSE: ")
                    {
                        auxi[0] = "Coste Perdida_Prod / Ud_tiempo";
                        auxi[1] = "_Perdida_Prod_por_Ud_tiempo";
                        subformFields.Add(sufijo2 + auxi[1], auxi[0]);
                    }

                    if (Ambito == "COSTE DESGLOSADO: " || Ambito == "COSTE SIN DESGLOSE: " || Ambito == "FALLO/PARADA: " || Ambito == "DESGLOSE DE FALLO/PARADA: ")
                    {
                        auxi[2] = "% reducción si Preventivo";
                        auxi[3] = sufijo2 + "_Reduccion_si_Preventivo";
                        subformFields.Add(auxi[3], auxi[2]);
                    }

                    subform = new Hashtable {
                        {"title", "Ley Normal de " + palabra_clave},
                        {"ambito", Ambito},
                        {"ley", (string)_parameters[nombre_de_ley]},
                        {"fields", subformFields}
                    };
                    break; 
            }

            if (!SilentMode)
            {
                if (updateParams)
                {
                    RemoveParameters(parametersToRemove);
                    AddParameters(parametersToAdd);
                }
                if (subform.Keys.Count != 0)
                    Subform((string)subform["title"], 
                            (string)subform["ambito"], 
                            (string)subform["ley"], 
                            (Hashtable)subform["fields"]);
            }
            
            // Mostrar botón cuando se solicita subform
            Button objButton = (Button)_subformButtons[nombre_de_ley];            
            objButton.Enabled = subform.Keys.Count != 0 ? true : false;            
        }

        private void Subform(string title, string ambito, string ley, Hashtable fields) 
        {
            FormDatos1 form = new FormDatos1();
            form.title = title;
            form.Text = ambito;
            form.fields = fields;
            form.extParameters = _parameters;
            
            form.ShowDialog();
            if (form.DialogResult == DialogResult.OK)
            {
                AddParameters(form.parameters);
            }
        }
        

        /* Otros */

        // Generar valor
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
        private double GenerarValor(double variable, string tipo_ley, string param1, string param2, string maximo, string minimo, string X1, string Y1, string X2, string Y2, double tiempo, Random r)
        {
            double valor_generado = 0;
            string auxi1;
            if (_parameters.ContainsKey(tipo_ley))
            {
                auxi1 = (string)_parameters[tipo_ley];
                if (auxi1 == "Ninguna Ley") valor_generado = 0;
                if (auxi1 == "Fijo por tiempo") valor_generado = (double)_parameters[param1] * tiempo;
                if (auxi1 == "Fijo por intervención") valor_generado = (double)_parameters[param1];
                if (auxi1 == "Fijo") valor_generado = (double)_parameters[param1];
                if (auxi1 == "Uniforme") valor_generado = GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme((double)_parameters[minimo], (double)_parameters[maximo], r) * tiempo;
                if (auxi1 == "Exponencial") valor_generado = GeneradoresDeAleatorios.Generador_Aleatorio_Exponencial((double)_parameters[param1], 1 / (double)_parameters[param2], (double)_parameters[minimo], (double)_parameters[maximo], r) * tiempo;
                if (auxi1 == "Weibull2P") valor_generado = GeneradoresDeAleatorios.Generador_Aleatorio_Weibull_2P((double)_parameters[param1], (double)_parameters[param2], (double)_parameters[minimo], (double)_parameters[maximo], r) * tiempo;
                if (auxi1 == "Normal") valor_generado = GeneradoresDeAleatorios.Generador_Aleatorio_Normal((double)_parameters[param1], (double)_parameters[param2], (double)_parameters[minimo], (double)_parameters[maximo], r) * tiempo;
                if (auxi1 == "Línea recta") valor_generado = (double)_parameters[Y1] + ((double)_parameters[Y2] - (double)_parameters[Y1]) * (variable - (double)_parameters[X1]) / ((double)_parameters[X2] - (double)_parameters[X1]) * tiempo;
            }

            return valor_generado;
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


        private void Simular()
        {
            // Se resetea selector de gráfica
            ResetComboBox7();
            // Se deshabilitan botones hasta que termine la simulación
            button4.Enabled = false;
            button_Reset.Enabled = false;

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
            double t_paro_recon = 0;
            double t_paro_diag = 0;
            double t_paro_prep = 0;
            double t_paro_desm = 0;
            double t_paro_repa = 0;
            double t_paro_ensam = 0;
            double t_paro_verif = 0;
            double t_paro_serv = 0;
            string clave;
            double coste_recon = 0;
            double coste_diag = 0;
            double coste_prep = 0;
            double coste_desm = 0;
            double coste_repa = 0;
            double coste_ensam = 0;
            double coste_verif = 0;
            double coste_serv = 0;
            double coste_prod_recon = 0;
            double coste_prod_diag = 0;
            double coste_prod_prep = 0;
            double coste_prod_desm = 0;
            double coste_prod_repa = 0;
            double coste_prod_ensam = 0;
            double coste_prod_verif = 0;
            double coste_prod_serv = 0;

            double Ratio_Correctivo_vs_Total = 0;
            double Ratio_Preventivo_vs_Total = 0;
            double TiempoHastaSiguientePreventivo = 0;
            double EficienciaMto = 100;

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

            rnd1 = GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme(0.0001, 0.001, r);
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
            if (auxi9 > 0) Tiempo_A_Simular = auxi9;

            //Fijar el tamaño máximo de la barra de seguimiento y ponerla a cero
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;

            //Borrar el contenido del textbox de salida de resultados y encabezarlo con los datos de partida
            textBox5.Enabled = true;
            textBox5.Text = "";
            textBox5.Text += "\r\n";
            textBox5.Text += "\r\n" + "------------------- DATOS DE PARTIDA -------------------------";
            textBox5.Text += "\r\n";

            foreach (string key in _parameters.Keys)
            {
                textBox5.Text += "\r\n" + key + " = " + _parameters[key].ToString();
            }
            textBox5.Text += "\r\n" + "Tiempo a Simular = " + Tiempo_A_Simular.ToString();
            textBox5.Text += "\r\n";
            textBox5.Text += "\r\n" + "-------------- FIN DE LOS DATOS DE PARTIDA -------------------";
            textBox5.Text += "\r\n";
            textBox5.Enabled = false;

            //TODO Hacer que se deshabiliten todos los inputs.
            //Deshabilitar el combobox de ver gráficas hasta que finalicen los cálculos
            DeshabilitarComboBoxVerGraficas();
            progressBar1.Visible = true;
            comboBox7.Visible = false;

            //Poner aqui el umbral de disponibilidad si esta activado el preventivo por disponibilidad
            if (_parameters.ContainsKey("preventivo") && _parameters.ContainsKey("tipo_de_preventivo"))
            {
                if ((string)_parameters["preventivo"] == "Activado")
                {
                    if ((string)_parameters["tipo_de_preventivo"] == "Por Disponibilidad") DisponibilidadMinimaAdmisible = (double)_parameters["disponibilidad_minima_admisible"] / 100;
                    if ((string)_parameters["tipo_de_preventivo"] == "Fijo por tiempo") TiempoHastaSiguientePreventivo = (double)_parameters["tiempo_entre_preventivos"];
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
                switch ((string)_parameters["ley_func"])
                {
                    case "Uniforme":
                        t = GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme((double)_parameters["Minimo_func"], (double)_parameters["Maximo_func"], r);
                        break;
                    case "Exponencial":
                        t = GeneradoresDeAleatorios.Generador_Aleatorio_Exponencial((double)_parameters["ley_func_param1"], 1 / (double)_parameters["ley_func_param2"], (double)_parameters["Minimo_func"], (double)_parameters["Maximo_func"], r);
                        break;
                    case "Weibull2P":
                        t = GeneradoresDeAleatorios.Generador_Aleatorio_Weibull_2P((double)_parameters["ley_func_param1"], (double)_parameters["ley_func_param2"], (double)_parameters["Minimo_func"], (double)_parameters["Maximo_func"], r);
                        break;
                    case "Normal":
                        t = GeneradoresDeAleatorios.Generador_Aleatorio_Normal((double)_parameters["ley_func_param1"], (double)_parameters["ley_func_param2"], (double)_parameters["Minimo_func"], (double)_parameters["Maximo_func"], r);
                        break;
                    //TODO Ojo, que pasa si se elige la poción Ninguna Ley.
                }

                //En caso de estar activado el Mto Preventivo fijo por tiempo controlar si el tiempo de funcionamiento de este ciclo excede al tiempo restante para el siguiente Mto preventivo
                if (_parameters.ContainsKey("preventivo") && _parameters.ContainsKey("tipo_de_preventivo"))
                {
                    if ((string)_parameters["preventivo"] == "Activado" && (string)_parameters["tipo_de_preventivo"] == "Fijo por tiempo" && TiempoHastaSiguientePreventivo < t)
                    {
                        t = TiempoHastaSiguientePreventivo;
                        tipo_mto_este_ciclo = "Preventivo";
                        TiempoHastaSiguientePreventivo = (double)_parameters["tiempo_entre_preventivos"];
                    }
                }

                //bajada de disponibilidad durante el periodo de funcionamiento, solo Exponencial y Weubull tienen sentido
                //Para las otras leyes se opta por no bajar la disponibilidad
                switch ((string)_parameters["ley_func"])
                {
                    case "Exponencial":
                        PuntoFinalBajadaDisponibilidad = Math.Exp(-(double)_parameters["ley_func_param2"] * t) - 1 + maximoY;
                        break;
                    case "Weibull2P":
                        PuntoFinalBajadaDisponibilidad = Math.Exp(-Math.Pow(t / (double)_parameters["ley_func_param2"], (double)_parameters["ley_func_param1"])) - 1 + maximoY;
                        break;
                    case "Uniforme":
                    case "Normal":
                        PuntoFinalBajadaDisponibilidad = maximoY;
                        break;
                }

                //Controlar la validez de t ==> encontrar el t que hace cero la disponibilidad (si procede)
                ContadorTiempoDisponibilidadPositiva = 0;
                if (PuntoFinalBajadaDisponibilidad <= DisponibilidadMinimaAdmisible)
                {
                    double DisponibilidadInstantanea = -1000;
                    for (int j = 1; j <= t; j++)
                    {
                        if ((string)_parameters["ley_func"] == "Exponencial") DisponibilidadInstantanea = Math.Exp(-(double)_parameters["ley_func_param2"] * j) - 1 + maximoY;
                        else if ((string)_parameters["ley_func"] == "Weibull2P") DisponibilidadInstantanea = Math.Exp(-Math.Pow(j / (double)_parameters["ley_func_param2"], (double)_parameters["ley_func_param1"])) - 1 + maximoY;
                        if (DisponibilidadInstantanea <= DisponibilidadMinimaAdmisible) break;
                        ContadorTiempoDisponibilidadPositiva += 1;
                    }
                    PuntoFinalBajadaDisponibilidad = DisponibilidadMinimaAdmisible;
                    t = ContadorTiempoDisponibilidadPositiva;
                    if (_parameters.ContainsKey("preventivo") && _parameters.ContainsKey("tipo_de_preventivo"))
                    {
                        if ((string)_parameters["preventivo"] == "Activado" && (string)_parameters["tipo_de_preventivo"] == "Por Disponibilidad") tipo_mto_este_ciclo = "Preventivo";
                    }
                }

                //Acumular tiempos funcionando
                TiempoFuncionandoAcumulado += t;
                TiempoParcialFuncionando = t;

                //Acumular el tiempo de funcionamiento al tiempo total simulado o "tiempo transcurrido"
                TiempoTranscurrido += t;

                //Si el mantenimiento en este ciclo es correctivo pero el preventivo Fijo por Tiempo está activado, decrementar el tiempo restante hasta el siguiente preventivo
                if (_parameters.ContainsKey("preventivo") && _parameters.ContainsKey("tipo_de_preventivo"))
                {
                    if (tipo_mto_este_ciclo == "Correctivo" && (string)_parameters["preventivo"] == "Activado" && (string)_parameters["tipo_de_preventivo"] == "Fijo por tiempo") TiempoHastaSiguientePreventivo -= t;
                }


                //Dibujar puntos intermedios para mejorar el aspecto de la gráfica
                if (PuntoFinalBajadaDisponibilidad >= 0)
                {
                    double TiempoIntermedio = 0;
                    double maxY_new = 0;
                    if ((string)_parameters["ley_func"] == "Exponencial" || (string)_parameters["ley_func"] == "Weibull2P")
                    {
                        for (int j = 1; j <= 9; j++)
                        {
                            //TiempoIntermedio = (TiempoTranscurrido - t) + j * (t / 10);
                            TiempoIntermedio = j * (t / 10);

                            if ((string)_parameters["ley_func"] == "Exponencial") maxY_new = Math.Exp(-(double)_parameters["ley_func_param2"] * TiempoIntermedio) - 1 + maximoY;
                            else if ((string)_parameters["ley_func"] == "Weibull2P") maxY_new = Math.Exp(-Math.Pow(TiempoIntermedio / (double)_parameters["ley_func_param2"], (double)_parameters["ley_func_param1"])) - 1 + maximoY;
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
                switch ((string)_parameters["ley_paro"])
                {
                    //B.1.-Generar tiempo de fallo/paro en el caso en que no exista desglose de tiempos de fallos/paradas

                    case "Uniforme":
                        t = GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme((double)_parameters["Minimo_paro"], (double)_parameters["Maximo_paro"], r);
                        break;
                    case "Exponencial":
                        t = GeneradoresDeAleatorios.Generador_Aleatorio_Exponencial((double)_parameters["ley_paro_param1"], 1 / (double)_parameters["ley_paro_param2"], (double)_parameters["Minimo_paro"], (double)_parameters["Maximo_paro"], r);
                        break;
                    case "Weibull2P":
                        t = GeneradoresDeAleatorios.Generador_Aleatorio_Weibull_2P((double)_parameters["ley_paro_param1"], (double)_parameters["ley_paro_param2"], (double)_parameters["Minimo_paro"], (double)_parameters["Maximo_paro"], r);
                        break;
                    case "Normal":
                        t = GeneradoresDeAleatorios.Generador_Aleatorio_Normal((double)_parameters["ley_paro_param1"], (double)_parameters["ley_paro_param2"], (double)_parameters["Minimo_paro"], (double)_parameters["Maximo_paro"], r);
                        break;

                    //B.2.-Generar tiempo de fallo/paro en el caso en que exista desglose de tiempos
                    case "Desglose de Fallos":
                        //Reconocimiento 
                        clave = "recon";
                        t_paro_recon = GenerarValor(TiempoTranscurrido, "ley_paro_" + clave, "ley_paro_" + clave + "_param1", "ley_paro_" + clave + "_param2", "Maximo_paro_" + clave, "Minimo_paro_" + clave, "X1_paro_" + clave, "Y1_paro_" + clave, "X2_paro_" + clave, "Y2_paro_" + clave, 1, r);

                        //Diagnostico 
                        clave = "diag";
                        t_paro_diag = GenerarValor(TiempoTranscurrido, "ley_paro_" + clave, "ley_paro_" + clave + "_param1", "ley_paro_" + clave + "_param2", "Maximo_paro_" + clave, "Minimo_paro_" + clave, "X1_paro_" + clave, "Y1_paro_" + clave, "X2_paro_" + clave, "Y2_paro_" + clave, 1, r);

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
                        break;
                }

                //B.3.-Decidir si el Mantenimiento es "Correctivo" ó "Preventivo" en función de t (de fall/paro) y datos de entrada
                //Si está activado el Preventivo por Disponibilidad no corresponde hacer nada aqui
                if (_parameters.ContainsKey("preventivo") && _parameters.ContainsKey("tipo_de_preventivo"))
                {
                    if (tipo_mto_este_ciclo == "Correctivo" && (string)_parameters["preventivo"] == "Activado" && (string)_parameters["tipo_de_preventivo"] == "Fijo por tiempo" && TiempoHastaSiguientePreventivo < t)
                    {
                        tipo_mto_este_ciclo = "Preventivo";
                        TiempoHastaSiguientePreventivo = (double)_parameters["tiempo_entre_preventivos"];
                    }
                }

                //B.4.-Corregir el "Tiempo hasta el siguiente preventivo" en caso de que el mto de este ciclo sea correctivo pero esté activado el preventivo "Fijo por Tiempo"
                if (_parameters.ContainsKey("preventivo") && _parameters.ContainsKey("tipo_de_preventivo"))
                {
                    if (tipo_mto_este_ciclo == "Correctivo" && (string)_parameters["preventivo"] == "Activado" && (string)_parameters["tipo_de_preventivo"] == "Fijo por tiempo" && TiempoHastaSiguientePreventivo > t) TiempoHastaSiguientePreventivo -= t;
                }

                //B.5.-Corregir los tiempos de fallo/parada en caso de que se este aplicando Preventivo sin desglose de tiempos
                if (_parameters.ContainsKey("ley_paro") && tipo_mto_este_ciclo == "Preventivo")
                {
                    if ((string)_parameters["ley_paro"] != "Desglose de Fallos" && _parameters.ContainsKey("paro_Reduccion_si_Preventivo")) t = (100 - (double)_parameters["paro_Reduccion_si_Preventivo"]) * t / 100;
                    //OJO FALTA POR ARREGLAR QUE ESTE CARGADO EL parametros["paro_Reduccion_si_Preventivo"] EN EL CASO DE NO DESGLOSE DEL TIEMPO DE PARO, EL FORMULARIO NO LO PREGUNTA

                    //B.6.-Corregir los tiempos de fallo/parada en caso de que se este aplicando Preventivo con desglose de tiempos
                    else if ((string)_parameters["ley_paro"] == "Desglose de Fallos")
                    {
                        if (_parameters.ContainsKey("paro_recon_Reduccion_si_Preventivo")) t_paro_recon = (100 - (double)_parameters["paro_recon_Reduccion_si_Preventivo"]) * t_paro_recon / 100;
                        if (_parameters.ContainsKey("paro_diag_Reduccion_si_Preventivo")) t_paro_diag = (100 - (double)_parameters["paro_diag_Reduccion_si_Preventivo"]) * t_paro_diag / 100;
                        if (_parameters.ContainsKey("paro_prep_Reduccion_si_Preventivo")) t_paro_prep = (100 - (double)_parameters["paro_prep_Reduccion_si_Preventivo"]) * t_paro_prep / 100;
                        if (_parameters.ContainsKey("paro_desm_Reduccion_si_Preventivo")) t_paro_desm = (100 - (double)_parameters["paro_desm_Reduccion_si_Preventivo"]) * t_paro_desm / 100;
                        if (_parameters.ContainsKey("paro_repa_Reduccion_si_Preventivo")) t_paro_repa = (100 - (double)_parameters["paro_repa_Reduccion_si_Preventivo"]) * t_paro_repa / 100;
                        if (_parameters.ContainsKey("paro_ensam_Reduccion_si_Preventivo")) t_paro_ensam = (100 - (double)_parameters["paro_ensam_Reduccion_si_Preventivo"]) * t_paro_ensam / 100;
                        if (_parameters.ContainsKey("paro_verif_Reduccion_si_Preventivo")) t_paro_verif = (100 - (double)_parameters["paro_verif_Reduccion_si_Preventivo"]) * t_paro_verif / 100;
                        if (_parameters.ContainsKey("paro_serv_Reduccion_si_Preventivo")) t_paro_serv = (100 - (double)_parameters["paro_serv_Reduccion_si_Preventivo"]) * t_paro_serv / 100;

                        //Se recalcula ahora el tiempo fallado/parado como la suma de los tiempos del desglose modificados
                        t = t_paro_recon + t_paro_diag + t_paro_prep + t_paro_desm + t_paro_repa + t_paro_ensam + t_paro_verif + t_paro_serv;
                    }
                }

                //B.7.-Acumular tiempos en las diferentes variables
                //Acumular y guardar          
                TiempoParadoAcumulado += t;
                TiempoParadoParcial = t;

                //Actualizacion del tiempo total transcurrido en la simulación
                TiempoTranscurrido += t;

                //Calcular el tiempo del ciclo (tiempo que dura el ciclo)
                TiempoDelCiclo = TiempoParadoParcial + TiempoParcialFuncionando;

                //Acumular tiempo de Mantenimiento a Correctivo o a Preventivo
                if (tipo_mto_este_ciclo == "Preventivo") TiempoPreventivoAcumulado += t;
                else if (tipo_mto_este_ciclo == "Correctivo") TiempoCorrectivoAcumulado += t;



                //C)ESTABLECER EL VALOR DE LA RECUPERACIÓN EN CASO DE QUE PROCEDA, TRATARLO Y USARLO
                //Establecer el valor de la máxima recuperacion posible dado por la Ley de Recuperación
                if (_parameters.ContainsKey("ley_recu"))
                {
                    switch ((string)_parameters["ley_recu"])
                    {
                        case "Siempre a Nuevo (GAN)":
                        case "Ninguna Ley":
                            maximoY = 1;
                            break;
                        case "Según tiempo (BAO)":
                            if (_parameters.ContainsKey("ley_func"))
                            {
                                if ((string)_parameters["ley_func"] == "Exponencial") maximoY = Math.Exp(-(double)_parameters["ley_func_param2"] * (TiempoTranscurrido - (double)_parameters["ley_func_param1"]));
                                else if ((string)_parameters["ley_func"] == "Weibull2P") maximoY = Math.Exp(-Math.Pow(TiempoTranscurrido / (double)_parameters["ley_func_param2"], (double)_parameters["ley_func_param1"]));
                            }
                            break;
                        case "Exponencial":
                            maximoY = Math.Exp(-(double)_parameters["ley_recu_param2"] * (TiempoTranscurrido - (double)_parameters["ley_recu_param1"]));
                            break;
                        case "Weibull2P":
                            maximoY = Math.Exp(-Math.Pow(TiempoTranscurrido / (double)_parameters["ley_recu_param2"], (double)_parameters["ley_recu_param1"]));
                            break;
                        case "Línea recta":
                            maximoY = (double)_parameters["Y1_recu"] + ((double)_parameters["Y2_recu"] - (double)_parameters["Y1_recu"]) * (TiempoTranscurrido - (double)_parameters["X1_recu"]) / ((double)_parameters["X2_recu"] - (double)_parameters["X1_recu"]);
                            break;
                    }
                    if (maximoY > 1) maximoY = 1;
                }

                //Modificar el valor de la maxima recuperación si se usa el "% de eficiencia del Mantenimiento"
                EficienciaMto = 100;
                if (_parameters.ContainsKey("ley_eficiencia_mto"))
                {
                    switch ((string)_parameters["ley_eficiencia_mto"])
                    {
                        //A)Determinación del parametro "EficienciaMto"
                        case "Fijo":
                            EficienciaMto = (double)_parameters["ley_eficiencia_mto_param1"];
                            break;
                        case "Uniforme":
                            EficienciaMto = GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme((double)_parameters["Minimo_eficiencia_mto"], (double)_parameters["Maximo_eficiencia_mto"], r);
                            break;
                        case "Exponencial":
                            EficienciaMto = GeneradoresDeAleatorios.Generador_Aleatorio_Exponencial((double)_parameters["ley_eficiencia_mto_param1"], 1 / (double)_parameters["ley_eficiencia_mto_param2"], (double)_parameters["Minimo_eficiencia_mto"], (double)_parameters["Maximo_eficiencia_mto"], r);
                            break;
                        case "Weibull2P":
                            EficienciaMto = GeneradoresDeAleatorios.Generador_Aleatorio_Weibull_2P((double)_parameters["ley_eficiencia_mto_param1"], (double)_parameters["ley_eficiencia_mto_param2"], (double)_parameters["Minimo_eficiencia_mto"], (double)_parameters["Maximo_eficiencia_mto"], r);
                            break;
                        case "Línea recta":
                            EficienciaMto = (double)_parameters["Y1_eficiencia_mto"] + ((double)_parameters["Y2_eficiencia_mto"] - (double)_parameters["Y1_eficiencia_mto"]) * (TiempoTranscurrido - (double)_parameters["X1_eficiencia_mto"]) / ((double)_parameters["X2_eficiencia_mto"] - (double)_parameters["X1_eficiencia_mto"]);
                            break;
                        case "Normal":
                            EficienciaMto = GeneradoresDeAleatorios.Generador_Aleatorio_Normal((double)_parameters["ley_eficiencia_mto_param1"], (double)_parameters["ley_eficiencia_mto_param2"], (double)_parameters["Minimo_eficiencia_mto"], (double)_parameters["Maximo_eficiencia_mto"], r);
                            break;
                    }
                    //B)Modificación de la variable que contiene el maximo de recuperación 
                    if (EficienciaMto > 0 && EficienciaMto <= 100) maximoY = maximoY * EficienciaMto / 100;
                }


                //Generar los costes de la recuperacion y de pérdida de producción
                if (_parameters.ContainsKey("ley_coste"))
                {
                    switch ((string)_parameters["ley_coste"])
                    {
                        case "Fijo por tiempo":
                            CosteEsteMantenimiento = (double)_parameters["ley_coste_param1"] * t;

                            //Si en lugar de un Mto correctivo se está realizando un Mto Preventivo entonces corregir el coste de Mantenimiento
                            if (tipo_mto_este_ciclo == "Preventivo" && _parameters.ContainsKey("coste_Reduccion_si_Preventivo")) CosteEsteMantenimiento = (100 - (double)_parameters["coste_Reduccion_si_Preventivo"]) * CosteEsteMantenimiento / 100;

                            CosteEstaPerdidaDeProduccion = (double)_parameters["ley_coste_param2"] * t;
                            break;
                        case "Fijo por intervención":
                            CosteEsteMantenimiento = (double)_parameters["ley_coste_param1"];

                            //Si en lugar de un Mto correctivo se está realizando un Mto Preventivo entonces corregir el coste de Mantenimiento 
                            if (tipo_mto_este_ciclo == "Preventivo" && _parameters.ContainsKey("coste_Reduccion_si_Preventivo")) CosteEsteMantenimiento = (100 - (double)_parameters["coste_Reduccion_si_Preventivo"]) * CosteEsteMantenimiento / 100;

                            CosteEstaPerdidaDeProduccion = (double)_parameters["ley_coste_param2"] * t;
                            break;
                        case "Weibull2P":
                            CosteEsteMantenimiento = GeneradoresDeAleatorios.Generador_Aleatorio_Weibull_2P((double)_parameters["ley_coste_param1"], (double)_parameters["ley_coste_param2"], (double)_parameters["Minimo_coste"], (double)_parameters["Maximo_coste"], r) * t;

                            //Si en lugar de un Mto correctivo se está realizando un Mto Preventivo entonces corregir el coste de Mantenimiento
                            if (tipo_mto_este_ciclo == "Preventivo" && _parameters.ContainsKey("coste_Reduccion_si_Preventivo")) CosteEsteMantenimiento = (100 - (double)_parameters["coste_Reduccion_si_Preventivo"]) * CosteEsteMantenimiento / 100;

                            CosteEstaPerdidaDeProduccion = (double)_parameters["coste_Perdida_Prod_por_Ud_tiempo"] * t;
                            break;
                        case "Normal":
                            CosteEsteMantenimiento = GeneradoresDeAleatorios.Generador_Aleatorio_Normal((double)_parameters["ley_coste_param1"], (double)_parameters["ley_coste_param2"], (double)_parameters["Minimo_coste"], (double)_parameters["Maximo_coste"], r) * t;

                            //Si en lugar de un Mto correctivo se está realizando un Mto Preventivo entonces corregir el coste de Mantenimiento
                            if (tipo_mto_este_ciclo == "Preventivo" && _parameters.ContainsKey("coste_Reduccion_si_Preventivo")) CosteEsteMantenimiento = (100 - (double)_parameters["coste_Reduccion_si_Preventivo"]) * CosteEsteMantenimiento / 100;

                            CosteEstaPerdidaDeProduccion = (double)_parameters["coste_Perdida_Prod_por_Ud_tiempo"] * t;
                            break;
                        case "Lineal creciente":
                            CosteEsteMantenimiento = ((double)_parameters["Y1_coste"] + ((double)_parameters["Y2_coste"] - (double)_parameters["Y1_coste"]) * (TiempoTranscurrido - (double)_parameters["X1_coste"]) / ((double)_parameters["X2_coste"] - (double)_parameters["X1_coste"])) * t;

                            //Si en lugar de un Mto correctivo se está realizando un Mto Preventivo entonces corregir el coste de Mantenimiento 
                            if (tipo_mto_este_ciclo == "Preventivo" && _parameters.ContainsKey("coste_Reduccion_si_Preventivo")) CosteEsteMantenimiento = (100 - (double)_parameters["coste_Reduccion_si_Preventivo"]) * CosteEsteMantenimiento / 100;

                            CosteEstaPerdidaDeProduccion = (double)_parameters["coste_Perdida_Prod_por_Ud_tiempo"] * t;
                            break;
                        case "Desglose de Costes":
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
                                if (_parameters.ContainsKey("coste_recon_Reduccion_si_Preventivo")) coste_recon = (100 - (double)_parameters["coste_recon_Reduccion_si_Preventivo"]) * coste_recon / 100;
                                if (_parameters.ContainsKey("coste_diag_Reduccion_si_Preventivo")) coste_diag = (100 - (double)_parameters["coste_diag_Reduccion_si_Preventivo"]) * coste_diag / 100;
                                if (_parameters.ContainsKey("coste_prep_Reduccion_si_Preventivo")) coste_prep = (100 - (double)_parameters["coste_prep_Reduccion_si_Preventivo"]) * coste_prep / 100;
                                if (_parameters.ContainsKey("coste_desm_Reduccion_si_Preventivo")) coste_desm = (100 - (double)_parameters["coste_desm_Reduccion_si_Preventivo"]) * coste_desm / 100;
                                if (_parameters.ContainsKey("coste_repa_Reduccion_si_Preventivo")) coste_repa = (100 - (double)_parameters["coste_repa_Reduccion_si_Preventivo"]) * coste_repa / 100;
                                if (_parameters.ContainsKey("coste_ensam_Reduccion_si_Preventivo")) coste_ensam = (100 - (double)_parameters["coste_ensam_Reduccion_si_Preventivo"]) * coste_ensam / 100;
                                if (_parameters.ContainsKey("coste_verif_Reduccion_si_Preventivo")) coste_verif = (100 - (double)_parameters["coste_verif_Reduccion_si_Preventivo"]) * coste_verif / 100;
                                if (_parameters.ContainsKey("coste_serv_Reduccion_si_Preventivo")) coste_serv = (100 - (double)_parameters["coste_serv_Reduccion_si_Preventivo"]) * coste_serv / 100;
                            }

                            //Se calcula ahora el coste de parada como la suma de los costes del desglose
                            CosteEsteMantenimiento = coste_recon + coste_diag + coste_prep + coste_desm + coste_repa + coste_ensam + coste_verif + coste_serv;


                            //Calculo de los costes desglosados de perdida de producción
                            if (_parameters.ContainsKey("coste_recon_Perdida_Prod_por_Ud_tiempo")) coste_prod_recon = (double)_parameters["coste_recon_Perdida_Prod_por_Ud_tiempo"] * t_paro_recon;
                            if (_parameters.ContainsKey("coste_diag_Perdida_Prod_por_Ud_tiempo")) coste_prod_diag = (double)_parameters["coste_diag_Perdida_Prod_por_Ud_tiempo"] * t_paro_diag;
                            if (_parameters.ContainsKey("coste_prep_Perdida_Prod_por_Ud_tiempo")) coste_prod_prep = (double)_parameters["coste_prep_Perdida_Prod_por_Ud_tiempo"] * t_paro_prep;
                            if (_parameters.ContainsKey("coste_desm_Perdida_Prod_por_Ud_tiempo")) coste_prod_desm = (double)_parameters["coste_desm_Perdida_Prod_por_Ud_tiempo"] * t_paro_desm;
                            if (_parameters.ContainsKey("coste_repa_Perdida_Prod_por_Ud_tiempo")) coste_prod_repa = (double)_parameters["coste_repa_Perdida_Prod_por_Ud_tiempo"] * t_paro_repa;
                            if (_parameters.ContainsKey("coste_ensam_Perdida_Prod_por_Ud_tiempo")) coste_prod_ensam = (double)_parameters["coste_ensam_Perdida_Prod_por_Ud_tiempo"] * t_paro_ensam;
                            if (_parameters.ContainsKey("coste_verif_Perdida_Prod_por_Ud_tiempo")) coste_prod_verif = (double)_parameters["coste_verif_Perdida_Prod_por_Ud_tiempo"] * t_paro_verif;
                            if (_parameters.ContainsKey("coste_serv_Perdida_Prod_por_Ud_tiempo")) coste_prod_serv = (double)_parameters["coste_serv_Perdida_Prod_por_Ud_tiempo"] * t_paro_serv;

                            //Calculo del coste total de perdida de produccion como suma de los costes desglosados de perdida de producción
                            CosteEstaPerdidaDeProduccion = coste_prod_recon + coste_prod_diag + coste_prod_prep + coste_prod_desm + coste_prod_repa + coste_prod_ensam + coste_prod_verif + coste_prod_serv;
                            break;
                    }

                    //Acumular costes derivados del paro/fallo
                    CosteTotalMantenimiento += CosteEsteMantenimiento;
                    CosteTotalPerdidaProduccion += CosteEstaPerdidaDeProduccion;
                }

                //Almacenar programa de Mantenimiento preventivo si procede e Incrementar los contadores de Preventivo o correctivo según proceda
                if (tipo_mto_este_ciclo == "Preventivo")
                {
                    programa_mto_preventivo[TiempoTranscurrido] = t;
                    Numero_de_Preventivos += 1;
                }

                else if (tipo_mto_este_ciclo == "Correctivo") Numero_de_Correctivos += 1;

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
                MTTR_SinLog = (TiempoParadoAcumulado - TiempoDeLogistica) / ContadorCiclosFuncionaPara;
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
                if ((string)_parameters["ley_paro"] == "Desglose de Fallos")
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
                if (_parameters.ContainsKey("ley_coste"))
                {
                    if ((string)_parameters["ley_coste"] == "Desglose de Costes")
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
                    if ((string)_parameters["ley_coste"] == "Desglose de Costes")
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

                if (_parameters.ContainsKey("preventivo") && _parameters.ContainsKey("tipo_de_preventivo"))
                {
                    if ((string)_parameters["preventivo"] == "Activado" && (string)_parameters["tipo_de_preventivo"] == "Fijo por tiempo")
                    {
                        textBox5.Text += " Tiempo hasta el siguiente preventivo   = " + TiempoHastaSiguientePreventivo.ToString("0.##") + "\r\n";
                    }
                }

                textBox5.Text += "  " + "\r\n";
                textBox5.Text += "  " + "\r\n";

                //Estimar el tamaño a rellenar de la barra de calculos y rellenarlo
                double PorcentajeDeSimulacionRealizadoEnEsteCiclo = (TiempoDelCiclo / Tiempo_A_Simular) * 100;
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
            foreach (string key in resultados.Keys)
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

            // Habilitar el ComboBox de Ver Gráficas
            HabilitarComboBoxVerGraficas();

            // Terminar de rellenar la barra de progreso de los calculos
            progressBar1.Value = progressBar1.Maximum;
            progressBar1.Visible = false;
            comboBox7.Visible = true;
            // Se vuelve a habilitar los botones
            button4.Enabled = true;
            button_Reset.Enabled = true;

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
    }
}

