var $mensajeIncorrecto = $('#mensajeIncorrecto');

$(function () {

    function validarLogin() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Home/ValidarLogin',
            data: {
                'usuario': $('#user').val(),
                'password': $('#password').val()
            },
            dataType: "json",
            success: function (data) {
                if (data.hasOwnProperty('mensajeExcepcion')) {
                    $('#modalError').modal('show');
                }

                if (data.hasOwnProperty('mensajeLogin')) {
                    $('#modalLoginFallido').modal('show');
                }

                if (data.hasOwnProperty('mensajeLoginIncorrecto')) {
                    $mensajeIncorrecto.removeClass("no-visible");
                    $mensajeIncorrecto.addClass("visible");

                    $('#password')[0].value = "";

                    if (data.idMensajeLoginIncorrecto == TipoLoginIncorrecto.UsuarioEnEspera) {
                        $('#modalUsuarioEnEspera').modal('show');
                    }

                    if (data.idMensajeLoginIncorrecto == TipoLoginIncorrecto.IntentosSuperados) {
                        $('#modalIntentosSuperados').modal('show');
                    }

                    if (data.idMensajeLoginIncorrecto == TipoLoginIncorrecto.UsuarioBloqueado) {
                        $('#modalUsuarioBloqueado').modal('show');
                        if (data.idUsuario != 0)
                            enviarCorreoUsuarioBloqueado(data.idUsuario);
                    }
                }

                if (data.hasOwnProperty('consentimientoLegal')) {
                    if (data.consentimientoLegal == 1) {
                        if (data.hasOwnProperty('id'))
                            window.location.href = "../" + data.pantallaInicioPerfil;
                    }
                    else {
                        // Usuario pendiente de aceptar textos legales
                        window.location.href = "../Home/AvisoLegal";
                    }
                }
            },
            error: function (data) {
                $('#modalError').modal('show');
            }
        });
    }

    function enviarCorreoUsuarioBloqueado(idUsuario) {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Home/EnviarCorreoUsuarioBloqueado',
            data: { 'idUsuario': idUsuario },
            dataType: "json",
            success: function (data) {
                if (data.Success == "False")
                    toastr.error("Error en la llamada al servicio 'EnviarCorreoUsuarioBloqueado'");
            }
        });
    }

    $('#btn-login').click(function () {
        validarLogin();
    });

    $('#user').keypress(function (e) {
        var keycode = (e.keyCode ? e.keyCode : e.which);
        if (keycode == '13') {
            validarLogin();
            e.preventDefault();
            return false;
        }
    });

    $('#password').keypress(function (e) {
        var keycode = (e.keyCode ? e.keyCode : e.which);
        if (keycode == '13') {
            validarLogin();
            e.preventDefault();
            return false;
        }
    });

    $(".pass-eye").click(function () {
        var pass = $('#password');
        if (this.classList.contains('pass_hidden')) {
            pass[0].type = 'password';
            $(".password_container").removeClass("pass_hidden");
        }
        else {
            pass[0].type = 'text';
            $(".password_container").addClass("pass_hidden");
        }
    });

    const TipoLoginIncorrecto = {
        UsuarioIncorrecto: 1,
        UsuarioInactivo: 2,
        UsuarioBloqueado: 3,
        UsuarioEnEspera: 4,
        PasswordIncorrecta: 5,
        IntentosSuperados: 6
    }
})