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
    
    public partial class Detalle_valor_item_mes
    {
        public int id_detalle_valor_item_mes { get; set; }
        public int id_valor_mes { get; set; }
        public System.DateTime fecha { get; set; }
        public decimal monto { get; set; }
        public string descripcion { get; set; }
    
        public virtual Valor_mes Valor_item_mes { get; set; }
    }
}
