var $itemComunicaciones = $('#item-comunicaciones');
var $itemNotificaciones = $('#item-notificaciones');

var $itemMenuNotificaciones1 = $('#item-menu-notificaciones-1');
var $itemMenuNotificaciones2 = $('#item-menu-notificaciones-2');
var $itemMenuNotificaciones3 = $('#item-menu-notificaciones-3');
var $itemMenuNotificaciones4 = $('#item-menu-notificaciones-4');
var $itemMenuNotificaciones5 = $('#item-menu-notificaciones-5');
var $itemMenuNotificaciones7 = $('#item-menu-notificaciones-7');

var $itemMenuComunicaciones1 = $('#item-menu-comunicaciones-1');
var $itemMenuComunicaciones2 = $('#item-menu-comunicaciones-2');
var $itemMenuComunicaciones6 = $('#item-menu-comunicaciones-6');

var $menuAdministracion = $('#menu-administracion');
var $menuConfiguracion = $('#menu-configuracion');

var $fechaDesde = $('#fechaDesde');
var $fechaHasta = $('#fechaHasta');

$(function () {

    if ((typeof idTipoEnvios !== 'undefined') && (idTipoEnvios != 0)) {
        if (idTipoEnvios == 1) {
            $itemComunicaciones.addClass('active');
            $itemNotificaciones.removeClass('active');
        }

        if (idTipoEnvios == 2) {
            $itemNotificaciones.addClass('active');
            $itemComunicaciones.removeClass('active');
        }
    }
    else {
        $itemNotificaciones.removeClass('active');
        $itemComunicaciones.removeClass('active');
    }

    if (idPerfil == Perfiles.Asignador)
    {
        $itemMenuNotificaciones2.addClass("display-none");
        $itemMenuNotificaciones3.addClass("display-none");
        $itemMenuNotificaciones4.addClass("display-none");
        $itemMenuNotificaciones5.addClass("display-none");
        $itemMenuNotificaciones7.addClass("display-none");

        $itemMenuComunicaciones2.addClass("display-none");
        $itemMenuComunicaciones6.addClass("display-none");

        $menuAdministracion.addClass("display-none");
        $menuConfiguracion.addClass("display-none");
    }

    if (idPerfil == Perfiles.Gestor)
    {
        $itemMenuNotificaciones1.addClass("display-none");
        $itemMenuNotificaciones7.addClass("display-none");

        $itemMenuComunicaciones1.addClass("display-none");

        $menuAdministracion.addClass("display-none");
        $menuConfiguracion.addClass("display-none");
    }

    if (idPerfil == Perfiles.UsuarioConsulta)
    {
        $menuAdministracion.addClass("display-none");
        $menuConfiguracion.addClass("display-none");
    }

    if ($.isEmptyObject($fechaDesde)) {
        $($fechaDesde).datepicker({
            format: 'dd/mm/yyyy'
        });
    }
    
    if ($.isEmptyObject($fechaHasta)) {
        $($fechaHasta).datepicker({
            format: 'dd/mm/yyyy'
        });
    }
    
    $('.icon-calendar-1').click(function (e) {
        $fechaDesde.datepicker().focus();
    });

    $('.icon-calendar-2').click(function (e) {
        $fechaHasta.datepicker().focus();
    });

    $.ajax({
        async: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Home/GetInfoUsuario",
        dataType: "json",
        success: function (data) {
            $("#menu-siglas-nombre")[0].innerText = data.siglas;
            $("#menu-login-usuario")[0].innerText = data.login;
            $("#menu-email-usuario")[0].innerText = data.email;
        },
        error: function (data) {
            toastr.error("Error en la llamada al servicio 'GetInfoUsuario'");
        }
    });

    $('#btn-ayuda').click(function () {
        $('#ayudaModal').modal({ backdrop: 'static', keyboard: false });
        $('#ayudaModal').modal('show');
    })

    $('#btn-cerrar-sesion').click(function () {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Home/CerrarSesion',
            dataType: "json",
            success: function (data) {
                window.location.href = "../Home/Login";
            }
        });
    });
})