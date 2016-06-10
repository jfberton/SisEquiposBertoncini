using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SisEquiposBertoncini.Aplicativo
{
    public static class @string
    {
        public static string RemoverAcentos(this string s)
        {
            Encoding destEncoding = Encoding.GetEncoding("iso-8859-8");

            return destEncoding.GetString(
             Encoding.Convert(Encoding.UTF8, destEncoding, Encoding.UTF8.GetBytes(s)));
        }
    }
}