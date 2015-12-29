<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="valor_decimal.ascx.cs" Inherits="SisEquiposBertoncini.Aplicativo.Controles.valor_decimal" %>
<link href="../../css/bootstrap.css" rel="stylesheet" />


<div class="input-group">
    <asp:TextBox runat="server" ID="tb_valor" CssClass="form-control" />
    <span class="input-group-btn">
        <button class="btn btn-warning" runat="server" id="btn_edit" onserverclick="btn_edit_ServerClick"><span class="glyphicon glyphicon-edit"></span></button>
        <button class="btn btn-success" runat="server" id="btn_ok" onserverclick="btn_ok_ServerClick"><span class="glyphicon glyphicon-ok"></span></button>
        <button class="btn btn-danger" runat="server" id="btn_cancel" onserverclick ="btn_cancel_ServerClick"><span class="glyphicon glyphicon-remove"></span></button>
    </span>
</div>
