using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIM
{
    class Distribuciones
    {
        public static double CDF_Exponencial(double parametro1, double tiempo)
        {
            double valor_funcion_exponencial = 0;
            valor_funcion_exponencial = Math.Exp(parametro1 * tiempo);

            return valor_funcion_exponencial;
        }

    }
}
