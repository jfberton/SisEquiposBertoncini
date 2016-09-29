<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="SisEquiposBertoncini._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="AdminLTE/assets/css/bootstrap.min.css"  rel="stylesheet" />
    <link href="AdminLTE/assets/css/grayscale.css" rel="stylesheet" />
    <link href="AdminLTE/assets/font-awesome/4.1.0/css/font-awesome.min.css" rel="stylesheet" />
    <link href="AdminLTE/assets/fonts/Lora.css" rel="stylesheet" />
    <link href="AdminLTE/assets/fonts/Montserrat.css" rel="stylesheet" />
    
    <script src="AdminLTE/assets/js/jquery.1.11.1.min.js"></script>
    <script src="AdminLTE/assets/js/bootstrap.min.js"></script>
    <script src="AdminLTE/assets/js/jquery.easing.min.js"></script>
    <script src="AdminLTE/assets/js/grayscale.js"></script>

     <title>Sistema Costos</title>
</head>
<body id="page-top" data-spy="scroll" data-target=".navbar-fixed-top">
    <!-- Intro Header -->
    <header class="intro">
        <div class="intro-body">
            <div class="container">
                <div class="row">
                    <div class="col-md-8 col-md-offset-2">
                        <h1 class="brand-heading">Costos</h1>
                        <p class="intro-text">
                            Sistema de administración de ingresos y egresos sobre empleados, equipos, conceptos
                            <br />
                        </p>

                        <br />
                        <form runat="server">
                            <div class="row">
                                <div class="col-md-6 col-md-offset-3">
                                    <input type="text" runat="server" id="inputUsr" class="form-control text-center" placeholder="Usuario" />
                                    <br />
                                    <input type="password" runat="server" id="inputPassword" class="form-control text-center" placeholder="Clave" />
                                    <br />
                                    <asp:Button Text="Ingresar" runat="server" CssClass="btn btn-default btn-block" OnClick="btn_ingresar_Click" ID="btn_ingresar" />
                                </div>
                            </div>
                           <%-- <div class="row">
                                <br />
                                <p>
                                    <small>- Administración
                                        <asp:Label Text="text" ID="lbl_anio" runat="server" />
                                        -</small>
                                    <br />
                                    <asp:Button Text="ver demo" ID="btn_ver_demo" OnClick="btn_ver_demo_Click" runat="server" />
                                </p>
                            </div>--%>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </header>


    <div class="modal fade" id="error_acceso" role="dialog" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content panel-danger">
                <div class="modal-header panel-heading">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="alert-danger"><span class="glyphicon glyphicon-remove-sign" aria-hidden="true"></span>ATENCIÓN</h4>
                </div>

                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12">
                            <p>
                                Las credenciales ingresadas son incorrectas
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
<%--<body>
    <div class="container">
        <div class="jumbotron">
            <h1>Sistema Web</h1>

            <p class="lead">Administración de horas y equipos</p>
            <form class="form-signin" id="form1" runat="server">
                <h2 class="form-signin-heading">Acceso al sitio</h2>
                <label for="inputUsr" class="sr-only">Usuario</label>
                <input type="text" runat="server" id="inputUsr" class="form-control" placeholder="Usuario" required autofocus />
                <label for="inputPassword" class="sr-only">Contraseña</label>
                <input type="password" runat="server" id="inputPassword" class="form-control" placeholder="Contraseña" required />
                <asp:Button Text="Ingresar" CssClass="btn btn-lg btn-primary btn-block" ID="btn_ingresar" OnClick="btn_ingresar_Click" runat="server" />
            </form>
        </div>
    </div>
</body>--%>
</html>
