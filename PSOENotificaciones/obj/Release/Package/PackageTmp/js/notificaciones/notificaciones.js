var $inputIdentificador = $('#inputIdentificador');
var $selectAdministracionPublica = $('#selectAdministracionPublica');
var $selectOrganismosRaiz = $('#selectOrganismosRaiz');
var $selectOrganismosEmisores = $('#selectOrganismosEmisores');
var $selectEstados = $('#selectEstados');
var $selectAsuntos = $('#selectAsuntos');
var $selectGrupos = $('#selectGrupos');
var $selectGruposModal = $('#selectGruposModal');

$(function () {

    verMenuPorPerfil();

    $(document).ready(function () {
        setTimeout(cargarSelectAdministracionPublica, 500);
        setTimeout(cargarSelectEstados, 500);
        setTimeout(cargarSelectAsuntos, 500);
        setTimeout(cargarSelectGrupos, 500);

        cargarNotificaciones();
        habilitarElementos();

        $('#btn-buscar').click(function (e) {
            e.preventDefault();

            if ($selectEstados.val() == '' && $inputIdentificador.val() == '' && $selectAdministracionPublica.val() == '' &&
                $selectOrganismosRaiz.val() == '' && $selectOrganismosEmisores.val() == '' && $selectAsuntos.val() == '' &&
                $selectGrupos.val() == '' && $fechaDesde.val() == '' && $fechaHasta.val() == '') {
                setTimeout(cargarNotificaciones, 1);
                $("#overlay").show();
            }
            else {
                var data = {
                    idEstados: ($selectEstados.val() == '' ? JSON.stringify(getEstadosPorPerfil()) : $selectEstados.val()),
                    identificador: ($inputIdentificador.val() == '' ? null : $inputIdentificador.val()),
                    administracionPublica: ($selectAdministracionPublica.val() == '' ? null : $selectAdministracionPublica.val()),
                    organismoRaiz: ($selectOrganismosRaiz.val() == '' ? null : $selectOrganismosRaiz.val()),
                    idOrganoEmisor: ($selectOrganismosEmisores.val() == '' ? null : $selectOrganismosEmisores.val()),
                    asunto: ($selectAsuntos.val() == '' ? null : $selectAsuntos.val()),
                    idGrupo: ($selectGrupos.val() == '' ? null : $selectGrupos.val()),
                    fechaDesde: ($fechaDesde.val() == '' ? null : $fechaDesde.val()),
                    fechaHasta: ($fechaHasta.val() == '' ? null : $fechaHasta.val())
                };

                setTimeout(cargarNotificaciones, 1, data);
                $("#overlay").show();
            }
        });

        $('#btn-limpiar').click(function (e) {
            $('#inputIdentificador')[0].value = "";
            $('#selectAdministracionPublica').selectpicker('val', '');
            cargarSelectOrganismoRaiz('');
            cargarSelectOrganismosEmisores('');
            $('#selectEstados').selectpicker('val', '');
            $('#selectAsuntos').selectpicker('val', '');
            $('#selectGrupos').selectpicker('val', '');
            $('#fechaDesde')[0].value = "";
            $('#fechaHasta')[0].value = "";
        });

        $('#table').on('dbl-click-cell.bs.table', function (field, value, row, $element) {
            if ($element.EstadosEnvio.ID != Estados.Comparecidas)
                window.location.href = "../Notificaciones/NotificacionDetalle?identificador=" + $element.Identificador + "&c=0";
            else
                window.location.href = "../Notificaciones/NotificacionComparecida?identificador=" + $element.Identificador;
        })

        $('#btn-agregar-grupo').click(function (e) {
            var idGrupo = $('#selectGruposModal').val();

            if (idGrupo != "") {
                var agregar = 1;
                var listElements = document.querySelectorAll("#gruposAsignados li");
                for (var i = 0; (li = listElements[i]); i++) {
                    if (li.id == idGrupo)
                        agregar = 0;
                }

                if (agregar == 1) {
                    $.ajax({
                        async: false,
                        type: 'GET',
                        contentType: 'application/json; charset=utf-8',
                        url: '/Base/GetGrupoById',
                        data: {
                            idGrupo: idGrupo
                        },
                        dataType: "json",
                        success: function (data) {
                            $("#gruposAsignados").append("<li id='" + data.ID + "'><strong class='eliminar-grupo' title='Quitar' onclick=\"javascript:eliminarGrupoAsignado('" + data.ID + "')\">X</strong>" + data.Nombre + "</li>");
                        },
                        error: function (data) {
                            toastr.error("Error en la llamada al servicio 'GetGrupoById'");
                        }
                    });
                }
            }
        });

        $('#btn-guardar-grupos').click(function (e) {
            e.preventDefault();
            $('#asignarGrupoModal').modal('hide');
            setTimeout(guardarGruposAsignadosPorEnvio, 1);
            $("#overlay").show();
        });

        function habilitarElementos() {
            if (idPerfil == Perfiles.Asignador || idPerfil == Perfiles.UsuarioConsulta)
                $('.btn-comparecer').addClass("display-none");

            if (idPerfil == Perfiles.Gestor || idPerfil == Perfiles.UsuarioConsulta)
                $('.btn-abrir-modal-asignar-grupo').addClass("display-none");

            //select asignada a
            if (idPerfil == Perfiles.Gestor || idPerfil == Perfiles.Asignador)
                $('#div-asignada').addClass("display-none");
        }

        function guardarGruposAsignadosPorEnvio() {
            var listaIdGrupos = '';
            var listElements = document.querySelectorAll("#gruposAsignados li");

            for (var i = 0; (li = listElements[i]); i++) {
                listaIdGrupos += li.id + ",";
            }

            $.ajax({
                async: false,
                type: 'GET',
                contentType: 'application/json; charset=utf-8',
                url: '/Notificaciones/AsignarGrupoEnvio',
                data: {
                    identificador: identificadorAsignar,
                    idGrupos: listaIdGrupos
                },
                dataType: "json",
                success: function (data) {
                    if (data.Success == "True") {
                        $(".cadena-" + identificadorAsignar)[0].innerText = data.cadenaGrupos;
                        //$("#overlay").hide();
                        cargarNotificaciones();
                        habilitarElementos();
                    }
                    else {
                        $("#overlay").hide();
                        toastr.error("Error en la llamada al servicio 'AsignarGrupoEnvio'");
                    }
                },
                error: function (data) {
                    $("#overlay").hide();
                    toastr.error("Error en la llamada al servicio 'AsignarGrupoEnvio'");
                }
            });
        }

        function cargarNotificaciones(data) {
            var urlFuncion = '/Notificaciones/GetNotificacionesBuscadorComplejo';

            var cargarCantidades = false;
            if (data == null) {
                cargarCantidades = true;
                urlFuncion = '/Notificaciones/GetNotificaciones';
                data = {
                    idEstados: JSON.stringify(getEstadosPorPerfil())
                };
            }

            $.ajax({
                async: false,
                type: 'GET',
                contentType: 'application/json; charset=utf-8',
                url: urlFuncion,
                data: data,
                dataType: "json",
                success: function (data) {
                    if (data.hasOwnProperty('responseText')) {
                        $("#overlay").hide();
                        toastr.error("Error en la llamada al servicio '" + urlFuncion + "'");
                    }
                    else {
                        if (cargarCantidades && data.CantidadesTotales != null) {
                            var $cantSinAsignar = $('#cantSinAsignar');
                            var $cantEnAlerta = $('#cantEnAlerta');
                            var $cantAsignadas = $('#cantAsignadas');
                            var $cantComparecidas = $('#cantComparecidas');
                            var $cantCaducadas = $('#cantCaducadas');
                            var $cantExternas = $('#cantExternas');

                            $cantSinAsignar[0].innerHTML = data.CantidadesTotales[0].Value,
                            $cantEnAlerta[0].innerHTML = data.CantidadesTotales[1].Value;
                            $cantAsignadas[0].innerHTML = data.CantidadesTotales[2].Value;
                            $cantComparecidas[0].innerHTML = data.CantidadesTotales[3].Value;
                            $cantCaducadas[0].innerHTML = data.CantidadesTotales[4].Value;
                            $cantExternas[0].innerHTML = data.CantidadesTotales[6].Value;

                            $('#tituloPantallaInicio')[0].innerHTML = "Notificaciones recibidas en los últimos " + data.NumeroUltimosDias + " días";
                        }

                        var $collapseOne = $('#collapseOne');
                        var $divContenedor = $('#div-contenedor');
                        var $divContenedorVacio = $('#div-contenedor-vacio');

                        if (data.ListaEnvios != null && data.ListaEnvios.length > 0) {
                            $divContenedor.removeClass('display-none')
                            $divContenedorVacio.addClass('display-none');

                            var $table = $('#table');
                            $table.bootstrapTable('removeAll');
                            $table.bootstrapTable('load', data.ListaEnvios);
                            export_fileName = 'Notificaciones';
                            $('#regPagina').innerText = "Notificaciones por página";
                        }
                        else {
                            $collapseOne.removeClass('collapse');
                            $divContenedorVacio.removeClass('display-none');
                            $divContenedor.addClass('display-none');
                        }

                        $("#overlay").hide();
                    }
                },
                error: function (data) {
                    $("#overlay").hide();
                    toastr.error("Error en la llamada al servicio '" + urlFuncion + "'");
                }
            });
        }
    });

    function verMenuPorPerfil() {
        var $btnNotificacionesEstado1 = $('#btn-notificaciones-estado-1');
        var $btnNotificacionesEstado2 = $('#btn-notificaciones-estado-2');
        var $btnNotificacionesEstado3 = $('#btn-notificaciones-estado-3');
        var $btnNotificacionesEstado4 = $('#btn-notificaciones-estado-4');
        var $btnNotificacionesEstado5 = $('#btn-notificaciones-estado-5');
        var $btnNotificacionesEstado7 = $('#btn-notificaciones-estado-7');

        if (idPerfil == Perfiles.Asignador) {
            $btnNotificacionesEstado2.addClass("no-visible");
            $btnNotificacionesEstado3.addClass("no-visible");
            $btnNotificacionesEstado4.addClass("no-visible");
            $btnNotificacionesEstado5.addClass("no-visible");
            $btnNotificacionesEstado7.addClass("no-visible");
        }

        if (idPerfil == Perfiles.Gestor) {
            $btnNotificacionesEstado1.addClass("display-none");
        }
    }

    function getEstadosPorPerfil() {
        var listaEstado = [];

        if (idPerfil == Perfiles.Administrador || idPerfil == Perfiles.UsuarioConsulta) {
            listaEstado.push(Estados.SinAsignar);
            listaEstado.push(Estados.EnAlerta);
            listaEstado.push(Estados.Asignadas);
            listaEstado.push(Estados.Comparecidas);
            listaEstado.push(Estados.Caducadas);
            listaEstado.push(Estados.Externas);
        }

        if (idPerfil == Perfiles.Asignador) {
            listaEstado.push(Estados.SinAsignar);
        }

        if (idPerfil == Perfiles.Gestor) {
            listaEstado.push(Estados.EnAlerta);
            listaEstado.push(Estados.Asignadas);
            listaEstado.push(Estados.Comparecidas);
            listaEstado.push(Estados.Caducadas);
        }

        return listaEstado;
    }
})