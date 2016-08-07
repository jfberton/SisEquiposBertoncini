<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="admin_feriados.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.admin_feriados" %>

<%@ Register Src="~/Aplicativo/Menues/menu_admin.ascx" TagPrefix="uc1" TagName="menu_admin" %>
<%@ Register Src="~/Aplicativo/Menues/menu_usuario.ascx" TagPrefix="uc1" TagName="menu_usuario" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_menu" runat="server">
    <uc1:menu_admin runat="server" ID="menu_admin" />
    <uc1:menu_usuario runat="server" ID="menu_usuario" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_body" runat="server">

    <ol class="breadcrumb">
        <li>Inicio</li>
        <li>Horas trabajadas</li>
        <li>Feriados</li>
    </ol>

    <div class="panel panel-default">
        <div class="panel-heading">
            <div class="row">
                <div class="col-md-5">
                    <h4 class="panel-title">Feriados</h4>
                </div>
            </div>
        </div>
        <div class="panel-body">
            <asp:GridView ID="gv_feriados" runat="server" EmptyDataText="No existen feriados para mostrar." OnPreRender="gv_feriados_PreRender"
                AutoGenerateColumns="False" GridLines="None" CssClass="display">
                <Columns>
                    <asp:BoundField DataField="feriado_fecha" HeaderText="Fecha" DataFormatString="{0:D}" ReadOnly="true" />
                    <asp:BoundField DataField="feriado_descripcion" HeaderText="Descripción" ReadOnly="true" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <button
                                type="button" class="btn btn-sm btn-danger"
                                data-toggle="modal"
                                data-target="#advertencia_eliminacion"
                                data-id='<%#Eval("feriado_id")%>'
                                data-introduccion="el feriado de fecha"
                                data-nombre='<%#String.Concat(Eval("feriado_fecha"), " \"", Eval("feriado_descripcion"),"\"") %>'>
                                <span class="glyphicon glyphicon-remove-circle" aria-hidden="true"></span>Eliminar
                            </button>
                            <button runat="server" class="btn btn-sm btn-default" id="btn_ver" causesvalidation="false" onserverclick="btn_ver_Click" data-id='<%#Eval("feriado_id")%>'>
                                <span class="glyphicon glyphicon-eye-open" aria-hidden="true"></span>Ver
                            </button>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <asp:GridView ID="gv_feriados_view" runat="server" EmptyDataText="No existen feriados para mostrar." OnPreRender="gv_feriados_PreRender"
                AutoGenerateColumns="False" GridLines="None" CssClass="display">
                <Columns>
                    <asp:BoundField DataField="feriado_fecha" HeaderText="Fecha" DataFormatString="{0:D}" ReadOnly="true" />
                    <asp:BoundField DataField="feriado_descripcion" HeaderText="Descripción" ReadOnly="true" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <button runat="server" class="btn btn-sm btn-default" id="btn_ver" causesvalidation="false" onserverclick="btn_ver_Click" data-id='<%#Eval("feriado_id")%>'>
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
                    <button type="button" class="btn btn-default pull-right" id="btn_agregar_feriado" runat="server" data-toggle="modal" data-target="#agregar_feriado">
                        <span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span>Agregar nuevo
                    </button>
                    <div class="modal fade" id="agregar_feriado" role="dialog" aria-hidden="true">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                    <h4 class="modal-title">Agregar feriado - asueto</h4>
                                </div>
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:ValidationSummary ID="validation_summary" runat="server" DisplayMode="BulletList"
                                                CssClass="validationsummary panel panel-danger" HeaderText="<div class='panel-heading'>&nbsp;Corrija los siguientes errores antes de continuar:</div>" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <table class="table-condensed" style="width: 100%">
                                                <tr>
                                                    <td>Fecha</td>
                                                    <td style="width: auto">
                                                        <div id="dtp_fecha_feriado" class="input-group date">
                                                            <input type="text" runat="server" id="tb_fecha_feriado" class="form-control" />
                                                            <span class="input-group-addon">
                                                                <span class="glyphicon glyphicon-calendar"></span>
                                                            </span>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <asp:RequiredFieldValidator ControlToValidate="tb_fecha_feriado" Text="<img src='../img/exclamation.gif' title='Debe ingresar la fecha del feriado - asueto' />"
                                                            ID="rv_fecha" runat="server" ErrorMessage="Debe ingresar la fecha del feriado - asueto">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:CustomValidator ErrorMessage="Debe ingresar una fecha válida" Text="<img src='../img/exclamation.gif' title='Debe ingresar una fecha válida' />" OnServerValidate="c_fecha_ServerValidate" ID="c_fecha" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>

                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <table class="table-condensed" style="width: 100%">
                                                <tr>
                                                    <td>Descripción</td>
                                                    <td style="width: auto">
                                                        <input type="text" id="tb_descripcion" class="form-control" runat="server" placeholder="Descripcion del feriado - asueto" /></td>
                                                    <td>
                                                        <asp:RequiredFieldValidator ControlToValidate="tb_descripcion" Text="<img src='../img/exclamation.gif' title='Debe ingresar la descripción del feriado - asueto' />"
                                                            ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe ingresar la descripción del feriado - asueto">
                                                        </asp:RequiredFieldValidator></td>
                                                </tr>
                                            </table>

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

        <div class="modal fade" id="ver_feriado" role="dialog" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Datos Feriado</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12">
                                <p>
                                    <label>Fecha</label>
                                    <asp:Label Text="" ID="lbl_fecha" runat="server" />
                                </p>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <p>
                                    <label>Descripción</label>
                                    <asp:Label Text="" ID="lbl_descripcion" runat="server" />
                                </p>
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
            $('#<%= gv_feriados.ClientID %>').DataTable({
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
            $('#<%= gv_feriados_view.ClientID %>').DataTable({
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

        $(function () {
            $('#dtp_fecha_feriado').datetimepicker({
                locale: 'es',
                format: 'dddd D [de] MMMM [de] YYYY'
            });
        });

    </script>
</asp:Content>
