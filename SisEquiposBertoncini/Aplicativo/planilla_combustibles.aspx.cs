using SisEquiposBertoncini.Aplicativo.Datos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisEquiposBertoncini.Aplicativo
{
    public partial class planilla_combustibles : System.Web.UI.Page
    {
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
                    if (usuariologueado.perfil != perfil_usuario.Admin && usuariologueado.perfil != perfil_usuario.Jefe)
                    {
                        Response.Redirect("~/Default.aspx?mode=trucho");
                    }
                }

                Cargar_ddls();

                Estado_busqueda(true);
            }
        }

        protected void btn_buscar_Click(object sender, EventArgs e)
        {
            int id_equipo = Convert.ToInt32(ddl_equipo.SelectedItem.Value);
            int anio = Convert.ToInt32(ddl_anio.SelectedItem.Value);
            int mes = Convert.ToInt32(ddl_mes.SelectedItem.Value);

            CrearMostrarTabla(id_equipo, mes, anio);
        }

        protected void btn_nueva_busqueda_Click(object sender, EventArgs e)
        {
            Estado_busqueda(true);
        }

        protected void gv_combustible_PreRender(object sender, EventArgs e)
        {
            if (gv_combustible.Rows.Count > 0)
            {
                gv_combustible.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }


        private void CrearMostrarTabla(int id_equipo, int mes, int anio)
        {
            using (var cxt = new Model1Container())
            {
                DateTime primer_dia_mes = new DateTime(anio, mes, 1);
                DateTime ultimo_dia_mes = new DateTime(anio, mes, DateTime.DaysInMonth(anio, mes));

                var items_planilla = (from pc in cxt.Planilla_combustibles
                                      where
                                        pc.id_equipo == id_equipo &&
                                        pc.fecha >= primer_dia_mes &&
                                        pc.fecha <= ultimo_dia_mes
                                      select pc).OrderBy(x => x.fecha).ToList();

                decimal km = 0;
                foreach (Planilla_combustible item in items_planilla)
                {
                    if (km > 0)
                    {
                        try
                        {
                            item.promedio = item.litros * Convert.ToDecimal(100) / (item.km - km);
                        }
                        catch
                        {
                            item.promedio = 0;
                        }
                    }
                    else
                    {
                        item.promedio = 0;
                    }

                    km = item.km;
                }

                cxt.SaveChanges();

                var items_grilla = (from ip in items_planilla
                                    select new
                                    {
                                        id_planilla_combustible = ip.id_planilla_combustible,
                                        id_equipo = ip.Equipo.id_equipo,
                                        equipo = ip.Equipo.nombre,
                                        fecha = ip.fecha,
                                        chofer = ip.chofer,
                                        tanque_lleno = ip.tanque_lleno,
                                        litros = ip.litros,
                                        km = ip.km,
                                        promedio = ip.promedio,
                                        costo_total_facturado = ip.costo_total_facturado,
                                        lugar = ip.lugar
                                    }).ToList();

                gv_combustible.DataSource = items_grilla;
                gv_combustible.DataBind();

                Estado_busqueda(false);
            }
        }

        private void Cargar_ddls()
        {
            using (var cxt = new Model1Container())
            {
                var equipos_habilitados = cxt.Equipos.Where(ee => ee.fecha_baja == null && !ee.Generico);
                foreach (Equipo equipo in equipos_habilitados.OrderBy(x => x.nombre))
                {
                    ddl_equipo.Items.Add(new ListItem() { Value = equipo.id_equipo.ToString(), Text = equipo.nombre });
                }

                for (int anio = 2015; anio <= DateTime.Today.Year + 1; anio++)
                {
                    ddl_anio.Items.Add(new ListItem() { Value = anio.ToString(), Text = anio.ToString() });
                }
            }
        }

        private void Estado_busqueda(bool habilitado)
        {
            div_resultados.Visible = !habilitado;

            ddl_equipo.Enabled = habilitado;
            ddl_anio.Enabled = habilitado;
            ddl_mes.Enabled = habilitado;

            btn_nueva_busqueda.Visible = !habilitado;
            btn_buscar.Visible = habilitado;
            gv_combustible.Visible = !habilitado;
        }

        protected void btn_agregar_linea_ServerClick(object sender, EventArgs e)
        {
            lbl_agregar_factura_combust_titulo.Text = "Agregar factura combustible al equipo " + ddl_equipo.SelectedItem.Text;
            tb_chofer.Value = string.Empty;
            tb_fecha_factura.Value = string.Empty;
            chk_tanque_lleno.Checked = false;
            tb_km.Value = string.Empty;
            tb_litros.Value = string.Empty;
            id_factura_combust_hidden.Value = "0";

            string script = "<script language=\"javascript\"  type=\"text/javascript\">$(document).ready(function() { $('#agregar_linea_combustible').modal('show')});</script>";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "ShowPopUp", script, false);
        }

        protected void gv_combustible_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer)
            {
                try
                {
                    if (((CheckBox)((e.Row.Cells[2]).Controls[0])).Checked)
                    {
                        e.Row.ControlStyle.BackColor = Color.LightGreen;
                    }
                }
                catch
                {

                }
            }
        }

        protected void cv_fecha_factura_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime fecha;
            bool ok = DateTime.TryParse(tb_fecha_factura.Value, out fecha);
            ok = ok && fecha.Month == ddl_mes.SelectedIndex + 1;
            ok = ok && fecha.Year == Convert.ToInt32(ddl_anio.SelectedValue);
            args.IsValid = ok;
        }

        protected void cv_litros_ServerValidate(object source, ServerValidateEventArgs args)
        {
            decimal litros;
            args.IsValid = decimal.TryParse(tb_litros.Value.Replace(".",","), out litros);
        }

        protected void cv_km_ServerValidate(object source, ServerValidateEventArgs args)
        {
            decimal km;
            args.IsValid = decimal.TryParse(tb_km.Value.Replace(".", ","), out km);
        }

        protected void cv_costo_ServerValidate(object source, ServerValidateEventArgs args)
        {
            decimal costo;
            args.IsValid = decimal.TryParse(tb_costo.Value.Replace(".", ","), out costo);
        }

        protected void btn_guardar_ServerClick(object sender, EventArgs e)
        {
            this.Validate();

            if (IsValid)
            {
                int anio = Convert.ToInt32(ddl_anio.SelectedItem.Value);
                int mes = Convert.ToInt32(ddl_mes.SelectedItem.Value);
                int id_equipo = Convert.ToInt32(ddl_equipo.SelectedItem.Value);
                using (var cxt = new Model1Container())
                {
                    Planilla_combustible pc = null;

                    if (id_factura_combust_hidden.Value != "0")
                    {
                        //existe la linea de factura
                        int id_planilla_combustible = Convert.ToInt32(id_factura_combust_hidden.Value);
                        pc = cxt.Planilla_combustibles.FirstOrDefault(ppcc => ppcc.id_planilla_combustible == id_planilla_combustible);
                        pc.id_equipo = id_equipo;
                        pc.chofer = tb_chofer.Value;
                        pc.fecha = Convert.ToDateTime(tb_fecha_factura.Value);
                        pc.tanque_lleno = chk_tanque_lleno.Checked == true;
                        pc.km = Convert.ToDecimal(tb_km.Value.Replace(".", ","));
                        pc.litros = Convert.ToDecimal(tb_litros.Value.Replace(".", ","));
                        pc.promedio = 0;
                        pc.costo_total_facturado = Convert.ToDecimal(tb_costo.Value.Replace(".", ","));
                        pc.lugar = tb_lugar.Value;
                    }
                    else
                    {
                        pc = new Datos.Planilla_combustible();
                       
                        pc.chofer = tb_chofer.Value;
                        pc.id_equipo = id_equipo;
                        pc.fecha = Convert.ToDateTime(tb_fecha_factura.Value);
                        pc.tanque_lleno = chk_tanque_lleno.Checked == true;
                        pc.km = Convert.ToDecimal(tb_km.Value.Replace(".", ","));
                        pc.litros = Convert.ToDecimal(tb_litros.Value.Replace(".", ","));
                        pc.promedio = 0;
                        pc.costo_total_facturado = Convert.ToDecimal(tb_costo.Value.Replace(".", ","));
                        pc.lugar = tb_lugar.Value;
                        cxt.Planilla_combustibles.Add(pc);
                    }

                    tb_chofer.Value = string.Empty;
                    tb_fecha_factura.Value = string.Empty;
                    chk_tanque_lleno.Checked = false;
                    tb_km.Value = string.Empty;
                    tb_litros.Value = string.Empty;
                    tb_costo.Value = string.Empty;
                    tb_lugar.Value = string.Empty;
                    id_factura_combust_hidden.Value = "0";

                    cxt.SaveChanges();

                    Insertar_editar_detalle_valor_mes_combustible(pc.id_equipo, mes, anio, pc.costo_total_facturado, pc.fecha, "Chofer: " + pc.chofer + " Km: " + pc.km + " litros: " + pc.litros);
                }

                CrearMostrarTabla(id_equipo, mes, anio);
            }
            else
            {
                string script = string.Empty;

                script = "<script language=\"javascript\" type=\"text/javascript\">$(document).ready(function() { $('#agregar_linea_combustible').modal('show')});</script>";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowPopUpError", script, false);
            }
        }

        protected void btn_aceptar_eliminacion_Click(object sender, EventArgs e)
        {
            int id_planilla_combustible = Convert.ToInt32(id_item_por_eliminar.Value);
            int id_equipo = Convert.ToInt32(ddl_equipo.SelectedItem.Value);
            int anio = Convert.ToInt32(ddl_anio.SelectedItem.Value);
            int mes = Convert.ToInt32(ddl_mes.SelectedItem.Value);

            using (var cxt = new Model1Container())
            {
                Planilla_combustible pc = cxt.Planilla_combustibles.First(ppcc => ppcc.id_planilla_combustible == id_planilla_combustible);

                Eliminar_detalle_valor_mes_combustible(pc.id_equipo, mes, anio, pc.costo_total_facturado, pc.fecha);

                cxt.Planilla_combustibles.Remove(pc);
                cxt.SaveChanges();

                CrearMostrarTabla(id_equipo, mes, anio);
            }
        }

        protected void btn_ver_ServerClick(object sender, EventArgs e)
        {
            int id_planilla_combustible = Convert.ToInt32(((System.Web.UI.HtmlControls.HtmlButton)sender).Attributes["data-id"]);

            using (var cxt = new Model1Container())
            {
                Planilla_combustible pc = cxt.Planilla_combustibles.First(ppcc => ppcc.id_planilla_combustible == id_planilla_combustible);

                tb_chofer.Value = pc.chofer;
                tb_fecha_factura.Value = pc.fecha.ToShortDateString();
                chk_tanque_lleno.Checked = pc.tanque_lleno;
                tb_km.Value = pc.km.ToString();
                tb_litros.Value = pc.litros.ToString();
                tb_costo.Value = pc.costo_total_facturado.ToString();
                id_factura_combust_hidden.Value = pc.id_planilla_combustible.ToString();
            }

            string script = "<script language=\"javascript\" type=\"text/javascript\">$(document).ready(function() { $('#agregar_linea_combustible').modal('show'); $('#dtp_fecha').data('DateTimePicker').date('" + tb_fecha_factura.Value + "');  });</script>";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowPopUp", script, false);
        }

        public void Insertar_editar_detalle_valor_mes_combustible(int id_equipo, int mes, int anio, decimal monto,DateTime fecha, string observaciones)
        {
            using (var cxt = new Model1Container())
            {
                Valor_mes valores_mes = null;
                Item_ingreso_egreso item = cxt.Items_ingresos_egresos.First(i => i.nombre == "Combustible");
                Ingreso_egreso_mensual_equipo io_equipo = cxt.Ingresos_egresos_mensuales_equipos.FirstOrDefault(io => io.anio == anio && io.mes == mes && io.id_equipo == id_equipo);

                if (io_equipo != null)
                {
                    valores_mes = cxt.Valores_meses.FirstOrDefault(vm => vm.id_item == item.id_item && vm.id_ingreso_egreso_mensual == io_equipo.id_ingreso_egreso_mensual);
                    if (valores_mes != null)
                    {
                        Detalle_valor_item_mes detalle = cxt.Detalle_valores_items_mes.FirstOrDefault(dim => dim.fecha == fecha && dim.monto == monto && dim.id_valor_mes == valores_mes.id);
                        if (detalle == null)
                        {
                            detalle = new Detalle_valor_item_mes();
                            valores_mes.Detalle.Add(detalle);
                        }

                        detalle.fecha = fecha;
                        detalle.monto = monto;
                        detalle.descripcion = observaciones;
                    }
                    else
                    {
                        valores_mes = new Valor_mes();
                        valores_mes.id_ingreso_egreso_mensual = io_equipo.id_ingreso_egreso_mensual;
                        valores_mes.id_item = item.id_item;
                        valores_mes.valor = 0;
                        io_equipo.Valores_meses.Add(valores_mes);

                        Detalle_valor_item_mes detalle = new Datos.Detalle_valor_item_mes();
                        detalle.fecha = fecha;
                        detalle.monto = monto;
                        detalle.descripcion = observaciones;
                        valores_mes.Detalle.Add(detalle);
                    }
                }
                else
                {
                    io_equipo = new Ingreso_egreso_mensual_equipo();
                    io_equipo.id_equipo = id_equipo;
                    io_equipo.mes = mes;
                    io_equipo.anio = anio;

                    valores_mes = new Valor_mes();
                    valores_mes.id_item = item.id_item;
                    valores_mes.valor = 0;
                    io_equipo.Valores_meses.Add(valores_mes);

                    Detalle_valor_item_mes detalle = new Datos.Detalle_valor_item_mes();
                    detalle.fecha = fecha;
                    detalle.monto = monto;
                    detalle.descripcion = observaciones;
                    valores_mes.Detalle.Add(detalle);

                    cxt.Ingresos_egresos_mensuales_equipos.Add(io_equipo);
                }

                cxt.SaveChanges();
            }
        }

        protected void Eliminar_detalle_valor_mes_combustible(int id_equipo, int mes, int anio, decimal monto, DateTime fecha)
        {
            using (var cxt = new Model1Container())
            {
                Valor_mes valores_mes = null;
                Item_ingreso_egreso item = cxt.Items_ingresos_egresos.First(i => i.nombre == "Combustible");
                Ingreso_egreso_mensual_equipo io_equipo = cxt.Ingresos_egresos_mensuales_equipos.FirstOrDefault(io => io.anio == anio && io.mes == mes && io.id_equipo == id_equipo);

                if (io_equipo != null)
                {
                    valores_mes = cxt.Valores_meses.FirstOrDefault(vm => vm.id_item == item.id_item && vm.id_ingreso_egreso_mensual == io_equipo.id_ingreso_egreso_mensual);
                    if (valores_mes != null)
                    {
                        Detalle_valor_item_mes detalle = cxt.Detalle_valores_items_mes.FirstOrDefault(dim => dim.fecha == fecha && dim.monto == monto && dim.id_valor_mes == valores_mes.id);
                        if (detalle != null)
                        {
                            valores_mes.Detalle.Remove(detalle);
                            cxt.SaveChanges();
                        }
                    }
                }
            }
        }

    }
}