<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="main_sistema.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.main_sistema" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_menu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_body" runat="server">
    <h1>Hola Administrador de sistema!</h1>
    <h2>Actualizar las horas cargadas a loa agentes <p><small><b>Procesos_globales.Actualizar_horas_todos_los_resumenes_mensuales()</b><br />Recupera todos los resumenes mensuales de todos los empleados que esten cargados y verifica si es soldador y actualiza las horas cargadas ya que los mismos no consideran ni guardia ni trabajos OUT</small></p></h2>
    <asp:Button Text="Actuizar Horas" ID="btn_actualiza_horas" OnClick="btn_actualiza_horas_Click" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_scripts" runat="server">
</asp:Content>
