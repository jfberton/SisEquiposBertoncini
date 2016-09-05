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
    public partial class admin_feriados : System.Web.UI.Page
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

                switch (usuariologueado.perfil)
                {
                    case perfil_usuario.Admin:
                        menu_admin.Visible = true;
                        menu_admin.Activar_Li("li_horas0ul_feriados");
                        menu_usuario.Visible = false;
                        break;
                    case perfil_usuario.Jefe:
                        menu_admin.Visible = true;
                        menu_admin.Activar_Li("li_horas0ul_feriados");
                        menu_usuario.Visible = false;
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

                if (usuariologueado.perfil == perfil_usuario.Jefe)
                {
                    btn_agregar_feriado.Visible = false;
                }

                CargarFeriados();
            }
        }

        private void CargarFeriados()
        {
            Usuario usuariologueado = Session["UsuarioLogueado"] as Usuario;
            using (var cxt = new Model1Container())
            {
                var feriados = (from ff in cxt.Feriados
                                select new
                                {
                                    feriado_id = ff.id_feriado,
                                    feriado_fecha = ff.fecha,
                                    feriado_descripcion = ff.descripcion
                                }).ToList();


                if (usuariologueado.perfil == perfil_usuario.Jefe)
                {
                    gv_feriados_view.DataSource = feriados;
                    gv_feriados_view.DataBind();
                    gv_feriados.Visible = false;
                }
                else
                {
                    gv_feriados.DataSource = feriados;
                    gv_feriados.DataBind();
                    gv_feriados_view.Visible = false;
                }
            }
        }

        private void MostrarPopUpFeriado()
        {
            string script = "<script language=\"javascript\"  type=\"text/javascript\">$(document).ready(function() { $('#ver_feriado').modal('show')});</script>";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "ShowPopUp", script, false);
        }

        protected void gv_feriados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.ControlStyle.BackColor = Color.LightGray;
            }
        }

        protected void btn_ver_Click(object sender, EventArgs e)
        {
            int id_feriado = Convert.ToInt32(((System.Web.UI.HtmlControls.HtmlButton)sender).Attributes["data-id"]);

            using (var cxt = new Model1Container())
            {
                Feriado feriado = cxt.Feriados.First(ff => ff.id_feriado == id_feriado);

                lbl_fecha.Text = feriado.fecha.ToLongDateString();
                lbl_descripcion.Text = feriado.descripcion;

            }

            MostrarPopUpFeriado();
        }

        protected void btn_aceptar_eliminacion_Click(object sender, EventArgs e)
        {
            int id_feriado = Convert.ToInt32(id_item_por_eliminar.Value);

            using (var cxt = new Model1Container())
            {
                Feriado feriado = cxt.Feriados.First(ff => ff.id_feriado == id_feriado);

                cxt.Feriados.Remove(feriado);
                cxt.SaveChanges();
            }

            CargarFeriados();
        }

        protected void btn_guardar_ServerClick(object sender, EventArgs e)
        {
            Validate();

            if (IsValid)
            {
                using (var cxt = new Model1Container())
                {
                    DateTime fecha;
                    if (DateTime.TryParse(tb_fecha_feriado.Value, out fecha))
                    {
                        Feriado feriado = new Feriado() { fecha = fecha, descripcion = tb_descripcion.Value };

                        cxt.Feriados.Add(feriado);

                        cxt.SaveChanges();

                        tb_descripcion.Value = string.Empty;
                        tb_fecha_feriado.Value = string.Empty;
                    }
                }

                CargarFeriados();
            }
            else
            {
                string script = string.Empty;

                script = "<script language=\"javascript\" type=\"text/javascript\">$(document).ready(function() { $('#agregar_area').modal('show')});</script>";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowPopUpError", script, false);
            }
        }

        protected void c_fecha_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime fecha;
            args.IsValid = DateTime.TryParse(tb_fecha_feriado.Value, out fecha);
        }

        protected void gv_feriados_PreRender(object sender, EventArgs e)
        {
            if (gv_feriados.Rows.Count > 0)
            {
                gv_feriados.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            if (gv_feriados_view.Rows.Count > 0)
            {
                gv_feriados_view.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
    }
}