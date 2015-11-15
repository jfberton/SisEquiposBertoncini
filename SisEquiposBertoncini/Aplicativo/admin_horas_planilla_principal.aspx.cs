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

                btn_nueva_busqueda.Visible = false;
                div_resultados_busqueda.Visible = false;
            }
        }

        private void Cargar_ddls()
        {
            for (int i = 2015; i <= DateTime.Today.Year; i++)
            {
                ddl_anio.Items.Add(new ListItem() { Text = i.ToString(), Value = i.ToString() });
            }
        }

        protected void btn_buscar_Click(object sender, EventArgs e)
        {
            Cargar_busqueda();
        }

        private void Cargar_busqueda()
        {
            btn_buscar.Visible = false;
            btn_nueva_busqueda.Visible = true;
            div_resultados_busqueda.Visible = true;

            using (var cxt = new Model1Container())
            {
                int mes = Convert.ToInt32(ddl_mes.SelectedItem.Value);
                int anio = Convert.ToInt32(ddl_anio.SelectedItem.Value);
                Categoria_empleado mecanicos = cxt.Categorias_empleados.First(cc => cc.nombre == "Mecánico");
                Categoria_empleado soldadores = cxt.Categorias_empleados.First(cc => cc.nombre == "Soldador");
                Categoria_empleado pintores = cxt.Categorias_empleados.First(cc => cc.nombre == "Pintor");

                List<Empleado> empleados = new List<Empleado>();

                if (ddl_tipo_empleado.SelectedItem.Text == "Mecánicos - Pintores")
                {
                    empleados = cxt.Empleados.Where(ee => (ee.id_categoria == mecanicos.id_categoria || ee.id_categoria == pintores.id_categoria) && ee.fecha_baja == null).ToList();
                }
                else
                {
                    empleados = cxt.Empleados.Where(ee => ee.id_categoria == soldadores.id_categoria && ee.fecha_baja == null).ToList();
                }

                var items_empleados = (from e in empleados
                                   select new
                                   {
                                       empleado_id = e.id_empleado,
                                       empleado = e.nombre,
                                       horas_ausente = (cxt.Resumenes_meses_empleados.FirstOrDefault(rme => rme.id_empleado == e.id_empleado && rme.mes == mes && rme.anio == anio) != null ? cxt.Resumenes_meses_empleados.FirstOrDefault(rme => rme.id_empleado == e.id_empleado && rme.mes == mes && rme.anio == anio).total_horas_ausente : "00:00"),
                                       horas_extra_50 = (cxt.Resumenes_meses_empleados.FirstOrDefault(rme => rme.id_empleado == e.id_empleado && rme.mes == mes && rme.anio == anio) != null ? cxt.Resumenes_meses_empleados.FirstOrDefault(rme => rme.id_empleado == e.id_empleado && rme.mes == mes && rme.anio == anio).total_horas_extra_50 : "00:00"),
                                       horas_extra_100 = (cxt.Resumenes_meses_empleados.FirstOrDefault(rme => rme.id_empleado == e.id_empleado && rme.mes == mes && rme.anio == anio) != null ? cxt.Resumenes_meses_empleados.FirstOrDefault(rme => rme.id_empleado == e.id_empleado && rme.mes == mes && rme.anio == anio).total_horas_extra_100 : "00:00"),
                                       sueldo = (cxt.Resumenes_meses_empleados.FirstOrDefault(rme=>rme.id_empleado==e.id_empleado && rme.mes == mes && rme.anio == anio)!=null?cxt.Resumenes_meses_empleados.FirstOrDefault(rme=>rme.id_empleado==e.id_empleado && rme.mes == mes && rme.anio == anio).Sueldo:0),
                                       dias_mes = (cxt.Resumenes_meses_empleados.FirstOrDefault(rme => rme.id_empleado == e.id_empleado && rme.mes == mes && rme.anio == anio) != null ? cxt.Resumenes_meses_empleados.FirstOrDefault(rme => rme.id_empleado == e.id_empleado && rme.mes == mes && rme.anio == anio).dias_laborables : 0),
                                       dias_out = (cxt.Resumenes_meses_empleados.FirstOrDefault(rme => rme.id_empleado == e.id_empleado && rme.mes == mes && rme.anio == anio) != null ? cxt.Resumenes_meses_empleados.FirstOrDefault(rme => rme.id_empleado == e.id_empleado && rme.mes == mes && rme.anio == anio).dias_out : 0),
                                   }).ToList();

                var items_grilla = (from ie in items_empleados
                                    select new
                                    {
                                        empleado_id = ie.empleado_id,
                                        empleado = ie.empleado,
                                        horas_ausente = Convert.ToDecimal(ie.horas_ausente.Split(':')[0]) + (Convert.ToDecimal(ie.horas_ausente.Split(':')[0]) / Convert.ToDecimal(60)),
                                        horas_extra_50 = Convert.ToDecimal(ie.horas_extra_50.Split(':')[0]) + (Convert.ToDecimal(ie.horas_extra_50.Split(':')[0]) / Convert.ToDecimal(60)),
                                        horas_extra_100 = Convert.ToDecimal(ie.horas_extra_100.Split(':')[0]) + (Convert.ToDecimal(ie.horas_extra_100.Split(':')[0]) / Convert.ToDecimal(60)),
                                        sueldo = ie.sueldo,
                                        dias_mes = ie.dias_mes,
                                        dias_out = ie.dias_out,
                                        costo_mensual_ponderado = ie.dias_mes > 0 ? ie.sueldo - (ie.sueldo / ie.dias_mes) * ie.dias_out : 0
                                    }).ToList();

                decimal total_sueldos = 0;
                decimal costo_mensual_ponderado_total = 0;
                decimal costo_hora_teorico_ajustado = 0;
                decimal dias_mes = 0;
                decimal cantidad_empleados = Convert.ToDecimal(items_grilla.Count);
                decimal horas_dia = Convert.ToDecimal(8);
                decimal total_horas_ausente = 0;
                decimal total_horas_extra_50 = 0;
                decimal total_horas_extra_100 = 0;

                foreach (var item in items_grilla)
                {
                    total_sueldos = total_sueldos + item.sueldo;
                    costo_mensual_ponderado_total = costo_mensual_ponderado_total + item.costo_mensual_ponderado;
                    total_horas_ausente = total_horas_ausente + item.horas_ausente;
                    total_horas_extra_50 = total_horas_extra_50 + item.horas_extra_50;
                    total_horas_extra_100 = total_horas_extra_100 + item.horas_extra_100;

                    if (dias_mes < item.dias_mes)
                    {
                        dias_mes = item.dias_mes;
                    }
                }

                costo_hora_teorico_ajustado = (costo_mensual_ponderado_total / dias_mes / cantidad_empleados / horas_dia);
                
                gv_planilla_empleados.DataSource = items_grilla;
                gv_planilla_empleados.DataBind();

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
                lbl_costo_ausentes_prueba.Text = (costo_hora_teorico_ajustado * Convert.ToDecimal(txt_horas_ausentes_totales_prueba.Text!=string.Empty?txt_horas_ausentes_totales_prueba.Text:"0")).ToString("$ #,##0.00");
                lbl_horas_ausentes_totales.Text = total_horas_ausente.ToString("#,##0.00");
                lbl_ausentismo_porcentaje.Text = (total_horas_ausente / (dias_mes * cantidad_empleados * horas_dia)).ToString("##0.00 %");
                lbl_ausentismo_porcentaje_prueba.Text = (Convert.ToDecimal(txt_horas_ausentes_totales_prueba.Text != string.Empty ? txt_horas_ausentes_totales_prueba.Text : "0") / (dias_mes * cantidad_empleados * horas_dia)).ToString("##0.00 %");
                lbl_costo_horas_hombre_teorico_ajustado_1.Text = lbl_costo_horas_hombre_teorico_ajustado.Text;


                lbl_costo_horas_hombre_teorico_ajustado_2.Text = lbl_costo_horas_hombre_teorico_ajustado.Text;
                lbl_costo_horas_hombre_teorico_ajustado_3.Text = lbl_costo_horas_hombre_teorico_ajustado.Text;



                ddl_anio.Enabled = false;
                ddl_mes.Enabled = false;
                ddl_tipo_empleado.Enabled = false;
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

    }
}