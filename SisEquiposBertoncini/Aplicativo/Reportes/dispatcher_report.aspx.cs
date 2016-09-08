using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisEquiposBertoncini.Aplicativo.Reportes
{
    public partial class dispatcher_report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string reporte = Request.QueryString["reporte"];
                switch (reporte)
                {
                    case "planilla_calculos":
                        Crear_reporte_planilla_calculos();
                        break;
                    case "valores_anuales_categoria":
                        Crear_reporte_valores_anuales_categoria();
                        break;
                    case "valores_anuales_categoria_resumen":
                        Crear_reporte_valores_anuales_categoria_resumen();
                        break;
                    case "valores_anuales_equipo":
                        Crear_reporte_valores_anuales_equipo();
                        break;
                    case "valores_anuales_equipo_resumen":
                        Crear_reporte_valores_anuales_equipo_resumen();
                        break;
                    case "planilla_resumen_valores_equipo_categoria":
                        Cargar_planilla_resumen_valores_equipo_categoria();
                        break;
                    default:
                        break;
                }
            }
        }

        private void Cargar_planilla_resumen_valores_equipo_categoria()
        {
            Reportes.Valores_anio_equipo ds = Session["ds_equipo_categoria_anio"] as Reportes.Valores_anio_equipo;
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(Resumen_valores_equipo_categoria_subreport);

            viewer.LocalReport.ReportPath = Server.MapPath("~/Aplicativo/Reportes/Valores_anio_equipo_categoria.rdlc");

            ReportDataSource equipo = new ReportDataSource("Equipo", ds.Datos_equipo.Rows);
            ReportDataSource detalle = new ReportDataSource("Equipos", ds.Equipos.Rows);

            viewer.LocalReport.DataSources.Add(equipo);
            viewer.LocalReport.DataSources.Add(detalle);

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

            string script = "<script type='text/javascript'>document.location.href='Report.aspx';</script>";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);
        }

        public void Resumen_valores_equipo_categoria_subreport(object sender, SubreportProcessingEventArgs e)
        {
            Reportes.Valores_anio_equipo ds = Session["ds_equipo_categoria_anio"] as Reportes.Valores_anio_equipo;
            ReportDataSource detalle = new ReportDataSource("detalle_item", ds.Detalle_item.Rows);
            e.DataSources.Add(detalle);
        }

        private void Crear_reporte_valores_anuales_equipo()
        {
            Reportes.Valores_anio_equipo ds = Session["ds_equipo_anio"] as Reportes.Valores_anio_equipo;
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.EnableExternalImages = true;

            viewer.LocalReport.ReportPath = Server.MapPath("~/Aplicativo/Reportes/Valores_anio_equipo_r.rdlc");

            ReportDataSource equipo = new ReportDataSource("Equipo", ds.Datos_equipo.Rows);
            ReportDataSource detalle = new ReportDataSource("Detalle", ds.Detalle_item.Rows);

            viewer.LocalReport.DataSources.Add(equipo);
            viewer.LocalReport.DataSources.Add(detalle);

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

            string script = "<script type='text/javascript'>document.location.href='Report.aspx';</script>";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);
        }

        private void Crear_reporte_valores_anuales_equipo_resumen()
        {
            Reportes.Valores_anio_equipo dsresumen = new Reportes.Valores_anio_equipo();
            Reportes.Valores_anio_equipo ds = Session["ds_equipo_anio"] as Reportes.Valores_anio_equipo;

            foreach (Reportes.Valores_anio_equipo.Datos_equipoRow dr in ds.Datos_equipo)
            {
                Reportes.Valores_anio_equipo.Datos_equipoRow dir = dsresumen.Datos_equipo.NewDatos_equipoRow();

                for (int i = 0; i < ds.Tables["Datos_equipo"].Columns.Count; i++)
                {
                    dir[i] = dr[i];
                }

                dsresumen.Datos_equipo.Rows.Add(dir);
            }

            foreach (System.Data.DataRow dr in ds.Detalle_item.Rows)
            {
                if (dr["Nombre_item"].ToString() == "INGRESOS" ||
                    dr["Nombre_item"].ToString() == "EGRESOS" ||
                    dr["Nombre_item"].ToString() == "Resultado Financiero" ||
                    dr["Nombre_item"].ToString() == "Porcentaje de ganancias" ||
                    dr["Nombre_item"].ToString() == "Velocidad de recupero")
                {
                    Reportes.Valores_anio_equipo.Detalle_itemRow dir = dsresumen.Detalle_item.NewDetalle_itemRow();

                    for (int i = 0; i < ds.Tables["Detalle_item"].Columns.Count; i++)
                    {
                        dir[i] = dr[i];
                    }

                    dsresumen.Detalle_item.Rows.Add(dir);
                }
            }

            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.EnableExternalImages = true;

            viewer.LocalReport.ReportPath = Server.MapPath("~/Aplicativo/Reportes/Valores_anio_equipo_r.rdlc");

            ReportDataSource equipo = new ReportDataSource("Equipo", dsresumen.Datos_equipo.Rows);
            ReportDataSource detalle = new ReportDataSource("Detalle", dsresumen.Detalle_item.Rows);

            viewer.LocalReport.DataSources.Add(equipo);
            viewer.LocalReport.DataSources.Add(detalle);

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

            string script = "<script type='text/javascript'>document.location.href='Report.aspx';</script>";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);
        }
        
        private void Crear_reporte_valores_anuales_categoria()
        {
            Reportes.Valores_anio_equipo ds = Session["ds_equipo_anio"] as Reportes.Valores_anio_equipo;
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.EnableExternalImages = true;

            viewer.LocalReport.ReportPath = Server.MapPath("~/Aplicativo/Reportes/Valores_anio_equipo_r.rdlc");

            ReportDataSource equipo = new ReportDataSource("Equipo", ds.Datos_equipo.Rows);
            ReportDataSource detalle = new ReportDataSource("Detalle", ds.Detalle_item.Rows);

            viewer.LocalReport.DataSources.Add(equipo);
            viewer.LocalReport.DataSources.Add(detalle);

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

            string script = "<script type='text/javascript'>document.location.href='Report.aspx';</script>";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);
        }

        private void Crear_reporte_valores_anuales_categoria_resumen()
        {
            Reportes.Valores_anio_equipo dsresumen = new Reportes.Valores_anio_equipo();
            Reportes.Valores_anio_equipo ds = Session["ds_equipo_anio"] as Reportes.Valores_anio_equipo;

            foreach (Reportes.Valores_anio_equipo.Datos_equipoRow dr in ds.Datos_equipo)
            {
                Reportes.Valores_anio_equipo.Datos_equipoRow dir = dsresumen.Datos_equipo.NewDatos_equipoRow();

                for (int i = 0; i < ds.Tables["Datos_equipo"].Columns.Count; i++)
                {
                    dir[i] = dr[i];
                }

                dsresumen.Datos_equipo.Rows.Add(dir);
            }

            foreach (System.Data.DataRow dr in ds.Detalle_item.Rows)
            {
                if (dr["Nombre_item"].ToString() == "INGRESOS" ||
                    dr["Nombre_item"].ToString() == "EGRESOS" ||
                    dr["Nombre_item"].ToString() == "Resultado Financiero" ||
                    dr["Nombre_item"].ToString() == "Porcentaje de ganancias" ||
                    dr["Nombre_item"].ToString() == "Velocidad de recupero")
                {
                    Reportes.Valores_anio_equipo.Detalle_itemRow dir = dsresumen.Detalle_item.NewDetalle_itemRow();

                    for (int i = 0; i < ds.Tables["Detalle_item"].Columns.Count; i++)
                    {
                        dir[i] = dr[i];
                    }

                    dsresumen.Detalle_item.Rows.Add(dir);
                }
            }

            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.EnableExternalImages = true;

            viewer.LocalReport.ReportPath = Server.MapPath("~/Aplicativo/Reportes/Valores_anio_equipo_r.rdlc");

            ReportDataSource equipo = new ReportDataSource("Equipo", dsresumen.Datos_equipo.Rows);
            ReportDataSource detalle = new ReportDataSource("Detalle", dsresumen.Detalle_item.Rows);

            viewer.LocalReport.DataSources.Add(equipo);
            viewer.LocalReport.DataSources.Add(detalle);

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

            string script = "<script type='text/javascript'>document.location.href='Report.aspx';</script>";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);
        }

        private void Crear_reporte_planilla_calculos()
        {
            planilla_calculos ds = Session["ds_planilla_calculos"] as Reportes.planilla_calculos;
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

            string script = "<script type='text/javascript'>document.location.href='Report.aspx';</script>";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);
        }

    }
}