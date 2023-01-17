$(function () {

    $("#btnRegistrar").click(function (e) {
        e.preventDefault();
        var $nombre = $('#txtNombre').val();
        var $descripcion = $('#txtDescripcion').val();

        if ($nombre == "") {
            $('#mensaje').text("El campo nombre es obligatorio");
            $('#alertModal').modal('show');
        } else {
            $.ajax({
                async: false,
                type: 'GET',
                contentType: 'application/json; charset=utf-8',
                url: '/Administracion/InsertarEditarGrupo',
                dataType: "json",
                data: {
                    id: -1,
                    nombre: $nombre,
                    descripcion: $descripcion
                },
                success: function (data) {
                    if (!data.hasOwnProperty("responseText"))
                        toastr.success("Grupo creado correctamente");
                    else
                        toastr.error("Error en la llamada al servicio 'InsertarEditarGrupo'");
                },
                error: function (data) {
                    toastr.error("Error en la llamada al servicio 'InsertarEditarGrupo'");
                }
            });
        }
    });
})