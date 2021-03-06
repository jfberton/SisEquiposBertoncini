﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="planilla_gastos_en _funcion_horas_hombre.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.planilla_gastos_en__funcion_horas_hombre" %>

<%@ Register Src="~/Aplicativo/Menues/menu_admin.ascx" TagPrefix="uc1" TagName="menu_admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
        <li>Gastos función horas hombre</li>
    </ol>
    <div id="div_print">
        <div class="row">
            <div class="col-md-12 text-center">
                <h3>
                    <label>Gastos en función de horas hombre de </label>
                    &nbsp;<asp:Label Text="" ID="lbl_tipo_empleado" runat="server" />&nbsp;-
                    <label>Mes </label>
                    &nbsp;<asp:Label Text="" ID="lbl_mes" runat="server" />&nbsp;
                    <label>Año </label>
                    &nbsp;<asp:Label Text="" ID="lbl_anio" runat="server" /></h3>
            </div>
        </div>
        <br />
        <div class="row" id="fila_edicion" runat="server">
            <div class="col-md-12">
                <table class="table table-condensed" style="width: 100%">
                    <tr>
                        <td>
                            <label>Insumos taller</label></td>
                        <td>
                            <asp:TextBox runat="server" CssClass="form-control" ID="tb_insumos_taller" /></td>
                        <td>
                            <label>Herramientas</label></td>
                        <td>
                            <asp:TextBox runat="server" CssClass="form-control" ID="tb_herramientas" /></td>
                        <td>
                            <label>Viaticos</label></td>
                        <td>
                            <asp:TextBox runat="server" CssClass="form-control" ID="tb_viaticos" /></td>
                        <td>
                            <label>Viaticos PP</label></td>
                        <td>
                            <asp:TextBox runat="server" CssClass="form-control" ID="tb_viaticos_presupuestados" /></td>

                    </tr>
                </table>
            </div>
            <div class="col-md-12">
                <div class="col-md-12">
                    <table class="table table-condensed" style="width: 100%">
                        <tr>
                            <td>
                                <label>Indumentaria</label></td>
                            <td>
                                <asp:TextBox runat="server" CssClass="form-control" ID="tb_indumentaria" /></td>
                            <td>
                                <label>Repuestos</label></td>
                            <td>
                                <asp:TextBox runat="server" CssClass="form-control" ID="tb_repuestos" /></td>
                            <td>
                                <label>Repuestos PP</label></td>
                            <td>
                                <asp:TextBox runat="server" CssClass="form-control" ID="tb_repuestos_pp" /></td>
                            <td>
                                <label>Gastos varios</label></td>
                            <td>
                                <asp:TextBox runat="server" CssClass="form-control" ID="tb_gastos_varios" /></td>
                            <td>
                                <label>Otros</label></td>
                            <td>
                                <asp:TextBox runat="server" CssClass="form-control" ID="tb_otros" /></td>
                        </tr>
                    </table>
                </div>
                <div class="col-md-12">
                    <button runat="server" id="btn_guardar_modificaciones" class="btn btn-default" onserverclick="btn_guardar_modificaciones_ServerClick"><span class="glyphicon glyphicon-save"></span>&nbsp;Aplicar</button>
                </div>
            </div>
        </div>

        <div class="row" id="fila_view" runat="server">
            <div class="col-md-12">
                <table class="table table-condensed" style="width: 100%">
                    <tr>
                        <td>
                            <label>Insumos taller</label></td>
                        <td>
                            <asp:Label Font-Bold="true" ID="lbl_insumos_taller" runat="server" /></td>
                        <td>
                            <label>Herramientas</label></td>
                        <td>
                            <asp:Label Font-Bold="true" ID="lbl_herramientas" runat="server" /></td>
                        <td>
                            <label>Viaticos</label></td>
                        <td>
                            <asp:Label Font-Bold="true" ID="lbl_viaticos" runat="server" /></td>
                        <td>
                            <label>Viaticos PP</label></td>
                        <td>
                            <asp:Label Font-Bold="true" ID="lbl_viaticos_pp" runat="server" /></td>

                    </tr>
                </table>
            </div>
            <div class="col-md-12">
                <table class="table table-condensed" style="width: 100%">
                    <tr>
                        <td>
                            <label>Indumentaria</label></td>
                        <td>
                            <asp:Label Font-Bold="true" ID="lbl_indumentaria" runat="server" /></td>
                        <td>
                            <label>Repuestos</label></td>
                        <td>
                            <asp:Label Font-Bold="true" ID="lbl_repuestos" runat="server" /></td>
                        <td>
                            <label>Repuestos PP</label></td>
                        <td>
                            <asp:Label Font-Bold="true" ID="lbl_repuestos_pp" runat="server" /></td>
                        <td>
                            <label>Gastos varios</label></td>
                        <td>
                            <asp:Label Font-Bold="true" ID="lbl_gastos_varios" runat="server" /></td>
                        <td>
                            <label>Otros</label></td>
                        <td>
                            <asp:Label Font-Bold="true" ID="lbl_otros" runat="server" /></td>
                    </tr>
                </table>
            </div>
        </div>

        <div class="row">
            <div runat="server" id="divTabla">
            </div>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-12 text-center">
            <button class="btn btn-default btn-lg" runat="server" id="btn_ver_planilla_principal" onserverclick="btn_ver_planilla_principal_ServerClick">
                <span class="glyphicon glyphicon-chevron-left"></span><span class="glyphicon glyphicon-chevron-left"></span>&nbsp;Planilla Principal
            </button>

            <button class="btn btn-default btn-lg" runat="server" id="btn_ver_planilla_calculos" onserverclick="btn_ver_planilla_calculos_ServerClick">
                <span class="glyphicon glyphicon-chevron-left"></span>&nbsp;Planilla de cálculos
            </button>

            <button class="btn btn-default btn-lg" onclick="Imprimir(); return false;">
                <span class="glyphicon glyphicon-print"></span>
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
