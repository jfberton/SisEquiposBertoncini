using SisEquiposBertoncini.Aplicativo.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisEquiposBertoncini.Aplicativo
{
    public partial class planilla_combustibles : System.Web.UI.Page
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
                else
                {
                    if (usuariologueado.perfil != perfil_usuario.Admin && usuariologueado.perfil != perfil_usuario.Jefe)
                    {
                        Response.Redirect("~/Default.aspx?mode=trucho");
                    }
                }

                Cargar_ddls();

                string str_id_equipo = Request.QueryString["eq"];
                string str_edita = Request.QueryString["ed"];
                string str_mes = Request.QueryString["m"];
                string str_anio = Request.QueryString["a"];

                if (str_id_equipo != null)
                {
                    ddl_equipo.SelectedValue = str_id_equipo;
                    ddl_mes.SelectedValue = str_mes;
                    ddl_anio.SelectedValue = str_anio;

                    Estado_busqueda(false);
                    using (var cxt = new Model1Container())
                    {
                        int id_equipo = Convert.ToInt32(str_id_equipo);
                        int anio = Convert.ToInt32(str_anio);
                        int mes = Convert.ToInt32(str_mes);

                        CrearMostrarTabla(id_equipo, mes, anio);
                    }
                }
                else
                {
                    Estado_busqueda(true);
                }
            }
        }

        protected void btn_buscar_Click(object sender, EventArgs e)
        {
            int id_equipo = Convert.ToInt32(ddl_equipo.SelectedItem.Value);
            int anio = Convert.ToInt32(ddl_anio.SelectedItem.Value);
            int mes = Convert.ToInt32(ddl_mes.SelectedItem.Value);

            Response.Redirect("~/Aplicativo/planilla_combustibles.aspx?eq=" + id_equipo.ToString() + "&ed=1&m=" + mes.ToString() + "&a=" + anio.ToString());
        }

        protected void btn_nueva_busqueda_Click(object sender, EventArgs e)
        {
            Estado_busqueda(true);
        }

        protected void gv_combustible_PreRender(object sender, EventArgs e)
        {
            if (gv_combustible.Rows.Count > 0)
            {
                gv_combustible.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }


        private void CrearMostrarTabla(int id_equipo, int mes, int anio)
        {
            using (var cxt = new Model1Container())
            {
                DateTime primer_dia_mes = new DateTime(anio, mes, 1);
                DateTime ultimo_dia_mes = new DateTime(anio,mes,DateTime.DaysInMonth(anio,mes));

                var items_planilla = (from pc in cxt.Planilla_combustibles
                                      where
                                        pc.id_equipo == id_equipo &&
                                        pc.fecha >= primer_dia_mes &&
                                        pc.fecha <= ultimo_dia_mes
                                      select pc).OrderBy(x=>x.fecha).ToList();

                decimal km = 0;
                foreach (Planilla_combustible item in items_planilla)
                {
                    if (km > 0 && item.tanque_lleno == true)
                    {
                        item.promedio = item.litros * Convert.ToDecimal(100) / (item.km - km);
                    }
                    else
                    {
                        item.promedio = 0;
                    }

                    km = item.km;
                }

                cxt.SaveChanges();
                
                var items_grilla = (from ip in items_planilla
                                    select new { 
                                        id_equipo = ip.Equipo.id_equipo,
                                        equipo = ip.Equipo.nombre,
                                        fecha = ip.fecha,
                                        chofer = ip.chofer,
                                        tanque_lleno = ip.tanque_lleno,
                                        litros = ip.litros,
                                        km = ip.km,
                                        promedio = ip.promedio,
                                        costo_total_facturado = ip.costo_total_facturado
                                    }).ToList();

                gv_combustible.DataSource = items_grilla;
                gv_combustible.DataBind();
            }
        }

        private void Cargar_ddls()
        {
            using (var cxt = new Model1Container())
            {
                var equipos_habilitados = cxt.Equipos.Where(ee => ee.fecha_baja == null && !ee.Generico);
                foreach (Equipo equipo in equipos_habilitados.OrderBy(x => x.nombre))
                {
                    ddl_equipo.Items.Add(new ListItem() { Value = equipo.id_equipo.ToString(), Text = equipo.nombre });
                }

                for (int anio = 2015; anio <= DateTime.Today.Year + 1; anio++)
                {
                    ddl_anio.Items.Add(new ListItem() { Value = anio.ToString(), Text = anio.ToString() });
                }
            }
        }

        private void Estado_busqueda(bool habilitado)
        {
            //div_buscar_primero.Visible = habilitado;
            //div_tree.Visible = !habilitado;

            ddl_equipo.Enabled = habilitado;
            ddl_anio.Enabled = habilitado;
            ddl_mes.Enabled = habilitado;

            btn_nueva_busqueda.Visible = !habilitado;
            btn_buscar.Visible = habilitado;
            gv_combustible.Visible = !habilitado;
        }


    }
}