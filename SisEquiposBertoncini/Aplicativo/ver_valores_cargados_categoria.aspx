<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ver_valores_cargados_categoria.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.ver_valores_cargados_categoria" %>
<%@ Register Src="~/Aplicativo/Menues/menu_admin.ascx" TagPrefix="uc1" TagName="menu_admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/jquery.treegrid.css" rel="stylesheet" />
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
                                    <asp:ListItem Text="2015" />
                                    <asp:ListItem Text="2015" />
                                    <asp:ListItem Text="2015" />
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
                                <asp:Button Text="Imprimir Resumen" ID="btn_imprimir_resumen" CssClass="btn btn-default" OnClick="btn_imprimir_resumen_Click" runat="server" />
                                <asp:Button Text="Nueva búsqueda" runat="server" ID="btn_nueva_busqueda" CssClass="btn btn-danger" OnClick="btn_nueva_busqueda_Click" />
                            </td>
                        </tr>
                    </table>
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
                <div class="col-md-12">
                </div>
            </div>
        </div>
    </div>

    <div runat="server" id="div_tree" style="width: 2800px; overflow-x: scroll">
        
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_scripts" runat="server">
    <script src="../js/jquery.treegrid.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.tree').treegrid();
        });
    </script>

</asp:Content>

