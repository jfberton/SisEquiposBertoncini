using SisEquiposBertoncini.Aplicativo.Controles;
using SisEquiposBertoncini.Aplicativo.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SisEquiposBertoncini.Aplicativo
{
    public partial class admin_conceptos_mensuales : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
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

            CargarConceptos();
        }

        private void CargarConceptos()
        {
            List<Item_ingreso_egreso> conceptos = new List<Item_ingreso_egreso>();
            using (var cxt = new Model1Container())
            {
                btn_agregar_rubro_egreso.Visible = cxt.Items_ingresos_egresos.Count(ii => ii.tipo == "Egreso" && ii.id_item_padre == null) == 0;
                btn_agregar_rubro_ingreso.Visible = cxt.Items_ingresos_egresos.Count(ii => ii.tipo == "Ingreso" && ii.id_item_padre == null) == 0;

                conceptos = (from ii in cxt.Items_ingresos_egresos.Include("Hijos")
                             select ii).ToList();
            }

            CrearMostrarTabla(conceptos);

        }

        private void CrearMostrarTabla(List<Item_ingreso_egreso> conceptos)
        {
            var roots = conceptos.Where(ii => ii.id_item_padre == null);

            HtmlGenericControl tree = new HtmlGenericControl("table");
            tree.Attributes.Add("class", "tree table");

            foreach (Item_ingreso_egreso item in roots)
            {
                AgregarNodo(item, tree);
            }

            div_tree.Controls.Clear();
            div_tree.Controls.Add(tree);
        }

        private void AgregarNodo(Item_ingreso_egreso item, HtmlGenericControl tree)
        {
            HtmlGenericControl row = new HtmlGenericControl("tr");
            row.Attributes.Add("class", "treegrid-" + item.id_item + (item.id_item_padre != null ? " treegrid-parent-" + item.id_item_padre : "") + (item.id_item_padre == null ? " h4" : "") + (item.tipo == "Ingreso" ? " alert-success" : " alert-danger"));
            row.Attributes.Add("title", item.descripcion);

            HtmlGenericControl column = new HtmlGenericControl("td");
            column.InnerHtml = (item.Hijos.Count > 0 ? "<strong>" : "") + item.nombre + (item.Hijos.Count > 0 ? "</strong>" : "");
            row.Controls.Add(column);
            tree.Controls.Add(row);


            Usuario usuariologueado = Session["UsuarioLogueado"] as Usuario;
            if (usuariologueado.perfil == perfil_usuario.Admin)
            {
                HtmlGenericControl column_agregar_eliminar = new HtmlGenericControl("td");

                HtmlButton boton_agregar = new HtmlButton();
                boton_agregar.Attributes.Add("runat", "server");
                boton_agregar.CausesValidation = false;
                boton_agregar.ID = "btn_agregar_nivel_" + item.id_item;
                boton_agregar.Attributes.Add("class", "btn btn-xs btn-success");
                boton_agregar.InnerText = "Agregar nivel";
                boton_agregar.ServerClick += boton_agregar_ServerClick;

                HtmlButton boton_eliminar = new HtmlButton();
                boton_eliminar.Attributes.Add("runat", "server");
                boton_eliminar.CausesValidation = false;
                boton_eliminar.ID = "btn_quitar_nivel_" + item.id_item;
                boton_eliminar.Attributes.Add("class", "btn btn-xs btn-danger");
                boton_eliminar.InnerText = "Eliminar nivel";
                boton_eliminar.ServerClick += boton_eliminar_ServerClick;
                column_agregar_eliminar.Controls.Add(boton_eliminar);
                column_agregar_eliminar.Controls.Add(boton_agregar);

                row.Controls.Add(column_agregar_eliminar);
            }

            foreach (Item_ingreso_egreso hijo in item.Hijos.OrderBy(x => x.id_item))
            {
                AgregarNodo(hijo, tree);
            }
        }

        void boton_agregar_ServerClick(object sender, EventArgs e)
        {
            int id_nivel = Convert.ToInt32(((System.Web.UI.HtmlControls.HtmlButton)sender).ID.Replace("btn_agregar_nivel_", ""));

            using (var cxt = new Model1Container())
            {
                Item_ingreso_egreso padre = cxt.Items_ingresos_egresos.FirstOrDefault(ii => ii.id_item == id_nivel);
                if (padre != null)
                {
                    hidden_id_padre.Value = id_nivel.ToString();
                    lbl_concepto_padre.Text = padre.nombre;
                    lbl_tipo_concepto.Text = padre.tipo;

                    string script = "<script language=\"javascript\"  type=\"text/javascript\">$(document).ready(function() { $('#nuevo_concepto').modal('show')});</script>";
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "ShowPopUp", script, false);
                }
            }
        }

        void boton_eliminar_ServerClick(object sender, EventArgs e)
        {
            int id_nivel = Convert.ToInt32(((System.Web.UI.HtmlControls.HtmlButton)sender).ID.Replace("btn_quitar_nivel_", ""));

            using (var cxt = new Model1Container())
            {
                Item_ingreso_egreso item_a_eliminar = cxt.Items_ingresos_egresos.FirstOrDefault(ii => ii.id_item == id_nivel);
                if (item_a_eliminar != null)
                {
                    if (item_a_eliminar.Hijos.Count > 0)
                    {
                        MessageBox.Show(this, "Debe eliminar primero los conceptos que dependen del que quiere eliminar.-", MessageBox.Tipo_MessageBox.Danger);
                    }
                    else
                    {
                        if (item_a_eliminar.Valores_mes.Count > 0)
                        {
                            MessageBox.Show(this, "El concepto ya tiene valores cargados asociados a , no se podrá eliminar", MessageBox.Tipo_MessageBox.Danger);
                        }
                        else
                        {
                            cxt.Items_ingresos_egresos.Remove(item_a_eliminar);
                            cxt.SaveChanges();
                            MessageBox.Show(this, "El concepto fue eliminado correctamente.", MessageBox.Tipo_MessageBox.Success, "Exito!", "../Aplicativo/admin_conceptos_mensuales.aspx");
                        }
                    }
                }
            }



        }

        protected void btn_agregar_rubro_ingreso_ServerClick(object sender, EventArgs e)
        {
            using (var cxt = new Model1Container())
            {
                try
                {
                    Item_ingreso_egreso concepto_ingreso = new Item_ingreso_egreso() { tipo = "Ingreso", nombre = "INGRESOS", descripcion = "Representa el total de los ingresos que produce el equipo" };
                    cxt.Items_ingresos_egresos.Add(concepto_ingreso);
                    cxt.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Error", MessageBox.Tipo_MessageBox.Danger);
                }
            }

            CargarConceptos();
        }

        protected void btn_agregar_rubro_egreso_ServerClick(object sender, EventArgs e)
        {
            using (var cxt = new Model1Container())
            {
                Item_ingreso_egreso concepto_ingreso = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "EGRESOS", descripcion = "Representa el total de los egresos que produce el equipo" };
                cxt.Items_ingresos_egresos.Add(concepto_ingreso);
                cxt.SaveChanges();
                tb_descripcion.Value = string.Empty;
                tb_nombre.Value = string.Empty;
            }

            CargarConceptos();
        }

        protected void btn_guardar_nuevo_concepto_ServerClick(object sender, EventArgs e)
        {
            using (var cxt = new Model1Container())
            {
                try
                {
                    int id_item_padre = 0;

                    if (int.TryParse(hidden_id_padre.Value, out id_item_padre))
                    {
                        Item_ingreso_egreso padre = cxt.Items_ingresos_egresos.FirstOrDefault(ii => ii.id_item == id_item_padre);
                        if (padre != null)
                        {
                            Item_ingreso_egreso concepto_ingreso = new Item_ingreso_egreso() { id_item_padre = id_item_padre, tipo = padre.tipo, nombre = tb_nombre.Value, descripcion = tb_descripcion.Value };
                            cxt.Items_ingresos_egresos.Add(concepto_ingreso);
                            cxt.SaveChanges();

                            tb_descripcion.Value = string.Empty;
                            tb_nombre.Value = string.Empty;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Error", MessageBox.Tipo_MessageBox.Danger);
                }
            }

            CargarConceptos();
        }
    }
}