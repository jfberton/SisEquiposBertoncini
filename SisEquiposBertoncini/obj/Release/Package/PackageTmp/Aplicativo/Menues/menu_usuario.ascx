<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="menu_usuario.ascx.cs" Inherits="SisEquiposBertoncini.Aplicativo.Menues.menu_usuario" %>
<%@ Register Src="~/Aplicativo/Controles/imagen_usuario.ascx" TagPrefix="uc1" TagName="imagen_usuario" %>

<li runat="server" id="li_areas"><a href="../Aplicativo/admin_areas.aspx"><i class="fa fa-link"></i><span>Areas</span></a></li>

<li runat="server" id="li_empleados" class="treeview">
    <a href="#"><i class="fa fa-link"></i><span>Empleados</span>
        <span class="pull-right-container">
            <i class="fa fa-angle-left pull-right"></i>
        </span>
    </a>
    <ul runat="server" id="ul_empleados" class="treeview-menu">
        <li runat="server" id="li_empleados0ul_empleados"><a href="../Aplicativo/admin_empleados.aspx">Empleados</a></li>
        <li runat="server" id="li_empleados0ul_categorias"><a href="../Aplicativo/admin_tipo_empleado.aspx">Categorías</a></li>
    </ul>
</li>


<li runat="server" id="li_horas" class="treeview">
    <a href="#"><i class="fa fa-link"></i><span>Horas trabajadas</span>
        <span class="pull-right-container">
            <i class="fa fa-angle-left pull-right"></i>
        </span>
    </a>
    <ul runat="server" id="ul_horas" class="treeview-menu">
        <li runat="server" id="li_horas0ul_feriados"><a href="../Aplicativo/admin_feriados.aspx">Feriados</a></li>
        <li runat="server" id="li_horas0ul_horas"><a href="../Aplicativo/admin_horas_mes.aspx">Horas mes</a></li>
    </ul>
</li>
