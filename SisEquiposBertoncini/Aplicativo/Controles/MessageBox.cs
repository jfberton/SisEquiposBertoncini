using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace SisEquiposBertoncini.Aplicativo.Controles
{
    public static class MessageBox
    {
        public enum Tipo_MessageBox
        {
            Success,
            Default,
            Warning,
            Danger,
            Info,
            Primary
        }

        public static void Show(Page pagina, string mensaje, Tipo_MessageBox tipo = Tipo_MessageBox.Default, string titulo = "Atención", string redireccion = "")
        {
            StringBuilder modal = new StringBuilder();
            modal.AppendLine("<div class=\"modal fade\" id=\"messagebox\" role=\"dialog\" aria-hidden=\"true\">");
            modal.AppendLine("<div class=\"modal-dialog\">");

            switch (tipo)
            {
                case Tipo_MessageBox.Success:
                    modal.AppendLine("<div class=\"modal-content panel-success\">");
                    modal.AppendLine("<div class=\"modal-header panel-heading\">");
                    modal.AppendLine("<button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>");
                    modal.AppendLine("<h4 class=\"alert-success\"><span class=\"glyphicon glyphicon-ok-sign\" aria-hidden=\"true\"></span> " + titulo + "</h4>");
                    modal.AppendLine("</div>");
                    break;
                case Tipo_MessageBox.Default:
                    modal.AppendLine("<div class=\"modal-content panel-default\">");
                    modal.AppendLine("<div class=\"modal-header panel-heading\">");
                    modal.AppendLine("<button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>");
                    modal.AppendLine("<h4 class=\"modal-title\">" + titulo + "</h4>");
                    modal.AppendLine("</div>");
                    break;
                case Tipo_MessageBox.Warning:
                    modal.AppendLine("<div class=\"modal-content panel-warning\">");
                    modal.AppendLine("<div class=\"modal-header panel-heading\">");
                    modal.AppendLine("<button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>");
                    modal.AppendLine("<h4 class=\"alert-warning\"><span class=\"glyphicon glyphicon-exclamation-sign\" aria-hidden=\"true\"></span> " + titulo + "</h4>");
                    modal.AppendLine("</div>");
                    break;
                case Tipo_MessageBox.Danger:
                    modal.AppendLine("<div class=\"modal-content panel-danger\">");
                    modal.AppendLine("<div class=\"modal-header panel-heading\">");
                    modal.AppendLine("<button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>");
                    modal.AppendLine("<h4 class=\"alert-danger\"><span class=\"glyphicon glyphicon-remove-sign\" aria-hidden=\"true\"></span> " + titulo + "</h4>");
                    modal.AppendLine("</div>");
                    break;
                case Tipo_MessageBox.Info:
                    modal.AppendLine("<div class=\"modal-content panel-info\">");
                    modal.AppendLine("<div class=\"modal-header panel-heading\">");
                    modal.AppendLine("<button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>");
                    modal.AppendLine("<h4 class=\"alert-info\"><span class=\"glyphicon glyphicon-info-sign\" aria-hidden=\"true\"></span> " + titulo + "</h4>");
                    modal.AppendLine("</div>");
                    break;
                case Tipo_MessageBox.Primary:
                    modal.AppendLine("<div class=\"modal-content panel-primary\">");
                    modal.AppendLine("<div class=\"modal-header panel-heading\">");
                    modal.AppendLine("<button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>");
                    modal.AppendLine("<h4 class=\"modal-title\"><span class=\"label label-primary\"><span class=\"glyphicon glyphicon-comment\" aria-hidden=\"true\"></span> " + titulo + "</span></h4>");
                    modal.AppendLine("</div>");
                    break;
                default:
                    break;
            }

            modal.AppendLine("<div class=\"modal-body\">");
            modal.AppendLine("<div class=\"row\">");
            modal.AppendLine("<div class=\"col-md-12\">");
            modal.AppendLine("<p>");
            modal.AppendLine(mensaje);
            modal.AppendLine("</p>");
            modal.AppendLine("</div>");
            modal.AppendLine("</div>");
            modal.AppendLine("</div>");
            modal.AppendLine("</div>");
            modal.AppendLine("</div>");

            string script = string.Empty;

            if (redireccion != "")
            {
                script = modal.ToString() + "<script language=\"javascript\"  type=\"text/javascript\">$(document).ready(function () {$('#messagebox').modal('show');$('#messagebox').on('hidden.bs.modal', function (e) {window.location = '" + redireccion + "';})});</script>";
            }
            else
            { 
                script = modal.ToString() + "<script language=\"javascript\"  type=\"text/javascript\">$(document).ready(function() { $('#messagebox').modal('show')});</script>";
            }

            ScriptManager.RegisterStartupScript(pagina, pagina.GetType(), "ShowMessageBox", script, false);

        }

    }
}