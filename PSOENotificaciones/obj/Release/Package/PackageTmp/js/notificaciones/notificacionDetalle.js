$(function () {

    if (verComparecer != '0')
        $('#div-comparecer').removeClass("display-none");
    else
        $('#div-volver').removeClass("display-none");

    verMenuPorPerfil();

    $(document).ready(function () {
        actulizarCantidades();
        cargarNotificacionDetalle();

        if (idPerfil == Perfiles.Asignador || idPerfil == Perfiles.UsuarioConsulta)
            $('#lnk-comparecer').hide();
    });

    function actulizarCantidades() {
        $('#cantSinAsignar')[0].innerHTML = (listaCantidadesNotificaciones[0].Value > 99 ? '<span>+</span>99' : listaCantidadesNotificaciones[0].Value);
        $('#cantEnAlerta')[0].innerHTML = (listaCantidadesNotificaciones[1].Value > 99 ? '<span>+</span>99' : listaCantidadesNotificaciones[1].Value);
        $('#cantAsignadas')[0].innerHTML = (listaCantidadesNotificaciones[2].Value > 99 ? '<span>+</span>99' : listaCantidadesNotificaciones[2].Value);
        $('#cantComparecidas')[0].innerHTML = (listaCantidadesNotificaciones[3].Value > 99 ? '<span>+</span>99' : listaCantidadesNotificaciones[3].Value);
        $('#cantCaducadas')[0].innerHTML = (listaCantidadesNotificaciones[4].Value > 99 ? '<span>+</span>99' : listaCantidadesNotificaciones[4].Value);
        $('#cantExternas')[0].innerHTML = (listaCantidadesNotificaciones[6].Value > 99 ? '<span>+</span>99' : listaCantidadesNotificaciones[6].Value);
    }

    function cargarNotificacionDetalle() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Notificaciones/GetNotificacionDetalle',
            data: {
                identificador: identificador
            },
            dataType: "json",
            success: function (data) {
                if (data.hasOwnProperty('responseText')) {
                    toastr.error("Error en la llamada al servicio 'GetNotificacionDetalle'");
                }
                else {
                    $('#fechaDisposicion')[0].innerHTML = dateFormat(data.FechaPuestaDisposicion);
                    $('#fechaCaducidad')[0].innerHTML = dateFormat(data.FechaCaducidad);
                    $('#identificador')[0].innerHTML = data.Identificador;
                    $('#asunto')[0].innerHTML = data.Concepto;
                    $('#titular')[0].innerHTML = data.Personas.NombreTitular;
                    $('#organismoEmisor')[0].innerHTML = data.OrganismosEmisores.OrganismoEmisor;
                    $('#vinculo')[0].innerHTML = data.Vinculos.Descripcion;
                    $('#organismoEmisorRaiz')[0].innerHTML = data.OrganismosEmisores.OrganismoRaiz;
                    $('#descripcion')[0].innerHTML = data.Descripcion;
                    $('#textMultiObservaciones')[0].value = data.Observaciones;

                    $('#fechaDisposicion2')[0].innerHTML = dateFormat(data.FechaPuestaDisposicion);
                    $('#asunto2')[0].innerHTML = data.Concepto;
                    $('#organismoEmisor2')[0].innerHTML = data.OrganismosEmisores.OrganismoEmisor;

                    if (data.MetadatosPublicos != null && data.MetadatosPublicos != "")
                        $('#metadatosPublicos')[0].innerHTML = decodeURIComponent(escape(window.atob(data.MetadatosPublicos)));
                    else
                        $('#div-metadatos-publicos').addClass("display-none");
                    
                    if (data.EstadosEnvio.ID == Estados.Externas) {
                        $('#titulo-detalle')[0].innerText = "Detalle de la notificación (comparecida externamente)";
                        $('.caducidad').addClass("display-none");
                        $('#lnk-comparecer').addClass("display-none");
                    }

                    if (idPerfil == Perfiles.Asignador) {
                        $('#lnk-comparecer').addClass("display-none");
                    }
                }
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'GetNotificacionDetalle'");
            }
        });
    }

    function verMenuPorPerfil() {
        var $itemMenuNotificacionesEstado1 = $('#item-menu-notificaciones-estado-1');
        var $itemMenuNotificacionesEstado2 = $('#item-menu-notificaciones-estado-2');
        var $itemMenuNotificacionesEstado3 = $('#item-menu-notificaciones-estado-3');
        var $itemMenuNotificacionesEstado4 = $('#item-menu-notificaciones-estado-4');
        var $itemMenuNotificacionesEstado5 = $('#item-menu-notificaciones-estado-5');
        var $itemMenuNotificacionesEstado7 = $('#item-menu-notificaciones-estado-7');

        if (idPerfil == Perfiles.Asignador)
        {
            $itemMenuNotificacionesEstado2.addClass("no-visible");
            $itemMenuNotificacionesEstado3.addClass("no-visible");
            $itemMenuNotificacionesEstado4.addClass("no-visible");
            $itemMenuNotificacionesEstado5.addClass("no-visible");
            $itemMenuNotificacionesEstado7.addClass("no-visible");
        }

        if (idPerfil == Perfiles.Gestor)
        {
            $itemMenuNotificacionesEstado1.addClass("display-none");
        }

        $('.menu-lateral').removeClass("no-visible");
    }

    $('#textMultiObservaciones').change(function (e) {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Notificaciones/SetObservaciones',
            data: {
                identificador: identificador,
                observaciones: $(this)[0].value
            },
            dataType: "json",
            success: function (data) {
                if (data.hasOwnProperty('responseText')) {
                    toastr.error("Error en la llamada al servicio 'SetObservaciones'");
                }
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'SetObservaciones'");
            }
        });
    });
})
