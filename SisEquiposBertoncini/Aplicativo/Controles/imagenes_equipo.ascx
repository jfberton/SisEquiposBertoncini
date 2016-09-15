<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="imagenes_equipo.ascx.cs" Inherits="SisEquiposBertoncini.Aplicativo.Controles.imagenes_equipo" %>
<ul runat="server" id="imgs_equipo" class="bxslider" style="max-height:350px">

</ul>

<script>
    $(document).ready(function () {
        slider = $('.bxslider').bxSlider({ mode: 'fade', adaptiveHeight: true, auto: true });

        $('.bxslider').imagesLoaded().always(function (instance) {
            slider.reloadSlider();
        });
    });
</script>
