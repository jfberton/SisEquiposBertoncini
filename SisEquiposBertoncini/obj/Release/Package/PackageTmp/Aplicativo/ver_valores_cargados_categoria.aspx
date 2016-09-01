<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ver_valores_cargados_categoria.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.ver_valores_cargados_categoria" %>

<%@ Register Src="~/Aplicativo/Menues/menu_admin.ascx" TagPrefix="uc1" TagName="menu_admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/jquery.treegrid.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_menu" runat="server">
    <uc1:menu_admin runat="server" ID="menu_admin" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_body" runat="server">

    <ol class="breadcrumb">
       <li>Inicio</li>
        <li>Ingresos - Egresos</li>
        <li>I/E por categoria</li>
        <li>Resumen año categoria</li>
    </ol>

    <div class="panel panel-default">
        <div class="panel-heading">
            <h1 class="panel-title">Visualiza los valores anuales cargados a la categoria
            </h1>
        </div>
        <div class="panel-body">
            <div class="row" runat="server" id="row_busqueda">
                <div class="col-md-4">
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
                <div class="col-md-5">
                    <table class="table-condensed" style="width: 100%">
                        <tr>
                            <td>
                                <asp:Button Text="Obtener" runat="server" ID="btn_buscar" CssClass="btn btn-default" OnClick="btn_buscar_Click" />
                                <asp:Button Text="Imprimir" ID="btn_imprimir" CssClass="btn btn-default" OnClick="btn_imprimir_Click" runat="server" />
                                <asp:Button Text="Imprimir Resumen" ID="btn_imprimir_resumen" CssClass="btn btn-default" OnClick="btn_imprimir_resumen_Click" runat="server" />
                                <asp:Button Text="Nueva búsqueda" runat="server" ID="btn_nueva_busqueda" CssClass="btn btn-danger" OnClick="btn_nueva_busqueda_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <br />
            <div class="row">

                <div class="col-md-12" runat="server" id="div_buscar_primero">
                    <div class="alert alert-warning alert-dismissible" role="alert">
                        <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <small>Seleccione los campos <strong>categoria y año</strong> y luego presione obtener.</small>
                    </div>
                </div>
                <div class="col-md-12">
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <div runat="server" id="div_tree">
            </div>
        </div>
    </div>
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
    <script src="../js/jquery.treegrid.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('.tree').treegrid();
            $('.table').DataTable({
                "scrollY": 350,
                "scrollX": true,
                "scrollCollapse": true,
                "fixedHeader": true,
                "fixedColumns": true,
                "paging": false,
                "ordering": false,
                "autoWidth": false,
                "columnDefs": [
                     { "width": "300px", "targets": 0 }, //conceptos
                     { "width": "130px", "targets": 1 }, //enero
                     { "width": "130px", "targets": 2 }, //febrero
                     { "width": "130px", "targets": 3 }, //marzo
                     { "width": "130px", "targets": 4 }, //abril
                     { "width": "130px", "targets": 5 }, //mayo
                     { "width": "130px", "targets": 6 }, //junio
                     { "width": "130px", "targets": 7 }, //julio
                     { "width": "130px", "targets": 8 }, //agosto
                     { "width": "130px", "targets": 9 }, //septiembre
                     { "width": "130px", "targets": 10 }, //octubre
                     { "width": "130px", "targets": 11 }, //noviembre
                     { "width": "130px", "targets": 12 }, //diciembre
                     { "width": "150px", "targets": 13 }, //acumulado
                     { "width": "130px", "targets": 14 } //promedio
                ],
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

