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
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_scripts" runat="server">
     <script src="../js/jquery.treegrid.js"></script>
    <script type="text/javascript">
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
