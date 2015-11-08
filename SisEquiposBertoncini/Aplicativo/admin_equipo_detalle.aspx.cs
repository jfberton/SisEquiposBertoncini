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
    public partial class admin_equipo_detalle : System.Web.UI.Page
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

                CargarCategorias();

                var session_id_equipo = Session["id_equipo"];

                if (session_id_equipo != null)
                {
                    using (var cxt = new Model1Container())
                    {
                        int id_equipo = Convert.ToInt32(session_id_equipo);
                        Equipo equipo = cxt.Equipos.Include("Items_por_amortizar").First(ee => ee.id_equipo == id_equipo);
                        Session["equipo"] = equipo;
                        Session["id_equipo"] = null;
                    }
                }

                CargarValoresEquipo();
            }
        }

        private void CargarCategorias()
        {
            ddl_categorias.Items.Add(new ListItem() { Value = "0", Text = "Seleccionar:" });
            using (var cxt = new Model1Container())
            {
                foreach (var categoria in cxt.Categorias_equipos)
                {
                    ddl_categorias.Items.Add(new ListItem() { Value = categoria.id_categoria.ToString(), Text = categoria.nombre });
                }
            }
        }

        private void CargarValoresEquipo()
        {
            Equipo equipo = Session["equipo"] as Equipo;

            using (var cxt = new Model1Container())
            {
                if (equipo == null)
                {
                    Session["equipo"] = new Equipo();
                    lbl_costo_mensual.Text = Cadena.Formato_moneda(0, Cadena.Moneda.dolares);
                    lbl_costo_total_0km.Text = Cadena.Formato_moneda(0, Cadena.Moneda.dolares);
                    lbl_valor_por_amortizar.Text = Cadena.Formato_moneda(0, Cadena.Moneda.dolares);
                }
                else
                {
                    tb_nombre.Value = equipo.nombre;
                    ddl_categorias.SelectedValue = equipo.id_categoria.ToString();
                    tb_notas_equipo.Value = equipo.notas;
                    chk_out.Checked = equipo.OUT;

                    int mes = DateTime.Today.Month;
                    int anio = DateTime.Today.Year;

                    var items = (from ii in equipo.Items_por_amortizar
                                 select new
                                 {
                                     id_parte = ii.id_item,
                                     nombre_parte = ii.nombre,
                                     costo_cero = ii.costo_cero_km_uss,
                                     porcentaje_usado = ii.porcentaje_usado,
                                     porcentaje_valor_residual = ii.porcentaje_valor_recidual,
                                     valor_por_amortizar = ii.valor_por_amortizar,
                                     meses_por_amortizar = ii.meses_por_amortizar,
                                     periodo_alta_anio = ii.periodo_alta_anio,
                                     periodo_alta_mes = ii.periodo_alta_mes,
                                     costo_mensual = ii.costo_mensual,
                                     meses_amortizados = ii.Periodos_amortizados(mes, anio),
                                     restan_amortizar = ii.Restan_por_amortizar(mes, anio)
                                 }).ToList();

                    var items_formateados = (from ii in items
                                             select new
                                             {
                                                 id_parte = ii.id_parte,
                                                 nombre_parte = ii.nombre_parte,
                                                 costo_cero = Cadena.Formato_moneda(ii.costo_cero, Cadena.Moneda.dolares),
                                                 porcentaje_usado = Cadena.Formato_porcentaje(ii.porcentaje_usado),
                                                 porcentaje_valor_residual = Cadena.Formato_porcentaje(ii.porcentaje_valor_residual),
                                                 valor_por_amortizar = Cadena.Formato_moneda(ii.valor_por_amortizar, Cadena.Moneda.dolares),
                                                 meses_por_amortizar = ii.meses_por_amortizar,
                                                 costo_mensual = Cadena.Formato_moneda(ii.costo_mensual, Cadena.Moneda.dolares),
                                                 periodo_alta = ii.periodo_alta_mes.ToString() + "/" + ii.periodo_alta_anio.ToString(),
                                                 meses_amortizados = ii.meses_amortizados,
                                                 restan_amortizar = ii.restan_amortizar
                                             }).ToList();

                    gv_partes.DataSource = items_formateados;
                    gv_partes.DataBind();

                    gv_partes.Font.Size = new FontUnit("11");

                    lbl_costo_total_0km.Text = Cadena.Formato_moneda(items.Sum(ii => ii.costo_cero), Cadena.Moneda.dolares);
                    lbl_costo_mensual.Text = Cadena.Formato_moneda(items.Where(x => x.restan_amortizar > 0).Sum(ii => ii.costo_mensual), Cadena.Moneda.dolares);
                    lbl_valor_por_amortizar.Text = Cadena.Formato_moneda(items.Where(x => x.restan_amortizar > 0).Sum(ii => ii.valor_por_amortizar), Cadena.Moneda.dolares);
                }
            }
        }

        protected void cv_categoria_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ddl_categorias.SelectedItem.Value != "0";
        }

        protected void cv_costo_cero_km_ServerValidate(object source, ServerValidateEventArgs args)
        {
            decimal d;
            args.IsValid = decimal.TryParse(tb_costo_cero_km.Value, out d);
        }

        protected void cv_porcentaje_usado_ServerValidate(object source, ServerValidateEventArgs args)
        {
            decimal d;
            args.IsValid = decimal.TryParse(tb_porcentaje_usado.Value, out d);
        }

        protected void cv_valor_residual_ServerValidate(object source, ServerValidateEventArgs args)
        {
            decimal d;
            args.IsValid = decimal.TryParse(tb_porcentaje_valor_residual.Value, out d);
        }

        protected void cv_meses_por_amortizar_ServerValidate(object source, ServerValidateEventArgs args)
        {
            int i;
            args.IsValid = int.TryParse(tb_meses_por_amortizar.Value, out i);
        }

        protected void btn_aceptar_parte_Click(object sender, EventArgs e)
        {
            Equipo equipo = Session["equipo"] as Equipo;

            Validate("parte");
            if (IsValid)
            {
                equipo.nombre = tb_nombre.Value;
                equipo.id_categoria = Convert.ToInt32(ddl_categorias.SelectedItem.Value);
                equipo.notas = tb_notas_equipo.Value;

                int mes = 0;
                int anio = 0;
                int.TryParse(tb_periodo_alta.Value.Split('/')[0], out mes);
                int.TryParse(tb_periodo_alta.Value.Split('/')[1], out anio);

                Item_por_amortizar parte = new Item_por_amortizar();
                parte.costo_cero_km_uss = Convert.ToDecimal(tb_costo_cero_km.Value);
                parte.meses_por_amortizar = Convert.ToInt32(tb_meses_por_amortizar.Value);
                parte.nombre = tb_nombre_parte.Value;
                parte.porcentaje_usado = Convert.ToDecimal(tb_porcentaje_usado.Value);
                parte.porcentaje_valor_recidual = Convert.ToDecimal(tb_porcentaje_valor_residual.Value);
                parte.periodo_alta_anio = anio;
                parte.periodo_alta_mes = mes;
                equipo.Items_por_amortizar.Add(parte);

                tb_costo_cero_km.Value = string.Empty;
                tb_meses_por_amortizar.Value = string.Empty;
                tb_nombre_parte.Value = string.Empty;
                tb_porcentaje_usado.Value = string.Empty;
                tb_porcentaje_valor_residual.Value = string.Empty;
                tb_periodo_alta.Value = string.Empty;

                Session["equipo"] = equipo;

                CargarValoresEquipo();
            }
        }

        protected void gv_partes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.ControlStyle.BackColor = Color.LightGray;
            }
        }

        protected void btn_cancelar_Click(object sender, EventArgs e)
        {
            Session["equipo"] = null;
            Response.Redirect("~/Aplicativo/admin_equipos.aspx");
        }

        protected void btn_aceptar_eliminacion_Click(object sender, EventArgs e)
        {
            string datos_parte_a_eliminar = id_item_por_eliminar.Value;
            int id = Convert.ToInt32(datos_parte_a_eliminar.Split('-')[0]);
            if (id > 0)
            {
                //existe y fue guardado
                using (var cxt = new Model1Container())
                {
                    Item_por_amortizar i = cxt.Items_por_amortizar.FirstOrDefault(ii => ii.id_item == id);
                    int id_equipo = i.Equipo.id_equipo;
                    cxt.Items_por_amortizar.Remove(i);
                    cxt.SaveChanges();
                    Session["equipo"] = cxt.Equipos.First(ee => ee.id_equipo == id_equipo);
                }
            }
            else
            {
                Equipo session_equipo = Session["equipo"] as Equipo;
                string nombre_item = datos_parte_a_eliminar.Split('-')[1];
                session_equipo.Items_por_amortizar.Remove(session_equipo.Items_por_amortizar.First(ii => ii.nombre == nombre_item));
            }

            CargarValoresEquipo();
        }

        protected void btn_guardar_equipo_ServerClick(object sender, EventArgs e)
        {
            this.Validate("equipo");
            if (IsValid)
            {
                Equipo session_equipo = Session["equipo"] as Equipo;

                using (var cxt = new Model1Container())
                {
                    Equipo equipo = new Equipo();
                    
                    if (session_equipo.id_equipo > 0)
                    {
                        equipo = cxt.Equipos.First(ee => ee.id_equipo == session_equipo.id_equipo);
                    }
                    else
                    {
                        cxt.Equipos.Add(equipo);
                    }

                    equipo.Generico = false;
                    equipo.notas = tb_notas_equipo.Value;
                    equipo.nombre = tb_nombre.Value;
                    equipo.id_categoria = Convert.ToInt32(ddl_categorias.SelectedItem.Value);
                    equipo.OUT = chk_out.Checked;

                    foreach (Item_por_amortizar parte in session_equipo.Items_por_amortizar)
                    {
                        if (parte.id_item == 0)
                        {
                            equipo.Items_por_amortizar.Add(new Item_por_amortizar()
                            {
                                costo_cero_km_uss = parte.costo_cero_km_uss,
                                meses_por_amortizar = parte.meses_por_amortizar,
                                nombre = parte.nombre,
                                porcentaje_usado = parte.porcentaje_usado,
                                porcentaje_valor_recidual = parte.porcentaje_valor_recidual,
                                periodo_alta_anio = parte.periodo_alta_anio,
                                periodo_alta_mes = parte.periodo_alta_mes
                            });
                        }
                    }

                    try
                    {
                        cxt.SaveChanges();
                        Session["equipo"] = null;
                        MessageBox.Show(this, "El equipo se guardo correctamente", MessageBox.Tipo_MessageBox.Success, "Genial!", "admin_equipos.aspx");
                    }
                    catch (Exception ex)
                    {
                        //inspeccion rapida y ((System.Data.Entity.Validation.DbEntityValidationException)ex).EntityValidationErrors para ver el error
                        MessageBox.Show(this, "Upps! ocurrió un error!", MessageBox.Tipo_MessageBox.Danger);
                    }
                }
            }
            else
            {
                MessageBox.Show(this, "Verifique los valores ingresados", MessageBox.Tipo_MessageBox.Info);
            }

        }

        protected void cv_periodo_alta_ServerValidate(object source, ServerValidateEventArgs args)
        {
            int mes = 0;
            int anio = 0;
            if (int.TryParse(tb_periodo_alta.Value.Split('/')[0], out mes) && int.TryParse(tb_periodo_alta.Value.Split('/')[1], out anio))
            {
                args.IsValid = true;
            }
            else
            {
                args.IsValid = false;
            }
        }
    }
}