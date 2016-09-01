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
    <h3>Planilla de combustibles</h3>

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
        <div class="col-md-12">
            <asp:GridView ID="gv_combustible" runat="server" OnPreRender="gv_combustible_PreRender"
                AutoGenerateColumns="False" GridLines="None" CssClass="display">
                <Columns>
                    <asp:BoundField DataField="fecha" HeaderText="Fecha" ReadOnly="true" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="chofer" HeaderText="Chofer" ReadOnly="true" />
                    <asp:CheckBoxField  DataField="tanque_lleno" HeaderText="Tanque lleno" ReadOnly="true" />
                    <asp:BoundField DataField="litros" HeaderText="Litros" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="km" HeaderText="Kilometros" ReadOnly="true" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,#}" />
                    <asp:BoundField DataField="promedio" HeaderText="Promedio" ReadOnly="true" DataFormatString="{0:#,#}" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="costo_total_facturado" HeaderText="Costo" ReadOnly="true" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" />
                </Columns>
            </asp:GridView>
        </div>
    </div>


</asp:Content>


<asp:Content ID="Content5" ContentPlaceHolderID="cph_scripts" runat="server">
    <script src="../js/jquery.dataTables.min.js"></script>
    <script src="../js/dataTables.bootstrap.min.js"></script>

    <script>
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
    </script>
</asp:Content>
