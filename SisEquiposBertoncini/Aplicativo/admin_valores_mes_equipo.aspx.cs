using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SisEquiposBertoncini.Aplicativo.Datos;
using System.Web.UI.HtmlControls;
using SisEquiposBertoncini.Aplicativo.Controles;
using System.Web.Services;

namespace SisEquiposBertoncini.Aplicativo
{
    public partial class admin_valores_mes_equipo : System.Web.UI.Page
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

                        Ingreso_egreso_mensual_equipo ioEquipo = cxt.Ingresos_egresos_mensuales_equipos.FirstOrDefault(iioo => iioo.id_equipo == id_equipo && iioo.mes == mes && iioo.anio == anio);
                        CrearMostrarTabla(ioEquipo);
                    }
                }
                else
                {
                    Estado_busqueda(true);
                }
            }
        }

        private void Cargar_ddls()
        {
            using (var cxt = new Model1Container())
            {
                var equipos_habilitados = cxt.Equipos.Where(ee => ee.fecha_baja == null && !ee.Generico);
                foreach (Equipo equipo in equipos_habilitados)
                {
                    ddl_equipo.Items.Add(new ListItem() { Value = equipo.id_equipo.ToString(), Text = equipo.nombre });
                }

                for (int anio = 2015; anio <= DateTime.Today.Year; anio++)
                {
                    ddl_anio.Items.Add(new ListItem() { Value = anio.ToString(), Text = anio.ToString() });
                }
            }
        }

        private void Estado_busqueda(bool habilitado)
        {
            div_buscar_primero.Visible = habilitado;
            div_tree.Visible = !habilitado;

            ddl_equipo.Enabled = habilitado;
            ddl_anio.Enabled = habilitado;
            ddl_mes.Enabled = habilitado;

            btn_nueva_busqueda.Visible = !habilitado;
            btn_buscar.Visible = habilitado;
        }

        protected void btn_buscar_Click(object sender, EventArgs e)
        {
            int id_equipo = Convert.ToInt32(ddl_equipo.SelectedItem.Value);
            int anio = Convert.ToInt32(ddl_anio.SelectedItem.Value);
            int mes = Convert.ToInt32(ddl_mes.SelectedItem.Value);

            Response.Redirect("~/Aplicativo/admin_valores_mes_equipo.aspx?eq=" + id_equipo.ToString() + "&ed=1&m=" + mes.ToString() + "&a=" + anio.ToString());
        }

        private void CrearMostrarTabla(Ingreso_egreso_mensual_equipo ioEquipo)
        {
            using (var cxt = new Model1Container())
            {
                Ingreso_egreso_mensual_equipo valores_equipo_mes_cxt;

                if (ioEquipo != null)
                {
                    valores_equipo_mes_cxt = cxt.Ingresos_egresos_mensuales_equipos.FirstOrDefault(ii => ii.id_ingreso_egreso_mensual == ioEquipo.id_ingreso_egreso_mensual);
                }
                else
                {
                    valores_equipo_mes_cxt = new Ingreso_egreso_mensual_equipo();
                    valores_equipo_mes_cxt.id_equipo = Convert.ToInt32(ddl_equipo.SelectedItem.Value);
                    valores_equipo_mes_cxt.mes = Convert.ToInt32(ddl_mes.SelectedItem.Value);
                    valores_equipo_mes_cxt.anio = Convert.ToInt32(ddl_anio.SelectedItem.Value);
                    cxt.Ingresos_egresos_mensuales_equipos.Add(valores_equipo_mes_cxt);
                    cxt.SaveChanges();
                }

                //Seteo el valor del concepto amortizacion es el costo del equipo por amortizar por el valor del dolar del mes
                int anio = Convert.ToInt32(ddl_anio.SelectedItem.Value);
                int mes = Convert.ToInt32(ddl_mes.SelectedItem.Value);
                Equipo equipo = cxt.Equipos.FirstOrDefault(ee => ee.id_equipo == valores_equipo_mes_cxt.id_equipo);
                Item_ingreso_egreso amortizacion = cxt.Items_ingresos_egresos.First(ii => ii.nombre == "Amortización" && ii.Padre.nombre == "Costos Fijos No Erogables");
                Valor_mes vm = valores_equipo_mes_cxt.Valores_meses.FirstOrDefault(ii => ii.id_item == amortizacion.id_item);

                if (vm == null)
                {
                    vm = new Valor_mes()
                    {
                        id_item = amortizacion.id_item,
                        valor = decimal.Round((equipo.Costo_amortizacion_mensual * ValorDolarMes.Obtener(mes, anio)), 2)
                    };

                    valores_equipo_mes_cxt.Valores_meses.Add(vm);
                    
                }
                else
                {
                    vm.valor = decimal.Round((equipo.Costo_amortizacion_mensual * ValorDolarMes.Obtener(mes, anio)), 2);
                }
                
                cxt.SaveChanges();
                
                List<Item_ingreso_egreso> conceptos;
                conceptos = cxt.Items_ingresos_egresos.ToList();
                var roots = conceptos.Where(ii => ii.id_item_padre == null);

                HtmlGenericControl tree = new HtmlGenericControl("table");
                tree.Attributes.Add("runat", "server");
                tree.Attributes.Add("class", "tree table");
                tree.ID = "tree";

                foreach (Item_ingreso_egreso item in roots)
                {
                    AgregarNodo(item, tree, cxt, valores_equipo_mes_cxt);
                }

                div_tree.Controls.Clear();

                div_tree.Controls.Add(tree);
            }

            Recalcular_subtotales();
        }

        private void AgregarNodo(Item_ingreso_egreso item, HtmlGenericControl tree, Model1Container cxt, Ingreso_egreso_mensual_equipo io_equipo)
        {
            item = cxt.Items_ingresos_egresos.First(ii => ii.id_item == item.id_item);

            Valor_mes vm = io_equipo.Valores_meses.FirstOrDefault(ii => ii.id_item == item.id_item);
            if (vm == null)
            {
                vm = new Valor_mes()
                {
                    id_item = item.id_item,
                    valor = 0
                };

                io_equipo.Valores_meses.Add(vm);
                cxt.SaveChanges();
            }

            HtmlGenericControl row = new HtmlGenericControl("tr");
            row.Attributes.Add("class", "treegrid-" + item.id_item + (item.id_item_padre != null ? " treegrid-parent-" + item.id_item_padre : "") + (item.id_item_padre == null ? " h4" : "") + (item.tipo == "Ingreso" ? " alert-success" : " alert-danger"));
            row.Attributes.Add("title", item.descripcion);

            HtmlGenericControl column = new HtmlGenericControl("td");
            column.InnerHtml = (item.Hijos.Count > 0 ? "<strong>" : "") + item.nombre + (item.Hijos.Count > 0 ? "</strong>" : "");
            row.Controls.Add(column);
            tree.Controls.Add(row);


            HtmlGenericControl column_valor = new HtmlGenericControl("td");
            if (item.Hijos.Count == 0 && !(item.nombre == "Impuestos" && item.Padre.nombre == "INGRESOS") && !(item.nombre == "Accesorios Hs Extra 24% (1/12x2)" && item.Padre.nombre == "Costos Variables") && !(item.nombre == "Amortización" && item.Padre.nombre == "Costos Fijos No Erogables"))
            {
                HtmlInputText valor = new HtmlInputText();
                valor.ID = "valor_id_valor_" + vm.id;
                valor.Attributes.Add("class", "form-control");
                valor.Attributes.Add("onkeypress", "Modifica_valor(this, event)");
                valor.Value = vm.valor.ToString();
                column_valor.Controls.Add(valor);
            }
            else
            {
                Label valor = new Label();
                valor.Enabled = false;
                valor.ID = "valor_id_valor_" + vm.id;
                valor.Attributes.Add("class", "form-control");
                valor.Text = Cadena.Formato_moneda(vm.valor, Cadena.Moneda.pesos);
                column_valor.Controls.Add(valor);
            }

            row.Controls.Add(column_valor);

            foreach (Item_ingreso_egreso hijo in item.Hijos.OrderBy(x => x.id_item))
            {
                AgregarNodo(hijo, tree, cxt, io_equipo);
            }
        }

        [WebMethod]
        public static void ActualizarValor(int id, decimal valor)
        {
            using (var cxt = new Model1Container())
            {
                Valor_mes vm = cxt.Valores_meses.FirstOrDefault(ii => ii.id == id);
                if (vm != null)
                {
                    vm.valor = valor;
                }

                //controlar campos calculados

                //impuesto sobre los ingresos en blanco
                if (vm.Item.nombre == "Trabajado" && vm.Item.Padre.nombre == "INGRESOS")
                {
                    int id_item_impuesto = cxt.Items_ingresos_egresos.First(x => x.nombre == "Impuestos" && x.Padre.nombre == "INGRESOS").id_item;
                    Valor_mes vm_impuesto = cxt.Valores_meses.FirstOrDefault(ii => ii.id_ingreso_egreso_mensual == vm.id_ingreso_egreso_mensual && ii.id_item == id_item_impuesto);
                    vm_impuesto.valor = valor * (Convert.ToDecimal(-5) / Convert.ToDecimal(100));
                }

                //Accesorios hs extra
                if (vm.Item.nombre == "Horas Extra Chofer" && vm.Item.Padre.nombre == "Costos Variables")
                {
                    int id_item_impuesto = cxt.Items_ingresos_egresos.First(x => x.nombre == "Accesorios Hs Extra 24% (1/12x2)" && x.Padre.nombre == "Costos Variables").id_item;
                    Valor_mes vm_impuesto = cxt.Valores_meses.FirstOrDefault(ii => ii.id_ingreso_egreso_mensual == vm.id_ingreso_egreso_mensual && ii.id_item == id_item_impuesto);
                    vm_impuesto.valor = (valor / Convert.ToDecimal(12)) * Convert.ToDecimal(2);
                }

                cxt.SaveChanges();
            }
        }

        private void Recalcular_subtotales()
        {
            List<Item_ingreso_egreso> roots = new List<Item_ingreso_egreso>();

            using (var cxt = new Model1Container())
            {
                int id_equipo = Convert.ToInt32(ddl_equipo.SelectedItem.Value);
                int anio = Convert.ToInt32(ddl_anio.SelectedItem.Value);
                int mes = Convert.ToInt32(ddl_mes.SelectedItem.Value);
                Ingreso_egreso_mensual_equipo ioEquipo = cxt.Ingresos_egresos_mensuales_equipos.FirstOrDefault(iioo => iioo.id_equipo == id_equipo && iioo.mes == mes && iioo.anio == anio);
                if (ioEquipo != null)
                {
                    roots = cxt.Items_ingresos_egresos.Where(ii => ii.id_item_padre == null).ToList();
                    foreach (Item_ingreso_egreso item in roots)
                    {
                        Valor_mes vm = cxt.Valores_meses.First(ii => ii.id_item == item.id_item && ii.id_ingreso_egreso_mensual == ioEquipo.id_ingreso_egreso_mensual);
                        ObtenerTotal(vm);
                    }
                }

            }
        }

        private decimal ObtenerTotal(Valor_mes item_valor)
        {
            decimal ret = 0;

            //obtengo el valor del item seleccionado, si no tiene hijos devuelvo el valor, si tiene hijos lo llamo de manera recursiva

            using (var cxt = new Model1Container())
            {
                Item_ingreso_egreso item = cxt.Items_ingresos_egresos.First(ii => ii.id_item == item_valor.id_item);

                if (item.Hijos.Count > 0)
                {
                    Label input_x = ((Label)BuscarControl(div_tree, "valor_id_valor_" + item_valor.id));

                    decimal total_item_valor = 0;
                    foreach (Item_ingreso_egreso hijo in item.Hijos)
                    {
                        Valor_mes vm = cxt.Valores_meses.First(ii => ii.id_item == hijo.id_item && ii.id_ingreso_egreso_mensual == item_valor.id_ingreso_egreso_mensual);
                        total_item_valor = total_item_valor + ObtenerTotal(vm);
                    }

                    input_x.Text = Cadena.Formato_moneda(total_item_valor, Cadena.Moneda.pesos);
                    Valor_mes vm_cxt = cxt.Valores_meses.FirstOrDefault(vvmm => vvmm.id == item_valor.id);
                    vm_cxt.valor = total_item_valor;
                    cxt.SaveChanges();

                    ret = total_item_valor;
                }
                else
                {
                    ret = item_valor.valor;
                }
            }

            return ret;
        }

        private Control BuscarControl(Control rootControl, string controlID)
        {
            Control ret = null;
            HtmlGenericControl tree = ((HtmlGenericControl)div_tree.Controls[0]);
            foreach (HtmlGenericControl item in tree.Controls)
            {
                Control valor_fila = ((Control)((HtmlGenericControl)item.Controls[1]).Controls[0]);
                if (valor_fila.ID == controlID)
                {
                    ret = valor_fila;
                    break;
                }
            }

            return ret;
        }

        protected void btn_nueva_busqueda_Click(object sender, EventArgs e)
        {
            Estado_busqueda(true);
        }
    }
}