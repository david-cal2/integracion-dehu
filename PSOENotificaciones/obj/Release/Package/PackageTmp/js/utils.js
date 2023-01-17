function dateFormat(value, row, index) {
    if (value != null)
        return moment(value).format('DD/MM/YYYY');
    else
        return '';
}

function dateFormatAll(value, row, index) {
    if (value != null)
        return moment(value).format('DD/MM/YYYY HH:mm:ss');
    else
        return '';
}

function dateFormatTiempo(value, row, index) {
    if (value != null)
        return moment(value).format('HH:mm:ss');
    else
        return '';
}

function datesSorter(a, b) {
    var aValue = moment(a).format('DD/MM/YYYY');
    if (a === '' || a === null) { aValue = 0; }

    var bValue = moment(b).format('DD/MM/YYYY');
    if (b === '' || b === null) { bValue = 0; }

    return aValue < bValue ? -1 : aValue > bValue ? 1 : 0;
}

function cargarSelectAdministracionPublica() {
    $.ajax({
        async: true,
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        url: '/Administracion/GetAdministracionPublica',
        dataType: "json",
        success: function (data) {
            data.forEach(value => {
                $selectAdministracionPublica.append($("<option>")
                    .val(value)
                    .html(value)
                );
            });

            $selectAdministracionPublica.selectpicker('refresh');
            $selectAdministracionPublica.selectpicker('render');
        }
    });
}

function cargarSelectOrganismoRaiz(administracionPublica) {
    $.ajax({
        async: true,
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        url: '/Administracion/GetOrganismosRaiz',
        data: {
            administracionPublica: administracionPublica
        },
        dataType: "json",
        success: function (data) {
            $selectOrganismosRaiz.empty();

            $selectOrganismosRaiz.append($("<option>")
                .val("Seleccionar")
                .html("Seleccionar")
            );

            data.forEach(value => {
                $selectOrganismosRaiz.append($("<option>")
                    .val(value)
                    .html(value)
                );
            });

            $selectOrganismosRaiz.prop('disabled', false);
            $selectOrganismosRaiz.selectpicker('refresh');
            $selectOrganismosRaiz.selectpicker('render');
        }
    });
}

function cargarSelectOrganismosEmisores(organismoRaiz) {
    $.ajax({
        async: true,
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        url: '/Administracion/GetOrganismosEmisores',
        data: {
            organismoRaiz: organismoRaiz
        },
        dataType: "json",
        success: function (data) {
            $selectOrganismosEmisores.empty();

            $selectOrganismosEmisores.append($("<option>")
                .val("Seleccionar")
                .html("Seleccionar")
            );

            data.forEach(value => {
                $selectOrganismosEmisores.append($("<option>")
                    .val(value.ID)
                    .html(value.OrganismoEmisor)
                );
            });

            $selectOrganismosEmisores.prop('disabled', false);
            $selectOrganismosEmisores.selectpicker('refresh');
            $selectOrganismosEmisores.selectpicker('render');
        },
        error: function (data) {
            toastr.error("Error en la llamada al servicio 'GetOrganismosEmisores'");
        }
    });
}

function setSelectAdministracionPublica() {
    cargarSelectOrganismoRaiz($selectAdministracionPublica.val());
}

function setSelectOrganismosRaiz() {
    cargarSelectOrganismosEmisores($selectOrganismosRaiz.val());
}

function cargarSelectAsuntos() {
    $.ajax({
        async: true,
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        url: '/Base/GetAsuntos',
        data: {
            idTipoEnvios: idTipoEnvios,
            idEstado: (idEstado == "0" ? null : idEstado)
        },
        dataType: "json",
        success: function (data) {
            data.forEach(value => {
                $selectAsuntos.append($("<option>")
                    .val(value)
                    .html(value)
                );
            });

            $selectAsuntos.selectpicker('refresh');
            $selectAsuntos.selectpicker('render');
        }
    });
}

