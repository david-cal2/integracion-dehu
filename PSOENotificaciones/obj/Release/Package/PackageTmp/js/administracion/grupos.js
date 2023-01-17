var $table = $('#table');

$(function () {

    $(document).ready(function () {
        cargarGrupos();
    });

    function cargarGrupos() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Administracion/GetGrupos',
            dataType: "json",
            success: function (data) {
                $table.bootstrapTable('removeAll');
                $table.bootstrapTable('load', data);
                export_fileName = 'Grupos';

                var $regPagina = $('#regPagina');
                $regPagina.text("Grupos por página");
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'GetGrupos'");
            }
        });
    }

    $(document).on("change", ".checkbox-grupo", function () {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Administracion/SetActivoGrupo',
            dataType: "json",
            data: {
                id: this.id,
                activo: this.checked
            },
            success: function (data) {
                if (data.hasOwnProperty("responseText"))
                    toastr.error("Error en la llamada al servicio 'SetActivoGrupo'");
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'SetActivoGrupo'");
            }
        });
    });
})