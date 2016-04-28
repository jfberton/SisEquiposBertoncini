﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class Model1Container : DbContext
    {
        public Model1Container()
            : base("name=Model1Container")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<Categoria_equipo> Categorias_equipos { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Categoria_empleado> Categorias_empleados { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Dia> Dias { get; set; }
        public DbSet<Detalle_dia> Detalles_dias { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Item_por_amortizar> Items_por_amortizar { get; set; }
        public DbSet<Valor_dolar> Valores_dolar { get; set; }
        public DbSet<Ingreso_egreso_mensual_equipo> Ingresos_egresos_mensuales_equipos { get; set; }
        public DbSet<Item_ingreso_egreso> Items_ingresos_egresos { get; set; }
        public DbSet<Valor_mes> Valores_meses { get; set; }
        public DbSet<Feriado> Feriados { get; set; }
        public DbSet<Resumen_mes_empleado> Resumenes_meses_empleados { get; set; }
        public DbSet<Detalle_valor_item_mes> Detalle_valores_items_mes { get; set; }
        public DbSet<Aux_planilla_calculo> Aux_planilla_calculos { get; set; }
        public DbSet<Aux_planilla_gastos_horas_hombre> Aux_planilla_gastos_horas_hombres { get; set; }
        public DbSet<Aux_planilla_gasto_administracion> Aux_planilla_gastos_administracion { get; set; }
        public DbSet<Planilla_gasto_administrativo> Planilla_gastos_administrativo { get; set; }
        public DbSet<Aux_total_categoria_mes> Aux_total_categoria_meses { get; set; }
        public DbSet<Valor_mes_categoria> Valor_mes_categorias { get; set; }
        public DbSet<Detalle_valor_item_mes_categoria> Detalle_valor_item_meses_categoria { get; set; }
        public DbSet<temp_table_filas_items_mes_equipo> temp_table_filas_items_mes_equipo { get; set; }
    
        public virtual int Obtener_listado_items_ingreso_egreso_mensual(Nullable<int> mes, Nullable<int> anio, Nullable<int> id_equipo)
        {
            var mesParameter = mes.HasValue ?
                new ObjectParameter("mes", mes) :
                new ObjectParameter("mes", typeof(int));
    
            var anioParameter = anio.HasValue ?
                new ObjectParameter("anio", anio) :
                new ObjectParameter("anio", typeof(int));
    
            var id_equipoParameter = id_equipo.HasValue ?
                new ObjectParameter("id_equipo", id_equipo) :
                new ObjectParameter("id_equipo", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Obtener_listado_items_ingreso_egreso_mensual", mesParameter, anioParameter, id_equipoParameter);
        }
    }
}
