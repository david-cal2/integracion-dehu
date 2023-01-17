var $mensajeIncorrecto = $('#mensajeIncorrecto');
var $errorModal = $('#errorModal');
var $advertModal = $('#advertModal');
var $validModal = $('#validModal');

$(function () {

    $('#btn-enviar').click(function () {
        enviarCorreoRecuperarPassword();
    })

    $("#mail").change(function () {
        if (isEmail(this.value))
            $('#btn-enviar').removeClass("btn-disabled");
        else
            $('#btn-enviar').addClass("btn-disabled");
    });

    function enviarCorreoRecuperarPassword() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Home/EnviarCorreoRecuperarPassword',
            data: {
                'email': $('#mail').val()
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

    function isEmail(email) {
        var EmailRegex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        return EmailRegex.test(email);
    }
})