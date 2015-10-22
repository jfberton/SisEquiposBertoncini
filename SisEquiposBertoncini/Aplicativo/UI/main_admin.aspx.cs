using SisEquiposBertoncini.Aplicativo.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisEquiposBertoncini.Aplicativo.UI
{
    public partial class main_admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                Usuario usr = Session["UsuarioLogueado"] as Usuario;
                lbl_nombre_usuario.Text = usr.nombre;
            }
        }
    }
}