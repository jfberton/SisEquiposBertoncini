<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="usr_modifica_datos.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.usr_modifica_datos" %>

<%@ Register Src="~/Aplicativo/Menues/menu_admin.ascx" TagPrefix="uc1" TagName="menu_admin" %>
<%@ Register Src="~/Aplicativo/Controles/imagen_usuario.ascx" TagPrefix="uc1" TagName="imagen_usuario" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_menu" runat="server">
    <uc1:menu_admin runat="server" ID="menu_admin" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_body" runat="server">
    <div class="panel panel-default">
        <div class="panel-heading">
            <h1 class="panel-title">Editar datos de usuario</h1>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-3">
                    <div class="row">
                        <div class="col-md-12">
                            <a href="#" data-toggle="modal" data-target="#editar_Imagen">
                                <uc1:imagen_usuario runat="server" ID="imagen_usuario" />
                            </a>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <small>Click en la imagen para cambiar</small>
                        </div>
                    </div>
                    <div class="modal fade" id="editar_Imagen" role="dialog" aria-hidden="true">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                    <h4 class="modal-title">Editar foto carnet</h4>
                                </div>
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="thumbnail">
                                                <img src="..." alt="..." id="imagen_agente">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="input-group">
                                                <span class="input-group-btn">
                                                    <span class="btn btn-primary btn-file">Seleccionar&hellip;
                                                          <input type="file" id="archivo_imagen" runat="server" accept=".jpg,.png,.gif" onchange="Previsualizar();" />
                                                    </span>
                                                </span>
                                                <input type="text" class="form-control" readonly>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button Text="Aceptar" ID="btn_aceptar_imagen" OnClick="btn_aceptar_imagen_Click" CssClass="btn btn-success" runat="server" />
                                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-9">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="row">
                                <div class="col-md-12">
                                    <label>Nombre y apellido</label>
                                    <asp:Label Text="" ID="lbl_usuario_nombre" runat="server" />
                                    <button type="button" class="btn btn-sm btn-warning" id="btn_editar_usuario_nombre" runat="server" data-toggle="modal" data-target="#editar_nombre_de_usuario">
                                        Editar
                                    </button>
                                    <div class="modal fade" id="editar_nombre_de_usuario" role="dialog" aria-hidden="true">
                                        <div class="modal-dialog">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                    <h4 class="modal-title">Editar nombre y apellido del usuario</h4>
                                                </div>
                                                <div class="modal-body">
                                                    <div class="row">
                                                        <div class="col-md-3">Nombre y apellido</div>
                                                        <div class="col-md-9">
                                                            <input type="text" runat="server" id="tb_usuario_nombre" class="form-control" placeholder="ingrese nombre y apellido " />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="modal-footer">
                                                    <asp:Button Text="Aceptar" ID="btn_aceptar_usuario_nombre" OnClick="btn_aceptar_usuario_nombre_Click" CssClass="btn btn-success" runat="server" />
                                                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="panel panel-default">
                                        <div class="panel-heading">
                                            <h1 class="panel-title">Datos de acceso</h1>
                                        </div>
                                        <div class="panel-body">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <label>Usuario</label>
                                                    <asp:Label Text="" ID="lbl_usr" runat="server" />
                                                    <button type="button" class="btn btn-sm btn-warning" id="btn_editar_usuario_usr" runat="server" data-toggle="modal" data-target="#editar_usuario_usr">
                                                        Editar
                                                    </button>
                                                    <div class="modal fade" id="editar_usuario_usr" role="dialog" aria-hidden="true">
                                                        <div class="modal-dialog">
                                                            <div class="modal-content">
                                                                <div class="modal-header">
                                                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                                    <h4 class="modal-title">Editar acceso del usaurio</h4>
                                                                </div>
                                                                <div class="modal-body">
                                                                    <div class="row">
                                                                        <div class="col-md-3">Acceso</div>
                                                                        <div class="col-md-9">
                                                                            <input type="text" runat="server" id="tb_usuario_usr" class="form-control" placeholder="ingrese acceso " />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="modal-footer">
                                                                    <asp:Button Text="Aceptar" ID="btn_aceptar_usuario_usr" OnClick="btn_aceptar_usuario_usr_Click" CssClass="btn btn-success" runat="server" />
                                                                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <button type="button" class="btn btn-sm btn-warning" id="btn_resetear_clave" runat="server" data-toggle="modal" data-target="#resetear_clave">
                                                        Resetear clave
                                                    </button>
                                                    <div class="modal fade" id="resetear_clave" role="dialog" aria-hidden="true">
                                                        <div class="modal-dialog">
                                                            <div class="modal-content">
                                                                <div class="modal-header">
                                                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                                    <h4 class="modal-title">Resetear clave de acceso</h4>
                                                                </div>
                                                                <div class="modal-body">
                                                                    <div class="row">
                                                                        <div class="col-md-3">Ingrese nueva clave</div>
                                                                        <div class="col-md-9">
                                                                            <input type="text" runat="server" id="tb_pass1" class="form-control" placeholder="Ingrese nueva clav" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-3">Repita la clave</div>
                                                                        <div class="col-md-9">
                                                                            <input type="text" runat="server" id="tb_pass2" class="form-control" placeholder="Repita la clave" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="modal-footer">
                                                                    <asp:Button Text="Confirmar" ID="btn_aceptar_reseteo_de_clave" OnClick="btn_aceptar_reseteo_de_clave_Click" CssClass="btn btn-success" runat="server" />
                                                                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_scripts" runat="server">
    <script>
        $(document).on('change', '.btn-file :file', function () {
            var input = $(this),
                numFiles = input.get(0).files ? input.get(0).files.length : 1,
                label = input.val().replace(/\\/g, '/').replace(/.*\//, '');

            var div = input.parent(0).parent(0).parent(0).parent(0);
            if (div.children().length == 2) {
                div.children()[1].remove();
            }

            if (input.get(0).files[0].size > 3145728) {
                var alerta = document.createElement("div");
                alerta.nodeName = "alerta";
                alerta.className = "label label-danger";
                alerta.innerHTML = "El archivo es demasiado grande (supera los 3mb), no será procesado!";
                div.append(alerta);
            }
            else {
                var correcto = document.createElement("div");
                correcto.nodeName = "alerta";
                correcto.className = "label label-success";
                correcto.innerHTML = "Tamaño de archivo válido.";
                div.append(correcto);
            }

            input.trigger('fileselect', [numFiles, label]);
        });

        $(document).ready(function () {
            $('.btn-file :file').on('fileselect', function (event, numFiles, label) {

                var input = $(this).parents('.input-group').find(':text'),
                    log = numFiles > 1 ? numFiles + ' files selected' : label;

                if (input.length) {
                    input.val(log);
                } else {
                    if (log) alert(log);
                }

            });
        });

        function Previsualizar() {
            var preview = document.getElementById("imagen_agente");
            var file = document.getElementById("<%= archivo_imagen.ClientID %>").files[0];
            var reader = new FileReader();

            reader.onloadend = function () {
                preview.src = reader.result;
            }

            if (file) {
                reader.readAsDataURL(file);
            } else {
                preview.src = "";
            }
        }
    </script>
</asp:Content>
