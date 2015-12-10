<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="planilla_gastos_en _funcion_horas_hombre.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.planilla_gastos_en__funcion_horas_hombre" %>

<%@ Register Src="~/Aplicativo/Menues/menu_admin.ascx" TagPrefix="uc1" TagName="menu_admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_menu" runat="server">
    <uc1:menu_admin runat="server" ID="menu_admin" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_body" runat="server">
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
    <div class="row">
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
                        <label>Viaticos presupuestados</label></td>
                    <td>
                        <asp:TextBox runat="server" CssClass="form-control" ID="tb_viaticos_presupuestados" /></td>
                    <td>
                        <button runat="server" id="btn_guardar_modificaciones" class="btn btn-default" onserverclick="btn_guardar_modificaciones_ServerClick"><span class="glyphicon glyphicon-save"></span>&nbsp;Aplicar</button></td>
                </tr>
            </table>
        </div>
    </div>
    <div class="row">
        <div runat="server" id="divTabla">
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
        </div>
    </div>
    <br />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_scripts" runat="server">
</asp:Content>
