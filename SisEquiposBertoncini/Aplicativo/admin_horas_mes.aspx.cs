﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SisEquiposBertoncini.Aplicativo.Datos;
using SisEquiposBertoncini.Aplicativo.Controles;
using System.Drawing;

namespace SisEquiposBertoncini.Aplicativo
{
    public partial class admin_horas_mes : System.Web.UI.Page
    {
        struct fila_valor_dia_mes
        {
            public string id_dia { get; set; }
            public DateTime dia { get; set; }
            public Estado_turno_dia estado_tm { get; set; }
            public Estado_turno_dia estado_tt { get; set; }
            public string horas_normales { get; set; }
            public string horas_extra_cincuenta { get; set; }
            public string horas_extra_cien { get; set; }
            public string horas_totales_dia { get; set; }
        }

        struct fila_detalle_dia
        {
            public int id_dia { get; set; }
            public int id_detalle_dia { get; set; }
            public DateTime fecha { get; set; }
            public int id_equipo { get; set; }
            public string equipo { get; set; }
            public string desde { get; set; }
            public string hasta { get; set; }
            public string total_movimiento { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Usuario usuariologueado = Session["UsuarioLogueado"] as Usuario;
                if (usuariologueado == null)
                {
                    Response.Redirect("~/Default.aspx?mode=session_end");
                }

                Cargar_ddls();

                var valores = Session["valores_horas"];
                if (valores != null)
                {
                    btn_buscar.Visible = false;
                    btn_nueva_busqueda.Visible = true;
                    div_valores_mes.Visible = true;
                }
                else
                {
                    btn_buscar.Visible = true;
                    btn_nueva_busqueda.Visible = false;
                    div_valores_mes.Visible = false;
                }
            }

        }

        private void Cargar_ddls()
        {
            using (var cxt = new Model1Container())
            {
                var tipos_empleados = (from t in cxt.Categorias_empleados
                                       select new
                                       {
                                           value = t.id_categoria,
                                           text = t.nombre
                                       }).ToList();

                ddl_tipo_empleado.DataTextField = "text";
                ddl_tipo_empleado.DataValueField = "value";
                ddl_tipo_empleado.DataSource = tipos_empleados;
                ddl_tipo_empleado.DataBind();

                int id_tipo_empleado = Convert.ToInt32(ddl_tipo_empleado.SelectedValue);

                var empleados = (from e in cxt.Empleados
                                 where e.id_categoria == id_tipo_empleado
                                 select new
                                 {
                                     value = e.id_empleado,
                                     text = e.nombre
                                 }).ToList();

                ddl_empleado.DataTextField = "text";
                ddl_empleado.DataValueField = "value";
                ddl_empleado.DataSource = empleados;
                ddl_empleado.DataBind();

                for (int i = 0; i < 12; i++)
                {
                    ddl_mes.Items.Add(
                        new ListItem()
                        {
                            Value = (i + 1).ToString(),
                            Text = new DateTime(2015, i + 1, 1).ToString("MMMM")
                        });
                }

                for (int i = 2015; i <= DateTime.Today.Year; i++)
                {
                    ddl_anio.Items.Add(
                        new ListItem()
                        {
                            Value = (i).ToString(),
                            Text = (i).ToString()
                        });
                }

                ddl_estado_turno_m.DataSource = Enum.GetNames(typeof(Estado_turno_dia));
                ddl_estado_turno_m.DataBind();

                ddl_estado_turno_t.DataSource = Enum.GetNames(typeof(Estado_turno_dia));
                ddl_estado_turno_t.DataBind();

                var equipos = (from ee in cxt.Equipos
                               where ee.fecha_baja == null
                               select new
                               {
                                   value = ee.id_equipo,
                                   text = ee.nombre
                               }).ToList();

                ddl_equipo.DataTextField = "text";
                ddl_equipo.DataValueField = "value";
                ddl_equipo.DataSource = equipos;
                ddl_equipo.DataBind();
            }
        }

