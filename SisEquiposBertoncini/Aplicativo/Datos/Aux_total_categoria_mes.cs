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
    
    public partial class Aux_total_categoria_mes
    {
        public Aux_total_categoria_mes()
        {
            this.Valores_mes = new HashSet<Valor_mes_categoria>();
        }
    
        public int id_aux_total_categoria_mes { get; set; }
        public int id_categoria_equipo { get; set; }
        public int mes { get; set; }
        public int anio { get; set; }
    
        public virtual ICollection<Valor_mes_categoria> Valores_mes { get; set; }
    }
}
