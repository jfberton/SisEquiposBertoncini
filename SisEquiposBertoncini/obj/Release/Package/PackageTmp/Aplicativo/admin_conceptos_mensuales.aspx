<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="admin_conceptos_mensuales.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.admin_conceptos_mensuales" %>

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
            <table class="table-condensed" style="width: 100%">
                <tr>
                    <td>
                        <h1 class="panel-title">Conceptos mensuales</h1>
                    </td>
                    <td>
                        <button runat="server" id="btn_agregar_rubro_ingreso" causesvalidation="false" onserverclick="btn_agregar_rubro_ingreso_ServerClick" class="btn btn-success"><span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span>Agregar concepto ingreso</button>
                        <button runat="server" id="btn_agregar_rubro_egreso" causesvalidation="false" onserverclick="btn_agregar_rubro_egreso_ServerClick" class="btn btn-danger"><span class="glyphicon glyphicon-minus-sign" aria-hidden="true"></span>Agregar concepto egreso</button>
                    </td>
                </tr>
            </table>

        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-12" runat="server" id="div_tree">
                </div>
            </div>

            <div class="modal fade" id="nuevo_concepto" role="dialog" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title">Insertar nuevo concepto</h4> 
                        </div>
                        <div class="modal-body">
                            <div class="row">
                               <input type="hidden" runat="server" id="hidden_id_padre" value="0" />
                                <div class="col-md-12 text-center">
                                    <h2>
                                        <small>Pertenece a <label><asp:Label Text="" ID="lbl_concepto_padre" runat="server" /></label> del tipo <label><asp:Label Text="" ID="lbl_tipo_concepto" runat="server" /></label>
                                        </small>
                                    </h2>
                                </div>
                            </div>
                            <br />
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
                                            <td>Nombre del concepto</td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="col-md-8">
                                    <table class="table-condensed" style="width: 100%">
                                        <tr>
                                            <td style="width: auto">
                                                <input type="text" id="tb_nombre" class="form-control" runat="server" placeholder="Nombre del concepto" /></td>
                                            <td>
                                                <asp:RequiredFieldValidator ControlToValidate="tb_nombre" Text="<img src='../img/exclamation.gif' title='Debe ingresar el nombre del concepto' />"
                                                    ID="rv_nombre" runat="server" ErrorMessage="Debe ingresar el nombre del concepto">
                                                </asp:RequiredFieldValidator></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                    <table class="table-condensed">
                                        <tr><td>Descripción:</td></tr>
                                    </table>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <textarea rows="5" class="form-control" runat="server" id="tb_descripcion" placeholder="Descripción del concepto (no obligatorio)"></textarea>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button id="btn_guardar_nuevo_concepto" runat="server" onserverclick="btn_guardar_nuevo_concepto_ServerClick" class="btn btn-success" validationgroup="equipo">
                                <span class="glyphicon glyphicon-floppy-disk" aria-hidden="true"></span> Guardar!
                            </button>
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal fade" id="editar_concepto" role="dialog" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title">Insertar nuevo concepto</h4> 
                        </div>
                        <div class="modal-body">
                            <div class="row">
                               <input type="hidden" runat="server" id="hidden_id_concepto" value="0" />
                                <div class="col-md-12 text-center">
                                    <h2>
                                        <small>Pertenece a <label><asp:Label Text="" ID="lbl_padre_concepto" runat="server" /></label> del tipo <label><asp:Label Text="" ID="lbl_tipo_padre_concepto" runat="server" /></label>
                                        </small>
                                    </h2>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="BulletList"
                                        CssClass="validationsummary panel panel-danger" HeaderText="<div class='panel-heading'>&nbsp;Corrija los siguientes errores antes de continuar:</div>" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4">
                                    <table class="table-condensed" style="width: 100%">
                                        <tr>
                                            <td>Nombre del concepto</td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="col-md-8">
                                    <table class="table-condensed" style="width: 100%">
                                        <tr>
                                            <td style="width: auto">
                                                <input type="text" id="txt_editar_nombre" class="form-control" runat="server" placeholder="Nombre del concepto" /></td>
                                            <td>
                                                <asp:RequiredFieldValidator ControlToValidate="txt_editar_nombre" Text="<img src='../img/exclamation.gif' title='Debe ingresar el nombre del concepto' />"
                                                    ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe ingresar el nombre del concepto">
                                                </asp:RequiredFieldValidator></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                    <table class="table-condensed">
                                        <tr><td>Descripción:</td></tr>
                                    </table>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <textarea rows="5" class="form-control" runat="server" id="txt_editar_descripcion" placeholder="Descripción del concepto (no obligatorio)"></textarea>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button id="btn_editar_concepto" runat="server" onserverclick="btn_editar_concepto_ServerClick" class="btn btn-success" validationgroup="equipo">
                                <span class="glyphicon glyphicon-floppy-disk" aria-hidden="true"></span> Guardar!
                            </button>
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_scripts" runat="server">
    <script src="../js/jquery.treegrid.js"></script>
    <script src="../js/jquery.cookie.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.tree').treegrid({
                'initialState': 'collapsed',
                'saveState': true,
            });
        });

        
    </script>
</asp:Content>
