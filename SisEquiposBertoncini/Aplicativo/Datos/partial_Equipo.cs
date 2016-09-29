using SisEquiposBertoncini.Aplicativo.Controles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SisEquiposBertoncini.Aplicativo.Datos
{
    public partial class Equipo
    {
        /// <summary>
        /// Devuelve la suma de los costos 0km de las partes que componen el equipo
        /// </summary>
        public decimal Costo_total_0km
        {
            get
            {
                decimal ret = 0;

                foreach (Item_por_amortizar parte in this.Items_por_amortizar)
                {
                    ret = ret + parte.costo_cero_km_uss;
                }

                return ret;
            }
        }

        /// <summary>
        /// Devuelve el monto total por amortizar de las partes que componen el equipo que todavia tienen meses por amortizar
        /// </summary>
        public decimal Total_por_amortizar
        {
            get
            {
                decimal ret = 0;

                foreach (Item_por_amortizar parte in this.Items_por_amortizar)
                {
                    if (parte.Restan_por_amortizar(DateTime.Today.Month, DateTime.Today.Year) > 0)
                    {
                        ret = ret + parte.valor_por_amortizar;
                    }
                }

                return ret;
            }
        }

        /// <summary>
        /// Devuelve el monto total mensual por amortizar de las partes que componen el equipo que todavia tienen meses por amortizar
        /// </summary>
        public decimal Costo_amortizacion_mensual()
        {
            decimal ret = 0;

            foreach (Item_por_amortizar parte in this.Items_por_amortizar)
            {
                if (parte.Restan_por_amortizar(DateTime.Today.Month, DateTime.Today.Year) > 0)
                {
                    ret = ret + parte.costo_mensual;
                }
            }

            return ret;
        }

        public decimal Costo_amortizacion_mensual(int mes, int anio)
        {
            decimal ret = 0;

            foreach (Item_por_amortizar parte in this.Items_por_amortizar)
            {
                if (parte.Restan_por_amortizar(mes, anio) > 0)
                {
                    ret = ret + parte.costo_mensual;
                }
            }

            return ret;
        }

        public string Horas_mes(int mes, int anio, string categoria_empleado)
        {
            string ret = "00:00";
            if (categoria_empleado == "Mecánicos")
            {
                foreach (Detalle_dia item in Detalles_dias.Where(x => x.Dia.fecha.Month == mes && x.Dia.fecha.Year == anio && (x.Dia.Empleado.Categoria.nombre == "Mecánico" || x.Dia.Empleado.Categoria.nombre == "Pintor")))
                {
                    ret = Horas_string.SumarHoras(new string[] { ret, item.total_movimiento });
                }
            }
            else
            {
                foreach (Detalle_dia item in Detalles_dias.Where(x => x.Dia.fecha.Month == mes && x.Dia.fecha.Year == anio && x.Dia.Empleado.Categoria.nombre == "Soldador"))
                {
                    ret = Horas_string.SumarHoras(new string[] { ret, item.total_movimiento });
                }
            }


            return ret;
        }

        public string Horas_mes(int mes, int anio, List<Empleado> empleados)
        {
            string ret = "00:00";

            foreach (Detalle_dia item in Detalles_dias.Where(x => x.Dia.fecha.Month == mes && x.Dia.fecha.Year == anio))
            {
                if (ExisteEnElListado(item.Dia.Empleado, empleados))
                {
                    ret = Horas_string.SumarHoras(new string[] { ret, item.total_movimiento });
                }
            }

            return ret;
        }

        private bool ExisteEnElListado(Empleado empleado, List<Empleado> empleados)
        {
            return empleados.FirstOrDefault(ee => ee.id_empleado == empleado.id_empleado) != null;
        }


        public enum Valor_mensual
        {
            Mano_obra,
            Insumos,
            Herramientas,
            Viaticos,
            ViaticosPP,
            Indumentaria,
            Repuestos,
            Repuestos_pp,
            Gastos_varios,
            Otros,
            admin_telefonia,
            admin_varios
        }



        public enum Tipo_empleado
        {
            Mecanicos_pintores,
            Soldadores,
            Grueros
        }

        public void Agregar_detalle_en_valor_mensual_segun_empleado(Tipo_empleado tipo, Valor_mensual item_valor, int mes, int anio, decimal monto)
        {
            using (var cxt = new Model1Container())
            {
                Ingreso_egreso_mensual_equipo iemensual = Ingresos_egresos_mensuales.FirstOrDefault(x => x.anio == anio && x.mes == mes);

                if (iemensual == null)
                {
                    iemensual = new Ingreso_egreso_mensual_equipo();
                    iemensual.id_equipo = this.id_equipo;
                    iemensual.mes = mes;
                    iemensual.anio = anio;
                    cxt.Ingresos_egresos_mensuales_equipos.Add(iemensual);
                    cxt.SaveChanges();
                }

                string nombre_item = ObtenerNombreItem_segun_empleado(tipo, item_valor);

                if (nombre_item != "")
                {
                    Item_ingreso_egreso item = cxt.Items_ingresos_egresos.FirstOrDefault(x => x.nombre == nombre_item);
                    if (item != null)
                    {
                        Valor_mes valor_mes = iemensual.Valores_meses.FirstOrDefault(x => x.id_item == item.id_item);
                        if (valor_mes == null)
                        {
                            valor_mes = new Valor_mes();
                            valor_mes.id_ingreso_egreso_mensual = iemensual.id_ingreso_egreso_mensual;
                            valor_mes.id_item = item.id_item;
                            valor_mes.valor = 0;
                            cxt.Valores_meses.Add(valor_mes);
                            cxt.SaveChanges();
                        }

                        //aca tengo el item valor mes del 
                        string descripcion_detalle = "";

                        switch (tipo)
                        {
                            case Tipo_empleado.Mecanicos_pintores:
                                switch (item_valor)
                                {
                                    case Valor_mensual.Mano_obra:
                                        descripcion_detalle = "Gastos obtenidos de planilla de cálculos Mecánicos-Pintores";
                                        break;
                                    case Valor_mensual.Insumos:
                                        descripcion_detalle = "Gastos obtenidos de planilla de gastos en función horas hombre Mecánicos-Pintores";
                                        break;
                                    case Valor_mensual.Herramientas:
                                        descripcion_detalle = "Gastos obtenidos de planilla de gastos en función horas hombre Mecánicos-Pintores";
                                        break;
                                    case Valor_mensual.Viaticos:
                                        descripcion_detalle = "Gastos obtenidos de planilla de gastos en función horas hombre Mecánicos-Pintores";
                                        break;
                                    case Valor_mensual.ViaticosPP:
                                        descripcion_detalle = "Gastos obtenidos de planilla de gastos en función horas hombre Mecánicos-Pintores";
                                        break;
                                    case Valor_mensual.Indumentaria:
                                        descripcion_detalle = "Gastos obtenidos de planilla de gastos en función horas hombre Mecánicos-Pintores";
                                        break;
                                    case Valor_mensual.Repuestos:
                                        descripcion_detalle = "Gastos obtenidos de planilla de gastos en función horas hombre Mecánicos-Pintores";
                                        break;
                                    case Valor_mensual.Repuestos_pp:
                                        descripcion_detalle = "Gastos obtenidos de planilla de gastos en función horas hombre Mecánicos-Pintores";
                                        break;
                                    case Valor_mensual.Gastos_varios:
                                        descripcion_detalle = "Gastos obtenidos de planilla de gastos en función horas hombre Mecánicos-Pintores";
                                        break;
                                    case Valor_mensual.Otros:
                                        descripcion_detalle = "Gastos obtenidos de planilla de gastos en función horas hombre Mecánicos-Pintores";
                                        break;

                                    default:
                                        break;
                                }
                                break;
                            case Tipo_empleado.Soldadores:
                                switch (item_valor)
                                {
                                    case Valor_mensual.Mano_obra:
                                        descripcion_detalle = "Gastos obtenidos de planilla de cálculos Soldadores";
                                        break;
                                    case Valor_mensual.Insumos:
                                        descripcion_detalle = "Gastos obtenidos de planilla de gastos en función horas hombre Soldadores";
                                        break;
                                    case Valor_mensual.Herramientas:
                                        descripcion_detalle = "Gastos obtenidos de planilla de gastos en función horas hombre Soldadores";
                                        break;
                                    case Valor_mensual.Viaticos:
                                        descripcion_detalle = "Gastos obtenidos de planilla de gastos en función horas hombre Soldadores";
                                        break;
                                    case Valor_mensual.ViaticosPP:
                                        descripcion_detalle = "Gastos obtenidos de planilla de gastos en función horas hombre Soldadores";
                                        break;
                                    case Valor_mensual.Indumentaria:
                                        descripcion_detalle = "Gastos obtenidos de planilla de gastos en función horas hombre Soldadores";
                                        break;
                                    case Valor_mensual.Repuestos:
                                        descripcion_detalle = "Gastos obtenidos de planilla de gastos en función horas hombre Soldadores";
                                        break;
                                    case Valor_mensual.Repuestos_pp:
                                        descripcion_detalle = "Gastos obtenidos de planilla de gastos en función horas hombre Soldadores";
                                        break;
                                    case Valor_mensual.Gastos_varios:
                                        descripcion_detalle = "Gastos obtenidos de planilla de gastos en función horas hombre Soldadores";
                                        break;
                                    case Valor_mensual.Otros:
                                        descripcion_detalle = "Gastos obtenidos de planilla de gastos en función horas hombre Soldadores";
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case Tipo_empleado.Grueros:
                                switch (item_valor)
                                {
                                    case Valor_mensual.Mano_obra:
                                        descripcion_detalle = "Gastos obtenidos de planilla de cálculos Grueros";
                                        break;
                                    case Valor_mensual.Insumos:
                                        descripcion_detalle = "Gastos obtenidos de planilla de gastos en función horas hombre Grueros";
                                        break;
                                    case Valor_mensual.Herramientas:
                                        descripcion_detalle = "Gastos obtenidos de planilla de gastos en función horas hombre Grueros";
                                        break;
                                    case Valor_mensual.Viaticos:
                                        descripcion_detalle = "Gastos obtenidos de planilla de gastos en función horas hombre Grueros";
                                        break;
                                    case Valor_mensual.ViaticosPP:
                                        descripcion_detalle = "Gastos obtenidos de planilla de gastos en función horas hombre Grueros";
                                        break;
                                    case Valor_mensual.Indumentaria:
                                        descripcion_detalle = "Gastos obtenidos de planilla de gastos en función horas hombre Grueros";
                                        break;
                                    case Valor_mensual.Repuestos:
                                        descripcion_detalle = "Gastos obtenidos de planilla de gastos en función horas hombre Grueros";
                                        break;
                                    case Valor_mensual.Repuestos_pp:
                                        descripcion_detalle = "Gastos obtenidos de planilla de gastos en función horas hombre Grueros";
                                        break;
                                    case Valor_mensual.Gastos_varios:
                                        descripcion_detalle = "Gastos obtenidos de planilla de gastos en función horas hombre Grueros";
                                        break;
                                    case Valor_mensual.Otros:
                                        descripcion_detalle = "Gastos obtenidos de planilla de gastos en función horas hombre Grueros";
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            default:
                                break;
                        }

                        Detalle_valor_item_mes detalle = valor_mes.Detalle.FirstOrDefault(x => x.descripcion == descripcion_detalle);

                        if (detalle == null)
                        {
                            detalle = new Detalle_valor_item_mes();
                            detalle.id_valor_mes = valor_mes.id;
                            detalle.fecha = new DateTime(anio, mes, DateTime.DaysInMonth(anio, mes));
                            detalle.descripcion = descripcion_detalle;
                            detalle.monto = monto;
                            cxt.Detalle_valores_items_mes.Add(detalle);
                        }
                        else
                        {
                            detalle = cxt.Detalle_valores_items_mes.First(x => x.id_detalle_valor_item_mes == detalle.id_detalle_valor_item_mes);
                            detalle.monto = monto;
                        }

                        cxt.SaveChanges();
                    }
                }

            }
        }

        private string ObtenerNombreItem_segun_empleado(Tipo_empleado tipo, Valor_mensual item_valor)
        {
            string ret = "";

            switch (tipo)
            {
                case Tipo_empleado.Mecanicos_pintores:
                    switch (item_valor)
                    {
                        case Valor_mensual.Mano_obra:
                            ret = "Mano Obra Taller";
                            break;
                        case Valor_mensual.Insumos:
                            ret = "Insumos Taller";
                            break;
                        case Valor_mensual.Herramientas:
                            ret = "Herramientas Taller";
                            break;
                        case Valor_mensual.Viaticos:
                            ret = "Viaticos Taller";
                            break;
                        case Valor_mensual.ViaticosPP:
                            ret = "Viaticos Presup Taller";
                            break;
                        case Valor_mensual.Indumentaria:
                            ret = "Indumentaria p personal(incluye elem de protección personal)";
                            break;
                        case Valor_mensual.Repuestos:
                            ret = "Repuestos Taller";
                            break;
                        case Valor_mensual.Repuestos_pp:
                            ret = "Repuestos PP Taller";
                            break;
                        case Valor_mensual.Gastos_varios:
                            ret = "Gastos varios Taller";
                            break;
                        case Valor_mensual.Otros:
                            ret = "Otros";
                            break;
                        default:
                            break;
                    }
                    break;
                case Tipo_empleado.Soldadores:
                    switch (item_valor)
                    {
                        case Valor_mensual.Mano_obra:
                            ret = "Mano Obra Soldadores";
                            break;
                        case Valor_mensual.Insumos:
                            ret = "Insumos Soldadores";
                            break;
                        case Valor_mensual.Herramientas:
                            ret = "Herramientas Soldadores";
                            break;
                        case Valor_mensual.Viaticos:
                            ret = "Viaticos Soldadores";
                            break;
                        case Valor_mensual.ViaticosPP:
                            ret = "Viaticos Presup Soldadores";
                            break;
                        case Valor_mensual.Indumentaria:
                            ret = "Indumentaria p personal(incluye elem de protección personal)";
                            break;
                        case Valor_mensual.Repuestos:
                            ret = "Repuestos Soldadores";
                            break;
                        case Valor_mensual.Repuestos_pp:
                            ret = "Repuestos PP Soldadores";
                            break;
                        case Valor_mensual.Gastos_varios:
                            ret = "Gastos varios Soldadores";
                            break;
                        case Valor_mensual.Otros:
                            ret = "Otros";
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            return ret;
        }

        public void Agregar_detalle(Valor_mensual item_valor, int mes, int anio, decimal monto)
        {
            using (var cxt = new Model1Container())
            {
                Ingreso_egreso_mensual_equipo iemensual = Ingresos_egresos_mensuales.FirstOrDefault(x => x.anio == anio && x.mes == mes);

                if (iemensual == null)
                {
                    iemensual = new Ingreso_egreso_mensual_equipo();
                    iemensual.id_equipo = this.id_equipo;
                    iemensual.mes = mes;
                    iemensual.anio = anio;
                    cxt.Ingresos_egresos_mensuales_equipos.Add(iemensual);
                    cxt.SaveChanges();
                }

                string nombre_item = ObtenerNombreItem(item_valor);

                if (nombre_item != "")
                {
                    Item_ingreso_egreso item = cxt.Items_ingresos_egresos.FirstOrDefault(x => x.nombre == nombre_item);
                    if (item != null)
                    {
                        Valor_mes valor_mes = iemensual.Valores_meses.FirstOrDefault(x => x.id_item == item.id_item);
                        if (valor_mes == null)
                        {
                            valor_mes = new Valor_mes();
                            valor_mes.id_ingreso_egreso_mensual = iemensual.id_ingreso_egreso_mensual;
                            valor_mes.id_item = item.id_item;
                            valor_mes.valor = 0;
                            cxt.Valores_meses.Add(valor_mes);
                            cxt.SaveChanges();
                        }

                        //aca tengo el item valor mes del 
                        string descripcion_detalle = "Gastos obtenidos de planilla de gastos administrativos";

                        Detalle_valor_item_mes detalle = valor_mes.Detalle.FirstOrDefault(x => x.descripcion == descripcion_detalle);

                        if (detalle == null)
                        {
                            detalle = new Detalle_valor_item_mes();
                            detalle.id_valor_mes = valor_mes.id;
                            detalle.fecha = new DateTime(anio, mes, DateTime.DaysInMonth(anio, mes));
                            detalle.descripcion = descripcion_detalle;
                            detalle.monto = monto;
                            cxt.Detalle_valores_items_mes.Add(detalle);
                        }
                        else
                        {
                            detalle = cxt.Detalle_valores_items_mes.First(x => x.id_detalle_valor_item_mes == detalle.id_detalle_valor_item_mes);
                            detalle.monto = monto;
                        }

                        cxt.SaveChanges();
                    }
                }
            }
        }

        private string ObtenerNombreItem(Valor_mensual item_valor)
        {
            string ret = "";

            switch (item_valor)
            {
                case Valor_mensual.admin_varios:
                    ret = "Gastos Administración";
                    break;
                case Valor_mensual.admin_telefonia:
                    ret = "Teléfono celular (abono)";
                    break;

                default:
                    break;
            }

            return ret;
        }

        public enum Accion_agregar_detalle
        {
            Agrego, 
            No_se_agrego_porque_ya_xiste
        }
        public Accion_agregar_detalle Agregar_detalle(DateTime dia, decimal monto, string item_informado, string descripcion)
        {
            Accion_agregar_detalle accion = Accion_agregar_detalle.Agrego;
            using (var cxt = new Model1Container())
            {
                int mes = dia.Month;
                int anio = dia.Year;

                Ingreso_egreso_mensual_equipo iemensual = Ingresos_egresos_mensuales.FirstOrDefault(x => x.anio == anio && x.mes == mes);

                if (iemensual == null)
                {
                    iemensual = new Ingreso_egreso_mensual_equipo();
                    iemensual.id_equipo = this.id_equipo;
                    iemensual.mes = mes;
                    iemensual.anio = anio;
                    cxt.Ingresos_egresos_mensuales_equipos.Add(iemensual);
                    cxt.SaveChanges();
                }

                
                Item_ingreso_egreso item = cxt.Items_ingresos_egresos.FirstOrDefault(x => x.nombre == item_informado);

                if (item != null)
                {
                    Valor_mes valor_mes = iemensual.Valores_meses.FirstOrDefault(x => x.id_item == item.id_item);
                    if (valor_mes == null)
                    {
                        valor_mes = new Valor_mes();
                        valor_mes.id_ingreso_egreso_mensual = iemensual.id_ingreso_egreso_mensual;
                        valor_mes.id_item = item.id_item;
                        valor_mes.valor = 0;
                        cxt.Valores_meses.Add(valor_mes);
                        cxt.SaveChanges();
                    }

                    //aca tengo el item valor mes del 
                    string descripcion_detalle = "[Importado] " + descripcion;

                    Detalle_valor_item_mes detalle = valor_mes.Detalle.FirstOrDefault(x => x.descripcion == descripcion_detalle && x.fecha == dia && x.monto == monto);

                    if (detalle == null)
                    {
                        detalle = new Detalle_valor_item_mes();
                        detalle.id_valor_mes = valor_mes.id;
                        detalle.fecha = dia;
                        detalle.descripcion = descripcion_detalle;
                        detalle.monto = monto;
                        cxt.Detalle_valores_items_mes.Add(detalle);
                        cxt.SaveChanges();
                    }
                    else
                    {
                        accion = Accion_agregar_detalle.No_se_agrego_porque_ya_xiste;
                    }
                }
            }

            return accion;
        }
    }
}