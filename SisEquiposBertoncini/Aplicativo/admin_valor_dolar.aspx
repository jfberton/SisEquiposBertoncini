<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="admin_valor_dolar.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.admin_valor_dolar" %>

<%@ Register Src="~/Aplicativo/Menues/menu_admin.ascx" TagPrefix="uc1" TagName="menu_admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_menu" runat="server">
    <uc1:menu_admin runat="server" ID="menu_admin" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_body" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    <div class="panel panel-default">
        <div class="panel-heading">
            <table class="table-condensed">
                <tr>
                    <td>
                        <h1 class="panel-title">Valores históricos dolar</h1>
                    </td>
                    <td>Año</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddl_anio" class="form-control" onchange="Selecciono_anio(this);">
                            <asp:ListItem Text="Seleccione año:" />
                        </asp:DropDownList></td>
                </tr>
            </table>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-12">
                    <table class="table-condensed" runat="server" id="tabla_valores_dolar_anio">
                        <tr>
                            <td>
                                <label>Enero</label></td>
                            <td>
                                <asp:TextBox runat="server" ID="valor_mes_1" onkeypress="Modifica_valor(this, event)" CssClass="form-control money" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label>Febrero</label></td>
                            <td>
                                <asp:TextBox runat="server" ID="valor_mes_2" onkeypress="Modifica_valor(this, event)" CssClass="form-control money" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label>Marzo</label></td>
                            <td>
                                <asp:TextBox runat="server" ID="valor_mes_3" onkeypress="Modifica_valor(this, event)" CssClass="form-control money" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label>Abril</label></td>
                            <td>
                                <asp:TextBox runat="server" ID="valor_mes_4" onkeypress="Modifica_valor(this, event)" CssClass="form-control money" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label>Mayo</label></td>
                            <td>
                                <asp:TextBox runat="server" ID="valor_mes_5" onkeypress="Modifica_valor(this, event)" CssClass="form-control money" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label>Junio</label></td>
                            <td>
                                <asp:TextBox runat="server" ID="valor_mes_6" onkeypress="Modifica_valor(this, event)" CssClass="form-control money" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label>Julio</label></td>
                            <td>
                                <asp:TextBox runat="server" ID="valor_mes_7" onkeypress="Modifica_valor(this, event)" CssClass="form-control money" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label>Agosto</label></td>
                            <td>
                                <asp:TextBox runat="server" ID="valor_mes_8" onkeypress="Modifica_valor(this, event)" CssClass="form-control money" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label>Septiembre</label></td>
                            <td>
                                <asp:TextBox runat="server" ID="valor_mes_9" onkeypress="Modifica_valor(this, event)" CssClass="form-control money" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label>Octubre</label></td>
                            <td>
                                <asp:TextBox runat="server" ID="valor_mes_10" onkeypress="Modifica_valor(this, event)" CssClass="form-control money" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label>Noviembre</label></td>
                            <td>
                                <asp:TextBox runat="server" ID="valor_mes_11" onkeypress="Modifica_valor(this, event)" CssClass="form-control money" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label>Diciembre</label></td>
                            <td>
                                <asp:TextBox runat="server" ID="valor_mes_12" onkeypress="Modifica_valor(this, event)" CssClass="form-control money" /></td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_scripts" runat="server">
    <script type="text/javascript">
        function Selecciono_anio(ddl) {
            if (ddl.selectedIndex > 0)
            {
                var url
                if (window.location.href.indexOf("?") > -1) {
                    url = window.location.href.split("=")[0] + "=" + ddl.value;
                    
                }
                else {
                    url = window.location.href + "?aa=" + ddl.value;
                }

                window.location = url;
            }
        }

        function Modifica_valor(obj, e) {
            if (e.keyCode == 13) {
                var mes = parseInt(obj.id.split('_')[obj.id.split('_').length - 1]);
                var valorstr = obj.value.replace('.', '').replace(',', '.');
                var valor = parseFloat(valorstr).toFixed(2);
                var anio = parseInt(document.getElementById("<%= ddl_anio.ClientID %>").value);
                PageMethods.ActualizarValor(mes, anio, valor, OnSuccessCallback, OnFailureCallback);
            }
        }

        function OnSuccessCallback(res) {
            //alert(res);
            location.reload();
        }

        $(document).ready(function () {
            $('.money').mask('000.000.000,00', { reverse: true });
        });


        function OnFailureCallback() {
            alert('Error');
        }
    </script>
</asp:Content>
