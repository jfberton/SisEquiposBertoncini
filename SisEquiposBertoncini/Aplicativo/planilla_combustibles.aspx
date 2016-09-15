<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="planilla_combustibles.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.planilla_combustibles" %>

<%@ Register Src="~/Aplicativo/Menues/menu_admin.ascx" TagPrefix="uc1" TagName="menu_admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_style" runat="server">
    <link href="../css/jquery.dataTables.min.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_menu" runat="server">
    <uc1:menu_admin runat="server" ID="menu_admin" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_body" runat="server">

    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>Planilla de combustibles
        </h1>

        <ol class="breadcrumb">
            <li>Inicio</li>
            <li>Planillas</li>
            <li class="active">Planilla de combustibles</li>
        </ol>
    </section>

    <!-- Main content -->
    <section class="content">

        <!-- Your Page Content Here -->
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
        <div class="row" runat="server" id="div_resultados">
            <div class="col-md-12">
                <button type="button" class="btn btn-default pull-right" causesvalidation="false" id="btn_agregar_linea" runat="server" onserverclick="btn_agregar_linea_ServerClick">
                    <span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span>Agregar nuevo
                </button>
            </div>
            <div class="col-md-12">
                <asp:GridView ID="gv_combustible" runat="server" OnPreRender="gv_combustible_PreRender" OnRowDataBound="gv_combustible_RowDataBound"
                    AutoGenerateColumns="False" GridLines="None" CssClass="display">
                    <Columns>
                        <asp:BoundField DataField="fecha" HeaderText="Fecha" ReadOnly="true" DataFormatString="{0:d}" />
                        <asp:BoundField DataField="chofer" HeaderText="Chofer" ReadOnly="true" />
                        <asp:CheckBoxField DataField="tanque_lleno" HeaderText="Tanque lleno" ReadOnly="true" />
                        <asp:BoundField DataField="litros" HeaderText="Litros" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="km" HeaderText="Kilometros" ReadOnly="true" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,#}" />
                        <asp:BoundField DataField="promedio" HeaderText="Promedio" ReadOnly="true" DataFormatString="{0:#,#}" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="costo_total_facturado" HeaderText="Costo" ReadOnly="true" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="lugar" HeaderText="Lugar" ReadOnly="true" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <button
                                    type="button" class="btn btn-sm btn-danger"
                                    data-toggle="modal"
                                    data-target="#advertencia_eliminacion"
                                    data-id='<%#Eval("id_planilla_combustible")%>'
                                    data-introduccion="la factura a nombre del chofer"
                                    data-nombre='<%#Eval("chofer")%>'>
                                    <span class="glyphicon glyphicon-remove-circle" aria-hidden="true"></span>Eliminar
                                </button>
                                <button runat="server" class="btn btn-sm btn-default" id="btn_ver" causesvalidation="false" onserverclick="btn_ver_ServerClick" data-id='<%#Eval("id_planilla_combustible")%>'>
                                    <span class="glyphicon glyphicon-eye-open" aria-hidden="true"></span>Ver
                                </button>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

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

        <div class="modal fade" id="agregar_linea_combustible" role="dialog" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">
                            <asp:Label Text="Agregar " ID="lbl_agregar_factura_combust_titulo" runat="server" /></h4>
                        <input type="hidden" runat="server" id="id_factura_combust_hidden" value="0" />
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
                                    <div class="col-md-4">
                                        <table>
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
                                                        <div id="dtp_fecha_factura" class="input-group date">
                                                            <input type="text" runat="server" id="tb_fecha_factura" class="form-control ddmmaaaa" />
                                                            <span class="input-group-addon">
                                                                <span class="glyphicon glyphicon-calendar"></span>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <asp:CustomValidator ID="cv_fecha_factura" runat="server" Text="<img src='../img/exclamation.gif' title='Debe ingresar una fecha de nacimiento válida' />"
                                                        ErrorMessage="Debe ingresar una fecha de factura válida" OnServerValidate="cv_fecha_factura_ServerValidate">
                                                    </asp:CustomValidator></td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="row">
                                    <div class="col-md-4">
                                        <table>
                                            <tr>
                                                <td>Chofer</td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="col-md-8">
                                        <input type="text" id="tb_chofer" runat="server" class="form-control" value="1" />
                                        <asp:RequiredFieldValidator ControlToValidate="tb_chofer" Text="<img src='../img/exclamation.gif' title='Debe ingresar el nombre del chofer asignado' />"
                                            ID="rv_nombre_empleado" runat="server" ErrorMessage="Debe ingresar el nombre del chofer asignado">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <table class="table">
                                    <tr>
                                        <td>Litros</td>
                                        <td>
                                            <input type="text" id="tb_litros" class="form-control" runat="server" value="1" />
                                            <asp:CustomValidator ID="cv_litros" runat="server" Text="<img src='../img/exclamation.gif' title='Debe ingresar una cantidad de litros numérica' />"
                                                ErrorMessage="Debe ingresar una cantidad de litros numérica" OnServerValidate="cv_litros_ServerValidate">
                                            </asp:CustomValidator>
                                        </td>
                                        <td>KM</td>
                                        <td>
                                            <input type="text" id="tb_km" runat="server" class="form-control" value="1" />
                                            <asp:CustomValidator ID="cv_km" runat="server" Text="<img src='../img/exclamation.gif' title='Debe ingresar una cantidad de km numérica' />"
                                                ErrorMessage="Debe ingresar una cantidad de km numérica" OnServerValidate="cv_km_ServerValidate">
                                            </asp:CustomValidator>
                                        </td>
                                        <td>Costo</td>
                                        <td>
                                            <input type="text" id="tb_costo" class="form-control" runat="server" value="1" />
                                            <asp:CustomValidator ID="cv_costo" runat="server" Text="<img src='../img/exclamation.gif' title='Debe ingresar una cantidad de litros numérica' />"
                                                ErrorMessage="Debe ingresar una cantidad de litros numérica" OnServerValidate="cv_costo_ServerValidate">
                                            </asp:CustomValidator>
                                        </td>
                                        <td>Tanque lleno</td>
                                        <td>
                                            <asp:CheckBox Text="" ID="chk_tanque_lleno" CssClass="form-control" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-2">
                                <table>
                                    <tr>
                                        <td>Lugar</td>
                                    </tr>
                                </table>
                            </div>
                            <div class="col-md-10">
                                <input type="text" id="tb_lugar" class="form-control" runat="server" value="" />
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
    </section>
</asp:Content>


<asp:Content ID="Content5" ContentPlaceHolderID="cph_scripts" runat="server">
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
            $('#<%= gv_combustible.ClientID %>').DataTable({
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
            $('#dtp_fecha_factura').datetimepicker({
                locale: 'es',
                format: 'DD/MM/YYYY'
            });
        });
    </script>

</asp:Content>
