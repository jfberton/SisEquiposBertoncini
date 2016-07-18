<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="admin_horas_mes.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.admin_horas_mes" %>

<%@ Register Src="~/Aplicativo/Menues/menu_admin.ascx" TagPrefix="uc1" TagName="menu_admin" %>
<%@ Register Src="~/Aplicativo/Menues/menu_usuario.ascx" TagPrefix="uc1" TagName="menu_usuario" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_menu" runat="server">
    <uc1:menu_admin runat="server" ID="menu_admin" />
    <uc1:menu_usuario runat="server" ID="menu_usuario" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_body" runat="server">
    <div class="panel panel-default">
        <div class="panel-heading">
            <h1 class="panel-title">Valores mensuales empleado</h1>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-6">
                    <div class="row">
                        <div class="col-md-5">
                            <div class="row">
                                <div class="col-md-2">
                                    <table class="table-condensed">
                                        <tr>
                                            <td>Tipo</td>
                                        </tr>
                                    </table>

                                </div>
                                <div class="col-md-10">
                                    <table class="table-condensed">
                                        <tr>
                                            <td>
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddl_tipo_empleado" AutoPostBack="true" OnSelectedIndexChanged="ddl_tipo_empleado_SelectedIndexChanged">
                                                </asp:DropDownList></td>
                                        </tr>
                                    </table>

                                </div>
                            </div>
                        </div>
                        <div class="col-md-7">
                            <div class="row">
                                <div class="col-md-3">
                                    <table class="table-condensed">
                                        <tr>
                                            <td>Empleado</td>
                                        </tr>
                                    </table>

                                </div>
                                <div class="col-md-9">
                                    <table class="table-condensed">
                                        <tr>
                                            <td>
                                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddl_empleado">
                                                </asp:DropDownList></td>
                                        </tr>
                                    </table>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="row">
                        <div class="col-md-1">
                            <table class="table-condensed">
                                <tr>
                                    <td>Mes</td>
                                </tr>
                            </table>

                        </div>
                        <div class="col-md-6">
                            <table class="table-condensed">
                                <tr>
                                    <td>
                                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddl_mes">
                                        </asp:DropDownList></td>
                                </tr>
                            </table>

                        </div>
                        <div class="col-md-1">
                            <table class="table-condensed">
                                <tr>
                                    <td>Año</td>
                                </tr>
                            </table>

                        </div>
                        <div class="col-md-4">
                            <table class="table-condensed">
                                <tr>
                                    <td>
                                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddl_anio">
                                        </asp:DropDownList></td>
                                </tr>
                            </table>

                        </div>
                    </div>
                </div>
                <div class="col-md-2">
                    <asp:Button Text="Buscar" runat="server" CssClass="btn btn-default btn-block" ID="btn_buscar" OnClick="btn_buscar_Click" />
                    <asp:Button Text="Nueva búsqueda" runat="server" CssClass="btn btn-warning btn-block" ID="btn_nueva_busqueda" OnClick="btn_nueva_busqueda_Click" />
                </div>
            </div>
            <br />
            <div runat="server" id="div_valores_mes">
                <div class="row">
                    <div class="col-md-12">
                        <asp:GridView ID="gv_valores_mes" runat="server" EmptyDataText="no hay valores por mostrar." OnRowDataBound="gv_valores_mes_RowDataBound"
                            AutoGenerateColumns="False" GridLines="None" CssClass="table table-condensed table-bordered">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <span class='<%#Eval("icon_alerta")%>' aria-hidden="true" title='<%#Eval("tooltip_alerta")%>'></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="dia" HeaderText="Día" ReadOnly="true" DataFormatString="{0:dddd', ' dd 'de' MMMM 'del' yyyy}" />
                                <asp:BoundField DataField="estado_tm" HeaderText="T. M." ReadOnly="true" />
                                <asp:BoundField DataField="estado_tt" HeaderText="T. T." ReadOnly="true" />
                                <asp:BoundField DataField="horas_normales" HeaderText="H. N." ReadOnly="true" />
                                <asp:BoundField DataField="horas_extra_cincuenta" HeaderText="H. E. 50%" ReadOnly="true" />
                                <asp:BoundField DataField="horas_extra_cien" HeaderText="H. E. 100%" ReadOnly="true" />
                                <asp:BoundField DataField="horas_totales_dia" HeaderText="Horas Totales" ReadOnly="true" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <button runat="server" class="btn btn-sm btn-default" id="btn_ver" causesvalidation="false" onserverclick="btn_ver_ServerClick" data-id='<%#Eval("id_dia")%>'>
                                            <span class="glyphicon glyphicon-eye-open" aria-hidden="true"></span>&nbsp;Ver - Editar
                                        </button>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                        <div class="modal fade" id="div_valores_dia" role="dialog" aria-hidden="true">
                            <div class="modal-dialog modal-lg">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                        <h4>Detalle del
                                            <asp:Label Text="" ID="lbl_fecha" runat="server" /></h4>
                                    </div>
                                    <div class="modal-body">
                                        <div runat="server" id="div_error_detalle">
                                            <div class="alert alert-danger">
                                                <p runat="server" id="p_error"></p>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-2">
                                                <table class="table-condensed">
                                                    <tr>
                                                        <td>Turno mañana</td>
                                                    </tr>
                                                </table>

                                            </div>
                                            <div class="col-md-4">
                                                <table class="table-condensed" style="width:100%">
                                                    <tr>
                                                        <td>
                                                            <asp:DropDownList runat="server" CssClass="form-control" ID="ddl_estado_turno_m" Width="100%">
                                                            </asp:DropDownList></td>
                                                    </tr>
                                                </table>

                                            </div>
                                            <div class="col-md-2">
                                                <table class="table-condensed">
                                                    <tr>
                                                        <td>Turno Tarde</td>
                                                    </tr>
                                                </table>

                                            </div>
                                            <div class="col-md-4">
                                                <table class="table-condensed" style="width:100%">
                                                    <tr>
                                                        <td>
                                                            <asp:DropDownList runat="server" CssClass="form-control" ID="ddl_estado_turno_t" Width="100%">
                                                            </asp:DropDownList></td>
                                                    </tr>
                                                </table>

                                            </div>
                                        </div>
                                        <h4><small>Detalle de horas trabajadas</small></h4>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="row">
                                                    <div class="col-md-3">
                                                        <table class="table-condensed">
                                                            <tr>
                                                                <td>Equipo</td>
                                                            </tr>
                                                        </table>

                                                    </div>
                                                    <div class="col-md-9">
                                                        <table class="table-condensed" style="width:100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList runat="server" CssClass="form-control" Width="100%" ID="ddl_equipo" >
                                                                    </asp:DropDownList></td>
                                                            </tr>
                                                        </table>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="row">
                                                    <div class="col-md-2">
                                                        <table class="table-condensed">
                                                            <tr>
                                                                <td>Desde</td>
                                                            </tr>
                                                        </table>

                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="form-group">
                                                            <div id="dtp_hora_desde" class="input-group date">
                                                                <input type="text" runat="server" id="tb_hora_desde" class="form-control" />
                                                                <span class="input-group-addon">
                                                                    <span class="glyphicon glyphicon-time"></span>
                                                                </span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <table class="table-condensed">
                                                            <tr>
                                                                <td>Hasta</td>
                                                            </tr>
                                                        </table>

                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="form-group">
                                                            <div id="dtp_hora_hasta" class="input-group date">
                                                                <input type="text" runat="server" id="tb_hora_hasta" class="form-control" />
                                                                <span class="input-group-addon">
                                                                    <span class="glyphicon glyphicon-time"></span>
                                                                </span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Button Text="Agregar" CssClass="btn btn-default btn-block" ID="btn_agregar_detalle_dia" OnClick="btn_agregar_detalle_dia_Click" runat="server" />
                                            </div>
                                        </div>

                                        <br />
                                        <div class="row" style="height: 200px; overflow-y: scroll;">
                                            <div class="col-md-12">
                                                <asp:GridView ID="gv_detalle_dia" runat="server" EmptyDataText="no hay valores por mostrar." OnRowDataBound="gv_detalle_dia_RowDataBound"
                                                    AutoGenerateColumns="False" GridLines="None" CssClass="table table-condensed table-bordered">
                                                    <Columns>
                                                        <asp:BoundField DataField="equipo" HeaderText="Equipo" ReadOnly="true" />
                                                        <asp:BoundField DataField="desde" HeaderText="Desde" ReadOnly="true" />
                                                        <asp:BoundField DataField="hasta" HeaderText="Hasta" ReadOnly="true" />
                                                        <asp:BoundField DataField="total_movimiento" HeaderText="Total movimiento" ReadOnly="true" />
                                                        <asp:TemplateField HeaderText="OUT">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbSelect" Checked='<%#Eval("_out")%>' runat="server" Enabled="false"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <button
                                                                    type="button" class="btn btn-sm btn-danger"
                                                                    data-toggle="modal"
                                                                    data-target="#advertencia_eliminacion"
                                                                    data-introduccion="el movimiento"
                                                                    data-nombre='<%#String.Concat(Eval("equipo")," desde: ",Eval("desde"), " hasta: ", Eval("hasta"))%>'
                                                                    data-id='<%#Eval("id_detalle_dia")%>'
                                                                    data-id_equipo='<%#Eval("id_equipo")%>'
                                                                    data-desde='<%#Eval("desde")%>'
                                                                    data-hasta='<%#Eval("hasta")%>'>
                                                                    <span class="glyphicon glyphicon-remove-circle" aria-hidden="true"></span>Eliminar
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
                                                                        <input type="hidden" runat="server" id="hidden_id_detalle_dia" />
                                                                        <input type="hidden" runat="server" id="hidden_id_equipo" />
                                                                        <input type="hidden" runat="server" id="hidden_desde" />
                                                                        <input type="hidden" runat="server" id="hidden_hasta" />
                                                                        <p id="texto_a_mostrar"></p>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="modal-footer">
                                                                <button runat="server" class="btn btn-danger" id="btn_eliminar_detalle" causesvalidation="false" onserverclick="btn_eliminar_detalle_ServerClick">
                                                                    Aceptar
                                                                </button>
                                                                <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                        <h4><small>Horas totales del día</small></h4>
                                        <div class="row">
                                            <div class="col-md-2">
                                                <label>Horas normales</label>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label Text="" ID="lbl_horas_normales" runat="server" />
                                            </div>
                                            <div class="col-md-2">
                                                <label>Horas extra 50%</label>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label Text="" ID="lbl_horas_extra_cincuenta" runat="server" />
                                            </div>
                                            <div class="col-md-3">
                                                <label>Horas extra 100%</label>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:Label Text="" ID="lbl_horas_extra_cien" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <button id="btn_guardar_detalle_dia" runat="server" onserverclick="btn_guardar_detalle_dia_ServerClick" class="btn btn-success">
                                            <span class="glyphicon glyphicon-floppy-disk" aria-hidden="true"></span>Guardar!
                                        </button>
                                        <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal fade" id="div_valores_dia_view" role="dialog" aria-hidden="true">
                            <div class="modal-dialog modal-lg">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                        <h4>Detalle del
                                            <asp:Label Text="" ID="lbl_fecha_view" runat="server" /></h4>
                                    </div>
                                    <div class="modal-body">
                                        <h4><small>Detalle de horas trabajadas</small></h4>
                                        <br />
                                        <div class="row" style="height: 200px; overflow-y: scroll;">
                                            <div class="col-md-12">
                                                <asp:GridView ID="gv_detalle_dia_view" runat="server" EmptyDataText="no hay valores por mostrar." OnRowDataBound="gv_detalle_dia_RowDataBound"
                                                    AutoGenerateColumns="False" GridLines="None" CssClass="table table-condensed table-bordered">
                                                    <Columns>
                                                        <asp:BoundField DataField="equipo" HeaderText="Equipo" ReadOnly="true" />
                                                        <asp:BoundField DataField="desde" HeaderText="Desde" ReadOnly="true" />
                                                        <asp:BoundField DataField="hasta" HeaderText="Hasta" ReadOnly="true" />
                                                        <asp:BoundField DataField="total_movimiento" HeaderText="Total movimiento" ReadOnly="true" />
                                                        <asp:TemplateField HeaderText="OUT">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbSelect" Checked='<%#Eval("_out")%>' runat="server" Enabled="false"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                        <h4><small>Horas totales del día</small></h4>
                                        <div class="row">
                                            <div class="col-md-2">
                                                <label>Horas normales</label>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label Text="" ID="lbl_horas_normales_view" runat="server" />
                                            </div>
                                            <div class="col-md-2">
                                                <label>Horas extra 50%</label>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label Text="" ID="lbl_horas_extra_cincuenta_view" runat="server" />
                                            </div>
                                            <div class="col-md-3">
                                                <label>Horas extra 100%</label>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:Label Text="" ID="lbl_horas_extra_cien_view" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
                <h3><small>Valores mes empleado</small></h3>
                <div class="row">
                    <div class="col-md-12">
                        <table class="table table-bordered">
                            <tr>
                                <td>
                                    <label>Días laborables</label></td>
                                <td>
                                    <asp:Label Text="" ID="lbl_dias_laborables" runat="server" /></td>
                                <td>
                                    <label>Total horas normales</label></td>
                                <td>
                                    <asp:Label Text="" ID="lbl_total_horas_normales" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Días ausentes</label></td>
                                <td>
                                    <asp:Label Text="" ID="lbl_dias_ausentes" runat="server" /></td>
                                <td>
                                    <label>Total horas extra 50%</label></td>
                                <td>
                                    <asp:Label Text="" ID="lbl_total_horas_extra_50" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Días presentes/vacaciones</label></td>
                                <td>
                                    <asp:Label Text="" ID="lbl_dias_presentes_vacaciones" runat="server" /></td>
                                <td>
                                    <label>Total horas extra 100%</label></td>
                                <td>
                                    <asp:Label Text="" ID="lbl_total_horas_extra_100" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Días por cargar</label></td>
                                <td>
                                    <asp:Label Text="" ID="lbl_días_por_cargar" runat="server" /></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Días OUT</label></td>
                                <td>
                                    <asp:Label Text="" ID="lbl_dias_out" runat="server" /></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Días presentes en días no laborables</label></td>
                                <td>
                                    <asp:Label Text="" ID="lbl_dias_presentes_en_dias_no_laborables" runat="server" /></td>
                                <td></td>
                                <td></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_scripts" runat="server">
    <script>
        $(function () {
            $('#dtp_hora_desde').datetimepicker({
                locale: 'es',
                format: 'LT'
            });
        });
        $(function () {
            $('#dtp_hora_hasta').datetimepicker({
                locale: 'es',
                format: 'LT'
            });
        });

        $('#advertencia_eliminacion').on('show.bs.modal', function (event) {
            // Button that triggered the modal
            var button = $(event.relatedTarget)

            // Extract info from data-* attributes
            var id = button.data('id')
            var introduccion = button.data('introduccion')
            var nombre = button.data('nombre')
            var id_equipo = button.data('id_equipo')
            var desde = button.data('desde')
            var hasta = button.data('hasta')

            // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
            var modal = $(this)
            modal.find('.modal-body #' + '<%= hidden_id_detalle_dia.ClientID %>').val(id)
            modal.find('.modal-body #' + '<%= hidden_id_equipo.ClientID %>').val(id_equipo)
            modal.find('.modal-body #' + '<%= hidden_desde.ClientID %>').val(desde)
            modal.find('.modal-body #' + '<%= hidden_hasta.ClientID %>').val(hasta)
            modal.find('.modal-body #texto_a_mostrar').text('Esta por eliminar ' + introduccion + ' ' + nombre + '. Desea continuar?')
        })

    </script>
</asp:Content>
