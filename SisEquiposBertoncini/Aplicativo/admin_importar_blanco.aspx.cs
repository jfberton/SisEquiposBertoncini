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

namespace SisEquiposBertoncini.Aplicativo
{
    public partial class admin_importar_blanco : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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
                if (dr[0].ToString().Contains("F") && (
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

                    if (nombreEquipo != "Sin clasificar" && nombreItem != "Sin clasificar")
                    {
                        //correcto
                        RegistroPorInsertar ri = new RegistroPorInsertar();
                        ri.Nombre_Equipo = nombreEquipo;
                        ri.Nombre_item = nombreItem;
                        ri.Comentario = dr[18].ToString();
                        ri.Dia = Convert.ToDateTime(dr[34].ToString());
                        ri.Monto = Convert.ToDecimal(dr[24].ToString()) > 0 ? Convert.ToDecimal(dr[24].ToString()) : Convert.ToDecimal(dr[3].ToString());
                        aInsertar.Add(ri);


                        //if (nombreItem == "Combustible")
                        //{
                        //    string chofer= (dr[18].ToString().Split(';'))[0];
                        //    bool tanque_lleno = (dr[18].ToString().Split(';'))[1]== "S";
                        //    decimal km = 0;
                        //    decimal litros = 0;
                        //    decimal.TryParse((dr[18].ToString().Split(';'))[2].Replace('.', ','), out km);
                        //    decimal.TryParse((dr[18].ToString().Split(';'))[3].Replace('.', ','), out litros);
                        //    Equipo eq = cxt.Equipos.FirstOrDefault(ee=>ee.nombre == nombreEquipo);


                        //    items_combustible.Add(new Planilla_combustible()
                        //    {
                        //        id_equipo = eq != null ? eq.id_equipo : 0,
                        //        fecha = Convert.ToDateTime(dr[7].ToString()),
                        //        chofer = chofer,
                        //        tanque_lleno = tanque_lleno,
                        //        km = km,
                        //        litros = litros,
                        //        costo_total_facturado = Convert.ToDecimal(dr[24].ToString()) + Convert.ToDecimal(dr[36].ToString()),
                        //        promedio = 0
                        //    });
                        //}

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
                            sinMatchear.Add(rsm);
                        }
                    }
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
                var equipos = items.GroupBy(test => test.Nombre_Equipo).Select(grp => grp.First()).ToList();

                foreach (var equipo in equipos)
                {
                    var registros = (from x in items where x.Nombre_Equipo == equipo.Nombre_Equipo select x).ToList();
                    Equipo eq = cxt.Equipos.First(ee => ee.nombre == equipo.Nombre_Equipo);

                    foreach (RegistroPorInsertar item in registros)
                    {
                        eq.Agregar_detalle(item.Dia, item.Monto, item.Nombre_item, item.Comentario);
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
                            promedio = item.promedio
                        });
                    }
                }

                cxt.SaveChanges();
            }

            MessageBox.Show(this, "Registros importados correctamente", MessageBox.Tipo_MessageBox.Success, "Perfecto!");
        }
    }
}

