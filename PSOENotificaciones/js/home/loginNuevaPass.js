var $mensajeIncorrecto = $('#mensajeIncorrecto');
var $mensajePass = $('#mensajePass');
var $errorModal = $('#errorModal');
var $validModal = $('#validModal');

$(function () {

    $("#password-nueva-1").change(function () {
        var pass1 = this;
        var pass2 = $("#password-nueva-2")[0].value;

        if (pass2 != this.value) {
            $mensajePass[0].innerText = "El campo no coincide con el de arriba";
            $mensajeIncorrecto.removeClass("display-none");
            $('#button-guardar').removeClass("btn-primary");
        }
        else {
            //validar formato
            if (pass1.length >= 8 && pass1.match(/[A-z]/) && pass1.match(/[A-Z]/) && pass1.match(/\d/)) {
                $mensajeIncorrecto.addClass("display-none");
                $('#button-guardar').addClass("btn-primary");
            } else {
                $mensajePass[0].innerText = "El campo no tiene el formato requerido";
                $mensajeIncorrecto.removeClass("display-none");
                $('#button-guardar').removeClass("btn-primary");
            }
        }
    });

    $("#password-nueva-2").change(function () {
        var pass1 = $("#password-nueva-1")[0].value;

        if (pass1 != this.value) {
            $mensajePass[0].innerText = "El campo no coincide con el de arriba";
            $mensajeIncorrecto.removeClass("display-none");
            $('#button-guardar').removeClass("btn-primary");
        }
        else {
            //validar formato
            if (pass1.length >= 8 && pass1.match(/[A-z]/) && pass1.match(/[A-Z]/) && pass1.match(/\d/)) {
                $mensajeIncorrecto.addClass("display-none");
                $('#button-guardar').addClass("btn-primary");
            } else {
                $mensajePass[0].innerText = "El campo no tiene el formato requerido";
                $mensajeIncorrecto.removeClass("display-none");
                $('#button-guardar').removeClass("btn-primary");
            }
        }
    });

    $(".pass-eye-1").click(function () {
        var pass = $('#password-nueva-1');
        if (this.classList.contains('pass_hidden')) {
            pass[0].type = 'password';
            $(".pass-eye-1").removeClass("pass_hidden");
        }
        else {
            pass[0].type = 'text';
            $(".pass-eye-1").addClass("pass_hidden");
        }
    });

    $(".pass-eye-2").click(function () {
        var pass = $('#password-nueva-2');
        if (this.classList.contains('pass_hidden')) {
            pass[0].type = 'password';
            $(".pass-eye-2").removeClass("pass_hidden");
        }
        else {
            pass[0].type = 'text';
            $(".pass-eye-2").addClass("pass_hidden");
        }
    });

    $('#button-guardar').click(function (e) {
        e.preventDefault();

        var pass1 = $("#password-nueva-1")[0].value;
        var pass2 = $("#password-nueva-2")[0].value;

        if (pass1 != pass2) {
            $mensajePass[0].innerText = "El campo no coincide con el de arriba";
            $mensajeIncorrecto.removeClass("display-none");
        }
        else {
            //validar formato
            if (pass1.length >= 8 && pass1.match(/[A-z]/) && pass1.match(/[A-Z]/) && pass1.match(/\d/)) {
                $mensajeIncorrecto.addClass("display-none");

                var resValidacion = validarDni();
                if (resValidacion == 1) {
                    guardarNuevaPassword();
                }

                if (resValidacion == 0) {
                    $('#dniErrorModal').modal({ backdrop: 'static', keyboard: false });
                    $('#dniErrorModal').modal('show');
                }

                if (resValidacion == -1) {
                    $('#dniErrorBloqueadoModal').modal({ backdrop: 'static', keyboard: false });
                    $('#dniErrorBloqueadoModal').modal('show');
                }
            } else {
                $mensajePass[0].innerText = "El campo no tiene el formato requerido";
                $mensajeIncorrecto.removeClass("no-visible").addClass("visible");
                $('#button-guardar').removeClass("display-none");
            }
        }
    });

    function validarDni() {
        var resValidacion = 0;
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Home/ComprobarDni',
            data: {
                'idUsuario': idUsuario,
                'dni': $("#dni-usuario")[0].value
            },
            dataType: "json",
            success: function (data) {
                if (data.hasOwnProperty('mensajeExcepcion')) {
                    toastr.error("Error en la llamada al servicio 'ComprobarDni'");
                }
                else {
                    resValidacion = data.ResultadoValidacion;
                }
            }
        });

        return resValidacion;
    }

    function guardarNuevaPassword() {
        var pass = $("#password-nueva-1")[0].value;
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Home/UpdateUsuarioPassword',
            data: {
                'idUsuario': idUsuario,
                'password': pass
            },
            dataType: "json",
            success: function (data) {
                if (data.hasOwnProperty('mensajeExcepcion')) {
                    $errorModal.modal('show');
                }
                else {
                    $validModal.modal('show');
                }
            }
        });
    }

    $('#button-aceptar').click(function () {
        window.location.href = "../Home/Login";
    });
})