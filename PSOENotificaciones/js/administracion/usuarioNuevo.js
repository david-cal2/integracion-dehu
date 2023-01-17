var $tableGrupos = $('#tableGrupos');
var $tableTodosGrupos = $('#tableTodosGrupos');

$(function () {

    $(document).ready(function () {
        cargarPerfiles();
        cargarTodosGrupos();
    });

    function cargarPerfiles() {
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
        var $login = $('#txtLogin').val();
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

        if ($login == "" || !isLogin($login)) {
            hayCamposErroneos = 1;
            $("#txtLogin").addClass("input-incorrecto");
            mensajeCamposErroneos += "El login debe de estar entre los 3 y 10 caracteres.<br/>"
        }
        else {
            $("#txtLogin").removeClass("input-incorrecto");
        }

        if ($perfil == "") {
            hayCamposErroneos = 1;
            $("#selectPerfiles").addClass("input-incorrecto");
            mensajeCamposErroneos += "El perfil es obligatorio.<br/>"
        }
        else {
            $("#selectPerfiles").removeClass("input-incorrecto");
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
                    id: -1,
                    nombre: $nombre,
                    ape: $apellidos,
                    nif: $NIF,
                    mail: $mail,
                    activo: $activo,
                    tlf: $telefono,
                    login: $login,
                    perId: $perfil,
                    bloqueado: $bloqueado,
                    pass: '',
                    estaActivo: false,
                    estaBloqueado: false
                },
                success: function (data) {
                    if (!data.hasOwnProperty("mensajeExcepcion")) {
                        if (data.Success == "True") {
                            toastr.success("Usuario creado correctamente");
                            idUsuarioEditado = data.idNuevoUsuario;
                            enviarCorreoNuevoUsuario(data.idNuevoUsuario);

                            $("#tituloUsuario")[0].innerText = $nombre + " " + $apellidos;

                            $("#btnAceptar").addClass("display-none");

                            $("#tab-detalle").removeClass("active");
                            $("#tab-grupos").addClass("active");

                            $("#tabDetalle").removeClass("active");
                            $("#tabDetalle").removeClass("show");

                            $("#tabGrupos").addClass("active");
                            $("#tabGrupos").addClass("show");

                            $("#txtNombre").prop("disabled", true);
                            $("#txtApellidos").prop("disabled", true);
                            $("#txtNIF").prop("disabled", true);
                            $("#txtEmail").prop("disabled", true);
                            $("#txtTelefono").prop("disabled", true);
                            $("#txtLogin").prop("disabled", true);
                            //$("#txtPass").prop("disabled", true);

                            $("#overlay").hide();
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

    function enviarCorreoNuevoUsuario(idUsu) {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Home/EnviarCorreoUsuarioNuevo',
            data: {
                idNuevoUsuario: idUsu
            },
            dataType: "json",
            success: function (data) {
                if (data.hasOwnProperty('mensajeExcepcion')) {
                    $errorModal.modal('show');
                }
                else {
                    if (data.Success == "True")
                        $validModal.modal('show');
                    else
                        $advertModal.modal('show');
                }
            }
        });
    }

    // Agregar Asignación de usuario a grupo
    $('#tableTodosGrupos tbody').on('click', '#spanAsignar', function () {
        if (idUsuarioEditado == 0) {
            $('#modalRegistroError').modal({ backdrop: 'static', keyboard: false });
            $('#modalRegistroError').modal('show');
        }
        else {
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
        }
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

    function isLogin(login) {
        var LoginRegex = /^.{3,10}$/;
        return LoginRegex.test(login);
    }
})