using SisEquiposBertoncini.Aplicativo.Controles;
using SisEquiposBertoncini.Aplicativo.Datos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisEquiposBertoncini.Aplicativo
{
    public partial class admin_areas : System.Web.UI.Page
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
                    if (usuariologueado.perfil == perfil_usuario.Jefe)
                    {
                        btn_agregar_area.Visible = false;
                    }
                }

                CargarAreas();
            }
        }

        private void CargarAreas()
        {
            using (var cxt = new Model1Container())
            {
                var areas = (from aa in cxt.Areas
                             select new
                             {
                                 area_id = aa.id_area,
                                 area_nombre = aa.nombre
                             }).ToList();

                var items_areas = (from aa in areas
                                   select new
                                   {
                                       area_id = aa.area_id,
                                       area_nombre = aa.area_nombre,
                                       area_empleados = cxt.Empleados.Count(ee => ee.id_area == aa.area_id && ee.fecha_baja == null)
                                   });
                Usuario usuariologueado = Session["UsuarioLogueado"] as Usuario;

                if (usuariologueado.perfil == perfil_usuario.Jefe)
                {
                    gv_areas_view.DataSource = items_areas;
                    gv_areas_view.DataBind();
                    gv_areas.Visible = false;
                }
                else
                {
                    gv_areas.DataSource = items_areas;
                    gv_areas.DataBind();
                    gv_areas_view.Visible = false;
                }

            }
        }

        private void MostrarPopUpArea()
        {
            string script = "<script language=\"javascript\"  type=\"text/javascript\">$(document).ready(function() { $('#ver_area').modal('show')});</script>";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "ShowPopUp", script, false);
        }

        protected void btn_ver_Click(object sender, EventArgs e)
        {
            int id_area = Convert.ToInt32(((System.Web.UI.HtmlControls.HtmlButton)sender).Attributes["data-id"]);

            using (var cxt = new Model1Container())
            {
                Area area = cxt.Areas.First(aa => aa.id_area == id_area);

                lbl_nombre_area.Text = area.nombre;

                var empleados = (from ee in area.Empleados
                                 where ee.fecha_baja == null
                                 select new
                                 {
                                     empleado_nombre = ee.nombre,
                                     empleado_tipo = ee.Categoria.nombre
                                 });

                gv_empleado.DataSource = empleados;
                gv_empleado.DataBind();
            }

            MostrarPopUpArea();
        }

        protected void btn_aceptar_eliminacion_Click(object sender, EventArgs e)
        {
            int id_area = Convert.ToInt32(id_item_por_eliminar.Value);

            using (var cxt = new Model1Container())
            {
                Area area = cxt.Areas.First(aa => aa.id_area == id_area);

                if (area.Empleados.Where(ee => ee.fecha_baja == null).Count() > 0)
                {
                    MessageBox.Show(this, "No se puede eliminar el área ya que la misma aún tiene empleados", MessageBox.Tipo_MessageBox.Danger, "Imposible eliminar!");
                }
                else
                {
                    cxt.Areas.Remove(area);
                    cxt.SaveChanges();
                }
            }

            CargarAreas();
        }

        protected void btn_guardar_ServerClick(object sender, EventArgs e)
        {
            Validate();
            if (IsValid)
            {
                using (var cxt = new Model1Container())
                {
                    Area area = new Area() { nombre = tb_nombre_area.Value };

                    cxt.Areas.Add(area);

                    cxt.SaveChanges();

                    tb_nombre_area.Value = string.Empty;
                }

                CargarAreas();
            }
            else
            {
                string script = string.Empty;

                script = "<script language=\"javascript\" type=\"text/javascript\">$(document).ready(function() { $('#agregar_area').modal('show')});</script>";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowPopUpError", script, false);
            }
        }

        protected void gv_areas_PreRender(object sender, EventArgs e)
        {
            if (gv_areas.Rows.Count > 0)
            {
                gv_areas.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if (gv_areas_view.Rows.Count > 0)
            {
                gv_areas_view.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if (gv_empleado.Rows.Count > 0)
            {
                gv_empleado.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
    }
}