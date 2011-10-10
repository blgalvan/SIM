
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace SIM
{
    public class FormateoDatos
    {
        /**************************************************************************/
        /****************** MÉTODO PARA CONTROL EVENTOS DE TECLADO ****************/
        /**************************************************************************/
        #region leer datos
        /// <summary>
        /// Método que sólo permite introducir en los textbox numeros y coma.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="obj"></param>
        public static void numOcoma(KeyPressEventArgs e, TextBox obj)
        {
            // Cuento las comas
            int count = 0;
            for (int i = 0; i < obj.Text.Length; i++)
            {
                int j = obj.Text[i].CompareTo(',');
                if (j == 0)
                {
                    count = 1;
                }
            }
            // Deshabilito la escritura de algunas teclas que no son números.
            if (((e.KeyChar) < 48 && e.KeyChar != 8 && e.KeyChar != 44) || e.KeyChar > 57 || (count == 1 && e.KeyChar == 44))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Método que sólo permite introducir en los textbox numeros, comas y salto de linea.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="obj"></param>
        public static void numOcoma_TextboxMultilinia(KeyPressEventArgs e, TextBox obj)
        {
            int count2 = 0;
            for (int k = 0; k <= 10; k++)
            {
                // Cuento las comas
                int count = 0;
                for (int i = 0; i < obj.Text.Length; i++)
                {
                    int j = obj.Text[i].CompareTo(',');
                    int j2 = obj.Text[i].CompareTo('\t');
                    if (j == 0)
                    {
                        count = 1;
                    }
                    if (j2 == 0)
                    {
                        count2 = 1;
                    }
                }

                // Deshabilito la escritura de algunas teclas que no son números.
                if (((e.KeyChar) < 48 && e.KeyChar != 8 && e.KeyChar != 44 && e.KeyChar != 13) || e.KeyChar > 57 || ((count == 1 && e.KeyChar == 44) && (count2 == 1 && e.KeyChar == 13)))
                {
                    e.Handled = true;
                }
                count2 = 0;
            }
        }

        /// <summary>
        /// Método que sólo permite introducir en los textbox numeros, comas y salto de linea.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="obj"></param>
        public static void numOcoma_TextboxMultilinia_2(KeyPressEventArgs e, TextBox obj)
        {
            int count2 = 0;
            for (int k = 0; k <= 10; k++)
            {
                // Cuento las comas
                int count = 0;
                for (int i = 0; i < obj.Text.Length; i++)
                {
                    int j = obj.Text[i].CompareTo(',');
                    int j2 = obj.Text[i].CompareTo('\t');
                    if (j == 0)
                    {
                        count = 1;
                    }
                    if (j2 == 0)
                    {
                        count2 = 1;
                    }
                }

                // Deshabilito la escritura de algunas teclas que no son números.
                if (((e.KeyChar) < 48 && e.KeyChar != 8 && e.KeyChar != 44 && e.KeyChar != 13 && e.KeyChar != 32) || e.KeyChar > 57 || ((count == 1 && e.KeyChar == 44) && (count2 == 1 && e.KeyChar == 13)))
                {
                    e.Handled = true;
                }
                count2 = 0;
            }
        }

        /// <summary>
        /// Método que comprueba que lo que se introduzca en los campos de texto sean SOLO numeros, no se permiten comas.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="obj"></param>
        public static void soloNum(KeyPressEventArgs e, TextBox obj)
        {
            // Se deshabilitan los caracteres que no sean números
            if (((e.KeyChar < 48) && (e.KeyChar != 8)) || (e.KeyChar > 57))
            {
                e.Handled = true;
            }
        }
        #endregion leer datos

        /**************************************************************************/
        /*********************           OTRAS FUNCIONES       ********************/
        /**************************************************************************/
        #region OTRAS_FUNCIONES
        /// <summary>
        /// Método que valida los datos introducidos en un textbox multilinea, y devuelve el numero de
        /// datos introducidos y un vector con los datos introducidos.
        /// </summary>
        /// <param name="TBOX"></param>
        /// <param name="numeroDatos"></param>
        /// <param name="split1"></param>
        public static void TratamientoTextbox(TextBox TBOX, ref int numeroDatos, ref string[] split1)
        {
            //Los datos se leeran como texto en el string llamado "datos_texto"
            string datos_texto = "";

            //Leer los datos en fotma de texto desde el TextBox correspondiente
            datos_texto = TBOX.Text;

            //Cambiar los puntos por comas (ya que se supone un error a la hora de introducir el dato)
            datos_texto = datos_texto.Replace(".", ",");

            //Cambiar los retorno de carro (enter)por un espacio(ya que da error al tratar el dato)
            datos_texto = datos_texto.Replace("\t", "\r");

            //Cambiar los espacios en blanco múltiples (dobles, triples, ...) por un espacio simple
            //Se supone que mas de un espacio en blanco entre caracteres es un error al introducir datos
            datos_texto = datos_texto.Replace("       ", " ");
            datos_texto = datos_texto.Replace("      ", " ");
            datos_texto = datos_texto.Replace("     ", " ");
            datos_texto = datos_texto.Replace("    ", " ");
            datos_texto = datos_texto.Replace("   ", " ");
            datos_texto = datos_texto.Replace("  ", " ");

            //Eliminar posibles espacios en blanco al principio y al final de la cadena de datos
            datos_texto = datos_texto.Trim();
            //Separar cada dato introducido por el usuario y guardarlos en "split[]" como texto
            split1 = datos_texto.Split(new Char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            numeroDatos = split1.Length;

            //Actualizamos datos introducidos en el textbox
            ActualizarTextBox(ref split1, TBOX, numeroDatos);
        }
        //Funcion que calcula el valor maximo de un vector 
        /// <summary>
        /// Método que devuelve el valor maximo de un vector.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static double maximo(double[] v)
        {
            double resultado = double.MinValue;
            for (int i = 0; i < v.Length; i++)
            {
                if (v[i] > resultado)
                {
                    resultado = v[i];
                }
            }
            return resultado;
        }
        /// <summary>
        /// Método que valida los datos introducidos en un textbox multilinea, y devuelve el numero de
        /// datos introducidos y un vector con los datos introducidos.
        /// </summary>
        /// <param name="TBOX"></param>
        /// <param name="numeroDatos"></param>
        /// <param name="split1"></param>
        public static void TratamientoTextbox_2(TextBox TBOX, ref int numeroDatos, ref string[] split1)
        {
            //Los datos se leeran como texto en el string llamado "datos_texto"
            string datos_texto = "";

            //Leer los datos en fotma de texto desde el TextBox correspondiente
            datos_texto = TBOX.Text;

            //Cambiar los puntos por comas (ya que se supone un error a la hora de introducir el dato)
            datos_texto = datos_texto.Replace(".", ",");

            //Cambiar los retorno de carro (enter)por un espacio(ya que da error al tratar el dato)
            datos_texto = datos_texto.Replace("\t", "\r");

            //Cambiar los espacios en blanco múltiples (dobles, triples, ...) por un espacio simple
            //Se supone que mas de un espacio en blanco entre caracteres es un error al introducir datos
            datos_texto = datos_texto.Replace("       ", " ");
            datos_texto = datos_texto.Replace("      ", " ");
            datos_texto = datos_texto.Replace("     ", " ");
            datos_texto = datos_texto.Replace("    ", " ");
            datos_texto = datos_texto.Replace("   ", " ");
            datos_texto = datos_texto.Replace("  ", " ");

            //Eliminar posibles espacios en blanco al principio y al final de la cadena de datos
            datos_texto = datos_texto.Trim();
            //Separar cada dato introducido por el usuario y guardarlos en "split[]" como texto
            //split1 = datos_texto.Split(new Char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            split1 = datos_texto.Split(new Char[] { '\r' }, StringSplitOptions.RemoveEmptyEntries);
            numeroDatos = split1.Length;

            //Actualizamos datos introducidos en el textbox
            ActualizarTextBox(ref split1, TBOX, numeroDatos);
        }
        /// <summary>
        /// Método que valida los datos introducidos en un textbox multilinea, y devuelve el numero de
        /// datos introducidos y varios vectores segun numeros que se hayan introducido separados por espacio.
        /// </summary>
        /// <param name="TBOX"></param>
        /// <param name="numeroDatos"></param>
        /// <param name="split1"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="y2"></param>
        /// <param name="y3"></param>
        /// <param name="y4"></param>
        /// <param name="y5"></param>
        // public static void TratamientoTextbox_3(TextBox TBOX, ref int numeroDatos, ref string[] split1, ref double[] x,
        // ref double[] y, ref double[] y2, ref double[] y3, ref double[] y4, ref double[] y5)

        public static void TratamientoTextbox_3(string TBOX, ref int numeroDatos, ref string[] split1, ref double[] x,
            ref double[] y, ref double[] y2, ref double[] y3, ref double[] y4, ref double[] y5)
        {
            //Los datos se leeran como texto en el string llamado "datos_texto"
            string datos_texto = "";

            //Leer los datos en fotma de texto desde el TextBox correspondiente
            //datos_texto = TBOX.Text;
            datos_texto = TBOX;

            //Cambiar los puntos por comas (ya que se supone un error a la hora de introducir el dato)
            datos_texto = datos_texto.Replace(".", ",");

            //Cambiar los retorno de carro (enter)por un espacio(ya que da error al tratar el dato)
            datos_texto = datos_texto.Replace("\t", "\r");

            //Cambiar los espacios en blanco múltiples (dobles, triples, ...) por un espacio simple
            //Se supone que mas de un espacio en blanco entre caracteres es un error al introducir datos
            datos_texto = datos_texto.Replace("       ", " ");
            datos_texto = datos_texto.Replace("      ", " ");
            datos_texto = datos_texto.Replace("     ", " ");
            datos_texto = datos_texto.Replace("    ", " ");
            datos_texto = datos_texto.Replace("   ", " ");
            datos_texto = datos_texto.Replace("  ", " ");

            //Eliminar posibles espacios en blanco al principio y al final de la cadena de datos
            datos_texto = datos_texto.Trim();
            //Separar cada dato introducido por el usuario y guardarlos en "split[]" como texto
            //split1 = datos_texto.Split(new Char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            split1 = datos_texto.Split(new Char[] { '\r' }, StringSplitOptions.RemoveEmptyEntries);
            numeroDatos = split1.Length;

            //Los datos se almacenarán como números en arrays llamados "x[]" e "y[]"
            //double[] x, y, y2, y3, y4, y5;
            x = new double[numeroDatos];
            y = new double[numeroDatos];
            y2 = new double[numeroDatos];
            y3 = new double[numeroDatos];
            y4 = new double[numeroDatos];
            y5 = new double[numeroDatos];

            //variables para controlar cada subcadena que puede contener uno o dos datos
            int longitud_subcadena; //almacenará la longitud de la subcadena completa
            int posicion_separador; //almacenará la posición del separador (espacio en blanco) entre el primer y el segundo dato


            //Convertir los datos de texto a número
            for (int k = 0; k <= numeroDatos - 1; k++)
            {
                //Eliminar posibles espacios en blanco al principio y final de la subcadena
                split1[k] = split1[k].Trim();

                //Averiguar la longitud total de la subcadena
                longitud_subcadena = split1[k].Length;

                //Averiguamos el numero de espacios en blanco para ver el numero de vectores a generar
                int cont = 0;
                int inicio = 0;
                int kk = 0;
                int pos = split1[kk].IndexOf(" ");
                //separamos los datos en cada indice del vector split que esten separados con espacio y los 
                //almacenamos en vectores VDatos                    
                while (pos != -1)
                {
                    cont++;
                    inicio = pos + 1;
                    pos = split1[kk].IndexOf(' ', inicio);
                }
                //Averiguar la posición del separador (espacio en blanco entre ambos datos)
                //Si no existe espacio en blanco implica que existe un solo dato y se devuelve un "-1"
                posicion_separador = split1[k].IndexOf(" ");

                if (cont == 0)
                    x[k] = Convert.ToDouble(split1[k].Trim());

                else if (cont == 1)
                {
                    //if (Convert.ToDouble(split1[k].Substring(0, posicion_separador)) != null)
                    x[k] = Convert.ToDouble(split1[k].Substring(0, posicion_separador));
                    //else//ponemos cero en caso de que el usurio no introduzca valor
                    // x[k] = 0;
                    //if (Convert.ToDouble(split1[k].Substring(posicion_separador + 1)) != null)
                    y[k] = Convert.ToDouble(split1[k].Substring(posicion_separador + 1));
                    //else
                    //  y[k] = 0;
                }
                else if (cont == 2)
                {
                    x[k] = Convert.ToDouble(split1[k].Substring(0, posicion_separador));
                    inicio = posicion_separador + 1;
                    posicion_separador = split1[k].IndexOf(' ', inicio);
                    y[k] = Convert.ToDouble(split1[k].Substring(inicio, posicion_separador - inicio));
                    y2[k] = Convert.ToDouble(split1[k].Substring(posicion_separador + 1));
                }
                else if (cont == 3)
                {
                    x[k] = Convert.ToDouble(split1[k].Substring(0, posicion_separador));
                    inicio = posicion_separador + 1;
                    posicion_separador = split1[k].IndexOf(' ', inicio);
                    y[k] = Convert.ToDouble(split1[k].Substring(inicio, posicion_separador - inicio));
                    inicio = posicion_separador + 1;
                    posicion_separador = split1[k].IndexOf(' ', inicio);
                    y2[k] = Convert.ToDouble(split1[k].Substring(inicio, posicion_separador - inicio));
                    y3[k] = Convert.ToDouble(split1[k].Substring(posicion_separador + 1));
                }
                else if (cont == 4)
                {
                    x[k] = Convert.ToDouble(split1[k].Substring(0, posicion_separador));
                    inicio = posicion_separador + 1;
                    posicion_separador = split1[k].IndexOf(' ', inicio);
                    y[k] = Convert.ToDouble(split1[k].Substring(inicio, posicion_separador - inicio));
                    inicio = posicion_separador + 1;
                    posicion_separador = split1[k].IndexOf(' ', inicio);
                    y2[k] = Convert.ToDouble(split1[k].Substring(inicio, posicion_separador - inicio));
                    inicio = posicion_separador + 1;
                    posicion_separador = split1[k].IndexOf(' ', inicio);
                    y3[k] = Convert.ToDouble(split1[k].Substring(inicio, posicion_separador - inicio));
                    y4[k] = Convert.ToDouble(split1[k].Substring(posicion_separador + 1));
                }
                else if (cont == 5)
                {
                    x[k] = Convert.ToDouble(split1[k].Substring(0, posicion_separador));
                    inicio = posicion_separador + 1;
                    posicion_separador = split1[k].IndexOf(' ', inicio);
                    y[k] = Convert.ToDouble(split1[k].Substring(inicio, posicion_separador - inicio));
                    inicio = posicion_separador + 1;
                    posicion_separador = split1[k].IndexOf(' ', inicio);
                    y2[k] = Convert.ToDouble(split1[k].Substring(inicio, posicion_separador - inicio));
                    inicio = posicion_separador + 1;
                    posicion_separador = split1[k].IndexOf(' ', inicio);
                    y3[k] = Convert.ToDouble(split1[k].Substring(inicio, posicion_separador - inicio));
                    inicio = posicion_separador + 1;
                    posicion_separador = split1[k].IndexOf(' ', inicio);
                    y4[k] = Convert.ToDouble(split1[k].Substring(inicio, posicion_separador - inicio));
                    y5[k] = Convert.ToDouble(split1[k].Substring(posicion_separador + 1));
                }
            }

            //Ya están los datos en forma numérica disponibles los arrays tipo double llamados "x[]" e "y[]"

            //El número de datos lo indica la variable tipo entero llamada "numero_datos"

            //Aqui finaliza la captura de los datos introducidos por el usuario
            //El resto de este codigo es solo para efectos de comprobación de que todo fuinciona bien

            //Convertir los datos en forma numérica a texto guardado en las variables de caracteres denominadas 
            //"resultado_x" y "resultado_y"
            string resultado_x = "";
            string resultado_y = "";
            string resultado_y2 = "";
            string resultado_y3 = "";
            string resultado_y4 = "";
            string resultado_y5 = "";

            for (int k = 0; k <= numeroDatos - 1; k++)
            {
                resultado_x = resultado_x + Convert.ToString(x[k]) + " ";
                resultado_y = resultado_y + Convert.ToString(y[k]) + " ";
                resultado_y2 = resultado_y2 + Convert.ToString(y2[k]) + " ";
                resultado_y3 = resultado_y3 + Convert.ToString(y3[k]) + " ";
                resultado_y4 = resultado_y4 + Convert.ToString(y4[k]) + " ";
                resultado_y5 = resultado_y5 + Convert.ToString(y5[k]) + " ";
            }

            //Actualizamos datos introducidos en el textbox
            //ActualizarTextBox(ref split1, TBOX, numeroDatos);

            //Ordenamos el vector. Solo se ordena el vector x
            double[] Vpos = new double[numeroDatos];
            //ActualizarTextBox_Ordenacion(ref x, ref y, numeroDatos, ref Vpos);
            ActualizarTextBox_Ordenacion(ref x, ref y, numeroDatos);
            // Ordenamos el resto de vectores segun el indice que devuelve el vector Vpos
            /*for (int i = 0; i < numeroDatos; i++)
            {
                y[i] = y[Convert.ToInt16(Vpos[i])];
                y[i] = y2[Convert.ToInt16(Vpos[i])];
                y[i] = y3[Convert.ToInt16(Vpos[i])];
                y[i] = y4[Convert.ToInt16(Vpos[i])];
                y[i] = y5[Convert.ToInt16(Vpos[i])];
            }*/

            /*ActualizarTextBox_Ordenacion(ref x, numeroDatos);
            ActualizarTextBox_Ordenacion(ref y, numeroDatos);
            ActualizarTextBox_Ordenacion(ref y2, numeroDatos);
            ActualizarTextBox_Ordenacion(ref y3, numeroDatos);
            ActualizarTextBox_Ordenacion(ref y4, numeroDatos);
            ActualizarTextBox_Ordenacion(ref y5, numeroDatos);*/
        }

        //Funcion que calcula el valor minimo de un vector 
        /// <summary>
        /// Método que devuelve el valor minimo de un vector.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static double minimo(double[] v)
        {
            double resultado = double.MaxValue;
            for (int i = 0; i < v.Length; i++)
            {
                if (v[i] < resultado)
                {
                    resultado = v[i];
                }
            }
            return resultado;
        }
        //Funcion que actualiza los datos introducidos por el usuario 
        //Sin ordenar textbox
        /// <summary>
        /// Método que se encarga de validar los datos introducidos en textbox y devuelve un vector con los datos validados.
        /// </summary>
        /// <param name="VectorString"></param>
        /// <param name="Tbox"></param>
        /// <param name="NUM_DATOS"></param>
        public static void ActualizarTextBox(ref string[] VectorString, TextBox Tbox, int NUM_DATOS)
        {
            string cad = "";
            Tbox.Text = "";
            double valornumerico;
            try
            {
                for (int k = 0; k < NUM_DATOS; k++)
                {
                    cad = EliminarComas(VectorString[k]);
                    valornumerico = Convert.ToDouble(cad);
                    VectorString[k] = Convert.ToString(valornumerico);
                    Tbox.Text += Convert.ToString(valornumerico) + "\r\n";
                }
            }
            catch
            {
                MessageBox.Show("Los valores introducidos no son correctos");
            }
        }
        //ordenando el textbox
        /// <summary>
        /// Método que se encarga de validar los datos introducidos en textbox y devuelve un vector con los datos validadosy ordenados.
        /// </summary>
        /// <param name="VectorString"></param>
        /// <param name="Tbox"></param>
        /// <param name="NUM_DATOS"></param>
        /*public static void ActualizarTextBox_Ordenacion(ref string[] VectorString, TextBox Tbox, int NUM_DATOS)
        {
            string cad = "";
            Tbox.Text = "";
            OrdenarBurbuja(VectorString);
            double valornumerico;
            try
            {
                for (int k = 0; k < NUM_DATOS; k++)
                {
                    cad = EliminarComas(VectorString[k]);
                    valornumerico = Convert.ToDouble(cad);
                    VectorString[k] = Convert.ToString(valornumerico);
                    Tbox.Text += Convert.ToString(valornumerico) + "\r\n";
                }
            }
            catch
            {
                MessageBox.Show("Los valores introducidos no son correctos");
            }
        }*/
         public static void ActualizarTextBox_Ordenacion(ref double[] Vector1, ref double[] Vector2, int NUM_DATOS)
        //public static void ActualizarTextBox_Ordenacion(ref double[] Vector1, ref double[] Vector2, int NUM_DATOS, ref double[] pos)
        {
            string cad = "";
            //OrdenarBurbuja(Vector1, Vector2, ref pos);
            OrdenarBurbuja(Vector1, Vector2);
            double valornumerico;
            try
            {
                for (int k = 0; k < NUM_DATOS; k++)
                {
                    cad = EliminarComas(Convert.ToString(Vector2[k]));
                    valornumerico = Convert.ToDouble(cad);
                    Vector2[k] = valornumerico;
                }
            }
            catch
            {
                MessageBox.Show("Los valores introducidos no son correctos");
            }
        }
        /// <summary>
        /// Metodo que devuelve un vector con los datos validados.
        /// </summary>
        /// <param name="TBX"></param>
        /// <param name="VectorDatos"></param>
        /// <param name="cadena"></param>
        /// <param name="NUM_DATOS"></param>
        public static void ValidarTextox(TextBox TBX, ref double[] VectorDatos, ref string cadena, int NUM_DATOS)
        {
            cadena = "";
            cadena = TBX.Text;
            cadena = TBX.Text.Replace("" + "\r\n", "0" + "\r\n");
            string[] VectorSplit = cadena.Split(new Char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            cadena = "";
            for (int j = 0; j < NUM_DATOS; j++)
            {
                if (j < VectorSplit.Length - 1)
                {
                    VectorDatos[j] = Convert.ToDouble(VectorSplit[j]);
                    VectorDatos[j] = VectorDatos[j] / 10;
                    cadena += Convert.ToString(VectorDatos[j]) + "\r\n";
                }
                else
                {
                    VectorDatos[j] = Convert.ToDouble(VectorSplit[j]);
                    cadena += Convert.ToString(VectorDatos[j]);
                }

            }
            int pos = cadena.IndexOf("\r\n", VectorSplit.Length - 2);
            if (pos != -1) cadena = cadena.Replace("\r\n", "@@@");
            VectorDatos[pos] = 0;
        }
        /// <summary>
        /// Metodo que devuelve un vector con los datos validados.
        /// </summary>
        /// <param name="TBX"></param>
        /// <param name="VectorDatos"></param>
        /// <param name="cadena"></param>
        /// <param name="CAD"></param>
        /// <param name="NUM_DATOS"></param>
        public static void ValidarTextox2(TextBox TBX, ref double[] VectorDatos, ref string cadena, ref string[] CAD, int NUM_DATOS)
        {
            cadena = "";
            cadena = TBX.Text;
            string[] VectorSplit = cadena.Split(new Char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            cadena = "";
            for (int j = 0; j < NUM_DATOS; j++)
            {
                VectorDatos[j] = Convert.ToDouble(VectorSplit[j]);
                CAD[j] = VectorSplit[j];
                cadena += CAD[j] + "\r\n";
            }
        }

        // ordenar elementos de un arreglo con el metodo burbuja
        /// <summary>
        /// Metodo que ordena un vector
        /// </summary>
        /// <param name="b"></param>
        /*public static void OrdenarBurbuja(string[] b)
        {
            for (int pasadas = 1; pasadas < b.Length; pasadas++) // pasadas
                for (int i = 0; i < b.Length - 1; i++)
                    if (Convert.ToDouble(b[i]) > Convert.ToDouble(b[i + 1]))      // comparar
                        intercambio(b, i);         // intercambiar
        }*/
        /// <summary>
        /// Metodo que ordena el vector b y devuelve el vector posicion que contiene las posiciones del vector ordenado
        /// </summary>
        /// <param name="b"></param>
        /// <param name="posicion"></param>
        public static void OrdenarBurbuja(double[] b1,double[] b2)
            //public static void OrdenarBurbuja(double[] b1,double[] b2, ref double[] posicion)
        {
            for (int pasadas = 1; pasadas < b1.Length; pasadas++) // pasadas
            {
                int j = 0;
                for (int i = 0; i < b1.Length - 1; i++)
                {
                    if ((b1[i]) > (b1[i + 1]))      // comparar
                    {
                        intercambio(b1,b2,i);         // intercambiar
                        //posicion[j] = i + 1;
                    }
                    j++;
                }
            }
        }
        // intercambio de dos elementos en un arreglo
        /// <summary>
        /// Metodo que se encarga de intercambiar los valores de un vector para ordenarlo
        /// </summary>
        /// <param name="c"></param>
        /// <param name="primero"></param>
       /* public static void intercambio(string[] c, int primero)
        {
            string temp;      // variable temporal para el intercambio
            temp = c[primero];
            c[primero] = c[primero + 1];
            c[primero + 1] = temp;
        }*/
        public static void intercambio(double[] c,double[] d, int primero)
        {
            double temp1, temp2;      // variable temporal para el intercambio
            temp1 = c[primero];
            temp2 = d[primero];
            c[primero] = c[primero + 1];
            d[primero] = d[primero + 1];
            c[primero + 1] = temp1;
            d[primero + 1] = temp2;
        }
        //Funcion que elimina mas de una coma introducida en los textbox multilinea 
        /// <summary>
        /// Metodo que elimina comas insertadas en un textbox. Solo mantiene una coma en caso de introducir 
        /// varias por roor, las elimina dejando solo una coma.
        /// </summary>
        /// <param name="cadena"></param>
        /// <returns></returns>
        public static string EliminarComas(string cadena)
        {
            int pos = cadena.IndexOf(",");
            if (pos != -1)
            {
                pos++;
                while ((pos = cadena.IndexOf(',', pos)) != -1)
                {
                    cadena = cadena.Remove(pos, 1);//elimina la coma en la posicion encontrada
                }
            }
            //verificamos si nos queda una coma al final de la cadena, y lo eliminamos
            int longitud = cadena.Length;
            if (cadena.IndexOf(',') == longitud - 1) cadena = cadena.Remove(longitud - 1, 1);//elimina la coma en la posicion encontrada
            return cadena;
        }


        #endregion
        /**************************************************************************/
        /*********************           TEXTBOX_CHANGED          *****************/
        /**************************************************************************/

        #region textboxchange funcion matematica

        //private void TIEMPO1_TextChanged(object sender, EventArgs e, int opcion)
        /// <summary>
        /// Metodo que actualiza los datos introducidos en un textbox pra hacer el calculo correspondiente.
        /// </summary>
        /// <param name="TBOX1"></param>
        /// <param name="TBOX2"></param>
        /// <param name="parametro1"></param>
        /// <param name="parametro2"></param>
        /// <param name="opcion"></param>
        public static void TIEMPO_TextChanged(TextBox TBOX1, TextBox TBOX2, double parametro1, double parametro2, int opcion)
        {
            //parametro1: puede ser lambda o beta
            //parametro2: es eta
            //TextBox tb = sender as TextBox;
            //Debug.Assert(tb != null, "sender no es de tipo textBox");

            if (TBOX1 != null)
            {
                double valor;
                double Funcion;
                try
                {
                    if (TBOX1.Text != "")
                    {
                        TBOX2.Enabled = true;
                        valor = Convert.ToDouble(TBOX1.Text);//dato introducido para el calculo
                        switch (opcion)
                        {
                            case 1://Funcion de Confiabilidad 
                                Funcion = Math.Exp((-1) * Math.Pow((valor / parametro2), parametro1));
                                TBOX2.Text = Funcion.ToString("0.#########");
                                break;
                            case 2://Funcion de Tiempo para confiabilidad
                                Funcion = parametro2 * Math.Pow(((Math.Log(1 / valor))), (1 / parametro1));
                                TBOX2.Text = Funcion.ToString("0.#########");
                                break;
                            case 3://Funcion de la Tasa de Fallos
                                Funcion = (parametro1 / parametro2) * Math.Pow(valor / parametro2, parametro1 - 1);
                                TBOX2.Text = Funcion.ToString("0.#########");
                                break;
                            case 4://Funcion de Tiempo para la Tasa de fallos
                                Funcion = parametro2 * Math.Pow(((parametro2 * valor) / parametro1), (1 / (parametro1 - 1)));
                                TBOX2.Text = Funcion.ToString("0.#########");
                                break;
                        }
                    }
                    else if (TBOX1.Text == "") TBOX2.Text = "";
                }
                catch
                {
                    TBOX2.Text = "";
                }
            }
        }
        #endregion

    }

}

