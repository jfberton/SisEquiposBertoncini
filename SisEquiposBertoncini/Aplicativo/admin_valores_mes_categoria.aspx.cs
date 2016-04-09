﻿using SisEquiposBertoncini.Aplicativo.Controles;
using SisEquiposBertoncini.Aplicativo.Datos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SisEquiposBertoncini.Aplicativo
{
    public partial class admin_valores_mes_categoria : System.Web.UI.Page
    {
        private struct fila_item_ingreso_egreso_equipo
        {
            public int id_item { get; set; }
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
                else
                {
                    if (usuariologueado.perfil != perfil_usuario.Admin)
                    {
                        Response.Redirect("~/Default.aspx?mode=trucho");
                    }
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

                string str_id_categoria = Request.QueryString["cat"];
                string str_edita = Request.QueryString["ed"];
                string str_mes = Request.QueryString["m"];
                string str_anio = Request.QueryString["a"];

                if (str_id_categoria != null)
                {
                    ddl_categoria.SelectedValue = str_id_categoria;
                    ddl_mes.SelectedValue = str_mes;
                    ddl_anio.SelectedValue = str_anio;

                    Estado_busqueda(false);
                    using (var cxt = new Model1Container())
                    {
                        int id_categoria = Convert.ToInt32(str_id_categoria);
                        int anio = Convert.ToInt32(str_anio);
                        int mes = Convert.ToInt32(str_mes);

                        CrearMostrarTabla();
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
                var categorias = cxt.Categorias_equipos;
                foreach (Categoria_equipo cat in categorias.OrderBy(x => x.nombre))
                {
                    ddl_categoria.Items.Add(new ListItem() { Value = cat.id_categoria.ToString(), Text = cat.nombre });
                }

                for (int anio = 2015; anio <= DateTime.Today.Year + 1; anio++)
                {
                    ddl_anio.Items.Add(new ListItem() { Value = anio.ToString(), Text = anio.ToString() });
                }
            }
        }

        private void Estado_busqueda(bool habilitado)
        {
            div_buscar_primero.Visible = habilitado;
            div_tree.Visible = !habilitado;

            ddl_categoria.Enabled = habilitado;
            ddl_anio.Enabled = habilitado;
            ddl_mes.Enabled = habilitado;

            btn_nueva_busqueda.Visible = !habilitado;
            btn_buscar.Visible = habilitado;
        }

        protected void btn_buscar_Click(object sender, EventArgs e)
        {
            int id_categoria = Convert.ToInt32(ddl_categoria.SelectedItem.Value);
            int anio = Convert.ToInt32(ddl_anio.SelectedItem.Value);
            int mes = Convert.ToInt32(ddl_mes.SelectedItem.Value);

            Response.Redirect("~/Aplicativo/admin_valores_mes_categoria.aspx?cat=" + id_categoria.ToString() + "&ed=1&m=" + mes.ToString() + "&a=" + anio.ToString());
        }

        private void CrearMostrarTabla()
        {//recorro los items ingresos y egresos y voy cargando los montos sumando los valores de los distintos equipos que pertenecen a la categoria

            using (var cxt = new Model1Container())
            {
                List<fila_item_ingreso_egreso_equipo> items_grilla = new List<fila_item_ingreso_egreso_equipo>();

                List<Item_ingreso_egreso> conceptos;
                conceptos = cxt.Items_ingresos_egresos.ToList();
                var roots = conceptos.Where(ii => ii.id_item_padre == null);

                foreach (Item_ingreso_egreso item in roots)
                {
                    AgregarItem(items_grilla, item);
                }

                Session["items_grilla"] = items_grilla;

                gv_items.DataSource = items_grilla;
                gv_items.DataBind();
                div_alert.Visible = false;
            }

        }

        private void AgregarItem(List<fila_item_ingreso_egreso_equipo> items_grilla, Item_ingreso_egreso item)
        {
            using (var cxt = new Model1Container())
            {
                string str_id_categoria = Request.QueryString["cat"];
                string str_mes = Request.QueryString["m"];
                string str_anio = Request.QueryString["a"];

                int id_categoria = Convert.ToInt32(str_id_categoria);
                int mes = Convert.ToInt32(str_mes);
                int anio = Convert.ToInt32(str_anio);
                List<Ingreso_egreso_mensual_equipo> io_equipos_categoria = (
                                                                            from io in cxt.Ingresos_egresos_mensuales_equipos
                                                                            where io.Equipo.id_categoria == id_categoria &&
                                                                            io.mes == mes &&
                                                                            io.anio == anio
                                                                            select io
                                                                            ).ToList();

                decimal valor_mes_equipos = (from io_mes in io_equipos_categoria
                                             select new
                                             {
                                                 Valor = ObtenerValor(io_mes, item)
                                             }).Sum(s => s.Valor);

                Aux_total_categoria_mes auxcategoria = cxt.Aux_total_categoria_meses.FirstOrDefault(x => x.mes == mes && x.anio == anio && x.id_categoria_equipo == id_categoria);
                decimal valor_mes_categoria = auxcategoria != null ? ObtenerValorCategoria(auxcategoria, item) : 0;

                decimal valor_mes = valor_mes_equipos + valor_mes_categoria;

                bool _visible = (item.Hijos.Count == 0 && !(item.nombre == "Impuestos" && item.Padre.nombre == "INGRESOS") && !(item.nombre == "Accesorios Hs Extra 24% (1/12x2)" && item.Padre.nombre == "Otros costos variables") && !(item.nombre == "Amortización" && item.Padre.nombre == "Costos Fijos No Erogables"));
                items_grilla.Add(new fila_item_ingreso_egreso_equipo()
                {
                    id_item = item.id_item,
                    concepto = item.nombre,
                    valor = valor_mes,
                    valorstr = valor_mes.ToString("$ #,##0.00"),
                    visible = _visible,
                    row_class = "treegrid-" + item.id_item + (item.id_item_padre != null ? " treegrid-parent-" + item.id_item_padre : "") + (item.id_item_padre == null ? " h4" : "") + (item.tipo == "Ingreso" ? " alert-success" : " alert-danger")
                });

                foreach (Item_ingreso_egreso hijo in item.Hijos.OrderBy(x => x.id_item))
                {
                    AgregarItem(items_grilla, hijo);
                }
            }

        }

        private decimal ObtenerValorCategoria(Aux_total_categoria_mes auxcategoria, Item_ingreso_egreso item)
        {
            decimal ret = 0;

            using (var cxt = new Model1Container())
            {
                auxcategoria = cxt.Aux_total_categoria_meses.First(x => x.id_aux_total_categoria_mes == auxcategoria.id_aux_total_categoria_mes);

                if (item.Hijos.Count > 0)
                {
                    foreach (Item_ingreso_egreso hijo in item.Hijos)
                    {
                        ret = ret + ObtenerValorCategoria(auxcategoria, hijo);
                    }

                    Valor_mes_categoria valor_mes_item = auxcategoria.Valores_mes.FirstOrDefault(x => x.id_item == item.id_item);
                    if (valor_mes_item != null)
                    {
                        valor_mes_item.valor = ret;
                    }
                    else
                    {
                        valor_mes_item = new Valor_mes_categoria()
                        {
                            id_item = item.id_item,
                            valor = 0
                        };

                        auxcategoria.Valores_mes.Add(valor_mes_item);
                    }
                    cxt.SaveChanges();
                }
                else
                {
                    Valor_mes_categoria valor_mes_item = auxcategoria.Valores_mes.FirstOrDefault(x => x.id_item == item.id_item);
                    if (valor_mes_item != null)
                    {
                        valor_mes_item.valor = valor_mes_item.Detalle.Sum(x => x.monto);
                    }
                    else
                    {
                        valor_mes_item = new Valor_mes_categoria()
                        {
                            id_item = item.id_item,
                            valor = 0
                        };

                        auxcategoria.Valores_mes.Add(valor_mes_item);
                    }

                    cxt.SaveChanges();
                    ret = valor_mes_item.valor;
                }
            }

            return ret;
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

                if ((ioEquipo.Equipo.EsTrabajo.HasValue && ioEquipo.Equipo.EsTrabajo.Value == true && item.mostrar_en_trabajo.HasValue && item.mostrar_en_trabajo.Value == true) ||
                    (ioEquipo.Equipo.EsTrabajo.HasValue && ioEquipo.Equipo.EsTrabajo.Value == false && item.mostrar_en_equipo.HasValue && item.mostrar_en_equipo.Value == true))
                {
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
            int id_item = Convert.ToInt32(((System.Web.UI.HtmlControls.HtmlButton)sender).Attributes["data-id"]);

            Ver_editar_valores_mes(id_item);
        }


        struct item_detalle
        {
            /// <summary>
            /// Id_equipo, 0 si es para la categoria
            /// </summary>
            public int id_item_detalle { get; set; }
            public int id_item_io { get; set; }
            public string equipo { get; set; }
            public DateTime fecha { get; set; }
            public decimal monto { get; set; }
            public string descripcion { get; set; }
        }

        private void Ver_editar_valores_mes(int id_item)
        {
            int mes = 0; int anio = 0;
            using (var cxt = new Model1Container())
            {
                string str_id_categoria = Request.QueryString["cat"];
                string str_mes = Request.QueryString["m"];
                string str_anio = Request.QueryString["a"];

                int id_categoria = Convert.ToInt32(str_id_categoria);
                mes = Convert.ToInt32(str_mes);
                anio = Convert.ToInt32(str_anio);

                var equipos = cxt.Equipos.Where(ee => ee.Categoria.id_categoria == id_categoria && ee.fecha_baja == null).OrderBy(e => e.nombre);

                ddl_equipo.Items.Clear();
                ddl_equipo.Items.Add(new ListItem() { Text = "Asociado a la categoria", Value = "0" });
                foreach (Equipo equipo in equipos)
                {
                    ddl_equipo.Items.Add(new ListItem() { Text = equipo.nombre, Value = equipo.id_equipo.ToString() });
                }

                Item_ingreso_egreso item = cxt.Items_ingresos_egresos.FirstOrDefault(ii => ii.id_item == id_item);

                //Valor_mes item_valor_mes = cxt.Valores_meses.First(x => x.id == id_valor_item_mes);
                hidden_id_item_01.Value = id_item.ToString();
                lbl_item.Text = item.nombre;
                lbl_categoria.Text = item.tipo;
                switch (mes)
                {
                    case 1:
                        lbl_mes.Text = "Enero " + anio.ToString();
                        break;
                    case 2:
                        lbl_mes.Text = "Febrero " + anio.ToString();
                        break;
                    case 3:
                        lbl_mes.Text = "Marzo " + anio.ToString();
                        break;
                    case 4:
                        lbl_mes.Text = "Abril " + anio.ToString();
                        break;
                    case 5:
                        lbl_mes.Text = "Mayo " + anio.ToString();
                        break;
                    case 6:
                        lbl_mes.Text = "Junio " + anio.ToString();
                        break;
                    case 7:
                        lbl_mes.Text = "Julio " + anio.ToString();
                        break;
                    case 8:
                        lbl_mes.Text = "Agosto " + anio.ToString();
                        break;
                    case 9:
                        lbl_mes.Text = "Septiembre " + anio.ToString();
                        break;
                    case 10:
                        lbl_mes.Text = "Octubre " + anio.ToString();
                        break;
                    case 11:
                        lbl_mes.Text = "Noviembre " + anio.ToString();
                        break;
                    case 12:
                        lbl_mes.Text = "Diciembre " + anio.ToString();
                        break;
                    default:
                        break;
                }

                tb_detalle_fecha.Value = string.Empty;
                tb_detalle_monto.Text = string.Empty;
                tb_detalle_descripcion.Text = string.Empty;

                List<item_detalle> detalle = new List<item_detalle>();

                List<Ingreso_egreso_mensual_equipo> io_equipos_categoria = (from io in cxt.Ingresos_egresos_mensuales_equipos
                                                                            where io.Equipo.id_categoria == id_categoria &&
                                                                            io.mes == mes &&
                                                                            io.anio == anio
                                                                            select io
                                                                                ).ToList();

                foreach (Ingreso_egreso_mensual_equipo item_io_equipo in io_equipos_categoria)
                {
                    var valor_mes = item_io_equipo.Valores_meses.FirstOrDefault(vm => vm.id_item == item.id_item);
                    if (valor_mes != null)
                    {
                        foreach (Detalle_valor_item_mes item_detalle in valor_mes.Detalle)
                        {
                            detalle.Add(new item_detalle() { id_item_detalle = item_detalle.id_detalle_valor_item_mes, id_item_io = item.id_item, equipo = item_io_equipo.Equipo.nombre, descripcion = item_detalle.descripcion, fecha = item_detalle.fecha, monto = item_detalle.monto });
                        }
                    }
                }

                Aux_total_categoria_mes aux_categoria_mes = cxt.Aux_total_categoria_meses.FirstOrDefault(aux => aux.mes == mes && aux.anio == anio && aux.id_categoria_equipo == id_categoria);
                if (aux_categoria_mes != null)
                {
                    var valor_mes_categoria = aux_categoria_mes.Valores_mes.FirstOrDefault(vm => vm.id_item == item.id_item);
                    if (valor_mes_categoria != null)
                    {
                        foreach (Detalle_valor_item_mes_categoria item_detalle in valor_mes_categoria.Detalle)
                        {
                            detalle.Add(new item_detalle() { id_item_detalle = item_detalle.id_detalle_valor_item_mes_categoria, id_item_io = item.id_item, equipo = "Asociado a la categoria", descripcion = item_detalle.descripcion, fecha = item_detalle.fecha, monto = item_detalle.monto });
                        }
                    }
                }

                gv_detalle.DataSource = detalle;
                gv_detalle.DataBind();

                lbl_total_item_mes.Text = detalle.Sum(x => x.monto).ToString("$ #,##0.00");
            }


            MostrarPopUpDetalle(mes, anio);
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

            string str_id_categoria = Request.QueryString["cat"];
            string str_mes = Request.QueryString["m"];
            string str_anio = Request.QueryString["a"];

            int id_categoria = Convert.ToInt32(str_id_categoria);
            int mes = Convert.ToInt32(str_mes);
            int anio = Convert.ToInt32(str_anio);

            int id_item = Convert.ToInt32(hidden_id_item_io.Value);
            int id_detalle = Convert.ToInt32(hidden_id_item_por_eliminar.Value);

            if (hidden_equipo.Value != "Asociado a la categoria")
            {
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
                }

                
            }
            else
            {
                int id_valor_item_mes = 0;
                using (var cxt = new Model1Container())
                {
                    Detalle_valor_item_mes_categoria detalle = cxt.Detalle_valor_item_meses_categoria.First(x => x.id_detalle_valor_item_mes_categoria == id_detalle);
                    Valor_mes_categoria vm = detalle.Valor_mes_categoria;
                    id_valor_item_mes = vm.id_valor_mes_categoria_item;

                    Aux_total_categoria_mes iocategoria = cxt.Aux_total_categoria_meses.First(x => x.id_aux_total_categoria_mes == detalle.Valor_mes_categoria.id_aux_total_categoria_mes);
                    mes = iocategoria.mes;
                    anio = iocategoria.anio;
                    cxt.Detalle_valor_item_meses_categoria.Remove(detalle);
                    cxt.SaveChanges();

                    decimal valor = vm.Detalle.Sum(x => x.monto);
                    vm.valor = valor;

                    //Accesorios hs extra
                    if (vm.Item_ingreso_egreso.nombre == "Horas Extra Chofer" && vm.Item_ingreso_egreso.Padre.nombre == "Costos Variables")
                    {
                        int id_item_impuesto = cxt.Items_ingresos_egresos.First(x => x.nombre == "Accesorios Hs Extra 24% (1/12x2)" && x.Padre.nombre == "Costos Variables").id_item;
                        Valor_mes_categoria vm_impuesto = cxt.Valor_mes_categorias.FirstOrDefault(ii => ii.id_aux_total_categoria_mes == vm.id_aux_total_categoria_mes && ii.id_item == id_item_impuesto);

                        Detalle_valor_item_mes_categoria detalle_impuesto = new Detalle_valor_item_mes_categoria();
                        detalle_impuesto.fecha = new DateTime(iocategoria.anio, iocategoria.mes, 1);
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
                }
            }

            div_alert.Visible = true;
            Ver_editar_valores_mes(id_item);
        }

        protected void btn_editar_detalle_ServerClick(object sender, EventArgs e)
        {
            int id_detalle = Convert.ToInt32(((System.Web.UI.HtmlControls.HtmlButton)sender).Attributes["data-id"]);
            string equipo = ((System.Web.UI.HtmlControls.HtmlButton)sender).Attributes["data-equipo"];
            hidden_id_detalle_01.Value = id_detalle.ToString();
            DateTime minDate;
            DateTime maxDate;

            if (equipo != "Asociado a la categoria")
            {
                using (var cxt = new Model1Container())
                {
                    Detalle_valor_item_mes detalle_por_editar = cxt.Detalle_valores_items_mes.First(dd => dd.id_detalle_valor_item_mes == id_detalle);
                    ddl_equipo.SelectedValue = cxt.Ingresos_egresos_mensuales_equipos.First(io => io.id_ingreso_egreso_mensual == detalle_por_editar.Valor_item_mes.id_ingreso_egreso_mensual).Equipo.id_equipo.ToString();
                    hidden_id_valor_mes.Value = detalle_por_editar.Valor_item_mes.id.ToString();
                    tb_detalle_monto.Text = detalle_por_editar.monto.ToString();
                    tb_detalle_fecha.Value = detalle_por_editar.fecha.ToShortDateString();
                    tb_detalle_descripcion.Text = detalle_por_editar.descripcion;
                    lbl_texto_boton_agregar_editar.Text = "Modificar";

                    minDate = new DateTime(detalle_por_editar.fecha.Year, detalle_por_editar.fecha.Month, 1);
                    maxDate = new DateTime(detalle_por_editar.fecha.Year, detalle_por_editar.fecha.Month, DateTime.DaysInMonth(detalle_por_editar.fecha.Year, detalle_por_editar.fecha.Month));
                }
            }
            else
            {
                using (var cxt = new Model1Container())
                {
                    Detalle_valor_item_mes_categoria detalle_por_editar = cxt.Detalle_valor_item_meses_categoria.First(dd => dd.id_detalle_valor_item_mes_categoria == id_detalle);
                    ddl_equipo.SelectedValue = "0";
                    hidden_id_valor_mes.Value = detalle_por_editar.Valor_mes_categoria.id_valor_mes_categoria_item.ToString();
                    tb_detalle_monto.Text = detalle_por_editar.monto.ToString();
                    tb_detalle_fecha.Value = detalle_por_editar.fecha.ToShortDateString();
                    tb_detalle_descripcion.Text = detalle_por_editar.descripcion;
                    lbl_texto_boton_agregar_editar.Text = "Modificar";

                    minDate = new DateTime(detalle_por_editar.fecha.Year, detalle_por_editar.fecha.Month, 1);
                    maxDate = new DateTime(detalle_por_editar.fecha.Year, detalle_por_editar.fecha.Month, DateTime.DaysInMonth(detalle_por_editar.fecha.Year, detalle_por_editar.fecha.Month));
                }
            }

            string script = "<script language=\"javascript\"  type=\"text/javascript\">$(document).ready(function() { $('#ver_detalle_item_mes').modal('show')}); $(function () { $('#dtp_fecha').datetimepicker({ locale: 'es', format: 'DD/MM/YYYY', minDate: '" + minDate.ToString("MM/dd/yyyy") + "', maxDate: '" + maxDate.ToString("MM/dd/yyyy") + "' }); });</script>";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "ShowPopUp", script, false);
        }

        protected void bt_cancelar_eliminacion_Click(object sender, EventArgs e)
        {
            int mes = 0; int anio = 0;
            int id_detalle = Convert.ToInt32(hidden_id_item_por_eliminar.Value);
            string equipo = hidden_equipo.Value;
            using (var cxt = new Model1Container())
            {
                if (equipo != "Asociado a la categoria")
                {
                    Detalle_valor_item_mes detalle = cxt.Detalle_valores_items_mes.First(x => x.id_detalle_valor_item_mes == id_detalle);
                    Ingreso_egreso_mensual_equipo ioequipo = cxt.Ingresos_egresos_mensuales_equipos.First(x => x.id_ingreso_egreso_mensual == detalle.Valor_item_mes.id_ingreso_egreso_mensual);
                    mes = ioequipo.mes;
                    anio = ioequipo.anio;
                }
                else
                {
                    Detalle_valor_item_mes_categoria detalle = cxt.Detalle_valor_item_meses_categoria.First(x => x.id_detalle_valor_item_mes_categoria == id_detalle);
                    Aux_total_categoria_mes iocategoria = cxt.Aux_total_categoria_meses.First(x => x.id_aux_total_categoria_mes == detalle.Valor_mes_categoria.id_aux_total_categoria_mes);
                    mes = iocategoria.mes;
                    anio = iocategoria.anio;
                }
            }

            MostrarPopUpDetalle(mes, anio);
        }

        protected void btn_agregar_detalle_ServerClick(object sender, EventArgs e)
        {
            string str_id_categoria = Request.QueryString["cat"];
            string str_mes = Request.QueryString["m"];
            string str_anio = Request.QueryString["a"];

            int id_categoria = Convert.ToInt32(str_id_categoria);
            int mes = Convert.ToInt32(str_mes);
            int anio = Convert.ToInt32(str_anio);

            int id_valor_item_mes = Convert.ToInt32(hidden_id_valor_mes.Value);
            int id_detalle = Convert.ToInt32(hidden_id_detalle_01.Value);
            int id_item = Convert.ToInt32(hidden_id_item_01.Value);
            hidden_id_detalle_01.Value = "0";
            Validate();
            if (IsValid)
            {
                using (var cxt = new Model1Container())
                {
                    if (ddl_equipo.SelectedItem.Text != "Asociado a la categoria")
                    {
                        Ingreso_egreso_mensual_equipo ioequipo;
                        Valor_mes vm;
                        int idEquipo = Convert.ToInt32(ddl_equipo.SelectedItem.Value);

                        ioequipo = cxt.Ingresos_egresos_mensuales_equipos.FirstOrDefault(x => x.mes == mes && x.anio == anio && x.id_equipo == idEquipo);
                        if (ioequipo == null)
                        {
                            ioequipo = new Ingreso_egreso_mensual_equipo();
                            ioequipo.anio = anio;
                            ioequipo.mes = mes;
                            ioequipo.id_equipo = idEquipo;

                            vm = new Valor_mes();
                            vm.valor = 0;
                            vm.id_item = id_item;

                            ioequipo.Valores_meses.Add(vm);

                            cxt.Ingresos_egresos_mensuales_equipos.Add(ioequipo);

                        }
                        else
                        {
                            vm = ioequipo.Valores_meses.FirstOrDefault(x => x.id_item == id_item);
                            if (vm == null)
                            {
                                vm = new Valor_mes();
                                vm.valor = 0;
                                vm.id_item = id_item;

                                ioequipo.Valores_meses.Add(vm);
                            }
                        }

                        cxt.SaveChanges();

                        ioequipo = cxt.Ingresos_egresos_mensuales_equipos.First(x => x.mes == mes && x.anio == anio && x.id_equipo == idEquipo);
                        vm = ioequipo.Valores_meses.First(x => x.id_item == id_item);


                        if (id_detalle > 0)
                        {
                            Detalle_valor_item_mes detalle = cxt.Detalle_valores_items_mes.First(x => x.id_detalle_valor_item_mes == id_detalle);
                            detalle.monto = Convert.ToDecimal(tb_detalle_monto.Text.Replace(".", ",").Replace("$", ""));
                            detalle.descripcion = tb_detalle_descripcion.Text;
                            detalle.fecha = Convert.ToDateTime(tb_detalle_fecha.Value);
                        }
                        else
                        {
                            Detalle_valor_item_mes detalle = new Detalle_valor_item_mes();
                            detalle.monto = Convert.ToDecimal(tb_detalle_monto.Text.Replace(".", ","));
                            detalle.descripcion = tb_detalle_descripcion.Text;
                            detalle.fecha = Convert.ToDateTime(tb_detalle_fecha.Value);
                            vm.Detalle.Add(detalle);
                        }

                        decimal valor = vm.Detalle.Sum(x => x.monto);
                        vm.valor = valor;

                        Item_ingreso_egreso item = cxt.Items_ingresos_egresos.First(x => x.id_item == id_item);

                        //Accesorios hs extra
                        if (item.nombre == "Horas Extra Chofer" && item.Padre.nombre == "Costos Variables")
                        {
                            int id_item_impuesto = cxt.Items_ingresos_egresos.First(x => x.nombre == "Accesorios Hs Extra 24% (1/12x2)" && x.Padre.nombre == "Costos Variables").id_item;
                            Valor_mes vm_impuesto = cxt.Valores_meses.FirstOrDefault(ii => ii.id_ingreso_egreso_mensual == vm.id_ingreso_egreso_mensual && ii.id_item == id_item_impuesto);

                            vm_impuesto = new Valor_mes();
                            vm_impuesto.valor = 0;
                            vm_impuesto.id_item = id_item_impuesto;

                            ioequipo.Valores_meses.Add(vm);

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
                    }
                    else
                    {
                        Aux_total_categoria_mes iocategoria;
                        Valor_mes_categoria vm;

                        iocategoria = cxt.Aux_total_categoria_meses.FirstOrDefault(x => x.mes == mes && x.anio == anio && x.id_categoria_equipo == id_categoria);
                        if (iocategoria == null)
                        {
                            iocategoria = new Aux_total_categoria_mes();
                            iocategoria.anio = anio;
                            iocategoria.mes = mes;
                            iocategoria.id_categoria_equipo = id_categoria;

                            vm = new Valor_mes_categoria();
                            vm.valor = 0;
                            vm.id_item = id_item;

                            iocategoria.Valores_mes.Add(vm);

                            cxt.Aux_total_categoria_meses.Add(iocategoria);

                        }
                        else
                        {
                            vm = iocategoria.Valores_mes.FirstOrDefault(x => x.id_item == id_item);
                            if (vm == null)
                            {
                                vm = new Valor_mes_categoria();
                                vm.valor = 0;
                                vm.id_item = id_item;

                                iocategoria.Valores_mes.Add(vm);
                            }
                        }

                        cxt.SaveChanges();

                        iocategoria = cxt.Aux_total_categoria_meses.First(x => x.mes == mes && x.anio == anio && x.id_categoria_equipo == id_categoria);
                        vm = iocategoria.Valores_mes.First(x => x.id_item == id_item);

                        if (id_detalle > 0)
                        {
                            Detalle_valor_item_mes_categoria detalle = cxt.Detalle_valor_item_meses_categoria.First(x => x.id_detalle_valor_item_mes_categoria == id_detalle);
                            detalle.monto = Convert.ToDecimal(tb_detalle_monto.Text.Replace(".", ",").Replace("$", ""));
                            detalle.descripcion = tb_detalle_descripcion.Text;
                            detalle.fecha = Convert.ToDateTime(tb_detalle_fecha.Value);
                        }
                        else
                        {
                            Detalle_valor_item_mes_categoria detalle = new Detalle_valor_item_mes_categoria();
                            detalle.monto = Convert.ToDecimal(tb_detalle_monto.Text.Replace(".", ","));
                            detalle.descripcion = tb_detalle_descripcion.Text;
                            detalle.fecha = Convert.ToDateTime(tb_detalle_fecha.Value);
                            vm.Detalle.Add(detalle);
                        }

                        decimal valor = vm.Detalle.Sum(x => x.monto);
                        vm.valor = valor;

                        Item_ingreso_egreso item = cxt.Items_ingresos_egresos.First(x => x.id_item == id_item);

                        //Accesorios hs extra
                        if (item.nombre == "Horas Extra Chofer" && item.Padre.nombre == "Costos Variables")
                        {
                            int id_item_impuesto = cxt.Items_ingresos_egresos.First(x => x.nombre == "Accesorios Hs Extra 24% (1/12x2)" && x.Padre.nombre == "Costos Variables").id_item;
                            Valor_mes_categoria vm_impuesto = cxt.Valor_mes_categorias.FirstOrDefault(ii => ii.id_aux_total_categoria_mes == vm.id_aux_total_categoria_mes && ii.id_item == id_item_impuesto);

                            vm_impuesto = new Valor_mes_categoria();
                            vm_impuesto.valor = 0;
                            vm_impuesto.id_item = id_item_impuesto;

                            iocategoria.Valores_mes.Add(vm);

                            Detalle_valor_item_mes_categoria detalle_impuesto = new Detalle_valor_item_mes_categoria();
                            detalle_impuesto.fecha = new DateTime(iocategoria.anio, iocategoria.mes, 1);
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
                    }


                    div_alert.Visible = true;
                }
            }

            lbl_texto_boton_agregar_editar.Text = "Agregar";

            Ver_editar_valores_mes(id_item);
        }

        protected void btn_refrescar_ServerClick(object sender, EventArgs e)
        {
            CrearMostrarTabla();
        }
    }
}