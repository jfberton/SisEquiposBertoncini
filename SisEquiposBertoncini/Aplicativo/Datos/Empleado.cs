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
    
    public partial class Empleado
    {
        public Empleado()
        {
            this.Dias = new HashSet<Dia>();
        }
    
        public int id_empleado { get; set; }
        public string nombre { get; set; }
        public int id_categoria { get; set; }
        public int id_area { get; set; }
        public string dni { get; set; }
        public System.DateTime fecha_nacimiento { get; set; }
        public Nullable<System.DateTime> fecha_baja { get; set; }
    
        public virtual Categoria_empleado Categoria { get; set; }
        public virtual ICollection<Dia> Dias { get; set; }
        public virtual Area Area { get; set; }
    }
}
