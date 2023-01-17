$(function () {

    //$('#fechaDesde').datepicker({
    //    format: 'dd/mm/yyyy'
    //});

    //$('#fechaHasta').datepicker({
    //    format: 'dd/mm/yyyy'
    //});

    $("#btnEjecutarProceso").click(function (e) {
        e.preventDefault();

        setTimeout(EjecutarTareaLocalizaManual, 1);

        $("#overlay").show();   
    });

    function EjecutarTareaLocalizaManual() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Tareas/EjecutarTareaLocalizaManual',
            dataType: "json",
            data: {
                idTipoEnvio: $("#selectTipoEnvio option:selected").val(),
                fechaDesde: $("#fechaDesde").val(),
                fechaHasta: $("#fechaHasta").val()
            },
            success: function (data) {
                if (data.Success == "True") {
                    $("#overlay").hide();
                    $('#modalProcesoDehu').modal('show');
                    setTimeout(EjecutarTareaLocalizaManualEnvioEmails, 500);
                }
                else {
                    $("#overlay").hide();
                    $('#modalErrorDehu').modal('show');
                }
            },
            error: function (data) {
                $("#overlay").hide();
                $('#modalErrorDehu').modal('show');
            }
        });
    }

    function EjecutarTareaLocalizaManualEnvioEmails() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Tareas/EjecutarTareaLocalizaManualEnvioEmails',
            dataType: "json",
            success: function (data) {
                if (data.Success == "False")
                    toastr.error("Error en la llamada al servicio 'EjecutarTareaLocalizaManualEnvioEmails'");
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'EjecutarTareaLocalizaManualEnvioEmails'");
            }
        });
    }
})