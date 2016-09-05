using SisEquiposBertoncini.Aplicativo.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SisEquiposBertoncini.Aplicativo.Menues
{
    public partial class menu_admin : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["ultimo_activo"] = "";
                //Usuario usu = (Usuario)Session["UsuarioLogueado"];
                //CargarDatos(usu);
            }
        }

        private void CargarDatos(Usuario usu)
        {
            if (usu != null)
            {
                var cxt = new Model1Container();

                //lbl_ApyNom.Text = usu.nombre;
                //imagen_usuario.Usuario = usu;
            }
        }

        public void Activar_Li(string nombreLI)
        {

            string[] controles_anidados = nombreLI.Split('0');

            if (controles_anidados.Count() > 1)
            {
                Control menu = FindControl(controles_anidados[0]);
                ((HtmlGenericControl)menu).Attributes.Clear();
                ((HtmlGenericControl)menu).Attributes.Add("class", "treeview active");

                Control contenedor = FindControl(controles_anidados[0].Replace("li", "ul"));
                ((HtmlGenericControl)contenedor).Attributes.Clear();
                ((HtmlGenericControl)contenedor).Attributes.Add("class", "treeview-menu menu-open");
                ((HtmlGenericControl)contenedor).Attributes.Add("style", "display: block;");

                //nombreLI = controles_anidados[1];
            }
            else
            {
                Control li = FindControl(nombreLI);
                if (li != null)
                {

                    if (Session["ultimo_activo"] != null)
                    {
                        string ultimo_activo = Session["ultimo_activo"].ToString();
                        Control ultimo_control_activo = FindControl(ultimo_activo);
                        if (ultimo_control_activo != null)
                        {
                            ((HtmlGenericControl)ultimo_control_activo).Attributes.Clear();
                        }
                    }

                    ((HtmlGenericControl)li).Attributes.Clear();
                    ((HtmlGenericControl)li).Attributes.Add("class", "active");
                    Session["ultimo_activo"] = nombreLI;
                }
            }
        }

        protected void lbl_CambiarClave_Click(object sender, EventArgs e)
        {
            Usuario emp = (Usuario )Session["UsuarioLogueado"];
            Response.Redirect("~/Aplicativo/usr_modifica_datos.aspx");
        }

        protected void lbl_logout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("~/default.aspx");
        }

    }
}