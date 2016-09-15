using SisEquiposBertoncini.Aplicativo.Controles;
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
    public partial class admin_equipos : System.Web.UI.Page
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

                if (usuariologueado.perfil == perfil_usuario.Jefe)
                {
                    btn_agregar.Visible = false;
                }

                menu_admin.Activar_Li("li_equipos0ul_equipos");

                CargarEquipos();
            }
        }

        private void CargarEquipos()
        {
            using (var cxt = new Model1Container())
            {
                var equipos = (from ee in cxt.Equipos
                               where ee.fecha_baja == null && !ee.Generico
                               select new
                               {
                                   equipo_id = ee.id_equipo,
                                   equipo_categoria = ee.Categoria.nombre,
                                   equipo_nombre = ee.nombre
                               }).ToList();

                Usuario usuariologueado = Session["UsuarioLogueado"] as Usuario;
                
                if (usuariologueado.perfil == perfil_usuario.Jefe)
                {
                    gv_equipos_view.DataSource = equipos;
                    gv_equipos_view.DataBind();
                    gv_equipos.Visible = false;
                }
                else
                {
                    gv_equipos.DataSource = equipos;
                    gv_equipos.DataBind();
                    gv_equipos_view.Visible = false;
                }
            }
        }

        protected void gv_equipos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.ControlStyle.BackColor = Color.LightGray;
            }
        }

        protected void btn_ver_ServerClick(object sender, EventArgs e)
        {
            int id_equipo = Convert.ToInt32(((System.Web.UI.HtmlControls.HtmlButton)sender).Attributes["data-id"]);
            int mes = DateTime.Today.Month;
            int anio = DateTime.Today.Year;

            using (var cxt = new Model1Container())
            {
                Equipo equipo = cxt.Equipos.FirstOrDefault(ee => ee.id_equipo == id_equipo);
                lbl_categoria.Text = equipo.Categoria.nombre;
                lbl_nombre.Text = equipo.nombre;
                lbl_notas_equipo.Text = equipo.notas;
                lbl_out.Text = equipo.OUT ? "-[ OUT ]-" : "";
                var items = (from ii in equipo.Items_por_amortizar
                             select new
                             {
                                 id_parte = ii.id_item,
                                 nombre_parte = ii.nombre,
                                 costo_cero = ii.costo_cero_km_uss,
                                 porcentaje_usado = ii.porcentaje_usado,
                                 porcentaje_valor_residual = ii.porcentaje_valor_recidual,
                                 valor_por_amortizar = ii.valor_por_amortizar,
                                 meses_por_amortizar = ii.meses_por_amortizar,
                                 periodo_alta_anio = ii.periodo_alta_anio,
                                 periodo_alta_mes = ii.periodo_alta_mes,
                                 costo_mensual = ii.costo_mensual,
                                 meses_amortizados = ii.Periodos_amortizados(mes, anio),
                                 restan_amortizar = ii.Restan_por_amortizar(mes, anio)
                             }).ToList();

                var items_formateados = (from ii in items
                                         select new
                                         {
                                             id_parte = ii.id_parte,
                                             nombre_parte = ii.nombre_parte,
                                             costo_cero = Cadena.Formato_moneda(ii.costo_cero, Cadena.Moneda.dolares),
                                             porcentaje_usado = Cadena.Formato_porcentaje(ii.porcentaje_usado),
                                             porcentaje_valor_residual = Cadena.Formato_porcentaje(ii.porcentaje_valor_residual),
                                             valor_por_amortizar = Cadena.Formato_moneda(ii.valor_por_amortizar, Cadena.Moneda.dolares),
                                             meses_por_amortizar = ii.meses_por_amortizar,
                                             costo_mensual = Cadena.Formato_moneda(ii.costo_mensual, Cadena.Moneda.dolares),
                                             periodo_alta = ii.periodo_alta_mes.ToString() + "/" + ii.periodo_alta_anio.ToString(),
                                             meses_amortizados = ii.meses_amortizados,
                                             restan_amortizar = ii.restan_amortizar
                                         }).ToList();

                gv_partes.DataSource = items_formateados;
                gv_partes.DataBind();

                gv_partes.Font.Size = new FontUnit("9");

                Controles.imagenes_equipo ie = LoadControl("~/Aplicativo/Controles/imagenes_equipo.ascx") as Controles.imagenes_equipo;
                ie.Equipo = equipo;
                div_imagenes_equipo.Controls.Clear();
                div_imagenes_equipo.Controls.Add(ie);

                //imagen_equipo.Equipo = equipo;

                lbl_costo_total_0km.Text = Cadena.Formato_moneda(items.Sum(ii => ii.costo_cero), Cadena.Moneda.dolares);
                lbl_costo_mensual.Text = Cadena.Formato_moneda(items.Where(x=>x.restan_amortizar > 0).Sum(ii => ii.costo_mensual), Cadena.Moneda.dolares);
                lbl_valor_por_amortizar.Text = Cadena.Formato_moneda(items.Where(x=>x.restan_amortizar > 0).Sum(ii => ii.valor_por_amortizar), Cadena.Moneda.dolares);
            }

            string script = "<script language=\"javascript\"  type=\"text/javascript\">$(document).ready(function() { $('#ver_equipo').modal('show'); });</script>";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "ShowPopUp", script, false);
        }

        private void MostrarPopUpEquipo()
        {
            string script = "<script language=\"javascript\"  type=\"text/javascript\">$(document).ready(function() { $('#ver_equipo').modal('show')});</script>";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "ShowPopUp", script, false);
        }

        protected void btn_aceptar_eliminacion_Click(object sender, EventArgs e)
        {
            using (var cxt = new Model1Container())
            {
                int id_equipo = Convert.ToInt32(id_item_por_eliminar.Value);

                Equipo equipo = cxt.Equipos.First(ee => ee.id_equipo == id_equipo);
                equipo.fecha_baja = DateTime.Today;

                cxt.SaveChanges();

                CargarEquipos();
            }
        }

        protected void btn_editar_ServerClick(object sender, EventArgs e)
        {
            int id_equipo = Convert.ToInt32(((System.Web.UI.HtmlControls.HtmlButton)sender).Attributes["data-id"]);
            Session["id_equipo"] = id_equipo;
            Response.Redirect("~/Aplicativo/admin_equipo_detalle.aspx");
        }

        protected void btn_agregar_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Aplicativo/admin_equipo_detalle.aspx");
        }

        protected void gv_equipos_PreRender(object sender, EventArgs e)
        {
            if (gv_equipos.Rows.Count > 0)
            {
                gv_equipos.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if (gv_equipos_view.Rows.Count > 0)
            {
                gv_equipos_view.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if (gv_partes.Rows.Count > 0)
            {
                gv_partes.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
    }
}