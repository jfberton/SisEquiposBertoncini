﻿using SisEquiposBertoncini.Aplicativo.Controles;
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
            }

            CrearCargarTabla();
        }

        private struct equipo_tabla
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

        private struct total_categoria_tabla
        {
            public string categoria { get; set; }
            public decimal horas { get; set; }
            public decimal monto { get; set; }
        }

        private void CrearCargarTabla()
        {
            int mes = Convert.ToInt32(Session["planilla_calculos_mes"]);
            int anio = Convert.ToInt32(Session["planilla_calculos_anio"]);
            string categoria_empleado = Session["planilla_calculos_categoria_empleado"].ToString();

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

            using (var cxt = new Model1Container())
            {
                Categoria_equipo camiones_y_carretones = cxt.Categorias_equipos.FirstOrDefault(x => x.nombre == "Camiones y carretones");
                Categoria_equipo gruas = cxt.Categorias_equipos.FirstOrDefault(x => x.nombre == "Gruas");
                Categoria_equipo vehiculos_menores = cxt.Categorias_equipos.FirstOrDefault(x => x.nombre == "Vehículos menores");
                Categoria_equipo ventas = cxt.Categorias_equipos.FirstOrDefault(x => x.nombre == "Ventas");
                Categoria_equipo trabajos_particulares = cxt.Categorias_equipos.FirstOrDefault(x => x.nombre == "Trabajos particulares");
                Categoria_equipo otros = cxt.Categorias_equipos.FirstOrDefault(x => x.nombre == "Otros");

                List<equipo_tabla> filas_camiones_y_carretones = new List<equipo_tabla>();
                List<equipo_tabla> filas_gruas = new List<equipo_tabla>();
                List<equipo_tabla> filas_vehiculos_menores = new List<equipo_tabla>();
                List<equipo_tabla> filas_ventas = new List<equipo_tabla>();
                List<equipo_tabla> filas_trabajos_particulares = new List<equipo_tabla>();
                List<equipo_tabla> filas_otros = new List<equipo_tabla>();

                List<total_categoria_tabla> filas_tabla_total_categoria = new List<total_categoria_tabla>();
                Session["filas_tabla_total_categoria"] = filas_tabla_total_categoria;

                string horas_guardia = cxt.Equipos.First(x => x.nombre == "Guardia").Horas_mes(mes, anio, categoria_empleado);
                string horas_varios_taller = cxt.Equipos.First(x => x.nombre == "Varios Taller").Horas_mes(mes, anio, categoria_empleado);

                //valores totales generales
                int equipos_guardia = 0;
                decimal horas_totales_equipos = 0;
                decimal horas_guardia_decimal = Convert.ToDecimal(horas_guardia.Split(':')[0]) + (Convert.ToDecimal(horas_guardia.Split(':')[1]) / Convert.ToDecimal(60));
                decimal horas_varios_taller_decimal = Convert.ToDecimal(horas_varios_taller.Split(':')[0]) + (Convert.ToDecimal(horas_varios_taller.Split(':')[1]) / Convert.ToDecimal(60));
                Session["totales_equipos_guardia"] = equipos_guardia;
                Session["totales_horas_totales_equipos"] = horas_totales_equipos;
                Session["totales_horas_guardia_decimal"] = horas_guardia_decimal;
                Session["totales_horas_varios_taller_decimal"] = horas_varios_taller_decimal;

                CargarFilasCategoria(camiones_y_carretones, filas_camiones_y_carretones, mes, anio, costo_hora);
                CargarFilasCategoria(gruas, filas_gruas, mes, anio, costo_hora);
                CargarFilasCategoria(vehiculos_menores, filas_vehiculos_menores, mes, anio, costo_hora);
                CargarFilasCategoria(ventas, filas_ventas, mes, anio, costo_hora);
                CargarFilasCategoria(trabajos_particulares, filas_trabajos_particulares, mes, anio, costo_hora);
                CargarFilasCategoria(otros, filas_otros, mes, anio, costo_hora);

                //recupero los totales cargados en los llamados anteriores
                equipos_guardia = Convert.ToInt32(Session["totales_equipos_guardia"]);
                horas_totales_equipos = Convert.ToDecimal(Session["totales_horas_totales_equipos"]);
                horas_guardia_decimal = Convert.ToDecimal(Session["totales_horas_guardia_decimal"]);
                horas_varios_taller_decimal = Convert.ToDecimal(Session["totales_horas_varios_taller_decimal"]);

                HtmlTableRow fila_totales = new HtmlTableRow();
                fila_totales.Controls.Add(new HtmlTableCell("th") { InnerHtml = horas_totales_equipos.ToString("#,##0.00") });
                fila_totales.Controls.Add(new HtmlTableCell("th") { InnerHtml = "100 %" });
                fila_totales.Controls.Add(new HtmlTableCell("th") { InnerHtml = horas_varios_taller_decimal.ToString("#,##0.00") });
                fila_totales.Controls.Add(new HtmlTableCell("th") { InnerHtml = equipos_guardia.ToString() });
                fila_totales.Controls.Add(new HtmlTableCell("th") { InnerHtml = horas_guardia_decimal.ToString("#,##0.00") });
                fila_totales.Controls.Add(new HtmlTableCell("th") { InnerHtml = (horas_totales_equipos + horas_varios_taller_decimal + horas_guardia_decimal).ToString("#,##0.00") });
                tabla.Controls.Add(fila_totales);

                Agregar_a_tabla(tabla, filas_camiones_y_carretones, horas_totales_equipos, horas_varios_taller_decimal, horas_guardia_decimal, equipos_guardia, costo_hora);
                Agregar_a_tabla(tabla, filas_gruas, horas_totales_equipos, horas_varios_taller_decimal, horas_guardia_decimal, equipos_guardia, costo_hora);
                Agregar_a_tabla(tabla, filas_vehiculos_menores, horas_totales_equipos, horas_varios_taller_decimal, horas_guardia_decimal, equipos_guardia, costo_hora);
                Agregar_a_tabla(tabla, filas_ventas, horas_totales_equipos, horas_varios_taller_decimal, horas_guardia_decimal, equipos_guardia, costo_hora);
                Agregar_a_tabla(tabla, filas_trabajos_particulares, horas_totales_equipos, horas_varios_taller_decimal, horas_guardia_decimal, equipos_guardia, costo_hora);
                Agregar_a_tabla(tabla, filas_otros, horas_totales_equipos, horas_varios_taller_decimal, horas_guardia_decimal, equipos_guardia, costo_hora);

                Cargar_tabla_resumen(horas_totales_equipos);
            }

            div_tabla.Controls.Clear();

            div_tabla.Controls.Add(tabla);

            if (categoria_empleado == "Mecánicos - Pintores")
            {
                Cargar_tabla_trabajos_out();
            }

            Cargar_tabla_final_planilla_calculos();
        }

        private void Cargar_tabla_trabajos_out()
        {
            int mes = Convert.ToInt32(Session["planilla_calculos_mes"]);
            int anio = Convert.ToInt32(Session["planilla_calculos_anio"]);
            string categoria_empleado = Session["planilla_calculos_categoria_empleado"].ToString();

            using (var cxt = new Model1Container())
            {
                List<Equipo> equipos = cxt.Equipos.Where(ee => ee.OUT && !ee.Generico && ee.fecha_baja == null).ToList();

                HtmlTable table = new HtmlTable();
                table.Attributes.Add("class", "table table-bordered");
                HtmlTableRow fila_encabezado = new HtmlTableRow();
                fila_encabezado.Controls.Add(new HtmlTableCell("th") { InnerText = "Trabajos out", BgColor = "lightgray" });
                fila_encabezado.Controls.Add(new HtmlTableCell("th") { InnerText = "Dias", BgColor = "lightgray" });
                table.Controls.Add(fila_encabezado);
                decimal dias_totales_out = 0;

                foreach (Equipo equipo in equipos)
                {
                    HtmlTableRow fila_trabajo_out = new HtmlTableRow();
                   
                    string horas_mes_equipo = equipo.Horas_mes(mes, anio, categoria_empleado);
                    decimal horas_out = Convert.ToDecimal(horas_mes_equipo.Split(':')[0]) + (Convert.ToDecimal(horas_mes_equipo.Split(':')[1]) / Convert.ToDecimal(60));
                    decimal dias_out = horas_out/8;
                    dias_totales_out = dias_out + dias_totales_out;

                    fila_trabajo_out.Controls.Add(new HtmlTableCell("th") { InnerText = equipo.nombre, BgColor = "lightgray" });
                    fila_trabajo_out.Controls.Add(new HtmlTableCell("th") { InnerText = dias_out.ToString("#,##0.00 'días.'") });

                    table.Controls.Add(fila_trabajo_out);
                }

                HtmlTableRow fila_pie = new HtmlTableRow();
                fila_pie.Controls.Add(new HtmlTableCell("th") { InnerText = "TOTAL", BgColor = "lightgray" });
                fila_pie.Controls.Add(new HtmlTableCell("th") { InnerText = dias_totales_out.ToString("#,##0.00 'días.'"), BgColor = "lightgray" });

                table.Controls.Add(fila_pie);

                div_tabla_OUT.Controls.Clear();
                div_tabla_OUT.Controls.Add(table);
            }
        }

        private void Cargar_tabla_resumen(decimal horas_totales_equipos)
        {
            HtmlTable tabla = new HtmlTable();
            tabla.Attributes.Add("class", "table table-bordered");
            HtmlTableRow fila_encabezado = new HtmlTableRow();
            fila_encabezado.Controls.Add(new HtmlTableCell("th") { InnerHtml = "Categoria", BgColor = "lightgray" });
            fila_encabezado.Controls.Add(new HtmlTableCell("th") { InnerHtml = "Horas", BgColor = "lightgray" });
            fila_encabezado.Controls.Add(new HtmlTableCell("th") { InnerHtml = "%", BgColor = "lightgray" });
            fila_encabezado.Controls.Add(new HtmlTableCell("th") { InnerHtml = "Monto", BgColor = "lightgray" });
            tabla.Controls.Add(fila_encabezado);

            List<total_categoria_tabla> filas_tabla_total_categoria = Session["filas_tabla_total_categoria"] as List<total_categoria_tabla>;


            foreach (total_categoria_tabla item in filas_tabla_total_categoria.OrderByDescending(x => x.horas))
            {
                HtmlTableRow fila_categoria = new HtmlTableRow();
                fila_categoria.Controls.Add(new HtmlTableCell("th") { InnerHtml = item.categoria, BgColor = "lightgray" });
                fila_categoria.Controls.Add(new HtmlTableCell("td") { InnerHtml = item.horas.ToString("#,##0.00") });
                fila_categoria.Controls.Add(new HtmlTableCell("td") { InnerHtml = (item.horas / horas_totales_equipos).ToString("P2") });
                fila_categoria.Controls.Add(new HtmlTableCell("td") { InnerHtml = item.monto.ToString("$ #,##0.00") });
                tabla.Controls.Add(fila_categoria);
            }

            div_tabla_resumen.Controls.Clear();
            div_tabla_resumen.Controls.Add(tabla);
        }

        private void Cargar_tabla_final_planilla_calculos()
        {
            int mes = Convert.ToInt32(Session["planilla_calculos_mes"]);
            int anio = Convert.ToInt32(Session["planilla_calculos_anio"]);
            string categoria_empleado = Session["planilla_calculos_categoria_empleado"].ToString();

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

            using (var cxt = new Model1Container())
            {
                List<Empleado> empleados = new List<Empleado>();

                if (categoria_empleado == "Mecánicos - Pintores")
                {
                    empleados = cxt.Empleados.Where(ee => (ee.Categoria.nombre == "Mecánico" || ee.Categoria.nombre == "Pintor") && ee.fecha_baja == null).ToList();
                }
                else
                {
                    empleados = cxt.Empleados.Where(ee => ee.Categoria.nombre == "Soldador" && ee.fecha_baja == null).ToList();
                }

                HtmlTable tabla = new HtmlTable();
                tabla.Attributes.Add("class", "table table-bordered");
                HtmlTableRow fila_encabezado_1 = new HtmlTableRow();
                fila_encabezado_1.Controls.Add(new HtmlTableCell("th") { RowSpan = 2, InnerHtml = "Empleado", BgColor = "lightgray" });
                fila_encabezado_1.Controls.Add(new HtmlTableCell("th") { ColSpan = 4, InnerHtml = "HORAS", BgColor = "lightgray" });
                fila_encabezado_1.Controls.Add(new HtmlTableCell("th") { ColSpan = 4, InnerHtml = "DIAS", BgColor = "lightgray" });
                fila_encabezado_1.Controls.Add(new HtmlTableCell("th") { ColSpan = 4, InnerHtml = "Resultado", BgColor = "lightgray" });
                HtmlTableRow fila_encabezado_2 = new HtmlTableRow();
                fila_encabezado_2.Controls.Add(new HtmlTableCell("th") { InnerHtml = "Normales", BgColor = "lightgray" });
                fila_encabezado_2.Controls.Add(new HtmlTableCell("th") { InnerHtml = "50 %", BgColor = "lightgray" });
                fila_encabezado_2.Controls.Add(new HtmlTableCell("th") { InnerHtml = "100 %", BgColor = "lightgray" });
                fila_encabezado_2.Controls.Add(new HtmlTableCell("th") { InnerHtml = "Totales", BgColor = "lightgray" });
                fila_encabezado_2.Controls.Add(new HtmlTableCell("th") { InnerHtml = "Laborales", BgColor = "lightgray" });
                fila_encabezado_2.Controls.Add(new HtmlTableCell("th") { InnerHtml = "Presentes", BgColor = "lightgray" });
                fila_encabezado_2.Controls.Add(new HtmlTableCell("th") { InnerHtml = "Ausentes", BgColor = "lightgray" });
                fila_encabezado_2.Controls.Add(new HtmlTableCell("th") { InnerHtml = "Diferencia", BgColor = "lightgray" });
                fila_encabezado_2.Controls.Add(new HtmlTableCell("th") { InnerHtml = "% Ausentismo", BgColor = "lightgray" });
                fila_encabezado_2.Controls.Add(new HtmlTableCell("th") { InnerHtml = "Ausentismo", BgColor = "lightgray" });
                fila_encabezado_2.Controls.Add(new HtmlTableCell("th") { InnerHtml = "$ Ausentismo", BgColor = "lightgray" });

                tabla.Controls.Add(fila_encabezado_1);
                tabla.Controls.Add(fila_encabezado_2);

                foreach (Empleado item in empleados)
                {
                    Resumen_mes_empleado rme = item.Resumenes_meses_empleado.FirstOrDefault(x=>x.anio == anio && x.mes == mes);

                    HtmlTableRow tr = new HtmlTableRow();
                    tr.Controls.Add(new HtmlTableCell() { InnerHtml = item.nombre, BgColor = "lightgray" }); 
                    tr.Controls.Add(new HtmlTableCell() { InnerHtml = rme != null ? (Convert.ToDecimal(rme.total_horas_normales.Split(':')[0]) + (Convert.ToDecimal(rme.total_horas_normales.Split(':')[1]) / Convert.ToDecimal(60))).ToString("#,##0.00") : 0.ToString("#,##0.00") });
                    tr.Controls.Add(new HtmlTableCell() { InnerHtml = rme != null ? (Convert.ToDecimal(rme.total_horas_extra_50.Split(':')[0]) + (Convert.ToDecimal(rme.total_horas_extra_50.Split(':')[1]) / Convert.ToDecimal(60))).ToString("#,##0.00") : 0.ToString("#,##0.00") });
                    tr.Controls.Add(new HtmlTableCell() { InnerHtml = rme != null ? (Convert.ToDecimal(rme.total_horas_extra_100.Split(':')[0]) + (Convert.ToDecimal(rme.total_horas_extra_100.Split(':')[1]) / Convert.ToDecimal(60))).ToString("#,##0.00") : 0.ToString("#,##0.00") });
                    tr.Controls.Add(new HtmlTableCell() { InnerHtml = rme != null ? (Convert.ToDecimal(Horas_string.SumarHoras(new string[] { rme.total_horas_normales, rme.total_horas_extra_50, rme.total_horas_extra_100 }).Split(':')[0]) + (Convert.ToDecimal(Horas_string.SumarHoras(new string[] { rme.total_horas_normales, rme.total_horas_extra_50, rme.total_horas_extra_100 }).Split(':')[1]) / Convert.ToDecimal(60))).ToString("#,##0.00") : 0.ToString("#,##0.00") });

                    tr.Controls.Add(new HtmlTableCell() { InnerHtml = rme != null ? rme.dias_laborables.ToString("#,##0.00") : 0.ToString("#,##0.00") });
                    tr.Controls.Add(new HtmlTableCell() { InnerHtml = rme != null ? rme.dias_presente.ToString("#,##0.00") : 0.ToString("#,##0.00") });
                    tr.Controls.Add(new HtmlTableCell() { InnerHtml = rme != null ? rme.dias_ausente.ToString("#,##0.00") : 0.ToString("#,##0.00") });
                    tr.Controls.Add(new HtmlTableCell() { InnerHtml = rme != null ? (rme.dias_laborables - rme.dias_ausente - rme.dias_presente).ToString("#,##0.00") : 0.ToString("#,##0.00") });

                    tr.Controls.Add(new HtmlTableCell() { InnerHtml = rme != null ? (rme.dias_ausente / rme.dias_laborables).ToString("#,##0.00") : 0.ToString("#,##0.00") });
                    tr.Controls.Add(new HtmlTableCell() { InnerHtml = rme != null ? rme.dias_ausente.ToString("#,##0.00") : 0.ToString("#,##0.00") });
                    tr.Controls.Add(new HtmlTableCell() { InnerHtml = rme != null ? (rme.dias_ausente * Convert.ToDecimal(8) * costo_hora).ToString("$ #,##0.00") : 0.ToString("$ #,##0.00") });

                    tabla.Controls.Add(tr);
                }

                div_tabla_resumen_ausentismo.Controls.Clear();
                div_tabla_resumen_ausentismo.Controls.Add(tabla);
            }
        }

        private void CargarFilasCategoria(Categoria_equipo categoria, List<equipo_tabla> filas_categoria, int mes, int anio, decimal costo_hora)
        {
            int equipos_guardia = Convert.ToInt32(Session["totales_equipos_guardia"]);
            decimal horas_totales_equipos = Convert.ToDecimal(Session["totales_horas_totales_equipos"]);
            decimal horas_guardia_decimal = Convert.ToDecimal(Session["totales_horas_guardia_decimal"]);
            decimal horas_varios_taller_decimal = Convert.ToDecimal(Session["totales_horas_varios_taller_decimal"]);
            string categoria_empleado = Session["planilla_calculos_categoria_empleado"].ToString();

            decimal horas_categoria = 0;

            List<total_categoria_tabla> filas_tabla_total_categoria = Session["filas_tabla_total_categoria"] as List<total_categoria_tabla>;


            using (var cxt = new Model1Container())
            {
                categoria = cxt.Categorias_equipos.First(x => x.id_categoria == categoria.id_categoria);

                foreach (Equipo equipo in categoria.Equipos.Where(x => !x.Generico))
                {
                    string horas_mes_equipo = equipo.Horas_mes(mes, anio, categoria_empleado);
                    equipo_tabla et = new equipo_tabla();
                    et.categoria = equipo.Categoria.nombre;
                    et.id_equipo = equipo.id_equipo;
                    et.nombre = equipo.nombre;
                    et.horas_mes = Convert.ToDecimal(horas_mes_equipo.Split(':')[0]) + (Convert.ToDecimal(horas_mes_equipo.Split(':')[1]) / Convert.ToDecimal(60));
                    et.reparte_guardia = cxt.Aux_planilla_calculos.FirstOrDefault(x => x.id_equipo == equipo.id_equipo && x.mes == mes && x.anio == anio) != null ? cxt.Aux_planilla_calculos.FirstOrDefault(x => x.id_equipo == equipo.id_equipo && x.mes == mes && x.anio == anio).considera_guardia : false;

                    horas_categoria = horas_categoria + et.horas_mes;

                    equipos_guardia = et.reparte_guardia ? equipos_guardia + 1 : equipos_guardia;
                    horas_totales_equipos = horas_totales_equipos + et.horas_mes;

                    filas_categoria.Add(et);
                }
            }

            filas_tabla_total_categoria.Add(new total_categoria_tabla() { categoria = categoria.nombre, horas = horas_categoria, monto = horas_categoria * costo_hora });

            Session["filas_tabla_total_categoria"] = filas_tabla_total_categoria;

            Session["totales_equipos_guardia"] = equipos_guardia;
            Session["totales_horas_totales_equipos"] = horas_totales_equipos;
            Session["totales_horas_guardia_decimal"] = horas_guardia_decimal;
            Session["totales_horas_varios_taller_decimal"] = horas_varios_taller_decimal;
        }

        private void Agregar_a_tabla(HtmlTable tabla, List<equipo_tabla> listado, decimal horas_totales_equipos, decimal horas_varios_taller, decimal horas_guardia, int equipos_guardia, decimal costo_hora)
        {
            bool primerfila = true;

            foreach (equipo_tabla item in listado)
            {
                HtmlTableRow fila_equipo = new HtmlTableRow();
                if (primerfila)
                {
                    primerfila = false;
                    HtmlTableCell columna_categoria = new HtmlTableCell("td") { RowSpan = listado.Count, BgColor = "lightgray" };
                    Label label = new Label();
                    label.CssClass = "rotar";
                    label.Text = item.categoria;
                    columna_categoria.Controls.Add(label);
                    fila_equipo.Controls.Add(columna_categoria);
                }

                decimal porcentaje_equipo_sobre_total = horas_totales_equipos > 0 ? (item.horas_mes / horas_totales_equipos) : 0;
                decimal horas_guardia_equipo = item.reparte_guardia ? horas_guardia / Convert.ToDecimal(equipos_guardia) : 0;

                fila_equipo.Controls.Add(new HtmlTableCell("td") { InnerHtml = item.nombre, BgColor = "lightgray" });
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
                fila_equipo.Controls.Add(new HtmlTableCell("td") { InnerHtml = ((item.horas_mes + (horas_varios_taller * porcentaje_equipo_sobre_total) + horas_guardia_equipo) * costo_hora).ToString("$ #,##0.00") });

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