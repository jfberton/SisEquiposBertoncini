<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="admin_importar_blanco.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.admin_importar_blanco" %>

<%@ Register Src="~/Aplicativo/Menues/menu_admin.ascx" TagPrefix="uc1" TagName="menu_admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .btn-file {
            position: relative;
            overflow: hidden;
        }

            .btn-file input[type=file] {
                position: absolute;
                top: 0;
                right: 0;
                min-width: 100%;
                min-height: 100%;
                font-size: 100px;
                text-align: right;
                filter: alpha(opacity=0);
                opacity: 0;
                outline: none;
                background: white;
                cursor: inherit;
                display: block;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_menu" runat="server">
    <uc1:menu_admin runat="server" ID="menu_admin" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_body" runat="server">
    <h2>Importar datos desde archivo</h2>

    <div class="editor-label">
        <div class="input-group">
            <span class="input-group-btn">
                <span class=" btn btn-default btn-file">Buscar
                    <input type="file" runat="server" name="data" id="data" />
                </span>
            </span>
            <input type="text" id="valdfil" class="form-control" readonly="true" />
            <span class="input-group-btn">
                <button class="btn btn-default" runat="server" onserverclick="btn_importar_Click">Importar</button></span>
        </div>
    </div>
    <br />
    <div id="div_resultados" runat="server" visible="false">
        <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
            <div class="panel panel-default">
                <div class="panel-heading" role="tab" id="headingOne">
                    <h4 class="panel-title">
                        <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">Datos ingresados
                        </a>
                    </h4>
                </div>
                <div id="collapseOne" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
                    <div class="panel-body">
                        <asp:GridView ID="GridView1" runat="server" OnRowDataBound="GridView1_RowDataBound">
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <div class="panel panel-default">
                <div class="panel-heading" role="tab" id="headingTwo">
                    <h4 class="panel-title">
                        <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">Registros correctos por insertar
        <asp:Label Text="" ID="lbl_cantidad_correctos" runat="server" />
                        </a>
                    </h4>
                </div>
                <div id="collapseTwo" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingTwo">
                    <div class="panel-body">
                        <asp:GridView ID="GridView2" runat="server">
                        </asp:GridView>
                        <br />
                        <button runat="server" class="btn btn-default" id="btn_impactar" onserverclick="btn_impactar_ServerClick">Impactar registros en detalles mensuales</button>
                    </div>
                </div>
            </div>
            <div class="panel panel-default">
                <div class="panel-heading" role="tab" id="headingThree">
                    <h4 class="panel-title">
                        <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseThree" aria-expanded="false" aria-controls="collapseThree">Registros correctos pero sin matchear
                        </a>
                    </h4>
                </div>
                <div id="collapseThree" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingThree">
                    <div class="panel-body">
                        <asp:GridView ID="GridView3" runat="server">
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_scripts" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            $(document).on('change', '.btn-file :file', function () {
                var input = $(this),
                    numFiles = input.get(0).files ? input.get(0).files.length : 1,
                    label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
                input.trigger('fileselect', [numFiles, label]);
            });

            $('.btn-file :file').on('fileselect', function (event, numFiles, label) {
                console.log(numFiles);
                console.log(label);
                $("#valdfil").val(label);
                $('#<%= div_resultados.ClientID %>').hide();
            });
        });

    </script>
</asp:Content>
