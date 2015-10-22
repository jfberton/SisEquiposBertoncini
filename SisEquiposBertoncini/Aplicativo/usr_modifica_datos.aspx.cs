using SisEquiposBertoncini.Aplicativo.Datos;
using SisEquiposBertoncini.Aplicativo.Seguridad;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisEquiposBertoncini.Aplicativo
{
    public partial class usr_modifica_datos : System.Web.UI.Page
    {

        private string pathImagenesDisco = System.Web.HttpRuntime.AppDomainAppPath + "img\\Usuarios\\";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Usuario usuariologueado = Session["UsuarioLogueado"] as Usuario;
                if (usuariologueado == null)
                {
                    Response.Redirect("~/Default.aspx?mode=session_end");
                }

                CargarDatosUsuario(usuariologueado);
            }
        }

        private void CargarDatosUsuario(Usuario usuariologueado)
        {
            using (var cxt = new Model1Container())
            {
                Usuario usr = cxt.Usuarios.FirstOrDefault(uu => uu.id == usuariologueado.id);
                imagen_usuario.Usuario = usr;
                lbl_usuario_nombre.Text = usr.nombre;
                lbl_usr.Text = usr.user;
                tb_usuario_nombre.Value = usr.nombre;
                tb_usuario_usr.Value = usr.user;
            }

        }

        protected void btn_aceptar_imagen_Click(object sender, EventArgs e)
        {
            Usuario usr = Session["UsuarioLogueado"] as Usuario;
            HttpPostedFile file = archivo_imagen.PostedFile;

            using (var cxt = new Model1Container())
            {
                Usuario usuario_cxt = cxt.Usuarios.First(aa => aa.id == usr.id);

                if (file != null && file.ContentLength > 0)
                {
                    if (file.ContentLength > 3145728)
                    {
                        MessageBox("El archivo a subir no debe superar los 3mb.");
                    }
                    else
                    {
                        string path_Destino = pathImagenesDisco + usuario_cxt.id.ToString() + "\\Original.jpg";

                        if (File.Exists(path_Destino))
                        {
                            File.Delete(path_Destino);
                        }

                        if (!Directory.Exists(pathImagenesDisco + usuario_cxt.id.ToString()))
                        {
                            Directory.CreateDirectory(pathImagenesDisco + usuario_cxt.id.ToString());
                        }

                        file.SaveAs(path_Destino);
                    }

                    imagen_usuario.Usuario = usuario_cxt;
                }
                else
                {
                    MessageBox("No hay seleccionada ninguna imagen");
                }
            }
        }

        private void MessageBox(string message)
        {
            string script = "<script language=\"javascript\"  type=\"text/javascript\">alert('" + message + "');</script>";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "AlertMessage", script, false);
        }

        protected void btn_aceptar_usuario_nombre_Click(object sender, EventArgs e)
        {
            Usuario usuariologueado = Session["UsuarioLogueado"] as Usuario;
            if (usuariologueado == null)
            {
                Response.Redirect("~/Default.aspx?mode=session_end");
            }

            using (var cxt = new Model1Container())
            {
                Usuario usr = cxt.Usuarios.FirstOrDefault(uu => uu.id == usuariologueado.id);
                usr.nombre = tb_usuario_nombre.Value;
                cxt.SaveChanges();

                CargarDatosUsuario(usr);
            }
        }

        protected void btn_aceptar_usuario_usr_Click(object sender, EventArgs e)
        {
            Usuario usuariologueado = Session["UsuarioLogueado"] as Usuario;
            if (usuariologueado == null)
            {
                Response.Redirect("~/Default.aspx?mode=session_end");
            }

            using (var cxt = new Model1Container())
            {
                Usuario usr = cxt.Usuarios.FirstOrDefault(uu => uu.id == usuariologueado.id);
                bool yaexisteusuarioporguardar = cxt.Usuarios.Count(uu => uu.id != usr.id && uu.user == tb_usuario_usr.Value) > 0;
                if (yaexisteusuarioporguardar)
                {
                    MessageBox("El usuario que esta intentando guardar ya fue tomado, por favor ingrese otro");
                }
                else
                {
                    usr.user = tb_usuario_usr.Value;
                    cxt.SaveChanges();
                    CargarDatosUsuario(usr);
                }
            }
        }

        protected void btn_aceptar_reseteo_de_clave_Click(object sender, EventArgs e)
        {
            Usuario usuariologueado = Session["UsuarioLogueado"] as Usuario;
            if (usuariologueado == null)
            {
                Response.Redirect("~/Default.aspx?mode=session_end");
            }

            using (var cxt = new Model1Container())
            {
                Usuario usr = cxt.Usuarios.FirstOrDefault(uu => uu.id == usuariologueado.id);
                if (tb_pass1.Value == tb_pass2.Value)
                {
                    usr.pass = Cripto.Encriptar(tb_pass1.Value);
                    cxt.SaveChanges();

                    CargarDatosUsuario(usr);
                }
                else
                {
                    MessageBox("Las claves no coinciden!");
                }
            }
        }

    }
}