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
                                        costo_mensual_ponderado = ie.sueldo-(ie.sueldo/ie.dias_mes)*ie.dias_out
                                    }).ToList();

                gv_planilla_empleados.DataSource = items_grilla;
                gv_planilla_empleados.DataBind();
            }
        }

        protected void btn_nueva_busqueda_Click(object sender, EventArgs e)
        {

        }

        protected void btn_editar_sueldo_empleado_ServerClick(object sender, EventArgs e)
        {

        }

        protected void gv_planilla_empleados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.ControlStyle.BackColor = Color.LightGray;
            }
        }
    }
}