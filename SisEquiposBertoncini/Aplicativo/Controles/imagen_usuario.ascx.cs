using SisEquiposBertoncini.Aplicativo.Datos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisEquiposBertoncini.Aplicativo.Controles
{
    public partial class imagen_usuario : System.Web.UI.UserControl
    {
        private string pathImagenesDisco = System.Web.HttpRuntime.AppDomainAppPath + "img\\Usuarios\\";

        protected void Page_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        Usuario usuario = null;

        public int Height
        {
            set
            {
                img_acceso.Height = value;
                img_cuenta.Height = value;
                img_cumple.Height = value;
            }
        }

        public int Width
        {
            set
            {
                img_acceso.Width = value;
                img_cuenta.Width = value;
                img_cumple.Width = value;
            }
        }

        public Usuario Usuario
        {
            set
            {
                Model1Container cxt = new Model1Container();
                usuario = cxt.Usuarios.FirstOrDefault(a => a.id == value.id);
                CargarDatos();
            }
        }

        private void CargarDatos()
        {
            if (usuario != null)
            {
                if (Directory.Exists(pathImagenesDisco + usuario.id))
                {
                    if (File.Exists(pathImagenesDisco + usuario.id + "\\Original.jpg"))
                    {
                        img_cuenta.ImageUrl = "~/img/Usuarios/" + usuario.id + "/Original.jpg";
                    }
                    else
                    {
                        img_cuenta.ImageUrl = "~/img/UsrDefault.jpg";
                    }
                }
                else
                {
                    img_cuenta.ImageUrl = "~/img/UsrDefault.jpg";
                }

            }
        }

        public void Refrescar()
        {
            this.Usuario = usuario;
        }
    }
}