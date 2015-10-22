<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="imagen_usuario.ascx.cs" Inherits="SisEquiposBertoncini.Aplicativo.Controles.imagen_usuario" %>

<div style="position: relative">
    <asp:Image ID="img_cuenta" ImageUrl="" BackColor="GrayText" Width="140" Height="140"
        runat="server" />
    <div style="position: absolute; top: 0; left: 0;">
        <asp:Image ID="img_acceso" ImageUrl="" Width="140" Height="140"
            runat="server" />
    </div>
    <div style="position: absolute; top: 0; left: 0;">
        <asp:Image ID="img_cumple" ImageUrl="" Width="140" Height="140"
            runat="server" />
    </div>
</div>
