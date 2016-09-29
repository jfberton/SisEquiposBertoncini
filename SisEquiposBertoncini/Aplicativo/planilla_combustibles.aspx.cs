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

                btn_agregar_linea.Visible = usuariologueado.perfil == perfil_usuario.Admin;

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

        private enum Meses
        {
            Enero = 1,
            Febrero = 2,
            Marzo = 3,
            Abril = 4,
            Mayo = 5,
            Junio = 6,
            Julio = 7,
            Agosto = 8,
            Septiembre = 9,
            Octubre = 10,
            Noviembre = 11,
            Diciembre = 12
        }

        private void CrearMostrarTabla(int id_equipo, int mes, int anio)
        {
            Reportes.planilla_combustible_ds ds = new Reportes.planilla_combustible_ds();
            Reportes.planilla_combustible_ds.GeneralRow gr = ds.General.NewGeneralRow();
            gr.Equipo = ddl_equipo.SelectedItem.Text;
            gr.Periodo = ((Meses)mes).ToString() + " " + anio;
            ds.General.Rows.Add(gr);

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
                                        playa = ip.playa.HasValue ? ip.playa : false,
                                        promedio = ip.promedio,
                                        costo_total_facturado = ip.costo_total_facturado,
                                        lugar = ip.lugar
                                    }).ToList();

                foreach (var item in items_grilla)
                {
                    Reportes.planilla_combustible_ds.DetalleRow dr = ds.Detalle.NewDetalleRow();
                    dr.Fecha = item.fecha.ToString("dd/MM/yyyy");
                    dr.Equipo = item.equipo;
                    dr.Chofer = item.chofer;
                    dr.Playa = item.playa.HasValue ? (item.playa.Value == true ? "Si" : "No") : "No";
                    dr.Tanque_lleno = item.tanque_lleno ? "Si" : "No";
                    dr.Litros = item.litros.ToString("#,##0.00 lr");
                    dr.Km = item.km.ToString("#,##0.00 km");
                    dr.Promedio = item.promedio.ToString("#,##0.00");
                    dr.Costo = item.costo_total_facturado.ToString("$ #,##0.00");
                    dr.Lugar = item.lugar;

                    ds.Detalle.Rows.Add(dr);
                }

                Session["planilla_combustible"] = ds;

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

                ddl_equipo_IO.Items.Add(new ListItem() { Value = "0", Text = " - " });

                foreach (Equipo equipo in equipos_habilitados.OrderBy(x => x.nombre))
                {
                    ddl_equipo.Items.Add(new ListItem() { Value = equipo.id_equipo.ToString(), Text = equipo.nombre });
                    ddl_equipo_IO.Items.Add(new ListItem() { Value = equipo.id_equipo.ToString(), Text = equipo.nombre });
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
                        //existe la linea de factura obtengo los datos de la misma

                        int id_planilla_combustible = Convert.ToInt32(id_factura_combust_hidden.Value);
                        pc = cxt.Planilla_combustibles.FirstOrDefault(ppcc => ppcc.id_planilla_combustible == id_planilla_combustible);
                       

                        Equipo eq_io = cxt.Equipos.FirstOrDefault(ee => ee.nombre == pc.lugar);

                        int pc_equipo = Convert.ToInt32(ddl_equipo.SelectedItem.Value);
                        int pc_equipo_io = eq_io != null ? eq_io.id_equipo : 0;
                        bool pc_playa = pc.playa.HasValue ? pc.playa.Value : false;
                        decimal pc_monto = pc.costo_total_facturado;
                        DateTime pc_fecha = pc.fecha;

                        //valores del item y equipo a eliminar en caso de ser necesario
                        int pc_id_equipo_IO = pc_equipo_io != 0 ? pc_equipo_io : pc_equipo;
                        string pc_item_nombre = pc_playa ? "Combustible Playa" : (pc_equipo == pc_equipo_io ? "Gastos Camioneta Individ." : "Combustible");
                        Item_ingreso_egreso pc_item = cxt.Items_ingresos_egresos.FirstOrDefault(ii => ii.nombre == pc_item_nombre);

                        //Obtengo los datos con los que deberia terminar para saber si es un update o tengo que eliminar el existente y crear uno nuevo
                        int nuevo_pc_equipo = Convert.ToInt32(ddl_equipo.SelectedItem.Value); //este nunca va cambiar creo una variable para poder comparar sin mesclar los nuevos con los viejos
                        int nuevo_pc_equipo_io = Convert.ToInt32(ddl_equipo_IO.SelectedItem.Value);
                        bool nuevo_pc_playa = chk_playa.Checked;
                        //con estos 3 valores puedo determinar si hay que eliminar y crear uno nuevo o simplemente hacer update
                        int nuevo_pc_id_equipo_IO = nuevo_pc_equipo_io != 0 ? nuevo_pc_equipo_io : nuevo_pc_equipo;
                        string nuevo_pc_item_nombre = nuevo_pc_playa ? "Combustible Playa" : (nuevo_pc_equipo == nuevo_pc_equipo_io ? "Gastos Camioneta Individ." : "Combustible");
                        Item_ingreso_egreso nuevo_pc_item = cxt.Items_ingresos_egresos.FirstOrDefault(ii => ii.nombre == nuevo_pc_item_nombre);

                        if (pc_item.id_item != nuevo_pc_item.id_item || pc_id_equipo_IO != nuevo_pc_id_equipo_IO)
                        {
                            //cambio el item o el equipo al cual asignar el gasto, asi que tengo que eliminar el anterior
                            Eliminar_detalle_valor_mes(pc_item, pc_id_equipo_IO, mes, anio, pc_monto, pc_fecha);
                        }

                        pc.id_equipo = id_equipo;
                        pc.chofer = tb_chofer.Value;
                        pc.fecha = Convert.ToDateTime(tb_fecha_factura.Value);
                        pc.tanque_lleno = chk_tanque_lleno.Checked == true;
                        pc.km = Convert.ToDecimal(tb_km.Value.Replace(".", ","));
                        pc.litros = Convert.ToDecimal(tb_litros.Value.Replace(".", ","));
                        pc.promedio = 0;
                        pc.costo_total_facturado = Convert.ToDecimal(tb_costo.Value.Replace(".", ","));
                        pc.lugar = ddl_equipo_IO.SelectedItem.Text;
                        pc.playa = chk_playa.Checked;

                        Insertar_editar_detalle_valor_mes(nuevo_pc_item, nuevo_pc_id_equipo_IO, mes, anio, pc.costo_total_facturado, pc.fecha, "Chofer: " + pc.chofer + " Km: " + pc.km + " litros: " + pc.litros);
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
                        pc.lugar = ddl_equipo_IO.SelectedItem.Text;
                        pc.playa = chk_playa.Checked;
                        cxt.Planilla_combustibles.Add(pc);

                        int pc_equipo = Convert.ToInt32(ddl_equipo.SelectedItem.Value);
                        Equipo eq_io = cxt.Equipos.FirstOrDefault(ee => ee.nombre == pc.lugar);
                        int pc_equipo_io = eq_io != null ? eq_io.id_equipo : 0;
                        bool pc_playa = pc.playa.HasValue ? pc.playa.Value : false;

                        //valores que necesito para crear el nuevo detalle de ingreso o egreso
                        int pc_id_equipo_IO = pc_equipo_io != 0 ? pc_equipo_io : pc_equipo;
                        string pc_item_nombre = pc_playa ? "Combustible Playa" : (pc_equipo == pc_equipo_io ? "Gastos Camioneta Individ." : "Combustible");
                        Item_ingreso_egreso pc_item = cxt.Items_ingresos_egresos.FirstOrDefault(ii => ii.nombre == pc_item_nombre);

                        Insertar_editar_detalle_valor_mes(pc_item, pc_id_equipo_IO, mes, anio, pc.costo_total_facturado, pc.fecha, "Chofer: " + pc.chofer + " Km: " + pc.km + " litros: " + pc.litros);

                    }

                    cxt.SaveChanges();
                    
                }

                tb_chofer.Value = string.Empty;
                tb_fecha_factura.Value = string.Empty;
                chk_tanque_lleno.Checked = false;
                tb_km.Value = string.Empty;
                tb_litros.Value = string.Empty;
                tb_costo.Value = string.Empty;
                ddl_equipo_IO.SelectedValue = "0";
                id_factura_combust_hidden.Value = "0";

                CrearMostrarTabla(id_equipo, mes, anio);
            }
            else
            {
                string script = string.Empty;

                script = "<script language=\"javascript\" type=\"text/javascript\">$(document).ready(function() { $('#agregar_linea_combustible').modal('show')});</script>";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowPopUpError", script, false);
            }
        }

        private void Insertar_editar_detalle_valor_mes(Item_ingreso_egreso pc_item, int pc_id_equipo_IO, int mes, int anio, decimal costo_total_facturado, DateTime fecha, string observaciones)
        {
            using (var cxt = new Model1Container())
            {
                Valor_mes valores_mes = null;
                Item_ingreso_egreso item = cxt.Items_ingresos_egresos.First(i => i.id_item == pc_item.id_item);
                Ingreso_egreso_mensual_equipo io_equipo = cxt.Ingresos_egresos_mensuales_equipos.FirstOrDefault(io => io.anio == anio && io.mes == mes && io.id_equipo == pc_id_equipo_IO);

                if (io_equipo != null)
                {
                    valores_mes = cxt.Valores_meses.FirstOrDefault(vm => vm.id_item == item.id_item && vm.id_ingreso_egreso_mensual == io_equipo.id_ingreso_egreso_mensual);
                    if (valores_mes != null)
                    {
                        Detalle_valor_item_mes detalle = cxt.Detalle_valores_items_mes.FirstOrDefault(dim => dim.fecha == fecha && dim.monto == costo_total_facturado && dim.id_valor_mes == valores_mes.id);
                        if (detalle == null)
                        {
                            detalle = new Detalle_valor_item_mes();
                            valores_mes.Detalle.Add(detalle);
                        }

                        detalle.fecha = fecha;
                        detalle.monto = costo_total_facturado;
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
                        detalle.monto = costo_total_facturado;
                        detalle.descripcion = observaciones;
                        valores_mes.Detalle.Add(detalle);
                    }
                }
                else
                {
                    io_equipo = new Ingreso_egreso_mensual_equipo();
                    io_equipo.id_equipo = pc_id_equipo_IO;
                    io_equipo.mes = mes;
                    io_equipo.anio = anio;

                    valores_mes = new Valor_mes();
                    valores_mes.id_item = item.id_item;
                    valores_mes.valor = 0;
                    io_equipo.Valores_meses.Add(valores_mes);

                    Detalle_valor_item_mes detalle = new Datos.Detalle_valor_item_mes();
                    detalle.fecha = fecha;
                    detalle.monto = costo_total_facturado;
                    detalle.descripcion = observaciones;
                    valores_mes.Detalle.Add(detalle);

                    cxt.Ingresos_egresos_mensuales_equipos.Add(io_equipo);
                }

                cxt.SaveChanges();
            }
        }

        private void Eliminar_detalle_valor_mes(Item_ingreso_egreso pc_item, int pc_id_equipo_a_eliminar, int mes, int anio, decimal monto, DateTime fecha)
        {
            using (var cxt = new Model1Container())
            {
                Valor_mes valores_mes = null;
                Item_ingreso_egreso item = cxt.Items_ingresos_egresos.First(i => i.id_item == pc_item.id_item);
                Ingreso_egreso_mensual_equipo io_equipo = cxt.Ingresos_egresos_mensuales_equipos.FirstOrDefault(io => io.anio == anio && io.mes == mes && io.id_equipo == pc_id_equipo_a_eliminar);

                if (io_equipo != null)
                {
                    valores_mes = cxt.Valores_meses.FirstOrDefault(vm => vm.id_item == item.id_item && vm.id_ingreso_egreso_mensual == io_equipo.id_ingreso_egreso_mensual);
                    if (valores_mes != null)
                    {
                        Detalle_valor_item_mes detalle = cxt.Detalle_valores_items_mes.FirstOrDefault(dim => dim.fecha == fecha && dim.monto == monto && dim.id_valor_mes == valores_mes.id);
                        if (detalle != null)
                        {
                            valores_mes.valor = valores_mes.valor - detalle.monto;
                            cxt.Detalle_valores_items_mes.Remove(detalle);
                            cxt.SaveChanges();
                        }
                    }
                }
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
                Equipo eq_io = cxt.Equipos.FirstOrDefault(ee => ee.nombre == pc.lugar);
                int pc_equipo = Convert.ToInt32(ddl_equipo.SelectedItem.Value);
                int pc_equipo_io = eq_io != null ? eq_io.id_equipo : 0;
                bool pc_playa = pc.playa.HasValue ? pc.playa.Value : false;
                decimal pc_monto = pc.costo_total_facturado;
                DateTime pc_fecha = pc.fecha;

                //valores del item y equipo a eliminar en caso de ser necesario
                int pc_id_equipo_IO = pc_equipo_io != 0 ? pc_equipo_io : pc_equipo;
                string pc_item_nombre = pc_playa ? "Combustible Playa" : (pc_equipo == pc_equipo_io ? "Gastos Camioneta Individ." : "Combustible");
                Item_ingreso_egreso pc_item = cxt.Items_ingresos_egresos.FirstOrDefault(ii => ii.nombre == pc_item_nombre);

                Eliminar_detalle_valor_mes(pc_item, pc_id_equipo_IO, mes, anio, pc_monto, pc_fecha);

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
                Equipo eq_io = cxt.Equipos.FirstOrDefault(ee => ee.nombre == pc.lugar);

                tb_chofer.Value = pc.chofer;
                tb_fecha_factura.Value = pc.fecha.ToShortDateString();
                chk_tanque_lleno.Checked = pc.tanque_lleno;
                tb_km.Value = pc.km.ToString();
                tb_litros.Value = pc.litros.ToString();
                tb_costo.Value = pc.costo_total_facturado.ToString();
                ddl_equipo_IO.SelectedValue = eq_io != null ? eq_io.id_equipo.ToString() : "0";
                chk_playa.Checked = pc.playa != null && pc.playa.Value == true;

                id_factura_combust_hidden.Value = pc.id_planilla_combustible.ToString();
            }

            string script = "<script language=\"javascript\" type=\"text/javascript\">$(document).ready(function() { $('#agregar_linea_combustible').modal('show'); $('#dtp_fecha').data('DateTimePicker').date('" + tb_fecha_factura.Value + "');  });</script>";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowPopUp", script, false);
        }

    }
}