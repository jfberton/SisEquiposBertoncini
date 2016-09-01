<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="planilla_resumen_horas_trabajadas.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.planilla_resumen_horas_trabajadas" %>

<%@ Register Src="~/Aplicativo/Menues/menu_admin.ascx" TagPrefix="uc1" TagName="menu_admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_menu" runat="server">
    <uc1:menu_admin runat="server" ID="menu_admin" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_body" runat="server">

    <ol class="breadcrumb">
        <li>Inicio</li>
        <li>Planillas</li>
        <li>Resumen horas trabajadas equipo</li>
    </ol>

    <h3>Planilla resumen de horas trabajadas mes</h3>
    <table class="table table-condensed" style="width: 100%">
        <tr>
            <td>Tipo empleado</td>
            <td>
                <asp:DropDownList runat="server" ID="ddl_tipo_empleado" AutoPostBack="true" OnSelectedIndexChanged="ddl_tipo_empleado_SelectedIndexChanged" CssClass="form-control">
                    <asp:ListItem Text="Todos" />
                    <asp:ListItem Text="Mecánicos - Pintores" />
                    <asp:ListItem Text="Soldadores" />
                </asp:DropDownList></td>
            <td>Empleado</td>
            <td>
                <asp:DropDownList runat="server" ID="ddl_empleado" CssClass="form-control">
                    <asp:ListItem Text="Todos" />
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
            <td>Año</td>
            <td>
                <asp:DropDownList runat="server" ID="ddl_anio" CssClass="form-control">
                </asp:DropDownList></td>
            <td>
                <asp:Button Text="Buscar" ID="btn_buscar" OnClick="btn_buscar_Click" CssClass="btn btn-sm btn-default" runat="server" />
                <asp:Button Text="Nueva busqueda" ID="btn_nueva_busqueda" OnClick="btn_nueva_busqueda_Click" CssClass="btn btn-sm btn-danger" runat="server" />
            </td>
        </tr>
    </table>
    <div class="alert alert-success text-center" role="alert">
        <label>
            Horas totales 
        <asp:Label Text="000:00" ID="lbl_horas_totales" runat="server" /></label>
    </div>

    <div class="row">
        <div class="col-md-12">
            <asp:GridView ID="gv_horas_equipo" runat="server" OnPreRender="gv_horas_equipo_PreRender"
                AutoGenerateColumns="False" GridLines="None" CssClass="display">
                <Columns>
                    <asp:BoundField DataField="equipo" HeaderText="Equipo" ReadOnly="true" />
                    <asp:BoundField DataField="horas" HeaderText="Horas totales" ReadOnly="true" />
                </Columns>
            </asp:GridView>

        </div>
    </div>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cph_style" runat="server">
    <link href="../css/jquery.dataTables.min.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cph_scripts" runat="server">

    <script src="../js/jquery.dataTables.min.js"></script>
    <script src="../js/dataTables.bootstrap.min.js"></script>

    <script>
        $(document).ready(function () {
            $('#<%= gv_horas_equipo.ClientID %>').DataTable({
                "scrollY": "400px",
                "scrollCollapse": true,
                "paging": false,
                "language": {
                    "search": "Buscar:",
                    "zeroRecords": "No se encontraron registros",
                    "info": "Mostrando _START_ de _END_ de _TOTAL_ registros",
                    "infoEmpty": "No hay registros disponibles",
                    "infoFiltered": "(filtrado de _MAX_ registros totales)"
                }
            });
        });
    </script>

</asp:Content>
