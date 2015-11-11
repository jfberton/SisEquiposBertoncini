using SisEquiposBertoncini.Aplicativo.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using datos = SisEquiposBertoncini.Aplicativo.Datos;
namespace SisEquiposBertoncini
{
    public partial class dispatcher : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Redireccionar();

        }

        private void Redireccionar()
        {
            Usuario user = Session["UsuarioLogueado"] as Usuario;

            switch (user.perfil)
            {
                case perfil_usuario.Admin:
                    Response.Redirect("~/Aplicativo/main_admin.aspx");
                    break;
                case perfil_usuario.Jefe:
                    break;
                case perfil_usuario.Supervisor:
                    break;
                case perfil_usuario.Usuario:
                    Response.Redirect("~/Aplicativo/main_usuario.aspx");
                    break;
                case perfil_usuario.Seleccionar:
                    break;
                default:
                    break;
            }
        }
    }
}