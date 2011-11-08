using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIM
{
    class Estadistica
    {
        /// <summary>
        /// REGRESION LINEAL SIMPLE realizada por el método de los mínimos cuadrados
        /// obtenida del libro de J.R Vizmanos.
        /// </summary>
        /// <param name="N"> Numero de parejas de datos que se reciben<\param>
        /// <param name="X">Vector que contiene la variable X de cada pareja de datos<\param >
        /// <param name="Y">Vector que contiene la variable Y de cada pareja de datos<\param> 
        /// <param name="ResulRegLin">Vector que contiene los resultados del ajuste realizado<\param> 
        public static void RegresionLineal(int N, double[] X, double[] Y, ref double[] ResulRegLin)
        {
            //NOTA: la posicion s[0] no se usa para mantener la notacion de JR Vizmanos y limitar
            //los errores de transcripcion
            double[] s = new double[6];

            double A, B, C, D;

            //Calculo de las sumas y productos básicos
            for (int i = 1; i <= N; i++)
            {
                s[1] = s[1] + X[i];
                s[2] = s[2] + Y[i];
                s[3] = s[3] + X[i] * X[i];
                s[4] = s[4] + Y[i] * Y[i];
                s[5] = s[5] + X[i] * Y[i];
            }

            //Calculo de los coeficientes de la ecuacion de regresion
            C = N * s[5] - s[2] * s[1];
            D = N * s[3] - s[1] * s[1];
            B = C / D;
            A=(s[2]-B*s[1])/N;

            //Calculo de coeficientes
            s[4] = s[4] - s[2] * s[2] / N;
            s[1] = B*(s[5]-s[1]*s[2]/N);
            s[2] = s[4] - s[1];
            s[5] = s[1] / s[4];

            //Carga del vector de resultados
            ResulRegLin[0] = Math.Sqrt(s[5]);           //posicion 0 para el Coeficiente de Correlacion
            ResulRegLin[1] = A;                         //posicion 1 para el termino independiente de la recta
            ResulRegLin[2] = B;                         //posicion 2 para el termino dependiente de la recta
            ResulRegLin[3] = s[5];                      //posicion 3 para el coeficiente de determinacion
            ResulRegLin[4] = s[2] / (N - 2);            //posicion 4 para la varianza de la estimacion
            ResulRegLin[5] = Math.Sqrt(s[2] / (N - 2)); //Posicion 5 para la desviacion típica de la estimacion

        }

    }
}
