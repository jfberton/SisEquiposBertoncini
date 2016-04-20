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
        public string ausente { get; set; }
        public string guardia { get; set; }
        public string varios_taller { get; set; }
    
        public virtual Empleado Empleado { get; set; }
        public virtual ICollection<Detalle_dia> Detalles { get; set; }
    }
}
