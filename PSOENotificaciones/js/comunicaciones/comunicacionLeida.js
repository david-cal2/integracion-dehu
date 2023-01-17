$(function () {

    verMenuPorPerfil();

    var errorComparecerDescripcion = "";
    var errorComparecerMetodo = "";

    $(document).ready(function () {
        actulizarCantidades();
        cargarRespuestaAcceso();
        cargarComunicacionDetalle();
        cargarContenidosDocumentos();
        comprobarErrores();
    });

    function comprobarErrores() {
        if (leer == "1") {
            if (errorNet == "0") {
                if (errorConectarDehu == "0") {
                    if (errorComparecerDescripcion != "") {
                        if (errorComparecerDescripcion == "Could not send Message." || errorComparecerDescripcion == "3009 No se ha podido recuperar el documento") {
                            $('#msg-status')[0].innerHTML = errorComparecerDescripcion;
                            $('#modalErrorDehu').modal('show');
                        }
                        else {
                            if (errorComparecerDescripcion == "4210 No se puede acceder a una comunicación ya leída") {
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
            url: '/Comunicaciones/GetCantidadEnviosLeida',
            dataType: "json",
            success: function (data) {
                if (data.CantidadesTotales != null) {
                    $('#cantSinAsignar')[0].innerHTML = (data.CantidadesTotales[0].Value > 99 ? '<span>+</span>99' : data.CantidadesTotales[0].Value);
                    $('#cantAsignadas')[0].innerHTML = (data.CantidadesTotales[2].Value > 99 ? '<span>+</span>99' : data.CantidadesTotales[2].Value);
                    $('#cantLeidas')[0].innerHTML = (data.CantidadesTotales[5].Value > 99 ? '<span>+</span>99' : data.CantidadesTotales[5].Value);
                }
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'GetCantidadEnviosLeida'");
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
                if (data.hasOwnProperty('responseText'))
                    toastr.error("Error en la llamada al servicio 'GetRespuestaAcceso'");
                else
                    $('#fechaAcceso')[0].innerHTML = dateFormat(data.Fecha);
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'GetRespuestaAcceso'");
            }
        });
    }

    function cargarComunicacionDetalle() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Comunicaciones/GetComunicacionDetalle',
            data: {
                identificador: identificador
            },
            dataType: "json",
            success: function (data) {
                if (data.hasOwnProperty('responseText')) {
                    toastr.error("Error en la llamada al servicio 'GetComunicacionDetalle'");
                    window.location.href = "../Comunicaciones/Comunicaciones";
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
                toastr.error("Error en la llamada al servicio 'GetComunicacionDetalle'");
            }
        });
    }

    function cargarContenidosDocumentos() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Comunicaciones/GetContenidosDocumentos',
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

                    if (errorComparecerMetodo != "") {
                        //Error Acceso
                        //if (errorComparecerMetodo == "ACCESO O COMPARECENCIA")
                        //    $('#btnLlamarDocumento').removeClass("display-none");

                        //Error Anexos
                        if (errorComparecerMetodo == "SOLICITUD DE ANEXOS DE REFERENCIA")
                            $('#btnLlamarAnexos').removeClass("display-none");
                        else
                            $('#btnLlamarAnexos').addClass("display-none");
                    }

                    //Si no hay anexos url ni referencias de anexos no se muestra el título de anexos
                    if ((data.ListaAnexosUrl.length == null || data.ListaAnexosUrl.length == 0) &&
                        (data.AnexosRefAcceso == null || data.AnexosRefAcceso.length == 0)) {
                        $('#titulo-anexos').addClass("display-none");
                    }

                    //Si hay referencias de anexos pero no tenemos el detalle ni el contenido de los mismos, se muestra el botón 'llamar anexos'
                    if ((data.AnexosRefAcceso != null && data.AnexosRefAcceso.length > 0) &&
                        (data.ListaAnexosReferencia == null || data.ListaAnexosReferencia.length == 0)) {
                        $('#btnLlamarAnexos').removeClass("display-none");
                    }
                }
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'GetContenidosDocumentos'");
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
        var $itemMenuComunicacionesEstado1 = $('#item-menu-comunicaciones-estado-1');
        var $itemMenuComunicacionesEstado3 = $('#item-menu-comunicaciones-estado-3');
        var $itemMenuComunicacionesEstado6 = $('#item-menu-comunicaciones-estado-6');

        if (idPerfil == Perfiles.Asignador) {
            $itemMenuComunicacionesEstado3.addClass("no-visible");
            $itemMenuComunicacionesEstado6.addClass("no-visible");
        }

        if (idPerfil == Perfiles.Gestor) {
            $itemMenuComunicacionesEstado1.addClass("display-none");
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

    function LlamarAnexos() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/Comunicaciones/LlamarAnexos',
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
})
