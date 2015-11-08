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
                                       sueldo = (cxt.Resumenes_meses_empleados.FirstOrDefault(rme=>rme.id_empleado==e.id_empleado && rme.mes == mes && rme.anio == anio)!=null?cxt.Resumenes_meses_empleados.FirstOrDefault(rme=>rme.id_empleado==e.id_empleado && rme.mes == mes && rme.anio == anio).Sueldo:0),
                                       dias_mes = (cxt.Resumenes_meses_empleados.FirstOrDefault(rme => rme.id_empleado == e.id_empleado && rme.mes == mes && rme.anio == anio) != null ? cxt.Resumenes_meses_empleados.FirstOrDefault(rme => rme.id_empleado == e.id_empleado && rme.mes == mes && rme.anio == anio).dias_laborables : 0),
                                       dias_out = (cxt.Resumenes_meses_empleados.FirstOrDefault(rme => rme.id_empleado == e.id_empleado && rme.mes == mes && rme.anio == anio) != null ? cxt.Resumenes_meses_empleados.FirstOrDefault(rme => rme.id_empleado == e.id_empleado && rme.mes == mes && rme.anio == anio).dias_out : 0),
                                   }).ToList();
                var items_grilla = (from ie in items_empleados
                                    select new
                                    {
                                        empleado_id = ie.empleado_id,
                                        empleado = ie.empleado,
                                        sueldo = ie.sueldo,
                                        dias_mes = ie.dias_mes,
                                        dias_out = ie.dias_out,
                                        costo_mensual_ponderado = ie.dias_mes > 0 ? ie.sueldo - (ie.sueldo / ie.dias_mes) * ie.dias_out : 0
                                    }).ToList();

                gv_planilla_empleados.DataSource = items_grilla;
                gv_planilla_empleados.DataBind();

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