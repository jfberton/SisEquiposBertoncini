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
    
    public partial class Equipo
    {
        public Equipo()
        {
            this.Detalles_dias = new HashSet<Detalle_dia>();
            this.Items_por_amortizar = new HashSet<Item_por_amortizar>();
            this.Ingresos_egresos_mensuales = new HashSet<Ingreso_egreso_mensual_equipo>();
            this.Planilla_combustible = new HashSet<Planilla_combustible>();
        }
    
        public int id_equipo { get; set; }
        public string nombre { get; set; }
        public int id_categoria { get; set; }
        public string notas { get; set; }
        public Nullable<System.DateTime> fecha_baja { get; set; }
        public bool OUT { get; set; }
        public bool Generico { get; set; }
        public Nullable<bool> EsTrabajo { get; set; }
    
        public virtual Categoria_equipo Categoria { get; set; }
        public virtual ICollection<Detalle_dia> Detalles_dias { get; set; }
        public virtual ICollection<Item_por_amortizar> Items_por_amortizar { get; set; }
        public virtual ICollection<Ingreso_egreso_mensual_equipo> Ingresos_egresos_mensuales { get; set; }
        public virtual ICollection<Planilla_combustible> Planilla_combustible { get; set; }
    }
}
