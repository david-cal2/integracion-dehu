$(function () {

    $(document).ready(function () {
        cargarUsuarioDetalle();
    });

    function cargarUsuarioDetalle() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Home/GetUsuarioDetalle',
            dataType: "json",
            success: function (data) {
                if (!data.hasOwnProperty("responseText")) {
                    $("#nombre")[0].value = data.Nombre;
                    $('#apellidos')[0].value = data.Apellidos;
                    $('#nif')[0].value = data.NIF;
                    $("#usuario")[0].value = data.LoginUsuario;
                    $('#mail')[0].value = data.Email;
                    $("#telefono")[0].value = data.Telefono;
                    $('#perfil')[0].innerHTML = data.CodigoHtmlPerfil;
                }
                else
                    toastr.error("Error en la llamada al servicio 'GetUsuarioDetalle'");
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'GetUsuarioDetalle'");
            }
        });
    }

    $('#btn-guardar').click(function (e) {
        e.preventDefault();

        var $nombre = $("#nombre")[0].value;
        var $apellidos = $('#apellidos')[0].value;
        var $NIF = $('#nif')[0].value;
        var $mail = $('#mail')[0].value;
        var $telefono = $("#telefono")[0].value;
        var $login = $("#usuario")[0].value;

        var hayCamposErroneos = 0;
        var mensajeCamposErroneos = "";

        if ($nombre == "") {
            hayCamposErroneos = 1;
            mensajeCamposErroneos += "El nombre es obligatorio.<br/>"
        }

        if ($NIF == "") {
            hayCamposErroneos = 1;
            mensajeCamposErroneos += "El NIF es obligatorio.<br/>"
        }

        if ($mail == "") {
            hayCamposErroneos = 1;
            mensajeCamposErroneos += "El email es obligatorio.<br/>"
        }

        if ($login == "") {
            hayCamposErroneos = 1;
            mensajeCamposErroneos += "El login es obligatorio.<br/>"
        }

        if (hayCamposErroneos == 1) {
            $('#mensaje').text(mensajeCamposErroneos);
            $('#alertModal').modal('show');
        } else {
            $.ajax({
                async: false,
                type: 'GET',
                contentType: 'application/json; charset=utf-8',
                url: '/Administracion/EditarUsuarioConectado',
                dataType: "json",
                data: {
                    nombre: $nombre,
                    apellidos: $apellidos,
                    nif: $NIF,
                    mail: $mail,
                    tlf: $telefono,
                    login: $login
                },
                success: function (data) {
                    if (!data.hasOwnProperty("responseText")) {
                        if (data.Success == "True") {
                            toastr.success("Usuario editado correctamente");
                        }
                        else {
                            $('#mensaje').text("El email ya está registrado");
                            $('#alertModal').modal('show');
                        }
                    }
                    else
                        toastr.error("Error en la llamada al servicio 'EditarUsuarioConectado'");
                },
                error: function (data) {
                    toastr.error("Error en la llamada al servicio 'EditarUsuarioConectado'");
                }
            });
        }
    });
})