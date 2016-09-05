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
    public partial class admin_tipo_equipo : System.Web.UI.Page
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
                    btn_agregar_categoria.Visible = false;
                }

                menu_admin.Activar_Li("li_equipos0ul_categoias");

                CargarCategorias();
            }
        }

        private void CargarCategorias()
        {
            using (var cxt = new Model1Container())
            {
                var categorias = (from aa in cxt.Categorias_equipos
                                  where aa.nombre != "Otros"
                                  select new
                                  {
                                      categoria_id = aa.id_categoria,
                                      categoria_nombre = aa.nombre,
                                      categoria_muestra = aa.toma_en_cuenta_planilla_costos_horas_hombre
                                  }).ToList();

                var items_categoria = (
                                        from cc in categorias
                                        select new
                                        {
                                            categoria_id = cc.categoria_id,
                                            categoria_nombre = cc.categoria_nombre,
                                            categoria_muestra = cc.categoria_muestra,
                                            categoria_cantidad_equipos = cxt.Equipos.Count(ee => ee.id_categoria == cc.categoria_id && ee.fecha_baja == null)
                                        }
                                        ).ToList();
                Usuario usuariologueado = Session["UsuarioLogueado"] as Usuario;
                if (usuariologueado.perfil == perfil_usuario.Jefe)
                {
                    gv_categorias_view.DataSource = items_categoria;
                    gv_categorias_view.DataBind();
                    gv_categorias.Visible = false;
                }
                else
                {
                    gv_categorias.DataSource = items_categoria;
                    gv_categorias.DataBind();
                    gv_categorias_view.Visible = false;
                }
                
            }
        }

        protected void btn_aceptar_categoria_Click(object sender, EventArgs e)
        {
            Validate("agregar");
            if (IsValid)
            {
                using (var cxt = new Model1Container())
                {
                    Categoria_equipo categoria = new Categoria_equipo() { nombre = tb_nombre_categoria.Value, descripcion = tb_descripcion_categoria.Value, toma_en_cuenta_planilla_costos_horas_hombre = chk_ver_muestra.Checked };

                    cxt.Categorias_equipos.Add(categoria);

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

        private void MostrarPopUpEditarCategoria()
        {
            string script = "<script language=\"javascript\"  type=\"text/javascript\">$(document).ready(function() { $('#editar_categoria').modal('show')});</script>";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "ShowPopUp", script, false);
        }

        protected void btn_aceptar_eliminacion_Click(object sender, EventArgs e)
        {
            int id_categoria = Convert.ToInt32(id_item_por_eliminar.Value);

            using (var cxt = new Model1Container())
            {
                Categoria_equipo categoria = cxt.Categorias_equipos.First(aa => aa.id_categoria == id_categoria);

                if (categoria.Equipos.Count > 0)
                {
                    MessageBox.Show(this, "No se puede eliminar la categoria ya que la misma aún tiene equipos asociados", MessageBox.Tipo_MessageBox.Danger, "Imposible eliminar!");
                }
                else
                {
                    cxt.Categorias_equipos.Remove(categoria);
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
                Categoria_equipo categoria = cxt.Categorias_equipos.First(aa => aa.id_categoria == id_categoria);

                lbl_nombre_categoria.Text = categoria.nombre;
                lbl_descripcion.Text = categoria.descripcion;
                chk_ver_muestra.Checked = categoria.toma_en_cuenta_planilla_costos_horas_hombre.HasValue ? categoria.toma_en_cuenta_planilla_costos_horas_hombre.Value : false; 

                var equipos = (from ee in cxt.Equipos
                               where
                                   ee.fecha_baja == null &&
                                   ee.id_categoria == categoria.id_categoria
                               select new
                               {
                                   nombre_equipo = ee.nombre
                               }).ToList();

                gv_equipos.DataSource = equipos;
                gv_equipos.DataBind();

            }

            MostrarPopUpCategoria();
        }

        protected void btn_editar_ServerClick(object sender, EventArgs e)
        {
            int id_categoria = Convert.ToInt32(((System.Web.UI.HtmlControls.HtmlButton)sender).Attributes["data-id"]);

            using (var cxt = new Model1Container())
            {
                Categoria_equipo categoria = cxt.Categorias_equipos.First(aa => aa.id_categoria == id_categoria);

                tb_editar_nombre_categoria.Value = categoria.nombre;
                tb_editar_descripcion_categoria.Value = categoria.descripcion;
                chk_editar_muestra.Checked = categoria.toma_en_cuenta_planilla_costos_horas_hombre.HasValue ? categoria.toma_en_cuenta_planilla_costos_horas_hombre.Value : false;
                hidden_id_editar_categoria.Value = id_categoria.ToString();
                var equipos = (from ee in cxt.Equipos
                               where
                                   ee.fecha_baja == null &&
                                   ee.id_categoria == categoria.id_categoria
                               select new
                               {
                                   nombre_equipo = ee.nombre
                               }).ToList();

                gv_equipos.DataSource = equipos;
                gv_equipos.DataBind();

            }

            MostrarPopUpEditarCategoria();
        }

        protected void btn_aceptar_edicion_Click(object sender, EventArgs e)
        {
            Validate("editar");
            if (IsValid)
            {
                using (var cxt = new Model1Container())
                {
                    int id_categoria = Convert.ToInt32(hidden_id_editar_categoria.Value);
                    
                    Categoria_equipo categoria = cxt.Categorias_equipos.First(cc => cc.id_categoria == id_categoria);
                    
                    categoria.nombre = tb_editar_nombre_categoria.Value;
                    categoria.descripcion = tb_editar_descripcion_categoria.Value;
                    categoria.toma_en_cuenta_planilla_costos_horas_hombre = chk_editar_muestra.Checked;

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

        protected void gv_categorias_PreRender(object sender, EventArgs e)
        {
            if (gv_categorias.Rows.Count > 0)
            {
                gv_categorias.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if (gv_categorias_view.Rows.Count > 0)
            {
                gv_categorias_view.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if (gv_equipos.Rows.Count > 0)
            {
                gv_equipos.HeaderRow.TableSection = TableRowSection.TableHeader;
            }


        }
    }
}