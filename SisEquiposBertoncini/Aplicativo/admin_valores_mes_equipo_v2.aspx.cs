﻿using SisEquiposBertoncini.Aplicativo.Controles;
using SisEquiposBertoncini.Aplicativo.Datos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisEquiposBertoncini.Aplicativo
{
    public partial class admin_valores_mes_equipo_v2 : System.Web.UI.Page
    {
        private struct fila_item_ingreso_egreso_equipo
        {
            public int id_valor_mes { get; set; }
            public string concepto { get; set; }
            public decimal valor { get; set; }
            public string valorstr { get; set; }
            public string row_class { get; set; }
            public bool visible { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Usuario usuariologueado = Session["UsuarioLogueado"] as Usuario;
                if (usuariologueado == null)
                {
                    Response.Redirect("~/Default.aspx?mode=session_end");
                }

                switch (usuariologueado.perfil)
                {
                    case perfil_usuario.Admin:
                        menu_admin.Visible = true;
                        menu_usuario.Visible = false;
                        break;
                    case perfil_usuario.Jefe:
                        break;
                    case perfil_usuario.Supervisor:
                        break;
                    case perfil_usuario.Usuario:
                        menu_admin.Visible = false;
                        menu_usuario.Visible = true;
                        break;
                    case perfil_usuario.Seleccionar:
                        break;
                    default:
                        break;
                }

                Cargar_ddls();

                string str_id_equipo = Request.QueryString["eq"];
                string str_edita = Request.QueryString["ed"];
                string str_mes = Request.QueryString["m"];
                string str_anio = Request.QueryString["a"];

                if (str_id_equipo != null)
                {
                    ddl_equipo.SelectedValue = str_id_equipo;
                    ddl_mes.SelectedValue = str_mes;
                    ddl_anio.SelectedValue = str_anio;

                    Estado_busqueda(false);
                    using (var cxt = new Model1Container())
                    {
                        int id_equipo = Convert.ToInt32(str_id_equipo);
                        int anio = Convert.ToInt32(str_anio);
                        int mes = Convert.ToInt32(str_mes);

                        Ingreso_egreso_mensual_equipo ioEquipo = cxt.Ingresos_egresos_mensuales_equipos.FirstOrDefault(iioo => iioo.id_equipo == id_equipo && iioo.mes == mes && iioo.anio == anio);
                        CrearMostrarTabla(ioEquipo);
                    }
                }
                else
                {
                    Estado_busqueda(true);
                }
            }
        }

        private void Cargar_ddls()
        {
            using (var cxt = new Model1Container())
            {
                var equipos_habilitados = cxt.Equipos.Where(ee => ee.fecha_baja == null && !ee.Generico);
                foreach (Equipo equipo in equipos_habilitados)
                {
                    ddl_equipo.Items.Add(new ListItem() { Value = equipo.id_equipo.ToString(), Text = equipo.nombre });
                }

                for (int anio = 2015; anio <= DateTime.Today.Year; anio++)
                {
                    ddl_anio.Items.Add(new ListItem() { Value = anio.ToString(), Text = anio.ToString() });
                }
            }
        }

        private void Estado_busqueda(bool habilitado)
        {
            div_buscar_primero.Visible = habilitado;
            div_tree.Visible = !habilitado;

            ddl_equipo.Enabled = habilitado;
            ddl_anio.Enabled = habilitado;
            ddl_mes.Enabled = habilitado;

            btn_nueva_busqueda.Visible = !habilitado;
            btn_buscar.Visible = habilitado;
        }

        protected void btn_buscar_Click(object sender, EventArgs e)
        {
            int id_equipo = Convert.ToInt32(ddl_equipo.SelectedItem.Value);
            int anio = Convert.ToInt32(ddl_anio.SelectedItem.Value);
            int mes = Convert.ToInt32(ddl_mes.SelectedItem.Value);

            Response.Redirect("~/Aplicativo/admin_valores_mes_equipo_v2.aspx?eq=" + id_equipo.ToString() + "&ed=1&m=" + mes.ToString() + "&a=" + anio.ToString());
        }

        private void CrearMostrarTabla(Ingreso_egreso_mensual_equipo ioEquipo)
        {
            using (var cxt = new Model1Container())
            {
                Ingreso_egreso_mensual_equipo valores_equipo_mes_cxt;

                if (ioEquipo != null)
                {
                    valores_equipo_mes_cxt = cxt.Ingresos_egresos_mensuales_equipos.FirstOrDefault(ii => ii.id_ingreso_egreso_mensual == ioEquipo.id_ingreso_egreso_mensual);
                }
                else
                {
                    valores_equipo_mes_cxt = new Ingreso_egreso_mensual_equipo();
                    valores_equipo_mes_cxt.id_equipo = Convert.ToInt32(ddl_equipo.SelectedItem.Value);
                    valores_equipo_mes_cxt.mes = Convert.ToInt32(ddl_mes.SelectedItem.Value);
                    valores_equipo_mes_cxt.anio = Convert.ToInt32(ddl_anio.SelectedItem.Value);
                    cxt.Ingresos_egresos_mensuales_equipos.Add(valores_equipo_mes_cxt);
                    cxt.SaveChanges();
                }

                //Seteo el valor del concepto amortizacion es el costo del equipo por amortizar por el valor del dolar del mes
                int anio = Convert.ToInt32(ddl_anio.SelectedItem.Value);
                int mes = Convert.ToInt32(ddl_mes.SelectedItem.Value);
                Equipo equipo = cxt.Equipos.FirstOrDefault(ee => ee.id_equipo == valores_equipo_mes_cxt.id_equipo);
                Item_ingreso_egreso amortizacion = cxt.Items_ingresos_egresos.First(ii => ii.nombre == "Amortización" && ii.Padre.nombre == "Costos Fijos No Erogables");
                Valor_mes vm = valores_equipo_mes_cxt.Valores_meses.FirstOrDefault(ii => ii.id_item == amortizacion.id_item);

                if (vm == null)
                {
                    vm = new Valor_mes()
                    {
                        id_item = amortizacion.id_item,
                        valor = decimal.Round((equipo.Costo_amortizacion_mensual * ValorDolarMes.Obtener(mes, anio)), 2)
                    };

                    valores_equipo_mes_cxt.Valores_meses.Add(vm);

                }
                else
                {
                    vm.valor = decimal.Round((equipo.Costo_amortizacion_mensual * ValorDolarMes.Obtener(mes, anio)), 2);
                }

                cxt.SaveChanges();

                List<fila_item_ingreso_egreso_equipo> items_grilla = new List<fila_item_ingreso_egreso_equipo>();

                List<Item_ingreso_egreso> conceptos;
                conceptos = cxt.Items_ingresos_egresos.ToList();
                var roots = conceptos.Where(ii => ii.id_item_padre == null);

                foreach (Item_ingreso_egreso item in roots)
                {
                    AgregarItem(items_grilla, item, valores_equipo_mes_cxt);
                }

                Session["items_grilla"] = items_grilla;

                gv_items.DataSource = items_grilla;
                gv_items.DataBind();
            }

        }

        private void AgregarItem(List<fila_item_ingreso_egreso_equipo> items_grilla, Item_ingreso_egreso item, Ingreso_egreso_mensual_equipo ioEquipo)
        {
            using (var cxt = new Model1Container())
            {
                ioEquipo = cxt.Ingresos_egresos_mensuales_equipos.First(x => x.id_ingreso_egreso_mensual == ioEquipo.id_ingreso_egreso_mensual);
                Valor_mes valor_mes = ioEquipo.Valores_meses.FirstOrDefault(ii => ii.id_item == item.id_item);
                if (valor_mes == null)
                {
                    valor_mes = new Valor_mes()
                    {
                        id_item = item.id_item,
                        valor = 0
                    };

                    ioEquipo.Valores_meses.Add(valor_mes);
                    cxt.SaveChanges();
                }

                decimal _valor = ObtenerValor(ioEquipo, item);
                bool _visible = (item.Hijos.Count == 0 && !(item.nombre == "Impuestos" && item.Padre.nombre == "INGRESOS") && !(item.nombre == "Accesorios Hs Extra 24% (1/12x2)" && item.Padre.nombre == "Otros costos variables") && !(item.nombre == "Amortización" && item.Padre.nombre == "Costos Fijos No Erogables"));
                items_grilla.Add(new fila_item_ingreso_egreso_equipo()
                {
                    id_valor_mes = valor_mes.id,
                    concepto = item.nombre,
                    valor = _valor,
                    valorstr = _valor.ToString("$ #,##0.00"),
                    visible = _visible,
                    row_class = "treegrid-" + item.id_item + (item.id_item_padre != null ? " treegrid-parent-" + item.id_item_padre : "") + (item.id_item_padre == null ? " h4" : "") + (item.tipo == "Ingreso" ? " alert-success" : " alert-danger")
                });

                foreach (Item_ingreso_egreso hijo in item.Hijos.OrderBy(x => x.id_item))
                {
                    AgregarItem(items_grilla, hijo, ioEquipo);
                }
            }

        }

        /// <summary>
        /// Crea, actualiza y obtiene el valor del item en el mes y el equipo
        /// </summary>
        /// <param name="ioEquipo"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private decimal ObtenerValor(Ingreso_egreso_mensual_equipo ioEquipo, Item_ingreso_egreso item)
        {
            decimal ret = 0;

            using (var cxt = new Model1Container())
            {
                ioEquipo = cxt.Ingresos_egresos_mensuales_equipos.First(x => x.id_ingreso_egreso_mensual == ioEquipo.id_ingreso_egreso_mensual);

                if (item.Hijos.Count > 0)
                {
                    foreach (Item_ingreso_egreso hijo in item.Hijos)
                    {
                        ret = ret + ObtenerValor(ioEquipo, hijo);
                    }

                    Valor_mes valor_mes_item = ioEquipo.Valores_meses.FirstOrDefault(x => x.id_item == item.id_item);
                    if (valor_mes_item != null)
                    {
                        valor_mes_item.valor = ret;
                    }
                    else
                    {
                        valor_mes_item = new Valor_mes()
                        {
                            id_item = item.id_item,
                            valor = 0
                        };

                        ioEquipo.Valores_meses.Add(valor_mes_item);
                    }
                    cxt.SaveChanges();
                }
                else
                {
                    Valor_mes valor_mes_item = ioEquipo.Valores_meses.FirstOrDefault(x => x.id_item == item.id_item);
                    if (valor_mes_item != null)
                    {
                        valor_mes_item.valor = valor_mes_item.Detalle.Sum(x => x.monto);
                    }
                    else
                    {
                        valor_mes_item = new Valor_mes()
                        {
                            id_item = item.id_item,
                            valor = 0
                        };

                        ioEquipo.Valores_meses.Add(valor_mes_item);
                    }

                    cxt.SaveChanges();
                    ret = valor_mes_item.valor;
                }
            }

            return ret;
        }

        protected void gv_items_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.ControlStyle.BackColor = Color.LightGray;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.CssClass = ((fila_item_ingreso_egreso_equipo)e.Row.DataItem).row_class;
            }
        }

        protected void btn_ver_editar_detalle_ServerClick(object sender, EventArgs e)
        {
            int id_valor_item_mes = Convert.ToInt32(((System.Web.UI.HtmlControls.HtmlButton)sender).Attributes["data-id"]);

            Ver_editar_valores_mes(id_valor_item_mes);
        }

        private void Ver_editar_valores_mes(int id_valor_item_mes)
        {
            int mes = 0; int anio = 0;
            using (var cxt = new Model1Container())
            {
                Valor_mes item_valor_mes = cxt.Valores_meses.First(x => x.id == id_valor_item_mes);
                Ingreso_egreso_mensual_equipo ioequipo = cxt.Ingresos_egresos_mensuales_equipos.First(x => x.id_ingreso_egreso_mensual == item_valor_mes.id_ingreso_egreso_mensual);
                mes = ioequipo.mes; anio = ioequipo.anio;
                hidden_id_valor_mes.Value = id_valor_item_mes.ToString();
                lbl_item.Text = item_valor_mes.Item.nombre;
                lbl_categoria.Text = item_valor_mes.Item.tipo;
                switch (ioequipo.mes)
                {
                    case 1:
                        lbl_mes.Text = "Enero " + ioequipo.anio.ToString();
                        break;
                    case 2:
                        lbl_mes.Text = "Febrero " + ioequipo.anio.ToString();
                        break;
                    case 3:
                        lbl_mes.Text = "Marzo " + ioequipo.anio.ToString();
                        break;
                    case 4:
                        lbl_mes.Text = "Abril " + ioequipo.anio.ToString();
                        break;
                    case 5:
                        lbl_mes.Text = "Mayo " + ioequipo.anio.ToString();
                        break;
                    case 6:
                        lbl_mes.Text = "Junio " + ioequipo.anio.ToString();
                        break;
                    case 7:
                        lbl_mes.Text = "Julio " + ioequipo.anio.ToString();
                        break;
                    case 8:
                        lbl_mes.Text = "Agosto " + ioequipo.anio.ToString();
                        break;
                    case 9:
                        lbl_mes.Text = "Septiembre " + ioequipo.anio.ToString();
                        break;
                    case 10:
                        lbl_mes.Text = "Octubre " + ioequipo.anio.ToString();
                        break;
                    case 11:
                        lbl_mes.Text = "Noviembre " + ioequipo.anio.ToString();
                        break;
                    case 12:
                        lbl_mes.Text = "Diciembre " + ioequipo.anio.ToString();
                        break;
                    default:
                        break;
                }

                tb_detalle_fecha.Value = string.Empty;
                tb_detalle_monto.Text = string.Empty;
                tb_detalle_descripcion.Text = string.Empty;

                var detalle = (from x in item_valor_mes.Detalle
                               select new
                               {
                                   detalle_id = x.id_detalle_valor_item_mes,
                                   detalle_fecha = x.fecha,
                                   detalle_monto = x.monto,
                                   detalle_descripcion = x.descripcion
                               }).ToList();

                gv_detalle.DataSource = detalle;
                gv_detalle.DataBind();

                lbl_total_item_mes.Text = item_valor_mes.Detalle.Sum(x => x.monto).ToString("$ #,##0.00");
            }


            MostrarPopUpDetalle(mes,anio);
        }

        private void MostrarPopUpDetalle(int mes, int anio)
        {
            DateTime minDate = new DateTime(anio, mes, 1);
            DateTime maxDate = new DateTime(anio, mes, DateTime.DaysInMonth(anio, mes));
            /*
              $(function () { $('#dtp_fecha').datetimepicker({ locale: 'es', format: 'DD/MM/YYYY', minDate: '01/01/2015', maxDate: '01/31/2015' }); });
             */
            string script = "<script language=\"javascript\"  type=\"text/javascript\">$(document).ready(function() { $('#ver_detalle_item_mes').modal('show')}); $(function () { $('#dtp_fecha').datetimepicker({ locale: 'es', format: 'DD/MM/YYYY', minDate: '" + minDate.ToString("MM/dd/yyyy") + "', maxDate: '" + maxDate.ToString("MM/dd/yyyy") + "' }); });</script>";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "ShowPopUp", script, false);
        }

        protected void btn_nueva_busqueda_Click(object sender, EventArgs e)
        {
            Estado_busqueda(true);
        }

        protected void cv_monto_ServerValidate(object source, ServerValidateEventArgs args)
        {
            decimal d;
            args.IsValid = decimal.TryParse(tb_detalle_monto.Text, out d);
        }

        protected void cv_fecha_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime dia;
            args.IsValid = DateTime.TryParse(tb_detalle_fecha.Value, out dia);
        }

        protected void gv_detalle_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.ControlStyle.BackColor = Color.LightGray;
            }
        }

        protected void btn_aceptar_eliminacion_Click(object sender, EventArgs e)
        {
            //int id_detalle = Convert.ToInt32(((System.Web.UI.HtmlControls.HtmlButton)sender).Attributes["data-id"]);
            int mes = 0; int anio = 0;
            int id_detalle = Convert.ToInt32(id_item_por_eliminar.Value);
            int id_valor_item_mes = 0;
            using (var cxt = new Model1Container())
            {
                
                Detalle_valor_item_mes detalle = cxt.Detalle_valores_items_mes.First(x => x.id_detalle_valor_item_mes == id_detalle);
                Valor_mes vm = detalle.Valor_item_mes;
                id_valor_item_mes = vm.id;
                Ingreso_egreso_mensual_equipo ioequipo = cxt.Ingresos_egresos_mensuales_equipos.First(x => x.id_ingreso_egreso_mensual == detalle.Valor_item_mes.id_ingreso_egreso_mensual);
                mes = ioequipo.mes;
                anio = ioequipo.anio;
                cxt.Detalle_valores_items_mes.Remove(detalle);
                cxt.SaveChanges();

                decimal valor = vm.Detalle.Sum(x => x.monto);
                vm.valor = valor;

                //controlar campos calculados

                //impuesto sobre los ingresos en blanco
                if (vm.Item.nombre == "Trabajado" && vm.Item.Padre.nombre == "INGRESOS")
                {
                    int id_item_impuesto = cxt.Items_ingresos_egresos.First(x => x.nombre == "Impuestos" && x.Padre.nombre == "INGRESOS").id_item;
                    Valor_mes vm_impuesto = cxt.Valores_meses.FirstOrDefault(ii => ii.id_ingreso_egreso_mensual == vm.id_ingreso_egreso_mensual && ii.id_item == id_item_impuesto);

                    Detalle_valor_item_mes detalle_impuesto = new Detalle_valor_item_mes();
                    detalle_impuesto.fecha = new DateTime(ioequipo.anio, ioequipo.mes, 1);
                    detalle_impuesto.descripcion = "Calculado automáticamente";
                    detalle_impuesto.monto = valor * (Convert.ToDecimal(-5) / Convert.ToDecimal(100));

                    if (vm_impuesto.Detalle.Count > 1)
                    {
                        vm_impuesto.Detalle.Clear();
                        cxt.SaveChanges();
                    }

                    if (vm_impuesto.Detalle.Count == 0)
                    {
                        vm_impuesto.Detalle.Add(detalle_impuesto);
                        cxt.SaveChanges();
                    }
                    else
                    {
                        vm_impuesto.Detalle.First().monto = detalle_impuesto.monto;
                        cxt.SaveChanges();
                    }
                }

                //Accesorios hs extra
                if (vm.Item.nombre == "Horas Extra Chofer" && vm.Item.Padre.nombre == "Costos Variables")
                {
                    int id_item_impuesto = cxt.Items_ingresos_egresos.First(x => x.nombre == "Accesorios Hs Extra 24% (1/12x2)" && x.Padre.nombre == "Costos Variables").id_item;
                    Valor_mes vm_impuesto = cxt.Valores_meses.FirstOrDefault(ii => ii.id_ingreso_egreso_mensual == vm.id_ingreso_egreso_mensual && ii.id_item == id_item_impuesto);

                    Detalle_valor_item_mes detalle_impuesto = new Detalle_valor_item_mes();
                    detalle_impuesto.fecha = new DateTime(ioequipo.anio, ioequipo.mes, 1);
                    detalle_impuesto.descripcion = "Calculado automáticamente";
                    detalle_impuesto.monto = valor * (Convert.ToDecimal(-5) / Convert.ToDecimal(100));

                    if (vm_impuesto.Detalle.Count > 1)
                    {
                        vm_impuesto.Detalle.Clear();
                        cxt.SaveChanges();
                    }

                    if (vm_impuesto.Detalle.Count == 0)
                    {
                        vm_impuesto.Detalle.Add(detalle_impuesto);
                        cxt.SaveChanges();
                    }
                    else
                    {
                        vm_impuesto.Detalle.First().monto = detalle_impuesto.monto;
                        cxt.SaveChanges();
                    }
                }

                CrearMostrarTabla(ioequipo);
            }

            Ver_editar_valores_mes(id_valor_item_mes);
        }

        protected void btn_editar_detalle_ServerClick(object sender, EventArgs e)
        {
            int id_detalle = Convert.ToInt32(((System.Web.UI.HtmlControls.HtmlButton)sender).Attributes["data-id"]);
        }

        protected void bt_cancelar_eliminacion_Click(object sender, EventArgs e)
        {
            int mes = 0; int anio = 0;
            int id_detalle = Convert.ToInt32(id_item_por_eliminar.Value);
            using (var cxt = new Model1Container())
            {
                Detalle_valor_item_mes detalle = cxt.Detalle_valores_items_mes.First(x => x.id_detalle_valor_item_mes == id_detalle);
                Ingreso_egreso_mensual_equipo ioequipo = cxt.Ingresos_egresos_mensuales_equipos.First(x => x.id_ingreso_egreso_mensual == detalle.Valor_item_mes.id_ingreso_egreso_mensual);
                mes = ioequipo.mes;
                anio = ioequipo.anio;
            }
            MostrarPopUpDetalle(mes, anio);
        }

        protected void btn_agregar_detalle_ServerClick(object sender, EventArgs e)
        {
            int id_valo_item_mes = Convert.ToInt32(hidden_id_valor_mes.Value);
            Validate();
            if (IsValid)
            {
                using (var cxt = new Model1Container())
                {
                    Valor_mes vm = cxt.Valores_meses.First(x => x.id == id_valo_item_mes);
                    Ingreso_egreso_mensual_equipo ioequipo = cxt.Ingresos_egresos_mensuales_equipos.First(x => x.id_ingreso_egreso_mensual == vm.id_ingreso_egreso_mensual);
                    Detalle_valor_item_mes detalle = new Detalle_valor_item_mes();
                    detalle.monto = Convert.ToDecimal(tb_detalle_monto.Text);
                    detalle.descripcion = tb_detalle_descripcion.Text;
                    detalle.fecha = Convert.ToDateTime(tb_detalle_fecha.Value);
                    vm.Detalle.Add(detalle);

                    decimal valor = vm.Detalle.Sum(x => x.monto);
                    vm.valor = valor;

                    //controlar campos calculados

                    //impuesto sobre los ingresos en blanco
                    if (vm.Item.nombre == "Trabajado" && vm.Item.Padre.nombre == "INGRESOS")
                    {
                        int id_item_impuesto = cxt.Items_ingresos_egresos.First(x => x.nombre == "Impuestos" && x.Padre.nombre == "INGRESOS").id_item;
                        Valor_mes vm_impuesto = cxt.Valores_meses.FirstOrDefault(ii => ii.id_ingreso_egreso_mensual == vm.id_ingreso_egreso_mensual && ii.id_item == id_item_impuesto);

                        Detalle_valor_item_mes detalle_impuesto = new Detalle_valor_item_mes();
                        detalle_impuesto.fecha = new DateTime(ioequipo.anio, ioequipo.mes, 1);
                        detalle_impuesto.descripcion = "Calculado automáticamente";
                        detalle_impuesto.monto = valor * (Convert.ToDecimal(-5) / Convert.ToDecimal(100));

                        if (vm_impuesto.Detalle.Count > 1)
                        {
                            vm_impuesto.Detalle.Clear();
                            cxt.SaveChanges();
                        }

                        if (vm_impuesto.Detalle.Count == 0)
                        {
                            vm_impuesto.Detalle.Add(detalle_impuesto);
                            cxt.SaveChanges();
                        }
                        else
                        {
                            vm_impuesto.Detalle.First().monto = detalle_impuesto.monto;
                            cxt.SaveChanges();
                        }
                    }

                    //Accesorios hs extra
                    if (vm.Item.nombre == "Horas Extra Chofer" && vm.Item.Padre.nombre == "Costos Variables")
                    {
                        int id_item_impuesto = cxt.Items_ingresos_egresos.First(x => x.nombre == "Accesorios Hs Extra 24% (1/12x2)" && x.Padre.nombre == "Costos Variables").id_item;
                        Valor_mes vm_impuesto = cxt.Valores_meses.FirstOrDefault(ii => ii.id_ingreso_egreso_mensual == vm.id_ingreso_egreso_mensual && ii.id_item == id_item_impuesto);

                        Detalle_valor_item_mes detalle_impuesto = new Detalle_valor_item_mes();
                        detalle_impuesto.fecha = new DateTime(ioequipo.anio, ioequipo.mes, 1);
                        detalle_impuesto.descripcion = "Calculado automáticamente";
                        detalle_impuesto.monto = valor * (Convert.ToDecimal(-5) / Convert.ToDecimal(100));

                        if (vm_impuesto.Detalle.Count > 1)
                        {
                            vm_impuesto.Detalle.Clear();
                            cxt.SaveChanges();
                        }

                        if (vm_impuesto.Detalle.Count == 0)
                        {
                            vm_impuesto.Detalle.Add(detalle_impuesto);
                            cxt.SaveChanges();
                        }
                        else
                        {
                            vm_impuesto.Detalle.First().monto = detalle_impuesto.monto;
                            cxt.SaveChanges();
                        }
                    }

                    cxt.SaveChanges();
                    CrearMostrarTabla(ioequipo);
                }


            }

            Ver_editar_valores_mes(id_valo_item_mes);
        }
    }
}