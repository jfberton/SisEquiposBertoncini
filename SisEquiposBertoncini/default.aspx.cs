using System;
using SisEquiposBertoncini.Aplicativo.Controles;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SisEquiposBertoncini.Aplicativo.Datos;
using SisEquiposBertoncini.Aplicativo.Seguridad;
using System.Web.Security;

namespace SisEquiposBertoncini
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session.Clear();

                switch (Request.QueryString["mode"])
                {
                    case "session_end":
                        MessageBox.Show(this, "Su sesión ha caducado", MessageBox.Tipo_MessageBox.Default, "Ups!");
                        break;
                    case "trucho":
                        Response.Write("<script>" +
                          "alert('Su perfil tiene definidos los permisos para ingresar en ese directorio.');" +
                          "location.href='Default.aspx';" + "</script>");
                        break;
                    default:
                        Session["CXT"] = new Model1Container();
                        Session["UsuarioLogueado"] = null;
                        inputUsr.Focus();
                        break;
                }
            }
        }

        protected void btn_ingresar_Click(object sender, EventArgs e)
        {
            using (var cxt = new Model1Container())
            {
                if (cxt.Usuarios.Count() == 0)
                {
                    InicializarDB();
                }

                string clave = Cripto.Encriptar(inputPassword.Value);
                Usuario usr = cxt.Usuarios.FirstOrDefault(uu => uu.user == inputUsr.Value && uu.pass == clave);
                if (usr != null)
                {
                    Session["UsuarioLogueado"] = usr;
                    FormsAuthentication.RedirectFromLoginPage(usr.nombre, false);
                }
                else
                {
                    MessageBox.Show(this, "Usuario o contraseña incorrecta!", MessageBox.Tipo_MessageBox.Danger, "Datos de acceso incorrectos");
                }

            }
        }

        private void InicializarDB()
        {
            using (var cxt = new Model1Container())
            {
                string clave = Cripto.Encriptar("123456");

                Usuario usr = new Usuario()
                {
                    nombre = "José Federico Bertoncini",
                    user = "berton",
                    pass = clave,
                    perfil = perfil_usuario.Admin
                };

                cxt.Usuarios.Add(usr);

                cxt.Areas.Add(new Area() { nombre = "Taller" });

                

                cxt.Categorias_empleados.Add(new Categoria_empleado() { nombre = "Administrativo", descripcion = "" });
                cxt.Categorias_empleados.Add(new Categoria_empleado() { nombre = "Mecánico", descripcion = "" });
                cxt.Categorias_empleados.Add(new Categoria_empleado() { nombre = "Soldador", descripcion = "" });
                cxt.Categorias_empleados.Add(new Categoria_empleado() { nombre = "Pintor", descripcion = "" });

                
                cxt.Categorias_equipos.Add(new Categoria_equipo() { nombre = "Gruas", descripcion = "" });
                cxt.Categorias_equipos.Add(new Categoria_equipo() { nombre = "Camiones y carretones", descripcion = "" });
                cxt.Categorias_equipos.Add(new Categoria_equipo() { nombre = "Vehículos menores", descripcion = "" });
                cxt.Categorias_equipos.Add(new Categoria_equipo() { nombre = "Ventas", descripcion = "Equipos dedicados a la venta" });
                cxt.Categorias_equipos.Add(new Categoria_equipo() { nombre = "Trabajos particulares", descripcion = "Agrupa trabajos particulares" });
                Categoria_equipo ce = new Categoria_equipo() { nombre = "Otros", descripcion = "Agrupa otros equipos - conceptos" };
                cxt.Categorias_equipos.Add(ce);
                cxt.SaveChanges();

                cxt.Equipos.Add(new Equipo() { Categoria = ce, Generico = true, nombre = "Ausencia", fecha_baja = null, OUT = false, notas = "" });
                cxt.Equipos.Add(new Equipo() { Categoria = ce, Generico = true, nombre = "Guardia", fecha_baja = null, OUT = false, notas = "" });
                cxt.Equipos.Add(new Equipo() { Categoria = ce, Generico = true, nombre = "Varios Taller", fecha_baja = null, OUT = false, notas = "" });
                cxt.SaveChanges();
            }
        }
    }
}