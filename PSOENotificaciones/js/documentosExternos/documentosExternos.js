$(function () {

    var typeMime;
    var nombreV;

    $(document).ready(function () {
        //CargarContenidosDocumentosExternos();
    });

    $('.btnSubirDoc').click(function (e) {
        e.preventDefault();

        var data = new FormData();
        var files = $("#fileDoc").get(0).files;
        var idTipo = $('#selectTipoDocumento').val();

        if (files.length > 0) {
            if (idTipo != "Seleccionar") {
                $('#subirDocModal').modal('hide');
                $("#overlay").show();

                data.append("UploadedFile", files[0]);
                typeMime = files[0].type;

                $.ajax({
                    type: "POST",
                    url: "/DocumentosExternos/UploadFile",
                    contentType: false,
                    processData: false,
                    data: data,
                    success: function (data) {
                        InsertDocumentoExterno();
                        CargarContenidosDocumentosExternos();
                        $("#overlay").hide();
                    }
                });
            }
            else {
                toastr.warning("Seleccione el tipo de archivo");
            }
        }
        else {
            toastr.warning("Seleccione un archivo");
        }
    });

    $('body').on('change', '#fileDoc', function () {
        nombreV = $("#fileDoc").get(0).files[0].name;
        $(".nombreArchivo")[0].innerText = $("#fileDoc").get(0).files[0].name;
        $(".upload-ok").show();
    });

    function InsertDocumentoExterno() {
        var $selectTipoDocumento = $('#selectTipoDocumento');

        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/DocumentosExternos/InsertDocumentoExterno',
            data: {
                identificador: identificador,
                typeMime: typeMime,
                nombre: nombreV,
                descripcion: $('#textDescripcionArchivo')[0].value,
                tipo: $selectTipoDocumento.val()
            },
            dataType: "json",
            success: function (data) {
                if (data.hasOwnProperty('responseText'))
                    toastr.error("Error en la llamada al servicio 'InsertDocumentoExterno'");
                else {
                    toastr.success("Archivo subido correctamente");
                    //$('#subirDocModal').modal('hide');
                }     
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'InsertDocumentoExterno'");
            }
        });
    }

    function CargarContenidosDocumentosExternos() {
        $.ajax({
            async: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: '/DocumentosExternos/GetDocumentosExternos',
            data: {
                identificador: identificador
            },
            dataType: "json",
            success: function (data) {
                if (data.hasOwnProperty('responseText'))
                    toastr.error("Error en la llamada al servicio 'GetDocumentosExternos'");
                else {
                    $("#contenedor-principal")[0].innerHTML = "";
                    $("#contenedor-acuse")[0].innerHTML = "";
                    $("#contenedor-anexos")[0].innerHTML = "";
                    $("#contenedor-otros")[0].innerHTML = "";

                    if (data != null && data.length > 0) {
                        for (var i = 0; i < data.length; i++) {
                            AddDocExterno(data[i], i);
                        }
                    }
                }
            },
            error: function (data) {
                toastr.error("Error en la llamada al servicio 'GetDocumentosExternos'");
            }
        });
    }

    function AddDocExterno(item, index) {
        if (item.Documento != null) {
            var icono = GetIconoDocExterno(item.TypeMime);
            var codigoHtmlExt = "<li><a id='linkDocAnexoRef-" + index + "' class='doc-link doc-ext-ref' onclick='javascript:abrirAnexo(this);' href='' title='Ver documento' code='data:" + item.TypeMime + ";base64," + encodeURI(item.Documento) + "'>" +
                "<span class='document " + icono + "'></span>" + item.Nombre + "</a></li>";

            if (item.TiposDocumentosExternos_ID == TipoDocumentoExterno.Principal) {
                $("#contenedor-principal")[0].innerHTML = $("#contenedor-principal")[0].innerHTML + codigoHtmlExt;
            }

            if (item.TiposDocumentosExternos_ID == TipoDocumentoExterno.Anexo) {
                $("#contenedor-acuse")[0].innerHTML = $("#contenedor-acuse")[0].innerHTML + codigoHtmlExt;
            }

            if (item.TiposDocumentosExternos_ID == TipoDocumentoExterno.Acuse) {
                $("#contenedor-anexos")[0].innerHTML = $("#contenedor-anexos")[0].innerHTML + codigoHtmlExt;
            }

            if (item.TiposDocumentosExternos_ID == TipoDocumentoExterno.Otros) {
                $("#contenedor-otros")[0].innerHTML = $("#contenedor-otros")[0].innerHTML + codigoHtmlExt;
            }
        }
    }

    function GetIconoDocExterno(mimeType) {
        if (mimeType == "application/pdf")
            return "doc_doc_up"
        else if (mimeType == "application/zip")
            return "doc_zip"
        else if (mimeType == "text/html")
            return "doc_file"
        else
            return "doc_doc_up";
    }
})