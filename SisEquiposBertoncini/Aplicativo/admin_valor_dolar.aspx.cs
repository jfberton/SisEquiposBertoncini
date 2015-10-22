using SisEquiposBertoncini.Aplicativo.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisEquiposBertoncini.Aplicativo
{
    public partial class admin_valor_dolar : System.Web.UI.Page
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

                CargarDDL();

                string aa = Request.QueryString["aa"];
                if (aa != null)
                {
                    int anio = Convert.ToInt32(aa);
                    ddl_anio.SelectedValue = aa;
                    CargarValoresDolar(anio);
                }
                else
                {
                    tabla_valores_dolar_anio.Visible = false;
                }
            }
        }

        private void CargarValoresDolar(int anio)
        {
            using (var cxt = new Model1Container())
            {
                var dolarMes = cxt.Valores_dolar.Where(ii => ii.anio == anio);
                for (int i = 0; i < 12; i++)
                {
                    Valor_dolar vd_mes = dolarMes.FirstOrDefault(dd => dd.mes == i + 1);

                    if (vd_mes == null)
                    {
                        vd_mes = new Valor_dolar() { mes = i + 1, anio = anio, valor = 0 };
                        cxt.Valores_dolar.Add(vd_mes);
                    }

                    TextBox tb = ((TextBox)EncontrarControl(tabla_valores_dolar_anio, "valor_mes_" + (i + 1).ToString()));
                    if (tb != null)
                    {
                        tb.Text = vd_mes.valor.ToString();
                    }

                }

                cxt.SaveChanges();
            }

            tabla_valores_dolar_anio.Visible = true;
        }

        private Control EncontrarControl(Control root, string id)
        {
            if (root.ID == id) return root;
            foreach (Control c in root.Controls)
            {
                Control t = EncontrarControl(c, id);
                if (t != null) return t;
            }
            return null;
        }

        private void CargarDDL()
        {
            for (int i = 2013; i <= DateTime.Today.Year; i++)
            {
                ddl_anio.Items.Add(new ListItem() { Text = i.ToString(), Value = i.ToString() });
            }
        }

        [WebMethod]
        public static void ActualizarValor(int mes, int anio, decimal valor)
        {
            using (var cxt = new Model1Container())
            {
                Valor_dolar vd_mes = cxt.Valores_dolar.FirstOrDefault(dd => dd.mes == mes && dd.anio == anio);
                vd_mes.valor = valor;
                cxt.SaveChanges();
            }
        }
    }
}