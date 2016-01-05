using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SisEquiposBertoncini.Aplicativo.Datos;
using System.Web.UI.HtmlControls;
using SisEquiposBertoncini.Aplicativo.Controles;

namespace SisEquiposBertoncini.Aplicativo
{
    public partial class planilla_resumen_horas_trabajadas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDDL_anio();
                btn_nueva_busqueda.Visible = false;
            }
        }

        private void CargarDDL_anio()
        {
            ddl_anio.Items.Clear();
            for (int i = 2015; i <= DateTime.Today.Year; i++)
            {
                ddl_anio.Items.Add(new ListItem(i.ToString()));
            }
        }

        protected void ddl_tipo_empleado_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddl_empleado.Items.Clear();

            if (ddl_tipo_empleado.SelectedItem.Text == "Todos")
            {
                ddl_empleado.Enabled = false;
                ddl_empleado.Items.Add(new ListItem("Todos"));
            }
            else
            {
                if (ddl_tipo_empleado.SelectedItem.Text == "Mecánicos - Pintores")
                {
                    using (var cxt = new Model1Container())
                    {
                        var empleados = (from ee in cxt.Empleados
                                         where
                                            ee.fecha_baja == null &&
                                            (ee.Categoria.nombre == "Mecánico" || ee.Categoria.nombre == "Pintor")
                                         select new
                                         {
                                             value = ee.id_empleado,
                                             text = ee.nombre
                                         }).ToList();

                        ddl_empleado.Items.Add(new ListItem("Todos", "0"));
                        foreach (var item in empleados)
                        {
                            ddl_empleado.Items.Add(new ListItem(item.text, item.value.ToString()));
                        }
                    }
                }
                else
                {
                    using (var cxt = new Model1Container())
                    {
                        var empleados = (from ee in cxt.Empleados
                                         where
                                            ee.fecha_baja == null &&
                                            ee.Categoria.nombre == "Soldador"
                                         select new
                                         {
                                             value = ee.id_empleado,
                                             text = ee.nombre
                                         }).ToList();

                        ddl_empleado.Items.Add(new ListItem("Todos", "0"));
                        foreach (var item in empleados)
                        {
                            ddl_empleado.Items.Add(new ListItem(item.text, item.value.ToString()));
                        }
                    }
                }
            }
        }

        protected void btn_buscar_Click(object sender, EventArgs e)
        {
            btn_buscar.Visible = false;
            btn_nueva_busqueda.Visible = true;
            ddl_anio.Enabled = false;
            ddl_mes.Enabled = false;
            ddl_tipo_empleado.Enabled = false;
            ddl_empleado.Enabled = false;

            using (var cxt = new Model1Container())
            {
                int mes = Convert.ToInt32(ddl_mes.SelectedItem.Value);
                int anio = Convert.ToInt32(ddl_anio.SelectedItem.Value);

                #region Obtengo empleados

                List<Empleado> empleados = new List<Empleado>();

                if (ddl_tipo_empleado.SelectedItem.Text == "Todos")
                {
                    empleados = (from ee in cxt.Empleados
                                 where ee.fecha_baja == null &&
                                       (ee.Categoria.nombre == "Mecánico" ||
                                        ee.Categoria.nombre == "Pintor" ||
                                        ee.Categoria.nombre == "Soldador")
                                 select ee).ToList();
                }
                else
                {
                    if (ddl_tipo_empleado.SelectedItem.Text == "Mecánicos - Pintores")
                    {
                        if (ddl_empleado.SelectedItem.Text == "Todos")
                        {
                            empleados = (from ee in cxt.Empleados
                                         where ee.fecha_baja == null &&
                                               (ee.Categoria.nombre == "Mecánico" ||
                                                ee.Categoria.nombre == "Pintor")
                                         select ee).ToList();
                        }
                        else
                        {
                            int id_empleado = Convert.ToInt32(ddl_empleado.SelectedItem.Value);
                            empleados.Add(cxt.Empleados.FirstOrDefault(ee => ee.id_empleado == id_empleado));
                        }
                    }
                    else
                    {
                        if (ddl_empleado.SelectedItem.Text == "Todos")
                        {
                            empleados = (from ee in cxt.Empleados
                                         where ee.fecha_baja == null &&
                                               ee.Categoria.nombre == "Soldador"
                                         select ee).ToList();
                        }
                        else
                        {
                            int id_empleado = Convert.ToInt32(ddl_empleado.SelectedItem.Value);
                            empleados.Add(cxt.Empleados.FirstOrDefault(ee => ee.id_empleado == id_empleado));
                        }
                    }
                }

                #endregion

                HtmlTable tabla = new HtmlTable();
                tabla.Attributes.Add("class", "table table-bordered");
                HtmlTableRow encabezado = new HtmlTableRow();
                encabezado.Cells.Add(new HtmlTableCell("th") { InnerHtml = "Equipos", BgColor = "lightgray" });
                encabezado.Cells.Add(new HtmlTableCell("th") { InnerHtml = "Horas equipo", BgColor = "lightgray" });
                tabla.Rows.Add(encabezado);

                List<Equipo> equipos = cxt.Equipos.Where(ee => !ee.Generico && ee.fecha_baja == null).ToList();
                string horasTotales = "00:00";
                foreach (Equipo item in equipos)
                {
                    string horas_equipo = item.Horas_mes(mes, anio, empleados);
                    HtmlTableRow fila_equipo = new HtmlTableRow();
                    fila_equipo.Cells.Add(new HtmlTableCell("td") { InnerHtml = item.nombre });
                    fila_equipo.Cells.Add(new HtmlTableCell("td") { Align = "right", InnerHtml = horas_equipo != "00:00" ? horas_equipo : "-" });
                    tabla.Rows.Add(fila_equipo);
                    horasTotales = Horas_string.SumarHoras(new string[] { horasTotales, horas_equipo });
                }

                lbl_horas_totales.Text = horasTotales;

                div_tabla.Controls.Clear();

                div_tabla.Controls.Add(tabla);
            }

        }

        protected void btn_nueva_busqueda_Click(object sender, EventArgs e)
        {
            btn_buscar.Visible = true;
            btn_nueva_busqueda.Visible = false;
            ddl_anio.Enabled = true;
            ddl_mes.Enabled = true;
            ddl_tipo_empleado.Enabled = true;
            ddl_empleado.Enabled = true;
        }

    }
}