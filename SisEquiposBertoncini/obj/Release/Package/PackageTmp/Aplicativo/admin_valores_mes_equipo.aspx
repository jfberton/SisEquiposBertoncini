<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="admin_valores_mes_equipo.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.admin_valores_mes_equipo" %>

<%@ Register Src="~/Aplicativo/Menues/menu_admin.ascx" TagPrefix="uc1" TagName="menu_admin" %>
<%@ Register Src="~/Aplicativo/Menues/menu_usuario.ascx" TagPrefix="uc1" TagName="menu_usuario" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/jquery.treegrid.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_menu" runat="server">
    <uc1:menu_admin runat="server" ID="menu_admin" />
    <uc1:menu_usuario runat="server" ID="menu_usuario" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_body" runat="server">
    <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server"></asp:ScriptManager>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h1 class="panel-title">Cargar - editar valores mensuales del equipo
            </h1>
        </div>
        <div class="panel-body">
            <div class="row" runat="server" id="row_busqueda">
                <div class="col-md-5">
                    <table class="table-condensed" style="width: 100%">
                        <tr>
                            <td>Equipo</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddl_equipo" CssClass="form-control">
                                </asp:DropDownList></td>
                        </tr>
                    </table>
                </div>
                <div class="col-md-7">
                    <div class="row">
                        <div class="col-md-5">
                            <table class="table-condensed" style="width: 100%">
                                <tr>
                                    <td>Mes</td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddl_mes" CssClass="form-control">
                                            <asp:ListItem Value="1" Text="Enero"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Febrero"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Marzo"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Abril"></asp:ListItem>
                                            <asp:ListItem Value="5" Text="Mayo"></asp:ListItem>
                                            <asp:ListItem Value="6" Text="Junio"></asp:ListItem>
                                            <asp:ListItem Value="7" Text="Julio"></asp:ListItem>
                                            <asp:ListItem Value="8" Text="Agosto"></asp:ListItem>
                                            <asp:ListItem Value="9" Text="Septimbre"></asp:ListItem>
                                            <asp:ListItem Value="10" Text="Octubre"></asp:ListItem>
                                            <asp:ListItem Value="11" Text="Noviembre"></asp:ListItem>
                                            <asp:ListItem Value="12" Text="Diciembre"></asp:ListItem>
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
                                        <asp:Button Text="Nueva búsqueda" runat="server" ID="btn_nueva_busqueda" CssClass="btn btn-danger" OnClick="btn_nueva_busqueda_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>

            </div>
            <br />
            <div class="row">

                <div class="col-md-12" runat="server" id="div_buscar_primero">
                    <div class="alert alert-warning alert-dismissible" role="alert">
                        <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <small>Seleccione los campos <strong>Equipo, mes y año</strong> y luego presione obtener.</small>
                    </div>
                </div>
                <div class="col-md-12" runat="server" id="div_tree">
                    
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="ver_detalle_item_mes" role="dialog" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">Detalle de montos cargados</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12">
                            <h2>
                                <asp:Label Text="" ID="lbl_item" runat="server" />
                                <small>
                                    <label>Categoría</label>
                                    <asp:Label Text="" ID="lbl_categoria" runat="server" /></small></h2>
                            <asp:Label Text="" ID="lbl_mes" runat="server" />
                        </div>
                    </div>
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
                                    <td>Fecha</td>
                                </tr>
                            </table>
                        </div>
                        <div class="col-md-8">
                            <table class="table-condensed" style="width: 100%">
                                <tr>
                                    <td>
                                        <div class="form-group">
                                            <div id="dtp_fecha" class="input-group date">
                                                <input type="text" runat="server" id="tb_detalle_fecha" class="form-control ddmmaaaa" />
                                                <span class="input-group-addon">
                                                    <span class="glyphicon glyphicon-calendar"></span>
                                                </span>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:CustomValidator ID="cv_fecha" runat="server" Text="<img src='../img/exclamation.gif' title='Debe ingresar una fecha válida dentro del mes en cuestión' />"
                                            ErrorMessage="Debe ingresar una fecha válida dentro del mes en cuestión" OnServerValidate="cv_fecha_ServerValidate">
                                        </asp:CustomValidator></td>
                                </tr>
                            </table>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <table class="table-condensed" style="width: 100%">
                                    <tr>
                                        <td>Monto</td>
                                    </tr>
                                </table>
                            </div>
                            <div class="col-md-8">
                                <table class="table-condensed" style="width: 100%">
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="tb_detalle_monto" />
                                        </td>
                                        <td>
                                            <asp:CustomValidator ID="cv_monto" runat="server" Text="<img src='../img/exclamation.gif' title='Debe ingresar un monto válido' />"
                                                ErrorMessage="Debe ingresar un monto válido" OnServerValidate="cv_monto_ServerValidate">
                                            </asp:CustomValidator></td>
                                    </tr>
                                </table>
                            </div>
                            <div class="col-md-8">
                                <table class="table-condensed" style="width: 100%">
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="tb_detalle_descripcion" Rows="2" TextMode="MultiLine" />
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-12">
                            <label>Detalle mes</label>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12" style="height: 300px; overflow-y: scroll;">
                            <asp:GridView ID="gv_detalle" runat="server" OnRowDataBound="gv_detalle_RowDataBound" EmptyDataText="No existen empleados en el área."
                                AutoGenerateColumns="False" GridLines="None" CssClass="table table-condensed table-bordered">
                                <Columns>
                                    <asp:BoundField DataField="detalle_fecha" HeaderText="Fecha" ReadOnly="true" />
                                    <asp:BoundField DataField="detalle_monto" HeaderText="Monto" ReadOnly="true" />
                                    <asp:BoundField DataField="detalle_descripcion" HeaderText="Descripción" ReadOnly="true" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <button
                                                type="button" class="btn btn-sm btn-danger"
                                                data-toggle="modal"
                                                data-target="#advertencia_eliminacion"
                                                data-id='<%#Eval("detalle_id")%>'
                                                data-introduccion="el detalle de fecha"
                                                data-nombre='<%#String.Concat(Eval("detalle_fecha"), " por un monto de ", Eval("detalle_monto"))%>'>
                                                <span class="glyphicon glyphicon-remove-circle" aria-hidden="true"></span>&nbsp;Eliminar
                                            </button>
                                            <button runat="server" class="btn btn-sm btn-warning" id="btn_editar_detalle" causesvalidation="false" onserverclick="btn_editar_detalle_ServerClick" data-id='<%#Eval("detalle_id")%>'>
                                                <span class="glyphicon glyphicon-eye-open" aria-hidden="true"></span>&nbsp;Ver
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
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_scripts" runat="server">
    <script src="../js/jquery.treegrid.js"></script>

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

    </script>

    <script type="text/javascript">
        $(function () {
            $('#dtp_fecha').datetimepicker({
                locale: 'es',
                format: 'DD/MM/YYYY'
            });
        });

        $(document).ready(function () {
            $('.tree').treegrid();
        });

        $(document).ready(function () {
            $('.money').mask('000.000.000,00', { reverse: true });
        });

    </script>

    <script type="text/javascript">
        function Modifica_valor(obj, e) {
            if (e.keyCode == 13) {
                var id = parseInt(obj.id.split('_')[obj.id.split('_').length - 1]);
                var valorstr = obj.value.replace('.', '').replace(',', '.');
                var valor = parseFloat(valorstr).toFixed(2);
                PageMethods.ActualizarValor(id, valor, OnSuccessCallback, OnFailureCallback);
            }
        }

        function OnSuccessCallback(res) {
            //alert(res);
            location.reload();
        }

        function OnFailureCallback() {
            alert('Error');
        }


    </script>

</asp:Content>
