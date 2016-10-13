<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ver_valores_cargados_equipo.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.ver_valores_cargados_equipo" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<%@ Register Src="~/Aplicativo/Menues/menu_admin.ascx" TagPrefix="uc1" TagName="menu_admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/jquery.treegrid.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_menu" runat="server">
    <uc1:menu_admin runat="server" ID="menu_admin" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_body" runat="server">

    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>Valores anuales equipo
       
                        <small>visualiza los valores anuales cargados al equipo</small>
        </h1>

        <ol class="breadcrumb">
            <li>Inicio</li>
            <li>Ingresos - Egresos</li>
            <li>I/E por equipo</li>
            <li class="active">Resumen año equipo</li>
        </ol>
    </section>

    <!-- Main content -->
    <section class="content">

        <!-- Your Page Content Here -->
        <div class="row" runat="server" id="row_busqueda">
            <div class="col-md-4">
                <table class="table-condensed" style="width: 100%">
                    <tr>
                        <td>Equipo</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddl_equipo" CssClass="form-control">
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
                            <button class="btn btn-default" runat="server" id="btn_imprimir" onclick="Imprimir(); return false;">Imprimir</button>
                            <button class="btn btn-default" runat="server" id="btn_imprimir_resumen" onclick="Imprimir_resumen(); return false;">Imprimir resumen</button>
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
                    <small>Seleccione los campos <strong>Equipo y año</strong> y luego presione obtener.</small>
                </div>
            </div>
            <div class="col-md-12">
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <div runat="server" id="div_tree" style="height: 100px;">
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <asp:Chart ID="Chart1" runat="server" Width="1178px" Height="163px" BackColor="0, 192, 192" BackGradientStyle="TopBottom" BackSecondaryColor="White" BorderlineColor="Black" BorderlineDashStyle="Solid"  >
                    <Series>
                        <asp:Series Name="velocidad_de_recupero" YValueType="Double" ChartType="Spline" ChartArea="chart_area" IsValueShownAsLabel="true" LabelFormat="n2" XValueType="String" IsXValueIndexed="True" MarkerStep="1" BorderWidth="3" MarkerStyle="Triangle" MarkerSize="7" MarkerColor="#000099">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="chart_area">
                            <AxisX Interval="1" IsMarginVisible="true">
                                <MajorGrid Interval="1" LineDashStyle="Dot" LineColor="Gray" />
                            </AxisX>
                            <AxisY IsMarginVisible="true">
                                <MajorGrid LineDashStyle="Dot" LineColor="Gray" />
                            </AxisY>
                        </asp:ChartArea>
                    </ChartAreas>
                    <Titles>
                        <asp:Title Name="Title1" Text="Velocidad de recupero" Font="Microsoft Sans Serif, 12pt">
                        </asp:Title>
                    </Titles>
                </asp:Chart>
            </div>
        </div>
      
    </section>
    <!-- /.content -->

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph_style" runat="server">
    <link href="../css/jquery.dataTables.min.css" rel="stylesheet" />
    <link href="../css/fixedColumns.dataTables.min.css" rel="stylesheet" />
    <link href="../css/fixedHeader.dataTables.min.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cph_scripts" runat="server">

    <script src="../js/jquery.treegrid.js"></script>
    <script src="../js/jquery.dataTables.min.js"></script>
    <script src="../js/dataTables.fixedColumns.min.js"></script>
    <script src="../js/dataTables.fixedHeader.min.js"></script>
    <script src="../js/jquery.fixedheadertable.min.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('.tree').treegrid();
            //$('.table').fixedHeaderTable({ altClass: 'odd'});
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
                     { "width": "100px", "targets": 1 }, //enero
                     { "width": "100px", "targets": 2 }, //febrero
                     { "width": "100px", "targets": 3 }, //marzo
                     { "width": "100px", "targets": 4 }, //abril
                     { "width": "100px", "targets": 5 }, //mayo
                     { "width": "100px", "targets": 6 }, //junio
                     { "width": "100px", "targets": 7 }, //julio
                     { "width": "100px", "targets": 8 }, //agosto
                     { "width": "100px", "targets": 9 }, //septiembre
                     { "width": "100px", "targets": 10 }, //octubre
                     { "width": "100px", "targets": 11 }, //noviembre
                     { "width": "100px", "targets": 12 }, //diciembre
                     { "width": "120px", "targets": 13 }, //acumulado
                     { "width": "100px", "targets": 14 } //promedio
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

        function Imprimir() {
            window.open('Reportes/dispatcher_report.aspx?reporte=valores_anuales_equipo', 'Valores anuales equipo', 'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=0,width=screen.width,height=screen.height,top=0,left=0');
        }

        function Imprimir_resumen() {
            window.open('Reportes/dispatcher_report.aspx?reporte=valores_anuales_equipo_resumen', 'Valores anuales equipo resumen', 'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=0,width=screen.width,height=screen.height,top=0,left=0');
        }
    </script>

</asp:Content>
