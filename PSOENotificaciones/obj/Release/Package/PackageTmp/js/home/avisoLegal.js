$(function () {

    $(document).ready(function () {
        //cargarTextosLegales();
    });

    function cargarTextosLegales() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Configuracion/GetParametros',
            dataType: "json",
            success: function (data) {
                var p3 = data[2].Valor;
                if (data.Descripcion = "ConsentimientoLegal")
                    $('#divTextosLegales').html(p3);
                else
                    $('#divTextosLegales').html("No se pudo obtener el texto para aceptar las condiciones legales, contacte con el administrador.");
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'GetParametros'");
            }
        });
    }

    $('#btnAceptarConsentimiento').click(function (e) {
        e.preventDefault();

        var $chkAceptar = $('#chkAcepto');

        if ($chkAceptar[0].checked) {
            $.ajax({
                async: false,
                type: 'GET',
                contentType: 'application/json; charset=utf-8',
                url: '/Administracion/AceptarAvisoLegal',
                dataType: "json",
                success: function (data) {
                    if (data.Success == "True")
                        window.location.href = "../" + data.pantallaInicioPerfil;
                    else
                        toastr.error("Error en la llamada al servicio 'AceptarAvisoLegal'");
                },
                error: function (data) {
                    toastr.error("Error en la llamada al servicio 'AceptarAvisoLegal'");
                }
            });
        } else {
            $chkAceptar.focus();
        }
    })
})