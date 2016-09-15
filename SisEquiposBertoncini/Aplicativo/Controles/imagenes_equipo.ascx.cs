using SisEquiposBertoncini.Aplicativo.Datos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SisEquiposBertoncini.Aplicativo.Controles
{
    public partial class imagenes_equipo : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        private void CargarImagenesEquipo()
        {
            string path = pathImagenesDisco + equipo.id_equipo.ToString();
            imgs_equipo.Controls.Clear();

            if (Directory.Exists(path) && Directory.GetFiles(path).Count() > 0)
            {
                string[] imagenes = Directory.GetFiles(path);
                
                var nombres_imagenes = (from ss in imagenes
                                        where !ss.Contains("Original.jpg")
                                        select new
                                        {
                                            nombre_imagen = Path.GetFileName(ss)
                                        }).ToList();

                foreach (var imagen in nombres_imagenes)
                {
                    HtmlGenericControl li = new HtmlGenericControl("li");
                    li.Controls.Add(
                            new HtmlImage()
                            {
                                Src = "~/img/Equipos/" + equipo.id_equipo.ToString() + "/" + imagen.nombre_imagen,
                            });

                    imgs_equipo.Controls.Add(li);
                }

            }
            else
            {
                HtmlGenericControl li = new HtmlGenericControl("li");
                li.Controls.Add(
                    new HtmlImage()
                    {
                        Src = "~/img/Equipos/add.png",
                    });
                imgs_equipo.Controls.Add(li);
            }

        }

        private string pathImagenesDisco = HttpRuntime.AppDomainAppPath + "img\\Equipos\\";

        Equipo equipo = null;

        public Equipo Equipo
        {
            set
            {
                Model1Container cxt = new Model1Container();
                equipo = cxt.Equipos.FirstOrDefault(a => a.id_equipo == value.id_equipo);
                CargarImagenesEquipo();
            }
        }
    }
}