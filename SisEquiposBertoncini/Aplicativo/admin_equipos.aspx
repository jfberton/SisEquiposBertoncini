<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="admin_equipos.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.admin_equipos" %>

<%@ Register Src="~/Aplicativo/Menues/menu_admin.ascx" TagPrefix="uc1" TagName="menu_admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_menu" runat="server">
    <uc1:menu_admin runat="server" ID="menu_admin" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_body" runat="server">

    <ol class="breadcrumb">
       <li>Inicio</li>
        <li>Equipos</li>
        <li>Equipos</li>
    </ol>

    <div class="panel panel-default">
        <div class="panel-heading">
            <div class="row">
                <div class="col-md-5">
                    <h4 class="panel-title">Equipos</h4>
                </div>
            </div>
        </div>
        <div class="panel-body">
            <asp:GridView ID="gv_equipos" runat="server" EmptyDataText="No existen equipos por mostrar." OnPreRender="gv_equipos_PreRender"
                AutoGenerateColumns="False" GridLines="None" CssClass="display">
                <Columns>
                    <asp:BoundField DataField="equipo_categoria" HeaderText="Categoria" ReadOnly="true" />
                    <asp:BoundField DataField="equipo_nombre" HeaderText="Nombre" ReadOnly="true" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <button
                                type="button" class="btn btn-sm btn-danger"
                                data-toggle="modal"
                                data-target="#advertencia_eliminacion"
                                data-id='<%#Eval("equipo_id")%>'
                                data-introduccion="el equipo"
                                data-nombre='<%#Eval("equipo_nombre")%>'>
                                <span class="glyphicon glyphicon-remove-circle" aria-hidden="true"></span>Eliminar
                            </button>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <button runat="server" class="btn btn-sm btn-warning" id="btn_editar" causesvalidation="false" onserverclick="btn_editar_ServerClick" data-id='<%#Eval("equipo_id")%>'>
                                <span class="glyphicon glyphicon-pencil" aria-hidden="true"></span>Editar
                            </button>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <button runat="server" class="btn btn-sm btn-default" id="btn_ver" causesvalidation="false" onserverclick="btn_ver_ServerClick" data-id='<%#Eval("equipo_id")%>'>
                                <span class="glyphicon glyphicon-eye-open" aria-hidden="true"></span>Ver
                            </button>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <asp:GridView ID="gv_equipos_view" runat="server" EmptyDataText="No existen equipos por mostrar." OnPreRender="gv_equipos_PreRender"
                AutoGenerateColumns="False" GridLines="None" CssClass="display">
                <Columns>
                    <asp:BoundField DataField="equipo_categoria" HeaderText="Categoria" ReadOnly="true" />
                    <asp:BoundField DataField="equipo_nombre" HeaderText="Nombre" ReadOnly="true" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <button runat="server" class="btn btn-sm btn-default" id="btn_ver" causesvalidation="false" onserverclick="btn_ver_ServerClick" data-id='<%#Eval("equipo_id")%>'>
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
                <div class="col-md-12 text-right">
                    <button runat="server" id="btn_agregar" onserverclick="btn_agregar_ServerClick" class="btn btn-default"><span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span>Agregar nuevo</button>
                </div>
            </div>
        </div>

        <div class="modal fade" id="ver_equipo" role="dialog" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12">
                                <h2>
                                    <asp:Label Text="" ID="lbl_nombre" runat="server" />
                                    <small>
                                        <label>Categoría</label>
                                        <asp:Label Text="" ID="lbl_categoria" runat="server" /></small></h2>
                                <asp:Label Text="" ID="lbl_out" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <label>Notas:</label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Label Text="" ID="lbl_notas_equipo" runat="server" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <label>Partes</label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="table-responsive">
                                <asp:GridView ID="gv_partes" runat="server" EmptyDataText="No existen partes del equipo por mostrar." OnPreRender="gv_equipos_PreRender"
                                    AutoGenerateColumns="False" GridLines="None" CssClass="display">
                                    <Columns>
                                        <asp:BoundField DataField="nombre_parte" HeaderText="Nombre" ReadOnly="true" ItemStyle-Width="110px" />
                                        <asp:BoundField DataField="costo_cero" HeaderText="Costo 0km" ReadOnly="true" ItemStyle-Width="110px" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="porcentaje_usado" HeaderText="Usado" ReadOnly="true" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="porcentaje_valor_residual" HeaderText="Valor recidual" ReadOnly="true" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="valor_por_amortizar" HeaderText="Valor por amortizar" ReadOnly="true" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="periodo_alta" HeaderText="Periodo alta" ReadOnly="true" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="meses_por_amortizar" HeaderText="Meses por amortizar" ReadOnly="true" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="meses_amortizados" HeaderText="Meses amortizados" ReadOnly="true" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="restan_amortizar" HeaderText="Restan amortizar" ReadOnly="true" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="costo_mensual" HeaderText="Costo mensual" ReadOnly="true" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Right" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <br />
                        <div class="row" runat="server" id="div_totales_partes">
                            <div class="col-md-12">
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
                        <br />
                        <div class="row">
                            <div class="col-md-12 text-right">
                                <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
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

    <script src="../../js/jquery.dataTables.min.js"></script>
    <script src="../../js/dataTables.bootstrap.min.js"></script>

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
    <script>
        $(document).ready(function () {
            $('#<%= gv_equipos.ClientID %>').DataTable({
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
            $('#<%= gv_equipos_view.ClientID %>').DataTable({
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
            $('#<%= gv_partes.ClientID %>').DataTable({
                "scrollY": "200px",
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
    </script>
</asp:Content>
