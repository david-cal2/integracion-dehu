var $table = $('#table');
var $selectAdministracionPublica = $('#selectAdministracionPublica');
var $selectOrganismosRaiz = $('#selectOrganismosRaiz');
var $selectOrganismosEmisores = $('#selectOrganismosEmisores');
var $selectProvincias = $('#selectProvincias');
var $selectCCAA = $('#selectCCAA');

$(function () {

    $(document).ready(function () {
        //$("#overlay").show();

        setTimeout(cargarSelectAdministracionPublica, 500);
        setTimeout(cargarSelectProvincias, 500);
        setTimeout(cargarSelectCCAA, 500);

        $('#table').on('load-success.bs.table', function (e) {
            var $regPagina = $('#regPagina');
            $regPagina.text("Organismos por página");
        });

        $('#btn-buscar').click(function (e) {
            e.preventDefault();

            if ($selectAdministracionPublica.val() == '' && $selectOrganismosRaiz.val() == '' &&
                $selectOrganismosEmisores.val() == '' && $selectProvincias.val() == '' && $selectCCAA.val() == '') {
                setTimeout(GetOrganismosEmisoresTabla, 1);
            }
            else {
                var data = {
                    administracionPublica: ($selectAdministracionPublica.val() == '' ? null : $selectAdministracionPublica.val()),
                    organismoRaiz: ($selectOrganismosRaiz.val() == '' ? null : $selectOrganismosRaiz.val()),
                    idOrganoEmisor: ($selectOrganismosEmisores.val() == '' ? null : $selectOrganismosEmisores.val()),
                    idProvincia: ($selectProvincias.val() == '' ? null : $selectProvincias.val()),
                    idCCAA: ($selectCCAA.val() == '' ? null : $selectCCAA.val()),
                };

                setTimeout(GetOrganismosEmisoresFiltro, 1, data);
            }
        });

        $('#btn-limpiar').click(function (e) {
            $('#selectAdministracionPublica').selectpicker('val', '');
            cargarSelectOrganismoRaiz('');
            cargarSelectOrganismosEmisores('');
            $('#selectProvincias').selectpicker('val', '');
            $('#selectCCAA').selectpicker('val', '');
        });
    });

    function GetOrganismosEmisoresFiltro(data) {
        $.ajax({
            async: true,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Administracion/GetOrganismosEmisoresTablaFiltro',
            data: data,
            dataType: "json",
            success: function (data) {
                $table.bootstrapTable('removeAll');
                $table.bootstrapTable('load', data);
                export_fileName = 'Organismos';

                $('#regPagina').innerText = "Organismos por página";
                $("#overlay").hide();
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio '" + urlFuncion + "'");
            }
        });
    }
});