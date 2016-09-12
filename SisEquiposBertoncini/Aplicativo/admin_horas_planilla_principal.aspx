<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="admin_horas_planilla_principal.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.admin_horas_planilla_principal" %>

<%@ Register Src="~/Aplicativo/Menues/menu_admin.ascx" TagPrefix="uc1" TagName="menu_admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_menu" runat="server">
    <uc1:menu_admin runat="server" ID="menu_admin" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_body" runat="server">

    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>Planilla principal
       
                        <small>descripción del cálculo del valor de la hora trabajada según empleado</small>
        </h1>

        <ol class="breadcrumb">
            <li><a href="#"><i></i>Inicio</a></li>
            <li><a href="#"><i></i>Planillas</a></li>
            <li class="active">En función de horas empleado</li>
        </ol>
    </section>

    <!-- Main content -->
    <section class="content">

        <!-- Your Page Content Here -->
        <div class="row">
            <div class="col-md-12">
                <table class="table-condensed" style="width: 100%;">
                    <tr>
                        <td>Categoría de empleado</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddl_tipo_empleado" CssClass="form-control">
                                <asp:ListItem Text="Mecánicos" Value="Mecánicos" />
                                <asp:ListItem Text="Soldadores" Value="Soldadores" />
                                <asp:ListItem Text="Grueros" Value="Grueros" />
                            </asp:DropDownList></td>
                        <td>Mes</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddl_mes" CssClass="form-control">
                                <asp:ListItem Text="Enero" Value="1" />
                                <asp:ListItem Text="Febrero" Value="2" />
                                <asp:ListItem Text="Marzo" Value="3" />
                                <asp:ListItem Text="Abril" Value="4" />
                                <asp:ListItem Text="Mayo" Value="5" />
                                <asp:ListItem Text="Junio" Value="6" />
                                <asp:ListItem Text="Julio" Value="7" />
                                <asp:ListItem Text="Agosto" Value="8" />
                                <asp:ListItem Text="Septiembre" Value="9" />
                                <asp:ListItem Text="Octubre" Value="10" />
                                <asp:ListItem Text="Noviembre" Value="11" />
                                <asp:ListItem Text="Diciembre" Value="12" />
                            </asp:DropDownList></td>
                        <td>Año
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddl_anio" CssClass="form-control">
                            </asp:DropDownList></td>
                        <td>
                            <asp:Button Text="Buscar" CssClass="btn btn-default" ID="btn_buscar" OnClick="btn_buscar_Click" runat="server" />
                            <asp:Button Text="Nueva búsqueda" CssClass="btn btn-warning" ID="btn_nueva_busqueda" OnClick="btn_nueva_busqueda_Click" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>

        <div class="row" runat="server" id="div_resultados_busqueda">
            <div class="col-md-12">
                <div class="row" id="fila_grilla_empleados">
                    <div class="col-md-12">
                        <asp:GridView ID="gv_planilla_empleados" runat="server" EmptyDataText="No existen empleados por mostrar en esta categoría." OnRowDataBound="gv_planilla_empleados_RowDataBound"
                            AutoGenerateColumns="False" GridLines="None" CssClass="table table-condensed table-bordered">
                            <Columns>
                                <asp:BoundField DataField="empleado" HeaderText="Nombre" ReadOnly="true" />
                                <asp:TemplateField HeaderText="Sueldo" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label Text='<%#String.Format("{0:$ #,###.00}",Eval("sueldo"))%>' runat="server" />
                                        <button
                                            type="button" class="btn btn-sm btn-default"
                                            data-toggle="modal"
                                            data-target="#editar_sueldo"
                                            data-id='<%#Eval("empleado_id")%>'>
                                            <span class="glyphicon glyphicon-pencil" aria-hidden="true"></span>Editar
                                        </button>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Días mes" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label Text='<%#Eval("dias_mes")%>' runat="server" />
                                        <button runat="server" class="btn btn-sm btn-default right" id="btn_ver" causesvalidation="false" onserverclick="btn_ver_ServerClick" data-id='<%#Eval("empleado_id")%>'>
                                            <span class="glyphicon glyphicon-eye-open" aria-hidden="true"></span>Ver mes
                                        </button>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="dias_out" HeaderText="Días OUT" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="costo_mensual_ponderado" HeaderText="Costo mensual ponderado" DataFormatString="{0:$ #,###.00}" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            </Columns>
                        </asp:GridView>

                        <asp:GridView ID="gv_planilla_empleados_view" runat="server" EmptyDataText="No existen empleados por mostrar en esta categoría." OnRowDataBound="gv_planilla_empleados_RowDataBound"
                            AutoGenerateColumns="False" GridLines="None" CssClass="table table-condensed table-bordered">
                            <Columns>
                                <asp:BoundField DataField="empleado" HeaderText="Nombre" ReadOnly="true" />
                                <asp:TemplateField HeaderText="Sueldo" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label Text='<%#String.Format("{0:$ #,###.00}",Eval("sueldo"))%>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Días mes" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label Text='<%#Eval("dias_mes")%>' runat="server" />
                                        <button runat="server" class="btn btn-sm btn-default right" id="btn_ver" causesvalidation="false" onserverclick="btn_ver_ServerClick" data-id='<%#Eval("empleado_id")%>'>
                                            <span class="glyphicon glyphicon-eye-open" aria-hidden="true"></span>Ver mes
                                        </button>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="dias_out" HeaderText="Días OUT" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="costo_mensual_ponderado" HeaderText="Costo mensual ponderado" DataFormatString="{0:$ #,###.00}" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                            </Columns>
                        </asp:GridView>

                        <div class="modal fade" id="editar_sueldo" role="dialog" aria-hidden="true">
                            <div class="modal-dialog">
                                <div class="modal-content panel-default">
                                    <div class="modal-header panel-heading">
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                        <h4 class="modal-title panel-title">Editar sueldo</h4>
                                    </div>
                                    <div class="modal-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <input type="hidden" runat="server" id="id_empleado_hidden" />
                                                <table class="table-condensed" style="width: 100%">
                                                    <tr>
                                                        <td>Sueldo</td>
                                                        <td>
                                                            <input type="text" runat="server" id="tb_sueldo_empleado" class="form-control" /></td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button Text="Aceptar" CssClass="btn btn-success" CausesValidation="false" ID="btn_editar_sueldo_empleado" OnClick="btn_editar_sueldo_empleado_Click" runat="server" />
                                        <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12" id="totales">
                            <div class="row">
                                <div class="col-md-3">Sueldos totales</div>
                                <div class="col-md-3">
                                    <asp:Label Text="$ 000.000,00" ID="lbl_sueldos_totales" runat="server" />
                                </div>
                                <div runat="server" id="div_costo_mensual_ponderado_total">
                                    <div class="col-md-3">Costo mensual ponderado total</div>
                                    <div class="col-md-3">
                                        <asp:Label Text="$ 000.000,00" ID="lbl_costo_mensual_ponderado_total" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <br />
                        </div>

                    </div>
                </div>

                <div class="row" id="costos_mensuales_hora_hombre">
                    <div class="col-md-6">
                        <table class="table table-bordered">
                            <tr>
                                <td>Masa salarial según planilla</td>
                                <td class="text-right">
                                    <asp:Label Text="" ID="lbl_sueldos_totales_1" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Cantidad empleados</td>
                                <td class="text-right">
                                    <asp:Label Text="" ID="lbl_cantidad_de_empleados" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Dias de 8 horas del mes</td>
                                <td class="text-right">
                                    <asp:Label Text="" ID="lbl_cantidad_de_dias" runat="server" />
                                </td>
                            </tr>
                            <tr class="alert-success">
                                <td>Costo horas hombre teórico</td>
                                <td class="text-right">
                                    <asp:Label Text="" ID="lbl_costo_horas_hombre_teorico" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="col-md-6" runat="server" id="tabla_masa_salarial_ajustada_menos_dias_OUT">
                        <table class="table table-bordered">
                            <tr>
                                <td>Masa salarial ajustada (menos días OUT)</td>
                                <td class="text-right">
                                    <asp:Label Text="" ID="lbl_costo_mensual_ponderado_total_1" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Cantidad empleados</td>
                                <td class="text-right">
                                    <asp:Label Text="" ID="lbl_cantidad_de_empleados_1" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Dias de 8 horas del mes</td>
                                <td class="text-right">
                                    <asp:Label Text="" ID="lbl_cantidad_de_dias_1" runat="server" />
                                </td>
                            </tr>
                            <tr class="alert-success">
                                <td>Costo hrs hombre  ajustado teórico</td>
                                <td class="text-right">
                                    <asp:Label Text="" ID="lbl_costo_horas_hombre_teorico_ajustado" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

                <div class="row" id="masa_salarial_ajustada">
                    <div class="col-md-12">
                        <table class="table table-bordered">
                            <tr>
                                <td style="width: 82%">
                                    <asp:Label Text="- COSTO DE LA MASA SALARIAL AJUSTADA (MENOS DIAS OUT)" ID="lbl_texto_masa_salarial" runat="server" /></td>
                                <td style="width: 9%" class="text-right">
                                    <asp:Label Text="" ID="lbl_costo_mensual_ponderado_total_2" runat="server" /></td>
                                <td style="width: 9%"></td>
                            </tr>
                        </table>
                    </div>
                </div>

                <div class="row" id="ausentes">
                    <div class="col-md-12">
                        <table class="table table-bordered">
                            <tr>
                                <td colspan="5" style="width: 82%">- AUSENTES</td>
                                <td style="width: 9%" class="text-right">
                                    <asp:Label Text="$ 00,0" ID="lbl_costo_ausentes" runat="server" /></td>
                                <td style="width: 9%" class="alert-info text-right">
                                    <asp:Label Text="$ 00,0" ID="lbl_costo_ausentes_prueba" runat="server" /></td>
                            </tr>
                            <tr>
                                <td style="width: 14%"></td>
                                <td style="width: 27%">Horas ausentes totales</td>
                                <td style="width: 13%">
                                    <asp:Label Text="00,00" ID="lbl_horas_ausentes_totales" runat="server" /></td>
                                <td style="width: 7%">
                                    <asp:TextBox runat="server" OnTextChanged="txt_prueba_TextChanged" AutoPostBack="true" Width="100%" ID="txt_horas_ausentes_totales_prueba" /></td>
                                <td style="width: 21%" class="text-right">% Ausentismo</td>
                                <td style="width: 9%" class="alert-danger">
                                    <asp:Label Text="0,00 %" ID="lbl_ausentismo_porcentaje" runat="server" /></td>
                                <td style="width: 9%">
                                    <asp:Label Text="0,00 %" ID="lbl_ausentismo_porcentaje_prueba" runat="server" /></td>
                            </tr>
                            <tr>
                                <td style="width: 14%"></td>
                                <td style="width: 27%">Costo horas hombre</td>
                                <td style="width: 13%">
                                    <asp:Label Text="$ 00,00" ID="lbl_costo_horas_hombre_teorico_ajustado_1" runat="server" /></td>
                                <td style="width: 7%"></td>
                                <td style="width: 21%"></td>
                                <td style="width: 9%"></td>
                                <td style="width: 9%"></td>
                            </tr>
                        </table>
                    </div>
                </div>

                <div class="row" id="horas_extra_50">
                    <div class="col-md-12">
                        <table class="table table-bordered">
                            <tr>
                                <td colspan="5" style="width: 82%">- HORAS EXTRAS AL 50%</td>
                                <td style="width: 9%" class="text-right">
                                    <asp:Label Text="$ 00,0" ID="lbl_costo_horas_extra_50" runat="server" /></td>
                                <td style="width: 9%" class="alert-info text-right">
                                    <asp:Label Text="$ 00,0" ID="lbl_costo_horas_extra_50_prueba" runat="server" /></td>
                            </tr>
                            <tr>
                                <td style="width: 14%"></td>
                                <td style="width: 27%">Hs extras al 50% totales</td>
                                <td style="width: 13%" class="text-right">
                                    <asp:Label Text="00,00" ID="lbl_horas_extra_totales_50" runat="server" /></td>
                                <td style="width: 7%" class="text-right">
                                    <asp:TextBox runat="server" Width="100%" ID="txt_horas_extra_totales_50_prueba" OnTextChanged="txt_prueba_TextChanged" AutoPostBack="true" /></td>
                                <td style="width: 21%"></td>
                                <td style="width: 9%"></td>
                                <td style="width: 9%"></td>
                            </tr>
                            <tr>
                                <td style="width: 14%"></td>
                                <td style="width: 27%">Costo horas hombre</td>
                                <td style="width: 13%" class="text-right">
                                    <asp:Label Text="$ 00,00" ID="lbl_costo_horas_hombre_teorico_ajustado_2" runat="server" /></td>
                                <td style="width: 7%"></td>
                                <td style="width: 21%"></td>
                                <td style="width: 9%"></td>
                                <td style="width: 9%"></td>
                            </tr>
                        </table>
                    </div>
                </div>

                <div class="row" id="horas_extra_100">
                    <div class="col-md-12">
                        <table class="table table-bordered">
                            <tr>
                                <td colspan="5" style="width: 82%">- HORAS EXTRAS AL 100%</td>
                                <td style="width: 9%" class="text-right">
                                    <asp:Label Text="$ 00,0" ID="lbl_costo_horas_extra_100" runat="server" /></td>
                                <td style="width: 9%" class="alert-info text-right">
                                    <asp:Label Text="$ 00,0" ID="lbl_costo_horas_extra_100_prueba" runat="server" /></td>
                            </tr>
                            <tr>
                                <td style="width: 14%"></td>
                                <td style="width: 27%">Hs extras al 100% totales</td>
                                <td style="width: 13%" class="text-right">
                                    <asp:Label Text="00,00" ID="lbl_horas_extra_totales_100" runat="server" /></td>
                                <td style="width: 7%" class="text-right">
                                    <asp:TextBox runat="server" Width="100%" ID="txt_horas_extra_totales_100_prueba" OnTextChanged="txt_prueba_TextChanged" AutoPostBack="true" /></td>
                                <td style="width: 21%"></td>
                                <td style="width: 9%"></td>
                                <td style="width: 9%"></td>
                            </tr>
                            <tr>
                                <td style="width: 14%"></td>
                                <td style="width: 27%">Costo horas hombre</td>
                                <td style="width: 13%" class="text-right">
                                    <asp:Label Text="$ 00,00" ID="lbl_costo_horas_hombre_teorico_ajustado_3" runat="server" /></td>
                                <td style="width: 7%"></td>
                                <td style="width: 21%"></td>
                                <td style="width: 9%"></td>
                                <td style="width: 9%"></td>
                            </tr>
                        </table>
                    </div>
                </div>

                <div class="row" id="nueva_masa_salarial">
                    <div class="col-md-12">
                        <table class="table table-bordered">
                            <tr>
                                <td style="width: 82%">- NUEVA MASA SALARIAL</td>
                                <td style="width: 9%" class="text-right">
                                    <asp:Label Text="$ 00,0" ID="lbl_nueva_masa_salarial" runat="server" /></td>
                                <td style="width: 9%" class="alert-info text-right">
                                    <asp:Label Text="$ 00,0" ID="lbl_nueva_masa_salarial_prueba" runat="server" /></td>
                            </tr>
                        </table>
                    </div>
                </div>

                <div class="row" id="resultado_mensual_final_planilla">
                    <div class="col-md-12">
                        <table class="table table-bordered">
                            <tr>
                                <td colspan="2" style="background-color: gray" class="text-center">
                                    <asp:Label Text="" ID="lbl_titulo_mes_fin_planilla" runat="server" ForeColor="Black" Font-Bold="true" /></td>
                                <td class="alert-info">
                                    <label>Monto Prueba</label></td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Nueva masa salarial</label></td>
                                <td>
                                    <asp:Label Text="" ID="lbl_nueva_masa_salarial_1" runat="server" /></td>
                                <td>
                                    <asp:Label Text="" ID="lbl_nueva_masa_salarial_prueba_1" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Hs realmente trabajadas</label></td>
                                <td>
                                    <asp:Label Text="" ID="lbl_horas_realmente_trabajadas" runat="server" /></td>
                                <td>
                                    <asp:Label Text="" ID="lbl_horas_trabajadas_segun_datos_prueba" runat="server" /></td>
                            </tr>
                            <tr class="alert-success">
                                <td>
                                    <h4>Costo horas hombre real ($)</h4>
                                </td>
                                <td>
                                    <h4>
                                        <asp:Label Text="" ID="lbl_costo_horas_hombre_real" runat="server" /></h4>
                                </td>
                                <td>
                                    <h4>
                                        <asp:Label Text="" ID="lbl_costo_horas_hombre_real_prueba" runat="server" /></h4>
                                </td>
                            </tr>
                            <tr class="alert-success">
                                <td>
                                    <h4>Valor dolar</h4>
                                </td>
                                <td>
                                    <h4>
                                        <asp:Label Text="" ID="lbl_valor_dolar_mes" runat="server" /></h4>
                                </td>
                                <td>
                                    <h4>
                                        <asp:TextBox runat="server" Width="100%" ID="txt_valor_dolar_mes_prueba" OnTextChanged="txt_prueba_TextChanged" AutoPostBack="true" /></h4>
                                </td>
                            </tr>
                            <tr class="alert-success">
                                <td>
                                    <h4>Costo horas hombre real (USS)</h4>
                                </td>
                                <td>
                                    <h4>
                                        <asp:Label Text="" ID="lbl_costo_horas_hombre_real_dolar" runat="server" /></h4>
                                </td>
                                <td>
                                    <h4>
                                        <asp:Label Text="" ID="lbl_costo_horas_hombre_real_prueba_dolar" runat="server" /></h4>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

            </div>
            <div class="col-md-12 text-right">
                <button class="btn btn-default btn-lg" runat="server" id="btn_ver_planilla_calculos" onserverclick="btn_ver_planilla_calculos_ServerClick">
                    <span class="glyphicon glyphicon-chevron-right"></span>&nbsp;Planilla de cálculos
                </button>
            </div>
        </div>


    </section>
    <!-- /.content -->

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_scripts" runat="server">
    <script>
        $(document).ready(function () {
            $('.money').mask('000.000.000,00', { reverse: true });
        });
    </script>

    <script>
        $('#editar_sueldo').on('show.bs.modal', function (event) {
            // Button that triggered the modal
            var button = $(event.relatedTarget)

            // Extract info from data-* attributes
            var id = button.data('id')

            // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
            var modal = $(this)
            modal.find('.modal-body #' + '<%= id_empleado_hidden.ClientID %>').val(id)
        })

    </script>
</asp:Content>
