<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="admin_equipo_detalle.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.admin_equipo_detalle" %>

<%@ Register Src="~/Aplicativo/Menues/menu_admin.ascx" TagPrefix="uc1" TagName="menu_admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_menu" runat="server">
    <uc1:menu_admin runat="server" ID="menu_admin" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_body" runat="server">
    <div class="panel panel-default">
        <div class="panel-heading">
            <h1 class="panel-title">Datos equipo</h1>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-12">
                    <asp:ValidationSummary ID="validation_summary" runat="server" DisplayMode="BulletList" ValidationGroup="equipo"
                        CssClass="validationsummary panel panel-danger" HeaderText="<div class='panel-heading'>&nbsp;Corrija los siguientes errores antes de continuar:</div>" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <table class="table-condensed">
                        <tr>
                            <td>Nombre</td>
                        </tr>
                    </table>
                </div>
                <div class="col-md-10">
                    <table class="table-condensed">
                        <tr>
                            <td>
                                <input type="text" runat="server" id="tb_nombre" class="form-control" /></td>
                            <td>
                                <asp:RequiredFieldValidator ControlToValidate="tb_nombre" Text="<img src='../img/exclamation.gif' title='Debe ingresar el nombre del equipo' />"
                                    ID="rv_nombre" runat="server" ErrorMessage="Debe ingresar el nombre del equipo" ValidationGroup="equipo">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <table class="table-condensed">
                        <tr>
                            <td>Categoría</td>
                        </tr>
                    </table>
                </div>
                <div class="col-md-10">
                    <table class="table-condensed">
                        <tr>
                            <td>
                                <asp:DropDownList runat="server" ID="ddl_categorias" CssClass="form-control">
                                </asp:DropDownList></td>
                            <td>
                                <asp:CustomValidator ID="cv_categoria" runat="server" Text="<img src='../img/exclamation.gif' title='Debe seleccionar una categoría' />"
                                    ErrorMessage="Debe seleccionar una categoría" OnServerValidate="cv_categoria_ServerValidate" ValidationGroup="equipo">
                                </asp:CustomValidator>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <table class="table-condensed">
                        <tr>
                            <td>Notas:</td>
                        </tr>
                    </table>
                </div>
                <div class="col-md-10">
                    <table class="table-condensed" style="width: 100%">
                        <tr>
                            <td>
                                <textarea rows="5" class="form-control" runat="server" id="tb_notas_equipo" placeholder="Notas del equipo (no obligatorio)"></textarea></td>
                        </tr>
                    </table>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-md-2">
                                    <table class="table-condensed">
                                        <tr>
                                            <td>
                                                <h1 class="panel-title">Partes</h1>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="col-md-10">
                                    <table class="table-condensed">
                                        <tr>
                                            <td>
                                                <button type="button" class="btn btn-default btn-xs pull-right" id="btn_agregar_parte" data-toggle="modal" data-target="#agregar_parte">
                                                    <span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span>Agregar parte
                                                </button>
                                                <div class="modal fade" id="agregar_parte" role="dialog" aria-hidden="true">
                                                    <div class="modal-dialog">
                                                        <div class="modal-content">
                                                            <div class="modal-header">
                                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                                <h4 class="modal-title">Agregar parte componente del equipo</h4>
                                                            </div>
                                                            <div class="modal-body">
                                                                <div class="row">
                                                                    <div class="col-md-12">
                                                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="BulletList" ValidationGroup="parte"
                                                                            CssClass="validationsummary panel panel-danger" HeaderText="<div class='panel-heading'>&nbsp;Corrija los siguientes errores antes de continuar:</div>" />
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-md-4">
                                                                        <table class="table-condensed" style="width: 100%">
                                                                            <tr>
                                                                                <td>Nombre</td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <div class="col-md-8">
                                                                        <table class="table-condensed" style="width: 100%">
                                                                            <tr>
                                                                                <td style="width: auto">
                                                                                    <input type="text" id="tb_nombre_parte" class="form-control" runat="server" placeholder="Nombre de la parte" /></td>
                                                                                <td>
                                                                                    <asp:RequiredFieldValidator ControlToValidate="tb_nombre_parte" Text="<img src='../img/exclamation.gif' title='Debe ingresar el nombre de la parte' />"
                                                                                        ID="rv_nombre_parte" runat="server" ErrorMessage="Debe ingresar el nombre de la parte" ValidationGroup="parte">
                                                                                    </asp:RequiredFieldValidator></td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </div>
                                                                <br />
                                                                <div class="row">
                                                                    <div class="col-md-6">
                                                                        <table class="table-condensed" style="width: 100%">
                                                                            <tr>
                                                                                <td>Costo 0Km. (USS)</td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <table class="table-condensed" style="width: 100%">
                                                                            <tr>
                                                                                <td style="width: auto">
                                                                                    <input type="text" id="tb_costo_cero_km" class="form-control" runat="server" placeholder="Costo 0Km" /></td>
                                                                                <td>
                                                                                    <asp:RequiredFieldValidator ControlToValidate="tb_costo_cero_km" Text="<img src='../img/exclamation.gif' title='Debe ingresar el costo de la parte' />"
                                                                                        ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe ingresar el costo de la parte" ValidationGroup="parte">
                                                                                    </asp:RequiredFieldValidator>
                                                                                    <asp:CustomValidator ID="cv_costo_cero_km" runat="server" Text="<img src='../img/exclamation.gif' title='Debe ingresar un monto válido' />"
                                                                                        ErrorMessage="Debe ingresar un monto válido" OnServerValidate="cv_costo_cero_km_ServerValidate" ValidationGroup="parte">
                                                                                    </asp:CustomValidator>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </div>
                                                                <br />
                                                                <div class="row">
                                                                    <div class="col-md-6">
                                                                        <table class="table-condensed" style="width: 100%">
                                                                            <tr>
                                                                                <td>Usado (%) 100% = nuevo</td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <table class="table-condensed" style="width: 100%">
                                                                            <tr>
                                                                                <td style="width: auto">
                                                                                    <input type="text" id="tb_porcentaje_usado" class="form-control" runat="server" placeholder="Porcentaje usado" /></td>
                                                                                <td>
                                                                                    <asp:RequiredFieldValidator ControlToValidate="tb_porcentaje_usado" Text="<img src='../img/exclamation.gif' title='Debe ingresar el porcentaje de usado' />"
                                                                                        ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe ingresar el porcentaje de usado" ValidationGroup="parte">
                                                                                    </asp:RequiredFieldValidator>
                                                                                    <asp:CustomValidator ID="cv_porcentaje_usado" runat="server" Text="<img src='../img/exclamation.gif' title='Debe ingresar un porcentaje válido' />"
                                                                                        ErrorMessage="Debe ingresar un porcentaje válido" OnServerValidate="cv_porcentaje_usado_ServerValidate" ValidationGroup="parte">
                                                                                    </asp:CustomValidator>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </div>
                                                                <br />
                                                                <div class="row">
                                                                    <div class="col-md-6">
                                                                        <table class="table-condensed" style="width: 100%">
                                                                            <tr>
                                                                                <td>Valor residual (%)</td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <table class="table-condensed" style="width: 100%">
                                                                            <tr>
                                                                                <td style="width: auto">
                                                                                    <input type="text" id="tb_porcentaje_valor_residual" class="form-control" runat="server" placeholder="Valor recidual " /></td>
                                                                                <td>
                                                                                    <asp:RequiredFieldValidator ControlToValidate="tb_porcentaje_valor_residual" Text="<img src='../img/exclamation.gif' title='Debe ingresar el porcentaje de valor residual' />"
                                                                                        ID="RequiredFieldValidator3" runat="server" ErrorMessage="Debe ingresar el porcentaje de valor residual" ValidationGroup="parte">
                                                                                    </asp:RequiredFieldValidator>
                                                                                    <asp:CustomValidator ID="cv_valor_residual" runat="server" Text="<img src='../img/exclamation.gif' title='Debe ingresar un porcentaje válido' />"
                                                                                        ErrorMessage="Debe ingresar un porcentaje válido" OnServerValidate="cv_valor_residual_ServerValidate" ValidationGroup="parte">
                                                                                    </asp:CustomValidator>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </div>
                                                                <br />
                                                                <div class="row">
                                                                    <div class="col-md-6">
                                                                        <table class="table-condensed" style="width: 100%">
                                                                            <tr>
                                                                                <td>Periodo alta</td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <table class="table-condensed" style="width: 100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <div class="form-group">
                                                                                        <div id="dtp_periodo_alta" class="input-group date">
                                                                                            <input type="text" runat="server" id="tb_periodo_alta" class="form-control" />
                                                                                            <span class="input-group-addon">
                                                                                                <span class="glyphicon glyphicon-calendar"></span>
                                                                                            </span>
                                                                                        </div>
                                                                                    </div>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:CustomValidator ID="cv_periodo_alta" runat="server" Text="<img src='../img/exclamation.gif' title='Debe ingresar una fecha de nacimiento válida' />"
                                                                                        ErrorMessage="Debe ingresar una fecha de nacimiento válida" OnServerValidate="cv_periodo_alta_ServerValidate">
                                                                                    </asp:CustomValidator></td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </div>

                                                                <br />
                                                                <div class="row">
                                                                    <div class="col-md-6">
                                                                        <table class="table-condensed" style="width: 100%">
                                                                            <tr>
                                                                                <td>Meses por amortizar</td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <table class="table-condensed" style="width: 100%">
                                                                            <tr>
                                                                                <td style="width: auto">
                                                                                    <input type="text" id="tb_meses_por_amortizar" class="form-control" runat="server" placeholder="Meses por amortizar" /></td>
                                                                                <td>
                                                                                    <asp:RequiredFieldValidator ControlToValidate="tb_meses_por_amortizar" Text="<img src='../img/exclamation.gif' title='Debe ingresar los meses por amortizar' />"
                                                                                        ID="RequiredFieldValidator4" runat="server" ErrorMessage="Debe ingresar los meses por amortizar" ValidationGroup="parte">
                                                                                    </asp:RequiredFieldValidator>
                                                                                    <asp:CustomValidator ID="cv_meses_por_amortizar" runat="server" Text="<img src='../img/exclamation.gif' title='Debe ingresar un número de meses válido' />"
                                                                                        ErrorMessage="Debe ingresar un número de meses válido" OnServerValidate="cv_meses_por_amortizar_ServerValidate" ValidationGroup="parte">
                                                                                    </asp:CustomValidator>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </div>
                                                                <br />

                                                            </div>
                                                            <div class="modal-footer">
                                                                <asp:Button Text="Aceptar" ID="btn_aceptar_parte" OnClick="btn_aceptar_parte_Click" ValidationGroup="parte" CssClass="btn btn-success" runat="server" />
                                                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:GridView ID="gv_partes" runat="server" EmptyDataText="No existen partes del equipo por mostrar." OnRowDataBound="gv_partes_RowDataBound"
                                        AutoGenerateColumns="False" GridLines="None" CssClass="table table-condensed table-bordered">
                                        <Columns>
                                            <asp:BoundField DataField="nombre_parte" HeaderText="Nombre" ReadOnly="true" ItemStyle-Width="100px" />
                                            <asp:BoundField DataField="costo_cero" HeaderText="Costo 0km" ReadOnly="true" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="porcentaje_usado" HeaderText="Usado" ReadOnly="true" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="porcentaje_valor_residual" HeaderText="Valor recidual" ReadOnly="true" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="valor_por_amortizar" HeaderText="Valor por amortizar" ReadOnly="true" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="periodo_alta" HeaderText="Periodo alta" ReadOnly="true" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="meses_por_amortizar" HeaderText="Meses por amortizar" ReadOnly="true" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="meses_amortizados" HeaderText="Meses amortizados" ReadOnly="true" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="restan_amortizar" HeaderText="Restan amortizar" ReadOnly="true" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="costo_mensual" HeaderText="Costo mensual" ReadOnly="true" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <button
                                                        type="button" class="btn btn-sm btn-danger"
                                                        data-toggle="modal"
                                                        data-target="#advertencia_eliminacion"
                                                        data-id='<%#Eval("id_parte")%>'
                                                        data-introduccion="la parte"
                                                        data-nombre='<%#Eval("nombre_parte")%>'>
                                                        <span class="glyphicon glyphicon-remove-circle" aria-hidden="true"></span> Eliminar
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
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <label>Total por amortizar de las partes del equipo</label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <table class="table">
                                        <tr>
                                            <td>
                                                <label>Costo total 0Km</label></td>
                                            <td>
                                                <asp:Label Text="" ID="lbl_costo_total_0km" runat="server" /></td>
                                            <td>
                                                <label>Total por amortizar</label></td>
                                            <td>
                                                <asp:Label Text="" ID="lbl_valor_por_amortizar" runat="server" /></td>
                                            <td>
                                                <label>Costo mensual</label></td>
                                            <td>
                                                <asp:Label Text="" ID="lbl_costo_mensual" runat="server" /></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-footer text-right">
            <button id="btn_guardar_equipo" runat="server" onserverclick="btn_guardar_equipo_ServerClick" class="btn btn-success" validationgroup="equipo">
                <span class="glyphicon glyphicon-floppy-disk" aria-hidden="true"></span>Guardar!
            </button>
            <asp:Button Text="Cancelar" CssClass="btn btn-default" ID="btn_cancelar" OnClick="btn_cancelar_Click" runat="server" />
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
            modal.find('.modal-body #' + '<%= id_item_por_eliminar.ClientID %>').val(id + '-' + nombre)
            modal.find('.modal-body #texto_a_mostrar').text('Esta por eliminar ' + introduccion + ' ' + nombre + '. Desea continuar?')
        })
    </script>

    <script>
        $(function () {
            $('#dtp_periodo_alta').datetimepicker({
                locale: 'es',
                format: 'MM/YYYY'
            });
        });
    </script>
</asp:Content>
