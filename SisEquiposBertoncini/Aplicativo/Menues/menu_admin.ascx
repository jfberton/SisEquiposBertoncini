<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="menu_admin.ascx.cs" Inherits="SisEquiposBertoncini.Aplicativo.Menues.menu_admin" %>

<li runat="server" id="li_areas"><a href="../Aplicativo/admin_areas.aspx"><i class="fa fa-sitemap"></i><span>Areas</span></a></li>

<li runat="server" id="li_empleados" class="treeview">
    <a href="#"><i class="fa fa-group"></i><span>Empleados</span>
        <span class="pull-right-container">
            <i class="fa fa-angle-left pull-right"></i>
        </span>
    </a>
    <ul runat="server" id="ul_empleados" class="treeview-menu">
        <li runat="server" id="li_empleados0ul_empleados"><a href="../Aplicativo/admin_empleados.aspx">Empleados</a></li>
        <li runat="server" id="li_empleados0ul_categorias"><a href="../Aplicativo/admin_tipo_empleado.aspx">Categorías</a></li>
    </ul>
</li>

<li runat="server" id="li_equipos" class="treeview">
    <a href="#"><i class="fa fa-car"></i><span>Equipos</span>
        <span class="pull-right-container">
            <i class="fa fa-angle-left pull-right"></i>
        </span>
    </a>
    <ul runat="server" id="ul_equipos" class="treeview-menu">
        <li runat="server" id="li_equipos0ul_equipos"><a href="../Aplicativo/admin_equipos.aspx">Equipos</a></li>
        <li runat="server" id="li_equipos0ul_categoias"><a href="../Aplicativo/admin_tipo_equipo.aspx">Categorias</a></li>
    </ul>
</li>

<li runat="server" id="li_io" class="treeview">
    <a href="#"><i class="fa fa-book"></i><span>Ingresos - Egresos</span>
        <span class="pull-right-container">
            <i class="fa fa-angle-left pull-right"></i>
        </span>
    </a>
    <ul runat="server" id="ul_io" class="treeview-menu">
        <li runat="server" id="li_io0ul_conceptos"><a href="../Aplicativo/admin_conceptos_mensuales.aspx">Conceptos I/E</a></li>
        <li class="dropdown-header">I/E por equipo</li>
        <li runat="server" id="li_io0ul_mes_equipo"><a href="../Aplicativo/admin_valores_mes_equipo_v2.aspx">Valores mensuales</a></li>
        <li runat="server" id="li_io0ul_anio_equipo"><a href="../Aplicativo/ver_valores_cargados_equipo.aspx">Resumen año equipo</a></li>
        <li class="dropdown-header">I/E por categoria</li>
        <li runat="server" id="li_io0ul_mes_categoria"><a href="../Aplicativo/admin_valores_mes_categoria.aspx">Valores mensuales</a></li>
        <li runat="server" id="li_io0ul_anio_categoria"><a href="../Aplicativo/ver_valores_cargados_categoria.aspx">Resumen año categoria</a></li>
        <li runat="server" id="li_io0ul_equipos_categoria"><a href="planilla_resumen_valores_equipo_categoria.aspx">Resumen año equipos categoria </a></li>
        <li class="dropdown-header">Administrar valores dolar</li>
        <li runat="server" id="li_io0ul_dolar"><a href="../Aplicativo/admin_valor_dolar.aspx">Editar valores dolar</a></li>
    </ul>
</li>

<li runat="server" id="li_horas" class="treeview">
    <a href="#"><i class="fa fa-clock-o"></i><span>Horas trabajadas</span>
        <span class="pull-right-container">
            <i class="fa fa-angle-left pull-right"></i>
        </span>
    </a>
    <ul runat="server" id="ul_horas" class="treeview-menu">
        <li runat="server" id="li_horas0ul_feriados"><a href="../Aplicativo/admin_feriados.aspx">Feriados</a></li>
        <li runat="server" id="li_horas0ul_horas"><a href="../Aplicativo/admin_horas_mes.aspx">Horas mes</a></li>
    </ul>
</li>

<li runat="server" id="li_planillas" class="treeview">
    <a href="#"><i class="fa fa-files-o"></i><span>Planillas</span>
        <span class="pull-right-container">
            <i class="fa fa-angle-left pull-right"></i>
        </span>
    </a>
    <ul runat="server" id="ul_planillas" class="treeview-menu">
        <li runat="server" id="li_planillas0ul_principal"><a href="admin_horas_planilla_principal.aspx">En función de horas empleado</a></li>
        <li runat="server" id="li_planillas0ul_administrativo"><a href="planilla_gastos_administrativos.aspx">Gastos administración</a></li>
        <li runat="server" id="li_planillas0ul_resumen_horas"><a href="planilla_resumen_horas_trabajadas.aspx">Resumen horas trabajadas equipo</a></li>
        <li><a href="planilla_combustibles.aspx">Planilla combustibles</a></li>
    </ul>
</li>

<li runat="server" id="li_importar" class="treeview">
    <a href="#"><i class="fa fa-download"></i><span>Importar</span>
        <span class="pull-right-container">
            <i class="fa fa-angle-left pull-right"></i>
        </span>
    </a>
    <ul runat="server" id="ul_importar" class="treeview-menu">
         <li runat="server" id="li_importar0ulfacturacion" ><a href="admin_importar_blanco.aspx">Importar facturado</a></li>
    </ul>
</li>
