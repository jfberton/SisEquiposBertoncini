using SisEquiposBertoncini.Aplicativo.Datos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisEquiposBertoncini
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        private string pathImagenesDisco = System.Web.HttpRuntime.AppDomainAppPath + "img\\Usuarios\\";

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                Usuario usuario = (Usuario)Session["UsuarioLogueado"];

                if (usuario != null)
                {
                    lbl_usuario.Text = usuario.nombre;
                    if (Directory.Exists(pathImagenesDisco + usuario.id))
                    {
                        if (File.Exists(pathImagenesDisco + usuario.id + "\\Original.jpg"))
                        {
                            img_cuenta.Src = "~/img/Usuarios/" + usuario.id + "/Original.jpg";
                        }
                        else
                        {
                            img_cuenta.Src = "~/img/UsrDefault.jpg";
                        }
                    }
                    else
                    {
                        img_cuenta.Src = "~/img/UsrDefault.jpg";
                    }

                }
            }
        }

    }
}