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
    
    public partial class Categoria_equipo
    {
        public Categoria_equipo()
        {
            this.Equipos = new HashSet<Equipo>();
        }
    
        public int id_categoria { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
    
        public virtual ICollection<Equipo> Equipos { get; set; }
    }
}
