using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisEquiposBertoncini
{
    public partial class error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ObtenerMostrarDatosError();
            }
        }

        private void ObtenerMostrarDatosError()
        {
            string id_error = Request.QueryString["id"];
            if (id_error != null && id_error != string.Empty)
            {
                ObtenerElError(id_error);
                fila_error.Visible = true;
            }
            else
            {
                fila_error.Visible = false;
            }
        }

        private struct MiError
        {
            public long nID { get; set; }
            public DateTime nDATE { get; set; }
            public string nAGENTE { get; set; }
            public string nMESSAGE { get; set; }
            public string nSOURCE { get; set; }
            public string nINSTANCE { get; set; }
            public string nDATA { get; set; }
            public string nURL { get; set; }
            public string nTARGETSITE { get; set; }
            public string nSTACKTRACE { get; set; }

        }

        private void ObtenerElError(string id_error)
        {
            string directorioRaiz = System.Web.HttpRuntime.AppDomainAppPath;
            StreamReader sr = new StreamReader(directorioRaiz + "Errores_Log.txt");

            List<string> listaErrores = new List<string>();
            List<MiError> listaMisErrores = new List<MiError>();
            string error = string.Empty;
            string linea = string.Empty;

            do
            {
                do
                {
                    linea = sr.ReadLine();
                    if (linea != null)
                    {
                        if (linea != "******************************************************************" && linea.TrimEnd().TrimStart().Length > 0)
                        {
                            if (linea.StartsWith("ERRORID") || linea.StartsWith("DATE") || linea.StartsWith("AGENTE") || linea.StartsWith("MESSAGE") || linea.StartsWith("SOURCE") || linea.StartsWith("INSTANCE") || linea.StartsWith("DATA") || linea.StartsWith("URL") || linea.StartsWith("TARGETSITE") || linea.StartsWith("STACKTRACE"))
                            {
                                error = error + linea + "|";
                            }
                            else
                            {
                                error = error + linea + ", ";
                            }
                            
                        }
                    }
                } while (linea != "******************************************************************" && linea != null && !sr.EndOfStream);
                //llego al final del documento o a la linea de asteriscos

                if (error != "" && error.StartsWith("ERRORID: " + id_error))
                {
                    var error_encontrado = new MiError()
                                            {
                                                nID = Convert.ToInt64(error.Split('|')[0].Replace("ERRORID:", "")),
                                                nDATE = Convert.ToDateTime(error.Split('|')[1].Replace("DATE:", "")),
                                                nAGENTE = error.Split('|')[2].Replace("AGENTE:", ""),
                                                nMESSAGE = error.Split('|')[3].Replace("MESSAGE:", ""),
                                                nSOURCE = error.Split('|')[4].Replace("SOURCE:", ""),
                                                nINSTANCE = error.Split('|')[5].Replace("INSTANCE:", ""),
                                                nDATA = error.Split('|')[6].Replace("DATA:", ""),
                                                nURL = error.Split('|')[7].Replace("URL:", ""),
                                                nTARGETSITE = error.Split('|')[8].Replace("TARGETSITE:", ""),
                                                nSTACKTRACE = error.Split('|')[9].Replace("STACKTRACE:", ""),
                                            };

                    lbl_numero_error.Text = error_encontrado.nID.ToString();
                    lbl_Data.Text = error_encontrado.nDATA;
                    lbl_Instance.Text = error_encontrado.nINSTANCE;
                    lbl_Message.Text = error_encontrado.nMESSAGE;
                    lbl_Source.Text = error_encontrado.nSOURCE;
                    lbl_StackTrace.Text = error_encontrado.nSTACKTRACE;
                    lbl_TargetSite.Text = error_encontrado.nTARGETSITE;
                    lbl_URL.Text = error_encontrado.nURL;

                    sr.Close();
                    break;
                }
                else
                {
                    error = string.Empty;
                    linea = string.Empty;
                }

            } while (!sr.EndOfStream);

            sr.Close();
        }

        protected void btn_VolverAEmpezar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/default.aspx");
        }
    }
}