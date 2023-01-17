$(function () {

    $(document).ready(function () {
        cargarAvisos(TipoEmails.BloqueoUsuario);
        cargarAvisos(TipoEmails.ProcesoCarga);
        cargarAvisos(TipoEmails.ProcesoCargaError);
        cargarAvisos(TipoEmails.AsignacionManual);
        cargarAvisos(TipoEmails.EnAlerta);
        cargarAvisos(TipoEmails.Caducadas);
        cargarAvisos(TipoEmails.Externas);
    });

    function cargarAvisos(idTipoEmail) {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Configuracion/CargarAvisos',
            dataType: "json",
            data: {
                idTipoEmail: idTipoEmail
            },
            success: function (data) {
                var $tableEmail = $('#tableEmail' + idTipoEmail);
                $tableEmail.bootstrapTable('removeAll');
                $tableEmail.bootstrapTable('load', data);
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'CargarAvisos'");
            }
        });
    }

    function guardarEmail($identificador, $nombre, $email, idTipoMail) {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Configuracion/InsertarMailAviso',
            dataType: "json",
            data: {
                id: $identificador,
                nombre: $nombre,
                mail: $email,
                activo: true,
                idTipoEmail: idTipoMail
            },
            success: function (data) {
                if (data.hasOwnProperty("responseText"))
                    toastr.error("Error en la llamada al servicio 'InsertarMailAviso'");
                else {
                    if (data.Success == "False") {
                        $('#mensaje').text("Ya esta registrado el email");
                        $('#alertModal').modal('show');
                    }
                }
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'InsertarMailAviso'");
            }
        });
    }

    $(".btnAceptar").click(function (e) {
        e.preventDefault();

        var idTipoMail = this.attributes[1].value;
        var $identificador = $('#identificador' + idTipoMail).val();
        var $nombre = $('#txtNombre' + idTipoMail).val();
        var $email = $('#txtEmail' + idTipoMail).val();
        var okInsert = true;

        if ($nombre == "" || $email == "")
        {
            okInsert = false;
            $('#mensaje').text("El campo nombre y el campo email no pueden estar vacíos");
            $('#alertModal').modal('show');
        }

        if (!document.getElementById("txtEmail" + idTipoMail).validity.valid)
        {
            okInsert = false;
            $('#mensaje').text("El mail no tiene el formato correcto");
            $('#alertModal').modal('show');
        }

        if (okInsert)
        {
            guardarEmail($identificador, $nombre, $email, idTipoMail);
            cargarAvisos(idTipoMail);
            $("#txtNombre" + idTipoMail).val("");
            $("#txtEmail" + idTipoMail).val("");
        }
    });

    $('.table tbody').on('click', '.editar_email', function () {
        var idEmail = this.attributes[1].value;

        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Configuracion/CargarEmail',
            dataType: "json",
            data: {
                idEmail: idEmail
            },
            success: function (data) {
                if (data.hasOwnProperty("responseText"))
                    toastr.error("Error en la llamada al servicio 'CargarEmail'");
                else {
                    var idTipoMail = data.TipoEmail;
                    $("#identificador" + idTipoMail).val(data.ID);
                    $("#txtNombre" + idTipoMail).val(data.Nombre);
                    $("#txtEmail" + idTipoMail).val(data.Email);
                }
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'CargarEmail'");
            }
        });
    });

    var $idDelete = -1;

    $('.table tbody').on('click', '#spanDelete', function () {
        var $row = $(this).closest("tr");
        $idDelete = $row.find("td:nth-child(1)").text();
        $('#validModal').modal('show');
    });

    $("#btnAceptarPopUp").click(function (e) {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Configuracion/BorrarMailAviso',
            dataType: "json",
            data: {
                id: $idDelete
            },
            success: function (data) {
                if (data.hasOwnProperty("responseText"))
                    toastr.error("Error en la llamada al servicio 'BorrarMailAviso'");
                else {
                    cargarAvisos(TipoEmails.BloqueoUsuario);
                    cargarAvisos(TipoEmails.ProcesoCarga);
                    cargarAvisos(TipoEmails.ProcesoCargaError);
                    cargarAvisos(TipoEmails.AsignacionManual);
                    cargarAvisos(TipoEmails.EnAlerta);
                    cargarAvisos(TipoEmails.Caducadas);
                    cargarAvisos(TipoEmails.Externas);
                }
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'BorrarMailAviso'");
            }
        });
    });
})