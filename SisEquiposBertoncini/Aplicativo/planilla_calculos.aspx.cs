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
    public partial class planilla_calculos : System.Web.UI.Page
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

                lbl_horas_normales.Text = Convert.ToDecimal(Session["planilla_calculos_horas_normales"]).ToString("#,##0.00");
                lbl_horas_extra_50.Text = Convert.ToDecimal(Session["planilla_calculos_horas_extra_50"]).ToString("#,##0.00");
                lbl_horas_extra_100.Text = Convert.ToDecimal(Session["planilla_calculos_horas_extra_100"]).ToString("#,##0.00");
                lbl_horas_totales.Text = (Convert.ToDecimal(Session["planilla_calculos_horas_normales"]) + Convert.ToDecimal(Session["planilla_calculos_horas_extra_50"]) + Convert.ToDecimal(Session["planilla_calculos_horas_extra_100"])).ToString("#,##0.00");

                lbl_costo_hora_seleccionado.Text = Convert.ToDecimal(Session["planilla_calculos_costo_hora_teorico"]).ToString("$ #,##0.00");

                //Session["planilla_calculos_costo_hora_teorico"] = (total_sueldos / dias_mes / cantidad_empleados / horas_dia);
                //Session["planilla_calculos_costo_hora_teorico_ajustado"] = costo_hora_teorico_ajustado;
                //Session["planilla_calculos_costo_hora_real"] = horas_realmente_trabajadas > 0 ? (nueva_masa_salarial / horas_realmente_trabajadas) : 0;

            }

            CrearCargarTabla();
        }

        private struct equipoTabla
        {
            public string categoria { get; set; }
            public int id_equipo { get; set; }
            public string nombre { get; set; }
            public decimal horas_mes { get; set; }
            public decimal porcentaje { get; set; }
            public decimal horas_taller { get; set; }
            public bool reparte_guardia { get; set; }
            public decimal horas_guardia { get; set; }
            public decimal horas_acumuladas { get; set; }
            public decimal monto_horas_acumuladas { get; set; }
        }

        private void CrearCargarTabla()
        {
            int mes = Convert.ToInt32(Session["planilla_calculos_mes"]);
            int anio = Convert.ToInt32(Session["planilla_calculos_anio"]);


            HtmlTable tabla = new HtmlTable();
            tabla.Attributes.Clear();
            tabla.Attributes.Add("class", "table table-bordered");

            HtmlTableRow encabezado_tr1 = new HtmlTableRow();
            encabezado_tr1.Controls.Add(new HtmlTableCell("th") { RowSpan = 3, ColSpan = 2 });
            encabezado_tr1.Controls.Add(new HtmlTableCell("th") { ColSpan = 2, InnerHtml = "HS DIRECTAS EN C/EQUIPO", BgColor = "lightgray" });
            encabezado_tr1.Controls.Add(new HtmlTableCell("th") { InnerHtml = "TALLER", BgColor = "lightgray" });
            encabezado_tr1.Controls.Add(new HtmlTableCell("th") { ColSpan = 2, InnerHtml = "GUARDIA", BgColor = "lightgray" });
            encabezado_tr1.Controls.Add(new HtmlTableCell("th") { RowSpan = 2, InnerHtml = "TOTAL HS ACUM.", BgColor = "lightgray" });
            encabezado_tr1.Controls.Add(new HtmlTableCell("th") { RowSpan = 3, InnerHtml = "MONTO HS ACUM.", BgColor = "lightgray" });
            tabla.Controls.Add(encabezado_tr1);

            HtmlTableRow encabezado_tr2 = new HtmlTableRow();
            encabezado_tr2.Controls.Add(new HtmlTableCell("th") { InnerHtml = "HS", BgColor = "lightgray" });
            encabezado_tr2.Controls.Add(new HtmlTableCell("th") { InnerHtml = "%", BgColor = "lightgray" });
            encabezado_tr2.Controls.Add(new HtmlTableCell("th") { InnerHtml = "Hrs. Varios Taller", BgColor = "lightgray" });
            encabezado_tr2.Controls.Add(new HtmlTableCell("th") { InnerHtml = "CANT EQUIPOS", BgColor = "lightgray" });
            encabezado_tr2.Controls.Add(new HtmlTableCell("th") { InnerHtml = "Hrs. Tot Guardia", BgColor = "lightgray" });
            tabla.Controls.Add(encabezado_tr2);


            using (var cxt = new Model1Container())
            {
                Categoria_equipo camiones_y_carretones = cxt.Categorias_equipos.FirstOrDefault(x => x.nombre == "Camiones y carretones");
                Categoria_equipo gruas = cxt.Categorias_equipos.FirstOrDefault(x => x.nombre == "Gruas");
                Categoria_equipo vehiculos_menores = cxt.Categorias_equipos.FirstOrDefault(x => x.nombre == "Vehículos menores");
                Categoria_equipo ventas = cxt.Categorias_equipos.FirstOrDefault(x => x.nombre == "Ventas");

                List<equipoTabla> filas_camiones_y_carretones = new List<equipoTabla>();
                List<equipoTabla> filas_gruas = new List<equipoTabla>();
                List<equipoTabla> filas_vehiculos_menores = new List<equipoTabla>();
                List<equipoTabla> filas_ventas = new List<equipoTabla>();
                string horas_guardia = cxt.Equipos.First(x => x.nombre == "Guardia").Horas_mes(mes, anio);
                string horas_varios_taller = cxt.Equipos.First(x => x.nombre == "Varios Taller").Horas_mes(mes, anio);

                int equipos_guardia = 0;
                decimal horas_totales_equipos = 0;
                decimal horas_guardia_decimal = Convert.ToDecimal(horas_guardia.Split(':')[0]) + (Convert.ToDecimal(horas_guardia.Split(':')[1]) / Convert.ToDecimal(60));
                decimal horas_varios_taller_decimal = Convert.ToDecimal(horas_varios_taller.Split(':')[0]) + (Convert.ToDecimal(horas_varios_taller.Split(':')[1]) / Convert.ToDecimal(60));

                foreach (Equipo equipo in camiones_y_carretones.Equipos)
                {
                    string horas_mes_equipo = equipo.Horas_mes(mes, anio);
                    equipoTabla et = new equipoTabla();
                    et.categoria = equipo.Categoria.nombre;
                    et.id_equipo = equipo.id_equipo;
                    et.nombre = equipo.nombre;
                    et.horas_mes = Convert.ToDecimal(horas_mes_equipo.Split(':')[0]) + (Convert.ToDecimal(horas_mes_equipo.Split(':')[1]) / Convert.ToDecimal(60));
                    et.reparte_guardia = cxt.Aux_planilla_calculos.FirstOrDefault(x => x.id_equipo == equipo.id_equipo && x.mes == mes && x.anio == anio) != null ? cxt.Aux_planilla_calculos.FirstOrDefault(x => x.id_equipo == equipo.id_equipo && x.mes == mes && x.anio == anio).considera_guardia : false;

                    equipos_guardia = et.reparte_guardia ? equipos_guardia + 1 : equipos_guardia;
                    horas_totales_equipos = horas_totales_equipos + et.horas_mes;

                    filas_camiones_y_carretones.Add(et);
                }

                foreach (Equipo equipo in gruas.Equipos)
                {
                    string horas_mes_equipo = equipo.Horas_mes(mes, anio);
                    equipoTabla et = new equipoTabla();
                    et.categoria = equipo.Categoria.nombre;
                    et.id_equipo = equipo.id_equipo;
                    et.nombre = equipo.nombre;
                    et.horas_mes = Convert.ToDecimal(horas_mes_equipo.Split(':')[0]) + (Convert.ToDecimal(horas_mes_equipo.Split(':')[1]) / Convert.ToDecimal(60));
                    et.reparte_guardia = cxt.Aux_planilla_calculos.FirstOrDefault(x => x.id_equipo == equipo.id_equipo && x.mes == mes && x.anio == anio) != null ? cxt.Aux_planilla_calculos.FirstOrDefault(x => x.id_equipo == equipo.id_equipo && x.mes == mes && x.anio == anio).considera_guardia : false;

                    equipos_guardia = et.reparte_guardia ? equipos_guardia + 1 : equipos_guardia;
                    horas_totales_equipos = horas_totales_equipos + et.horas_mes;

                    filas_gruas.Add(et);
                }

                foreach (Equipo equipo in vehiculos_menores.Equipos)
                {
                    string horas_mes_equipo = equipo.Horas_mes(mes, anio);
                    equipoTabla et = new equipoTabla();
                    et.categoria = equipo.Categoria.nombre;
                    et.id_equipo = equipo.id_equipo;
                    et.nombre = equipo.nombre;
                    et.horas_mes = Convert.ToDecimal(horas_mes_equipo.Split(':')[0]) + (Convert.ToDecimal(horas_mes_equipo.Split(':')[1]) / Convert.ToDecimal(60));
                    et.reparte_guardia = cxt.Aux_planilla_calculos.FirstOrDefault(x => x.id_equipo == equipo.id_equipo && x.mes == mes && x.anio == anio) != null ? cxt.Aux_planilla_calculos.FirstOrDefault(x => x.id_equipo == equipo.id_equipo && x.mes == mes && x.anio == anio).considera_guardia : false;

                    equipos_guardia = et.reparte_guardia ? equipos_guardia + 1 : equipos_guardia;
                    horas_totales_equipos = horas_totales_equipos + et.horas_mes;

                    filas_vehiculos_menores.Add(et);
                }

                foreach (Equipo equipo in ventas.Equipos)
                {
                    string horas_mes_equipo = equipo.Horas_mes(mes, anio);
                    equipoTabla et = new equipoTabla();
                    et.categoria = equipo.Categoria.nombre;
                    et.id_equipo = equipo.id_equipo;
                    et.nombre = equipo.nombre;
                    et.horas_mes = Convert.ToDecimal(horas_mes_equipo.Split(':')[0]) + (Convert.ToDecimal(horas_mes_equipo.Split(':')[1]) / Convert.ToDecimal(60));
                    et.reparte_guardia = cxt.Aux_planilla_calculos.FirstOrDefault(x => x.id_equipo == equipo.id_equipo && x.mes == mes && x.anio == anio) != null ? cxt.Aux_planilla_calculos.FirstOrDefault(x => x.id_equipo == equipo.id_equipo && x.mes == mes && x.anio == anio).considera_guardia : false;

                    equipos_guardia = et.reparte_guardia ? equipos_guardia + 1 : equipos_guardia;
                    horas_totales_equipos = horas_totales_equipos + et.horas_mes;

                    filas_ventas.Add(et);
                }

                HtmlTableRow fila_totales = new HtmlTableRow();
                fila_totales.Controls.Add(new HtmlTableCell("th") { InnerHtml = horas_totales_equipos.ToString("#,##0.00") });
                fila_totales.Controls.Add(new HtmlTableCell("th") { InnerHtml = "100 %" });
                fila_totales.Controls.Add(new HtmlTableCell("th") { InnerHtml = horas_varios_taller_decimal.ToString("#,##0.00") });
                fila_totales.Controls.Add(new HtmlTableCell("th") { InnerHtml = equipos_guardia.ToString() });
                fila_totales.Controls.Add(new HtmlTableCell("th") { InnerHtml = horas_guardia_decimal.ToString("#,##0.00") });
                fila_totales.Controls.Add(new HtmlTableCell("th") { InnerHtml = (horas_totales_equipos + horas_varios_taller_decimal + horas_guardia_decimal).ToString("#,##0.00") });
                tabla.Controls.Add(fila_totales);

                Agregar_a_tabla(tabla, filas_camiones_y_carretones, horas_totales_equipos, horas_varios_taller_decimal, horas_guardia_decimal, equipos_guardia);
                Agregar_a_tabla(tabla, filas_gruas, horas_totales_equipos, horas_varios_taller_decimal, horas_guardia_decimal, equipos_guardia);
                Agregar_a_tabla(tabla, filas_vehiculos_menores, horas_totales_equipos, horas_varios_taller_decimal, horas_guardia_decimal, equipos_guardia);
                Agregar_a_tabla(tabla, filas_ventas, horas_totales_equipos, horas_varios_taller_decimal, horas_guardia_decimal, equipos_guardia);

            }

            div_tabla.Controls.Clear();

            div_tabla.Controls.Add(tabla);
        }

        private void Agregar_a_tabla(HtmlTable tabla, List<equipoTabla> listado, decimal horas_totales_equipos, decimal horas_varios_taller, decimal horas_guardia, int equipos_guardia)
        {
            bool primerfila = true;
            decimal costo_hora = 0;
            switch (ddl_costo_hora.SelectedItem.Text)
            {
                case "Costo hora teórico":
                    costo_hora = Convert.ToDecimal(Session["planilla_calculos_costo_hora_teorico"]);
                    break;

                case "Costo hora teórico ajustado":
                    costo_hora = Convert.ToDecimal(Session["planilla_calculos_costo_hora_teorico_ajustado"]);
                    break;

                case "Costo hora real":
                    costo_hora = Convert.ToDecimal(Session["planilla_calculos_costo_hora_real"]);
                    break;

                default:
                    break;
            }

            foreach (equipoTabla item in listado)
            {
                HtmlTableRow fila_equipo = new HtmlTableRow();

                if (primerfila)
                {
                    primerfila = false;
                    HtmlTableCell columna_categoria = new HtmlTableCell("td") { RowSpan = listado.Count };
                    Label label = new Label();
                    label.CssClass = "rotar";
                    label.Text = item.categoria;
                    columna_categoria.Controls.Add(label);
                    fila_equipo.Controls.Add(columna_categoria);
                }

                decimal porcentaje_equipo_sobre_total = horas_totales_equipos > 0 ? (item.horas_mes / horas_totales_equipos) : 0;
                decimal horas_guardia_equipo = item.reparte_guardia ? horas_guardia / Convert.ToDecimal(equipos_guardia) : 0;

                fila_equipo.Controls.Add(new HtmlTableCell("td") { InnerHtml = item.nombre });
                fila_equipo.Controls.Add(new HtmlTableCell("td") { InnerHtml = item.horas_mes.ToString("#,##0.00") });
                fila_equipo.Controls.Add(new HtmlTableCell("td") { InnerHtml = porcentaje_equipo_sobre_total.ToString("P2") });
                fila_equipo.Controls.Add(new HtmlTableCell("td") { InnerHtml = (horas_varios_taller * porcentaje_equipo_sobre_total).ToString("#,##0.00") });

                CheckBox chk_equipo_guardia = new CheckBox();
                chk_equipo_guardia.Checked = item.reparte_guardia;
                chk_equipo_guardia.ID = "chk_" + item.id_equipo.ToString();
                chk_equipo_guardia.AutoPostBack = true;
                chk_equipo_guardia.CheckedChanged += chk_equipo_guardia_CheckedChanged;

                HtmlTableCell reparte_guardia = new HtmlTableCell("td");
                reparte_guardia.Controls.Add(chk_equipo_guardia);

                fila_equipo.Controls.Add(reparte_guardia);

                fila_equipo.Controls.Add(new HtmlTableCell("td") { InnerHtml = horas_guardia_equipo.ToString("#,##0.00") });
                fila_equipo.Controls.Add(new HtmlTableCell("td") { InnerHtml = (item.horas_mes + (horas_varios_taller * porcentaje_equipo_sobre_total) + horas_guardia_equipo).ToString("#,##0.00") });
                fila_equipo.Controls.Add(new HtmlTableCell("td") { InnerHtml = ((item.horas_mes + (horas_varios_taller * porcentaje_equipo_sobre_total) + horas_guardia_equipo)*costo_hora).ToString("$ #,##0.00") });

                tabla.Controls.Add(fila_equipo);
            }
        }

        void chk_equipo_guardia_CheckedChanged(object sender, EventArgs e)
        {
            int mes = Convert.ToInt32(Session["planilla_calculos_mes"]);
            int anio = Convert.ToInt32(Session["planilla_calculos_anio"]);
            int id_equipo = Convert.ToInt32(((CheckBox)sender).ID.Replace("chk_", ""));
            using (var cxt = new Model1Container())
            {
                Aux_planilla_calculo aux = cxt.Aux_planilla_calculos.FirstOrDefault(x => x.mes == mes && x.anio == anio && x.id_equipo == id_equipo);
                if (aux == null)
                {
                    aux = new Aux_planilla_calculo();
                    aux.anio = anio;
                    aux.mes = mes;
                    aux.id_equipo = id_equipo;

                    cxt.Aux_planilla_calculos.Add(aux);
                }

                aux.considera_guardia = ((CheckBox)sender).Checked;
                cxt.SaveChanges();
            }

            CrearCargarTabla();
        }

        protected void ddl_costo_hora_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbl_costo_hora_seleccionado.Text = Convert.ToDecimal(Session["planilla_calculos_costo_hora_teorico"]).ToString("$ #,##0.00");

            switch (ddl_costo_hora.SelectedItem.Text)
            {
                case "Costo hora teórico":
                    lbl_costo_hora_seleccionado.Text = Convert.ToDecimal(Session["planilla_calculos_costo_hora_teorico"]).ToString("$ #,##0.00");
                    break;

                case "Costo hora teórico ajustado":
                    lbl_costo_hora_seleccionado.Text = Convert.ToDecimal(Session["planilla_calculos_costo_hora_teorico_ajustado"]).ToString("$ #,##0.00");
                    break;

                case "Costo hora real":
                    lbl_costo_hora_seleccionado.Text = Convert.ToDecimal(Session["planilla_calculos_costo_hora_real"]).ToString("$ #,##0.00");
                    break;

                default:
                    break;
            }
        }
    }
}