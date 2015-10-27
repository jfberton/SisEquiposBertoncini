﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="admin_horas_planilla_principal.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.admin_horas_planilla_principal" %>

<%@ Register Src="~/Aplicativo/Menues/menu_admin.ascx" TagPrefix="uc1" TagName="menu_admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_menu" runat="server">
    <uc1:menu_admin runat="server" ID="menu_admin" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_body" runat="server">
    <div class="panel panel-default">
        <div class="panel-heading">
            <h1 class="panel-title">Planilla principal</h1>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-12">
                    <table class="table-condensed" style="width: 100%;">
                        <tr>
                            <td>Categoría de empleado</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddl_tipo_empleado" CssClass="form-control">
                                    <asp:ListItem Text="Mecánicos - Pintores" />
                                    <asp:ListItem Text="Soldadores" />
                                </asp:DropDownList></td>
                            <td>Mes</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddl_mes" CssClass="form-control">
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
                            <td>Año
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddl_anio" CssClass="form-control">
                                </asp:DropDownList></td>
                            <td>
                                <asp:Button Text="Buscar" CssClass="btn btn-default" ID="btn_buscar" OnClick="btn_buscar_Click" runat="server" />
                                <asp:Button Text="Nueva búsqueda" CssClass="btn btn-warning" ID="btn_nueva_busqueda" OnClick="btn_nueva_busqueda_Click" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="row" runat="server" id="div_resultados_busqueda">
                <div class="col-md-12">
                    <asp:GridView ID="gv_planilla_empleados" runat="server" EmptyDataText="No existen empleados por mostrar en esta categoría." OnRowDataBound="gv_planilla_empleados_RowDataBound"
                        AutoGenerateColumns="False" GridLines="None" CssClass="table table-condensed table-bordered">
                        <Columns>
                            <asp:BoundField DataField="empleado" HeaderText="Nombre" ReadOnly="true" />
                             <asp:TemplateField HeaderText="Sueldo">
                                <ItemTemplate>
                                    <asp:Label Text='<%#Eval("sueldo")%>' runat="server" />
                                    <button runat="server" class="btn btn-sm btn-default" id="btn_editar_sueldo_empleado" causesvalidation="false" onserverclick="btn_editar_sueldo_empleado_ServerClick" data-id='<%#Eval("empleado_id")%>'>
                                        <span class="glyphicon glyphicon-edit" aria-hidden="true"></span> Editar
                                    </button>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Días mes">
                                <ItemTemplate>
                                    <asp:Label Text='<%#Eval("dias_mes")%>' runat="server" />
                                    <button runat="server" class="btn btn-sm btn-default" id="btn_ver" causesvalidation="false" onserverclick="btn_ver_ServerClick" data-id='<%#Eval("empleado_id")%>'>
                                        <span class="glyphicon glyphicon-eye-open" aria-hidden="true"></span> Ver mes
                                    </button>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:BoundField DataField="dias_out" HeaderText="Días OUT" ReadOnly="true" />
                            <asp:BoundField DataField="costo_mensual_ponderado" HeaderText="Costo mensual ponderado" ReadOnly="true" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_scripts" runat="server">
</asp:Content>
