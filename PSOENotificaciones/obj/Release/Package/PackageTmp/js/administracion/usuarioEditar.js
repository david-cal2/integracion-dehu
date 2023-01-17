var $tableGrupos = $('#tableGrupos');
var $tableTodosGrupos = $('#tableTodosGrupos');
var $estaActivo = false;
var $estaBloqueado = false;

$(function () {

    $(document).ready(function () {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Administracion/CargarPerfiles',
            dataType: "json",
            success: function (data) {
                var $selectPerfiles = $('#selectPerfiles');
                data.forEach(value => {
                    $selectPerfiles.append($("<option>")
                        .val(value.ID)
                        .html(value.Nombre)
                    );
                });

                $selectPerfiles.selectpicker('refresh');
                $selectPerfiles.selectpicker('render');
            }
        });

        cargarUsuarioDetalle();
        cargarGruposUsuario();

        $("#conTodosGrupos").hide();
    });

    function cargarUsuarioDetalle() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Administracion/GetUsuarioDetalle',
            dataType: "json",
            data: {
                idUsuario: idUsuarioEditado
            },
            success: function (data) {
                if (!data.hasOwnProperty("responseText")) {
                    $("#tituloUsuario")[0].innerText = data.Nombre + " " + data.Apellidos;
                    $("#txtNombre")[0].value = data.Nombre;
                    $('#txtApellidos')[0].value = data.Apellidos;
                    $('#txtNIF')[0].value = data.NIF;
                    $("#txtEmail")[0].value = data.Email;
                    $('#txtTelefono')[0].value = data.Telefono;
                    $("#txtLogin")[0].value = data.LoginUsuario;
                    $('#selectPerfiles').selectpicker('val', data.IdPerfil);
                    $('#toggle-activo')[0].checked = data.Activo;
                    $('#toggle-bloqueado')[0].checked = data.Bloqueado;

                    $estaActivo = data.Activo;
                    $estaBloqueado = data.Bloqueado;
                }
                else
                    toastr.error("Error en la llamada al servicio 'GetUsuarioDetalle'");
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'GetUsuarioDetalle'");
            }
        });
    }

    function cargarGruposUsuario() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Administracion/CargarGruposPorUsuario',
            dataType: "json",
            data: {
                id: idUsuarioEditado
            },
            success: function (data) {
                if (!data.hasOwnProperty("responseText")) {
                    $tableGrupos.bootstrapTable('removeAll');
                    $tableGrupos.bootstrapTable('load', data);
                }
                else
                    toastr.error("Error en la llamada al servicio 'CargarGruposPorUsuario'");
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'CargarGruposPorUsuario'");
            }
        });
    }

    function cargarTodosGrupos() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Administracion/GetGrupos',
            dataType: "json",
            success: function (data) {
                $tableTodosGrupos.bootstrapTable('removeAll');
                $tableTodosGrupos.bootstrapTable('load', data);

                var $regPagina = $('#regPagina');
                $regPagina.text("Grupos por página");
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'GetGrupos'");
            }
        });
    }

    function enviarCorreoUsuarioBloqueado(idUsuario) {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Home/EnviarCorreoUsuarioBloqueado',
            data: {
                'idUsuario': idUsuario
            },
            dataType: "json",
            success: function (data) {
                if (data.Success == "False")
                    toastr.error("Error en la llamada al servicio 'EnviarCorreoUsuarioBloqueado'");
            }
        });
    }

    function enviarCorreoDesbloqueoUsuario(idUsuario) {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Home/EnviarCorreoDesbloqueoUsuario',
            data: {
                'idUsuario': idUsuario
            },
            dataType: "json",
            success: function (data) {
                if (data.Success == "False")
                    toastr.error("Error en la llamada al servicio 'EnviarCorreoDesbloqueoUsuario'");
            }
        });
    }

    $("#btnAceptar").click(function (e) {
        e.preventDefault();
        $("#overlay").show();
        setTimeout(InsertarActualizar, 100);
    });

    function InsertarActualizar() {
        var $nombre = $('#txtNombre').val();
        var $apellidos = $('#txtApellidos').val();
        var $NIF = $('#txtNIF').val();
        var $mail = $("#txtEmail").val();
        var $telefono = $('#txtTelefono').val();
        var $perfil = $('#selectPerfiles').val();
        var $activo = $('#toggle-activo')[0].checked;
        var $bloqueado = $('#toggle-bloqueado')[0].checked;

        var hayCamposErroneos = 0;
        var mensajeCamposErroneos = "";

        if ($nombre == "" || $nombre.length < 3) {
            hayCamposErroneos = 1;
            $("#txtNombre").addClass("input-incorrecto");
            mensajeCamposErroneos += "El nombre es obligatorio (mínimo 3 caracteres).<br/>"
        }
        else {
            $("#txtNombre").removeClass("input-incorrecto");
        }

        if ($NIF == "" || !validarDNI($NIF)) {
            hayCamposErroneos = 1;
            $("#txtNIF").addClass("input-incorrecto");
            mensajeCamposErroneos += "El DNI tiene un formato incorrecto.<br/>"
        }
        else {
            $("#txtNIF").removeClass("input-incorrecto");
        }

        if ($mail == "" || !isEmail($mail)) {
            hayCamposErroneos = 1;
            $("#txtEmail").addClass("input-incorrecto");
            mensajeCamposErroneos += "El email tiene un formato incorrecto.<br/>"
        }
        else {
            $("#txtEmail").removeClass("input-incorrecto");
        }

        if (hayCamposErroneos == 1) {
            $("#overlay").hide();
            $("#litCamposIncorrectos")[0].innerHTML = mensajeCamposErroneos;
            $('#modalCamposIncorrectos').modal({ backdrop: 'static', keyboard: false });
            $("#modalCamposIncorrectos").modal('show');
        } else {
            $.ajax({
                async: false,
                type: 'GET',
                contentType: 'application/json; charset=utf-8',
                url: '/Administracion/InsertarEditarUsuario',
                dataType: "json",
                data: {
                    id: idUsuarioEditado,
                    nombre: $nombre,
                    ape: $apellidos,
                    nif: $NIF,
                    mail: $mail,
                    activo: $activo,
                    tlf: $telefono,
                    login: "",
                    perId: $perfil,
                    bloqueado: $bloqueado,
                    pass: "",
                    estaActivo: $estaActivo,
                    estaBloqueado: $estaBloqueado
                },
                success: function (data) {
                    if (!data.hasOwnProperty("mensajeExcepcion")) {
                        if (data.Success == "True") {
                            $("#overlay").hide();
                            toastr.success("Usuario actualizado correctamente");

                            if ($estaBloqueado == false && $bloqueado == true && idUsuarioEditado != -1)
                                enviarCorreoUsuarioBloqueado(idUsuarioEditado);

                            if ($estaBloqueado == true && $bloqueado == false && idUsuarioEditado != -1)
                                enviarCorreoDesbloqueoUsuario(idUsuarioEditado);
                        }
                        else {
                            $("#overlay").hide();
                            $('#mensaje').text(data.responseText);
                            $('#alertModal').modal({ backdrop: 'static', keyboard: false });
                            $('#alertModal').modal('show');
                        }
                    }
                    else {
                        $("#overlay").hide();
                        toastr.error("Error en la llamada al servicio 'InsertarEditarUsuario'");
                    }
                },
                error: function (data) {
                    $("#overlay").hide();
                    toastr.error("Error en la llamada al servicio 'InsertarEditarUsuario'");
                }
            });
        }
    }

    $("#btnAsignar").click(function (e) {
        e.preventDefault();
        $("#conTodosGrupos").show();
        cargarTodosGrupos();
    });

    // Agregar Asignación de usuario a grupo
    $('#tableTodosGrupos tbody').on('click', '#spanAsignar', function () {
        // Recuperamos la fila
        var $row = $(this).closest("tr");
        // Recuperamos los valores de cada columna
        var $colIdGrupo = $row.find("td:nth-child(1)").text();

        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Administracion/AsignarUsuarioGrupo',
            dataType: "json",
            data: {
                idUsu: idUsuarioEditado,
                idGrupo: $colIdGrupo
            },
            success: function (data) {
                if (data.responseText == "Ya existe") {
                    $('#mensaje').text("El usuario ya pertenece al grupo");
                    $('#alertModal').modal({ backdrop: 'static', keyboard: false });
                    $('#alertModal').modal('show');
                } else {
                    cargarGruposUsuario();
                }
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'AsignarUsuarioGrupo'");
            }
        });

    });

    // Eliminar Asignación de usuario a grupo
    $('#tableGrupos tbody').on('click', '#spanEliminar', function () {
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
                cargarGruposUsuario();
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'EliminarUsuarioGrupo'");
            }
        });
    });

    function isEmail(email) {
        var EmailRegex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        return EmailRegex.test(email);
    }
})