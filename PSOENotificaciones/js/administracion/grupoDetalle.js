var $table = $('#table');
var $tablaUsu = $('#tablaUsuarios');
var $tablaOrg = $('#tablaOrganismos');
var $tableTodosUsu = $('#tableTodosUsuarios');
var $tablaTodosOrg = $('#tableTodosOrganismos');

$(function () {

    $(document).ready(function () {
        cargarGrupoDetalle();
        cargarUsuariosGrupo();
        cargarOrganismos();
    });

    function cargarGrupoDetalle() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Administracion/GetDetalleGrupo',
            dataType: "json",
            data: {
                id: idGrupoEditado
            },
            success: function (data) {
                if (!data.hasOwnProperty("responseText")) {
                    $("#tituloGrupo")[0].innerText = data.Descripcion;
                    $("#txtNombre")[0].value = data.Nombre;
                    $("#txtDescripcion")[0].value = data.Descripcion;
                }
                else {
                    toastr.error("Error en la llamada al servicio 'GetDetalleGrupo'");
                } 
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'GetDetalleGrupo'");
            }
        });
    }

    function cargarUsuariosGrupo() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Administracion/CargarUsuarioPorGrupos',
            dataType: "json",
            data: {
                id: idGrupoEditado
            },
            success: function (data) {
                $tablaUsu.bootstrapTable('removeAll');
                $tablaUsu.bootstrapTable('load', data);
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'CargarUsuarioPorGrupos'");
            }
        });
    }

    function cargarTodosUsuarios() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Administracion/GetUsuarios',
            dataType: "json",
            success: function (data) {
                $tableTodosUsu.bootstrapTable('removeAll');
                $tableTodosUsu.bootstrapTable('load', data);
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'GetUsuarios'");
            }
        });
    }

    function cargarOrganismos() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Administracion/CargarOrganismoPorId',
            dataType: "json",
            data: {
                id: idGrupoEditado
            },
            success: function (data) {
                $tablaOrg.bootstrapTable('removeAll');
                $tablaOrg.bootstrapTable('load', data);
                $('#regPagina').text("Organismos por página");
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'CargarOrganismoPorId'");
            }
        });
    }

    function cargarTodosOrganismos() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Administracion/GetOrganismosEmisoresTabla',
            dataType: "json",
            success: function (data) {
                $tablaTodosOrg.bootstrapTable('removeAll');
                $tablaTodosOrg.bootstrapTable('load', data);
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'GetOrganismosEmisoresTabla'");
            }
        });
    }

    $("#btnAceptar").click(function (e) {
        e.preventDefault();
        var $nombre = $('#txtNombre').val();
        var $descripcion = $('#txtDescripcion').val();

        if ($nombre == "") {
            $('#mensaje').text("El campo nombre es obligatorio");
            $('#alertModal').modal('show');
        } else {
            $.ajax({
                async: false,
                type: 'GET',
                contentType: 'application/json; charset=utf-8',
                url: '/Administracion/InsertarEditarGrupo',
                dataType: "json",
                data: {
                    id: idGrupoEditado,
                    nombre: $nombre,
                    descripcion: $descripcion
                },
                success: function (data) {
                    if (!data.hasOwnProperty("responseText"))
                        toastr.success("Detalle grabado correctamente");
                    else
                        toastr.error("Error en la llamada al servicio 'InsertarEditarGrupo'");
                },
                error: function (data) {
                    toastr.error("Error en la llamada al servicio 'InsertarEditarGrupo'");
                }
            });
        }
    });

    $("#btnAgregarUsuario").click(function (e) {
        e.preventDefault();
        $("#conTodosUsuarios").show();
        cargarTodosUsuarios();
    });

    $("#btnAgregarOrganismo").click(function (e) {
       // e.preventDefault();
        $("#conTodosOrganismos").show();
        //cargarTodosOrganismos();
        window.scrollTo(0, document.body.scrollHeight);
    });

    // Agregar Asignación de usuario a grupo
    $('#tableTodosUsuarios tbody').on('click', '#spanAsignar', function () {
        // Recuperamos la fila
        var $row = $(this).closest("tr");
        // Recuperamos los valores de cada columna
        var $colIdUsuario = $row.find("td:nth-child(1)").text();

        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Administracion/AsignarUsuarioGrupo',
            dataType: "json",
            data: {
                idUsu: $colIdUsuario,
                idGrupo: idGrupoEditado
            },
            success: function (data) {
                if (data.responseText == "Ya existe") {
                    $('#mensaje').text("El usuario ya pertenece al grupo");
                    $('#alertModal').modal('show');
                } else {
                    cargarUsuariosGrupo(idGrupoEditado);
                }
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'AsignarUsuarioGrupo'");
            }
        });

    });

    // Eliminar Asignación de usuario a grupo
    $('#tablaUsuarios tbody').on('click', '#spanEliminar', function () {
        // Recuperamos la fila
        var $row = $(this).closest("tr");
        // Recuperamos los valores de cada columna
        var $colIdUsuGrupo = $row.find("td:nth-child(1)").text();

        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Administracion/EliminarUsuarioGrupo',
            dataType: "json",
            data: {
                idUsuGrupo: $colIdUsuGrupo
            },
            success: function (data) {
                cargarUsuariosGrupo(idGrupoEditado);
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'EliminarUsuarioGrupo'");
            }
        });

    });

    // Agregar Asignación de organismo a grupo
    $('#tableTodosOrganismos tbody').on('click', '#spanAsignarOrg', function () {
        // Recuperamos la fila
        var $row = $(this).closest("tr");
        // Recuperamos los valores de cada columna
        var $colIdOrg = $row.find("td:nth-child(1)").text();

        $("#overlay").show();
        setTimeout(AsignarOrganismoAGrupo, 100, $colIdOrg);
    });

    // Eliminar Asignación de Organismo a grupo
    $('#tablaOrganismos tbody').on('click', '#spanEliminarOrg', function () {
        // Recuperamos la fila
        var $row = $(this).closest("tr");
        // Recuperamos los valores de cada columna
        var $colIdOrgGrupo = $row.find("td:nth-child(1)").text();

        $("#overlay").show();
        setTimeout(EliminarOrganismoAGrupo, 100, $colIdOrgGrupo);
    });

    function AsignarOrganismoAGrupo(idOrgV) {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Administracion/AsignarOrgGrupo',
            dataType: "json",
            data: {
                idOrg: idOrgV,
                idGrupo: idGrupoEditado
            },
            success: function (data) {
                if (data.responseText == "Ya existe") {
                    $("#overlay").hide();
                    $('#mensaje').text("El Organismo ya pertenece al grupo");
                    $('#alertModal').modal('show');
                } else {
                    cargarOrganismos(idGrupoEditado);
                    $("#overlay").hide();
                    toastr.success("Organismo agregado correctamente");
                }
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'AsignarOrgGrupo'");
            }
        });
    }

    function EliminarOrganismoAGrupo(idOrgGrupoV) {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Administracion/EliminarOrganismoGrupo',
            dataType: "json",
            data: {
                idOrgGrupo: idOrgGrupoV
            },
            success: function (data) {
                cargarOrganismos(idGrupoEditado);
                $("#overlay").hide();
                toastr.success("Organismo eliminado correctamente");
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'EliminarOrganismoGrupo'");
            }
        });
    }
})