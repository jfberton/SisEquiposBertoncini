<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="admin_valores_mes_equipo_v2.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.admin_valores_mes_equipo_v2" %>

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
                    <asp:GridView ID="gv_items" runat="server" OnRowDataBound="gv_items_RowDataBound" EmptyDataText="No existen conceptos de ingresos - egresos."
                        AutoGenerateColumns="False" GridLines="None" CssClass="tree_valores table table-condensed table-bordered" ShowHeader="False">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label Text='<%#Eval("concepto")%>' ID="lbl_concepto" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label Text='<%#Eval("valorstr")%>' ID="lbl_valor" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <button runat="server" class="btn btn-sm btn-warning" id="btn_ver_editar_detalle"
                                        causesvalidation="false" onserverclick="btn_ver_editar_detalle_ServerClick"
                                        title='<%#String.Concat("Ver, editar detalle ","\"",Eval("concepto"),"\"")%>'
                                        data-id='<%#Eval("id_valor_mes")%>' visible='<%#Eval("visible")%>'>
                                        <span class="glyphicon glyphicon-eye-open" aria-hidden="true"></span>&nbsp;Ver
                                    </button>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="ver_detalle_item_mes" role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-body">
                    <div class="row">
                        <input type="hidden" id="hidden_id_valor_mes" runat="server" />
                        <div class="col-md-12">
                            <h4>
                                <asp:Label Text="" ID="lbl_item" runat="server" />
                                <small>
                                    <label>Categoría</label>
                                    <asp:Label Text="" ID="lbl_categoria" runat="server" />
                                    - 
                                        <asp:Label Text="" ID="lbl_mes" runat="server" /></small>

                            </h4>

                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:ValidationSummary ID="validation_summary" runat="server" DisplayMode="BulletList"
                                CssClass="validationsummary panel panel-danger" HeaderText="<div class='panel-heading'>&nbsp;Corrija los siguientes errores antes de continuar:</div>" />
                        </div>
                    </div>
                    <div class="row">
                        <div class=" col-md-6">
                            <table style="width: 100%">
                                <tr>
                                    <td style="vertical-align: text-top">Fecha&nbsp;</td>
                                    <td style="width: 100%">
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
                        <div class="col-md-6">
                            <table style="width: 100%">
                                <tr>
                                    <td style="vertical-align: text-top">Monto&nbsp;</td>
                                    <td style="width: 100%">
                                            <asp:TextBox runat="server" CssClass="form-control" ID="tb_detalle_monto" />
                                    </td>
                                    <td>
                                        <asp:CustomValidator ID="cv_monto" runat="server" Text="<img src='../img/exclamation.gif' title='Debe ingresar un monto válido' />"
                                            ErrorMessage="Debe ingresar un monto válido" OnServerValidate="cv_monto_ServerValidate">
                                        </asp:CustomValidator></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-10">
                            <table style="width: 100%">
                                <tr>
                                    <td style="vertical-align: text-top">Descripción&nbsp;
                                    </td>
                                    <td style="width:100%">
                                        <asp:TextBox runat="server" CssClass="form-control" ID="tb_detalle_descripcion" /></td>
                                </tr>
                            </table>
                        </div>
                        <div class="col-md-2">
                            <input type="hidden" value ="0" runat="server" id="hidden_id_detalle_01" />
                            <button runat="server" id="btn_agregar_detalle" class="btn btn-success " onserverclick="btn_agregar_detalle_ServerClick">
                                <span class="glyphicon glyphicon-plus-sign"></span>&nbsp;Agregar
                            </button>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <label>Detalle mes</label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12" style="height: 200px; overflow-y: scroll;">
                            <asp:GridView ID="gv_detalle" runat="server" Font-Size="Small" OnRowDataBound="gv_detalle_RowDataBound" EmptyDataText="No existen valores cargados."
                                AutoGenerateColumns="False" GridLines="None" CssClass="table table-condensed table-bordered">
                                <Columns>
                                    <asp:BoundField DataField="detalle_fecha" HeaderText="Fecha" DataFormatString="{0:d}" ReadOnly="true" />
                                    <asp:BoundField DataField="detalle_monto" DataFormatString="{0:$ #,##0.00}" HeaderText="Monto" ReadOnly="true" />
                                    <asp:BoundField DataField="detalle_descripcion" HeaderText="Descripción" ReadOnly="true" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <button
                                                type="button" class="btn btn-sm btn-danger"
                                                data-toggle="modal"
                                                data-target="#advertencia_eliminacion"
                                                data-id='<%#Eval("detalle_id")%>'
                                                data-introduccion="el detalle de fecha"
                                                data-nombre='<%#String.Concat(Eval("detalle_fecha"), " por un monto de $ ", Eval("detalle_monto"))%>'>
                                                <span class="glyphicon glyphicon-remove-circle" aria-hidden="true"></span>&nbsp;Eliminar
                                            </button>
                                             <button runat="server" class="btn btn-sm btn-warning" id="btn_editar_detalle" causesvalidation="false" onserverclick="btn_editar_detalle_ServerClick" data-id='<%#Eval("detalle_id")%>'>
                                                <span class="glyphicon glyphicon-pencil" aria-hidden="true"></span>&nbsp;Editar
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
                                            <asp:Button Text="Cancelar" CssClass="btn btn-default" CausesValidation="false" ID="bt_cancelar_eliminacion" OnClick="bt_cancelar_eliminacion_Click" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <label>
                                    Total
                                    <asp:Label Text="" ID="lbl_total_item_mes" runat="server" /></label></td>
                            <td>
                                <button type="button" class="btn btn-default pull-right" data-dismiss="modal">Cerrar</button></td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_scripts" runat="server">
    <script src="../js/jquery.treegrid.js"></script>
    <script src="../js/jquery.cookie.js"></script>
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
       

        $(document).ready(function () {
            $('.tree_valores').treegrid({
                'initialState': 'collapsed',
                'saveState': true,
            });
        });

    </script>

</asp:Content>
