using OfficeOpenXml;
using SisEquiposBertoncini.Aplicativo.Controles;
using SisEquiposBertoncini.Aplicativo.Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisEquiposBertoncini.Aplicativo
{
    public partial class admin_importar_ventas_presupuestadas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                menu_admin.Activar_Li("li_importar0ulpresupuestado");
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
            public string Nombre_item_informado { get; set; }
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

            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    DateTime fecha = Convert.ToDateTime(dr[0].ToString());
                    string cuenta = dr[1].ToString();
                    string sector = dr[2].ToString();
                    string equipo = dr[3].ToString();
                    string observ = dr[4].ToString();
                    decimal monto = Convert.ToDecimal(dr[5].ToString());
                    string cuenta_en_la_que_tiene_que_ir = dr[6].ToString();

                    using (var cxt = new Model1Container())
                    {
                        Categoria_equipo _sector = cxt.Categorias_equipos.FirstOrDefault(ce => ce.nombre == sector);
                        Equipo _equipo = cxt.Equipos.FirstOrDefault(ee => ee.nombre == equipo);
                        Item_ingreso_egreso _cuenta_en_la_que_va = cxt.Items_ingresos_egresos.FirstOrDefault(ii => ii.nombre == cuenta_en_la_que_tiene_que_ir);

                        if ( _equipo != null && _cuenta_en_la_que_va!= null)
                        {
                            aInsertar.Add(new RegistroPorInsertar()
                            {
                                Nombre_Equipo = equipo,
                                Nombre_item = cuenta_en_la_que_tiene_que_ir,
                                Dia = fecha,
                                Monto = monto,
                                Comentario = observ
                            });
                        }
                        else
                        {
                            sinMatchear.Add(new RegistroSinMachear()
                            {
                                Nombre_Equipo_informado = equipo,
                                Nombre_item_informado = cuenta_en_la_que_tiene_que_ir,
                                Dia = fecha,
                                Monto = monto,
                                Comentario = observ,
                                Motivo = _equipo == null && _cuenta_en_la_que_va == null ? "No se encontro el equipo \"" + equipo + "\" ni la cuenta \"" + cuenta_en_la_que_tiene_que_ir + "\"" : (_equipo == null ? "No se encontro el equipo \"" + equipo + "\"": "No se encontro la cuenta \"" + cuenta_en_la_que_tiene_que_ir + "\"")
                            });
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

            GridView2.DataSource = aInsertar;
            GridView2.DataBind();
            GridView3.DataSource = sinMatchear;
            GridView3.DataBind();

            div_resultados.Visible = true;
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

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