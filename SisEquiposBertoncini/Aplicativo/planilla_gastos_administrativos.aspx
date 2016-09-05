<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="planilla_gastos_administrativos.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.planilla_gastos_administrativos" %>

<%@ Register Src="~/Aplicativo/Menues/menu_admin.ascx" TagPrefix="uc1" TagName="menu_admin" %>
<%@ Register Src="~/Aplicativo/Controles/valor_decimal.ascx" TagPrefix="uc1" TagName="valor_decimal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_menu" runat="server">
    <uc1:menu_admin runat="server" ID="menu_admin" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_body" runat="server">

    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>Planilla de gastos administrativos
       
                        <small>detalle de la distribución en los equipos de los gastos administrativos</small>
        </h1>

        <ol class="breadcrumb">
            <li>Inicio</li>
            <li>Planillas</li>
            <li class="active">Gastos administración</li>
        </ol>
    </section>

    <!-- Main content -->
    <section class="content">

        <!-- Your Page Content Here -->
        <table class="table table-bordered">
            <tr>
                <td>Mes</td>
                <td>
                    <asp:DropDownList runat="server" ID="ddl_mes" CssClass="form-control" Width="100%">
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
                <td>Año</td>
                <td>
                    <asp:DropDownList runat="server" ID="ddl_anio" CssClass="form-control" Width="100%">
                    </asp:DropDownList></td>
                <td>
                    <asp:Button Text="buscar" ID="btn_buscar" CssClass="btn btn-default" OnClick="btn_buscar_Click" runat="server" />
                    <asp:Button Text="Nueva busqueda" ID="btn_nueva_busqueda" Visible="false" CssClass="btn btn-danger" OnClick="btn_nueva_busqueda_Click" runat="server" />
                </td>

            </tr>
        </table>
        <h4><u>Montos por distribuir</u></h4>
        <table class="table table-bordered" id="table_edit" runat="server">
            <tr>
                <td>Telefonía celular</td>
                <td>
                    <asp:TextBox runat="server" ID="tb_telefonia_celular" CssClass="form-control" />
                </td>
                <td>Sueldos administración</td>
                <td>
                    <asp:TextBox runat="server" ID="tb_sueldos_administracion" CssClass="form-control" />
                </td>
            </tr>
            <tr>
                <td>Honorarios varios</td>
                <td>
                    <asp:TextBox runat="server" ID="tb_honorarios_sistema" CssClass="form-control" />
                </td>
                <%-- <td>Honorarios contable</td>
            <td>
                <asp:TextBox runat="server" id="tb_honorarios_contables" CssClass="form-control"/>
            </td>--%>
            </tr>
            <tr>
                <td>Papelería - Librería</td>
                <td>
                    <asp:TextBox runat="server" ID="tb_papeleria_libreria" CssClass="form-control" />
                </td>
                <td>Otros</td>
                <td>
                    <asp:TextBox runat="server" ID="tb_otros" CssClass="form-control" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td></td>
                <td>
                    <asp:Button Text="Aplicar cambios" ID="btn_aplicar" CssClass="btn btn-default" OnClick="btn_aplicar_Click" runat="server" /></td>
            </tr>
        </table>

        <table class="table table-bordered" id="table_view" runat="server">
            <tr>
                <td>Telefonía celular</td>
                <td>
                    <asp:Label ID="lbl_telefonia_celular" runat="server" />
                </td>
                <td>Sueldos administración</td>
                <td>
                    <asp:Label ID="lbl_sueldos_administracion" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Honorarios varios</td>
                <td>
                    <asp:Label ID="lbl_honorarios_varios" runat="server" />
                </td>
                <%--  <td>Honorarios contable</td>
            <td>
                <asp:Label id="lbl_honorarios_contables" runat="server" />
            </td>--%>
            </tr>
            <tr>
                <td>Papelería - Librería</td>
                <td>
                    <asp:Label ID="lbl_papeleria" runat="server" />
                </td>
                <td>Otros</td>
                <td>
                    <asp:Label ID="lbl_otros" runat="server" />
                </td>
            </tr>
        </table>
        <h4><u>Equipos</u></h4>
        <div class="row" id="div_agregar_equipo" runat="server">
            <div class="col-md-12">
                <table class="table table-bordered">
                    <tr>
                        <td>Seleccione equipo a agregar</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddl_equipos" CssClass="form-control" Width="100%">
                            </asp:DropDownList>
                        </td>
                        <td>Porcentaje de participación (max
                        <asp:Label Text="" ID="lbl_maximo_nivel_porcentaje_participacion" runat="server"></asp:Label>
                            )</td>
                        <td>
                            <asp:TextBox runat="server" CssClass="form-control" ID="tb_porcentaje" />
                        </td>
                        <td>
                            <asp:Button Text="Agregar" runat="server" ID="btn_agregar_equipo" CssClass="btn btn-default" OnClick="btn_agregar_equipo_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div runat="server" id="div_detalle">
                </div>
            </div>
        </div>
    </section>
    <!-- /.content -->
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_scripts" runat="server">
</asp:Content>
