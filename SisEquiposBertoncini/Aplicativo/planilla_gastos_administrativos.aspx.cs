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
    public partial class planilla_gastos_administrativos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Cargar_ddls();
                
            }
            CargarDatos();
        }

        private void CargarDatos()
        {
            using (var cxt = new Model1Container())
            {
                var equipos = cxt.Equipos.Where(x => !x.Generico && x.fecha_baja == null);
                int anio = Convert.ToInt32(ddl_anio.SelectedItem.Text);
                int mes = Convert.ToInt32(ddl_mes.SelectedItem.Value);
                var datos_planilla = cxt.Planilla_gastos_administrativo.FirstOrDefault(x => x.anio == anio && x.mes == mes);

                if (datos_planilla == null)
                {
                    datos_planilla = new Planilla_gasto_administrativo();
                    datos_planilla.mes = mes;
                    datos_planilla.anio = anio;
                    datos_planilla.monto_telefonia_celular = 0;
                    datos_planilla.monto_sueldos = 0;
                    datos_planilla.monto_honorarios_sistema = 0;
                    datos_planilla.monto_honorarios_contables = 0;
                    datos_planilla.monto_papeleria = 0;
                    datos_planilla.monto_otros = 0;
                    cxt.Planilla_gastos_administrativo.Add(datos_planilla);
                    cxt.SaveChanges();
                }

                tb_telefonia_celular.Text = datos_planilla.monto_telefonia_celular.ToString("$ #,##0.00");
                tb_sueldos_administracion.Text = datos_planilla.monto_sueldos.ToString("$ #,##0.00");
                tb_honorarios_contables.Text = datos_planilla.monto_honorarios_contables.ToString("$ #,##0.00");
                tb_honorarios_sistema.Text = datos_planilla.monto_honorarios_sistema.ToString("$ #,##0.00");
                tb_papeleria_libreria.Text = datos_planilla.monto_papeleria.ToString("$ #,##0.00");
                tb_otros.Text = datos_planilla.monto_otros.ToString("$ #,##0.00");

                HtmlTable tabla = new HtmlTable();
                tabla.Attributes.Add("class", "table table-bordered");
                HtmlTableRow encabezado = new HtmlTableRow();
                encabezado.Cells.Add(new HtmlTableCell("th") { RowSpan = 2, InnerHtml = "Equipos" });
                encabezado.Cells.Add(new HtmlTableCell("th") { InnerHtml = "%" });
                encabezado.Cells.Add(new HtmlTableCell("th") { InnerHtml = "Teléfono" });
                encabezado.Cells.Add(new HtmlTableCell("th") { InnerHtml = "Sueldos" });
                encabezado.Cells.Add(new HtmlTableCell("th") { InnerHtml = "Honorario Sist." });
                encabezado.Cells.Add(new HtmlTableCell("th") { InnerHtml = "Honorario Cont." });
                encabezado.Cells.Add(new HtmlTableCell("th") { InnerHtml = "Papel. - Libre." });
                encabezado.Cells.Add(new HtmlTableCell("th") { InnerHtml = "Otros" });
                encabezado.Cells.Add(new HtmlTableCell("th") { InnerHtml = "Total sin telefonia" });

                HtmlTableRow encabezado1 = new HtmlTableRow();
                encabezado1.Cells.Add(new HtmlTableCell("th") { InnerHtml = datos_planilla.Detalle.Sum(x=>x.porcentaje).ToString("P2") });
                encabezado1.Cells.Add(new HtmlTableCell("th") { InnerHtml = datos_planilla.monto_telefonia_celular.ToString("$ #,##0.00") });
                encabezado1.Cells.Add(new HtmlTableCell("th") { InnerHtml = datos_planilla.monto_sueldos.ToString("$ #,##0.00") });
                encabezado1.Cells.Add(new HtmlTableCell("th") { InnerHtml = datos_planilla.monto_honorarios_sistema.ToString("$ #,##0.00") });
                encabezado1.Cells.Add(new HtmlTableCell("th") { InnerHtml = datos_planilla.monto_honorarios_contables.ToString("$ #,##0.00") });
                encabezado1.Cells.Add(new HtmlTableCell("th") { InnerHtml = datos_planilla.monto_papeleria.ToString("$ #,##0.00") });
                encabezado1.Cells.Add(new HtmlTableCell("th") { InnerHtml = datos_planilla.monto_otros.ToString("$ #,##0.00") });

                decimal montoTotal = datos_planilla.monto_sueldos + datos_planilla.monto_honorarios_sistema + datos_planilla.monto_honorarios_contables + datos_planilla.monto_papeleria + datos_planilla.monto_otros;
                encabezado1.Cells.Add(new HtmlTableCell("th") { InnerHtml = montoTotal.ToString("$ #,##0.00") });
                
                tabla.Rows.Add(encabezado);
                tabla.Rows.Add(encabezado1);
   

                foreach (Equipo equipo in equipos)
                {
                    HtmlTableRow fila_equipo = new HtmlTableRow();
                    fila_equipo.Cells.Add(new HtmlTableCell("td") { InnerHtml = equipo.nombre });

                    var detalle_equipo_planilla = datos_planilla.Detalle.FirstOrDefault(x => x.id_equipo == equipo.id_equipo);

                    valor_decimal tb_equipo = (valor_decimal)LoadControl("~/Aplicativo/Controles/valor_decimal.ascx");

                    tb_equipo.ID = "tb_porcentaje_equipo_" + equipo.id_equipo.ToString();
                    tb_equipo.Formato_valor = valor_decimal.Formato.porcentaje;
                    tb_equipo.Valor_str = detalle_equipo_planilla != null ? detalle_equipo_planilla.porcentaje.ToString() : " 0 ";
                    tb_equipo.Modifico_valor += valor_decimal_Modifico_valor;

                    HtmlTableCell columna_porcentaje = new HtmlTableCell("td");
                    columna_porcentaje.Controls.Add(tb_equipo);
                    fila_equipo.Cells.Add(columna_porcentaje);
                    fila_equipo.Cells.Add(new HtmlTableCell("td") { InnerHtml = detalle_equipo_planilla != null ? (datos_planilla.monto_telefonia_celular * detalle_equipo_planilla.porcentaje).ToString("$ #,##0.00") : " - " });
                    fila_equipo.Cells.Add(new HtmlTableCell("td") { InnerHtml = detalle_equipo_planilla != null ? (datos_planilla.monto_sueldos * detalle_equipo_planilla.porcentaje).ToString("$ #,##0.00") : " - " });
                    fila_equipo.Cells.Add(new HtmlTableCell("td") { InnerHtml = detalle_equipo_planilla != null ? (datos_planilla.monto_honorarios_sistema * detalle_equipo_planilla.porcentaje).ToString("$ #,##0.00") : " - " });
                    fila_equipo.Cells.Add(new HtmlTableCell("td") { InnerHtml = detalle_equipo_planilla != null ? (datos_planilla.monto_honorarios_contables * detalle_equipo_planilla.porcentaje).ToString("$ #,##0.00") : " - " });
                    fila_equipo.Cells.Add(new HtmlTableCell("td") { InnerHtml = detalle_equipo_planilla != null ? (datos_planilla.monto_papeleria * detalle_equipo_planilla.porcentaje).ToString("$ #,##0.00") : " - " });
                    fila_equipo.Cells.Add(new HtmlTableCell("td") { InnerHtml = detalle_equipo_planilla != null ? (datos_planilla.monto_otros * detalle_equipo_planilla.porcentaje).ToString("$ #,##0.00") : " - " });

                    decimal monto = detalle_equipo_planilla != null ?
                        (datos_planilla.monto_sueldos * detalle_equipo_planilla.porcentaje) +
                        (datos_planilla.monto_honorarios_sistema * detalle_equipo_planilla.porcentaje) +
                        (datos_planilla.monto_honorarios_contables * detalle_equipo_planilla.porcentaje) +
                        (datos_planilla.monto_papeleria * detalle_equipo_planilla.porcentaje) +
                        (datos_planilla.monto_otros * detalle_equipo_planilla.porcentaje) : 0;

                    fila_equipo.Cells.Add(new HtmlTableCell("td") { InnerHtml = monto.ToString("$ #,##0.00") });

                    tabla.Rows.Add(fila_equipo);
                }
                
                div_detalle.Controls.Clear();

                div_detalle.Controls.Add(tabla);
            }
        }

        private void Cargar_ddls()
        {
            for (int i = 2015; i <= DateTime.Today.Year + 1; i++)
            {
                ddl_anio.Items.Add(new ListItem() { Text = i.ToString(), Value = i.ToString() });
            }
        }

        protected void btn_aplicar_cambios_Click(object sender, EventArgs e)
        {
            using (var cxt = new Model1Container())
            {
                var equipos = cxt.Equipos.Where(x => !x.Generico && x.fecha_baja == null);
                int anio = Convert.ToInt32(ddl_anio.SelectedItem.Text);
                int mes = Convert.ToInt32(ddl_mes.SelectedItem.Value);
                var datos_planilla = cxt.Planilla_gastos_administrativo.FirstOrDefault(x => x.anio == anio && x.mes == mes);

                if (datos_planilla == null)
                {
                    datos_planilla = new Planilla_gasto_administrativo();
                    datos_planilla.mes = mes;
                    datos_planilla.anio = anio;
                    datos_planilla.monto_telefonia_celular = 0;
                    datos_planilla.monto_sueldos = 0;
                    datos_planilla.monto_honorarios_sistema = 0;
                    datos_planilla.monto_honorarios_contables = 0;
                    datos_planilla.monto_papeleria = 0;
                    datos_planilla.monto_otros = 0;
                    cxt.Planilla_gastos_administrativo.Add(datos_planilla);
                    cxt.SaveChanges();
                }

                decimal monto_telefonia = 0;
                decimal monto_sueldo = 0;
                decimal monto_honorario_sistema = 0;
                decimal monto_honorario_contable = 0;
                decimal monto_papeleria = 0;
                decimal monto_otros = 0;

                datos_planilla.monto_telefonia_celular = decimal.TryParse(tb_telefonia_celular.Text.Replace("$",""), out monto_telefonia) ? monto_telefonia : 0;
                datos_planilla.monto_sueldos = decimal.TryParse(tb_sueldos_administracion.Text.Replace("$", ""), out monto_sueldo) ? monto_sueldo : 0;
                datos_planilla.monto_honorarios_sistema = decimal.TryParse(tb_honorarios_sistema.Text.Replace("$", ""), out monto_honorario_sistema) ? monto_honorario_sistema : 0;
                datos_planilla.monto_honorarios_contables = decimal.TryParse(tb_honorarios_contables.Text.Replace("$", ""), out monto_honorario_contable) ? monto_honorario_contable : 0;
                datos_planilla.monto_papeleria = decimal.TryParse(tb_papeleria_libreria.Text.Replace("$", ""), out monto_papeleria) ? monto_papeleria : 0;
                datos_planilla.monto_otros = decimal.TryParse(tb_otros.Text.Replace("$", ""), out monto_otros) ? monto_otros : 0;
                
                cxt.SaveChanges();
            }

            CargarDatos();
        }

        protected void valor_decimal_Modifico_valor(object sender, EventArgs e)
        {
            //MessageBox.Show(this, "Se modifico el valor el nuevo valor es " + valor_decimal.Valor.ToString());

            valor_decimal tb_equipo = ((valor_decimal)sender);

            using (var cxt = new Model1Container())
            {
                int id_equipo = Convert.ToInt32(tb_equipo.ID.Replace("tb_porcentaje_equipo_", ""));
                decimal porcentaje = tb_equipo.Valor;
                //decimal.TryParse(tb_equipo.Text, out porcentaje);

                if (porcentaje > 0)
                {
                    porcentaje = porcentaje / Convert.ToDecimal(100);

                    int anio = Convert.ToInt32(ddl_anio.SelectedItem.Text);
                    int mes = Convert.ToInt32(ddl_mes.SelectedItem.Value);
                    var datos_planilla = cxt.Planilla_gastos_administrativo.FirstOrDefault(x => x.anio == anio && x.mes == mes);

                    decimal porcentaje_restante = Convert.ToDecimal(1) - datos_planilla.Detalle.Sum(x => x.porcentaje);

                    if (porcentaje > porcentaje_restante)
                    {
                        porcentaje = porcentaje_restante;

                        var detalle_equipo_planilla = datos_planilla.Detalle.FirstOrDefault(x => x.id_equipo == id_equipo);

                        if (detalle_equipo_planilla == null)
                        {
                            datos_planilla.Detalle.Add(new Aux_planilla_gasto_administracion() { id_equipo = id_equipo, porcentaje = porcentaje });
                        }
                        else
                        {
                            detalle_equipo_planilla.porcentaje = porcentaje;
                        }

                        cxt.SaveChanges();

                        MessageBox.Show(this, "El porcentaje exede lo que resta para llegar a 100%, se asignará " + porcentaje_restante.ToString("P2"), MessageBox.Tipo_MessageBox.Warning);
                    }
                    else
                    {
                        var detalle_equipo_planilla = datos_planilla.Detalle.FirstOrDefault(x => x.id_equipo == id_equipo);

                        if (detalle_equipo_planilla == null)
                        {
                            datos_planilla.Detalle.Add(new Aux_planilla_gasto_administracion() { id_equipo = id_equipo, porcentaje = porcentaje });
                        }
                        else
                        {
                            detalle_equipo_planilla.porcentaje = porcentaje;
                        }

                        cxt.SaveChanges();
                    }
                }

                CargarDatos();
            }


        }
    }
}