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
    public partial class imagen_equipo : System.Web.UI.UserControl
    {
        private string pathImagenesDisco = System.Web.HttpRuntime.AppDomainAppPath + "img\\Equipos\\";

        protected void Page_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        Equipo equipo = null;

        public int Height
        {
            set
            {
                img_acceso.Height = value;
                img_cuenta.Height = value;
            }
        }

        public int Width
        {
            set
            {
                img_acceso.Width = value;
                img_cuenta.Width = value;
            }
        }

        public Equipo Equipo
        {
            set
            {
                Model1Container cxt = new Model1Container();
                equipo = cxt.Equipos.FirstOrDefault(a => a.id_equipo == value.id_equipo);
                CargarDatos();
            }
        }

        private void CargarDatos()
        {
            if (equipo != null)
            {
                if (Directory.Exists(pathImagenesDisco + equipo.id_equipo))
                {
                    if (File.Exists(pathImagenesDisco + equipo.id_equipo + "\\Original.jpg"))
                    {
                        img_cuenta.ImageUrl = "~/img/Equipos/" + equipo.id_equipo + "/Original.jpg";
                    }
                    else
                    {
                        img_cuenta.ImageUrl = "~/img/Equipos/add.png";
                    }
                }
                else
                {
                    img_cuenta.ImageUrl = "~/img/Equipos/add.png";
                }
            }
        }

        public void Refrescar()
        {
            this.Equipo = equipo;
        }
    }
}