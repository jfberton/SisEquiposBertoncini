using SisEquiposBertoncini.Aplicativo.Seguridad;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace SisEquiposBertoncini
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            DateTime fechahoraerror = DateTime.Now;
            if (Server.GetLastError() != null)
            {
                // At this point we have information about the error
                HttpContext ctx = HttpContext.Current;
                Exception ex = ctx.Server.GetLastError().GetBaseException();
                string querystring = string.Empty;

                //hago esta consulta primero porque siempre me tira un error de que no encuentra
                //una imagen aparentemente pero no puedo determinar que es asi que lo obvio nomas
                if (!(ex.Message == "File does not exist." && ex.Source == "System.Web" && ex.InnerException == null))
                {
                    //guardo en el log.
                    string directorioRaiz = System.Web.HttpRuntime.AppDomainAppPath;
                    
                    StreamWriter sw;
                   
                    sw = File.AppendText(directorioRaiz + "Errores_Log.txt");

                    sw.WriteLine(" \n");
                    sw.WriteLine("******************************************************************");

                    sw.WriteLine("\nERRORID: " + fechahoraerror.Ticks.ToString());
                    sw.WriteLine("\nDATE: " + fechahoraerror);

                    if (ctx.User != null && ctx.User.Identity.IsAuthenticated)
                    {
                        sw.WriteLine("\nAGENTE: " + ctx.User.Identity.Name.Replace("Bienvenido ", "").Replace("|", ""));
                    }

                    sw.WriteLine("\nMESSAGE: " + ex.Message);

                    sw.WriteLine("\nSOURCE: " + ex.Source);
                    sw.WriteLine("\nINSTANCE: " + ex.InnerException);

                    sw.WriteLine("\nDATA: " + ex.Data);

                    sw.WriteLine("\nURL: " + ctx.Request.Url.ToString());

                    sw.WriteLine("\nTARGETSITE: " + ex.TargetSite);

                    sw.WriteLine("\nSTACKTRACE: " + ex.StackTrace + "\n");
                    sw.WriteLine("\n******************************************************************");
                    sw.Close();

                }

                ctx.Server.ClearError();

                Response.Redirect("~/Error.aspx?id=" + fechahoraerror.Ticks.ToString());
            }
        }
    }
}