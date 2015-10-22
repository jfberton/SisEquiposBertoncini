using SisEquiposBertoncini.Aplicativo.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SisEquiposBertoncini.Aplicativo.Controles
{
    public static class ValorDolarMes
    {
        public static decimal Obtener(int mes, int anio)
        {
            decimal ret = 0;
            
            using (var cxt = new Model1Container())
            {
                Valor_dolar valor_dolar_mes = cxt.Valores_dolar.FirstOrDefault(x => x.mes == mes && x.anio == anio);
                if (valor_dolar_mes != null)
                {
                    ret = valor_dolar_mes.valor;
                }
            }

            return ret;
        }
    }
}