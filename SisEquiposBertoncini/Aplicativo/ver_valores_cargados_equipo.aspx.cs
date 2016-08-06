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
    public partial class ver_valores_cargados_equipo : System.Web.UI.Page
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

                string str_id_equipo = Request.QueryString["eq"];
                string str_anio = Request.QueryString["a"];

                if (str_id_equipo != null)
                {
                    ddl_equipo.SelectedValue = str_id_equipo;
                    ddl_anio.SelectedValue = str_anio;

                    Estado_busqueda(false);
                    using (var cxt = new Model1Container())
                    {
                        int id_equipo = Convert.ToInt32(str_id_equipo);
                        int anio = Convert.ToInt32(str_anio);

                        //recorro los meses y voy actualizando o creando los ingresos y egresos mensuales
                        int mes_actual = (DateTime.Today.Year > anio) ? 12 : DateTime.Today.Month;
                        int resultado = 0;
                        for (int i = 0; i < mes_actual; i++)
                        {
                            resultado = cxt.Obtener_listado_items_ingreso_egreso_mensual(i + 1, anio, id_equipo);
                        }

                        CrearMostrarTabla(id_equipo, anio);
                        Estado_busqueda(false);
                    }
                }
                else
                {
                    Estado_busqueda(true);
                }
            }
        }

        private void Cargar_ddls()
        {
            using (var cxt = new Model1Container())
            {
                var equipos_habilitados = cxt.Equipos.Where(ee => ee.fecha_baja == null && !ee.Generico);
                foreach (Equipo equipo in equipos_habilitados)
                {
                    ddl_equipo.Items.Add(new ListItem() { Value = equipo.id_equipo.ToString(), Text = equipo.nombre });
                }

                for (int anio = 2015; anio <= DateTime.Today.Year; anio++)
                {
                    ddl_anio.Items.Add(new ListItem() { Value = anio.ToString(), Text = anio.ToString() });
                }
            }
        }

        private void Estado_busqueda(bool habilitado)
        {
            div_buscar_primero.Visible = habilitado;
            div_tree.Visible = !habilitado;

            ddl_equipo.Enabled = habilitado;
            ddl_anio.Enabled = habilitado;

            btn_nueva_busqueda.Visible = !habilitado;
            btn_imprimir_resumen.Visible = !habilitado;
            btn_buscar.Visible = habilitado;
            btn_imprimir.Visible = !habilitado;
        }

        protected void btn_buscar_Click(object sender, EventArgs e)
        {
            int id_equipo = Convert.ToInt32(ddl_equipo.SelectedItem.Value);
            int anio = Convert.ToInt32(ddl_anio.SelectedItem.Value);

            Response.Redirect("~/Aplicativo/ver_valores_cargados_equipo.aspx?eq=" + id_equipo.ToString() + "&a=" + anio.ToString());
        }

        private void CrearMostrarTabla(int id_equipo, int anio)
        {
            Reportes.Valores_anio_equipo ds = new Reportes.Valores_anio_equipo();

            using (var cxt = new Model1Container())
            {

                List<Item_ingreso_egreso> conceptos;
                conceptos = cxt.Items_ingresos_egresos.ToList();
                var roots = conceptos.Where(ii => ii.id_item_padre == null);

                Table tree = new Table();
                tree.Attributes.Add("runat", "server");
                tree.Attributes.Add("class", "tree table");
                tree.ID = "tree";

                #region título

                TableRow row = new TableRow();
                row.TableSection = TableRowSection.TableHeader;
                row.Attributes.Add("class", "treegrid-0");
                row.Attributes.Add("title", "Conceptos");

                TableCell column = new TableCell();
                column.Style.Value = "width:350px;background-color:lightgray";
                column.Text = "<b>Conceptos</b>";
                row.Cells.Add(column);

                for (int i = 0; i < 12; i++)
                {
                    DateTime d = new DateTime(DateTime.Today.Year, i + 1, 1);

                    TableCell column_mes = new TableCell();
                    column_mes.Style.Value = "width:350px;background-color:lightgray";
                    column_mes.Attributes.Add("align", "center");
                    column_mes.Text = "<b>" + d.ToString("MMMM") + "</b>";
                    row.Cells.Add(column_mes);
                }

                TableCell column_totales = new TableCell();
                column_totales.Style.Value = "width:350px;background-color:lightgray";
                column_totales.Attributes.Add("align", "center");
                column_totales.Text = "<b>TOTAL " + ddl_anio.Text + "</b>";
                row.Cells.Add(column_totales);

                TableCell column_prom = new TableCell();
                column_prom.Style.Value = "width:350px;background-color:lightgray";
                column_prom.Attributes.Add("align", "center");
                column_prom.Text = "<b>Promedio mensual</b>";
                row.Cells.Add(column_prom);

                tree.Rows.Add(row);

                #endregion

                resumen_equipo_anio resumen_equipo_anio = new resumen_equipo_anio(anio, id_equipo);

                Reportes.Valores_anio_equipo.Datos_equipoRow der = ds.Datos_equipo.NewDatos_equipoRow();
                der.Nombre = resumen_equipo_anio.Equipo.nombre;
                der.Año = resumen_equipo_anio.Año.ToString();
                ds.Datos_equipo.Rows.Add(der);

                Session["ds_equipo_anio"] = ds;


                foreach (Item_ingreso_egreso item in roots)
                {
                    AgregarNodo(item, tree, cxt, resumen_equipo_anio);
                }

                #region Pie de tabla

                List<resumen_equipo_anio.resultados_economicos_financieros> resultados = resumen_equipo_anio.analisis_economico_financiero();

                //agregar analisis financiero
                #region Titulo analisis financiero
                TableRow row_af = new TableRow();
                row_af.Attributes.Add("class", "treegrid-0");
                row_af.Attributes.Add("title", "Análisis Financiero");

                TableCell column_af = new TableCell();
                column_af.Style.Value = "width:350px;background-color:lightgray";
                column_af.Text = "<b>Análisis Financiero</b>";
                row_af.Cells.Add(column_af);

                for (int i = 0; i < 12; i++)
                {
                    //DateTime d = new DateTime(DateTime.Today.Year, i + 1, 1);

                    TableCell column_mes = new TableCell();
                    column_mes.Style.Value = "width:200px;background-color:lightgray";
                    //column_mes.Attributes.Add("align", "center");
                    //column_mes.Text = "<b>" + d.ToString("MMMM") + "</b>";
                    row_af.Cells.Add(column_mes);
                }

                TableCell column_totales_af = new TableCell();
                column_totales_af.Style.Value = "width:250px;background-color:lightgray";
                //column_totales_af.Attributes.Add("align", "center");
                //column_totales_af.Text = "<b>TOTAL " + ddl_anio.Text + "</b>";
                row_af.Cells.Add(column_totales_af);

                TableCell column_prom_af = new TableCell();
                column_prom_af.Style.Value = "width:200px;background-color:lightgray";
                //column_prom_af.Attributes.Add("align", "center");
                //column_prom_af.Text = "<b>Promedio mensual</b>";
                row_af.Cells.Add(column_prom_af);

                tree.Rows.Add(row_af);

                #endregion

                Agregar_fila_analisis_economico_financiero(resumen_equipo_anio.conceptos_analisis_economico_financiero.finan_resultado, resultados, tree);

                //agregar porcentaje de ganancias
                Agregar_fila_analisis_economico_financiero(resumen_equipo_anio.conceptos_analisis_economico_financiero.porcentaje_de_ganancias, resultados, tree);

                //agregar velocidad de recupero
                Agregar_fila_analisis_economico_financiero(resumen_equipo_anio.conceptos_analisis_economico_financiero.velocidad_de_recupero, resultados, tree);

                #endregion

                div_tree.Controls.Clear();

                div_tree.Controls.Add(tree);
            }
        }

        private void Agregar_fila_analisis_economico_financiero(resumen_equipo_anio.conceptos_analisis_economico_financiero concepto, List<resumen_equipo_anio.resultados_economicos_financieros> resultados, Table tree)
        {
            TableRow row = new TableRow();
            row.Attributes.Add("class", "treegrid-" + concepto.ToString());

            //concepto
            Reportes.Valores_anio_equipo ds = Session["ds_equipo_anio"] as Reportes.Valores_anio_equipo;
            Reportes.Valores_anio_equipo.Detalle_itemRow dir = ds.Detalle_item.NewDetalle_itemRow();

            TableCell column_resultado = new TableCell();
            string tooltip = "";
            switch (concepto)
            {
                case resumen_equipo_anio.conceptos_analisis_economico_financiero.finan_resultado:
                    dir.Nombre_item = "Resultado Financiero";
                    dir.Bold = "SI";

                    column_resultado.Text = "Resultado Financiero";
                    tooltip = "Ingresos totales - Costos Fijos Erogables - Impuestos (5% del ingreso facturado)";
                    break;
                case resumen_equipo_anio.conceptos_analisis_economico_financiero.porcentaje_de_ganancias:
                    dir.Nombre_item = "Porcentaje de ganancias";
                    dir.Bold = "SI";

                    column_resultado.Text = "Porcentaje de ganancias";
                    tooltip = "Resultado financiero / Ingresos";
                    break;
                case resumen_equipo_anio.conceptos_analisis_economico_financiero.velocidad_de_recupero:
                    dir.Nombre_item = "Velocidad de recupero";
                    dir.Bold = "SI";

                    column_resultado.Text = "Velocidad de recupero";
                    tooltip = "Amortización / Resultado financiero";
                    break;
                default:
                    break;
            }

            row.Cells.Add(column_resultado);
            tree.Rows.Add(row);

            //valores mensuales, mes 13 = total, mes 14 = promedio
            for (int i = 0; i < 14; i++)
            {
                resumen_equipo_anio.resultados_economicos_financieros vm = resultados.First(x => x.agrupacion == ((resumen_equipo_anio.agrupaciones)i + 1) && x.tipo == concepto);

                TableCell column_valor = new TableCell();
                column_valor.Attributes.Add("align", "right");

                Label valor = new Label();
                valor.ToolTip = tooltip;
                valor.Enabled = false;
                valor.ID = "valor_mes_" + vm.agrupacion.ToString() + "_id_concepto_" + vm.tipo.ToString();
                valor.Attributes.Add("class", "form-control");
                switch (concepto)
                {
                    case resumen_equipo_anio.conceptos_analisis_economico_financiero.finan_resultado:
                        dir[i + 1] = Cadena.Formato_moneda(vm.valor, Cadena.Moneda.pesos);
                        valor.Text = Cadena.Formato_moneda(vm.valor, Cadena.Moneda.pesos);
                        break;
                    case resumen_equipo_anio.conceptos_analisis_economico_financiero.porcentaje_de_ganancias:
                        dir[i + 1] = Cadena.Formato_porcentaje(vm.valor * Convert.ToDecimal(100));
                        valor.Text = Cadena.Formato_porcentaje(vm.valor * Convert.ToDecimal(100));
                        break;
                    case resumen_equipo_anio.conceptos_analisis_economico_financiero.velocidad_de_recupero:
                        dir[i + 1] = vm.valor.ToString();
                        valor.Text = vm.valor.ToString();
                        break;
                    default:
                        break;
                }

                if (vm.valor < 0)
                {
                    valor.ForeColor = Color.Red;
                }
                column_valor.Controls.Add(valor);

                row.Cells.Add(column_valor);
            }

            ds.Detalle_item.Rows.Add(dir);
            Session["ds_equipo_anio"] = ds;
        }

        private void AgregarNodo(Item_ingreso_egreso concepto, Table tree, Model1Container cxt, resumen_equipo_anio valores_anuales)
        {
            TableRow row = new TableRow();
            row.Attributes.Add("class", "treegrid-" + concepto.id_item + (concepto.id_item_padre != null ? " treegrid-parent-" + concepto.id_item_padre : "") + (concepto.id_item_padre == null ? " h4" : "") + (concepto.tipo == "Ingreso" ? " alert-success" : " alert-danger"));
            row.Attributes.Add("title", concepto.descripcion);

            Reportes.Valores_anio_equipo ds = Session["ds_equipo_anio"] as Reportes.Valores_anio_equipo;
            Reportes.Valores_anio_equipo.Detalle_itemRow dir = ds.Detalle_item.NewDetalle_itemRow();

            //concepto
            TableCell column = new TableCell();

            dir.Nombre_item = concepto.nombre;
            dir.Bold = concepto.Hijos.Count > 0 ? "SI" : "NO";

            column.Text = (concepto.Hijos.Count > 0 ? "<strong>" : "") + concepto.nombre + (concepto.Hijos.Count > 0 ? "</strong>" : "");
            row.Cells.Add(column);


            List<resumen_equipo_anio.valor_item_mes> valores_concepto = valores_anuales.Obtener_agrupacion_por_concepto(concepto.id_item);

            //valores mensuales, mes 13 = total, mes 14 = promedio
            for (int i = 0; i < 14; i++)
            {
                resumen_equipo_anio.valor_item_mes vm = valores_concepto.First(x => x.agrupacion == ((resumen_equipo_anio.agrupaciones)i + 1));

                dir[i + 1] = Cadena.Formato_moneda(vm.valor, Cadena.Moneda.pesos);

                TableCell column_valor = new TableCell();
                column_valor.Attributes.Add("align", "right");

                Label valor = new Label();
                valor.Enabled = false;
                valor.ID = "valor_mes_" + vm.agrupacion.ToString() + "_id_concepto_" + vm.id_concepto.ToString();
                valor.Attributes.Add("class", "form-control");
                valor.Text = Cadena.Formato_moneda(vm.valor, Cadena.Moneda.pesos);
                column_valor.Controls.Add(valor);

                row.Cells.Add(column_valor);
            }

            if (((Label)row.Cells[14].Controls[0]).Text != "$ 0,00")
            {
                tree.Rows.Add(row);
            }

            ds.Detalle_item.Rows.Add(dir);

            Session["ds_equipo_anio"] = ds;

            foreach (Item_ingreso_egreso hijo in concepto.Hijos)
            {
                if ((valores_anuales.Equipo.EsTrabajo.HasValue && valores_anuales.Equipo.EsTrabajo.Value == true && hijo.mostrar_en_trabajo.HasValue && hijo.mostrar_en_trabajo.Value == true) ||
                   (valores_anuales.Equipo.EsTrabajo.HasValue && valores_anuales.Equipo.EsTrabajo.Value == false && hijo.mostrar_en_equipo.HasValue && hijo.mostrar_en_equipo.Value == true))
                {
                    AgregarNodo(hijo, tree, cxt, valores_anuales);
                }
            }
        }

        protected void btn_nueva_busqueda_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Aplicativo/ver_valores_cargados_equipo.aspx");
        }

        protected void btn_imprimir_Click(object sender, EventArgs e)
        {
            Reportes.Valores_anio_equipo ds = Session["ds_equipo_anio"] as Reportes.Valores_anio_equipo;
            RenderReport(ds);
        }

        private void RenderReport(Reportes.Valores_anio_equipo ds)
        {
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.EnableExternalImages = true;

            viewer.LocalReport.ReportPath = Server.MapPath("~/Aplicativo/Reportes/Valores_anio_equipo_r.rdlc");

            ReportDataSource equipo = new ReportDataSource("Equipo", ds.Datos_equipo.Rows);
            ReportDataSource detalle = new ReportDataSource("Detalle", ds.Detalle_item.Rows);

            viewer.LocalReport.DataSources.Add(equipo);
            viewer.LocalReport.DataSources.Add(detalle);

            Microsoft.Reporting.WebForms.Warning[] warnings = null;
            string[] streamids = null;
            string mimeType = null;
            string encoding = null;
            string extension = null;
            string deviceInfo = null;
            byte[] bytes = null;

            deviceInfo = "<DeviceInfo><SimplePageHeaders>True</SimplePageHeaders></DeviceInfo>";

            //Render the report
            bytes = viewer.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
            Session["Reporte"] = bytes;

            string script = "<script type='text/javascript'>window.open('Reportes/Report.aspx');</script>";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);
        }

        protected void btn_imprimir_resumen_Click(object sender, EventArgs e)
        {
            Reportes.Valores_anio_equipo dsresumen = new Reportes.Valores_anio_equipo();
            Reportes.Valores_anio_equipo ds = Session["ds_equipo_anio"] as Reportes.Valores_anio_equipo;

            foreach (Reportes.Valores_anio_equipo.Datos_equipoRow dr in ds.Datos_equipo)
            {
                Reportes.Valores_anio_equipo.Datos_equipoRow dir = dsresumen.Datos_equipo.NewDatos_equipoRow();

                for (int i = 0; i < ds.Tables["Datos_equipo"].Columns.Count; i++)
                {
                    dir[i] = dr[i];
                }

                dsresumen.Datos_equipo.Rows.Add(dir);
            }

            foreach (System.Data.DataRow dr in ds.Detalle_item.Rows)
            {
                if (dr["Nombre_item"].ToString() == "INGRESOS" ||
                    dr["Nombre_item"].ToString() == "EGRESOS" ||
                    dr["Nombre_item"].ToString() == "Resultado Financiero" ||
                    dr["Nombre_item"].ToString() == "Porcentaje de ganancias" ||
                    dr["Nombre_item"].ToString() == "Velocidad de recupero")
                {
                    Reportes.Valores_anio_equipo.Detalle_itemRow dir = dsresumen.Detalle_item.NewDetalle_itemRow();

                    for (int i = 0; i < ds.Tables["Detalle_item"].Columns.Count; i++)
                    {
                        dir[i] = dr[i];
                    }

                    dsresumen.Detalle_item.Rows.Add(dir);
                }
            }

            RenderReport(dsresumen);
        }
    }
}