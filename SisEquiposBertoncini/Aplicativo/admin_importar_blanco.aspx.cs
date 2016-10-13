using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using System.IO;
using System.Data;
using SisEquiposBertoncini.Aplicativo.Controles;
using SisEquiposBertoncini.Aplicativo.Datos;
using System.Runtime.InteropServices;

namespace SisEquiposBertoncini.Aplicativo
{
    public partial class admin_importar_blanco : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                menu_admin.Activar_Li("li_importar0ulfacturacion");
            }
        }

        private struct RegistroPorInsertar
        {
            public string Nombre_Equipo { get; set; }
            public string Nombre_item { get; set; }
            public DateTime Dia { get; set; }
            public decimal Monto { get; set; }
            public string Comentario { get; set; }
        }

        private struct RegistroSinMachear
        {
            public string Nombre_Equipo_informado { get; set; }
            public string Nombre_equipo_DB { get; set; }
            public string Nombre_item_informado { get; set; }
            public string Nombre_item_DB { get; set; }
            public DateTime Dia { get; set; }
            public decimal Monto { get; set; }
            public string Comentario { get; set; }
            public string Motivo { get; set; }
        }

        private void ProcesarDataTable(DataTable dt)
        {
            DataTable dtret = new DataTable();
            List<RegistroPorInsertar> aInsertar = new List<RegistroPorInsertar>();
            List<RegistroSinMachear> sinMatchear = new List<RegistroSinMachear>();
            List<Planilla_combustible> items_combustible = new List<Planilla_combustible>();
            Model1Container cxt = new Model1Container();

            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    if ((dr[0].ToString().Contains("F") || dr[0].ToString() == "RE") && (
                                    dr[33].ToString() == "Camiones y Carretones" ||
                                    dr[33].ToString() == "Grúas" ||
                                    dr[33].ToString() == "Trabajos Especiales" ||
                                    dr[33].ToString() == "Taller de Mantenimiento" ||
                                    dr[33].ToString() == "Taller de Soldadura" ||
                                    dr[33].ToString() == "Ventas"
                                ))
                    {
                        string nombreEquipo = CrossClass.ObtenerEquipoDB(dr[31].ToString());
                        string nombreItem = CrossClass.ObtenerItemDB(dr[27].ToString());
                        string nombre_equipo_combustible = nombreEquipo;//el equipo que figura en la linea es al que se le asigna la linea de combustible, si la misma tiene un "lugar" (trabajo) es a este al cual se le tiene que asignar el movimiento de entrada salida, por eso si es combuistible cambio el nombre del equipo al que a futuro asignare el gasto de entrada salida

                        bool _existe_equipo = cxt.Equipos.FirstOrDefault(ee => ee.nombre == nombreEquipo) != null;
                        bool _existe_item = cxt.Items_ingresos_egresos.FirstOrDefault(ii => ii.nombre == nombreItem) != null;

                        if (nombreEquipo != "Sin clasificar" && nombreItem != "Sin clasificar" && _existe_equipo && _existe_item)
                        {
                            bool correcto = true; //hasta aca el item es correcto, verifico si es combustible y se estan bien cargados los datos, si es asi se agrega a correctos, sino naranja
                            decimal monto = Convert.ToDecimal(dr[24].ToString()) > 0 ? Convert.ToDecimal(dr[24].ToString()) : Convert.ToDecimal(dr[3].ToString());
                            if (dr[12].ToString() == "A")
                            {
                                monto = monto + Convert.ToDecimal(dr[36].ToString());
                            }

                            if (nombreItem == "Combustible")
                            {
                                string[] campos_combustible = dr[18].ToString().Split(';');
                                if (campos_combustible.Length == 4 || campos_combustible.Length == 5)
                                {
                                    string chofer = campos_combustible[0];
                                    bool tanque_lleno = campos_combustible[1] == "S";
                                    decimal km = 0;
                                    decimal litros = 0;
                                    decimal.TryParse(campos_combustible[2].Replace('.', ','), out km);
                                    decimal.TryParse(campos_combustible[3].Replace('.', ','), out litros);

                                    Equipo eq;

                                    if (campos_combustible.Length == 5)
                                    {
                                        nombreEquipo = campos_combustible[4];
                                    }

                                    eq = cxt.Equipos.FirstOrDefault(ee => ee.nombre == nombreEquipo);
                                    Equipo eq_combustible = cxt.Equipos.FirstOrDefault(ee => ee.nombre == nombre_equipo_combustible);

                                    if (eq != null)
                                    {
                                        if (eq.nombre == eq_combustible.nombre && campos_combustible.Length == 5)
                                        {
                                            //vino nombre de equipo y el mismo es igual el equipo asignado es decir va en el item "Gastos Camioneta Individ."
                                            nombreItem = "Gastos Camioneta Individ.";
                                        }

                                        items_combustible.Add(new Planilla_combustible()
                                        {
                                            id_equipo = eq_combustible.id_equipo,
                                            fecha = Convert.ToDateTime(dr[7].ToString()),
                                            chofer = chofer,
                                            tanque_lleno = tanque_lleno,
                                            km = km,
                                            litros = litros,
                                            costo_total_facturado = monto,
                                            promedio = 0,
                                            playa = false,
                                            lugar = eq.nombre == eq_combustible.nombre ? (campos_combustible.Length == 5 ? eq_combustible.nombre : " - ") : eq.nombre
                                        });
                                    }
                                    else
                                    {
                                        correcto = false;
                                        RegistroSinMachear rsm = new RegistroSinMachear();
                                        rsm.Nombre_equipo_DB = nombreEquipo;
                                        rsm.Nombre_item_DB = nombreItem;
                                        rsm.Nombre_Equipo_informado = dr[31].ToString();
                                        rsm.Nombre_item_informado = dr[27].ToString();
                                        rsm.Comentario = dr[18].ToString();
                                        rsm.Dia = Convert.ToDateTime(dr[8].ToString());
                                        rsm.Monto = monto;
                                        rsm.Motivo = "No se encontro el equipo/trabajo informado para agendar el egreso.-";
                                        sinMatchear.Add(rsm);
                                    }

                                }
                                else
                                {
                                    correcto = false;
                                    RegistroSinMachear rsm = new RegistroSinMachear();
                                    rsm.Nombre_equipo_DB = nombreEquipo;
                                    rsm.Nombre_item_DB = nombreItem;
                                    rsm.Nombre_Equipo_informado = dr[31].ToString();
                                    rsm.Nombre_item_informado = dr[27].ToString();
                                    rsm.Comentario = dr[18].ToString();
                                    rsm.Dia = Convert.ToDateTime(dr[8].ToString());
                                    rsm.Monto = monto;
                                    rsm.Motivo = "Observaciones sin formato preestablecido";
                                    sinMatchear.Add(rsm);
                                }
                            }

                            if (correcto)
                            {
                                //correcto
                                RegistroPorInsertar ri = new RegistroPorInsertar();
                                ri.Nombre_Equipo = nombreEquipo;
                                ri.Nombre_item = nombreItem;
                                ri.Comentario = dr[18].ToString();
                                ri.Dia = Convert.ToDateTime(dr[34].ToString());
                                ri.Monto = monto;
                                aInsertar.Add(ri);
                            }
                        }
                        else
                        {
                            //correcto pero sin matchear
                            if (dr[31].ToString() != "NO ES VEHÍCULO")
                            {
                                RegistroSinMachear rsm = new RegistroSinMachear();
                                rsm.Nombre_equipo_DB = nombreEquipo;
                                rsm.Nombre_item_DB = nombreItem;
                                rsm.Nombre_Equipo_informado = dr[31].ToString();
                                rsm.Nombre_item_informado = dr[27].ToString();
                                rsm.Comentario = dr[18].ToString();
                                rsm.Dia = Convert.ToDateTime(dr[8].ToString());
                                rsm.Monto = Convert.ToDecimal(dr[24].ToString()) > 0 ? Convert.ToDecimal(dr[24].ToString()) : Convert.ToDecimal(dr[3].ToString());
                                rsm.Motivo = "Equipo o item informado en excel sin machear con valores en base de datos";
                                sinMatchear.Add(rsm);
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            lbl_cantidad_correctos.Text = aInsertar.Count().ToString();

            Session["items_correctos"] = aInsertar;
            Session["items_combustible"] = items_combustible;

            GridView2.DataSource = aInsertar;
            GridView2.DataBind();
            GridView3.DataSource = sinMatchear;
            GridView3.DataBind();

            div_resultados.Visible = true;
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (
                        ((System.Data.DataRowView)e.Row.DataItem).Row.ItemArray[0].ToString().Contains("F") &&
                            (
                                ((System.Data.DataRowView)e.Row.DataItem).Row.ItemArray[33].ToString() == "Camiones y Carretones" ||
                                ((System.Data.DataRowView)e.Row.DataItem).Row.ItemArray[33].ToString() == "Grúas" ||
                                ((System.Data.DataRowView)e.Row.DataItem).Row.ItemArray[33].ToString() == "Trabajos Especiales" ||
                                ((System.Data.DataRowView)e.Row.DataItem).Row.ItemArray[33].ToString() == "Taller de Mantenimiento" ||
                                ((System.Data.DataRowView)e.Row.DataItem).Row.ItemArray[33].ToString() == "Taller de Soldadura" ||
                                ((System.Data.DataRowView)e.Row.DataItem).Row.ItemArray[33].ToString() == "Ventas"
                            ) &&
                        CrossClass.ObtenerEquipoDB(((System.Data.DataRowView)e.Row.DataItem).Row.ItemArray[31].ToString()) != "Sin clasificar" &&
                        CrossClass.ObtenerItemDB(((System.Data.DataRowView)e.Row.DataItem).Row.ItemArray[27].ToString()) != "Sin clasificar"
                        )
                    {
                        e.Row.BackColor = System.Drawing.Color.LightGray;
                    }
                }
            }
            catch
            {

            }

        }

        protected void btn_importar_Click(object sender, EventArgs e)
        {
            HttpPostedFile file = data.PostedFile;

            if (file != null && file.ContentLength > 0)
            {

                string fname = Path.GetFileName(file.FileName);

                if (Path.GetExtension(fname) == ".xlsx" || Path.GetExtension(fname) == ".xls")
                {
                    Guid file_temp = Guid.NewGuid();
                    string path_temp = System.Web.HttpRuntime.AppDomainAppPath + "Temp\\";
                    string full_file_temp_path_xls = path_temp + file_temp + Path.GetExtension(fname);
                    string full_file_temp_path_xlsx = full_file_temp_path_xls;

                    file.SaveAs(full_file_temp_path_xls);

                    try
                    {
                        if (Path.GetExtension(full_file_temp_path_xls) == ".xls")
                        {
                            var app = new Microsoft.Office.Interop.Excel.Application();
                            app.DisplayAlerts = false;
                            var wb = app.Workbooks.Open(Filename: full_file_temp_path_xls, ReadOnly: true, Editable: false, Notify: false);

                            full_file_temp_path_xlsx = full_file_temp_path_xls + "x";
                            wb.SaveAs(Filename: full_file_temp_path_xlsx, FileFormat: Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook);
                            wb.Close();
                            app.Quit();
                        }


                        if (Path.GetExtension(full_file_temp_path_xlsx) == ".xlsx")
                        {

                            StreamReader sr = new StreamReader(full_file_temp_path_xlsx);
                            ExcelPackage package = new ExcelPackage(sr.BaseStream);
                            DataTable dt = package.ToDataTable();
                            Session["table_importada"] = dt;

                            GridView1.DataSource = dt;
                            GridView1.DataBind();

                            ProcesarDataTable(dt);
                            RefrescarTablas();
                        }
                    }
                    catch (Exception ex)
                    {
                        Aplicativo.Controles.MessageBox.Show(this, "Ocurrio un error al procesar el archivo verifique que el mismo no este protegido " + ex.Message, Controles.MessageBox.Tipo_MessageBox.Danger);
                    }
                }
                else
                {
                    Aplicativo.Controles.MessageBox.Show(this, "El archivo debe ser excel verifique la extencion deberá ser .xls o .xlsx", Controles.MessageBox.Tipo_MessageBox.Warning);
                }
            }
            else
            {
                Aplicativo.Controles.MessageBox.Show(this, "Primero debe seleccionar un archivo.", Controles.MessageBox.Tipo_MessageBox.Warning);
            }



        }

        protected void btn_impactar_ServerClick(object sender, EventArgs e)
        {
            List<RegistroPorInsertar> items = Session["items_correctos"] as List<RegistroPorInsertar>;
            List<Planilla_combustible> items_combustible = Session["items_combustible"] as List<Planilla_combustible>;

            using (var cxt = new Model1Container())
            {
                int cantidad_registros_ingreso_egreso = 0;
                int cantidad_registros_tabla_combustible = 0;

                foreach (RegistroPorInsertar item in items)
                {
                    Equipo eq = cxt.Equipos.First(ee => ee.nombre == item.Nombre_Equipo);

                    if (eq != null)
                    {

                        #region Copio y pego el metodo "Equipo.Agregar_detalle" porque no encuentro el motivo por el cual si hay mas de dos items para un mismo equipo agrega unicamente el primero

                        int mes = item.Dia.Month;
                        int anio = item.Dia.Year;

                        Ingreso_egreso_mensual_equipo iemensual = eq.Ingresos_egresos_mensuales.FirstOrDefault(x => x.anio == anio && x.mes == mes);

                        if (iemensual == null)
                        {
                            iemensual = new Ingreso_egreso_mensual_equipo();
                            iemensual.id_equipo = eq.id_equipo;
                            iemensual.mes = mes;
                            iemensual.anio = anio;
                            cxt.Ingresos_egresos_mensuales_equipos.Add(iemensual);
                            cxt.SaveChanges();
                        }


                        Item_ingreso_egreso item_io = cxt.Items_ingresos_egresos.FirstOrDefault(x => x.nombre == item.Nombre_item);

                        if (item_io != null)
                        {
                            Valor_mes valor_mes = iemensual.Valores_meses.FirstOrDefault(x => x.id_item == item_io.id_item);
                            if (valor_mes == null)
                            {
                                valor_mes = new Valor_mes();
                                valor_mes.id_ingreso_egreso_mensual = iemensual.id_ingreso_egreso_mensual;
                                valor_mes.id_item = item_io.id_item;
                                valor_mes.valor = 0;
                                cxt.Valores_meses.Add(valor_mes);
                                cxt.SaveChanges();
                            }

                            //aca tengo el item valor mes del 
                            string descripcion_detalle = "[Importado] " + item.Comentario;

                            Detalle_valor_item_mes detalle = valor_mes.Detalle.FirstOrDefault(x => x.descripcion == descripcion_detalle && x.fecha == item.Dia && x.monto == item.Monto);

                            if (detalle == null)
                            {
                                detalle = new Detalle_valor_item_mes();
                                detalle.id_valor_mes = valor_mes.id;
                                detalle.fecha = item.Dia;
                                detalle.descripcion = descripcion_detalle;
                                detalle.monto = item.Monto;
                                cxt.Detalle_valores_items_mes.Add(detalle);
                                cxt.SaveChanges();
                            }
                        }

                        #endregion

                        cantidad_registros_ingreso_egreso++;
                    }
                }

                foreach (Planilla_combustible item in items_combustible)
                {
                    Planilla_combustible item_planilla = cxt.Planilla_combustibles.FirstOrDefault(pc =>
                                                                                pc.id_equipo == item.id_equipo &&
                                                                                pc.fecha == item.fecha &&
                                                                                pc.chofer == item.chofer &&
                                                                                pc.litros == item.litros &&
                                                                                pc.tanque_lleno == item.tanque_lleno &&
                                                                                pc.km == item.km &&
                                                                                pc.costo_total_facturado == item.costo_total_facturado);
                    if (item_planilla == null)
                    {
                        cxt.Planilla_combustibles.Add(new Planilla_combustible()
                        {
                            fecha = item.fecha,
                            chofer = item.chofer,
                            id_equipo = item.id_equipo,
                            tanque_lleno = item.tanque_lleno,
                            litros = item.litros,
                            km = item.km,
                            costo_total_facturado = item.costo_total_facturado,
                            promedio = item.promedio,
                            playa = item.playa,
                            lugar = item.lugar
                        });
                    }
                    else
                    {
                        item_planilla.fecha = item.fecha;
                        item_planilla.chofer = item.chofer;
                        item_planilla.id_equipo = item.id_equipo;
                        item_planilla.tanque_lleno = item.tanque_lleno;
                        item_planilla.litros = item.litros;
                        item_planilla.km = item.km;
                        item_planilla.costo_total_facturado = item.costo_total_facturado;
                        item_planilla.promedio = item.promedio;
                        item_planilla.playa = item.playa;
                        item_planilla.lugar = item.lugar;
                    }

                    cantidad_registros_tabla_combustible++;
                }

                cxt.SaveChanges();
            }

            MessageBox.Show(this, "Registros importados correctamente", MessageBox.Tipo_MessageBox.Success, "Perfecto!");
        }

        protected void Gridvew_PreRender(object sender, EventArgs e)
        {
            if (GridView1.Rows.Count > 0)
            {
                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if (GridView2.Rows.Count > 0)
            {
                GridView2.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if (GridView3.Rows.Count > 0)
            {
                GridView3.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        private void RefrescarTablas()
        {
            string script = "<script language=\"javascript\"  type=\"text/javascript\">$('#<%= GridView1.ClientID %>').DataTable.ajax.draw(); $('#<%= GridView2.ClientID %>').DataTable.ajax.draw(); $('#<%= GridView3.ClientID %>').DataTable.ajax.draw();</script>";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "RefrescarTablas", script, false);
        }
    }
}

