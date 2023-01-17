$(function () {
    $("#btnBuscar").click(function (e) {
        GetPeticiones();
        GetTablasHistorial();
    });


    function GetPeticiones() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Auditoria/GetPeticiones',
            dataType: "json",
            data: {
                identificador: $("#inputIdentificador")[0].value
            },
            success: function (data) {
                if (!data.hasOwnProperty("responseText")) {
                    if (data.auditoria != null) {

                        if (data.auditoria.Envio != null) {
                            if (data.auditoria.Envio)
                                $("#tipoEnvio")[0].innerHTML = "NOTIFICACIÓN";
                            else
                                $("#tipoEnvio")[0].innerHTML = "COMUNICACIÓN";
                        }

                        if (data.auditoria.PeticionLocaliza != null) {
                            $("#fechaLocaliza")[0].innerHTML = dateFormatAll(data.auditoria.PeticionLocaliza.Fecha);

                            if (data.auditoria.PeticionLocaliza.Usuarios == null)
                                $("#tipoPeticionLocaliza")[0].innerHTML = "AUTOMÁTICA";
                            else
                                $("#tipoPeticionLocaliza")[0].innerHTML = "MANUAL";

                            if (data.auditoria.RespuestaLocaliza != null) {
                                $("#fechaRespuesta")[0].innerHTML = dateFormatAll(data.auditoria.RespuestaLocaliza.Fecha);
                                $("#codigoRespuesta")[0].innerHTML = data.auditoria.RespuestaLocaliza.CodigoRespuesta;
                                $("#descripcionRespuesta")[0].innerHTML = data.auditoria.RespuestaLocaliza.DescripcionRespuesta;
                                $("#xmlRepuestaLocaliza")[0].value = data.auditoria.RespuestaLocaliza.NombreXml;
                            }

                            $("#div-localiza").removeClass("display-none");
                        }

                        if (data.auditoria.PeticionAcceso != null) {
                            $("#fechaAcceso")[0].innerHTML = dateFormatAll(data.auditoria.PeticionAcceso.Fecha);
                            $("#usuarioAcceso")[0].innerHTML = data.auditoria.PeticionAcceso.Usuarios.Nombre;
                            $("#conceptoAcceso")[0].innerHTML = data.auditoria.PeticionAcceso.Concepto;

                            if (data.auditoria.RespuestaAcceso != null) {
                                $("#fechaRespuestaAcceso")[0].innerHTML = dateFormatAll(data.auditoria.RespuestaAcceso.Fecha);
                                $("#codigoRespuestaAcceso")[0].innerHTML = data.auditoria.RespuestaAcceso.CodigoRespuesta;
                                $("#descripcionRespuestaAcceso")[0].innerHTML = data.auditoria.RespuestaAcceso.DescripcionRespuesta;

                                if (data.auditoria.DetalleDocumento != null) {
                                    $("#nombreDocuemnto")[0].innerHTML = data.auditoria.DetalleDocumento.Nombre;
                                    $("#mimeType")[0].innerHTML = data.auditoria.DetalleDocumento.MimeType;

                                    if (data.auditoria.HashDocumento != null) {
                                        $("#hash")[0].innerHTML = data.auditoria.HashDocumento.Hash;
                                        $("#algoritmo")[0].innerHTML = data.auditoria.HashDocumento.AlgoritmoHash;
                                    }
                                }
                            }

                            $("#div-acceso").removeClass("display-none");
                        }

                        if (data.auditoria.PeticionAcusePdf != null) {
                            if (data.auditoria.RespuestaLocaliza != null) {
                                if (data.auditoria.AcusePdf != null) {
                                    $("#nombreAcuse")[0].innerHTML = data.auditoria.AcusePdf.NombreAcuse;
                                    $("#mimeTypeAcuse")[0].innerHTML = data.auditoria.AcusePdf.MimeType;
                                }
                            }

                            $("#div-acuse").removeClass("display-none");
                        }
                    }
                }
                else
                    toastr.error("Error en la llamada al servicio 'GetPeticiones'");
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'GetPeticiones'");
            }
        });
    }

    function GetTablasHistorial() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Auditoria/GetHistorialCambiosEnvio',
            dataType: "json",
            data: {
                identificador: $("#inputIdentificador")[0].value
            },
            success: function (data) {
                if (!data.hasOwnProperty("responseText")) {
                    var $tableEstados = $('#tableEstados');
                    $tableEstados.bootstrapTable('removeAll');
                    $tableEstados.bootstrapTable('load', data.ListaEstados);

                    var $tableGrupos = $('#tableGrupos');
                    $tableGrupos.bootstrapTable('removeAll');
                    $tableGrupos.bootstrapTable('load', data.ListaGrupos);
                }
                else
                    toastr.error("Error en la llamada al servicio 'GetHistorialCambiosEnvio'");
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'GetHistorialCambiosEnvio'");
            }
        });
    }
})