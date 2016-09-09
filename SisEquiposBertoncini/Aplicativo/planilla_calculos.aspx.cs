using Microsoft.Reporting.WebForms;
using SisEquiposBertoncini.Aplicativo.Controles;
using SisEquiposBertoncini.Aplicativo.Datos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

                lbl_mes.Text = ObtenerMes(Session["planilla_calculos_mes"].ToString());
                lbl_anio.Text = Session["planilla_calculos_anio"].ToString();
                lbl_tipo_empleado.Text = Session["planilla_calculos_categoria_empleado"].ToString();

                lbl_costo_hora_seleccionado.Text = Convert.ToDecimal(Session["planilla_calculos_costo_hora_teorico"]).ToString("$ #,##0.00");

                CrearCargarTabla();
            }
        }

        private string ObtenerMes(string p)
        {
            switch (p)
            {
                case "1":
                    return "enero";
                case "2":
                    return "febrero";
                case "3":
                    return "marzo";
                case "4":
                    return "abril";
                case "5":
                    return "mayo";
                case "6":
                    return "junio";
                case "7":
                    return "julio";
                case "8":
                    return "agosto";
                case "9":
                    return "septiembre";
                case "10":
                    return "octubre";
                case "11":
                    return "noviembre";
                case "12":
                    return "diciembre";

                default:
                    return "";
            }
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

        private struct equipo_tabla_out
        {
            public int id_equipo { get; set; }
            public string nombre { get; set; }
            public decimal dias_out { get; set; }
        }

        private struct total_categoria_tabla
        {
            public string categoria { get; set; }
            public decimal horas { get; set; }
            public decimal monto { get; set; }
        }

        private struct item_combinado
        {
            public Categoria_equipo categoria { get; set; }
            public List<equipo_tabla> filas_categoria { get; set; }
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
            if (categoria_empleado != "Soldadores")
            {
                encabezado_tr1.Controls.Add(new HtmlTableCell("th") { ColSpan = 2, InnerHtml = "GUARDIA", BgColor = "lightgray" });
            }
            encabezado_tr1.Controls.Add(new HtmlTableCell("th") { RowSpan = 2, InnerHtml = "TOTAL HS ACUM.", BgColor = "lightgray" });
            encabezado_tr1.Controls.Add(new HtmlTableCell("th") { RowSpan = 3, InnerHtml = "MONTO HS ACUM.", BgColor = "lightgray" });
            tabla.Controls.Add(encabezado_tr1);

            HtmlTableRow encabezado_tr2 = new HtmlTableRow();
            encabezado_tr2.Controls.Add(new HtmlTableCell("th") { InnerHtml = "HS", BgColor = "lightgray" });
            encabezado_tr2.Controls.Add(new HtmlTableCell("th") { InnerHtml = "%", BgColor = "lightgray" });
            encabezado_tr2.Controls.Add(new HtmlTableCell("th") { InnerHtml = "Hrs. Varios Taller", BgColor = "lightgray" });
            if (categoria_empleado != "Soldadores")
            {
                encabezado_tr2.Controls.Add(new HtmlTableCell("th") { InnerHtml = "CANT EQUIPOS", BgColor = "lightgray" });
                encabezado_tr2.Controls.Add(new HtmlTableCell("th") { InnerHtml = "Hrs. Tot Guardia", BgColor = "lightgray" });
            }
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
                List<item_combinado> categorias = new List<item_combinado>();

                foreach (Categoria_equipo categoria in cxt.Categorias_equipos)
                {
                    if (categoria.toma_en_cuenta_planilla_costos_horas_hombre.HasValue && categoria.toma_en_cuenta_planilla_costos_horas_hombre.Value)
                    {
                        categorias.Add(new item_combinado() { categoria = categoria, filas_categoria = new List<equipo_tabla>() });
                    }
                }

                List<equipo_tabla> filas_out_soldadores = new List<equipo_tabla>();

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

                foreach (item_combinado item in categorias)
                {
                    CargarFilasCategoria(item.categoria, item.filas_categoria, mes, anio, costo_hora);
                }

                if (categoria_empleado == "Soldadores" || categoria_empleado == "Grueros")
                {
                    List<Equipo> equipos = cxt.Equipos.Where(x => x.OUT).ToList();
                    CargarFilasCategoria_OUT(equipos, filas_out_soldadores, mes, anio, costo_hora);
                }

                //recupero los totales cargados en los llamados anteriores
                equipos_guardia = Convert.ToInt32(Session["totales_equipos_guardia"]);
                horas_totales_equipos = Convert.ToDecimal(Session["totales_horas_totales_equipos"]);
                horas_guardia_decimal = Convert.ToDecimal(Session["totales_horas_guardia_decimal"]);
                horas_varios_taller_decimal = Convert.ToDecimal(Session["totales_horas_varios_taller_decimal"]);

                //Ahora que tengo los totales del mes, elimino las filas cuyos equipos no tienen valor y las ordeno de mayor a menor por categoria

                foreach (item_combinado item in categorias)
                {
                    CorregirFilas(item.filas_categoria, horas_totales_equipos, horas_guardia_decimal, horas_varios_taller_decimal, equipos_guardia);
                }

                if (categoria_empleado == "Soldadores" || categoria_empleado == "Grueros")
                {
                    CorregirFilas(filas_out_soldadores, horas_totales_equipos, horas_guardia_decimal, horas_varios_taller_decimal, equipos_guardia);
                }

                HtmlTableRow fila_totales = new HtmlTableRow();
                fila_totales.Controls.Add(new HtmlTableCell("th") { InnerHtml = horas_totales_equipos.ToString("#,##0.00") });
                fila_totales.Controls.Add(new HtmlTableCell("th") { InnerHtml = "100 %" });
                fila_totales.Controls.Add(new HtmlTableCell("th") { InnerHtml = horas_varios_taller_decimal.ToString("#,##0.00") });
                if (categoria_empleado != "Soldadores")
                {
                    fila_totales.Controls.Add(new HtmlTableCell("th") { InnerHtml = equipos_guardia.ToString() });
                    fila_totales.Controls.Add(new HtmlTableCell("th") { InnerHtml = horas_guardia_decimal.ToString("#,##0.00") });
                }
                fila_totales.Controls.Add(new HtmlTableCell("th") { InnerHtml = (horas_totales_equipos + horas_varios_taller_decimal + horas_guardia_decimal).ToString("#,##0.00") });
                tabla.Controls.Add(fila_totales);

                Session["planilla_gastos_en_funcion_horas_hombre_categorias"] = new List<categoria_paraPlanillaGastosenFuncionHorasHombre>();
                Session["planilla_gastos_en_funcion_horas_hombre_mes"] = mes;
                Session["planilla_gastos_en_funcion_horas_hombre_anio"] = anio;
                Session["planilla_gastos_en_funcion_horas_hombre_tipo_empleado"] = categoria_empleado;

                ///REPORTE - Creo y agrego las primeras filas del dataset para el reporte
                Reportes.planilla_calculos ds = new Reportes.planilla_calculos();
                //(ds reporte) Agrego la primer fila de totales despues de los titulos de la tabla equipo horas
                Reportes.planilla_calculos.Equipos_horas_totalRow eht = ds.Equipos_horas_total.NewEquipos_horas_totalRow();
                eht.Hs_directas = horas_totales_equipos.ToString("#,##0.00");
                eht.Porcentaje = "100 %";
                eht.Hs_taller = horas_varios_taller_decimal.ToString("#,##0.00");
                if (categoria_empleado != "Soldadores")
                {
                    eht.Agrega_guardia = equipos_guardia.ToString();
                    eht.Horas_guardia = horas_guardia_decimal.ToString("#,##0.00");
                }
                else
                {
                    eht.Agrega_guardia = equipos_guardia.ToString();
                    eht.Horas_guardia = 0.ToString("#,##0.00");
                }
                eht.Total_horas = (horas_totales_equipos + horas_varios_taller_decimal + horas_guardia_decimal).ToString("#,##0.00");
                ds.Equipos_horas_total.Rows.Add(eht);

                //(ds reporte) Agrego la fila de datos generales 
                Reportes.planilla_calculos.Datos_generalesRow dg = ds.Datos_generales.NewDatos_generalesRow();
                dg.Mes = new DateTime(anio, mes, 1).ToString("MMMM");
                dg.Anio = anio.ToString();
                dg.Tipo_empleado = categoria_empleado;
                dg.Horas_normales = lbl_horas_normales.Text;
                dg.Horas_extra_50 = lbl_horas_extra_50.Text;
                dg.Horas_extra_100 = lbl_horas_extra_100.Text;
                dg.Horas_totales = lbl_horas_totales.Text;
                dg.Calculado_en_base_a = ddl_costo_hora.SelectedItem.Text;
                dg.Costo_hora = costo_hora.ToString("$ #,##0.00");
                ds.Datos_generales.Rows.Add(dg);
                //(ds reporte) guardo el adtaset en una variable de session para poder seguir cargandolo desde las demas funciones
                Session["ds_planilla_calculos"] = ds;
                foreach (item_combinado item in categorias)
                {
                    Agregar_a_tabla(tabla, item.filas_categoria, horas_totales_equipos, horas_varios_taller_decimal, horas_guardia_decimal, equipos_guardia, costo_hora);
                }

                if (categoria_empleado == "Soldadores" || categoria_empleado == "Grueros")
                {
                    //tengo que agregar los trabajos outs como trabajos dentro de la misma tabla

                    Agregar_a_tabla(tabla, filas_out_soldadores, horas_totales_equipos, horas_varios_taller_decimal, horas_guardia_decimal, equipos_guardia, costo_hora);
                }

                Cargar_tabla_resumen(horas_totales_equipos + horas_guardia_decimal + horas_varios_taller_decimal);
            }

            div_tabla.Controls.Clear();

            div_tabla.Controls.Add(tabla);

            if (categoria_empleado == "Mecánicos - Pintores")
            {
                Cargar_tabla_trabajos_out();
            }

            Cargar_tabla_final_planilla_calculos();


        }

        private void CorregirFilas(List<equipo_tabla> filas_otros, decimal horas_totales_equipos, decimal horas_guardia, decimal horas_varios_taller, int equipos_guardia)
        {
            List<equipo_tabla> ret = new List<equipo_tabla>();

            foreach (equipo_tabla item in filas_otros)
            {
                decimal porcentaje_equipo_sobre_total = horas_totales_equipos > 0 ? (item.horas_mes / horas_totales_equipos) : 0;
                decimal horas_guardia_equipo = item.reparte_guardia ? horas_guardia / Convert.ToDecimal(equipos_guardia) : 0;
                if ((item.horas_mes + (horas_varios_taller * porcentaje_equipo_sobre_total) + horas_guardia_equipo) > 0)
                {
                    ret.Add(new equipo_tabla()
                    {
                        categoria = item.categoria,
                        horas_acumuladas = item.horas_mes + (horas_varios_taller * porcentaje_equipo_sobre_total) + horas_guardia_equipo,
                        horas_guardia = item.horas_guardia,
                        horas_mes = item.horas_mes,
                        horas_taller = item.horas_taller,
                        id_equipo = item.id_equipo,
                        monto_horas_acumuladas = item.monto_horas_acumuladas,
                        nombre = item.nombre,
                        porcentaje = item.porcentaje,
                        reparte_guardia = item.reparte_guardia
                    });
                }
            }

            filas_otros.Clear();

            foreach (equipo_tabla item in ret.OrderByDescending(x => x.horas_acumuladas))
            {
                filas_otros.Add(new equipo_tabla()
                {
                    categoria = item.categoria,
                    horas_acumuladas = item.horas_acumuladas,
                    horas_guardia = item.horas_guardia,
                    horas_mes = item.horas_mes,
                    horas_taller = item.horas_taller,
                    id_equipo = item.id_equipo,
                    monto_horas_acumuladas = item.monto_horas_acumuladas,
                    nombre = item.nombre,
                    porcentaje = item.porcentaje,
                    reparte_guardia = item.reparte_guardia
                });
            }

        }

        private void Cargar_tabla_trabajos_out()
        {
            int mes = Convert.ToInt32(Session["planilla_calculos_mes"]);
            int anio = Convert.ToInt32(Session["planilla_calculos_anio"]);
            string categoria_empleado = Session["planilla_calculos_categoria_empleado"].ToString();
            Reportes.planilla_calculos ds = Session["ds_planilla_calculos"] as Reportes.planilla_calculos;

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

                List<equipo_tabla_out> outs = new List<equipo_tabla_out>();

                foreach (Equipo equipo in equipos)
                {
                    string horas_mes_equipo = equipo.Horas_mes(mes, anio, categoria_empleado);
                    if (Horas_string.HoraNoNula(horas_mes_equipo))
                    {
                        decimal horas_out = Convert.ToDecimal(horas_mes_equipo.Split(':')[0]) + (Convert.ToDecimal(horas_mes_equipo.Split(':')[1]) / Convert.ToDecimal(60));
                        decimal dias_out = horas_out / 8;
                        dias_totales_out = dias_out + dias_totales_out;


                        outs.Add(new equipo_tabla_out()
                        {
                            id_equipo = equipo.id_equipo,
                            nombre = equipo.nombre,
                            dias_out = dias_out
                        });
                    }

                }

                foreach (equipo_tabla_out item in outs.OrderByDescending(x => x.dias_out))
                {
                    Reportes.planilla_calculos.Trabajos_outRow tor = ds.Trabajos_out.NewTrabajos_outRow();
                    tor.Trabajo = item.nombre;
                    tor.Dias = item.dias_out.ToString("#,##0.00 'días.'");
                    ds.Trabajos_out.Rows.Add(tor);

                    HtmlTableRow fila_trabajo_out = new HtmlTableRow();
                    fila_trabajo_out.Controls.Add(new HtmlTableCell("th") { InnerText = item.nombre, BgColor = "lightgray" });
                    fila_trabajo_out.Controls.Add(new HtmlTableCell("th") { InnerText = item.dias_out.ToString("#,##0.00 'días.'") });
                    table.Controls.Add(fila_trabajo_out);
                }

                HtmlTableRow fila_pie = new HtmlTableRow();
                fila_pie.Controls.Add(new HtmlTableCell("th") { InnerText = "TOTAL", BgColor = "lightgray" });
                fila_pie.Controls.Add(new HtmlTableCell("th") { InnerText = dias_totales_out.ToString("#,##0.00 'días.'"), BgColor = "lightgray" });

                Reportes.planilla_calculos.Trabajos_outRow totor = ds.Trabajos_out.NewTrabajos_outRow();
                totor.Trabajo = "TOTAL";
                totor.Dias = dias_totales_out.ToString("#,##0.00 'días.'");
                ds.Trabajos_out.Rows.Add(totor);

                table.Controls.Add(fila_pie);

                Session["ds_planilla_calculos"] = ds;
                div_tabla_OUT.Controls.Clear();
                div_tabla_OUT.Controls.Add(table);
            }
        }

        private void Cargar_tabla_resumen(decimal horas_totales_equipos)
        {
            Reportes.planilla_calculos ds = Session["ds_planilla_calculos"] as Reportes.planilla_calculos;

            if (horas_totales_equipos == 0) { horas_totales_equipos = 1; }
            HtmlTable tabla = new HtmlTable();
            tabla.Attributes.Add("class", "table table-bordered");
            HtmlTableRow fila_encabezado = new HtmlTableRow();
            fila_encabezado.Controls.Add(new HtmlTableCell("th") { InnerHtml = "Categoria", BgColor = "lightgray" });
            fila_encabezado.Controls.Add(new HtmlTableCell("th") { InnerHtml = "Horas", BgColor = "lightgray" });
            fila_encabezado.Controls.Add(new HtmlTableCell("th") { InnerHtml = "%", BgColor = "lightgray" });
            fila_encabezado.Controls.Add(new HtmlTableCell("th") { InnerHtml = "Monto", BgColor = "lightgray" });
            tabla.Controls.Add(fila_encabezado);

            List<total_categoria_tabla> filas_tabla_total_categoria = Session["filas_tabla_total_categoria"] as List<total_categoria_tabla>;


            foreach (total_categoria_tabla item in filas_tabla_total_categoria.Where(x => x.categoria != "").OrderByDescending(x => x.horas))
            {
                Reportes.planilla_calculos.Totales_categoriaRow tcdr = ds.Totales_categoria.NewTotales_categoriaRow();
                tcdr.Categoria = item.categoria;
                tcdr.Horas = item.horas.ToString("#,##0.00");
                tcdr.Porcentaje = (item.horas / horas_totales_equipos).ToString("P2");
                tcdr.Monto = item.monto.ToString("$ #,##0.00");
                ds.Totales_categoria.Rows.Add(tcdr);

                HtmlTableRow fila_categoria = new HtmlTableRow();
                fila_categoria.Controls.Add(new HtmlTableCell("th") { InnerHtml = item.categoria, BgColor = "lightgray" });
                fila_categoria.Controls.Add(new HtmlTableCell("td") { InnerHtml = item.horas.ToString("#,##0.00") });
                fila_categoria.Controls.Add(new HtmlTableCell("td") { InnerHtml = (item.horas / horas_totales_equipos).ToString("P2") });
                fila_categoria.Controls.Add(new HtmlTableCell("td") { InnerHtml = item.monto.ToString("$ #,##0.00") });
                tabla.Controls.Add(fila_categoria);
            }

            Reportes.planilla_calculos.Totales_categoriaRow totcdr = ds.Totales_categoria.NewTotales_categoriaRow();
            totcdr.Categoria = "TOTALES";
            totcdr.Horas = filas_tabla_total_categoria.Sum(x => x.horas).ToString("#,##0.00");
            totcdr.Porcentaje = "100 %"; ;
            totcdr.Monto = filas_tabla_total_categoria.Sum(x => x.monto).ToString("$ #,##0.00");
            ds.Totales_categoria.Rows.Add(totcdr);

            HtmlTableRow fila_total = new HtmlTableRow();
            fila_total.Controls.Add(new HtmlTableCell("th") { InnerHtml = "TOTALES", BgColor = "lightgray" });
            fila_total.Controls.Add(new HtmlTableCell("th") { InnerHtml = filas_tabla_total_categoria.Sum(x => x.horas).ToString("#,##0.00") });
            fila_total.Controls.Add(new HtmlTableCell("th") { InnerHtml = "100 %" });
            fila_total.Controls.Add(new HtmlTableCell("th") { InnerHtml = filas_tabla_total_categoria.Sum(x => x.monto).ToString("$ #,##0.00") });
            tabla.Controls.Add(fila_total);

            Session["ds_planilla_calculos"] = ds;

            div_tabla_resumen.Controls.Clear();
            div_tabla_resumen.Controls.Add(tabla);
        }

        private void Cargar_tabla_final_planilla_calculos()
        {
            int mes = Convert.ToInt32(Session["planilla_calculos_mes"]);
            int anio = Convert.ToInt32(Session["planilla_calculos_anio"]);
            string categoria_empleado = Session["planilla_calculos_categoria_empleado"].ToString();
            Reportes.planilla_calculos ds = Session["ds_planilla_calculos"] as Reportes.planilla_calculos;
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
                    

                    Resumen_mes_empleado rme = item.Resumenes_meses_empleado.FirstOrDefault(x => x.anio == anio && x.mes == mes);

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

                    Reportes.planilla_calculos.AsistenciaRow adr = ds.Asistencia.NewAsistenciaRow();
                    adr.Empleado = item.nombre;
                    adr.Hs_normales = rme != null ? (Convert.ToDecimal(rme.total_horas_normales.Split(':')[0]) + (Convert.ToDecimal(rme.total_horas_normales.Split(':')[1]) / Convert.ToDecimal(60))).ToString("#,##0.00") : 0.ToString("#,##0.00");
                    adr.Hs_50 = rme != null ? (Convert.ToDecimal(rme.total_horas_extra_50.Split(':')[0]) + (Convert.ToDecimal(rme.total_horas_extra_50.Split(':')[1]) / Convert.ToDecimal(60))).ToString("#,##0.00") : 0.ToString("#,##0.00");
                    adr.Hs_100 = rme != null ? (Convert.ToDecimal(rme.total_horas_extra_100.Split(':')[0]) + (Convert.ToDecimal(rme.total_horas_extra_100.Split(':')[1]) / Convert.ToDecimal(60))).ToString("#,##0.00") : 0.ToString("#,##0.00");
                    adr.Hs_totales = rme != null ? (Convert.ToDecimal(Horas_string.SumarHoras(new string[] { rme.total_horas_normales, rme.total_horas_extra_50, rme.total_horas_extra_100 }).Split(':')[0]) + (Convert.ToDecimal(Horas_string.SumarHoras(new string[] { rme.total_horas_normales, rme.total_horas_extra_50, rme.total_horas_extra_100 }).Split(':')[1]) / Convert.ToDecimal(60))).ToString("#,##0.00") : 0.ToString("#,##0.00");
                    adr.Dias_laborables = rme != null ? rme.dias_laborables.ToString("#,##0.00") : 0.ToString("#,##0.00");
                    adr.Dias_presentes = rme != null ? rme.dias_presente.ToString("#,##0.00") : 0.ToString("#,##0.00");
                    adr.Dias_ausentes = rme != null ? rme.dias_ausente.ToString("#,##0.00") : 0.ToString("#,##0.00");
                    adr.Dias_diferencia = rme != null ? (rme.dias_laborables - rme.dias_ausente - rme.dias_presente).ToString("#,##0.00") : 0.ToString("#,##0.00");
                    adr.Res_porc_aus = rme != null ? (rme.dias_ausente / rme.dias_laborables).ToString("#,##0.00") : 0.ToString("#,##0.00");
                    adr.Res_aus = rme != null ? rme.dias_ausente.ToString("#,##0.00") : 0.ToString("#,##0.00");
                    adr.Res_pesos_aus = rme != null ? (rme.dias_ausente * Convert.ToDecimal(8) * costo_hora).ToString("$ #,##0.00") : 0.ToString("$ #,##0.00");
                    ds.Asistencia.Rows.Add(adr);
                }

                Session["ds_planilla_calculos"] = ds;

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

            using (var cxt = new Model1Container())
            {
                categoria = cxt.Categorias_equipos.First(x => x.id_categoria == categoria.id_categoria);

                foreach (Equipo equipo in categoria.Equipos.Where(x => !x.Generico && !x.OUT))
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

            Session["totales_equipos_guardia"] = equipos_guardia;
            Session["totales_horas_totales_equipos"] = horas_totales_equipos;
            Session["totales_horas_guardia_decimal"] = horas_guardia_decimal;
            Session["totales_horas_varios_taller_decimal"] = horas_varios_taller_decimal;
        }

        private void CargarFilasCategoria_OUT(List<Equipo> equipos, List<equipo_tabla> filas_categoria, int mes, int anio, decimal costo_hora)
        {
            int equipos_guardia = Convert.ToInt32(Session["totales_equipos_guardia"]);
            decimal horas_totales_equipos = Convert.ToDecimal(Session["totales_horas_totales_equipos"]);
            decimal horas_guardia_decimal = Convert.ToDecimal(Session["totales_horas_guardia_decimal"]);
            decimal horas_varios_taller_decimal = Convert.ToDecimal(Session["totales_horas_varios_taller_decimal"]);
            string categoria_empleado = Session["planilla_calculos_categoria_empleado"].ToString();

            decimal horas_categoria = 0;

            using (var cxt = new Model1Container())
            {
                foreach (Equipo equipo in equipos)
                {
                    string horas_mes_equipo = equipo.Horas_mes(mes, anio, categoria_empleado);
                    if (Horas_string.HoraNoNula(horas_mes_equipo))
                    {
                        equipo_tabla et = new equipo_tabla();
                        et.categoria = "Trabajos OUT";
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
            }

            Session["totales_equipos_guardia"] = equipos_guardia;
            Session["totales_horas_totales_equipos"] = horas_totales_equipos;
            Session["totales_horas_guardia_decimal"] = horas_guardia_decimal;
            Session["totales_horas_varios_taller_decimal"] = horas_varios_taller_decimal;
        }

        public struct equipo_paraPlanillaGastosenFuncionHorasHombre
        {
            public int id_equipo { get; set; }
            public string equipo { get; set; }
            public decimal horas { get; set; }
        }
        public struct categoria_paraPlanillaGastosenFuncionHorasHombre
        {
            public string categoria { get; set; }
            public decimal total_horas_categoria { get; set; }
            public List<equipo_paraPlanillaGastosenFuncionHorasHombre> equipos { get; set; }
        }

        private void Agregar_a_tabla(HtmlTable tabla, List<equipo_tabla> listado, decimal horas_totales_equipos, decimal horas_varios_taller, decimal horas_guardia, int equipos_guardia, decimal costo_hora)
        {


            bool primerfila = true;
            string categoria_empleado = Session["planilla_calculos_categoria_empleado"].ToString();
            List<total_categoria_tabla> filas_tabla_total_categoria = Session["filas_tabla_total_categoria"] as List<total_categoria_tabla>;
            Reportes.planilla_calculos ds = Session["ds_planilla_calculos"] as Reportes.planilla_calculos;

            List<categoria_paraPlanillaGastosenFuncionHorasHombre> categorias_para_grilla_gastos_en_funcion_de_horas_hombre = Session["planilla_gastos_en_funcion_horas_hombre_categorias"] as List<categoria_paraPlanillaGastosenFuncionHorasHombre>;
            categoria_paraPlanillaGastosenFuncionHorasHombre categoria_gastos_horas_hombre = new categoria_paraPlanillaGastosenFuncionHorasHombre();

            string categoria = "";
            decimal horas = 0;
            decimal monto = 0;

            foreach (equipo_tabla item in listado)
            {
                Reportes.planilla_calculos.Equipos_horasRow ehr = ds.Equipos_horas.NewEquipos_horasRow();
                decimal porcentaje_equipo_sobre_total = horas_totales_equipos > 0 ? (item.horas_mes / horas_totales_equipos) : 0;
                decimal horas_guardia_equipo = item.reparte_guardia ? horas_guardia / Convert.ToDecimal(equipos_guardia) : 0;

                if ((item.horas_mes + (horas_varios_taller * porcentaje_equipo_sobre_total) + horas_guardia_equipo) > 0)
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
                        categoria = item.categoria;
                        categoria_gastos_horas_hombre.categoria = item.categoria;
                        categoria_gastos_horas_hombre.equipos = new List<equipo_paraPlanillaGastosenFuncionHorasHombre>();

                    }

                    fila_equipo.Controls.Add(new HtmlTableCell("td") { InnerHtml = item.nombre, BgColor = "lightgray" });
                    fila_equipo.Controls.Add(new HtmlTableCell("td") { InnerHtml = item.horas_mes.ToString("#,##0.00") });
                    fila_equipo.Controls.Add(new HtmlTableCell("td") { InnerHtml = porcentaje_equipo_sobre_total.ToString("P2") });
                    fila_equipo.Controls.Add(new HtmlTableCell("td") { InnerHtml = (horas_varios_taller * porcentaje_equipo_sobre_total).ToString("#,##0.00") });

                    if (categoria_empleado != "Soldadores")
                    {
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

                        horas = horas + (item.horas_mes + (horas_varios_taller * porcentaje_equipo_sobre_total) + horas_guardia_equipo);
                        categoria_gastos_horas_hombre.equipos.Add(new equipo_paraPlanillaGastosenFuncionHorasHombre() { id_equipo = item.id_equipo, equipo = item.nombre, horas = (item.horas_mes + (horas_varios_taller * porcentaje_equipo_sobre_total) + horas_guardia_equipo) });
                        monto = monto + ((item.horas_mes + (horas_varios_taller * porcentaje_equipo_sobre_total) + horas_guardia_equipo) * costo_hora);

                    }
                    else
                    {
                        fila_equipo.Controls.Add(new HtmlTableCell("td") { InnerHtml = (item.horas_mes + (horas_varios_taller * porcentaje_equipo_sobre_total)).ToString("#,##0.00") });

                        fila_equipo.Controls.Add(new HtmlTableCell("td") { InnerHtml = ((item.horas_mes + (horas_varios_taller * porcentaje_equipo_sobre_total)) * costo_hora).ToString("$ #,##0.00") });

                        horas = horas + (item.horas_mes + (horas_varios_taller * porcentaje_equipo_sobre_total));
                        categoria_gastos_horas_hombre.equipos.Add(new equipo_paraPlanillaGastosenFuncionHorasHombre() { id_equipo = item.id_equipo, equipo = item.nombre, horas = (item.horas_mes + (horas_varios_taller * porcentaje_equipo_sobre_total)) });
                        monto = monto + ((item.horas_mes + (horas_varios_taller * porcentaje_equipo_sobre_total) + horas_guardia_equipo) * costo_hora);
                    }

                    //(ds reporte) Agrego el equipo a la tabla del reporte
                    ehr.Categoria = item.categoria;
                    ehr.Equipo = item.nombre;
                    ehr.Hs_directas = item.horas_mes.ToString("#,##0.00");
                    ehr.Porcentaje = porcentaje_equipo_sobre_total.ToString("P2");
                    ehr.Hs_taller = (horas_varios_taller * porcentaje_equipo_sobre_total).ToString("#,##0.00");
                    ehr.Agrega_guardia = item.reparte_guardia ? "SI" : " - ";
                    ehr.Horas_guardia = horas_guardia_equipo.ToString("#,##0.00");
                    ehr.Total_horas = (item.horas_mes + (horas_varios_taller * porcentaje_equipo_sobre_total) + horas_guardia_equipo).ToString("#,##0.00");
                    ehr.Total_pesos = ((item.horas_mes + (horas_varios_taller * porcentaje_equipo_sobre_total) + horas_guardia_equipo) * costo_hora).ToString("$ #,##0.00");
                    ds.Equipos_horas.Rows.Add(ehr);

                    //Guardar este monto como mano de obra en items mensuales
                    using (var cxt = new Model1Container())
                    {
                        int mes = Convert.ToInt32(Session["planilla_calculos_mes"]);
                        int anio = Convert.ToInt32(Session["planilla_calculos_anio"]);
                        string tipo_empleado = Session["planilla_calculos_categoria_empleado"].ToString();
                        switch (tipo_empleado)
                        {
                            case "Mecánicos - Pintores":
                                cxt.Equipos.First(x => x.id_equipo == item.id_equipo).Agregar_detalle_en_valor_mensual_segun_empleado(Equipo.Tipo_empleado.Mecanicos_pintores, Equipo.Valor_mensual.Mano_obra, mes, anio, ((item.horas_mes + (horas_varios_taller * porcentaje_equipo_sobre_total) + horas_guardia_equipo) * costo_hora));
                                break;
                            case "Soldadores":
                                cxt.Equipos.First(x => x.id_equipo == item.id_equipo).Agregar_detalle_en_valor_mensual_segun_empleado(Equipo.Tipo_empleado.Soldadores, Equipo.Valor_mensual.Mano_obra, mes, anio, ((item.horas_mes + (horas_varios_taller * porcentaje_equipo_sobre_total) + horas_guardia_equipo) * costo_hora));
                                break;
                            case "Grueros":
                                cxt.Equipos.First(x => x.id_equipo == item.id_equipo).Agregar_detalle_en_valor_mensual_segun_empleado(Equipo.Tipo_empleado.Grueros, Equipo.Valor_mensual.Mano_obra, mes, anio, ((item.horas_mes + (horas_varios_taller * porcentaje_equipo_sobre_total) + horas_guardia_equipo) * costo_hora));
                                break;
                            default:
                                break;
                        }

                    }

                    tabla.Controls.Add(fila_equipo);
                }
            }

            filas_tabla_total_categoria.Add(new total_categoria_tabla() { categoria = categoria, horas = horas, monto = monto });
            categoria_gastos_horas_hombre.total_horas_categoria = horas;

            categorias_para_grilla_gastos_en_funcion_de_horas_hombre.Add(categoria_gastos_horas_hombre);
            Session["planilla_gastos_en_funcion_horas_hombre_categorias"] = categorias_para_grilla_gastos_en_funcion_de_horas_hombre;
            Session["filas_tabla_total_categoria"] = filas_tabla_total_categoria;
            Session["ds_planilla_calculos"] = ds;
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

            CrearCargarTabla();
        }

        protected void btn_ver_planilla_principal_ServerClick(object sender, EventArgs e)
        {
            Session["planilla_principal_mes"] = Convert.ToInt32(Session["planilla_calculos_mes"]);
            Session["planilla_principal_anio"] = Convert.ToInt32(Session["planilla_calculos_anio"]);
            Session["planilla_principal_tipo_empleado"] = Session["planilla_calculos_categoria_empleado"].ToString();

            Response.Redirect("~/Aplicativo/admin_horas_planilla_principal.aspx");
        }

        protected void btn_ver_planilla_gastos_horas_hombre_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Aplicativo/planilla_gastos_en _funcion_horas_hombre.aspx");
        }

        protected void btn_imprimir_Click(object sender, EventArgs e)
        {
            Reportes.planilla_calculos ds = Session["ds_planilla_calculos"] as Reportes.planilla_calculos;
            RenderReport(ds);
        }

        private void RenderReport(Reportes.planilla_calculos ds)
        {
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.EnableExternalImages = true;

            viewer.LocalReport.ReportPath = Server.MapPath("~/Aplicativo/Reportes/planilla_calculos_r.rdlc");

            ReportDataSource datos_generales = new ReportDataSource("Datos_generales", ds.Datos_generales.Rows);
            ReportDataSource trabajos_out = new ReportDataSource("Trabajos_out", ds.Trabajos_out.Rows);
            ReportDataSource totales_categoria = new ReportDataSource("Totales_categoria", ds.Totales_categoria.Rows);
            ReportDataSource equipos_horas = new ReportDataSource("Equipos_horas", ds.Equipos_horas.Rows);
            ReportDataSource asistencia = new ReportDataSource("Asistencia", ds.Asistencia.Rows);
            ReportDataSource equipos_horas_total = new ReportDataSource("Equipos_horas_total", ds.Equipos_horas_total.Rows);

            viewer.LocalReport.DataSources.Add(datos_generales);
            viewer.LocalReport.DataSources.Add(trabajos_out);
            viewer.LocalReport.DataSources.Add(totales_categoria);
            viewer.LocalReport.DataSources.Add(equipos_horas);
            viewer.LocalReport.DataSources.Add(asistencia);
            viewer.LocalReport.DataSources.Add(equipos_horas_total);

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
    }
}