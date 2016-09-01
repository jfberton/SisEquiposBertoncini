<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="planilla_resumen_valores_equipo_categoria.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.planilla_resumen_valores_equipo_categoria" %>

<%@ Register Src="~/Aplicativo/Menues/menu_admin.ascx" TagPrefix="uc1" TagName="menu_admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_menu" runat="server">
    <uc1:menu_admin runat="server" ID="menu_admin" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_body" runat="server">
        
    <ol class="breadcrumb">
       <li>Inicio</li>
        <li>Ingresos - Egresos</li>
        <li>I/E por categoria</li>
        <li>Resumen año equipos categoría</li>
    </ol>

    <div class="panel panel-default">
        <div class="panel-heading">
            <h1 class="panel-title">Visualiza los valores anuales cargados al equipo
            </h1>
        </div>
        <div class="panel-body">
            <div class="row" runat="server" id="row_busqueda">
                <div class="col-md-5">
                    <table class="table-condensed" style="width: 100%">
                        <tr>
                            <td>Equipo</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddl_categoria" CssClass="form-control">
                                </asp:DropDownList></td>
                        </tr>
                    </table>
                </div>
                <div class="col-md-3">
                    <table class="table-condensed" style="width: 100%">
                        <tr>
                            <td>Año</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddl_anio" CssClass="form-control">
                                </asp:DropDownList></td>
                        </tr>
                    </table>
                </div>
                <div class="col-md-4">
                    <table class="table-condensed" style="width: 100%">
                        <tr>
                            <td>
                                <asp:Button Text="Obtener" runat="server" ID="btn_buscar" CssClass="btn btn-default" OnClick="btn_buscar_Click" />
                                <asp:Button Text="Imprimir" ID="btn_imprimir" CssClass="btn btn-default" OnClick="btn_imprimir_Click" runat="server" />
                                <asp:Button Text="Nueva búsqueda" runat="server" ID="btn_nueva_busqueda" CssClass="btn btn-danger" OnClick="btn_nueva_busqueda_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <asp:GridView ID="gv_detalle_equipos" runat="server" OnPreRender="gv_detalle_equipos_PreRender" EmptyDataText="No existen resultados"
        AutoGenerateColumns="False" GridLines="None" CssClass="display" ShowHeader="true" >
        <Columns>
            <asp:BoundField DataField="nombre_equipo" HeaderText="Equipo" ReadOnly="true" />
            <asp:BoundField DataField="tipo_resultado" HeaderText="Tipo resultado" ReadOnly="true" />
            <asp:BoundField DataField="enero" HeaderText="Enero" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
            <asp:BoundField DataField="febrero" HeaderText="Febrero" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
            <asp:BoundField DataField="marzo" HeaderText="Marzo" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
            <asp:BoundField DataField="abril" HeaderText="Abril" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
            <asp:BoundField DataField="mayo" HeaderText="Mayo" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
            <asp:BoundField DataField="junio" HeaderText="Junio" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
            <asp:BoundField DataField="julio" HeaderText="Julio" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
            <asp:BoundField DataField="agosto" HeaderText="Agosto" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
            <asp:BoundField DataField="septiembre" HeaderText="Septiembre" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
            <asp:BoundField DataField="octubre" HeaderText="Octubre" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
            <asp:BoundField DataField="noviembre" HeaderText="Noviembre" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
            <asp:BoundField DataField="diciembre" HeaderText="Diciembre" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
            <asp:BoundField DataField="total" HeaderText="Total Anual" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
            <asp:BoundField DataField="promedio" HeaderText="Promedio mensual" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
        </Columns>
    </asp:GridView>
</asp:Content>


<asp:Content ID="Content5" ContentPlaceHolderID="cph_style" runat="server">
    <link href="../css/jquery.dataTables.min.css" rel="stylesheet" />
    <link href="../css/fixedColumns.dataTables.min.css" rel="stylesheet" />
    <link href="../css/fixedHeader.dataTables.min.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cph_scripts" runat="server">

    <script src="../js/jquery.dataTables.min.js"></script>
    <script src="../js/dataTables.fixedColumns.min.js"></script>
    <script src="../js/dataTables.fixedHeader.min.js"></script>

    <script>
        $(document).ready(function () {
            var table = $('#<%= gv_detalle_equipos.ClientID %>').DataTable({
                "scrollY": 350,
                "scrollX": true,
                "scrollCollapse": true,
                "fixedHeader": true,
                "fixedColumns": {
                    "leftColumns": 2
                },
                "paging": false,
                "ordering": false,
                "autoWidth": false,
                "columnDefs": [
                     { "width": "150px", "targets": 0 }, //equipo
                     { "width": "200px", "targets": 1 }, //Concepto
                     { "width": "130px", "targets": 2 }, //enero
                     { "width": "130px", "targets": 3 }, //febrero
                     { "width": "130px", "targets": 4 }, //marzo
                     { "width": "130px", "targets": 5 }, //abril
                     { "width": "130px", "targets": 6 }, //mayo
                     { "width": "130px", "targets": 7 }, //junio
                     { "width": "130px", "targets": 8 }, //julio
                     { "width": "130px", "targets": 9 }, //agosto
                     { "width": "130px", "targets": 10 }, //septiembre
                     { "width": "130px", "targets": 11 }, //octubre
                     { "width": "130px", "targets": 12 }, //noviembre
                     { "width": "130px", "targets": 13 }, //diciembre
                     { "width": "150px", "targets": 14 }, //acumulado
                     { "width": "130px", "targets": 15 } //promedio
                ],
                "language": {
                    "search": "Buscar:",
                    "zeroRecords": "No se encontraron registros",
                    "info": "Mostrando _START_ de _END_ de _TOTAL_ registros",
                    "infoEmpty": "No hay registros disponibles",
                    "infoFiltered": "(filtrado de _MAX_ registros totales)"
                },
            });
        });
    </script>
</asp:Content>
