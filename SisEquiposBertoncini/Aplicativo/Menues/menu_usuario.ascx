<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="menu_usuario.ascx.cs" Inherits="SisEquiposBertoncini.Aplicativo.Menues.menu_usuario" %>
<%@ Register Src="~/Aplicativo/Controles/imagen_usuario.ascx" TagPrefix="uc1" TagName="imagen_usuario" %>

<nav class="navbar navbar-default navbar-fixed-top">
    <div class="container">
        <div class="navbar-header">
            <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1" aria-expanded="false">
                <span class="sr-only">Toggle navigation</span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
            </button>
            <a class="navbar-brand" href="#">Sistema integral</a>
        </div>

        <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
            <ul class="nav navbar-nav">

                <li class="dropdown">
                    <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">Horas trabajadas<span class="caret"></span></a>
                    <ul class="dropdown-menu">
                        <li><a href="../Aplicativo/admin_feriados.aspx">Editar feriados</a></li>
                        <li><a href="../Aplicativo/admin_horas_mes.aspx">Cargar horas mes</a></li>
                    </ul>
                </li>

            </ul>
            <ul class="nav navbar-nav navbar-right">
                <li class="dropdown">
                    <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false" onclick="actualizarFechaHora();">
                        <span class="glyphicon glyphicon-user" data-toggle="tooltip" data-placement="left" title="Administrar cuenta"></span>
                        <span class="caret"></span>
                    </a>
                    <ul class="dropdown-menu" role="menu">
                        <li class="dropdown-header">
                            <asp:Label Text="" ID="lbl_fecha_hora" runat="server" /></li>
                        <li class="divider"></li>
                        <li class="dropdown-header">
                            <asp:Label Text="" ID="lbl_ApyNom" runat="server" />
                            <uc1:imagen_usuario runat="server" ID="imagen_usuario" />
                        </li>
                        <li>
                            <asp:LinkButton Text="Editar datos" ID="lbl_CambiarClave" CausesValidation="false" OnClick="lbl_CambiarClave_Click" runat="server" />
                        </li>
                        <li class="divider"></li>
                        <li>
                            <asp:LinkButton Text="Cerrar sesión" ID="lbl_logout" CausesValidation="false" OnClick="lbl_logout_Click" runat="server" /></li>
                    </ul>
                </li>
            </ul>
        </div>
    </div>
</nav>
