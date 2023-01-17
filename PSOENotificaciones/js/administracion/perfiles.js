var $table = $('#table');

$(function () {
    $(document).ready(function () {
        cargarPerfiles();
    });

    function cargarPerfiles() {
        $('#conAgregar').hide();
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Administracion/CargarPerfiles',
            dataType: "json",
            success: function (data) {
                $table.bootstrapTable('removeAll');
                $table.bootstrapTable('load', data);
                var $regPagina = $('#regPagina');
                $regPagina.innerText = "Perfiles por página";
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'CargarPerfiles'");
            }
        });
    }
})