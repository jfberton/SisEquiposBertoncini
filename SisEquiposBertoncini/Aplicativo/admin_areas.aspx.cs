﻿using SisEquiposBertoncini.Aplicativo.Controles;
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
                                       area_empleados = cxt.Empleados.Count(ee => ee.id_area == aa.area_id)
                                   });

                gv_areas.DataSource = items_areas;
                gv_areas.DataBind();
            }
        }

        private void MostrarPopUpArea()
        {
            string script = "<script language=\"javascript\"  type=\"text/javascript\">$(document).ready(function() { $('#ver_area').modal('show')});</script>";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "ShowPopUp", script, false);
        }

        protected void gv_areas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.ControlStyle.BackColor = Color.LightGray;
            }
        }

        protected void btn_ver_Click(object sender, EventArgs e)
        {
            int id_area = Convert.ToInt32(((System.Web.UI.HtmlControls.HtmlButton)sender).Attributes["data-id"]);

            using (var cxt = new Model1Container())
            {
                Area area = cxt.Areas.First(aa => aa.id_area == id_area);

                lbl_nombre_area.Text = area.nombre;

                var empleados = (from ee in area.Empleados
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

        protected void gv_empleado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.ControlStyle.BackColor = Color.LightGray;
            }
        }

        protected void btn_aceptar_eliminacion_Click(object sender, EventArgs e)
        {
            int id_area = Convert.ToInt32(id_item_por_eliminar.Value);

            using (var cxt = new Model1Container())
            {
                Area area = cxt.Areas.First(aa => aa.id_area == id_area);

                if (area.Empleados.Count > 0)
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
    }
}