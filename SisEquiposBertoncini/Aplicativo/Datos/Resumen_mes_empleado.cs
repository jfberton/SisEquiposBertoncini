//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SisEquiposBertoncini.Aplicativo.Datos
{
    using System;
    using System.Collections.Generic;
    
    public partial class Resumen_mes_empleado
    {
        public int id_resumen_mes { get; set; }
        public int id_empleado { get; set; }
        public int mes { get; set; }
        public int anio { get; set; }
        public decimal dias_laborables { get; set; }
        public decimal dias_ausente { get; set; }
        public decimal dias_presente { get; set; }
        public decimal dias_por_cargar { get; set; }
        public decimal dias_out { get; set; }
        public decimal dias_presentes_en_dias_no_laborables { get; set; }
        public string total_horas_normales { get; set; }
        public string total_horas_extra_50 { get; set; }
        public string total_horas_extra_100 { get; set; }
    
        public virtual Empleado Empleado { get; set; }
    }
}