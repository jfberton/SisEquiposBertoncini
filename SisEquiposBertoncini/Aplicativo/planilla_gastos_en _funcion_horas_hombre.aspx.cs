﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SisEquiposBertoncini.Aplicativo.Datos;
using System.Web.UI.HtmlControls;

namespace SisEquiposBertoncini.Aplicativo
{
    public partial class planilla_gastos_en__funcion_horas_hombre : System.Web.UI.Page
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

                CargarTodosLosDatos(true);
            }
            else
            {
                CargarTodosLosDatos(false);
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

        private void CargarTodosLosDatos( bool actualiza_textboxes)
        {
            List<SisEquiposBertoncini.Aplicativo.planilla_calculos.categoria_paraPlanillaGastosenFuncionHorasHombre> categorias = Session["planilla_gastos_en_funcion_horas_hombre_categorias"] as List<SisEquiposBertoncini.Aplicativo.planilla_calculos.categoria_paraPlanillaGastosenFuncionHorasHombre>;
            int mes = Convert.ToInt32(Session["planilla_gastos_en_funcion_horas_hombre_mes"]);
            int anio = Convert.ToInt32(Session["planilla_gastos_en_funcion_horas_hombre_anio"]);
            string categoria_empleado = Session["planilla_gastos_en_funcion_horas_hombre_tipo_empleado"].ToString();

            lbl_mes.Text = ObtenerMes(Session["planilla_calculos_mes"].ToString());
            lbl_anio.Text = Session["planilla_calculos_anio"].ToString();
            lbl_tipo_empleado.Text = Session["planilla_calculos_categoria_empleado"].ToString();



            using (var cxt = new Model1Container())
            {
                Aux_planilla_gastos_horas_hombre aux = cxt.Aux_planilla_gastos_horas_hombres.FirstOrDefault(x => x.anio == anio && x.mes == mes && x.tipo_empleado == categoria_empleado);
                if (aux == null)
                {
                    aux = new Aux_planilla_gastos_horas_hombre();
                    aux.mes = mes;
                    aux.anio = anio;
                    aux.tipo_empleado = categoria_empleado;
                    aux.insumos_taller = 0;
                    aux.herramientas = 0;
                    aux.viaticos = 0;
                    aux.viaticos_presupuestados = 0;
                    cxt.Aux_planilla_gastos_horas_hombres.Add(aux);
                    cxt.SaveChanges();
                }

                if (actualiza_textboxes)
                {
                    tb_insumos_taller.Text = aux.insumos_taller.ToString();
                    tb_herramientas.Text = aux.herramientas.ToString();
                    tb_viaticos.Text = aux.viaticos.ToString();
                    tb_viaticos_presupuestados.Text = aux.viaticos_presupuestados.ToString();
                }

                decimal total_horas_categorias = categorias.Sum(x => x.total_horas_categoria);

                if (total_horas_categorias == 0) { total_horas_categorias = Convert.ToDecimal(1); }

                List<Itemtabla> items = new List<Itemtabla>();

                foreach (SisEquiposBertoncini.Aplicativo.planilla_calculos.categoria_paraPlanillaGastosenFuncionHorasHombre categoria in categorias.Where(x => x.categoria != null))
                {
                    foreach (SisEquiposBertoncini.Aplicativo.planilla_calculos.equipo_paraPlanillaGastosenFuncionHorasHombre equipo in categoria.equipos)
                    {
                        items.Add(new Itemtabla()
                        {
                            categoria = categoria.categoria,
                            equipo = equipo.equipo,
                            horas = equipo.horas,
                            porcentaje = equipo.horas / total_horas_categorias,
                            monto_insumos_taller = aux.insumos_taller * (equipo.horas / total_horas_categorias),
                            monto_herramientas = aux.herramientas * (equipo.horas / total_horas_categorias),
                            monto_viaticos = aux.viaticos * (equipo.horas / total_horas_categorias),
                            monto_viaticos_presupuestados = aux.viaticos_presupuestados * (equipo.horas / total_horas_categorias)
                        });
                    }

                    items.Add(new Itemtabla
                    {
                        categoria = categoria.categoria,
                        equipo = "TOTALES",
                        horas = categoria.total_horas_categoria,
                        porcentaje = categoria.total_horas_categoria / total_horas_categorias,
                        monto_insumos_taller = aux.insumos_taller * (categoria.total_horas_categoria / total_horas_categorias),
                        monto_herramientas = aux.herramientas * (categoria.total_horas_categoria / total_horas_categorias),
                        monto_viaticos = aux.viaticos * (categoria.total_horas_categoria / total_horas_categorias),
                        monto_viaticos_presupuestados = aux.viaticos_presupuestados * (categoria.total_horas_categoria / total_horas_categorias)
                    });
                }

                CargarTabla(items, total_horas_categorias, aux);
            }
        }

        private void CargarTabla(List<Itemtabla> items, decimal total_horas_categorias, Aux_planilla_gastos_horas_hombre aux)
        {
            divTabla.Controls.Clear();

            HtmlTable tabla = new HtmlTable();
            tabla.Attributes.Add("class", "table table-bordered");

            HtmlTableRow fila_encabezado = new HtmlTableRow();
            fila_encabezado.Controls.Add(new HtmlTableCell("th") { ColSpan = 2, RowSpan = 2 });
            fila_encabezado.Controls.Add(new HtmlTableCell("th") { ColSpan = 2, InnerHtml = "TOTAL HS ACUM.", BgColor = "lightgray" });
            fila_encabezado.Controls.Add(new HtmlTableCell("th") { InnerHtml = "Insumos taller", BgColor = "lightgray" });
            fila_encabezado.Controls.Add(new HtmlTableCell("th") { InnerHtml = "Herramientas", BgColor = "lightgray" });
            fila_encabezado.Controls.Add(new HtmlTableCell("th") { InnerHtml = "Viaticos", BgColor = "lightgray" });
            fila_encabezado.Controls.Add(new HtmlTableCell("th") { InnerHtml = "Viaticos presupuestados", BgColor = "lightgray" });
            fila_encabezado.Controls.Add(new HtmlTableCell("th") { InnerHtml = "TOTAL GASTOS", BgColor = "lightgray" });

            HtmlTableRow fila_encabezado1 = new HtmlTableRow();
            fila_encabezado1.Controls.Add(new HtmlTableCell("th") { InnerHtml = total_horas_categorias.ToString("#,##0.00") });
            fila_encabezado1.Controls.Add(new HtmlTableCell("th") { InnerHtml = "100 %" });
            fila_encabezado1.Controls.Add(new HtmlTableCell("th") { InnerHtml = aux.insumos_taller.ToString("$ #,##0.00") });
            fila_encabezado1.Controls.Add(new HtmlTableCell("th") { InnerHtml = aux.herramientas.ToString("$ #,##0.00") });
            fila_encabezado1.Controls.Add(new HtmlTableCell("th") { InnerHtml = aux.viaticos.ToString("$ #,##0.00") });
            fila_encabezado1.Controls.Add(new HtmlTableCell("th") { InnerHtml = aux.viaticos_presupuestados.ToString("$ #,##0.00") });
            fila_encabezado1.Controls.Add(new HtmlTableCell("th") { InnerHtml = (aux.insumos_taller + aux.herramientas + aux.viaticos + aux.viaticos_presupuestados).ToString("C2") });

            tabla.Controls.Add(fila_encabezado);
            tabla.Controls.Add(fila_encabezado1);

            bool primera_fila_categoria = true;

            foreach (Itemtabla item in items)
            {
                if (item.equipo != "TOTALES")
                {
                    HtmlTableRow fila = new HtmlTableRow();

                    if (primera_fila_categoria)
                    {
                        primera_fila_categoria = false;
                        int filas_categoria = items.Count(x => x.categoria == item.categoria) - 1;
                        fila.Controls.Add(new HtmlTableCell("td") { RowSpan = filas_categoria, InnerHtml = item.categoria, BgColor = "lightgray" });
                    }

                    fila.Controls.Add(new HtmlTableCell("td") { InnerHtml = item.equipo, BgColor = "lightgray" });
                    fila.Controls.Add(new HtmlTableCell("td") { InnerHtml = item.horas.ToString("#,##0.00") });
                    fila.Controls.Add(new HtmlTableCell("td") { InnerHtml = item.porcentaje.ToString("P2") });
                    fila.Controls.Add(new HtmlTableCell("td") { InnerHtml = item.monto_insumos_taller.ToString("$ #,##0.00") });
                    fila.Controls.Add(new HtmlTableCell("td") { InnerHtml = item.monto_herramientas.ToString("$ #,##0.00") });
                    fila.Controls.Add(new HtmlTableCell("td") { InnerHtml = item.monto_viaticos.ToString("$ #,##0.00") });
                    fila.Controls.Add(new HtmlTableCell("td") { InnerHtml = item.monto_viaticos_presupuestados.ToString("$ #,##0.00") });
                    fila.Controls.Add(new HtmlTableCell("td") { InnerHtml = (item.monto_herramientas + item.monto_insumos_taller + item.monto_viaticos + item.monto_viaticos_presupuestados).ToString("C2") });

                    tabla.Controls.Add(fila);
                }
                else
                {
                    primera_fila_categoria = true;
                    HtmlTableRow fila = new HtmlTableRow();
                    fila.Controls.Add(new HtmlTableCell("td") { ColSpan = 8 });
                    fila.Controls.Add(new HtmlTableCell("th") { InnerHtml = (item.monto_herramientas + item.monto_insumos_taller + item.monto_viaticos + item.monto_viaticos_presupuestados).ToString("C2"), BgColor = "lightgray" });

                    tabla.Controls.Add(fila);
                }

            }

            divTabla.Controls.Add(tabla);
        }

        public struct Itemtabla
        {
            public string categoria { get; set; }
            public string equipo { get; set; }
            public decimal horas { get; set; }
            public decimal porcentaje { get; set; }
            public decimal monto_insumos_taller { get; set; }
            public decimal monto_herramientas { get; set; }
            public decimal monto_viaticos { get; set; }
            public decimal monto_viaticos_presupuestados { get; set; }
        }

        protected void btn_guardar_modificaciones_ServerClick(object sender, EventArgs e)
        {
            using (var cxt = new Model1Container())
            {
                int mes = Convert.ToInt32(Session["planilla_gastos_en_funcion_horas_hombre_mes"]);
                int anio = Convert.ToInt32(Session["planilla_gastos_en_funcion_horas_hombre_anio"]);
                string categoria_empleado = Session["planilla_gastos_en_funcion_horas_hombre_tipo_empleado"].ToString();

                Aux_planilla_gastos_horas_hombre aux = cxt.Aux_planilla_gastos_horas_hombres.FirstOrDefault(x => x.anio == anio && x.mes == mes && x.tipo_empleado == categoria_empleado);
                if (aux == null)
                {
                    aux = new Aux_planilla_gastos_horas_hombre();
                    aux.mes = mes;
                    aux.anio = anio;
                    aux.tipo_empleado = categoria_empleado;
                    aux.insumos_taller = 0;
                    aux.herramientas = 0;
                    aux.viaticos = 0;
                    aux.viaticos_presupuestados = 0;
                    cxt.Aux_planilla_gastos_horas_hombres.Add(aux);
                }

                decimal insumos_taller = 0;
                decimal herramientas = 0;
                decimal viaticos = 0;
                decimal viaticos_presupuestados = 0;

                decimal.TryParse(tb_insumos_taller.Text, out insumos_taller);
                decimal.TryParse(tb_herramientas.Text, out herramientas);
                decimal.TryParse(tb_viaticos.Text, out viaticos);
                decimal.TryParse(tb_viaticos_presupuestados.Text, out viaticos_presupuestados);

                aux.insumos_taller = insumos_taller;
                aux.herramientas = herramientas;
                aux.viaticos = viaticos;
                aux.viaticos_presupuestados = viaticos_presupuestados;

                cxt.SaveChanges();

            }

            CargarTodosLosDatos(true);
        }

        protected void btn_ver_planilla_calculos_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Aplicativo/planilla_calculos.aspx");
        }

        protected void btn_ver_planilla_principal_ServerClick(object sender, EventArgs e)
        {
            Session["planilla_principal_mes"] = Convert.ToInt32(Session["planilla_gastos_en_funcion_horas_hombre_mes"]);
            Session["planilla_principal_anio"] = Convert.ToInt32(Session["planilla_gastos_en_funcion_horas_hombre_anio"]);
            Session["planilla_principal_tipo_empleado"] = Session["planilla_gastos_en_funcion_horas_hombre_tipo_empleado"].ToString();

            Response.Redirect("~/Aplicativo/admin_horas_planilla_principal.aspx");
        }
    }
}