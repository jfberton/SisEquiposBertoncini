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
    
    public partial class Planilla_gasto_administrativo
    {
        public Planilla_gasto_administrativo()
        {
            this.Detalle = new HashSet<Aux_planilla_gasto_administracion>();
        }
    
        public int id_planilla_gastos_administartivos { get; set; }
        public int mes { get; set; }
        public int anio { get; set; }
        public decimal monto_telefonia_celular { get; set; }
        public decimal monto_sueldos { get; set; }
        public decimal monto_honorarios_sistema { get; set; }
        public decimal monto_honorarios_contables { get; set; }
        public decimal monto_papeleria { get; set; }
        public decimal monto_otros { get; set; }
    
        public virtual ICollection<Aux_planilla_gasto_administracion> Detalle { get; set; }
    }
}