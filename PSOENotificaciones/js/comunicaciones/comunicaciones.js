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

        cargarComunicaciones();
        habilitarElementos();

        $('#btn-buscar').click(function (e) {
            e.preventDefault();

            if ($selectEstados.val() == '' && $inputIdentificador.val() == '' && $selectAdministracionPublica.val() == '' &&
                $selectOrganismosRaiz.val() == '' && $selectOrganismosEmisores.val() == '' && $selectAsuntos.val() == '' &&
                $selectGrupos.val() == '' && $fechaDesde.val() == '' && $fechaHasta.val() == '') {
                setTimeout(cargarComunicaciones, 1);
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

                setTimeout(cargarComunicaciones, 1, data);
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
            if ($element.EstadosEnvio.ID != Estados.Leidas)
                window.location.href = "../Comunicaciones/ComunicacionDetalle?identificador=" + $element.Identificador + "&c=0";
            else
                window.location.href = "../Comunicaciones/ComunicacionLeida?identificador=" + $element.Identificador;
        });

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
                        cargarComunicaciones();
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

        function cargarComunicaciones(data) {
            var urlFuncion = '/Comunicaciones/GetComunicacionesBuscadorComplejo';

            var cargarCantidades = false;
            if (data == null) {
                cargarCantidades = true;
                urlFuncion = '/Comunicaciones/GetComunicaciones';
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
                            var $cantAsignadas = $('#cantAsignadas');
                            var $cantLeidas = $('#cantLeidas');

                            $cantSinAsignar[0].innerHTML = data.CantidadesTotales[0].Value;
                            $cantAsignadas[0].innerHTML = data.CantidadesTotales[2].Value;
                            $cantLeidas[0].innerHTML = data.CantidadesTotales[5].Value;

                            $('#tituloPantallaInicio')[0].innerHTML = "Comunicaciones recibidas en los últimos " + data.NumeroUltimosDias + " días";
                        }

                        var $divContenedor = $('#div-contenedor');
                        var $divContenedorVacio = $('#div-contenedor-vacio');

                        if (data.ListaEnvios != null && data.ListaEnvios.length > 0) {
                            $divContenedor.removeClass('display-none')
                            $divContenedorVacio.addClass('display-none');

                            var $table = $('#table');
                            $table.bootstrapTable('removeAll');
                            $table.bootstrapTable('load', data.ListaEnvios);
                            export_fileName = 'Comunicaciones';
                            var $regPagina = $('#regPagina');
                            $regPagina.text("Comunicaciones por página");
                        }
                        else {
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
        var $btnComunicacionesEstado1 = $('#btn-comunicaciones-estado-1');
        var $btnComunicacionesEstado3 = $('#btn-comunicaciones-estado-3');
        var $btnComunicacionesEstado6 = $('#btn-comunicaciones-estado-6');

        if (idPerfil == Perfiles.Asignador) {
            $btnComunicacionesEstado3.addClass("no-visible");
            $btnComunicacionesEstado6.addClass("no-visible");
        }

        if (idPerfil == Perfiles.Gestor) {
            $btnComunicacionesEstado1.addClass("display-none");
        }
    }

    function getEstadosPorPerfil() {
        var listaEstado = [];

        if (idPerfil == Perfiles.Administrador || idPerfil == Perfiles.UsuarioConsulta) {
            listaEstado.push(Estados.SinAsignar);
            listaEstado.push(Estados.Asignadas);
            listaEstado.push(Estados.Leidas);
        }

        if (idPerfil == Perfiles.Asignador) {
            listaEstado.push(Estados.SinAsignar);
        }

        if (idPerfil == Perfiles.Gestor) {
            listaEstado.push(Estados.Asignadas);
            listaEstado.push(Estados.Leidas);
        }

        return listaEstado;
    }
})