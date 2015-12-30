<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="menu_admin.ascx.cs" Inherits="SisEquiposBertoncini.Aplicativo.Menues.menu_admin" %>
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
                <li><a href="../Aplicativo/admin_areas.aspx">Areas</a></li>
                <li class="dropdown">
                    <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">Empleados <span class="caret"></span></a>
                    <ul class="dropdown-menu">
                        <li><a href="../Aplicativo/admin_empleados.aspx">Empleados</a></li>
                        <li><a href="../Aplicativo/admin_tipo_empleado.aspx">Categorías</a></li>
                    </ul>
                </li>

                <li class="dropdown">
                    <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">Equipos <span class="caret"></span></a>
                    <ul class="dropdown-menu">
                        <li><a href="../Aplicativo/admin_equipos.aspx">Equipos</a></li>
                        <li><a href="../Aplicativo/admin_tipo_equipo.aspx">Categorias</a></li>
                    </ul>
                </li>

                <li class="dropdown">
                    <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">Ingresos - Egresos <span class="caret"></span></a>
                    <ul class="dropdown-menu">
                        <li class="dropdown-header">I/E por equipo</li>
                        <li><a href="../Aplicativo/admin_conceptos_mensuales.aspx">Editar conceptos</a></li>
                        <li><a href="../Aplicativo/admin_valores_mes_equipo_v2.aspx">Editar valores mensuales</a></li>
                        <li><a href="../Aplicativo/ver_valores_cargados_equipo.aspx">Resumen año equipo</a></li>
                        <li class="divider"></li>
                        <li class="dropdown-header">Administrar valores dolar</li>
                        <li><a href="../Aplicativo/admin_valor_dolar.aspx">Editar valores dolar</a></li>
                    </ul>
                </li>

                <li class="dropdown">
                    <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">Horas trabajadas<span class="caret"></span></a>
                    <ul class="dropdown-menu">
                        <li><a href="../Aplicativo/admin_feriados.aspx">Editar feriados</a></li>
                        <li><a href="../Aplicativo/admin_horas_mes.aspx">Cargar horas mes</a></li>
                        <li class="divider"></li>
                        <li class="dropdown-header">Planillas</li>
                        <li><a href="admin_horas_planilla_principal.aspx">Principal</a></li>
                        <li><a href="planilla_gastos_administrativos.aspx">Planilla gastos administ.</a></li>
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
