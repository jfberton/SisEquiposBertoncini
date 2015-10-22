<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="SisEquiposBertoncini._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="css/jumbotron-narrow.css" rel="stylesheet" />
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/mi_css.css" rel="stylesheet" />
    <link href="css/signin.css" rel="stylesheet" />

    <script src="js/jquery-1.11.3.min.js"></script>
    <script src="js/bootstrap.js"></script>

    <title></title>
</head>
<body>
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
</body>
</html>
