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
    
    public partial class Dia
    {
        public Dia()
        {
            this.Detalles = new HashSet<Detalle_dia>();
        }
    
        public int id_dia { get; set; }
        public System.DateTime fecha { get; set; }
        public int id_empleado { get; set; }
        public string horas_normales { get; set; }
        public string horas_extra_50 { get; set; }
        public string horas_extra_100 { get; set; }
        public Estado_turno_dia estado_tm { get; set; }
        public Estado_turno_dia estado_tt { get; set; }
    
        public virtual Empleado Empleado { get; set; }
        public virtual ICollection<Detalle_dia> Detalles { get; set; }
    }
}
