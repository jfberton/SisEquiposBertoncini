//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SisEquiposBertoncini.Aplicativo.Datos
{
    using System;
    using System.Collections.Generic;
    
    public partial class Item_por_amortizar
    {
        public int id_item { get; set; }
        public int id_equipo { get; set; }
        public string nombre { get; set; }
        public decimal costo_cero_km_uss { get; set; }
        public decimal porcentaje_usado { get; set; }
        public decimal porcentaje_valor_recidual { get; set; }
        public int meses_por_amortizar { get; set; }
        public int periodo_alta_mes { get; set; }
        public int periodo_alta_anio { get; set; }
    
        public virtual Equipo Equipo { get; set; }
    }
}
