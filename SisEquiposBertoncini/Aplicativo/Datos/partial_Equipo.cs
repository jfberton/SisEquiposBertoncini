using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SisEquiposBertoncini.Aplicativo.Datos
{
    public partial class Equipo
    {
        /// <summary>
        /// Devuelve la suma de los costos 0km de las partes que componen el equipo
        /// </summary>
        public decimal Costo_total_0km
        {
            get
            {
                decimal ret = 0;

                foreach (Item_por_amortizar parte in this.Items_por_amortizar)
                {
                    ret = ret + parte.costo_cero_km_uss;
                }

                return ret;
            }
        }

        /// <summary>
        /// Devuelve el monto total por amortizar de las partes que componen el equipo que todavia tienen meses por amortizar
        /// </summary>
        public decimal Total_por_amortizar
        { 
            get
            {
                decimal ret = 0;

                foreach (Item_por_amortizar parte in this.Items_por_amortizar)
                {
                    if (parte.Restan_por_amortizar(DateTime.Today.Month, DateTime.Today.Year) > 0)
                    {
                        ret = ret + parte.valor_por_amortizar;
                    }
                }

                return ret;
            }
        }

        /// <summary>
        /// Devuelve el monto total mensual por amortizar de las partes que componen el equipo que todavia tienen meses por amortizar
        /// </summary>
        public decimal Costo_amortizacion_mensual
        {
            get
            {
                decimal ret = 0;

                foreach (Item_por_amortizar parte in this.Items_por_amortizar)
                {
                    if (parte.Restan_por_amortizar(DateTime.Today.Month, DateTime.Today.Year) > 0)
                    {
                        ret = ret + parte.costo_mensual;
                    }
                }

                return ret;
            }
        }
    }
}