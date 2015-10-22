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
    public partial class imagen_empleado : System.Web.UI.UserControl
    {
        private string pathImagenesDisco = System.Web.HttpRuntime.AppDomainAppPath + "img\\Empleados\\";

        protected void Page_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        Empleado empleado = null;

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

        public Empleado Empleado
        {
            set
            {
                Model1Container cxt = new Model1Container();
                empleado = cxt.Empleados.FirstOrDefault(a => a.id_empleado == value.id_empleado);
                CargarDatos();
            }
        }

        private void CargarDatos()
        {
            if (empleado != null)
            {
                if (Directory.Exists(pathImagenesDisco + empleado.id_empleado))
                {
                    if (File.Exists(pathImagenesDisco + empleado.id_empleado + "\\Original.jpg"))
                    {
                        img_cuenta.ImageUrl = "~/img/Empleados/" + empleado.id_empleado + "/Original.jpg";
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
            this.Empleado = empleado;
        }
    }
}