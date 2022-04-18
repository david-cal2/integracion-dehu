using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ConsoleAppPruebaDehu
{
    public class DocumentoAnexo
    {
        public string Nombre { get; set; }
        public MemoryStream Contenido { get; set; }
        public string MimeType { get; set; }
        public string Metadatos { get; set; }
    }

    public class PeticionConsultaAnexos
    {
        public string NifReceptor { get; set; }
        public string Identificador { get; set; }
        public int CodigoOrigen { get; set; }
        public byte[] Referencia { get; set; }
        public Opciones OpcionesConsultaAnexos { get; set; }
    }

    public class RespuestaConsultaAnexos
    {
        public string CodigoRespuesta { get; set; }
        public string DescripcionRespuesta { get; set; }
        public string Documento { get; set; }
        public Opciones OpcionesRespuestaConsultaAnexo { get; set; }
    }

    class ConsultarAnexos
    {
        private static async void GetConsultaAnexos()
        {
            var url = $"https://se-dehuws.redsara.es/wsdl/GD_Dehu/v2/Gd-Dehu-Ws_se.wsdl";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            try
            {
                RestClient client = new RestClient(url);
                //client.AcceptedContentTypes = "application/json";
                RestRequest respuesta = new RestRequest();
                var response2 = client.ExecuteAsync<PeticionConsultaAnexos>(respuesta);
            }
            catch (WebException ex)
            {
                // Handle error
            }
        }
    }
}
