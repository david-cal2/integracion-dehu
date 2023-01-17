var $mensajeIncorrecto = $('#mensajeIncorrecto');
var $mensajePass = $('#mensajePass');
var $errorModal = $('#errorModal');
var $validModal = $('#validModal');

$(function () {

    $("#password-nueva-1").change(function () {
        var pass1 = this.value;
        var pass2 = $("#password-nueva-2")[0].value;

        if (pass1 != pass2) {
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
        var pass2 = this.value;

        if (pass1 != pass2) {
            $mensajePass[0].innerText = "El campo no coincide con el de arriba";
            $mensajeIncorrecto.removeClass("display-none");
            $('#button-guardar').removeClass("btn-primary");
        }
        else {
            //validar formato
            if (pass2.length >= 8 && pass2.match(/[A-z]/) && pass2.match(/[A-Z]/) && pass2.match(/\d/)) {
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
        var pass = $('#password-actual');
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
        var pass = $('#password-nueva-1');
        if (this.classList.contains('pass_hidden')) {
            pass[0].type = 'password';
            $(".pass-eye-2").removeClass("pass_hidden");
        }
        else {
            pass[0].type = 'text';
            $(".pass-eye-2").addClass("pass_hidden");
        }
    });

    $(".pass-eye-3").click(function () {
        var pass = $('#password-nueva-2');
        if (this.classList.contains('pass_hidden')) {
            pass[0].type = 'password';
            $(".pass-eye-3").removeClass("pass_hidden");
        }
        else {
            pass[0].type = 'text';
            $(".pass-eye-3").addClass("pass_hidden");
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
                guardarNuevaPassword();
            } else {
                $mensajePass[0].innerText = "El campo no tiene el formato requerido";
                $mensajeIncorrecto.removeClass("display-none");
                $('#button-guardar').addClass("btn-disabled");
            }
        }
    });

    function guardarNuevaPassword() {
        var passActual = $("#password-actual")[0].value;
        var passNueva = $("#password-nueva-1")[0].value;
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Home/UpdatePasswordPerfil',
            data: {
                'passwordActual': passActual,
                'passwordNueva': passNueva
            },
            dataType: "json",
            success: function (data) {
                if (!data.hasOwnProperty('mensajeExcepcion')) {
                    if (data.Success == "True") {
                        $('#passActualError').addClass("display-none");
                        toastr.success("Contraseña guardada correctamente");
                    }
                    else {
                        $('#passActualError').removeClass("display-none");
                        $('#mensaje').text("La constraseña actual no coincide");
                        $('#alertModal').modal('show');
                    }   
                }
                else
                    toastr.error("Error en la llamada al servicio 'UpdateUsuarioPassword'");
            }
        });
    }
})