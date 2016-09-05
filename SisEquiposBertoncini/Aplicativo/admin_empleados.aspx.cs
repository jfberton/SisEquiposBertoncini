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
    public partial class admin_empleados : System.Web.UI.Page
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
                        menu_admin.Activar_Li("li_empleados0ul_empleados");
                        menu_usuario.Visible = false;
                        menu_usuario.Activar_Li("li_empleados0ul_empleados");
                        break;
                    case perfil_usuario.Jefe:
                        menu_admin.Visible = true;
                        menu_admin.Activar_Li("li_empleados0ul_empleados");
                        menu_usuario.Visible = false;
                        menu_usuario.Activar_Li("li_empleados0ul_empleados");
                        break;
                    case perfil_usuario.Supervisor:
                        break;
                    case perfil_usuario.Usuario:
                        menu_admin.Activar_Li("li_empleados0ul_empleados");
                        break;
                    case perfil_usuario.Seleccionar:
                        break;
                    default:
                        break;
                }

                if (usuariologueado.perfil == perfil_usuario.Jefe)
                {
                    btn_agregar_empleado.Visible = false;
                }

                CargarDDLs();

                CargarEmpleados();

               
            }
        }

        private void CargarEmpleados()
        {
            Usuario usuariologueado = Session["UsuarioLogueado"] as Usuario;

            using (var cxt = new Model1Container())
            {
                var empleados = (from ee in cxt.Empleados
                                 select new
                                 {
                                     empleado_id = ee.id_empleado,
                                     empleado_area = ee.Area.nombre,
                                     empleado_categoria = ee.Categoria.nombre,
                                     empleado_nombre = ee.nombre,
                                     empleado_dni = ee.dni,
                                     empleado_fecha_nacimiento = ee.fecha_nacimiento,
                                     empleado_fecha_alta = ee.fecha_alta,
                                     empleado_fecha_baja = ee.fecha_baja
                                 }).ToList();

                if (usuariologueado.perfil == perfil_usuario.Jefe)
                {
                    gv_empleados_view.DataSource = empleados;
                    gv_empleados_view.DataBind();
                    gv_empleados.Visible = false;
                }
                else
                {

                    gv_empleados.DataSource = empleados;
                    gv_empleados.DataBind();
                    gv_empleados_view.Visible = false;
                }

            }
        }

        private void CargarDDLs()
        {
            ddl_areas.Items.Add(new ListItem() { Value = "0", Text = "Seleccionar:" });
            ddl_categorias.Items.Add(new ListItem() { Value = "0", Text = "Seleccionar:" });
            using (var cxt = new Model1Container())
            {
                foreach (var area in cxt.Areas)
                {
                    ddl_areas.Items.Add(new ListItem() { Value = area.id_area.ToString(), Text = area.nombre });
                }

                foreach (var categoria in cxt.Categorias_empleados)
                {
                    ddl_categorias.Items.Add(new ListItem() { Value = categoria.id_categoria.ToString(), Text = categoria.nombre });
                }
            }
        }

        protected void gv_empleados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.Header)
            {
                var emp = e.Row.Cells[6].Text;
                if (emp != "&nbsp;")
                {
                    e.Row.ControlStyle.Font.Strikeout = true;
                    e.Row.ControlStyle.Font.Italic = true;
                }
            }
        }

        protected void cv_area_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ddl_areas.SelectedItem.Value != "0";
        }


        protected void btn_ver_empleado_Click(object sender, ImageClickEventArgs e)
        {
            int id_empleado = Convert.ToInt32(((ImageButton)sender).CommandArgument);

            using (var cxt = new Model1Container())
            {
                Empleado empleado = cxt.Empleados.First(ee => ee.id_empleado == id_empleado);

                lbl_area_empleado.Text = empleado.Area.nombre;
                lbl_categoria_empleado.Text = empleado.Categoria.nombre;
                lbl_dni_empleado.Text = empleado.dni;
                lbl_fecha_nacimiento_empleado.Text = empleado.fecha_nacimiento.ToShortDateString();
                lbl_nombre_empleado.Text = empleado.nombre;
                lbl_fecha_alta.Text = empleado.fecha_alta.Value.ToShortDateString();
                lbl_fecha_baja.Text = empleado.fecha_baja != null ? empleado.fecha_baja.Value.ToShortDateString() : "-";
            }

            MostrarPopUpEmpleado();
        }

        private void MostrarPopUpEmpleado()
        {
            string script = "<script language=\"javascript\"  type=\"text/javascript\">$(document).ready(function() { $('#ver_empleado').modal('show')});</script>";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "ShowPopUp", script, false);
        }

        protected void cv_categoria_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ddl_categorias.SelectedItem.Value != "0";
        }

        protected void cv_fecha_nacimiento_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime fecha;
            args.IsValid = DateTime.TryParse(tb_fecha_nacimiento_empleado.Value, out fecha);
        }

        protected void btn_aceptar_eliminacion_Click(object sender, EventArgs e)
        {
            using (var cxt = new Model1Container())
            {
                int id_empleado = Convert.ToInt32(id_empleado_por_eliminar.Value);

                Empleado empleado = cxt.Empleados.First(ee => ee.id_empleado == id_empleado);
                empleado.fecha_baja = DateTime.Today;

                cxt.SaveChanges();
                CargarEmpleados();
            }
        }

        protected void btn_guardar_ServerClick(object sender, EventArgs e)
        {
            this.Validate();
            if (IsValid)
            {

                using (var cxt = new Model1Container())
                {
                    Empleado empleado = null;

                    if (id_empleado_hidden.Value != "0")
                    {//existe empleado
                        int id_empleado = Convert.ToInt32(id_empleado_hidden.Value);
                        empleado = cxt.Empleados.FirstOrDefault(ee => ee.id_empleado == id_empleado);
                        empleado.dni = tb_dni_empleado.Value;
                        empleado.fecha_alta = Convert.ToDateTime(tb_fecha_alta_empleado.Value);
                        if (tb_fecha_baja_empleado.Value != "")
                        {
                            empleado.fecha_baja = Convert.ToDateTime(tb_fecha_baja_empleado.Value);
                        }
                        else
                        {
                            empleado.fecha_baja = null;
                        }
                        empleado.fecha_nacimiento = Convert.ToDateTime(tb_fecha_nacimiento_empleado.Value);
                        empleado.id_area = Convert.ToInt32(ddl_areas.SelectedItem.Value);
                        empleado.id_categoria = Convert.ToInt32(ddl_categorias.SelectedItem.Value);

                        empleado.nombre = tb_nombre_empleado.Value;
                    }
                    else
                    {
                        empleado = new Empleado()
                        {
                            dni = tb_dni_empleado.Value,
                            fecha_baja = null,
                            fecha_alta = Convert.ToDateTime(tb_fecha_alta_empleado.Value),
                            fecha_nacimiento = Convert.ToDateTime(tb_fecha_nacimiento_empleado.Value),
                            id_area = Convert.ToInt32(ddl_areas.SelectedItem.Value),
                            id_categoria = Convert.ToInt32(ddl_categorias.SelectedItem.Value),
                            nombre = tb_nombre_empleado.Value
                        };

                        if (tb_fecha_baja_empleado.Value != "")
                        {
                            empleado.fecha_baja = Convert.ToDateTime(tb_fecha_baja_empleado.Value);
                        }
                        else
                        {
                            empleado.fecha_baja = null;
                        }

                        cxt.Empleados.Add(empleado);
                    }

                    tb_dni_empleado.Value = string.Empty;
                    tb_fecha_nacimiento_empleado.Value = string.Empty;
                    tb_fecha_alta_empleado.Value = string.Empty;
                    tb_fecha_baja_empleado.Value = string.Empty;
                    ddl_areas.SelectedIndex = 0;
                    ddl_categorias.SelectedIndex = 0;
                    tb_nombre_empleado.Value = string.Empty;
                    id_empleado_hidden.Value = "0";

                    cxt.SaveChanges();
                }

                CargarEmpleados();
            }
            else
            {
                string script = string.Empty;

                script = "<script language=\"javascript\" type=\"text/javascript\">$(document).ready(function() { $('#agregar_empleado').modal('show')});</script>";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowPopUpError", script, false);
            }
        }

        protected void btn_editar_empleado_ServerClick(object sender, EventArgs e)
        {
            int id_empleado = Convert.ToInt32(((System.Web.UI.HtmlControls.HtmlButton)sender).Attributes["data-id"]);

            using (var cxt = new Model1Container())
            {
                Empleado empleado = cxt.Empleados.FirstOrDefault(emp => emp.id_empleado == id_empleado);
                if (empleado != null)
                {
                    id_empleado_hidden.Value = id_empleado.ToString();
                    lbl_agregar_empleado_titulo.Text = "Editar empleado";
                    ddl_areas.SelectedValue = empleado.Area.id_area.ToString();
                    ddl_categorias.SelectedValue = empleado.Categoria.id_categoria.ToString();
                    tb_nombre_empleado.Value = empleado.nombre;
                    tb_dni_empleado.Value = empleado.dni;
                    tb_fecha_nacimiento_empleado.Value = empleado.fecha_nacimiento.ToShortDateString();
                    tb_fecha_alta_empleado.Value = empleado.fecha_alta != null ? empleado.fecha_alta.Value.ToShortDateString() : "";
                    tb_fecha_baja_empleado.Value = empleado.fecha_baja != null ? empleado.fecha_baja.Value.ToShortDateString() : "";
                }

            }

            string script = "<script language=\"javascript\"  type=\"text/javascript\">$(document).ready(function() { $('#agregar_empleado').modal('show')});</script>";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "ShowPopUp", script, false);

        }

        protected void btn_agregar_empleado_ServerClick(object sender, EventArgs e)
        {
            lbl_agregar_empleado_titulo.Text = "Agregar empleado";
            tb_dni_empleado.Value = string.Empty;
            tb_fecha_nacimiento_empleado.Value = string.Empty;
            tb_fecha_alta_empleado.Value = string.Empty;
            tb_fecha_baja_empleado.Value = string.Empty;
            ddl_areas.SelectedIndex = 0;
            ddl_categorias.SelectedIndex = 0;
            tb_nombre_empleado.Value = string.Empty;
            id_empleado_hidden.Value = "0";

            string script = "<script language=\"javascript\"  type=\"text/javascript\">$(document).ready(function() { $('#agregar_empleado').modal('show')});</script>";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "ShowPopUp", script, false);
        }

        protected void cv_fecha_alta_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime fecha;
            args.IsValid = DateTime.TryParse(tb_fecha_alta_empleado.Value, out fecha);
        }

        protected void cv_fecha_baja_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime fecha;
            args.IsValid = tb_fecha_baja_empleado.Value == "" || DateTime.TryParse(tb_fecha_alta_empleado.Value, out fecha);
        }

        protected void gv_empleados_PreRender(object sender, EventArgs e)
        {
            if (gv_empleados.Rows.Count > 0)
            {
                gv_empleados.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if(gv_empleados_view.Rows.Count > 0)
            {
                gv_empleados_view.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
    }
}