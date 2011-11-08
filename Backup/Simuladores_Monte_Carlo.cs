using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIM
{
    class Simuladores_Monte_Carlo
    {


        //public List<PointF> Parejas_Datos;
        
        
        public static double Simulador1_Disponibilidad(double Tiempo_A_Simular, string ley_func, double ley_func_param1, double ley_func_param2, double MinimoFuncionando,
                             double MaximoFuncionando, string ley_paro, double ley_paro_param1, double ley_paro_param2, double MinimoParado, double MaximoParado, Random r)
        {
            double TiempoFuncionandoAcumulado = 0;
            double TiempoParadoAcumulado = 0;
            double Disponibilidad;
            double t = 0;
            
            //BUCLE QUE REALIZA CADA SIMULACIÓN
            do
            {
                
                //Generar tiempo funcionando y acumularlo
                if (ley_func == "Uniforme") t = GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme(MinimoFuncionando, MaximoFuncionando, r);
                if (ley_func == "Exponencial") t = GeneradoresDeAleatorios.Generador_Aleatorio_Exponencial(ley_func_param1, 1/ley_func_param2, MinimoFuncionando, MaximoFuncionando, r);
                if (ley_func == "Weibull") t = GeneradoresDeAleatorios.Generador_Aleatorio_Weibull_2P(ley_func_param1, ley_func_param2, MinimoFuncionando, MaximoFuncionando, r);
                if (ley_func == "Normal") t = GeneradoresDeAleatorios.Generador_Aleatorio_Normal(ley_func_param1, ley_func_param2, MinimoFuncionando, MaximoFuncionando, r);
                TiempoFuncionandoAcumulado += t;

                //Generar tiempo parado y acumularlo
                if (ley_paro == "Uniforme") t = GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme(MinimoParado, MaximoParado, r);
                if (ley_paro == "Exponencial") t = GeneradoresDeAleatorios.Generador_Aleatorio_Exponencial(ley_paro_param1, 1/ley_paro_param2, MinimoParado, MaximoParado, r);
                if (ley_paro == "Weibull") t = GeneradoresDeAleatorios.Generador_Aleatorio_Weibull_2P(ley_paro_param1, ley_paro_param2, MinimoParado, MaximoParado, r);
                if (ley_paro == "Normal") t = GeneradoresDeAleatorios.Generador_Aleatorio_Normal(ley_paro_param1, ley_paro_param2, MinimoParado, MaximoParado, r);
                TiempoParadoAcumulado += t;

                //Calcular Disponibilidad
                Disponibilidad = TiempoFuncionandoAcumulado / (TiempoFuncionandoAcumulado + TiempoParadoAcumulado);             

            } while (TiempoFuncionandoAcumulado + TiempoParadoAcumulado <= Tiempo_A_Simular);

            return Disponibilidad;
        }


        /*
        public static double Simulador2_Disponibilidad(double Tiempo_A_Simular, string ley_func, double ley_func_param1, double ley_func_param2, double MinimoFuncionando,
                             double MaximoFuncionando, string ley_paro, double ley_paro_param1, double ley_paro_param2, double MinimoParado, double MaximoParado, ref List<PointF> Parejas_Datos, Random r)
        {
            double TiempoFuncionandoAcumulado = 0;
            double TiempoParadoAcumulado = 0;
            double Disponibilidad;
            double t = 0;
            //List<PointF> Parejas_Datos = new List<PointF>();

            //BUCLE QUE REALIZA CADA SIMULACIÓN
            do
            {
               
                Parejas_Datos.Add(new PointF((float)TiempoFuncionandoAcumulado, 0.0));
                Parejas_Datos.Add(new PointF((float)TiempoFuncionandoAcumulado, 1.0));

                //Generar tiempo funcionando y acumularlo
                if (ley_func == "Uniforme") t = GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme(MinimoFuncionando, MaximoFuncionando, r);
                if (ley_func == "Exponencial") t = GeneradoresDeAleatorios.Generador_Aleatorio_Exponencial(ley_func_param1, ley_func_param2, MinimoFuncionando, MaximoFuncionando, r);
                if (ley_func == "Weibull") t = GeneradoresDeAleatorios.Generador_Aleatorio_Weibull_2P(ley_func_param1, ley_func_param2, MinimoFuncionando, MaximoFuncionando, r);
                if (ley_func == "Normal") t = GeneradoresDeAleatorios.Generador_Aleatorio_Normal(ley_func_param1, ley_func_param2, MinimoFuncionando, MaximoFuncionando, r);
                TiempoFuncionandoAcumulado += t;

                //Añadir datos para representar el perfil de funcionamiento-fallo
                Parejas_Datos.Add(new PointF((float)TiempoFuncionandoAcumulado, 1.0));
                Parejas_Datos.Add(new PointF((float)TiempoFuncionandoAcumulado, 0.0));

                //Generar tiempo parado y acumularlo
                if (ley_paro == "Uniforme") t = GeneradoresDeAleatorios.Generador_Aleatorio_Uniforme(MinimoParado, MaximoParado, r);
                if (ley_paro == "Exponencial") t = GeneradoresDeAleatorios.Generador_Aleatorio_Exponencial(ley_paro_param1, ley_paro_param2, MinimoParado, MaximoParado, r);
                if (ley_paro == "Weibull") t = GeneradoresDeAleatorios.Generador_Aleatorio_Weibull_2P(ley_paro_param1, ley_paro_param2, MinimoParado, MaximoParado, r);
                if (ley_paro == "Normal") t = GeneradoresDeAleatorios.Generador_Aleatorio_Normal(ley_paro_param1, ley_paro_param2, MinimoParado, MaximoParado, r);
                TiempoParadoAcumulado += t;

                Parejas_Datos.Add(new PointF((float)(TiempoFuncionandoAcumulado, 0.0));

                //Calcular Disponibilidad
                Disponibilidad = TiempoFuncionandoAcumulado / (TiempoFuncionandoAcumulado + TiempoParadoAcumulado);

            } while (TiempoFuncionandoAcumulado + TiempoParadoAcumulado <= Tiempo_A_Simular);

            return Disponibilidad;
        }*/


        







    }
}
