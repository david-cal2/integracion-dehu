$(function () {

    if (verComparecer != '0')
        $('#div-comparecer').removeClass("display-none");
    else
        $('#div-volver').removeClass("display-none");

    verMenuPorPerfil();

    $(document).ready(function () {
        actulizarCantidades();
        cargarComunicacionDetalle();
    });

    function actulizarCantidades() {
        $('#cantSinAsignar')[0].innerHTML = (listaCantidadesComunicaciones[0].Value > 99 ? '<span>+</span>99' : listaCantidadesComunicaciones[0].Value);
        $('#cantAsignadas')[0].innerHTML = (listaCantidadesComunicaciones[2].Value > 99 ? '<span>+</span>99' : listaCantidadesComunicaciones[2].Value);
        $('#cantLeidas')[0].innerHTML = (listaCantidadesComunicaciones[5].Value > 99 ? '<span>+</span>99' : listaCantidadesComunicaciones[5].Value);
    }

    function cargarComunicacionDetalle() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Comunicaciones/GetComunicacionDetalle',
            data: {
                identificador: identificador
            },
            dataType: "json",
            success: function (data) {
                if (data.hasOwnProperty('responseText')) {
                    toastr.error("Error en la llamada al servicio 'GetComunicacionDetalle'");
                }
                else {
                    $('#fechaDisposicion')[0].innerHTML = dateFormat(data.FechaPuestaDisposicion);
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
                }
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'GetComunicacionDetalle'");
            }
        });
    }

    function verMenuPorPerfil() {
        var $itemMenuComunicacionesEstado1 = $('#item-menu-comunicaciones-estado-1');
        var $itemMenuComunicacionesEstado3 = $('#item-menu-comunicaciones-estado-3');
        var $itemMenuComunicacionesEstado6 = $('#item-menu-comunicaciones-estado-6');

        if (idPerfil == Perfiles.Asignador) {
            $itemMenuComunicacionesEstado3.addClass("no-visible");
            $itemMenuComunicacionesEstado6.addClass("no-visible");
        }

        if (idPerfil == Perfiles.Gestor) {
            $itemMenuComunicacionesEstado1.addClass("display-none");
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
