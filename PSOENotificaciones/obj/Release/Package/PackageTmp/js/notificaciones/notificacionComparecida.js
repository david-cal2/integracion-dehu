$(function () {

    verMenuPorPerfil();

    var errorComparecerDescripcion = "";
    var errorComparecerMetodo = "";

    $(document).ready(function () {
        actulizarCantidades();
        cargarRespuestaAcceso();
        cargarNotificacionDetalle();
        cargarContenidosDocumentos();
        comprobarCaducidad();
        comprobarErrores();
    });

    function comprobarErrores() {
        if (comparecer == "1") {
            if (errorNet == "0") {
                if (errorConectarDehu == "0") {
                    if (errorComparecerDescripcion != "") {
                        if (errorComparecerDescripcion == "Could not send Message." || errorComparecerDescripcion == "3009 No se ha podido recuperar el documento") {
                            $('#msg-status')[0].innerHTML = errorComparecerDescripcion;
                            $('#modalErrorDehu').modal('show');
                        }
                        else {
                            if (errorComparecerDescripcion == "4209 No se puede acceder a una notificación ya comparecida") {
                                $('#modalYaComparecida').modal('show');
                            }
                            else {
                                $('#msg-error')[0].innerHTML = errorComparecerDescripcion;
                                $('#msg-error-metodo')[0].innerHTML = errorComparecerMetodo;
                                $('#modalComparecidaConErrores').modal('show');
                            }
                        }
                    }
                    else {
                        if (status == "200" && idRespuestaAcceso != "0")
                            $('#modalComparecida').modal('show');
                        else {
                            $('#msg-status')[0].innerHTML = "Status: " + status;
                            $('#modalErrorDehu').modal('show');
                        }
                    }
                }
                else
                    $('#modalErrorDehu').modal('show');
            }
            else
                $('#modalErrorNet').modal('show');
        }
    }

    function actulizarCantidades() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Notificaciones/GetCantidadEnviosComparecida',
            dataType: "json",
            success: function (data) {
                if (data.CantidadesTotales != null) {
                    $('#cantSinAsignar')[0].innerHTML = data.CantidadesTotales[0].Value;
                    $('#cantEnAlerta')[0].innerHTML = data.CantidadesTotales[1].Value;
                    $('#cantAsignadas')[0].innerHTML = data.CantidadesTotales[2].Value;
                    $('#cantComparecidas')[0].innerHTML = data.CantidadesTotales[3].Value;
                    $('#cantCaducadas')[0].innerHTML = data.CantidadesTotales[4].Value;
                    $('#cantExternas')[0].innerHTML = data.CantidadesTotales[6].Value;
                }
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'GetCantidadEnviosComparecida'");
            }
        });
    }

    function cargarRespuestaAcceso() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Base/GetRespuestaAcceso',
            data: {
                identificador: identificador
            },
            dataType: "json",
            success: function (data) {
                if (!data.hasOwnProperty('responseText'))
                    $('#fechaComparecida')[0].innerHTML = dateFormat(data.Fecha);
            },
            error: function (data) {
                //toastr.error("Error en la llamada al servicio 'GetRespuestaAcceso'");
            }
        });
    }

    function cargarNotificacionDetalle() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Notificaciones/GetNotificacionDetalle',
            data: {
                identificador: identificador
            },
            dataType: "json",
            success: function (data) {
                if (data.hasOwnProperty('responseText')) {
                    toastr.error("Error en la llamada al servicio 'GetNotificacionDetalle'");
                    window.location.href = "../Notificaciones/Notificaciones";
                }
                else {
                    if (data.ErrorCodigo != null)
                        errorComparecerDescripcion = data.ErrorDescripcion;

                    if (data.ErrorMetodo != null)
                        errorComparecerMetodo = data.ErrorMetodo;

                    $('#fechaDisposicion')[0].innerHTML = dateFormat(data.FechaPuestaDisposicion);
                    $('#identificador')[0].innerHTML = data.Identificador;
                    $('#asunto')[0].innerHTML = data.Concepto;
                    $('#titular')[0].innerHTML = data.Personas.NombreTitular;
                    $('#organismoEmisor')[0].innerHTML = data.OrganismosEmisores.OrganismoEmisor;
                    $('#vinculo')[0].innerHTML = data.Vinculos.Descripcion;
                    $('#organismoEmisorRaiz')[0].innerHTML = data.OrganismosEmisores.OrganismoRaiz;
                    $('#descripcion')[0].innerHTML = data.Descripcion;
                    $('#textMultiObservaciones')[0].value = data.Observaciones;

                    if (data.MetadatosPublicos != null && data.MetadatosPublicos != "")
                        $('#metadatosPublicos')[0].innerHTML = decodeURIComponent(escape(window.atob(data.MetadatosPublicos)));
                    else
                        $('#div-metadatos-publicos').addClass("display-none");
                }
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'GetNotificacionDetalle'");
            }
        });
    }

    function cargarContenidosDocumentos() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Notificaciones/GetContenidosDocumentos',
            data: {
                identificador: identificador,
                idRespuestaAcceso: idRespuestaAcceso
            },
            dataType: "json",
            success: function (data) {
                if (data.hasOwnProperty('responseText'))
                    toastr.error("Error en la llamada al servicio 'GetContenidosDocumentos'");
                else {
                    //Documento principal
                    if (data.DetalleDocumento != null) {
                        if (data.DetalleDocumento.Metadatos != null)
                            $('#metadatosPrivados')[0].innerHTML = decodeURIComponent(escape(window.atob(data.DetalleDocumento.Metadatos)))

                        var icono = getIconoDoc(data.DetalleDocumento.MimeType);

                        if (data.DetalleDocumento.EnlaceDocumento != null && data.DetalleDocumento.EnlaceDocumento != "") {
                            $('#linkDocEnlace')[0].innerHTML = "<span class='document " + icono + "'></span>Documento";
                            $('#linkDocEnlace')[0].code = data.DetalleDocumento.EnlaceDocumento;
                            $('#linkDocRef').addClass("display-none");
                        }

                        if (data.Contenido != null && data.Contenido != "") {
                            $('#linkDocEnlace').addClass("display-none");
                            $('#linkDocRef')[0].innerHTML = "<span class='document " + icono + "'></span>Documento";
                            $('#linkDocRef')[0].setAttribute("code", "data:" + data.DetalleDocumento.MimeType + ";base64," + encodeURI(data.Contenido.Href));
                        }
                    }

                    //Anexos url
                    if (data.ListaAnexosUrl != null && data.ListaAnexosUrl.length > 0) {
                        for (var i = 0; i < data.ListaAnexosUrl.length; i++) {
                            addDocAnexoUrl(data.ListaAnexosUrl[i], i);
                        }
                    }

                    //Anexos referencia
                    if (data.ListaAnexosReferencia != null && data.ListaAnexosReferencia.length > 0) {
                        for (var i = 0; i < data.ListaAnexosReferencia.length; i++) {
                            addDocAnexoReferencia(data.ListaAnexosReferencia[i], i);
                        }
                    }

                    //Acuse recibo
                    if (data.AcusePdf != null && data.ContenidoAcuse != null) {
                        var icono = getIconoDoc(data.AcusePdf.MimeType);
                        $('#linkAcusePdf')[0].innerHTML = "<span class='document " + icono + "'></span>Resguardo";
                        $('#linkAcusePdf')[0].setAttribute("code", "data:" + data.AcusePdf.MimeType + ";base64," + encodeURI(data.ContenidoAcuse.Href));
                    }

                    if (errorComparecerMetodo != "") {
                        //Error Acceso
                        //if (errorComparecerMetodo == "ACCESO O COMPARECENCIA")
                        //    $('#btnLlamarDocumento').removeClass("display-none");

                        //Error Anexos
                        if (errorComparecerMetodo == "SOLICITUD DE ANEXOS DE REFERENCIA")
                            $('#btnLlamarAnexos').removeClass("display-none");
                        else
                            $('#btnLlamarAnexos').addClass("display-none");

                        //Error Acuse
                        if (errorComparecerMetodo == "SOLICITUD DE ACUSE DE RECIBO")
                            $('#btnLlamarAcuse').removeClass("display-none");
                        else
                            $('#btnLlamarAcuse').addClass("display-none");
                    }

                    //Si no hay anexos url ni referencias de anexos no se muestra el título de anexos
                    if ((data.ListaAnexosUrl.length == null || data.ListaAnexosUrl.length == 0) &&
                        (data.AnexosRefAcceso == null || data.AnexosRefAcceso.length == 0))
                    {
                        $("#pestana-3").hide();
                        $("#div-tab-3").hide();
                    }

                    //Si hay referencias de anexos pero no tenemos el detalle ni el contenido de los mismos, se muestra el botón 'llamar anexos'
                    if ((data.AnexosRefAcceso != null && data.AnexosRefAcceso.length > 0) &&
                        (data.ListaAnexosReferencia == null || data.ListaAnexosReferencia.length == 0))
                    {
                        $('#btnLlamarAnexos').removeClass("display-none");
                    }
                }
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'GetContenidosDocumentos'");
            }
        });
    }

    function comprobarCaducidad() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Notificaciones/ComprobarCaducidad',
            data: {
                identificador: identificador
            },
            dataType: "json",
            success: function (data) {
                if (data.hasOwnProperty('responseText'))
                    toastr.error("Error en la llamada al servicio 'ComprobarCaducidad'");
                else {
                    if (data.hasOwnProperty('Fecha')) {
                        $('#fechaCaducada')[0].innerHTML = dateFormat(data.Fecha);
                        $('#div-fecha-caducada').removeClass("display-none");

                        $('#textMultiCaducidad')[0].value = "Esta notificación caducó el " + dateFormat(data.Fecha) +
                            '\r\n' + "Posteriormente, con fecha " + $('#fechaComparecida')[0].innerHTML + " ha sido comparecida." +
                            '\r\n' + "Por favor, consulte el estado de la misma en la plataforma de DEHú";

                        $('#div-observaciones-caducada').removeClass("display-none");
                    }
                }
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'ComprobarCaducidad'");
            }
        });
    }

    function getIconoDoc(mimeType) {
        if (mimeType == "application/pdf")
            return "doc_doc";
        if (mimeType == "application/zip")
            return "doc_zip";
        if (mimeType == "text/html")
            return "doc_file";
    }

    function addDocAnexoUrl(item, index) {
        if (item != null) {
            var icono = getIconoDoc(item.MimeType);
            var codigoHtmlAnexoUrl = "<li><a id='linkDocAnexoUrl-" + index + "' class='doc-link doc-ref doc-enlace' code='" + item.EnlaceDocumento + "' href='javascript:void(0);' title='Ver documento' target='_blank'>" +
                "<span class='document " + icono + "'></span>" + item.Nombre + "</a></li>";

            $("#contenedor-anexos").innerHTML = $("#contenedor-anexos").innerHTML + codigoHtmlAnexoUrl;
        }
    }

    function addDocAnexoReferencia(item, index) {
        if (item.DocAnexo != null && item.ContenidoAnexo != null) {
            var icono = getIconoDoc(item.DocAnexo.MimeType);
            var codigoHtmlAnexoRef = "<li><a id='linkDocAnexoRef-" + index + "' class='doc-link doc-ref' onclick='javascript:abrirAnexo(this);' href='' title='Ver documento' code='data:" + item.DocAnexo.MimeType + ";base64," + encodeURI(item.ContenidoAnexo.Value) + "'>" +
                "<span class='document " + icono + "'></span>" + item.DocAnexo.Nombre + "</a></li>";

            $("#contenedor-anexos")[0].innerHTML = $("#contenedor-anexos")[0].innerHTML + codigoHtmlAnexoRef;
        }
    }

    function verMenuPorPerfil() {
        var $itemMenuNotificacionesEstado1 = $('#item-menu-notificaciones-estado-1');
        var $itemMenuNotificacionesEstado2 = $('#item-menu-notificaciones-estado-2');
        var $itemMenuNotificacionesEstado3 = $('#item-menu-notificaciones-estado-3');
        var $itemMenuNotificacionesEstado4 = $('#item-menu-notificaciones-estado-4');
        var $itemMenuNotificacionesEstado5 = $('#item-menu-notificaciones-estado-5');
        var $itemMenuNotificacionesEstado7 = $('#item-menu-notificaciones-estado-7');

        if (idPerfil == Perfiles.Asignador) {
            $itemMenuNotificacionesEstado2.addClass("no-visible");
            $itemMenuNotificacionesEstado3.addClass("no-visible");
            $itemMenuNotificacionesEstado4.addClass("no-visible");
            $itemMenuNotificacionesEstado5.addClass("no-visible");
            $itemMenuNotificacionesEstado7.addClass("no-visible");
        }

        if (idPerfil == Perfiles.Gestor) {
            $itemMenuNotificacionesEstado1.addClass("display-none");
        }

        $('.menu-lateral').removeClass("no-visible");
    }

    $('#textMultiObservaciones').change(function (e) {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Notificaciones/SetObservaciones',
            data: {
                identificador: identificador,
                observaciones: $(this)[0].value
            },
            dataType: "json",
            success: function (data) {
                if (data.hasOwnProperty('responseText'))
                    toastr.error("Error en la llamada al servicio 'SetObservaciones'");
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'SetObservaciones'");
            }
        });
    });

    $('.doc-ref').click(function (e) {
        var w = window.open('about:blank');
        w.document.body.appendChild(w.document.createElement('iframe')).src = e.target.getAttribute("code");
        w.document.body.style.margin = 0;
        w.document.getElementsByTagName("iframe")[0].style.width = '100%';
        w.document.getElementsByTagName("iframe")[0].style.height = '100%';
        w.document.getElementsByTagName("iframe")[0].style.border = 0;
        e.preventDefault();
    });

    $('.doc-enlace').click(function (e) {
        e.preventDefault();
        url_enlace = e.target.code;
        $('#modalAbrirEnlace').modal('show');
    });

    $('#btnLlamarAnexos').click(function (e) {
        $("#overlay").show();
        setTimeout(LlamarAnexos, 1000); 
    });

    $('#btnLlamarAcuse').click(function (e) {
        $("#overlay").show();
        LlamarAcuse(LlamarAcuse, 1000);
    });

    function LlamarAnexos() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Notificaciones/LlamarAnexos',
            data: {
                identificador: identificador,
            },
            dataType: "json",
            success: function (data) {
                $("#overlay").hide();

                if (data.hasOwnProperty('responseText')) {
                    toastr.error("Error en la llamada al servicio 'LlamarAnexos'");
                }
                else {
                    cargarContenidosDocumentos();
                }
            },
            error: function (data) {
                $("#overlay").hide();
                toastr.error("Error en la llamada al servicio 'LlamarAnexos'");
            }
        });
    }

    function LlamarAcuse() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Notificaciones/LlamarAcusePdf',
            data: {
                identificador: identificador,
            },
            dataType: "json",
            success: function (data) {
                $("#overlay").hide();

                if (data.hasOwnProperty('responseText')) {
                    toastr.error("Error en la llamada al servicio 'LlamarAcusePdf'");
                }
                else {
                    cargarContenidosDocumentos();
                }
            },
            error: function (data) {
                $("#overlay").hide();
                toastr.error("Error en la llamada al servicio 'LlamarAcusePdf'");
            }
        });
    }
})
