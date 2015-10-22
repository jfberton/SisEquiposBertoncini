using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SisEquiposBertoncini.Aplicativo.Controles
{
    public static class Cadena
    {
        public enum Moneda
        {
            pesos,
            dolares,
            unidades_bertoncini
        }

        public static string Formato_moneda(decimal valor, Moneda formato)
        {
            string ret = valor.ToString("n2");

            switch (formato)
            {
                case Moneda.pesos:
                    ret = "$ " + ret;
                    break;
                case Moneda.dolares:
                    ret = "USS " + ret;
                    break;
                case Moneda.unidades_bertoncini:
                    ret = "UB " + ret;
                    break;
                default:
                    break;
            }

            return ret;
        }

        public static string Formato_porcentaje(decimal valor)
        {
            string ret = decimal.Round(valor,2).ToString("n2");

            ret = ret + " %";

            return ret;
        }
    }
}