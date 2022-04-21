using RestSharp;
using System;
using System.IO;
using System.Net;
using System.Text.Json;
using ServiceReference1;
using System.ServiceModel;

namespace ConsoleAppPruebaDehu
{
    public enum TipoEnvio
    {
        Comunicaciones = 1,
        Notificaciones = 2
    }

    public enum Vinculo
    {
        Titular = 1,
        Destinatario = 2,
        Apoderado = 3
    }

    public class Opciones
    {
        public string Opcion { get; set; }
    }

    public class Envios
    {
        public string Identificador { get; set; }
        public int CodigoOrigen { get; set; }
        public string Concepto { get; set; }
        public string Descripción { get; set; }
        public Organismos OrganismoEmisor { get; set; }
        public Organismos OrganismoEmisorRaiz { get; set; }
        public DateTime FechaPuestaDisposicion { get; set; }
        public TipoEnvio TipoEnvio { get; set; }
        public Vinculo Vinculo { get; set; }
        public Personas Titular { get; set; }
        public string MetadatosPublicos { get; set; }
        public Opciones OpcionesEnvio { get; set; }
    }

    public class Personas
    {
        public string NombreTitular { get; set; }
        public string NifTitular { get; set; }
        public string CodigoDIR3 { get; set; }
        public string CodigoDIRe { get; set; }
        public string DescripcionEntidad { get; set; }
    }

    public class Organismos
    {
        public string CodigoOrganismo { get; set; }
        public string NombreOrganismo { get; set; }
        public string NifOrganismo { get; set; }
    }

    public class PeticionLocaliza
    {
        public string NifTitular { get; set; }
        public string NifDestinatario { get; set; }
        public string CodigoDestino { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public TipoEnvio TipoEnvio { get; set; }
        public Opciones OpcionesLocaliza { get; set; }
    }

    public class RespuestaLocaliza
    {
        public string CodigoRespuesta { get; set; }
        public string DescripcionRespuesta { get; set; }
        public string NifPeticion { get; set; }
        public Envios Envios { get; set; }
        public bool HayMasResultados { get; set; }
        public TipoEnvio OpcionesRespuestaLocaliza { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hola Dehú - Inicio proceso Localiza");
            GetNotificacionesLocaliza();
        }

        private static void GetNotificacionesLocaliza()
        {     
            try
            {
                LocalizaRequest requestLocaliza = new LocalizaRequest();
                Localiza objLocaliza = new Localiza();

                BasicHttpBinding binding = new BasicHttpBinding();
                System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress("http://localhost/");
                DEHuWsPortTypeClient client = new DEHuWsPortTypeClient(binding, remoteAddress);

                //ServiceReference1.LocalizaResponse responseLocaliza = await client.LocalizaAsync(objLocaliza);
                System.Threading.Tasks.Task<ServiceReference1.LocalizaResponse> responseLocaliza = client.LocalizaAsync(objLocaliza);
                if (responseLocaliza.IsCompleted)
                {
                    Console.WriteLine(responseLocaliza.Result);
                    //Grabar en bbdd
                    //responseLocaliza.Result.RespuestaLocaliza.
                }  
                else
                    Console.WriteLine(responseLocaliza.Status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //var url = $"https://se-dehuws.redsara.es/wsdl/GD_Dehu/v2/Gd-Dehu-Ws_se.wsdl";
            //var request = (HttpWebRequest)WebRequest.Create(url);
            //request.Method = "GET";
            //request.ContentType = "application/json";
            //request.Accept = "application/json";
            //try
            //{
            //    using (WebResponse response = request.GetResponse())
            //    {
            //        using (Stream strReader = response.GetResponseStream())
            //        {
            //            if (strReader == null) return;
            //            using (StreamReader objReader = new StreamReader(strReader))
            //            {
            //                var client = new RestClient(url);
            //                RestRequest respuesta = new RestRequest();
            //                var response2 = client.ExecuteAsync<PeticionLocaliza>(respuesta);

            //                //var repositories = await JsonSerializer.DeserializeAsync<RespuestaLocaliza>(strReader);
            //                //Console.WriteLine(repositories);

            //                //string responseBody = objReader.ReadToEnd();
            //                // Do something with responseBody
            //                //Console.WriteLine(responseBody);
            //            }
            //        }
            //    }
            //}
            //catch (WebException ex)
            //{
            //    // Handle error
            //}
        }
    }
}
