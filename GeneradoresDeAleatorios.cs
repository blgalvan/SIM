using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIM
{
    public class GeneradoresDeAleatorios
    {
        public static double Generador_Aleatorio_Uniforme(double minimo_admisible, double maximo_admisible, Random r)
        {
            double aleatorio_uniforme;
            aleatorio_uniforme = minimo_admisible + (maximo_admisible - minimo_admisible) * r.NextDouble();

            return aleatorio_uniforme;
        }


        public static double Generador_Aleatorio_Exponencial(double Gamma, double mu, double minimo_admisible, double maximo_admisible, Random r)
        {
            //parametro1=Gamma, parametro2=Lambda
            double aleatorio_exponencial;
            do
            {
                aleatorio_exponencial = Gamma + (-(mu) * Math.Log(r.NextDouble()));
            } while (aleatorio_exponencial < minimo_admisible || aleatorio_exponencial > maximo_admisible);

            return aleatorio_exponencial;
        }

        public static double Generador_Aleatorio_Weibull_2P(double beta, double eta, double minimo_admisible, double maximo_admisible, Random r)
        {
            double aleatorio_weibull;
            do
            {
                aleatorio_weibull = eta * Math.Pow(((r.NextDouble())), 1 / beta);
            } while (aleatorio_weibull < minimo_admisible || aleatorio_weibull > maximo_admisible);

            return aleatorio_weibull;
        }


        public static double Generador_Aleatorio_Normal(double media, double desviacion_tipica, double minimo_admisible, double maximo_admisible, Random r)
        {
            //Genera un número aleatorio normal con N(media,desviacion_tipica)
            double aleatorio_normal;

            do
            {
                double Suma_Aleatorios_Uniformes = 0;

                for (int i = 1; i <= 12; i++)
                {
                    Suma_Aleatorios_Uniformes = Suma_Aleatorios_Uniformes + r.NextDouble();
                }
                aleatorio_normal = (Suma_Aleatorios_Uniformes - 6) * desviacion_tipica + media;
            } while (aleatorio_normal < minimo_admisible || aleatorio_normal > maximo_admisible);

            return aleatorio_normal;
        }


    }
}