function cargarSelectEstados() {
    $.ajax({
        async: true,
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        url: '/Base/GetEstados',
        data: {
            idTipoEnvios: idTipoEnvios
        },
        dataType: "json",
        success: function (data) {
            data.forEach(value => {
                $selectEstados.append($("<option>")
                    .val(value.ID)
                    .html(value.Descripcion)
                );
            });

            $selectEstados.selectpicker('refresh');
            $selectEstados.selectpicker('render');
        }
    });
}

function cargarSelectGrupos() {
    $.ajax({
        async: true,
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        url: '/Administracion/GetGruposAcitvos',
        dataType: "json",
        success: function (data) {
            data.forEach(value => {
                $selectGrupos.append($("<option>")
                    .val(value.ID)
                    .html(value.Nombre)
                );
                $selectGruposModal.append($("<option>")
                    .val(value.ID)
                    .html(value.Nombre)
                );
            });

            $selectGrupos.selectpicker('refresh');
            $selectGrupos.selectpicker('render');

            $selectGruposModal.selectpicker('refresh');
            $selectGruposModal.selectpicker('render');
        }
    });
}

function cargarSelectProvincias() {
    $.ajax({
        async: true,
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        url: '/Administracion/GetProvincias',
        dataType: "json",
        success: function (data) {
            data.forEach(value => {
                $selectProvincias.append($("<option>")
                    .val(value.ID)
                    .html(value.Provincia)
                );
            });

            $selectProvincias.selectpicker('refresh');
            $selectProvincias.selectpicker('render');
        }
    });
}

function cargarSelectCCAA() {
    $.ajax({
        async: true,
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        url: '/Administracion/GetComunidadesAutonomas',
        dataType: "json",
        success: function (data) {
            data.forEach(value => {
                $selectCCAA.append($("<option>")
                    .val(value.ID)
                    .html(value.ComunidadAutonoma)
                );
            });

            $selectCCAA.selectpicker('refresh');
            $selectCCAA.selectpicker('render');
        }
    });
}

function validarDNI(dni) {
    var DniRegex = /^[0-9]{8,8}[A-Za-z]$/g;
    return (DniRegex.test(dni) && dni.length == 9);
}

$('#txtNIF').keyup(function (e) {
    e.currentTarget.value = e.currentTarget.value.toUpperCase();
});

$('#txtNIF').change(function (e) {
    e.currentTarget.value = e.currentTarget.value.padStart(9, '0');
});

$('#dni-usuario').keyup(function (e) {
    e.currentTarget.value = e.currentTarget.value.toUpperCase();
});

$('#dni-usuario').change(function (e) {
    e.currentTarget.value = e.currentTarget.value.padStart(9, '0');
});

$('.linkManual').click(function (e) {
    e.preventDefault();
    window.open('../pdf/ManualUsuario.pdf', '_blank');
});

const TipoEnvios = {
    Comunicaciones: 1,
    Notificaciones: 2
}

const Estados = {
    SinAsignar: 1,
    EnAlerta: 2,
    Asignadas: 3,
    Comparecidas: 4,
    Caducadas: 5,
    Leidas: 6,
    Externas: 7
}

const Perfiles = {
    Administrador: 1,
    Asignador: 2,
    Gestor: 3,
    UsuarioConsulta: 4
}

const TipoEmails = {
    AltaUsuario: 1,
    BloqueoUsuario: 2,
    DesbloqueoUsuario: 3,
    ProcesoCarga: 4,
    ProcesoCargaError: 5,
    AsignacionManual: 6,
    EnAlerta: 7,
    Caducadas: 8,
    Externas: 9,
}

const TipoDocumentoExterno = {
    Principal: 1,
    Anexo: 2,
    Acuse: 3,
    Otros: 4
}

toastr.options = {
    "closeButton": false,
    "debug": false,
    "newestOnTop": false,
    "progressBar": false,
    "positionClass": "toast-top-center",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000"
}