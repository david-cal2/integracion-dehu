$(function () {

    $(document).ready(function () {
        cargarParametros();
        cargarAlertas();
        cargarHorario();
    });

    function cargarParametros() {
        var $p1 = $('#txtParam1');
        var $p2 = $('#txtParam2');

        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Configuracion/GetParametros',
            dataType: "json",
            success: function (data) {
                $p1.val(data[0].Valor);
                $p2.val(data[1].Valor);
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'GetParametros'");
            }
        });
    }

    function cargarAlertas() {
        var $p1 = $('#txtParam3');
        var $p2 = $('#txtParam4');

        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Configuracion/GetAlertas',
            dataType: "json",
            success: function (data) {
                $p1.val(data[0].Valor);
                $p2.val(data[1].Valor);
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'GetAlertas'");
            }
        });
    }

    function cargarHorario() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Configuracion/GetHorario',
            dataType: "json",
            success: function (data) {
                var $Lunes = $('#chkLunes');
                var $HoraLunes = $('#tHoraLunes');
                var $Martes = $('#chkMartes');
                var $HoraMartes = $('#tHoraMartes');
                var $Miercoles = $('#chkMiercoles');
                var $HoraMiercoles = $('#tHoraMiercoles');
                var $Jueves = $('#chkJueves');
                var $HoraJueves = $('#tHoraJueves');
                var $Viernes = $('#chkViernes');
                var $HoraViernes = $('#tHoraViernes');
                var $Sabado = $('#chkSabado');
                var $HoraSabado = $('#tHoraSabado');
                var $Domingo = $('#chkDomingo');
                var $HoraDomingo = $('#tHoraDomingo');

                if (data[0].Activo == 1) {
                    $Lunes[0].checked = true;
                    var hora = data[0].Horario.Hours;
                    var minutos = data[0].Horario.Minutes;

                    if (hora < 10)
                        hora = hora.toString().padStart(2, '0');
                    if (minutos < 10)
                        minutos = minutos.toString().padStart(2, '0');

                    var tiempo = hora + ':' + minutos;
                    $HoraLunes.val(tiempo);
                }
                if (data[1].Activo == 1) {
                    $Martes[0].checked = true;
                    var hora = data[1].Horario.Hours;
                    var minutos = data[1].Horario.Minutes;

                    if (hora < 10)
                        hora = hora.toString().padStart(2, '0');
                    if (minutos < 10)
                        minutos = minutos.toString().padStart(2, '0');

                    var tiempo = hora + ':' + minutos;
                    $HoraMartes.val(tiempo);
                }
                if (data[2].Activo == 1) {
                    $Miercoles[0].checked = true;
                    var hora = data[2].Horario.Hours;
                    var minutos = data[2].Horario.Minutes;

                    if (hora < 10)
                        hora = hora.toString().padStart(2, '0');
                    if (minutos < 10)
                        minutos = minutos.toString().padStart(2, '0');

                    var tiempo = hora + ':' + minutos;
                    $HoraMiercoles.val(tiempo);
                }
                if (data[3].Activo == 1) {
                    $Jueves[0].checked = true;
                    var hora = data[3].Horario.Hours;
                    var minutos = data[3].Horario.Minutes;

                    if (hora < 10)
                        hora = hora.toString().padStart(2, '0');
                    if (minutos < 10)
                        minutos = minutos.toString().padStart(2, '0');

                    var tiempo = hora + ':' + minutos;
                    $HoraJueves.val(tiempo);
                }
                if (data[4].Activo == 1) {
                    $Viernes[0].checked = true;
                    var hora = data[4].Horario.Hours;
                    var minutos = data[4].Horario.Minutes;

                    if (hora < 10)
                        hora = hora.toString().padStart(2, '0');
                    if (minutos < 10)
                        minutos = minutos.toString().padStart(2, '0');

                    var tiempo = hora + ':' + minutos;
                    $HoraViernes.val(tiempo);
                }
                if (data[5].Activo == 1) {
                    $Sabado[0].checked = true;
                    var hora = data[5].Horario.Hours;
                    var minutos = data[5].Horario.Minutes;

                    if (hora < 10)
                        hora = hora.toString().padStart(2, '0');
                    if (minutos < 10)
                        minutos = minutos.toString().padStart(2, '0');

                    var tiempo = hora + ':' + minutos;
                    $HoraSabado.val(tiempo);
                }
                if (data[6].Activo == 1) {
                    $Domingo[0].checked = true;
                    var hora = data[6].Horario.Hours;
                    var minutos = data[6].Horario.Minutes;

                    if (hora < 10)
                        hora = hora.toString().padStart(2, '0');
                    if (minutos < 10)
                        minutos = minutos.toString().padStart(2, '0');

                    var tiempo = hora + ':' + minutos;
                    $HoraDomingo.val(tiempo);
                }
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'GetHorario'");
            }
        });
    }

    $("#btnAceptar").click(function (e) {
        e.preventDefault();
        var $p1 = $('#txtParam1').val();
        var $p2 = $('#txtParam2').val();

        guardarParametros(1, $p1);
        guardarParametros(2, $p2);

        var $p3 = $('#txtParam3').val();
        var $p4 = $('#txtParam4').val();

        guardarAlertas(1, $p3);
        guardarAlertas(2, $p4);

        var $Lunes = $('#chkLunes');
        var $HoraLunes = $('#tHoraLunes').val();
        var $Martes = $('#chkMartes');
        var $HoraMartes = $('#tHoraMartes').val();
        var $Miercoles = $('#chkMiercoles');
        var $HoraMiercoles = $('#tHoraMiercoles').val();
        var $Jueves = $('#chkJueves');
        var $HoraJueves = $('#tHoraJueves').val();
        var $Viernes = $('#chkViernes');
        var $HoraViernes = $('#tHoraViernes').val();
        var $Sabado = $('#chkSabado');
        var $HoraSabado = $('#tHoraSabado').val();
        var $Domingo = $('#chkDomingo');
        var $HoraDomingo = $('#tHoraDomingo').val();

        if ($HoraLunes != "")
            guardarHorario("Lunes", $HoraLunes, $Lunes[0].checked);

        if ($HoraMartes != "")
            guardarHorario("Martes", $HoraMartes, $Martes[0].checked);

        if ($HoraMiercoles != "")
            guardarHorario("Miércoles", $HoraMiercoles, $Miercoles[0].checked);

        if ($HoraJueves != "")
            guardarHorario("Jueves", $HoraJueves, $Jueves[0].checked);

        if ($HoraViernes != "")
            guardarHorario("Viernes", $HoraViernes, $Viernes[0].checked);

        if ($HoraSabado != "")
            guardarHorario("Sábado", $HoraSabado, $Sabado[0].checked);

        if ($HoraDomingo != "")
            guardarHorario("Domingo", $HoraDomingo, $Domingo[0].checked);

        $('#modalParametros').modal('show');
    });

    function guardarParametros(id, val) {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Configuracion/SetParametros',
            dataType: "json",
            data: {
                id: id,
                valor: val
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'SetParametros'");
            }
        });
    }

    function guardarAlertas(id, val) {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Configuracion/SetAlertas',
            dataType: "json",
            data: {
                id: id,
                valor: val
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'SetAlertas'");
            }
        });
    }

    function guardarHorario(dia, hora, act) {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Configuracion/SetHorario',
            dataType: "json",
            data: {
                diaSemana: dia,
                horario: hora,
                activo: act
            },
            success: function (data) {
                if (data.hasOwnProperty("responseText"))
                    toastr.error("Error en la llamada al servicio 'SetHorario'");
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'SetHorario'");
            }
        });
    }

    $(".time-control").blur(function () {
        var partes = this.value.split(':');

        var minutosCorrectos = partes[1];
        if (partes[1] != "00" && partes[1] != "15" && partes[1] != "30" && partes[1] != "45")
            minutosCorrectos = "00";

        this.value = partes[0] + ":" + minutosCorrectos;
    });
})