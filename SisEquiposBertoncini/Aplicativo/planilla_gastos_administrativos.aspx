<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="planilla_gastos_administrativos.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.planilla_gastos_administrativos" %>

<%@ Register Src="~/Aplicativo/Menues/menu_admin.ascx" TagPrefix="uc1" TagName="menu_admin" %>
<%@ Register Src="~/Aplicativo/Controles/valor_decimal.ascx" TagPrefix="uc1" TagName="valor_decimal" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_menu" runat="server">
    <uc1:menu_admin runat="server" ID="menu_admin" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_body" runat="server">
    <h3>Planilla de gastos administrativos</h3><br />
    <table class="table table-bordered">
        <tr>
            <td>Mes</td>
            <td>
                <asp:DropDownList runat="server" ID="ddl_mes" AutoPostBack="true" CssClass="form-control" Width="100%">
                    <asp:ListItem Text="Enero" Value="1" />
                    <asp:ListItem Text="Febrero" Value="2" />
                    <asp:ListItem Text="Marzo" Value="3" />
                    <asp:ListItem Text="Abril" Value="4" />
                    <asp:ListItem Text="Mayo" Value="5" />
                    <asp:ListItem Text="Junio" Value="6" />
                    <asp:ListItem Text="Julio" Value="7" />
                    <asp:ListItem Text="Agosto" Value="8" />
                    <asp:ListItem Text="Septiembre" Value="9" />
                    <asp:ListItem Text="Octubre" Value="10" />
                    <asp:ListItem Text="Noviembre" Value="11" />
                    <asp:ListItem Text="Diciembre" Value="12" />
                </asp:DropDownList></td>
            <td>Año</td>
            <td>
                <asp:DropDownList runat="server" ID="ddl_anio" AutoPostBack="true" CssClass="form-control" Width="100%">
                </asp:DropDownList></td>
        </tr>
    </table>
    <h4><u>Montos por distribuir</u></h4>
    <table class="table table-bordered">
       
        <tr>
            <td>Telefonía celular</td>
            <td>
                <asp:TextBox runat="server" ID="tb_telefonia_celular" CssClass="form-control" /></td>
            <td>Sueldos administración</td>
            <td>
                <asp:TextBox runat="server" ID="tb_sueldos_administracion" CssClass="form-control" /></td>
        </tr>
        <tr>
            <td>Honorarios de sistema</td>
            <td>
                <asp:TextBox runat="server" ID="tb_honorarios_sistema" CssClass="form-control" /></td>
            <td>Honorarios contable</td>
            <td>
                <asp:TextBox runat="server" ID="tb_honorarios_contables" CssClass="form-control" /></td>
        </tr>
        <tr>
            <td>Papelería - Librería</td>
            <td>
                <asp:TextBox runat="server" ID="tb_papeleria_libreria" CssClass="form-control" /></td>
            <td>Otros</td>
            <td>
                <asp:TextBox runat="server" ID="tb_otros" CssClass="form-control" /></td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:Button Text="Aplicar" ID="btn_aplicar_cambios" CssClass="btn btn-default" OnClick="btn_aplicar_cambios_Click" runat="server" /></td>
        </tr>
    </table>
    <div class="row">
        <div class="col-md-12">
            <div runat="server" id="div_detalle">

            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_scripts" runat="server">
</asp:Content>
