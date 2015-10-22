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

                CargarDDLs();

                CargarEmpleados();
            }
        }

        private void CargarEmpleados()
        {
            using (var cxt = new Model1Container())
            {
                var empleados = (from ee in cxt.Empleados
                                 where ee.fecha_baja == null
                                 select new {
                                     empleado_id = ee.id_empleado,
                                     empleado_area = ee.Area.nombre,
                                     empleado_categoria = ee.Categoria.nombre,
                                     empleado_nombre = ee.nombre,
                                     empleado_dni = ee.dni,
                                     empleado_fecha_nacimiento = ee.fecha_nacimiento
                                 }).ToList();
                gv_empleados.DataSource = empleados;
                gv_empleados.DataBind();
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
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.ControlStyle.BackColor = Color.LightGray;
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
                    Empleado empleado = new Empleado()
                    {
                        dni = tb_dni_empleado.Value,
                        fecha_baja = null,
                        fecha_nacimiento = Convert.ToDateTime(tb_fecha_nacimiento_empleado.Value),
                        id_area = Convert.ToInt32(ddl_areas.SelectedItem.Value),
                        id_categoria = Convert.ToInt32(ddl_categorias.SelectedItem.Value),
                        nombre = tb_nombre_empleado.Value
                    };


                    cxt.Empleados.Add(empleado);

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
    }
}