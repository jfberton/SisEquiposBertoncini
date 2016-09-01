<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="admin_importar_xls.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.admin_importar_xls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_menu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_body" runat="server">
    <asp:FileUpload ID="FileUpload1" runat="server" />
    <asp:Button ID="btn_leer" runat="server" OnClick="btn_leer_Click" Text="Importar" />
    <br />
    <asp:GridView ID="GridView1" runat="server">
    </asp:GridView>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_scripts" runat="server">
</asp:Content>
