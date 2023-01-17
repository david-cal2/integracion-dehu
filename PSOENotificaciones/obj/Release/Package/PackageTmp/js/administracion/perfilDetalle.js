$(function () {

    $(document).ready(function () {
        cargarPantallasInicio();
        cargarPerfilDetalle();
    });

    function cargarPantallasInicio() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Administracion/GetPantallasInicio',
            dataType: "json",
            success: function (data) {
                var $selectPantallasInicio = $('#selectPantallasInicio');
                data.forEach(value => {
                    $selectPantallasInicio.append($("<option>")
                        .val(value.ID)
                        .html(value.Nombre)
                    );
                });
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'GetPantallasInicio'");
            }
        });
    }

    function cargarPerfilDetalle() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Administracion/GetPerfilDetalle',
            dataType: "json",
            data: {
                id: idPerfilEditar
            },
            success: function (data) {
                if (data.hasOwnProperty('responseText'))
                    toastr.error("Error en la llamada al servicio 'GetPerfilDetalle'");
                else {
                    $("#inputNombre")[0].value = data.Nombre;
                    $("#inputDescripcion")[0].innerText = data.Descripcion;

                    if (data.Notificaciones == true) {
                        $("#styled-checkbox-1").prop('checked', true);
                    }

                    if (data.Administracion == true) {
                        $("#styled-checkbox-2").prop('checked', true);
                    }

                    if (data.Configuracion == true) {
                        $("#styled-checkbox-3").prop('checked', true);
                    }

                    if (data.PantallasInicio != null)
                        $('#selectPantallasInicio')[0].value = data.PantallasInicio.ID;
                }
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'GetPerfilDetalle'");
            }
        });
    }

    $("#btnAceptar").click(function (e) {
        e.preventDefault();
        var $nombre = $('#inputNombre').val();
        var $descripcion = $('#inputDescripcion').val();
        var $notif = $('#styled-checkbox-1')[0].checked;
        var $admin = $('#styled-checkbox-2')[0].checked;
        var $config = $('#styled-checkbox-3')[0].checked;
        var $pantallaInicio = $('#selectPantallasInicio').val();

        if ($nombre == "" || $descripcion == "") {
            $('#mensaje').text("El campo nombre y descripción son obligatorios");
            $('#alertModal').modal('show');
        } else {
            $.ajax({
                async: false,
                type: 'GET',
                contentType: 'application/json; charset=utf-8',
                url: '/Administracion/ActualizarPerfil',
                dataType: "json",
                data: {
                    idPerfilEditado: idPerfilEditar,
                    nombre: $nombre,
                    desc: $descripcion,
                    noti: $notif,
                    admi: $admin,
                    confi: $config,
                    pantInicio: $pantallaInicio
                },
                success: function (data) {
                    if (!data.hasOwnProperty("responseText"))
                        $('#modalPerfilEditado').modal('show');
                    else
                        toastr.error("Error en la llamada al servicio 'ActualizarPerfil'");
                },
                error: function (data) {
                    toastr.error("Error en la llamada al servicio 'ActualizarPerfil'");
                }
            });
        }
    });
})