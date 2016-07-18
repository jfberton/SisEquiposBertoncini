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
            Usuario usuariologueado = Session["UsuarioLogueado"] as Usuario;

            if (usuariologueado.perfil == perfil_usuario.Jefe)
            {
                div_agregar_equipo.Visible = false;
            }

            if (usuariologueado.perfil == perfil_usuario.Jefe)
            {
                table_edit.Visible = false;
                table_view.Visible = true;
                div_agregar_equipo.Visible = false;
            }
            else
            {
                table_edit.Visible = true;
                table_view.Visible = false;
                div_agregar_equipo.Visible = true;
            }

            if (!IsPostBack)
            {
                table_edit.Visible = false;
                table_view.Visible = false;
                Cargar_ddls();
            }
            else
            {
                if (btn_nueva_busqueda.Visible)
                {
                    CargarDatos();
                }
            }
        }

        private decimal ObtenerValor(string valor)
        {
            decimal ret = 0;
            if (valor.Contains("$"))
            {
                decimal.TryParse(valor.Replace("$", "").Replace(".", ""), out ret);
            }
            else
            {
                decimal.TryParse(valor.Replace(".", ","), out ret);
            }

            return ret;
        }

        protected void btn_buscar_Click(object sender, EventArgs e)
        {
            Usuario usuariologueado = Session["UsuarioLogueado"] as Usuario;
            using (var cxt = new Model1Container())
            {
                int anio = Convert.ToInt32(ddl_anio.SelectedItem.Text);
                int mes = Convert.ToInt32(ddl_mes.SelectedItem.Value);
                var datos_planilla = cxt.Planilla_gastos_administrativo.FirstOrDefault(x => x.anio == anio && x.mes == mes);

                if (datos_planilla != null)
                {
                    tb_telefonia_celular.Text = lbl_telefonia_celular.Text = Cadena.Formato_moneda(datos_planilla.monto_telefonia_celular, Cadena.Moneda.pesos);
                    tb_sueldos_administracion.Text = lbl_sueldos_administracion.Text = Cadena.Formato_moneda(datos_planilla.monto_sueldos, Cadena.Moneda.pesos);
                    tb_honorarios_contables.Text = lbl_honorarios_contables.Text = Cadena.Formato_moneda(datos_planilla.monto_honorarios_contables, Cadena.Moneda.pesos);
                    tb_honorarios_sistema.Text = lbl_honorarios_varios.Text = Cadena.Formato_moneda(datos_planilla.monto_honorarios_sistema, Cadena.Moneda.pesos);
                    tb_papeleria_libreria.Text = lbl_papeleria.Text = Cadena.Formato_moneda(datos_planilla.monto_papeleria, Cadena.Moneda.pesos);
                    tb_otros.Text = lbl_otros.Text = Cadena.Formato_moneda(datos_planilla.monto_otros, Cadena.Moneda.pesos);
                }
            }

            if (usuariologueado.perfil == perfil_usuario.Jefe)
            {
                table_edit.Visible = false;
                table_view.Visible = true;
            }
            else
            {
                table_edit.Visible = true;
                table_view.Visible = false;
            }

            CargarDatos();
            btn_buscar.Visible = false;
            btn_nueva_busqueda.Visible = true;
        }

        protected void btn_nueva_busqueda_Click(object sender, EventArgs e)
        {
            btn_buscar.Visible = true;
            btn_nueva_busqueda.Visible = false;
            LimpiarTodo();
        }

        private void LimpiarTodo()
        {
            div_detalle.Controls.Clear();
            tb_telefonia_celular.Text = "";
            tb_sueldos_administracion.Text = "";
            tb_honorarios_contables.Text = "";
            tb_honorarios_sistema.Text = "";
            tb_papeleria_libreria.Text = "";
            tb_otros.Text = "";
        }

        private struct itemTabla
        {
            public int Equipo_id { get; set; }
            public string Equipo { get; set; }
            public decimal Porc { get; set; }
            public decimal Sueldo { get; set; }
            public decimal Honor_varios { get; set; }
            public decimal Honor_contable { get; set; }
            public decimal Papel_libre { get; set; }
            public decimal Otros { get; set; }
            public decimal Telefono { get; set; }
            public decimal Total { get; set; }
        }

        private void CargarDatos()
        {
            Usuario usuariologueado = Session["UsuarioLogueado"] as Usuario;
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

                decimal porcentaje_restante = Convert.ToDecimal(1) - datos_planilla.Detalle.Sum(x => x.porcentaje);
                lbl_maximo_nivel_porcentaje_participacion.Text = porcentaje_restante.ToString("P2");

                HtmlTable tabla = new HtmlTable();
                tabla.Attributes.Add("class", "table table-bordered");
                HtmlTableRow encabezado = new HtmlTableRow();
                encabezado.Cells.Add(new HtmlTableCell("th") { RowSpan = 2, InnerHtml = "Equipos", BgColor = "lightgray" });
                encabezado.Cells.Add(new HtmlTableCell("th") { InnerHtml = "Porc. Asignado", BgColor = "lightgray" });
                encabezado.Cells.Add(new HtmlTableCell("th") { InnerHtml = "Sueldos", BgColor = "lightgray" });
                encabezado.Cells.Add(new HtmlTableCell("th") { InnerHtml = "Honorario Varios.", BgColor = "lightgray" });
                encabezado.Cells.Add(new HtmlTableCell("th") { InnerHtml = "Honorario Cont.", BgColor = "lightgray" });
                encabezado.Cells.Add(new HtmlTableCell("th") { InnerHtml = "Papel. - Libre.", BgColor = "lightgray" });
                encabezado.Cells.Add(new HtmlTableCell("th") { InnerHtml = "Otros", BgColor = "lightgray" });
                encabezado.Cells.Add(new HtmlTableCell("th") { InnerHtml = "Teléfono", BgColor = "lightgray" });
                encabezado.Cells.Add(new HtmlTableCell("th") { InnerHtml = "Total sin telefonia", BgColor = "lightgray" });
                encabezado.Cells.Add(new HtmlTableCell("th") { BgColor = "lightgray" });

                HtmlTableRow encabezado1 = new HtmlTableRow();
                encabezado1.Cells.Add(new HtmlTableCell("th") { InnerHtml = datos_planilla.Detalle.Sum(x => x.porcentaje).ToString("P2"), BgColor = "lightgray" });
                encabezado1.Cells.Add(new HtmlTableCell("th") { InnerHtml = datos_planilla.monto_sueldos.ToString("$ #,##0.00"), BgColor = "lightgray" });
                encabezado1.Cells.Add(new HtmlTableCell("th") { InnerHtml = datos_planilla.monto_honorarios_sistema.ToString("$ #,##0.00"), BgColor = "lightgray" });
                encabezado1.Cells.Add(new HtmlTableCell("th") { InnerHtml = datos_planilla.monto_honorarios_contables.ToString("$ #,##0.00"), BgColor = "lightgray" });
                encabezado1.Cells.Add(new HtmlTableCell("th") { InnerHtml = datos_planilla.monto_papeleria.ToString("$ #,##0.00"), BgColor = "lightgray" });
                encabezado1.Cells.Add(new HtmlTableCell("th") { InnerHtml = datos_planilla.monto_otros.ToString("$ #,##0.00"), BgColor = "lightgray" });
                encabezado1.Cells.Add(new HtmlTableCell("th") { InnerHtml = datos_planilla.monto_telefonia_celular.ToString("$ #,##0.00"), BgColor = "lightgray" });
                decimal montoTotal = datos_planilla.monto_sueldos + datos_planilla.monto_honorarios_sistema + datos_planilla.monto_honorarios_contables + datos_planilla.monto_papeleria + datos_planilla.monto_otros;
                encabezado1.Cells.Add(new HtmlTableCell("th") { InnerHtml = montoTotal.ToString("$ #,##0.00"), BgColor = "lightgray" });
                encabezado1.Cells.Add(new HtmlTableCell("th") { BgColor = "lightgray" });

                tabla.Rows.Add(encabezado);
                tabla.Rows.Add(encabezado1);

                #region recorro los equipos, actualizo la base de datos con el monto gastos administrativo
                List<itemTabla> itemsTabla = new List<itemTabla>();

                foreach (Equipo equipo in equipos)
                {
                    decimal monto = 0;
                    var detalle_equipo_planilla = datos_planilla.Detalle.FirstOrDefault(x => x.id_equipo == equipo.id_equipo);
                    if (equipo.nombre != "ventas")
                    {
                        if (detalle_equipo_planilla != null && detalle_equipo_planilla.porcentaje > 0)
                        {
                            monto = ((datos_planilla.monto_sueldos / Convert.ToDecimal(2)) * detalle_equipo_planilla.porcentaje) +
                                ((datos_planilla.monto_honorarios_sistema / Convert.ToDecimal(2)) * detalle_equipo_planilla.porcentaje) +
                                ((datos_planilla.monto_honorarios_contables / Convert.ToDecimal(2)) * detalle_equipo_planilla.porcentaje) +
                                ((datos_planilla.monto_papeleria / Convert.ToDecimal(2)) * detalle_equipo_planilla.porcentaje) +
                                ((datos_planilla.monto_otros / Convert.ToDecimal(2)) * detalle_equipo_planilla.porcentaje);

                            itemsTabla.Add(new itemTabla()
                            {
                                Equipo_id = equipo.id_equipo,
                                Equipo = equipo.nombre,
                                Porc = detalle_equipo_planilla.porcentaje,
                                Sueldo = ((datos_planilla.monto_sueldos / Convert.ToDecimal(2)) * detalle_equipo_planilla.porcentaje),
                                Honor_varios = ((datos_planilla.monto_honorarios_sistema / Convert.ToDecimal(2)) * detalle_equipo_planilla.porcentaje),
                                Honor_contable = ((datos_planilla.monto_honorarios_contables / Convert.ToDecimal(2)) * detalle_equipo_planilla.porcentaje),
                                Papel_libre = ((datos_planilla.monto_papeleria / Convert.ToDecimal(2)) * detalle_equipo_planilla.porcentaje),
                                Otros = ((datos_planilla.monto_otros / Convert.ToDecimal(2)) * detalle_equipo_planilla.porcentaje),
                                Telefono = (datos_planilla.monto_telefonia_celular * detalle_equipo_planilla.porcentaje),
                                Total = monto
                            });
                        }
                    }
                    else
                    {
                        //se llama ventas el equipo ventas lo agrego directamente a la tabla, el resto se agregará al recorrer el listado que genero con los equipos que no son ventas
                        HtmlTableRow fila_equipo = new HtmlTableRow();
                        fila_equipo.Attributes.Add("class", "alert-info");
                        fila_equipo.Cells.Add(new HtmlTableCell("td") { InnerHtml = "VENTAS"});

                        if (detalle_equipo_planilla == null)
                        {
                            detalle_equipo_planilla = new Aux_planilla_gasto_administracion() { id_equipo = equipo.id_equipo, porcentaje = 0 };
                            cxt.SaveChanges();
                        }

                        HtmlTableCell columna_porcentaje = new HtmlTableCell("td");
                        fila_equipo.Cells.Add(columna_porcentaje);
                        fila_equipo.Cells.Add(new HtmlTableCell("td") { InnerHtml = detalle_equipo_planilla != null ? ((datos_planilla.monto_sueldos / Convert.ToDecimal(2))).ToString("$ #,##0.00") : " - " });
                        fila_equipo.Cells.Add(new HtmlTableCell("td") { InnerHtml = detalle_equipo_planilla != null ? ((datos_planilla.monto_honorarios_sistema / Convert.ToDecimal(2))).ToString("$ #,##0.00") : " - " });
                        fila_equipo.Cells.Add(new HtmlTableCell("td") { InnerHtml = detalle_equipo_planilla != null ? ((datos_planilla.monto_honorarios_contables / Convert.ToDecimal(2))).ToString("$ #,##0.00") : " - " });
                        fila_equipo.Cells.Add(new HtmlTableCell("td") { InnerHtml = detalle_equipo_planilla != null ? ((datos_planilla.monto_papeleria / Convert.ToDecimal(2))).ToString("$ #,##0.00") : " - " });
                        fila_equipo.Cells.Add(new HtmlTableCell("td") { InnerHtml = detalle_equipo_planilla != null ? ((datos_planilla.monto_otros / Convert.ToDecimal(2))).ToString("$ #,##0.00") : " - " });
                        fila_equipo.Cells.Add(new HtmlTableCell("th"));

                        // en el monto se considera el 50% de lo ingresado excepto en telefonia celular
                        monto = detalle_equipo_planilla != null ?
                            ((datos_planilla.monto_sueldos / Convert.ToDecimal(2))) +
                            ((datos_planilla.monto_honorarios_sistema / Convert.ToDecimal(2))) +
                            ((datos_planilla.monto_honorarios_contables / Convert.ToDecimal(2))) +
                            ((datos_planilla.monto_papeleria / Convert.ToDecimal(2))) +
                            ((datos_planilla.monto_otros / Convert.ToDecimal(2))) : 0;

                        fila_equipo.Cells.Add(new HtmlTableCell("th") { InnerHtml = monto.ToString("$ #,##0.00") });

                        tabla.Rows.Add(fila_equipo);
                    }

                    if (detalle_equipo_planilla != null)
                    {
                        equipo.Agregar_detalle(Equipo.Valor_mensual.admin_telefonia, mes, anio, datos_planilla.monto_telefonia_celular * detalle_equipo_planilla.porcentaje);
                        equipo.Agregar_detalle(Equipo.Valor_mensual.admin_varios, mes, anio, monto);
                    }
                    else
                    {
                        equipo.Agregar_detalle(Equipo.Valor_mensual.admin_telefonia, mes, anio, 0);
                        equipo.Agregar_detalle(Equipo.Valor_mensual.admin_varios, mes, anio, 0);
                    }
                }

                #endregion

                #region recorro la lista que obtuve anteriormente y asigno los valores de manera ordenada a la tabla
                foreach (itemTabla item in itemsTabla.OrderByDescending(p => p.Porc))
                {
                    HtmlTableRow fila_equipo = new HtmlTableRow();
                    fila_equipo.Cells.Add(new HtmlTableCell("td") { InnerHtml = item.Equipo });

                    fila_equipo.Cells.Add(new HtmlTableCell("td") { InnerHtml = item.Porc.ToString("P2") });
                    fila_equipo.Cells.Add(new HtmlTableCell("td") { InnerHtml = item.Sueldo.ToString("$ #,##0.00")});
                    fila_equipo.Cells.Add(new HtmlTableCell("td") { InnerHtml = item.Honor_varios.ToString("$ #,##0.00")});
                    fila_equipo.Cells.Add(new HtmlTableCell("td") { InnerHtml = item.Honor_contable.ToString("$ #,##0.00")});
                    fila_equipo.Cells.Add(new HtmlTableCell("td") { InnerHtml = item.Papel_libre.ToString("$ #,##0.00")});
                    fila_equipo.Cells.Add(new HtmlTableCell("td") { InnerHtml = item.Otros.ToString("$ #,##0.00")});
                    fila_equipo.Cells.Add(new HtmlTableCell("th") { InnerHtml = item.Telefono.ToString("$ #,##0.00")});
                    fila_equipo.Cells.Add(new HtmlTableCell("th") { InnerHtml = item.Total.ToString("$ #,##0.00") });
                    
                    HtmlTableCell columna_eliminar = new HtmlTableCell("td");
                    if (usuariologueado.perfil != perfil_usuario.Jefe)
                    {
                        Button eliminar = new Button();
                        eliminar.Text = "Eliminar";
                        eliminar.CssClass = "btn btn-sm btn-danger";
                        eliminar.CommandArgument = item.Equipo_id.ToString();
                        eliminar.Click += Eliminar_Click;
                        columna_eliminar.Controls.Add(eliminar);
                    }
                    
                    fila_equipo.Cells.Add(columna_eliminar);

                    tabla.Rows.Add(fila_equipo);
                }
                #endregion


                div_detalle.Controls.Clear();

                div_detalle.Controls.Add(tabla);
            }
        }

        private void Eliminar_Click(object sender, EventArgs e)
        {
            int id_equipo = Convert.ToInt32(((Button)sender).CommandArgument);
            using (var cxt = new Model1Container())
            {
                int anio = Convert.ToInt32(ddl_anio.SelectedItem.Text);
                int mes = Convert.ToInt32(ddl_mes.SelectedItem.Value);
                var datos_planilla = cxt.Planilla_gastos_administrativo.FirstOrDefault(x => x.anio == anio && x.mes == mes);
                var detalle_equipo_planilla = datos_planilla.Detalle.FirstOrDefault(x => x.id_equipo == id_equipo);
                if (detalle_equipo_planilla != null)
                {
                    cxt.Aux_planilla_gastos_administracion.Remove(detalle_equipo_planilla);
                    cxt.SaveChanges();
                    CargarDatos();
                }
            }

        }

        private void Cargar_ddls()
        {
            for (int i = 2015; i <= DateTime.Today.Year + 1; i++)
            {
                ddl_anio.Items.Add(new ListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            using (var cxt = new Model1Container())
            {
                var equipos = cxt.Equipos.Where(x => !x.Generico && x.fecha_baja == null);
                foreach (Equipo item in equipos)
                {
                    ddl_equipos.Items.Add(new ListItem() { Text = item.nombre, Value = item.id_equipo.ToString() });

                }
            }
        }


        protected void btn_aplicar_Click(object sender, EventArgs e)
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

                datos_planilla.monto_telefonia_celular = ObtenerValor(tb_telefonia_celular.Text);
                datos_planilla.monto_sueldos = ObtenerValor(tb_sueldos_administracion.Text);
                datos_planilla.monto_honorarios_sistema = ObtenerValor(tb_honorarios_sistema.Text);
                datos_planilla.monto_honorarios_contables = ObtenerValor(tb_honorarios_contables.Text);
                datos_planilla.monto_papeleria = ObtenerValor(tb_papeleria_libreria.Text);
                datos_planilla.monto_otros = ObtenerValor(tb_otros.Text);

                cxt.SaveChanges();

                tb_telefonia_celular.Text = Cadena.Formato_moneda(datos_planilla.monto_telefonia_celular, Cadena.Moneda.pesos);
                tb_sueldos_administracion.Text = Cadena.Formato_moneda(datos_planilla.monto_sueldos, Cadena.Moneda.pesos);
                tb_honorarios_contables.Text = Cadena.Formato_moneda(datos_planilla.monto_honorarios_contables, Cadena.Moneda.pesos);
                tb_honorarios_sistema.Text = Cadena.Formato_moneda(datos_planilla.monto_honorarios_sistema, Cadena.Moneda.pesos);
                tb_papeleria_libreria.Text = Cadena.Formato_moneda(datos_planilla.monto_papeleria, Cadena.Moneda.pesos);
                tb_otros.Text = Cadena.Formato_moneda(datos_planilla.monto_otros, Cadena.Moneda.pesos);
            }

            CargarDatos();
        }

        protected void btn_agregar_equipo_Click(object sender, EventArgs e)
        {
            int id_equipo = Convert.ToInt32(ddl_equipos.SelectedValue);
            decimal porcentaje = ObtenerValor(tb_porcentaje.Text) / Convert.ToDecimal(100);
            if (porcentaje > 0)
            {
                using (var cxt = new Model1Container())
                {
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
            else
            {
                MessageBox.Show(this, "El porcentaje debe ser mayor a cero y menor o igual al máximo disponible", MessageBox.Tipo_MessageBox.Warning);
            }


        }
    }
}