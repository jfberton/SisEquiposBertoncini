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
    
    public partial class Detalle_valor_item_mes_categoria
    {
        public int id_detalle_valor_item_mes_categoria { get; set; }
        public System.DateTime fecha { get; set; }
        public decimal monto { get; set; }
        public string descripcion { get; set; }
        public int id_valor_mes_categoria_item { get; set; }
    
        public virtual Valor_mes_categoria Valor_mes_categoria { get; set; }
    }
}
