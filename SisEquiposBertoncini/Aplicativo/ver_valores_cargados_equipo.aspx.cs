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
    public partial class ver_valores_cargados_equipo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Usuario usuariologueado = Session["UsuarioLogueado"] as Usuario;
            if (usuariologueado == null)
            {
                Response.Redirect("~/Default.aspx?mode=session_end");
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

                    CrearMostrarTabla(id_equipo, anio);
                    Estado_busqueda(false);
                }
            }
            else
            {
                Estado_busqueda(true);
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
            btn_buscar.Visible = habilitado;
        }

        protected void btn_buscar_Click(object sender, EventArgs e)
        {
            int id_equipo = Convert.ToInt32(ddl_equipo.SelectedItem.Value);
            int anio = Convert.ToInt32(ddl_anio.SelectedItem.Value);

            Response.Redirect("~/Aplicativo/ver_valores_cargados_equipo.aspx?eq=" + id_equipo.ToString() + "&a=" + anio.ToString());
        }

        private void CrearMostrarTabla(int id_equipo, int anio)
        {
            using (var cxt = new Model1Container())
            {
                List<Item_ingreso_egreso> conceptos;
                conceptos = cxt.Items_ingresos_egresos.ToList();
                var roots = conceptos.Where(ii => ii.id_item_padre == null);

                HtmlGenericControl tree = new HtmlGenericControl("table");
                tree.Attributes.Add("runat", "server");
                tree.Attributes.Add("class", "tree table");
                tree.ID = "tree";

                #region título

                HtmlGenericControl row = new HtmlGenericControl("tr");
                row.Attributes.Add("class", "treegrid-0");
                row.Attributes.Add("title", "Conceptos");

                HtmlGenericControl column = new HtmlGenericControl("td");
                column.Style.Value = "width:350px;background-color:lightgray";
                column.InnerHtml = "<b>Conceptos</b>";
                row.Controls.Add(column);

                for (int i = 0; i < 12; i++)
                {
                    DateTime d = new DateTime(DateTime.Today.Year, i + 1, 1);

                    HtmlGenericControl column_mes = new HtmlGenericControl("td");
                    column_mes.Style.Value = "width:200px;background-color:lightgray";
                    column_mes.Attributes.Add("align", "center");
                    column_mes.InnerHtml = "<b>" + d.ToString("MMMM") + "</b>";
                    row.Controls.Add(column_mes);
                }

                HtmlGenericControl column_totales = new HtmlGenericControl("td");
                column_totales.Style.Value = "width:250px;background-color:lightgray";
                column_totales.Attributes.Add("align", "center");
                column_totales.InnerHtml = "<b>TOTAL " + ddl_anio.Text + "</b>";
                row.Controls.Add(column_totales);

                HtmlGenericControl column_prom = new HtmlGenericControl("td");
                column_prom.Style.Value = "width:200px;background-color:lightgray";
                column_prom.Attributes.Add("align", "center");
                column_prom.InnerHtml = "<b>Promedio mensual</b>";
                row.Controls.Add(column_prom);

                tree.Controls.Add(row);

                #endregion

                resumen_equipo_anio resumen_equipo_anio = new resumen_equipo_anio(anio, id_equipo);

                foreach (Item_ingreso_egreso item in roots)
                {
                    AgregarNodo(item, tree, cxt, resumen_equipo_anio);
                }

                #region Analisis econimico - financiero

                List<resumen_equipo_anio.resultados_economicos_financieros> resultados = resumen_equipo_anio.analisis_economico_financiero();

                //agregar analisis ecomómico
                #region Titulo analisis ecomómico
                HtmlGenericControl row_ae = new HtmlGenericControl("tr");
                row_ae.Attributes.Add("class", "treegrid-0");
                row_ae.Attributes.Add("title", "Análisis Económico");

                HtmlGenericControl column_ae = new HtmlGenericControl("td");
                column_ae.Style.Value = "width:350px;background-color:lightgray";
                column_ae.InnerHtml = "<b>Análisis Económico</b>";
                row_ae.Controls.Add(column_ae);

                for (int i = 0; i < 12; i++)
                {
                    DateTime d = new DateTime(DateTime.Today.Year, i + 1, 1);

                    HtmlGenericControl column_mes = new HtmlGenericControl("td");
                    column_mes.Style.Value = "width:200px;background-color:lightgray";
                    column_mes.Attributes.Add("align", "center");
                    column_mes.InnerHtml = "<b>" + d.ToString("MMMM") + "</b>";
                    row_ae.Controls.Add(column_mes);
                }

                HtmlGenericControl column_totales_ae = new HtmlGenericControl("td");
                column_totales_ae.Style.Value = "width:250px;background-color:lightgray";
                column_totales_ae.Attributes.Add("align", "center");
                column_totales_ae.InnerHtml = "<b>TOTAL " + ddl_anio.Text + "</b>";
                row_ae.Controls.Add(column_totales_ae);

                HtmlGenericControl column_prom_ae = new HtmlGenericControl("td");
                column_prom_ae.Style.Value = "width:200px;background-color:lightgray";
                column_prom_ae.Attributes.Add("align", "center");
                column_prom_ae.InnerHtml = "<b>Promedio mensual</b>";
                row_ae.Controls.Add(column_prom_ae);

                tree.Controls.Add(row_ae);

                #endregion

                Agregar_fila_analisis_economico_financiero(resumen_equipo_anio.conceptos_analisis_economico_financiero.econo_resultado, resultados, tree);
                Agregar_fila_analisis_economico_financiero(resumen_equipo_anio.conceptos_analisis_economico_financiero.econo_impuesto, resultados, tree);
                Agregar_fila_analisis_economico_financiero(resumen_equipo_anio.conceptos_analisis_economico_financiero.econo_resultado_despues_impuestos, resultados, tree);

                //agregar analisis financiero
                #region Titulo analisis financiero
                HtmlGenericControl row_af = new HtmlGenericControl("tr");
                row_af.Attributes.Add("class", "treegrid-0");
                row_af.Attributes.Add("title", "Análisis Financiero");

                HtmlGenericControl column_af = new HtmlGenericControl("td");
                column_af.Style.Value = "width:350px;background-color:lightgray";
                column_af.InnerHtml = "<b>Análisis Financiero</b>";
                row_af.Controls.Add(column_af);

                for (int i = 0; i < 12; i++)
                {
                    DateTime d = new DateTime(DateTime.Today.Year, i + 1, 1);

                    HtmlGenericControl column_mes = new HtmlGenericControl("td");
                    column_mes.Style.Value = "width:200px;background-color:lightgray";
                    column_mes.Attributes.Add("align", "center");
                    column_mes.InnerHtml = "<b>" + d.ToString("MMMM") + "</b>";
                    row_af.Controls.Add(column_mes);
                }

                HtmlGenericControl column_totales_af = new HtmlGenericControl("td");
                column_totales_af.Style.Value = "width:250px;background-color:lightgray";
                column_totales_af.Attributes.Add("align", "center");
                column_totales_af.InnerHtml = "<b>TOTAL " + ddl_anio.Text + "</b>";
                row_af.Controls.Add(column_totales_af);

                HtmlGenericControl column_prom_af = new HtmlGenericControl("td");
                column_prom_af.Style.Value = "width:200px;background-color:lightgray";
                column_prom_af.Attributes.Add("align", "center");
                column_prom_af.InnerHtml = "<b>Promedio mensual</b>";
                row_af.Controls.Add(column_prom_af);

                tree.Controls.Add(row_af);

                #endregion

                Agregar_fila_analisis_economico_financiero(resumen_equipo_anio.conceptos_analisis_economico_financiero.finan_resultado, resultados, tree);
                Agregar_fila_analisis_economico_financiero(resumen_equipo_anio.conceptos_analisis_economico_financiero.finan_impuesto, resultados, tree);
                Agregar_fila_analisis_economico_financiero(resumen_equipo_anio.conceptos_analisis_economico_financiero.finan_resultado_despues_impuestos, resultados, tree);

                #endregion

                div_tree.Controls.Clear();

                div_tree.Controls.Add(tree);
            }
        }

        private void Agregar_fila_analisis_economico_financiero(resumen_equipo_anio.conceptos_analisis_economico_financiero concepto, List<resumen_equipo_anio.resultados_economicos_financieros> resultados, HtmlGenericControl tree)
        {
            HtmlGenericControl row = new HtmlGenericControl("tr");
            row.Attributes.Add("class", "treegrid-" + concepto.ToString());
            
            //concepto
            HtmlGenericControl column_resultado = new HtmlGenericControl("td");
            switch (concepto)
	        {
		        case resumen_equipo_anio.conceptos_analisis_economico_financiero.econo_resultado:
                    column_resultado.InnerHtml = "Resultado económico";
                 break;
                case resumen_equipo_anio.conceptos_analisis_economico_financiero.econo_impuesto:
                    column_resultado.InnerHtml = " Impuesto Ganancias";
                 break;
                case resumen_equipo_anio.conceptos_analisis_economico_financiero.econo_resultado_despues_impuestos:
                    column_resultado.InnerHtml = " Resultado despues de Imp";
                 break;
                case resumen_equipo_anio.conceptos_analisis_economico_financiero.finan_resultado:
                    column_resultado.InnerHtml = " Resultado Financiero";
                 break;
                case resumen_equipo_anio.conceptos_analisis_economico_financiero.finan_impuesto:
                    column_resultado.InnerHtml = " Impuesto Ganancias";
                 break;
                case resumen_equipo_anio.conceptos_analisis_economico_financiero.finan_resultado_despues_impuestos:
                    column_resultado.InnerHtml = " Resultado despues de Imp";
                 break;
                default:
                 break;
	        }
            
            row.Controls.Add(column_resultado);
            tree.Controls.Add(row);

            //valores mensuales, mes 13 = total, mes 14 = promedio
            for (int i = 0; i < 14; i++)
            {
                resumen_equipo_anio.resultados_economicos_financieros vm = resultados.First(x => x.agrupacion == ((resumen_equipo_anio.agrupaciones)i + 1) && x.tipo == concepto);

                HtmlGenericControl column_valor = new HtmlGenericControl("td");
                column_valor.Attributes.Add("align", "right");

                Label valor = new Label();
                valor.Enabled = false;
                valor.ID = "valor_mes_" + vm.agrupacion.ToString() + "_id_concepto_" + vm.tipo.ToString();
                valor.Attributes.Add("class", "form-control");
                valor.Text = Cadena.Formato_moneda(vm.valor, Cadena.Moneda.pesos);
                column_valor.Controls.Add(valor);

                row.Controls.Add(column_valor);
            }
        }

        private void AgregarNodo(Item_ingreso_egreso concepto, HtmlGenericControl tree, Model1Container cxt, resumen_equipo_anio valores_anuales)
        {
            HtmlGenericControl row = new HtmlGenericControl("tr");
            row.Attributes.Add("class", "treegrid-" + concepto.id_item + (concepto.id_item_padre != null ? " treegrid-parent-" + concepto.id_item_padre : "") + (concepto.id_item_padre == null ? " h4" : "") + (concepto.tipo == "Ingreso" ? " alert-success" : " alert-danger"));
            row.Attributes.Add("title", concepto.descripcion);

            //concepto
            HtmlGenericControl column = new HtmlGenericControl("td");
            column.InnerHtml = (concepto.Hijos.Count > 0 ? "<strong>" : "") + concepto.nombre + (concepto.Hijos.Count > 0 ? "</strong>" : "");
            row.Controls.Add(column);
            tree.Controls.Add(row);

            List<resumen_equipo_anio.valor_item_mes> valores_concepto = valores_anuales.Obtener_agrupacion_por_concepto(concepto.id_item);

            //valores mensuales, mes 13 = total, mes 14 = promedio
            for (int i = 0; i < 14; i++)
            {
                resumen_equipo_anio.valor_item_mes vm = valores_concepto.First(x => x.agrupacion == ((resumen_equipo_anio.agrupaciones)i + 1));

                HtmlGenericControl column_valor = new HtmlGenericControl("td");
                column_valor.Attributes.Add("align", "right");

                Label valor = new Label();
                valor.Enabled = false;
                valor.ID = "valor_mes_" + vm.agrupacion.ToString() + "_id_concepto_" + vm.id_concepto.ToString();
                valor.Attributes.Add("class", "form-control");
                valor.Text = Cadena.Formato_moneda(vm.valor, Cadena.Moneda.pesos);
                column_valor.Controls.Add(valor);

                row.Controls.Add(column_valor);
            }

            foreach (Item_ingreso_egreso hijo in concepto.Hijos)
            {
                AgregarNodo(hijo, tree, cxt, valores_anuales);
            }
        }

        protected void btn_nueva_busqueda_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Aplicativo/ver_valores_cargados_equipo.aspx");
        }
    }
}