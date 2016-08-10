using SisEquiposBertoncini.Aplicativo.Controles;
using SisEquiposBertoncini.Aplicativo.Datos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisEquiposBertoncini.Aplicativo
{
    public partial class admin_horas_planilla_principal : System.Web.UI.Page
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

                Cargar_ddls();

                if (Session["planilla_principal_tipo_empleado"] != null)
                {
                    int mes = 0;
                    int anio = 0;
                    string tipo_empleado = Session["planilla_principal_tipo_empleado"].ToString();

                    int.TryParse(Session["planilla_principal_mes"].ToString(), out mes);
                    int.TryParse(Session["planilla_principal_anio"].ToString(), out anio);

                    ddl_anio.SelectedValue = anio.ToString();
                    ddl_mes.SelectedValue = mes.ToString();
                    ddl_tipo_empleado.SelectedValue = tipo_empleado;

                    Session["planilla_principal_mes"] = null;
                    Session["planilla_principal_anio"] = null;
                    Session["planilla_principal_tipo_empleado"] = null;

                    Cargar_busqueda(mes, anio, tipo_empleado);
                }
                else
                {
                    btn_nueva_busqueda.Visible = false;
                    div_resultados_busqueda.Visible = false;
                }
            }
        }

        private void Cargar_ddls()
        {
            for (int i = 2015; i <= DateTime.Today.Year + 1; i++)
            {
                ddl_anio.Items.Add(new ListItem() { Text = i.ToString(), Value = i.ToString() });
            }
        }

        protected void btn_buscar_Click(object sender, EventArgs e)
        {
            Cargar_busqueda();
        }

        private int ObtenerColumna(string p)
        {
            int id = 0;
            foreach (DataControlField item in gv_planilla_empleados.Columns)
            {
                if (item.HeaderText == p)
                {
                    id = gv_planilla_empleados.Columns.IndexOf(item);
                    break;
                }
            }

            return id;
        }

        struct itemTablaEmpleado
        {
            public int id_empleado { get; set; }
            public string empleado { get; set; }
            public string horas_normales { get; set; }
            public string horas_ausente { get; set; }
            public string horas_extra_50 { get; set; }
            public string horas_extra_100 { get; set; }
            public decimal sueldo { get; set; }
            public decimal dias_mes { get; set; }
            public decimal dias_out { get; set; }
        }

        private void Cargar_busqueda(int pmes = 0, int panio = 0, string ptipo_empleado = "")
        {
            btn_buscar.Visible = false;
            btn_nueva_busqueda.Visible = true;
            div_resultados_busqueda.Visible = true;
            Usuario usuariologueado = Session["UsuarioLogueado"] as Usuario;

            using (var cxt = new Model1Container())
            {
                int mes = pmes != 0 ? pmes : Convert.ToInt32(ddl_mes.SelectedItem.Value);
                int anio = panio != 0 ? panio : Convert.ToInt32(ddl_anio.SelectedItem.Value);
                int mesAnterior = new DateTime(anio, mes, 1).AddMonths(-1).Month;
                int anioAnterior = new DateTime(anio, mes, 1).AddMonths(-1).Year;

                DateTime primer_dia_mes = new DateTime(anio, mes, 1);
                DateTime ultimo_dia_mes = new DateTime(anio, mes, DateTime.DaysInMonth(anio, mes));
                
                Categoria_empleado mecanicos = cxt.Categorias_empleados.First(cc => cc.nombre == "Mecánico");
                Categoria_empleado soldadores = cxt.Categorias_empleados.First(cc => cc.nombre == "Soldador");
                Categoria_empleado pintores = cxt.Categorias_empleados.First(cc => cc.nombre == "Pintor");
                Categoria_empleado grueros = cxt.Categorias_empleados.First(cc => cc.nombre == "Gruero");


                List<Empleado> empleados = new List<Empleado>();

                string tipo_empleado = ptipo_empleado != "" ? ptipo_empleado : ddl_tipo_empleado.SelectedItem.Text;

                switch (tipo_empleado)
                {
                    case "Mecánicos - Pintores":
                        empleados = cxt.Empleados.Where(ee => (ee.id_categoria == mecanicos.id_categoria || ee.id_categoria == pintores.id_categoria) && (ee.fecha_alta == null || ee.fecha_alta.Value <= ultimo_dia_mes) && (ee.fecha_baja == null || (ee.fecha_baja.Value >= primer_dia_mes))).ToList();
                        break;
                    case "Soldadores":
                        empleados = cxt.Empleados.Where(ee => ee.id_categoria == soldadores.id_categoria && (ee.fecha_alta == null || ee.fecha_alta.Value <= ultimo_dia_mes) && (ee.fecha_baja == null || ee.fecha_baja.Value >= primer_dia_mes)).ToList();
                        break;
                    case "Grueros":
                        empleados = cxt.Empleados.Where(ee => ee.id_categoria == grueros.id_categoria && (ee.fecha_alta == null || ee.fecha_alta.Value <= ultimo_dia_mes) && (ee.fecha_baja == null || ee.fecha_baja.Value >= primer_dia_mes)).ToList();
                        break;
                    default:
                        break;
                }

                List<itemTablaEmpleado> filas = new List<itemTablaEmpleado>();

                foreach (Empleado e in empleados)
                {
                    Resumen_mes_empleado rme_mes_actual = cxt.Resumenes_meses_empleados.FirstOrDefault(rme => rme.id_empleado == e.id_empleado && rme.mes == mes && rme.anio == anio);
                    Resumen_mes_empleado rme_mes_anterior = cxt.Resumenes_meses_empleados.FirstOrDefault(rme => rme.id_empleado == e.id_empleado && rme.mes == mesAnterior && rme.anio == anioAnterior);

                    itemTablaEmpleado item = new itemTablaEmpleado();
                    item.id_empleado = e.id_empleado;
                    item.empleado = e.nombre;
                    item.horas_normales = rme_mes_actual!= null ? rme_mes_actual.total_horas_normales : "00:00";
                    item.horas_ausente = rme_mes_actual!= null ? rme_mes_actual.total_horas_ausente : "00:00";
                    item.horas_extra_50 = rme_mes_actual != null ? rme_mes_actual.total_horas_extra_50 : "00:00";
                    item.horas_extra_100 = rme_mes_actual != null ? rme_mes_actual.total_horas_extra_100 : "00:00";

                    item.sueldo = rme_mes_actual != null ? 
                        ((rme_mes_actual.Sueldo == 0 && rme_mes_anterior != null) ? rme_mes_anterior.Sueldo : rme_mes_actual.Sueldo) : 
                        rme_mes_anterior != null ? rme_mes_anterior.Sueldo : 0;
                    item.dias_mes = rme_mes_actual != null ? rme_mes_actual.dias_laborables : 0;
                    item.dias_out = rme_mes_actual != null ? rme_mes_actual.dias_out : 0;
                    
                    filas.Add(item);
                }

                var items_empleados = filas;

                var items_grilla = (from ie in items_empleados
                                    select new
                                    {
                                        empleado_id = ie.id_empleado,
                                        empleado = ie.empleado,
                                        horas_normales = Convert.ToDecimal(ie.horas_normales.Split(':')[0]) + (Convert.ToDecimal(ie.horas_normales.Split(':')[1]) / Convert.ToDecimal(60)),
                                        horas_ausente = Convert.ToDecimal(ie.horas_ausente.Split(':')[0]) + (Convert.ToDecimal(ie.horas_ausente.Split(':')[1]) / Convert.ToDecimal(60)),
                                        horas_extra_50 = Convert.ToDecimal(ie.horas_extra_50.Split(':')[0]) + (Convert.ToDecimal(ie.horas_extra_50.Split(':')[1]) / Convert.ToDecimal(60)),
                                        horas_extra_100 = Convert.ToDecimal(ie.horas_extra_100.Split(':')[0]) + (Convert.ToDecimal(ie.horas_extra_100.Split(':')[1]) / Convert.ToDecimal(60)),
                                        sueldo = ie.sueldo,
                                        dias_mes = ie.dias_mes,
                                        dias_out = (ddl_tipo_empleado.SelectedItem.Text == "Mecánicos - Pintores" ? ie.dias_out : Convert.ToDecimal(0)),
                                        costo_mensual_ponderado = ddl_tipo_empleado.SelectedItem.Text == "Mecánicos - Pintores" ? (ie.dias_mes > 0 ? ie.sueldo - (ie.sueldo / ie.dias_mes) * ie.dias_out : 0) : (ie.sueldo)
                                    }).ToList();

                if (ddl_tipo_empleado.SelectedItem.Text != "Mecánicos - Pintores")
                {
                    //gv_planilla_empleados.Columns[ObtenerColumna("Días mes")].Visible = false;
                    gv_planilla_empleados.Columns[ObtenerColumna("Días OUT")].Visible = false;
                    gv_planilla_empleados.Columns[ObtenerColumna("Costo mensual ponderado")].Visible = false;

                    div_costo_mensual_ponderado_total.Visible = false;
                    tabla_masa_salarial_ajustada_menos_dias_OUT.Visible = false;
                    lbl_texto_masa_salarial.Text = "- COSTO HORAS HOMBRE TEÓRICO";

                }

                decimal total_sueldos = 0;
                decimal costo_mensual_ponderado_total = 0;
                decimal costo_hora_teorico_ajustado = 0;
                decimal dias_mes = 1;
                decimal cantidad_empleados = Convert.ToDecimal(items_grilla.Count);
                decimal horas_dia = Convert.ToDecimal(8);
                decimal total_horas_normales = 0;
                decimal total_horas_ausente = 0;
                decimal total_horas_extra_50 = 0;
                decimal total_horas_extra_100 = 0;

                decimal prueba_ausente = 0;
                decimal prueba_50 = 0;
                decimal prueba_100 = 0;
                decimal prueba_dolar = 1;

                decimal.TryParse(txt_horas_ausentes_totales_prueba.Text.Replace('.', ','), out prueba_ausente);
                decimal.TryParse(txt_horas_extra_totales_50_prueba.Text.Replace('.', ','), out prueba_50);
                decimal.TryParse(txt_horas_extra_totales_100_prueba.Text.Replace('.', ','), out prueba_100);
                decimal.TryParse(txt_valor_dolar_mes_prueba.Text.Replace('.', ','), out prueba_dolar);
                if (prueba_dolar == 0)
                {
                    prueba_dolar = 1;
                }

                foreach (var item in items_grilla)
                {
                    total_sueldos = total_sueldos + item.sueldo;
                    costo_mensual_ponderado_total = costo_mensual_ponderado_total + item.costo_mensual_ponderado;
                    total_horas_normales = total_horas_normales + item.horas_normales;
                    total_horas_ausente = total_horas_ausente + item.horas_ausente;
                    total_horas_extra_50 = total_horas_extra_50 + item.horas_extra_50;
                    total_horas_extra_100 = total_horas_extra_100 + item.horas_extra_100;

                    if (dias_mes < item.dias_mes)
                    {
                        dias_mes = item.dias_mes;
                    }
                }

                costo_hora_teorico_ajustado = (costo_mensual_ponderado_total / dias_mes / cantidad_empleados / horas_dia);

                if (usuariologueado.perfil == perfil_usuario.Jefe)
                {
                    gv_planilla_empleados_view.DataSource = items_grilla;
                    gv_planilla_empleados_view.DataBind();
                    gv_planilla_empleados.Visible = false;
                }
                else
                {
                    gv_planilla_empleados.DataSource = items_grilla;
                    gv_planilla_empleados.DataBind();
                    gv_planilla_empleados_view.Visible = false;
                }

                lbl_costo_mensual_ponderado_total.Text = costo_mensual_ponderado_total.ToString("$ #,##0.00");
                lbl_costo_mensual_ponderado_total_1.Text = lbl_costo_mensual_ponderado_total.Text;
                lbl_costo_mensual_ponderado_total_2.Text = lbl_costo_mensual_ponderado_total.Text;

                lbl_sueldos_totales.Text = total_sueldos.ToString("$ #,##0.00");
                lbl_sueldos_totales_1.Text = lbl_sueldos_totales.Text;

                lbl_cantidad_de_empleados.Text = items_grilla.Count.ToString();
                lbl_cantidad_de_empleados_1.Text = lbl_cantidad_de_empleados.Text;

                lbl_cantidad_de_dias.Text = dias_mes.ToString();
                lbl_cantidad_de_dias_1.Text = lbl_cantidad_de_dias.Text;

                lbl_costo_horas_hombre_teorico.Text = (total_sueldos / dias_mes / cantidad_empleados / horas_dia).ToString("$ #,##0.00");
                lbl_costo_horas_hombre_teorico_ajustado.Text = costo_hora_teorico_ajustado.ToString("$ #,##0.00");

                //div_ausentes
                lbl_costo_ausentes.Text = (costo_hora_teorico_ajustado * total_horas_ausente).ToString("$ #,##0.00");
                lbl_costo_ausentes_prueba.Text = (costo_hora_teorico_ajustado * prueba_ausente).ToString("$ #,##0.00");
                lbl_horas_ausentes_totales.Text = total_horas_ausente.ToString("#,##0.00");
                lbl_ausentismo_porcentaje.Text = (total_horas_ausente / (dias_mes * cantidad_empleados * horas_dia)).ToString("##0.00 %");
                lbl_ausentismo_porcentaje_prueba.Text = (prueba_ausente / (dias_mes * cantidad_empleados * horas_dia)).ToString("##0.00 %");
                lbl_costo_horas_hombre_teorico_ajustado_1.Text = lbl_costo_horas_hombre_teorico_ajustado.Text;

                //div_he_50%
                lbl_costo_horas_hombre_teorico_ajustado_2.Text = lbl_costo_horas_hombre_teorico_ajustado.Text;
                lbl_horas_extra_totales_50.Text = total_horas_extra_50.ToString("#,##0.00");
                lbl_costo_horas_extra_50.Text = (costo_hora_teorico_ajustado * total_horas_extra_50).ToString("$ #,##0.00");
                lbl_costo_horas_extra_50_prueba.Text = (costo_hora_teorico_ajustado * prueba_50).ToString("$ #,##0.00");

                //div_he_100%
                lbl_costo_horas_hombre_teorico_ajustado_3.Text = lbl_costo_horas_hombre_teorico_ajustado.Text;
                lbl_horas_extra_totales_100.Text = total_horas_extra_100.ToString("#,##0.00");
                lbl_costo_horas_extra_100.Text = (costo_hora_teorico_ajustado * total_horas_extra_100).ToString("$ #,##0.00");
                lbl_costo_horas_extra_100_prueba.Text = (costo_hora_teorico_ajustado * prueba_100).ToString("$ #,##0.00");

                //div nueva masa salarial
                decimal nueva_masa_salarial = costo_mensual_ponderado_total +
                                                    (costo_hora_teorico_ajustado * total_horas_ausente) +
                                                    (costo_hora_teorico_ajustado * total_horas_extra_50) +
                                                    (costo_hora_teorico_ajustado * total_horas_extra_100);

                lbl_nueva_masa_salarial.Text = nueva_masa_salarial.ToString("$ #,##0.00");

                decimal nueva_masa_salarial_prueba = costo_mensual_ponderado_total +
                                                        (costo_hora_teorico_ajustado * prueba_ausente) +
                                                        (costo_hora_teorico_ajustado * prueba_50) +
                                                        (costo_hora_teorico_ajustado * prueba_100);

                lbl_nueva_masa_salarial_prueba.Text = nueva_masa_salarial_prueba.ToString("$ #,##0.00");




                //div resumen final planilla
                lbl_titulo_mes_fin_planilla.Text = ddl_mes.SelectedItem.Text + " - " + ddl_anio.SelectedItem.Text;

                lbl_nueva_masa_salarial_1.Text = lbl_nueva_masa_salarial.Text;
                lbl_nueva_masa_salarial_prueba_1.Text = lbl_nueva_masa_salarial_prueba.Text;

                decimal horas_realmente_trabajadas = total_horas_normales + total_horas_extra_50 + total_horas_extra_100;
                lbl_horas_realmente_trabajadas.Text = horas_realmente_trabajadas.ToString("#,##0.00");
                lbl_horas_realmente_trabajadas.ToolTip = "Total horas normales: " + total_horas_normales.ToString("#,##0.00") + " - Total horas extra 50%: " + total_horas_extra_50.ToString("#,##0.00") + " - Total horas extra 100%: " + total_horas_extra_100.ToString("#,##0.00");

                decimal horas_trabajadas_segun_datos_prueba = prueba_100 > 0 ? (total_horas_normales + total_horas_extra_50 + total_horas_extra_100) : (prueba_50 > 0 ? (total_horas_normales + total_horas_extra_50) : (total_horas_normales));
                string tooltip = prueba_100 > 0 ? "Total horas normales: " + total_horas_normales.ToString("#,##0.00") + " - Total horas extra 50%: " + total_horas_extra_50.ToString("#,##0.00") + " - Total horas extra 100%: " + total_horas_extra_100.ToString("#,##0.00") : (prueba_50 > 0 ? "Total horas normales: " + total_horas_normales.ToString("#,##0.00") + " - Total horas extra 50%: " + total_horas_extra_50.ToString("#,##0.00") : "Total horas normales: " + total_horas_normales.ToString("#,##0.00"));
                lbl_horas_trabajadas_segun_datos_prueba.Text = horas_trabajadas_segun_datos_prueba.ToString("#,##0.00");
                lbl_horas_trabajadas_segun_datos_prueba.ToolTip = tooltip;

                decimal valor_dolar_mes = cxt.Valores_dolar.FirstOrDefault(x => x.mes == mes && x.anio == anio) != null ? cxt.Valores_dolar.First(x => x.mes == mes && x.anio == anio).valor : 0;
                lbl_valor_dolar_mes.Text = valor_dolar_mes.ToString("#,##0.00");


                lbl_costo_horas_hombre_real.Text = horas_realmente_trabajadas > 0 ? (nueva_masa_salarial / horas_realmente_trabajadas).ToString("$ #,##0.00") : 0.ToString("$ #,##0.00");
                lbl_costo_horas_hombre_real_prueba.Text = horas_trabajadas_segun_datos_prueba > 0 ? (nueva_masa_salarial_prueba / horas_trabajadas_segun_datos_prueba).ToString("$ #,##0.00") : 0.ToString("$ #,##0.00");

                lbl_costo_horas_hombre_real_dolar.Text = ((horas_realmente_trabajadas > 0 ? (nueva_masa_salarial / horas_realmente_trabajadas) : 0) / valor_dolar_mes).ToString("$ #,##0.00");
                lbl_costo_horas_hombre_real_prueba_dolar.Text = ((horas_trabajadas_segun_datos_prueba > 0 ? (nueva_masa_salarial_prueba / horas_trabajadas_segun_datos_prueba) : 0) / prueba_dolar).ToString("$ #,##0.00");

                ddl_anio.Enabled = false;
                ddl_mes.Enabled = false;
                ddl_tipo_empleado.Enabled = false;

                Session["planilla_calculos_mes"] = mes;
                Session["planilla_calculos_anio"] = anio;

                Session["planilla_calculos_horas_normales"] = total_horas_normales;
                Session["planilla_calculos_horas_extra_50"] = total_horas_extra_50;
                Session["planilla_calculos_horas_extra_100"] = total_horas_extra_100;

                Session["planilla_calculos_costo_hora_teorico"] = (total_sueldos / dias_mes / cantidad_empleados / horas_dia);
                Session["planilla_calculos_costo_hora_teorico_ajustado"] = costo_hora_teorico_ajustado;
                Session["planilla_calculos_costo_hora_real"] = horas_realmente_trabajadas > 0 ? (nueva_masa_salarial / horas_realmente_trabajadas) : 0;

                Session["planilla_calculos_categoria_empleado"] = ddl_tipo_empleado.SelectedItem.Text;
            }
        }

        protected void btn_nueva_busqueda_Click(object sender, EventArgs e)
        {
            ddl_anio.Enabled = true;
            ddl_mes.Enabled = true;
            ddl_tipo_empleado.Enabled = true;

            btn_nueva_busqueda.Visible = false;
            div_resultados_busqueda.Visible = false;
            btn_buscar.Visible = true;
        }


        protected void gv_planilla_empleados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.ControlStyle.BackColor = Color.LightGray;
            }
        }

        protected void btn_ver_ServerClick(object sender, EventArgs e)
        {
            string str_id_empleado = ((System.Web.UI.HtmlControls.HtmlButton)sender).Attributes["data-id"];
            int id_empleado = Convert.ToInt32(str_id_empleado);
            int mes = Convert.ToInt32(ddl_mes.SelectedItem.Value);
            int anio = Convert.ToInt32(ddl_anio.SelectedItem.Value);
            Session["valores_horas"] = new List<string>() { id_empleado.ToString(), mes.ToString(), anio.ToString() };
            Response.Redirect("~/Aplicativo/admin_horas_mes.aspx");
        }

        protected void btn_editar_sueldo_empleado_Click(object sender, EventArgs e)
        {
            int id_empleado = Convert.ToInt32(id_empleado_hidden.Value);
            int mes = Convert.ToInt32(ddl_mes.SelectedItem.Value);
            int anio = Convert.ToInt32(ddl_anio.SelectedItem.Value);

            decimal sueldo = 0;
            decimal.TryParse(tb_sueldo_empleado.Value, out sueldo);

            using (var cxt = new Model1Container())
            {
                Resumen_mes_empleado rme = cxt.Resumenes_meses_empleados.FirstOrDefault(rrmmee => rrmmee.id_empleado == id_empleado && rrmmee.mes == mes && rrmmee.anio == anio);
                if (rme != null)
                {
                    rme.Sueldo = sueldo;
                }
                else
                {
                    rme = new Resumen_mes_empleado()
                    {
                        id_empleado = id_empleado,
                        mes = mes,
                        anio = anio,
                        dias_laborables = 0,
                        dias_ausente = 0,
                        dias_presente = 0,
                        dias_por_cargar = 0,
                        dias_out = 0,
                        dias_presentes_en_dias_no_laborables = 0,
                        total_horas_normales = "00:00",
                        total_horas_extra_50 = "00:00",
                        total_horas_extra_100 = "00:00",
                        Sueldo = sueldo,
                        total_horas_ausente = "00:00",
                        total_horas_guardia = "00:00",
                        total_horas_varios_taller = "00:00"
                    };

                    cxt.Resumenes_meses_empleados.Add(rme);
                }

                cxt.SaveChanges();
            }

            tb_sueldo_empleado.Value = string.Empty;
            Cargar_busqueda();
        }

        protected void txt_prueba_TextChanged(object sender, EventArgs e)
        {
            Cargar_busqueda();
        }

        protected void btn_ver_planilla_calculos_ServerClick(object sender, EventArgs e)
        {
            //ya estan cargados en variables de session los datos necesarios.
            //se cargan en la funcion Cargar_busqueda
            Response.Redirect("~/Aplicativo/planilla_calculos.aspx");
        }
    }
}