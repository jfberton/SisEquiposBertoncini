<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="planilla_calculos.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.planilla_calculos" %>

<%@ Register Src="~/Aplicativo/Menues/menu_admin.ascx" TagPrefix="uc1" TagName="menu_admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .rotar {
            /* Rotate div */
            writing-mode: tb-rl;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_menu" runat="server">
    <uc1:menu_admin runat="server" ID="menu_admin" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_body" runat="server">

    <ol class="breadcrumb">
        <li>Inicio</li>
        <li>Planillas</li>
        <li>En función de horas empleado</li>
        <li>Planilla de cálculos</li>
    </ol>

    <div id="div_print">
        <div class="row">
            <div class="col-md-12 text-center">
                <h3>
                    <label>Planilla de cálculos de </label>
                    &nbsp;<asp:Label Text="" ID="lbl_tipo_empleado" runat="server" />&nbsp;-
                    <label>Mes </label>
                    &nbsp;<asp:Label Text="" ID="lbl_mes" runat="server" />&nbsp;
                    <label>Año </label>
                    &nbsp;<asp:Label Text="" ID="lbl_anio" runat="server" /></h3>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-md-3">
                <table class="table">
                    <tr>
                        <td style="background-color: lightgray">
                            <label>Horas Totales</label></td>
                        <td style="background-color: whitesmoke">
                            <label>
                                <asp:Label Text="" ID="lbl_horas_totales" runat="server" /></label></td>
                    </tr>
                </table>
            </div>
            <div class="col-md-3">
                <table class="table">
                    <tr>
                        <td style="background-color: lightgray">
                            <label>Horas Normales</label></td>
                        <td style="background-color: whitesmoke">
                            <asp:Label Text="" ID="lbl_horas_normales" runat="server" /></td>
                    </tr>
                </table>
            </div>
            <div class="col-md-3">
                <table class="table">
                    <tr>
                        <td style="background-color: lightgray">
                            <label>Horas Extra 50%</label></td>
                        <td style="background-color: whitesmoke">
                            <asp:Label Text="" ID="lbl_horas_extra_50" runat="server" /></td>
                    </tr>
                </table>
            </div>
            <div class="col-md-3">
                <table class="table">
                    <tr>
                        <td style="background-color: lightgray">
                            <label>Horas Extra 100%</label></td>
                        <td style="background-color: whitesmoke">
                            <asp:Label Text="" ID="lbl_horas_extra_100" runat="server" /></td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <table>
                    <tr>
                        <td>Calcular en base al&nbsp;
                                <asp:DropDownList runat="server" ID="ddl_costo_hora" AutoPostBack="true" CausesValidation="false" OnSelectedIndexChanged="ddl_costo_hora_SelectedIndexChanged">
                                    <asp:ListItem Text="Costo hora teórico" />
                                    <asp:ListItem Text="Costo hora teórico ajustado" />
                                    <asp:ListItem Text="Costo hora real" />
                                </asp:DropDownList></td>
                        <td>&nbsp;<label><asp:Label Text="" ID="lbl_costo_hora_seleccionado" runat="server" /></label></td>
                    </tr>
                </table>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-md-12">
                <div id="div_tabla" runat="server"></div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <div id="div_tabla_OUT" runat="server"></div>
            </div>
            <div class="col-md-6">
                <div id="div_tabla_resumen" runat="server"></div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div id="div_tabla_resumen_ausentismo" runat="server"></div>
            </div>
        </div>
        <br />
    </div>
    <div class="row">
        <div class="col-md-12 text-center">
            <button class="btn btn-default btn-lg" runat="server" id="btn_ver_planilla_principal" onserverclick="btn_ver_planilla_principal_ServerClick">
                <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;Planilla principal
            </button>

             <button class="btn btn-default btn-lg" onclick="Imprimir(); return false;">
                <span class="glyphicon glyphicon-print"></span>
            </button>

            <button class="btn btn-default btn-lg" runat="server" id="btn_ver_planilla_gastos_horas_hombre" onserverclick="btn_ver_planilla_gastos_horas_hombre_ServerClick">
                Gastos funcion horas hombre&nbsp;<span class="glyphicon glyphicon-chevron-right"></span>
            </button>
        </div>
    </div>
    <br />
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="cph_scripts" runat="server">

    <script type="text/javascript">
        function Imprimir() {
            //var printContents = document.getElementById('div_print').innerHTML;
            //var originalContents = document.body.innerHTML;

            //document.body.innerHTML = printContents;

            //window.print();

            //document.body.innerHTML = originalContents;

            var content = "<html>";
            content += document.getElementById("div_print").innerHTML;
            content += "</body>";
            content += "</html>";

            var printWin = window.open('', '', 'left=0,top=0,width=800,height=600,toolbar=0,scrollbars=0,status =0');
            printWin.document.write(content);
            printWin.document.close();
            printWin.focus();
            printWin.print();
            printWin.close();
        }
    </script>

</asp:Content>
