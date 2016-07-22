<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="planilla_resumen_valores_equipo_categoria.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.planilla_resumen_valores_equipo_categoria" %>

<%@ Register Src="~/Aplicativo/Menues/menu_admin.ascx" TagPrefix="uc1" TagName="menu_admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_menu" runat="server">
    <uc1:menu_admin runat="server" ID="menu_admin" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_body" runat="server">
    <div class="panel panel-default">
        <div class="panel-heading">
            <h1 class="panel-title">Visualiza los valores anuales cargados al equipo
            </h1>
        </div>
        <div class="panel-body">
            <div class="row" runat="server" id="row_busqueda">
                <div class="col-md-5">
                    <table class="table-condensed" style="width: 100%">
                        <tr>
                            <td>Equipo</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddl_categoria" CssClass="form-control">
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
                                <asp:Button Text="Imprimir" ID="btn_imprimir" CssClass="btn btn-default" OnClick="btn_imprimir_Click" runat="server" />
                                <asp:Button Text="Nueva búsqueda" runat="server" ID="btn_nueva_busqueda" CssClass="btn btn-danger" OnClick="btn_nueva_busqueda_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <asp:GridView ID="gv_detalle_equipos" runat="server" OnRowDataBound="gv_detalle_equipos_RowDataBound" EmptyDataText="No existen resultados"
        AutoGenerateColumns="False" GridLines="None" CssClass="table table-condensed table-bordered" ShowHeader="true" Width="1800px">
        <Columns>
            <asp:BoundField DataField="nombre_equipo" HeaderText="Equipo" ReadOnly="true"/>
            <asp:BoundField DataField="tipo_resultado" HeaderText="Tipo resultado" ReadOnly="true" />
            <asp:BoundField DataField="enero" HeaderText="Enero" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
            <asp:BoundField DataField="febrero" HeaderText="Febrero" ReadOnly="true" ItemStyle-HorizontalAlign="Right"/>
            <asp:BoundField DataField="marzo" HeaderText="Marzo" ReadOnly="true" ItemStyle-HorizontalAlign="Right"/>
            <asp:BoundField DataField="abril" HeaderText="Abril" ReadOnly="true" ItemStyle-HorizontalAlign="Right"/>
            <asp:BoundField DataField="mayo" HeaderText="Mayo" ReadOnly="true" ItemStyle-HorizontalAlign="Right"/>
            <asp:BoundField DataField="junio" HeaderText="Junio" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
            <asp:BoundField DataField="julio" HeaderText="Julio" ReadOnly="true" ItemStyle-HorizontalAlign="Right"/>
            <asp:BoundField DataField="agosto" HeaderText="Agosto" ReadOnly="true" ItemStyle-HorizontalAlign="Right"/>
            <asp:BoundField DataField="septiembre" HeaderText="Septiembre" ReadOnly="true" ItemStyle-HorizontalAlign="Right"/>
            <asp:BoundField DataField="octubre" HeaderText="Octubre" ReadOnly="true" ItemStyle-HorizontalAlign="Right"/>
            <asp:BoundField DataField="noviembre" HeaderText="Noviembre" ReadOnly="true" ItemStyle-HorizontalAlign="Right"/>
            <asp:BoundField DataField="diciembre" HeaderText="Diciembre" ReadOnly="true" ItemStyle-HorizontalAlign="Right"/>
            <asp:BoundField DataField="total" HeaderText="Total Anual" ReadOnly="true" ItemStyle-HorizontalAlign="Right"/>
            <asp:BoundField DataField="promedio" HeaderText="Promedio mensual" ReadOnly="true" ItemStyle-HorizontalAlign="Right"/>
        </Columns>
    </asp:GridView>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_scripts" runat="server">
   
</asp:Content>
