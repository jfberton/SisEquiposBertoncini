﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="admin_tipo_empleado.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.admin_tipo_empleado" %>

<%@ Register Src="~/Aplicativo/Menues/menu_admin.ascx" TagPrefix="uc1" TagName="menu_admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_menu" runat="server">
    <uc1:menu_admin runat="server" ID="menu_admin" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_body" runat="server">

    <ol class="breadcrumb">
       <li>Inicio</li>
        <li>Empleados</li>
        <li>Categorías</li>
    </ol>

    <div class="panel panel-default">
        <div class="panel-heading">
            <div class="row">
                <div class="col-md-5">
                    <h4 class="panel-title">Categorías de empleados</h4>
                </div>
            </div>
        </div>
        <div class="panel-body">
            <asp:GridView ID="gv_categorias" runat="server" EmptyDataText="No existen categorías por mostrar." OnPreRender="gv_categorias_PreRender"
                AutoGenerateColumns="False" GridLines="None" CssClass="display">
                <Columns>
                    <asp:BoundField DataField="categoria_nombre" HeaderText="Nombre" ReadOnly="true" />
                    <asp:BoundField DataField="categoria_cantidad_empleados" HeaderText="Empleados" ReadOnly="true" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <button
                                type="button" class="btn btn-sm btn-danger"
                                data-toggle="modal"
                                data-target="#advertencia_eliminacion"
                                data-id='<%#Eval("categoria_id")%>'
                                data-introduccion="la categoría"
                                data-nombre='<%#Eval("categoria_nombre")%>'>
                                <span class="glyphicon glyphicon-remove-circle" aria-hidden="true"></span>Eliminar
                            </button>

                            <button runat="server" class="btn btn-sm btn-default" id="btn_ver" causesvalidation="false" onserverclick="btn_ver_ServerClick" data-id='<%#Eval("categoria_id")%>'>
                                <span class="glyphicon glyphicon-eye-open" aria-hidden="true"></span>Ver
                            </button>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <asp:GridView ID="gv_categorias_view" runat="server" EmptyDataText="No existen categorías por mostrar." OnPreRender="gv_categorias_PreRender"
                AutoGenerateColumns="False" GridLines="None" CssClass="table table-condensed table-bordered">
                <Columns>
                    <asp:BoundField DataField="categoria_nombre" HeaderText="Nombre" ReadOnly="true" />
                    <asp:BoundField DataField="categoria_cantidad_empleados" HeaderText="Empleados" ReadOnly="true" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <button runat="server" class="btn btn-sm btn-default" id="btn_ver" causesvalidation="false" onserverclick="btn_ver_ServerClick" data-id='<%#Eval("categoria_id")%>'>
                                <span class="glyphicon glyphicon-eye-open" aria-hidden="true"></span>Ver
                            </button>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <div class="modal fade" id="advertencia_eliminacion" role="dialog" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content panel-danger">
                        <div class="modal-header panel-heading">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title panel-title"><span class="glyphicon glyphicon-remove-sign" aria-hidden="true"></span>ATENCIÓN!!</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-12">
                                    <input type="hidden" runat="server" id="id_item_por_eliminar" />
                                    <p id="texto_a_mostrar"></p>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button Text="Aceptar" CssClass="btn btn-success" CausesValidation="false" ID="btn_aceptar_eliminacion" OnClick="btn_aceptar_eliminacion_Click" runat="server" />
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>
                </div>
            </div>

        </div>
        <div class="panel-footer">
            <div class="row">
                <div class="col-md-12">
                    <button type="button" class="btn btn-default pull-right" id="btn_agregar_categoria" runat="server" data-toggle="modal" data-target="#agregar_categoria">
                        <span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span>Agregar nuevo
                    </button>
                    <div class="modal fade" id="agregar_categoria" role="dialog" aria-hidden="true">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                    <h4 class="modal-title">Agregar categoría</h4>
                                </div>
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:ValidationSummary ID="validation_summary" runat="server" DisplayMode="BulletList"
                                                CssClass="validationsummary panel panel-danger" HeaderText="<div class='panel-heading'>&nbsp;Corrija los siguientes errores antes de continuar:</div>" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <table class="table-condensed" style="width: 100%">
                                                <tr>
                                                    <td>Nombre categoría</td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div class="col-md-8">
                                            <table class="table-condensed" style="width: 100%">
                                                <tr>
                                                    <td style="width: auto">
                                                        <input type="text" id="tb_nombre_categoria" class="form-control" runat="server" placeholder="Nombre de la categoría" /></td>
                                                    <td>
                                                        <asp:RequiredFieldValidator ControlToValidate="tb_nombre_categoria" Text="<img src='../img/exclamation.gif' title='Debe ingresar el nombre de la categoría' />"
                                                            ID="rv_nombre_categoria" runat="server" ErrorMessage="Debe ingresar el nombre de la categoría">
                                                        </asp:RequiredFieldValidator></td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-12">
                                            Descripción:
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <textarea rows="5" class="form-control" runat="server" id="tb_descripcion_categoria" placeholder="Descripción de la categoría (no obligatorio)"></textarea>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <button id="btn_guardar" runat="server" onserverclick="btn_guardar_ServerClick" class="btn btn-success" validationgroup="equipo">
                                        <span class="glyphicon glyphicon-floppy-disk" aria-hidden="true"></span>Guardar!
                                    </button>
                                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="ver_categoria" role="dialog" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Datos categoría seleccionada</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12">
                                <p>
                                    <label>Nombre de la categoría</label>
                                    <asp:Label Text="" ID="lbl_nombre_categoria" runat="server" />
                                </p>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <label>Descripción</label>
                                <br />
                                <asp:Label Text="" ID="lbl_descripcion" runat="server" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <label>Empleados</label>
                                <br />
                                <asp:GridView ID="gv_empleados" runat="server" EmptyDataText="No existen empleados para mostrar." OnPreRender="gv_categorias_PreRender"
                                    AutoGenerateColumns="False" GridLines="None" CssClass="table table-condensed table-bordered">
                                    <Columns>
                                        <asp:BoundField DataField="area_empleado" HeaderText="Area" ReadOnly="true" />
                                        <asp:BoundField DataField="nombre_empleado" HeaderText="Nombre" ReadOnly="true" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
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
        $('#advertencia_eliminacion').on('show.bs.modal', function (event) {
            // Button that triggered the modal
            var button = $(event.relatedTarget)

            // Extract info from data-* attributes
            var id = button.data('id')
            var introduccion = button.data('introduccion')
            var nombre = button.data('nombre')

            // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
            var modal = $(this)
            modal.find('.modal-body #' + '<%= id_item_por_eliminar.ClientID %>').val(id)
            modal.find('.modal-body #texto_a_mostrar').text('Esta por eliminar ' + introduccion + ' ' + nombre + '. Desea continuar?')
        })

        $(document).ready(function () {
            $('#<%= gv_categorias.ClientID %>').DataTable({
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
             $('#<%= gv_categorias_view.ClientID %>').DataTable({
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
             $('#<%= gv_empleados.ClientID %>').DataTable({
                 "scrollY": "200px",
                 "scrollCollapse": true,
                 "searching": false,
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
