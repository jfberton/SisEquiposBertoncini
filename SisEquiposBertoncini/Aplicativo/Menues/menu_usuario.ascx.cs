using SisEquiposBertoncini.Aplicativo.Controles;
using SisEquiposBertoncini.Aplicativo.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisEquiposBertoncini.Aplicativo.Menues
{
    public partial class menu_usuario : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Usuario usu = (Usuario)Session["UsuarioLogueado"];
                CargarDatos(usu);
            }
        }

        private void CargarDatos(Usuario usu)
        {
            if (usu != null)
            {
                var cxt = new Model1Container();

                lbl_ApyNom.Text = usu.nombre;
                imagen_usuario.Usuario = usu;
            }
        }

        protected void lbl_CambiarClave_Click(object sender, EventArgs e)
        {
            Usuario emp = (Usuario)Session["UsuarioLogueado"];
            Response.Redirect("~/Aplicativo/usr_modifica_datos.aspx");
        }

        protected void lbl_logout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("~/default.aspx");
        }
    }
}