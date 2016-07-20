using Microsoft.Reporting.WebForms;
using SisEquiposBertoncini.Aplicativo.Controles;
using SisEquiposBertoncini.Aplicativo.Datos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SisEquiposBertoncini.Aplicativo
{
    public partial class planilla_resumen_valores_equipo_categoria : System.Web.UI.Page
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
                    if (usuariologueado.perfil != perfil_usuario.Admin && usuariologueado.perfil != perfil_usuario.Jefe)
                    {
                        Response.Redirect("~/Default.aspx?mode=trucho");
                    }
                }

                Cargar_ddls();
                Estado_busqueda(true);

                //string str_id_categoria = Request.QueryString["cat"];
                //string str_anio = Request.QueryString["a"];

                //if (str_id_categoria != null)
                //{
                //    ddl_categoria.SelectedValue = str_id_categoria;
                //    ddl_anio.SelectedValue = str_anio;

                //    Estado_busqueda(false);
                //    using (var cxt = new Model1Container())
                //    {
                //        int id_categoria = Convert.ToInt32(str_id_categoria);
                //        int anio = Convert.ToInt32(str_anio);

                //        CrearMostrarTabla(id_categoria, anio);
                //        Estado_busqueda(false);
                //    }
                //}
                //else
                //{
                //    Estado_busqueda(true);
                //}
            }
        }

        private void Cargar_ddls()
        {
            using (var cxt = new Model1Container())
            {
                var categorias = cxt.Categorias_equipos;
                foreach (Categoria_equipo categoria in categorias)
                {
                    ddl_categoria.Items.Add(new ListItem() { Value = categoria.id_categoria.ToString(), Text = categoria.nombre });
                }

                for (int anio = 2015; anio <= DateTime.Today.Year; anio++)
                {
                    ddl_anio.Items.Add(new ListItem() { Value = anio.ToString(), Text = anio.ToString() });
                }
            }
        }

        private void Estado_busqueda(bool habilitado)
        {
            ddl_categoria.Enabled = habilitado;
            ddl_anio.Enabled = habilitado;
            if (habilitado)
            {
                gv_detalle_equipos.DataSource = null;
                gv_detalle_equipos.DataBind();
            }
            btn_nueva_busqueda.Visible = !habilitado;
            //btn_imprimir_resumen.Visible = !habilitado;
            btn_buscar.Visible = habilitado;
            //btn_imprimir.Visible = !habilitado;
        }

        protected void btn_buscar_Click(object sender, EventArgs e)
        {
            int id_equipo = Convert.ToInt32(ddl_categoria.SelectedItem.Value);
            int anio = Convert.ToInt32(ddl_anio.SelectedItem.Value);

           // Response.Redirect("~/Aplicativo/planilla_resumen_valores_equipo_categoria.aspx?cat=" + id_equipo.ToString() + "&a=" + anio.ToString());
            CrearMostrarTabla(id_equipo, anio);
        }

        private struct item_grilla_detalle
        {
            public int id_equipo { get; set; }
            public string nombre_equipo { get; set; }
            public string tipo_resultado { get; set; }
            public string enero { get; set; }
            public string febrero { get; set; }
            public string marzo { get; set; }
            public string abril { get; set; }
            public string mayo { get; set; }
            public string junio { get; set; }
            public string julio { get; set; }
            public string agosto { get; set; }
            public string septiembre { get; set; }
            public string octubre { get; set; }
            public string noviembre { get; set; }
            public string diciembre { get; set; }
            public string total { get; set; }
            public string promedio { get; set; }

        }


        struct Equipo_categoria
        {
            public Equipo Equipo { get; set; }
            public List<SisEquiposBertoncini.Aplicativo.Datos.resumen_equipo_anio.resultados_economicos_financieros> resultados { get; set; }
        }

        private void CrearMostrarTabla(int id_categoria, int anio)
        {
            Reportes.Valores_anio_equipo ds = new Reportes.Valores_anio_equipo();

            using (var cxt = new Model1Container())
            {
                var equipos = cxt.Equipos.Where(ee => ee.id_categoria == id_categoria && ee.fecha_baja == null);
                List<Equipo_categoria> equipos_categoria = new List<Equipo_categoria>();

                foreach (Equipo equipo in equipos)
                {
                    resumen_equipo_anio rea = new resumen_equipo_anio(anio, equipo.id_equipo);
                    
                    Equipo_categoria item = new Equipo_categoria()
                    {
                        Equipo = equipo,
                        resultados = rea.analisis_economico_financiero()
                    };

                    equipos_categoria.Add(item);
                }

                List<item_grilla_detalle> items_tabla = new List<item_grilla_detalle>();

                foreach (Equipo_categoria item in equipos_categoria)
                {
                    item_grilla_detalle item_equipo_resultadoFinanciero = new item_grilla_detalle();
                    item_equipo_resultadoFinanciero.id_equipo = item.Equipo.id_equipo;
                    item_equipo_resultadoFinanciero.nombre_equipo = item.Equipo.nombre;
                    item_equipo_resultadoFinanciero.tipo_resultado = "Resultado Financiero";
                    item_equipo_resultadoFinanciero.enero = Cadena.Formato_moneda(item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.finan_resultado && rr.agrupacion == resumen_equipo_anio.agrupaciones.enero).valor, Cadena.Moneda.pesos);
                    item_equipo_resultadoFinanciero.febrero = Cadena.Formato_moneda(item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.finan_resultado && rr.agrupacion == resumen_equipo_anio.agrupaciones.febrero).valor, Cadena.Moneda.pesos);
                    item_equipo_resultadoFinanciero.marzo = Cadena.Formato_moneda(item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.finan_resultado && rr.agrupacion == resumen_equipo_anio.agrupaciones.marzo).valor, Cadena.Moneda.pesos);
                    item_equipo_resultadoFinanciero.abril = Cadena.Formato_moneda(item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.finan_resultado && rr.agrupacion == resumen_equipo_anio.agrupaciones.abril).valor, Cadena.Moneda.pesos);
                    item_equipo_resultadoFinanciero.mayo = Cadena.Formato_moneda(item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.finan_resultado && rr.agrupacion == resumen_equipo_anio.agrupaciones.mayo).valor, Cadena.Moneda.pesos);
                    item_equipo_resultadoFinanciero.junio = Cadena.Formato_moneda(item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.finan_resultado && rr.agrupacion == resumen_equipo_anio.agrupaciones.junio).valor, Cadena.Moneda.pesos);
                    item_equipo_resultadoFinanciero.julio = Cadena.Formato_moneda(item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.finan_resultado && rr.agrupacion == resumen_equipo_anio.agrupaciones.julio).valor, Cadena.Moneda.pesos);
                    item_equipo_resultadoFinanciero.agosto = Cadena.Formato_moneda(item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.finan_resultado && rr.agrupacion == resumen_equipo_anio.agrupaciones.agosto).valor, Cadena.Moneda.pesos);
                    item_equipo_resultadoFinanciero.septiembre = Cadena.Formato_moneda(item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.finan_resultado && rr.agrupacion == resumen_equipo_anio.agrupaciones.septiembre).valor, Cadena.Moneda.pesos);
                    item_equipo_resultadoFinanciero.octubre = Cadena.Formato_moneda(item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.finan_resultado && rr.agrupacion == resumen_equipo_anio.agrupaciones.octubre).valor, Cadena.Moneda.pesos);
                    item_equipo_resultadoFinanciero.noviembre = Cadena.Formato_moneda(item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.finan_resultado && rr.agrupacion == resumen_equipo_anio.agrupaciones.noviembre).valor, Cadena.Moneda.pesos);
                    item_equipo_resultadoFinanciero.diciembre = Cadena.Formato_moneda(item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.finan_resultado && rr.agrupacion == resumen_equipo_anio.agrupaciones.diciembre).valor, Cadena.Moneda.pesos);
                    item_equipo_resultadoFinanciero.total = Cadena.Formato_moneda(item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.finan_resultado && rr.agrupacion == resumen_equipo_anio.agrupaciones.total_anio).valor, Cadena.Moneda.pesos);
                    item_equipo_resultadoFinanciero.promedio = Cadena.Formato_moneda(item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.finan_resultado && rr.agrupacion == resumen_equipo_anio.agrupaciones.promedio_mensual).valor, Cadena.Moneda.pesos);

                    item_grilla_detalle item_equipo_Porcentaje= new item_grilla_detalle();
                    item_equipo_Porcentaje.id_equipo = item.Equipo.id_equipo;
                    item_equipo_Porcentaje.nombre_equipo = "";
                    item_equipo_Porcentaje.tipo_resultado = "Porcentaje de ganancias";
                    item_equipo_Porcentaje.enero = Cadena.Formato_porcentaje(item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.porcentaje_de_ganancias && rr.agrupacion == resumen_equipo_anio.agrupaciones.enero).valor);
                    item_equipo_Porcentaje.febrero = Cadena.Formato_porcentaje(item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.porcentaje_de_ganancias && rr.agrupacion == resumen_equipo_anio.agrupaciones.febrero).valor);
                    item_equipo_Porcentaje.marzo = Cadena.Formato_porcentaje(item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.porcentaje_de_ganancias && rr.agrupacion == resumen_equipo_anio.agrupaciones.marzo).valor);
                    item_equipo_Porcentaje.abril = Cadena.Formato_porcentaje(item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.porcentaje_de_ganancias && rr.agrupacion == resumen_equipo_anio.agrupaciones.abril).valor);
                    item_equipo_Porcentaje.mayo = Cadena.Formato_porcentaje(item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.porcentaje_de_ganancias && rr.agrupacion == resumen_equipo_anio.agrupaciones.mayo).valor);
                    item_equipo_Porcentaje.junio = Cadena.Formato_porcentaje(item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.porcentaje_de_ganancias && rr.agrupacion == resumen_equipo_anio.agrupaciones.junio).valor);
                    item_equipo_Porcentaje.julio = Cadena.Formato_porcentaje(item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.porcentaje_de_ganancias && rr.agrupacion == resumen_equipo_anio.agrupaciones.julio).valor);
                    item_equipo_Porcentaje.agosto = Cadena.Formato_porcentaje(item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.porcentaje_de_ganancias && rr.agrupacion == resumen_equipo_anio.agrupaciones.agosto).valor);
                    item_equipo_Porcentaje.septiembre = Cadena.Formato_porcentaje(item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.porcentaje_de_ganancias && rr.agrupacion == resumen_equipo_anio.agrupaciones.septiembre).valor);
                    item_equipo_Porcentaje.octubre = Cadena.Formato_porcentaje(item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.porcentaje_de_ganancias && rr.agrupacion == resumen_equipo_anio.agrupaciones.octubre).valor);
                    item_equipo_Porcentaje.noviembre = Cadena.Formato_porcentaje(item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.porcentaje_de_ganancias && rr.agrupacion == resumen_equipo_anio.agrupaciones.noviembre).valor);
                    item_equipo_Porcentaje.diciembre = Cadena.Formato_porcentaje(item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.porcentaje_de_ganancias && rr.agrupacion == resumen_equipo_anio.agrupaciones.diciembre).valor);
                    item_equipo_Porcentaje.total = Cadena.Formato_porcentaje(item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.porcentaje_de_ganancias && rr.agrupacion == resumen_equipo_anio.agrupaciones.total_anio).valor);
                    item_equipo_Porcentaje.promedio = Cadena.Formato_porcentaje(item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.porcentaje_de_ganancias && rr.agrupacion == resumen_equipo_anio.agrupaciones.promedio_mensual).valor);

                    item_grilla_detalle item_equipo_Velocidad= new item_grilla_detalle();
                    item_equipo_Velocidad.id_equipo = item.Equipo.id_equipo;
                    item_equipo_Velocidad.nombre_equipo = "";
                    item_equipo_Velocidad.tipo_resultado = "Velocidad de recupero";
                    item_equipo_Velocidad.enero = item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.velocidad_de_recupero && rr.agrupacion == resumen_equipo_anio.agrupaciones.enero).valor.ToString();
                    item_equipo_Velocidad.febrero = item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.velocidad_de_recupero && rr.agrupacion == resumen_equipo_anio.agrupaciones.febrero).valor.ToString();
                    item_equipo_Velocidad.marzo = item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.velocidad_de_recupero && rr.agrupacion == resumen_equipo_anio.agrupaciones.marzo).valor.ToString();
                    item_equipo_Velocidad.abril = item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.velocidad_de_recupero && rr.agrupacion == resumen_equipo_anio.agrupaciones.abril).valor.ToString();
                    item_equipo_Velocidad.mayo = item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.velocidad_de_recupero && rr.agrupacion == resumen_equipo_anio.agrupaciones.mayo).valor.ToString();
                    item_equipo_Velocidad.junio = item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.velocidad_de_recupero && rr.agrupacion == resumen_equipo_anio.agrupaciones.junio).valor.ToString();
                    item_equipo_Velocidad.julio = item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.velocidad_de_recupero && rr.agrupacion == resumen_equipo_anio.agrupaciones.julio).valor.ToString();
                    item_equipo_Velocidad.agosto = item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.velocidad_de_recupero && rr.agrupacion == resumen_equipo_anio.agrupaciones.agosto).valor.ToString();
                    item_equipo_Velocidad.septiembre = item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.velocidad_de_recupero && rr.agrupacion == resumen_equipo_anio.agrupaciones.septiembre).valor.ToString();
                    item_equipo_Velocidad.octubre = item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.velocidad_de_recupero && rr.agrupacion == resumen_equipo_anio.agrupaciones.octubre).valor.ToString();
                    item_equipo_Velocidad.noviembre = item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.velocidad_de_recupero && rr.agrupacion == resumen_equipo_anio.agrupaciones.noviembre).valor.ToString();
                    item_equipo_Velocidad.diciembre = item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.velocidad_de_recupero && rr.agrupacion == resumen_equipo_anio.agrupaciones.diciembre).valor.ToString();
                    item_equipo_Velocidad.total = item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.velocidad_de_recupero && rr.agrupacion == resumen_equipo_anio.agrupaciones.total_anio).valor.ToString();
                    item_equipo_Velocidad.promedio = item.resultados.First(rr => rr.tipo == resumen_equipo_anio.conceptos_analisis_economico_financiero.velocidad_de_recupero && rr.agrupacion == resumen_equipo_anio.agrupaciones.promedio_mensual).valor.ToString();

                    items_tabla.Add(item_equipo_resultadoFinanciero);
                    items_tabla.Add(item_equipo_Porcentaje);
                    items_tabla.Add(item_equipo_Velocidad);
                }

                gv_detalle_equipos.DataSource = items_tabla;
                gv_detalle_equipos.DataBind();

            }

            Estado_busqueda(false);
        }

        protected void btn_nueva_busqueda_Click(object sender, EventArgs e)
        {
            Estado_busqueda(true);
        }

        protected void btn_imprimir_Click(object sender, EventArgs e)
        {
            //Reportes.Valores_anio_equipo ds = Session["ds_equipo_anio"] as Reportes.Valores_anio_equipo;
            //RenderReport(ds);
        }

        private void RenderReport(Reportes.Valores_anio_equipo ds)
        {
            //ReportViewer viewer = new ReportViewer();
            //viewer.ProcessingMode = ProcessingMode.Local;
            //viewer.LocalReport.EnableExternalImages = true;

            //viewer.LocalReport.ReportPath = Server.MapPath("~/Aplicativo/Reportes/Valores_anio_equipo_r.rdlc");

            //ReportDataSource equipo = new ReportDataSource("Equipo", ds.Datos_equipo.Rows);
            //ReportDataSource detalle = new ReportDataSource("Detalle", ds.Detalle_item.Rows);

            //viewer.LocalReport.DataSources.Add(equipo);
            //viewer.LocalReport.DataSources.Add(detalle);

            //Microsoft.Reporting.WebForms.Warning[] warnings = null;
            //string[] streamids = null;
            //string mimeType = null;
            //string encoding = null;
            //string extension = null;
            //string deviceInfo = null;
            //byte[] bytes = null;

            //deviceInfo = "<DeviceInfo><SimplePageHeaders>True</SimplePageHeaders></DeviceInfo>";

            ////Render the report
            //bytes = viewer.LocalReport.Render("PDF", deviceInfo, out  mimeType, out encoding, out extension, out streamids, out warnings);
            //Session["Reporte"] = bytes;

            //string script = "<script type='text/javascript'>window.open('Reportes/Report.aspx');</script>";
            //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);
        }

        protected void btn_imprimir_resumen_Click(object sender, EventArgs e)
        {
            //Reportes.Valores_anio_equipo dsresumen = new Reportes.Valores_anio_equipo();
            //Reportes.Valores_anio_equipo ds = Session["ds_equipo_anio"] as Reportes.Valores_anio_equipo;

            //foreach (Reportes.Valores_anio_equipo.Datos_equipoRow dr in ds.Datos_equipo)
            //{
            //    Reportes.Valores_anio_equipo.Datos_equipoRow dir = dsresumen.Datos_equipo.NewDatos_equipoRow();

            //    for (int i = 0; i < ds.Tables["Datos_equipo"].Columns.Count; i++)
            //    {
            //        dir[i] = dr[i];
            //    }

            //    dsresumen.Datos_equipo.Rows.Add(dir);
            //}

            //foreach (System.Data.DataRow dr in ds.Detalle_item.Rows)
            //{
            //    if (dr["Nombre_item"].ToString() == "INGRESOS" ||
            //        dr["Nombre_item"].ToString() == "EGRESOS" ||
            //        dr["Nombre_item"].ToString() == "Resultado Financiero" ||
            //        dr["Nombre_item"].ToString() == "Porcentaje de ganancias" ||
            //        dr["Nombre_item"].ToString() == "Velocidad de recupero")
            //    {
            //        Reportes.Valores_anio_equipo.Detalle_itemRow dir = dsresumen.Detalle_item.NewDetalle_itemRow();

            //        for (int i = 0; i < ds.Tables["Detalle_item"].Columns.Count; i++)
            //        {
            //            dir[i] = dr[i];
            //        }

            //        dsresumen.Detalle_item.Rows.Add(dir);
            //    }
            //}

            //RenderReport(dsresumen);
        }

        protected void gv_detalle_equipos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.ControlStyle.BackColor = Color.LightGray;
            }
        }


    }
}