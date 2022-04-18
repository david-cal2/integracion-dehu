using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ConsoleAppPruebaDehu
{
    public enum Evento
    {
        Aceptada = 1
    }

    public class HashDocumento
    {
        public string hash { get; set; }
        public string algoritmoHash { get; set; }
    }

    public class DetalleDocumento
    {
        public string Nombre { get; set; }
        public MemoryStream Contenido { get; set; }
        public HashDocumento HashDocumento { get; set; }
        public string MimeType { get; set; }
        public string Metadatos { get; set; }
        public string EnlaceDocumento { get; set; }
        public byte[] ReferenciaDocumento { get; set; }
        public byte[] ReferenciaPdfAcuse { get; set; }
        public string CsvResguardo { get; set; }
    }

    public class AnexoUrl
    {
        public string Nombre { get; set; }
        public string MimeType { get; set; }
        public string EnlaceDocumento { get; set; }
    }

    public class AnexosUrl
    {
        public AnexoUrl anexoUrl { get; set; }
    }

    public class AnexoReferencia
    {
        public string Nombre { get; set; }
        public string MimeType { get; set; }
        public string ReferenciaDocumento { get; set; }
    }

    public class AnexosReferencia
    {
        public string anexoReferencia { get; set; }
        public MemoryStream AnexosUrl { get; set; }
    }

    public class Anexos
    {
        public AnexosReferencia AnexosReferencia { get; set; }
        public AnexosUrl AnexosUrl { get; set; }
    }

    public class PeticionAcceso
    {
        public string Identificador { get; set; }
        public int CodigoOrigen { get; set; }
        public string NifReceptor { get; set; }
        public string NombreReceptor { get; set; }
        public Evento Evento { get; set; }
        public string Concepto { get; set; }
        public Opciones OpcionesPeticionAcceso { get; set; }
    }

    public class RespuestaAcceso
    {
        public string CodigoRespuesta { get; set; }
        public string DescripcionRespuesta { get; set; }
        public string Identificador { get; set; }
        public int CodigoOrigen { get; set; }
        public string FechaEvento { get; set; }
        public DetalleDocumento Documento { get; set; }
        public Anexos Anexos { get; set; }
        public Opciones OpcionesRespuestaPeticionAcceso { get; set; }
    }

    class Acceso
    {
        private static async void GetNotificacionesAcceso()
        {
            var url = $"https://se-dehuws.redsara.es/wsdl/GD_Dehu/v2/Gd-Dehu-Ws_se.wsdl";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            var client = new RestClient(url);
                            RestRequest respuesta = new RestRequest();
                            var response2 = client.ExecuteAsync<PeticionAcceso>(respuesta);

                            //var repositories = await JsonSerializer.DeserializeAsync<RespuestaLocaliza>(strReader);
                            //Console.WriteLine(repositories);

                            //string responseBody = objReader.ReadToEnd();
                            // Do something with responseBody
                            //Console.WriteLine(responseBody);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                // Handle error
            }
        }
    }
}
