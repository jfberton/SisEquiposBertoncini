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
                    <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">Ingresos - Egresos<span class="caret"></span></a>
                    <ul class="dropdown-menu">
                        <li><a href="../Aplicativo/admin_conceptos_mensuales.aspx">Conceptos I/E</a></li>
                        <li class="dropdown-header">I/E por equipo</li>
                        <li><a href="../Aplicativo/admin_valores_mes_equipo_v2.aspx">Valores mensuales</a></li>
                        <li><a href="../Aplicativo/ver_valores_cargados_equipo.aspx">Resumen año equipo</a></li>
                        <li class="divider"></li>
                        <li class="dropdown-header">I/E por categoria</li>
                        <li><a href="../Aplicativo/admin_valores_mes_categoria.aspx">Valores mensuales</a></li>
                        <li><a href="../Aplicativo/ver_valores_cargados_categoria.aspx">Resumen año categoria</a></li>
                        <li><a href="planilla_resumen_valores_equipo_categoria.aspx">Resumen año equipos categoria </a></li>
                        <li class="divider"></li>
                        <li class="dropdown-header">Administrar valores dolar</li>
                        <li><a href="../Aplicativo/admin_valor_dolar.aspx">Editar valores dolar</a></li>
                    </ul>
                </li>

                <li class="dropdown">
                    <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">Horas trabajadas<span class="caret"></span></a>
                    <ul class="dropdown-menu">
                        <li><a href="../Aplicativo/admin_feriados.aspx">Feriados</a></li>
                        <li><a href="../Aplicativo/admin_horas_mes.aspx">Horas mes</a></li>
                    </ul>
                </li>

                <li class="dropdown">
                    <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">Planillas<span class="caret"></span></a>
                    <ul class="dropdown-menu">
                        <li><a href="admin_horas_planilla_principal.aspx">En función de horas empleado</a></li>
                        <li><a href="planilla_gastos_administrativos.aspx">Gastos administración</a></li>
                        <li><a href="planilla_resumen_horas_trabajadas.aspx">Resumen horas trabajadas equipo</a></li>
                        <%--<li><a href="planilla_combustibles.aspx">Planilla combustibles</a></li>--%>
                        
                    </ul>
                </li>
                <li class="dropdown">
                    <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">Importar<span class="caret"></span></a>
                    <ul class="dropdown-menu">
                        <li><a href="admin_importar_blanco.aspx">Blancos sistema gestión</a></li>
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

