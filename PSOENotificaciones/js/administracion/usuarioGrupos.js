var $tableGrupos = $('#tableGrupos');

$(function () {

    $(document).ready(function () {
        cargarGruposUsuario();
    });

    function cargarGruposUsuario() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Administracion/CargarGruposPorUsuarioPerfil',
            dataType: "json",
            success: function (data) {
                if (!data.hasOwnProperty("responseText")) {
                    $tableGrupos.bootstrapTable('removeAll');
                    $tableGrupos.bootstrapTable('load', data);
                }
                else {
                    toastr.error("Error en la llamada al servicio 'CargarGruposPorUsuarioPerfil'");
                }
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'CargarGruposPorUsuarioPerfil'");
            }
        });
    }
})