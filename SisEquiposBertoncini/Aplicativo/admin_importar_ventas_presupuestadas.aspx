<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="admin_importar_ventas_presupuestadas.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.admin_importar_ventas_presupuestadas" %>

<%@ Register Src="~/Aplicativo/Menues/menu_admin.ascx" TagPrefix="uc1" TagName="menu_admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .btn-file {
            position: relative;
            overflow: hidden;
        }

            .btn-file input[type=file] {
                position: absolute;
                top: 0;
                right: 0;
                min-width: 100%;
                min-height: 100%;
                font-size: 100px;
                text-align: right;
                filter: alpha(opacity=0);
                opacity: 0;
                outline: none;
                background: white;
                cursor: inherit;
                display: block;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_menu" runat="server">
    <uc1:menu_admin runat="server" ID="menu_admin" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_body" runat="server">

    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>Importar datos persupuestados
       
                        <small>importa datos de presupuestados desde el archivo excel seleccionado</small>
        </h1>

        <ol class="breadcrumb">
            <li><a href="#"><i></i>Inicio</a></li>
            <li class="active">Importar presupuestado</li>
        </ol>
    </section>

    <!-- Main content -->
    <section class="content">

        <!-- Your Page Content Here -->
        <div class="editor-label">
            <div class="input-group">
                <span class="input-group-btn">
                    <span class=" btn btn-warning btn-file">Buscar PP
                    <input type="file" runat="server" name="data" class="text-warning" id="data" />
                    </span>
                </span>
                <input type="text" id="valdfil" class="form-control" readonly="true" />
                <span class="input-group-btn">
                    <button class="btn btn-warning" runat="server" onserverclick="btn_importar_Click">Importar</button></span>
            </div>
        </div>
        <br />
        <div id="div_resultados" runat="server" visible="false">
            <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                <div class="panel panel-default">
                    <div class="panel-heading" role="tab" id="headingOne">
                        <h4 class="panel-title">
                            <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">Datos ingresados
                            </a>
                        </h4>
                    </div>
                    <div id="collapseOne" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
                        <div class="panel-body">
                            <asp:GridView ID="GridView1" OnPreRender="Gridvew_PreRender" GridLines="None" CssClass="display" runat="server" OnRowDataBound="GridView1_RowDataBound">
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading" role="tab" id="headingTwo">
                        <h4 class="panel-title">
                            <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">Registros correctos por insertar
        <asp:Label Text="" ID="lbl_cantidad_correctos" runat="server" />
                            </a>
                        </h4>
                    </div>
                    <div id="collapseTwo" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingTwo">
                        <div class="panel-body">
                            <asp:GridView ID="GridView2" CssClass="table" OnPreRender="Gridvew_PreRender" runat="server">
                            </asp:GridView>
                            <br />
                            <button runat="server" class="btn btn-default" id="btn_impactar" onserverclick="btn_impactar_ServerClick">Impactar registros en detalles mensuales</button>
                        </div>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading" role="tab" id="headingThree">
                        <h4 class="panel-title">
                            <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseThree" aria-expanded="false" aria-controls="collapseThree">Registros correctos pero sin matchear
                            </a>
                        </h4>
                    </div>
                    <div id="collapseThree" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingThree">
                        <div class="panel-body">
                            <asp:GridView ID="GridView3" CssClass="table" OnPreRender="Gridvew_PreRender" runat="server">
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <!-- /.content -->

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph_style" runat="server">
    <link href="../css/jquery.dataTables.min.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cph_scripts" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            $(document).on('change', '.btn-file :file', function () {
                var input = $(this),
                    numFiles = input.get(0).files ? input.get(0).files.length : 1,
                    label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
                input.trigger('fileselect', [numFiles, label]);
            });

            $('.btn-file :file').on('fileselect', function (event, numFiles, label) {
                console.log(numFiles);
                console.log(label);
                $("#valdfil").val(label);
                $('#<%= div_resultados.ClientID %>').hide();
            });
        });

    </script>
    <script src="../js/jquery.dataTables.min.js"></script>
    <script src="../js/dataTables.bootstrap.min.js"></script>
    <script>

        $(document).ready(function () {
            $('#<%= GridView1.ClientID %>').DataTable({
                //"scrollY": "400px",
                //"scrollX": true,
                //"scrollCollapse": true,
                "paging": false,
                "language": {
                    "search": "Buscar:",
                    "zeroRecords": "No se encontraron registros",
                    "info": "Mostrando _START_ de _END_ de _TOTAL_ registros",
                    "infoEmpty": "No hay registros disponibles",
                    "infoFiltered": "(filtrado de _MAX_ registros totales)"
                }
            });
            $('#<%= GridView2.ClientID %>').DataTable({
                //"scrollY": "400px",
                //"scrollX": true,
                //"scrollCollapse": true,
                "paging": false,
                "language": {
                    "search": "Buscar:",
                    "zeroRecords": "No se encontraron registros",
                    "info": "Mostrando _START_ de _END_ de _TOTAL_ registros",
                    "infoEmpty": "No hay registros disponibles",
                    "infoFiltered": "(filtrado de _MAX_ registros totales)"
                }
            });
            $('#<%= GridView3.ClientID %>').DataTable({
                //"scrollY": "400px",
                //"scrollX": true,
                //"scrollCollapse": true,
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