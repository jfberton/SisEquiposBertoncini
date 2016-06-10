<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="admin_empleados.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.admin_empleados" %>

<%@ Register Src="~/Aplicativo/Menues/menu_admin.ascx" TagPrefix="uc1" TagName="menu_admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_menu" runat="server">
    <uc1:menu_admin runat="server" ID="menu_admin" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_body" runat="server">

    <div class="panel panel-default">
        <div class="panel-heading">
            <div class="row">
                <div class="col-md-5">
                    <h4 class="panel-title">Empleados</h4>
                </div>
                <div class="col-md-7">
                    <div class="input-group">
                        <span class="input-group-addon">Buscar</span>
                        <input name="txtTerm" class="form-control" onkeyup="filtrar(this, '<%=gv_empleados.ClientID %>')" placeholder="ingrese texto buscado" type="text" />
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-body" style="height: 300px; overflow-y: scroll;">
            <asp:GridView ID="gv_empleados" runat="server" EmptyDataText="No existen empleados para mostrar." OnRowDataBound="gv_empleados_RowDataBound"
                AutoGenerateColumns="False" GridLines="None" CssClass="table table-condensed table-bordered">
                <Columns>
                    <asp:BoundField DataField="empleado_area" HeaderText="Area" ReadOnly="true" />
                    <asp:BoundField DataField="empleado_categoria" HeaderText="Categoría" ReadOnly="true" />
                    <asp:BoundField DataField="empleado_nombre" HeaderText="Nombre y apellido" ReadOnly="true" />
                    <asp:BoundField DataField="empleado_dni" HeaderText="D.N.I." ReadOnly="true" />
                    <asp:BoundField DataField="empleado_fecha_nacimiento" HeaderText="Fecha de nacimiento" DataFormatString="{0:d}" ReadOnly="true" />
                    <asp:BoundField DataField="empleado_fecha_alta" HeaderText="Fecha de alta" DataFormatString="{0:d}" ReadOnly="true" />
                    <asp:BoundField DataField="empleado_fecha_baja" HeaderText="Fecha de baja" ItemStyle-Font-Italic="true" ItemStyle-ForeColor="DeepPink" DataFormatString="{0:d}" ReadOnly="true" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <button
                                type="button" class="btn btn-sm btn-warning" id="btn_editar_empleado"
                                runat="server" onserverclick="btn_editar_empleado_ServerClick"
                                data-id='<%#Eval("empleado_id")%>'
                                causesvalidation="false">
                                <span class="glyphicon glyphicon-eye-open" aria-hidden="true"></span>&nbsp;Editar
                            </button>

                            <button
                                type="button" class="btn btn-sm btn-default"
                                data-toggle="modal"
                                data-target="#ver_empleado"
                                data-id='<%#Eval("empleado_id")%>'
                                data-area='<%#Eval("empleado_area")%>'
                                data-categoria='<%#Eval("empleado_categoria")%>'
                                data-dni='<%#Eval("empleado_dni")%>'
                                data-nacimiento='<%#Eval("empleado_fecha_nacimiento")%>'
                                data-nombre='<%#Eval("empleado_nombre")%>'>
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
                                    <input type="hidden" runat="server" id="id_empleado_por_eliminar" />
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
                    <button type="button" class="btn btn-default pull-right" causesvalidation="false" id="btn_agregar_empleado" runat="server" onserverclick="btn_agregar_empleado_ServerClick">
                        <span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span>Agregar nuevo
                    </button>

                    <div class="modal fade" id="agregar_empleado" role="dialog" aria-hidden="true">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                    <h4 class="modal-title">
                                        <asp:Label Text="Agregar empleado" ID="lbl_agregar_empleado_titulo" runat="server" /></h4>
                                    <input type="hidden" runat="server" id="id_empleado_hidden"  value="0" />
                                </div>
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:ValidationSummary ID="validation_summary" runat="server" DisplayMode="BulletList"
                                                CssClass="validationsummary panel panel-danger" HeaderText="<div class='panel-heading'>&nbsp;Corrija los siguientes errores antes de continuar:</div>" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <table class="table-condensed" style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <label>Área</label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList runat="server" Width="100%" CssClass="form-control" ID="ddl_areas">
                                                                </asp:DropDownList></td>
                                                            <td>
                                                                <asp:CustomValidator ID="cv_area" runat="server" Text="<img src='../img/exclamation.gif' title='Debe seleccionar un área' />"
                                                                    ErrorMessage="Debe seleccionar un área" OnServerValidate="cv_area_ServerValidate">
                                                                </asp:CustomValidator></td>
                                                        </tr>
                                                    </table>

                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <table class="table-condensed" style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <label>Categoría</label></td>
                                                            <td>
                                                                <asp:DropDownList runat="server" Width="100%" CssClass="form-control" ID="ddl_categorias">
                                                                </asp:DropDownList></td>
                                                            <td>
                                                                <asp:CustomValidator ID="cv_categoria" runat="server" Text="<img src='../img/exclamation.gif' title='Debe seleccionar una categoría' />"
                                                                    ErrorMessage="Debe seleccionar una categoría" OnServerValidate="cv_categoria_ServerValidate">
                                                                </asp:CustomValidator>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <table class="table-condensed" style="width: 100%">
                                                        <tr>
                                                            <td>Nombre y apellido</td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div class="col-md-8">
                                                    <table class="table-condensed" style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <input type="text" runat="server" id="tb_nombre_empleado" class="form-control" /></td>
                                                            <td>
                                                                <asp:RequiredFieldValidator ControlToValidate="tb_nombre_empleado" Text="<img src='../img/exclamation.gif' title='Debe ingresar el nombre del empleado' />"
                                                                    ID="rv_nombre_empleado" runat="server" ErrorMessage="Debe ingresar el nombre del empleado">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <table class="table-condensed" style="width: 100%">
                                                        <tr>
                                                            <td>D.N.I.</td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div class="col-md-8">
                                                    <table class="table-condensed" style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <input type="text" runat="server" id="tb_dni_empleado" class="form-control dni" /></td>
                                                            <td>
                                                                <asp:RequiredFieldValidator ControlToValidate="tb_dni_empleado" Text="<img src='../img/exclamation.gif' title='Debe ingresar el D.N.I. del empleado' />"
                                                                    ID="rv_dni_empleado" runat="server" ErrorMessage="Debe ingresar el D.N.I. del empleado">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <table class="table-condensed" style="width: 100%">
                                                        <tr>
                                                            <td>Fecha de nacimiento</td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div class="col-md-8">
                                                    <table class="table-condensed" style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <div class="form-group">
                                                                    <div id="dtp_fecha_nacimiento_empleado" class="input-group date">
                                                                        <input type="text" runat="server" id="tb_fecha_nacimiento_empleado" class="form-control ddmmaaaa" />
                                                                        <span class="input-group-addon">
                                                                            <span class="glyphicon glyphicon-calendar"></span>
                                                                        </span>
                                                                    </div>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <asp:CustomValidator ID="cv_fecha_nacimiento" runat="server" Text="<img src='../img/exclamation.gif' title='Debe ingresar una fecha de nacimiento válida' />"
                                                                    ErrorMessage="Debe ingresar una fecha de nacimiento válida" OnServerValidate="cv_fecha_nacimiento_ServerValidate">
                                                                </asp:CustomValidator></td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <table class="table-condensed" style="width: 100%">
                                                        <tr>
                                                            <td>Fecha de alta</td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div class="col-md-8">
                                                    <table class="table-condensed" style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <div class="form-group">
                                                                    <div id="dtp_fecha_alta_empleado" class="input-group date">
                                                                        <input type="text" runat="server" id="tb_fecha_alta_empleado" class="form-control ddmmaaaa" />
                                                                        <span class="input-group-addon">
                                                                            <span class="glyphicon glyphicon-calendar"></span>
                                                                        </span>
                                                                    </div>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <asp:CustomValidator ID="cv_fecha_alta" runat="server" Text="<img src='../img/exclamation.gif' title='Debe ingresar una fecha de alta válida' />"
                                                                    ErrorMessage="Debe ingresar una fecha de alta válida" OnServerValidate="cv_fecha_alta_ServerValidate">
                                                                </asp:CustomValidator></td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                
                                            </div>
                                        </div>
                                    </div><div class="row">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <table class="table-condensed" style="width: 100%">
                                                        <tr>
                                                            <td>Fecha de baja</td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div class="col-md-8">
                                                    <table class="table-condensed" style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <div class="form-group alert-danger">
                                                                    <div id="dtp_fecha_baja_empleado" class="input-group date">
                                                                        <input type="text" runat="server" id="tb_fecha_baja_empleado" class="form-control alert-danger ddmmaaaa" />
                                                                        <span class="input-group-addon">
                                                                            <span class="glyphicon glyphicon-calendar"></span>
                                                                        </span>
                                                                    </div>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <asp:CustomValidator ID="cv_fecha_baja" runat="server" Text="<img src='../img/exclamation.gif' title='Debe ingresar una fecha de alta válida' />"
                                                                    ErrorMessage="Debe ingresar una fecha de alta válida" OnServerValidate="cv_fecha_baja_ServerValidate">
                                                                </asp:CustomValidator></td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
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

        <div class="modal fade" id="ver_empleado" role="dialog" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Datos empleado seleccionado</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-6">
                                <p>
                                    <label>Area</label>
                                    <asp:Label Text="" ID="lbl_area_empleado" runat="server" />
                                </p>
                            </div>
                            <div class="col-md-6">
                                <p>
                                    <label>Categoría</label>
                                    <asp:Label Text="" ID="lbl_categoria_empleado" runat="server" />
                                </p>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <p>
                                    <label>Nombre y apellido</label>
                                    <asp:Label Text="" ID="lbl_nombre_empleado" runat="server" />
                                </p>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <p>
                                    <label>D.N.I.</label>
                                    <asp:Label Text="" ID="lbl_dni_empleado" runat="server" />
                                </p>
                            </div>
                            <div class="col-md-6">
                                <p>
                                    <label>Fecha nacimiento</label>
                                    <asp:Label Text="" ID="lbl_fecha_nacimiento_empleado" runat="server" />
                                </p>
                            </div>
                        </div>
                         <div class="row">
                            <div class="col-md-6">
                                <p>
                                    <label>Fecha alta</label>
                                    <asp:Label Text="" ID="lbl_fecha_alta" runat="server" />
                                </p>
                            </div>
                            <div class="col-md-6">
                                <p>
                                    <label>Fecha baja</label>
                                    <asp:Label Text="" ID="lbl_fecha_baja" runat="server" />
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>


    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cph_scripts" runat="server">
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
            modal.find('.modal-body #' + '<%= id_empleado_por_eliminar.ClientID %>').val(id)
            modal.find('.modal-body #texto_a_mostrar').text('Esta por eliminar ' + introduccion + ' ' + nombre + '. Desea continuar?')
        })

        $('#ver_empleado').on('show.bs.modal', function (event) {
            // Button that triggered the modal
            var button = $(event.relatedTarget)

            // Extract info from data-* attributes
            var id = button.data('id')
            var area = button.data('area')
            var categoria = button.data('categoria')
            var dni = button.data('dni')
            var nacimiento = button.data('nacimiento')
            var nombre = button.data('nombre')

            // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
            var modal = $(this)
            modal.find('.modal-body #' + '<%= lbl_area_empleado.ClientID %>').text(area)
            modal.find('.modal-body #' + '<%= lbl_categoria_empleado.ClientID %>').text(categoria)
            modal.find('.modal-body #' + '<%= lbl_nombre_empleado.ClientID %>').text(nombre)
            modal.find('.modal-body #' + '<%= lbl_dni_empleado.ClientID %>').text(dni)
            modal.find('.modal-body #' + '<%= lbl_fecha_nacimiento_empleado.ClientID %>').text(nacimiento.substring(0, 11))
        })
    </script>

    <script>
        $(function () {
            $('#dtp_fecha_nacimiento_empleado').datetimepicker({
                locale: 'es',
                format: 'DD/MM/YYYY'
            });

            $('#dtp_fecha_alta_empleado').datetimepicker({
                locale: 'es',
                format: 'DD/MM/YYYY'
            });

            $('#dtp_fecha_baja_empleado').datetimepicker({
                locale: 'es',
                format: 'DD/MM/YYYY'
            });
        });

        $(document).ready(function () {
            $('.dni').mask('00.000.000');
            $('.ddmmaaaa').mask('00/00/0000');
        });

        function filtrar(phrase, _id) {
            var words = phrase.value.toLowerCase().split(" ");
            var table = document.getElementById(_id);
            var ele;
            for (var r = 1; r < table.rows.length; r++) {
                ele = table.rows[r].innerHTML.replace(/<[^>]+>/g, "");
                var displayStyle = 'none';
                for (var i = 0; i < words.length; i++) {
                    if (ele.toLowerCase().indexOf(words[i]) >= 0)
                        displayStyle = '';
                    else {
                        displayStyle = 'none';
                        break;
                    }
                }
                table.rows[r].style.display = displayStyle;
            }
        }
    </script>
</asp:Content>
