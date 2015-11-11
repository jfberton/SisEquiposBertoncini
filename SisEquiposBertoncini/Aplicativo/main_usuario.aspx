<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="main_usuario.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.main_usuario" %>

<%@ Register Src="~/Aplicativo/Menues/menu_usuario.ascx" TagPrefix="uc1" TagName="menu_usuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_menu" runat="server">
    <uc1:menu_usuario runat="server" ID="menu_usuario" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_body" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_scripts" runat="server">
</asp:Content>