        protected void gv_valores_mes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.ControlStyle.BackColor = Color.LightGray;
            }
            else
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.Cells[1].Text == "Ninguno")
                    {
                        e.Row.Cells[1].BackColor = System.Drawing.Color.LightGray;
                        e.Row.Cells[1].ForeColor = System.Drawing.Color.DarkGray;
                    }

                    if (e.Row.Cells[2].Text == "Ninguno")
                    {
                        e.Row.Cells[2].BackColor = System.Drawing.Color.LightGray;
                        e.Row.Cells[2].ForeColor = System.Drawing.Color.DarkGray;
                    }

                    if (e.Row.Cells[1].Text == "Vacaciones")
                    {
                        e.Row.Cells[1].BackColor = System.Drawing.Color.LightYellow;
                        e.Row.Cells[1].ForeColor = System.Drawing.Color.DarkGoldenrod;
                    }

                    if (e.Row.Cells[2].Text == "Vacaciones")
                    {
                        e.Row.Cells[2].BackColor = System.Drawing.Color.LightYellow;
                        e.Row.Cells[2].ForeColor = System.Drawing.Color.DarkGoldenrod;
                    }

                    if (e.Row.Cells[1].Text == "Presente")
                    {
                        e.Row.Cells[1].BackColor = System.Drawing.Color.LightGreen;
                        e.Row.Cells[1].ForeColor = System.Drawing.Color.DarkGreen;
                    }

                    if (e.Row.Cells[2].Text == "Presente")
                    {
                        e.Row.Cells[2].BackColor = System.Drawing.Color.LightGreen;
                        e.Row.Cells[2].ForeColor = System.Drawing.Color.DarkGreen;
                    }

                    if (e.Row.Cells[1].Text == "Ausente" || e.Row.Cells[1].Text == "Ausente_injust")
                    {
                        e.Row.Cells[1].BackColor = System.Drawing.Color.LightPink;
                        e.Row.Cells[1].ForeColor = System.Drawing.Color.DarkRed;
                    }

                    if (e.Row.Cells[2].Text == "Ausente" || e.Row.Cells[2].Text == "Ausente_injust")
                    {
                        e.Row.Cells[2].BackColor = System.Drawing.Color.LightPink;
                        e.Row.Cells[2].ForeColor = System.Drawing.Color.DarkRed;
                    }
                }
            }
        }

        protected void btn_buscar_Click(object sender, EventArgs e)
        {
            CargarValoresMes();
        }

        private void CargarValoresMes()
        {
            int id_empleado = Convert.ToInt32(ddl_empleado.SelectedValue);
            int mes = Convert.ToInt32(ddl_mes.SelectedValue);
            int anio = Convert.ToInt32(ddl_anio.SelectedValue);
            int dias_mes = DateTime.DaysInMonth(anio, mes);
            btn_nueva_busqueda.Visible = true;
            btn_buscar.Visible = false;
            div_valores_mes.Visible = true;

            List<fila_valor_dia_mes> valores_mes = new List<fila_valor_dia_mes>();

            decimal dias_laborables = 0;
            decimal dias_ausentes= 0;
            decimal dias_presentes = 0;
            decimal dias_por_cargar = 0;
            decimal dias_out = 0;
            decimal dias_presentes_en_dias_no_laborables = 0;
            string total_horas_normales = "00:00";
            string total_horas_extra_50 = "00:00";
            string total_horas_extra_100 = "00:00";

            using (var cxt = new Model1Container())
            {
                for (int i = 1; i < dias_mes; i++)
                {
                    DateTime fecha_buscada = new DateTime(anio, mes, i);
                    fila_valor_dia_mes ret = new fila_valor_dia_mes();
                    Dia dia = cxt.Dias.FirstOrDefault(d => d.fecha == fecha_buscada && d.id_empleado == id_empleado);
                    if (dia != null)
                    {
                        ret.id_dia = "id_" + dia.id_dia.ToString();
                        ret.dia = dia.fecha;
                        ret.estado_tm = dia.estado_tm;
                        ret.estado_tt = dia.estado_tt;
                        ret.horas_normales = dia.horas_normales;
                        ret.horas_extra_cincuenta = dia.horas_extra_50;
                        ret.horas_extra_cien = dia.horas_extra_100;
                        ret.horas_totales_dia = Horas_string.SumarHoras(new string[] { dia.horas_normales, dia.horas_extra_50, dia.horas_extra_100 });

                        dias_ausentes = (ret.estado_tm == Estado_turno_dia.Ausente || ret.estado_tm == Estado_turno_dia.Ausente_injust) ? dias_ausentes + (Convert.ToDecimal(5) / Convert.ToDecimal(10)) : dias_ausentes;
                        dias_ausentes = (ret.estado_tt == Estado_turno_dia.Ausente || ret.estado_tt == Estado_turno_dia.Ausente_injust) ? dias_ausentes + (Convert.ToDecimal(5) / Convert.ToDecimal(10)) : dias_ausentes;

                        dias_presentes = (ret.estado_tm == Estado_turno_dia.Presente || ret.estado_tm == Estado_turno_dia.Vacaciones) ? dias_presentes + (Convert.ToDecimal(5) / Convert.ToDecimal(10)) : dias_presentes;
                        dias_presentes = (ret.estado_tt == Estado_turno_dia.Presente || ret.estado_tt == Estado_turno_dia.Vacaciones) ? dias_presentes + (Convert.ToDecimal(5) / Convert.ToDecimal(10)) : dias_presentes;

                        total_horas_normales = Horas_string.SumarHoras(new string[] { total_horas_normales, dia.horas_normales });
                        total_horas_extra_50 = Horas_string.SumarHoras(new string[] { total_horas_extra_50, dia.horas_extra_50 });
                        total_horas_extra_100 = Horas_string.SumarHoras(new string[] { total_horas_extra_100, dia.horas_extra_100 });

                        dias_presentes_en_dias_no_laborables = (dia.fecha.DayOfWeek == DayOfWeek.Saturday && ret.estado_tt == Estado_turno_dia.Presente) ? dias_presentes_en_dias_no_laborables + (Convert.ToDecimal(5) / Convert.ToDecimal(10)) : dias_presentes_en_dias_no_laborables;

                        dias_presentes_en_dias_no_laborables = (dia.fecha.DayOfWeek == DayOfWeek.Sunday && ret.estado_tm == Estado_turno_dia.Presente) ? dias_presentes_en_dias_no_laborables + (Convert.ToDecimal(5) / Convert.ToDecimal(10)) : dias_presentes_en_dias_no_laborables;
                        dias_presentes_en_dias_no_laborables = (dia.fecha.DayOfWeek == DayOfWeek.Sunday && ret.estado_tt == Estado_turno_dia.Presente) ? dias_presentes_en_dias_no_laborables + (Convert.ToDecimal(5) / Convert.ToDecimal(10)) : dias_presentes_en_dias_no_laborables;

                        foreach (Detalle_dia detalle in dia.Detalles)
                        {
                            if(detalle.Equipo.OUT)
                            {
                                dias_out = dias_out + Horas_string.HorasADecimales(Horas_string.RestarHoras(detalle.hora_hasta, detalle.hora_desde));
                            }
                        }
                    }
                    else
                    {
                        ret.id_dia = "fecha_" + anio.ToString() + "_" + mes.ToString() + "_" + i;
                        ret.dia = fecha_buscada;
                        ret.estado_tm = Estado_turno_dia.Ninguno;
                        ret.estado_tt = Estado_turno_dia.Ninguno;
                        ret.horas_normales = "00:00";
                        ret.horas_extra_cincuenta = "00:00";
                        ret.horas_extra_cien = "00:00";
                        ret.horas_totales_dia = "00:00";

                        switch (ret.dia.DayOfWeek)
                        {
                            case DayOfWeek.Saturday:
                                dias_por_cargar += (Convert.ToDecimal(5) / Convert.ToDecimal(10));
                                break;
                            case DayOfWeek.Sunday:
                                break;
                            default:
                                dias_por_cargar++;
                                break;
                        }

                    }
                    valores_mes.Add(ret);

                    switch (ret.dia.DayOfWeek)
                    {
                        case DayOfWeek.Saturday:
                            dias_laborables += (Convert.ToDecimal(5) / Convert.ToDecimal(10));
                            break;
                        case DayOfWeek.Sunday:
                            break;
                        default:
                            dias_laborables++;
                            break;
                    }
                }
            }

            gv_valores_mes.DataSource = valores_mes;
            gv_valores_mes.DataBind();


            lbl_dias_ausentes.Text = dias_ausentes.ToString();
            lbl_dias_laborables.Text = dias_laborables.ToString();
            lbl_dias_out.Text = dias_out.ToString();
            lbl_días_por_cargar.Text = dias_por_cargar.ToString();
            lbl_dias_presentes_en_dias_no_laborables.Text = dias_presentes_en_dias_no_laborables.ToString();
            lbl_dias_presentes_vacaciones.Text = dias_presentes.ToString();
            lbl_total_horas_normales.Text = total_horas_normales;
            lbl_total_horas_extra_50.Text = total_horas_extra_50;
            lbl_total_horas_extra_100.Text = total_horas_extra_100;

            Session["valores_horas"] = valores_mes;
        }

        protected void btn_nueva_busqueda_Click(object sender, EventArgs e)
        {
            Session["valores_horas"] = null;
            btn_nueva_busqueda.Visible = false;
            btn_buscar.Visible = true;
            div_valores_mes.Visible = false;
        }

        protected void btn_ver_ServerClick(object sender, EventArgs e)
        {
            string str_id_dia = ((System.Web.UI.HtmlControls.HtmlButton)sender).Attributes["data-id"];
            Dia dia;
            List<fila_detalle_dia> detalle = new List<fila_detalle_dia>();
            div_error_detalle.Visible = false;
            using (var cxt = new Model1Container())
            {
                if (str_id_dia.Contains("id"))
                {
                    int id_dia = Convert.ToInt32(str_id_dia.Replace("id_", ""));
                    dia = cxt.Dias.FirstOrDefault(d => d.id_dia == id_dia);
                    foreach (Detalle_dia item_detalle in dia.Detalles)
                    {
                        fila_detalle_dia fila_detalle = new fila_detalle_dia();
                        fila_detalle.id_equipo = item_detalle.Equipo.id_equipo;
                        fila_detalle.fecha = item_detalle.Dia.fecha;
                        fila_detalle.id_dia = item_detalle.id_dia;
                        fila_detalle.id_detalle_dia = item_detalle.id_detalle_dia;
                        fila_detalle.equipo = item_detalle.Equipo.nombre;
                        fila_detalle.desde = item_detalle.hora_desde;
                        fila_detalle.hasta = item_detalle.hora_hasta;
                        fila_detalle.total_movimiento = Horas_string.RestarHoras(item_detalle.hora_hasta, item_detalle.hora_desde);

                        detalle.Add(fila_detalle);
                    }
                }
                else
                {
                    int fecha_dia = Convert.ToInt32(str_id_dia.Replace("fecha_", "").Split('_')[2]);
                    int fecha_mes = Convert.ToInt32(str_id_dia.Replace("fecha_", "").Split('_')[1]);
                    int fecha_año = Convert.ToInt32(str_id_dia.Replace("fecha_", "").Split('_')[0]);

                    dia = new Dia();
                    dia.fecha = new DateTime(fecha_año, fecha_mes, fecha_dia);
                    dia.horas_normales = "00:00";
                    dia.horas_extra_50 = "00:00";
                    dia.horas_extra_100 = "00:00";
                    dia.estado_tm = Estado_turno_dia.Ninguno;
                    dia.estado_tt = Estado_turno_dia.Ninguno;
                    dia.id_empleado = Convert.ToInt32(ddl_empleado.SelectedValue);

                    cxt.Dias.Add(dia);
                }
            }

            gv_detalle_dia.DataSource = detalle;
            gv_detalle_dia.DataBind();
            
            lbl_fecha.Text = dia.fecha.ToString("dddd', ' dd 'de' MMMM 'del' yyyy");
            ddl_estado_turno_m.SelectedValue = dia.estado_tm.ToString();
            ddl_estado_turno_t.SelectedValue = dia.estado_tt.ToString();

            lbl_horas_normales.Text = dia.horas_normales;
            lbl_horas_extra_cincuenta.Text = dia.horas_extra_50;
            lbl_horas_extra_cien.Text = dia.horas_extra_100;

            Session["DiaSeleccionado"] = dia;
            Session["Detalle"] = detalle;
            
            MostrarPopUpValoresDia();
        }

        protected void ddl_tipo_empleado_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var cxt = new Model1Container())
            {
                int id_tipo_empleado = Convert.ToInt32(ddl_tipo_empleado.SelectedValue);

                var empleados = (from ee in cxt.Empleados
                                 where ee.id_categoria == id_tipo_empleado
                                 select new
                                 {
                                     value = ee.id_empleado,
                                     text = ee.nombre
                                 }).ToList();

                ddl_empleado.DataTextField = "text";
                ddl_empleado.DataValueField = "value";
                ddl_empleado.DataSource = empleados;
                ddl_empleado.DataBind();
            }

        }

        private void MostrarPopUpValoresDia()
        {
            string script = "<script language=\"javascript\"  type=\"text/javascript\">$(document).ready(function() { $('#div_valores_dia').modal('show')});</script>";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "ShowPopUp", script, false);
        }

        protected void btn_agregar_detalle_dia_Click(object sender, EventArgs e)
        {
            string errores = string.Empty;
            bool tiene_error = false;

            errores = ddl_equipo.SelectedItem.Text != string.Empty ? errores : "• Debe seleccionar un equipo.";
            tiene_error = ddl_equipo.SelectedItem.Text != string.Empty ? false : true;

            if (!tiene_error)
            {
                tiene_error = tb_hora_desde.Value != string.Empty ? false : true;
                errores = !tiene_error ? errores : "• Debe agregar hora desde.<br/>";
            }

            if (!tiene_error)
            {
                tiene_error = tb_hora_hasta.Value != string.Empty ? false : true;
                errores = !tiene_error ? errores : "• Debe agregar hora hasta.<br/>";
            }

            if (!tiene_error)
            {
                tiene_error = Horas_string.AMayorQueB(tb_hora_hasta.Value, tb_hora_desde.Value) ? false : true;
                errores = !tiene_error ? errores : "• La hora desde debe ser menor a la hora hasta.<br/>";
            }

            if (!tiene_error)
            {
                div_error_detalle.Visible = false;
                Dia dia = Session["DiaSeleccionado"] as Dia;
                List<fila_detalle_dia> filas = Session["Detalle"] as List<fila_detalle_dia>;
                fila_detalle_dia detalle = new fila_detalle_dia();
                detalle.id_equipo = Convert.ToInt32(ddl_equipo.SelectedItem.Value);
                detalle.fecha = dia.fecha;
                detalle.equipo = ddl_equipo.SelectedItem.Text;
                detalle.desde = tb_hora_desde.Value;
                detalle.hasta = tb_hora_hasta.Value;
                detalle.total_movimiento = Horas_string.RestarHoras(tb_hora_hasta.Value, tb_hora_desde.Value);
                filas.Add(detalle);

                string horasTotales = "00:00";

                foreach (fila_detalle_dia item_detalle in filas)
                {
                    horasTotales = Horas_string.SumarHoras(new string[] { horasTotales, Horas_string.RestarHoras(item_detalle.hasta, item_detalle.desde) });
                }

                dia.horas_normales = ObtenerHoras(horasTotales, dia.fecha, TipoHora.Normales);
                dia.horas_extra_50= ObtenerHoras(horasTotales, dia.fecha, TipoHora.Extra50);
                dia.horas_extra_100 = ObtenerHoras(horasTotales, dia.fecha, TipoHora.Extra100);

                gv_detalle_dia.DataSource = filas;
                gv_detalle_dia.DataBind();

                lbl_horas_normales.Text = dia.horas_normales;
                lbl_horas_extra_cincuenta.Text = dia.horas_extra_50;
                lbl_horas_extra_cien.Text = dia.horas_extra_100;

                Session["DiaSeleccionado"] = dia;
                Session["Detalle"] = filas;

            }
            else
            {
                errores = "<label>Corrija los siguientes errores antes de continuar:</label><br/>" + errores;
                div_error_detalle.Visible = true;
                p_error.InnerHtml = errores;
            }

            MostrarPopUpValoresDia();
        }

        protected void btn_guardar_detalle_dia_ServerClick(object sender, EventArgs e)
        {
            //guardar
            Dia dia = Session["DiaSeleccionado"] as Dia;
            List<fila_detalle_dia> filas = Session["Detalle"] as List<fila_detalle_dia>;

            string errores = string.Empty;
            bool tiene_error = false;


            Estado_turno_dia estadotm = (Estado_turno_dia)Enum.Parse(typeof(Estado_turno_dia), ddl_estado_turno_m.SelectedValue);
            Estado_turno_dia estadott = (Estado_turno_dia)Enum.Parse(typeof(Estado_turno_dia), ddl_estado_turno_t.SelectedValue);

            if (dia.fecha.DayOfWeek != DayOfWeek.Sunday)
            {
                tiene_error = estadotm != Estado_turno_dia.Ninguno ? false : true;
                errores = !tiene_error ? string.Empty : errores + "• Debe agendar estado para el turno mañana <br/>";
            }
            if (!tiene_error)
            {
                if (dia.fecha.DayOfWeek != DayOfWeek.Saturday && dia.fecha.DayOfWeek != DayOfWeek.Sunday)
                {
                    tiene_error = estadott != Estado_turno_dia.Ninguno ? false : true;
                    errores = !tiene_error ? string.Empty : errores + "• Debe agendar estado para el turno tarde <br/>";
                }
            }

            if (errores == string.Empty)
            {
                using (var cxt = new Model1Container())
                {
                    if (dia.id_dia == 0)
                    {
                        Dia dia_cxt = new Dia();
                        dia_cxt.id_empleado = Convert.ToInt32(ddl_empleado.SelectedItem.Value);
                        if (dia.fecha.DayOfWeek == DayOfWeek.Sunday)
                        {
                            if (estadotm != Estado_turno_dia.Ausente && estadotm != Estado_turno_dia.Ausente_injust)
                            {
                                dia_cxt.estado_tm = estadotm;
                            }
                            else
                            {
                                dia_cxt.estado_tm = Estado_turno_dia.Ninguno;
                            }
                        }
                        else
                        {
                            dia_cxt.estado_tm = estadotm;
                        }

                        if (dia.fecha.DayOfWeek == DayOfWeek.Sunday || dia.fecha.DayOfWeek == DayOfWeek.Saturday)
                        {
                            if (estadott != Estado_turno_dia.Ausente && estadott != Estado_turno_dia.Ausente_injust)
                            {
                                dia_cxt.estado_tt = estadott;
                            }
                            else
                            {
                                dia_cxt.estado_tt = Estado_turno_dia.Ninguno;
                            }
                        }
                        else
                        {
                            dia_cxt.estado_tt = estadott;
                        }

                        dia_cxt.fecha = dia.fecha;
                        dia_cxt.horas_normales = dia.horas_normales;
                        dia_cxt.horas_extra_50 = dia.horas_extra_50;
                        dia_cxt.horas_extra_100 = dia.horas_extra_100;

                        foreach (fila_detalle_dia item_detalle in filas)
                        {
                            if (item_detalle.id_detalle_dia == 0)
                            {
                                dia_cxt.Detalles.Add(new Detalle_dia() { id_equipo = item_detalle.id_equipo, hora_desde = item_detalle.desde, hora_hasta = item_detalle.hasta });
                            }
                        }

                        try
                        {
                            cxt.Dias.Add(dia_cxt);
                            cxt.SaveChanges();
                            Session["DiaSeleccionado"] = null;
                            Session["Detalle"] = null;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, "CHANN!!! LPM<br/>Error: " + ex.Message + "<br/>Inner: " + ex.InnerException, MessageBox.Tipo_MessageBox.Danger);
                        }

                    }
                    else
                    { 
                        //ya existe el dia, hay que agregar los cambios
                        Dia dia_cxt = cxt.Dias.First(d => d.id_dia == dia.id_dia);
                        if (dia.fecha.DayOfWeek == DayOfWeek.Sunday)
                        {
                            if (estadotm != Estado_turno_dia.Ausente && estadotm != Estado_turno_dia.Ausente_injust)
                            {
                                dia_cxt.estado_tm = estadotm;
                            }
                            else
                            {
                                dia_cxt.estado_tm = Estado_turno_dia.Ninguno;
                            }
                        }
                        else
                        {
                            dia_cxt.estado_tm = estadotm;
                        }

                        if (dia.fecha.DayOfWeek == DayOfWeek.Sunday || dia.fecha.DayOfWeek == DayOfWeek.Saturday)
                        {
                            if (estadott != Estado_turno_dia.Ausente && estadott != Estado_turno_dia.Ausente_injust)
                            {
                                dia_cxt.estado_tt = estadott;
                            }
                            else
                            {
                                dia_cxt.estado_tt = Estado_turno_dia.Ninguno;
                            }
                        }
                        else
                        {
                            dia_cxt.estado_tt = estadott;
                        }


                        dia_cxt.horas_normales = dia.horas_normales;
                        dia_cxt.horas_extra_50 = dia.horas_extra_50;
                        dia_cxt.horas_extra_100 = dia.horas_extra_100;

                        foreach (fila_detalle_dia item_detalle in filas)
                        {
                            if (item_detalle.id_detalle_dia == 0)
                            {
                                dia_cxt.Detalles.Add(new Detalle_dia() { id_equipo = item_detalle.id_equipo, hora_desde = item_detalle.desde, hora_hasta = item_detalle.hasta });
                            }
                        }

                        try
                        {
                            cxt.SaveChanges();
                            Session["DiaSeleccionado"] = null;
                            Session["Detalle"] = null;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, "CHANN!!! LPM<br/>Error: " + ex.Message + "<br/>Inner: " + ex.InnerException, MessageBox.Tipo_MessageBox.Danger);
                        }
                    }
                }
                CargarValoresMes();
            }
            else
            {
                errores = "<label>Corrija los siguientes errores antes de continuar:</label><br/>" + errores;
                div_error_detalle.Visible = true;
                p_error.InnerHtml = errores;
                MostrarPopUpValoresDia();
            }
        }

        protected void btn_eliminar_detalle_ServerClick(object sender, EventArgs e)
        {
            string str_id_detalle_dia = hidden_id_detalle_dia.Value;
            string str_id_equipo = hidden_id_equipo.Value;
            string str_desde = hidden_desde.Value;
            string str_hasta = hidden_hasta.Value;

            Dia dia = Session["DiaSeleccionado"] as Dia;
            List<fila_detalle_dia> filas = Session["Detalle"] as List<fila_detalle_dia>;

            if (str_id_detalle_dia != "0")
            {
                int id_detalle_dia = Convert.ToInt32(str_id_detalle_dia);
                using (var cxt = new Model1Container())
                {
                    Detalle_dia detalle = cxt.Detalles_dias.First(dd => dd.id_detalle_dia == id_detalle_dia);
                    Dia dia_cxt = detalle.Dia;
                    cxt.Detalles_dias.Remove(detalle);

                    int index = -1;
                    
                    foreach (fila_detalle_dia item in filas)
                    {
                        if (item.id_detalle_dia == detalle.id_detalle_dia)
                        {
                            index = filas.IndexOf(item);
                            break;
                        }
                    }

                    filas.RemoveAt(index);

                    string horasTotales = "00:00";
                    foreach (fila_detalle_dia item_detalle in filas)
                    {
                        horasTotales = Horas_string.SumarHoras(new string[] { horasTotales, Horas_string.RestarHoras(item_detalle.hasta, item_detalle.desde) });
                    }

                    dia_cxt.horas_normales = ObtenerHoras(horasTotales, dia_cxt.fecha, TipoHora.Normales);
                    dia_cxt.horas_extra_50 = ObtenerHoras(horasTotales, dia_cxt.fecha, TipoHora.Extra50);
                    dia_cxt.horas_extra_100 = ObtenerHoras(horasTotales, dia_cxt.fecha, TipoHora.Extra100);

                    cxt.SaveChanges();
                    Session["DiaSeleccionado"] = dia_cxt;
                    lbl_horas_normales.Text = dia_cxt.horas_normales;
                    lbl_horas_extra_cincuenta.Text = dia_cxt.horas_extra_50;
                    lbl_horas_extra_cien.Text = dia_cxt.horas_extra_100;
                }
            }
            else
            {
                int index = -1;
                foreach (fila_detalle_dia item in filas)
                {
                    if (item.id_equipo.ToString() == str_id_equipo && item.desde == str_desde && item.hasta == str_hasta && item.id_detalle_dia == 0)
                    {
                        index = filas.IndexOf(item);
                        break;
                    }
                }

                filas.RemoveAt(index);

                string horasTotales = "00:00";
                foreach (fila_detalle_dia item_detalle in filas)
                {
                    horasTotales = Horas_string.SumarHoras(new string[] { horasTotales, Horas_string.RestarHoras(item_detalle.hasta, item_detalle.desde) });
                }

                dia.horas_normales = ObtenerHoras(horasTotales, dia.fecha, TipoHora.Normales);
                dia.horas_extra_50 = ObtenerHoras(horasTotales, dia.fecha, TipoHora.Extra50);
                dia.horas_extra_100 = ObtenerHoras(horasTotales, dia.fecha, TipoHora.Extra100);
                Session["DiaSeleccionado"] = dia;
                lbl_horas_normales.Text = dia.horas_normales;
                lbl_horas_extra_cincuenta.Text = dia.horas_extra_50;
                lbl_horas_extra_cien.Text = dia.horas_extra_100;
            }

            

            gv_detalle_dia.DataSource = filas;
            gv_detalle_dia.DataBind();

            

            Session["Detalle"] = filas;

            CargarValoresMes();

            MostrarPopUpValoresDia();
        }

        private enum TipoHora
        {
            Normales,
            Extra50,
            Extra100
        }

        private string ObtenerHoras(string horas_totales, DateTime dia, TipoHora tipo_buscado)
        {
            string horas_normales = "00:00";
            string horas_extra_50 = "00:00";
            string horas_extra_100 = "00:00";

            switch (dia.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    if (Horas_string.AMayorQueB("04:00", horas_totales))
                    {
                        horas_normales = horas_totales;
                        horas_totales = "00:00";
                    }
                    else
                    {
                        horas_normales = "04:00";
                        horas_totales = Horas_string.RestarHoras(horas_totales, "04:00");
                    }

                    horas_extra_50 = horas_totales;
                    horas_extra_100 = "00:00";
                    break;
                case DayOfWeek.Sunday:
                    horas_normales = "00:00";
                    horas_extra_50 = "00:00";
                    horas_extra_100 = horas_totales;
                    break;
                default:
                    if (Horas_string.AMayorQueB("08:00", horas_totales))
                    {
                        horas_normales = horas_totales;
                        horas_totales = "00:00";
                    }
                    else
                    {
                        horas_normales = "08:00";
                        horas_totales = Horas_string.RestarHoras(horas_totales, "08:00");
                    }
                    horas_extra_50 = horas_totales;
                    horas_extra_100 = "00:00";
                    break;
            }

            switch (tipo_buscado)
            {
                case TipoHora.Normales:
                    return horas_normales;
                case TipoHora.Extra50:
                    return horas_extra_50;
                case TipoHora.Extra100:
                    return horas_extra_100;
                default:
                    return "00:00";
            }
        }
    }
}