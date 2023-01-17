var $table = $('#table');

$(function () {

    $(document).ready(function () {
        cargarUsuarios();
    });

    function cargarUsuarios() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Administracion/GetUsuarios',
            dataType: "json",
            success: function (data) {
                $table.bootstrapTable('removeAll');
                $table.bootstrapTable('load', data);
                export_fileName = 'Usuarios';

                var $regPagina = $('#regPagina');
                $regPagina.text("Usuarios por página");
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'GetUsuarios'");
            }
        });
    }

    $(document).on("change", ".checkbox-grupo", function () {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Administracion/SetActivoUsuario',
            dataType: "json",
            data: {
                id: this.id,
                activo: this.checked
            },
            success: function (data) {
                if (data.hasOwnProperty("responseText"))
                    toastr.error("Error en la llamada al servicio 'SetActivoUsuario'");
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'SetActivoUsuario'");
            }
        });
    });
})