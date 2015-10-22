using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SisEquiposBertoncini.Aplicativo.Datos
{
    public partial class Item_por_amortizar
    {
        public decimal valor_por_amortizar
        {
            get
            {
                decimal d = 0;

                d = costo_cero_km_uss - (costo_cero_km_uss * (1 - (porcentaje_usado/100))) - ((costo_cero_km_uss - (costo_cero_km_uss * (1 - (porcentaje_usado/100)))) * (porcentaje_valor_recidual/100));
                    
                return decimal.Round(d,2);
            }
        }

        public decimal costo_mensual
        {
            get
            {
                decimal d = 0;

                d = meses_por_amortizar > 0 ? valor_por_amortizar / meses_por_amortizar : 0;

                return decimal.Round(d, 2);
            }
        }

        public int Periodos_amortizados(int mes, int anio)
        {
            int ret = 0;

            int anio_cursor = this.periodo_alta_anio;
            int mes_cursor = this.periodo_alta_mes;

            while (anio>= anio_cursor)
            {
                while ((mes_cursor <= 12 && anio != anio_cursor) || (mes_cursor <= mes && anio == anio_cursor))
                {
                    ret++;
                    mes_cursor++;
                }
                mes_cursor = 1;
                anio_cursor++;
            }

            return ret;
        }

        public int Restan_por_amortizar(int mes, int anio)
        {
            int ret = 0;

            ret = this.meses_por_amortizar - Periodos_amortizados(mes, anio);

            if (ret < 0) ret = 0;

            return ret;
        }
    }
}