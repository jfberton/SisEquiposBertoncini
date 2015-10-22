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
    public partial class admin_tipo_empleado : System.Web.UI.Page
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

                CargarCategorias();
            }
        }

        private void CargarCategorias()
        {
            using (var cxt = new Model1Container())
            {
                var categorias = (from aa in cxt.Categorias_empleados
                                  select new
                                  {
                                      categoria_id = aa.id_categoria,
                                      categoria_nombre = aa.nombre
                                  }).ToList();

                var items_categoria = (
                                        from cc in categorias
                                        select new
                                        {
                                            categoria_id = cc.categoria_id,
                                            categoria_nombre = cc.categoria_nombre,
                                            categoria_cantidad_empleados = cxt.Empleados.Count(ee => ee.id_categoria == cc.categoria_id)
                                        }
                                        ).ToList();

                gv_categorias.DataSource = items_categoria;
                gv_categorias.DataBind();
            }
        }

        protected void gv_categorias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.ControlStyle.BackColor = Color.LightGray;
            }

        }

        private void MostrarPopUpCategoria()
        {
            string script = "<script language=\"javascript\"  type=\"text/javascript\">$(document).ready(function() { $('#ver_categoria').modal('show')});</script>";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "ShowPopUp", script, false);
        }

        protected void btn_aceptar_eliminacion_Click(object sender, EventArgs e)
        {
            int id_categoria = Convert.ToInt32(id_item_por_eliminar.Value);

            using (var cxt = new Model1Container())
            {
                Categoria_empleado categoria = cxt.Categorias_empleados.First(aa => aa.id_categoria == id_categoria);

                if (categoria.Empleados.Count > 0)
                {
                    MessageBox.Show(this, "No se puede eliminar la categoria ya que la misma aún tiene empleados asociados", MessageBox.Tipo_MessageBox.Danger, "Imposible eliminar!");
                }
                else
                {
                    cxt.Categorias_empleados.Remove(categoria);
                    cxt.SaveChanges();
                }
            }

            CargarCategorias();
        }

        protected void btn_ver_ServerClick(object sender, EventArgs e)
        {
            int id_categoria = Convert.ToInt32(((System.Web.UI.HtmlControls.HtmlButton)sender).Attributes["data-id"]);

            using (var cxt = new Model1Container())
            {
                Categoria_empleado categoria = cxt.Categorias_empleados.First(aa => aa.id_categoria == id_categoria);

                lbl_nombre_categoria.Text = categoria.nombre;
                lbl_descripcion.Text = categoria.descripcion;

                var empleados = (from ee in cxt.Empleados
                                 where
                                     ee.fecha_baja == null &&
                                     ee.id_categoria == categoria.id_categoria
                                 select new
                                 {
                                     area_empleado = ee.Area.nombre,
                                     nombre_empleado = ee.nombre
                                 }).ToList();

                gv_empleados.DataSource = empleados;
                gv_empleados.DataBind();

            }

            MostrarPopUpCategoria();
        }

        protected void btn_guardar_ServerClick(object sender, EventArgs e)
        {
            Validate();
            if (IsValid)
            {
                using (var cxt = new Model1Container())
                {
                    Categoria_empleado categoria = new Categoria_empleado() { nombre = tb_nombre_categoria.Value, descripcion = tb_descripcion_categoria.Value };

                    cxt.Categorias_empleados.Add(categoria);

                    cxt.SaveChanges();
                }

                CargarCategorias();
            }
            else
            {
                string script = string.Empty;

                script = "<script language=\"javascript\" type=\"text/javascript\">$(document).ready(function() { $('#agregar_categoria').modal('show')});</script>";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowPopUpError", script, false);
            }
        }
    }
}