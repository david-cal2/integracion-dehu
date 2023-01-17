using PSOENotificaciones.Contexto;
using PSOENotificaciones.Interfaces;
using System;
using System.Configuration;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace PSOENotificaciones.Servicios
{
    [DataContract(Namespace = "http://example.com/eg2")]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    class DehuWsComunicaciones : System.ServiceModel.ClientBase<IDehuWsComunicaciones>, IDehuWsComunicaciones
    {
        public DehuWsComunicaciones(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : base(binding, remoteAddress)
        {
        }

        public  string localizaComunicacionesSinAsignar(string peticionComunicaciones)
        {
            return base.Channel.localizaComunicacionesSinAsignar(peticionComunicaciones);
        }
    }

    public class EjecuteClientComunicaciones
    {
        public static string respuesta = null;

        public object Llama(DateTime? fechadesde, DateTime? fechahasta, int idPeticionLocaliza)
        {
            ClientNetparaServiciosJavaComunicaciones.LlamarJava(fechadesde, fechahasta, idPeticionLocaliza);
            return null;
        }
    }

    class ClientNetparaServiciosJavaComunicaciones
    {
        public static string LlamarJava(DateTime? fechadesde , DateTime? fechahasta, int idPeticionLocaliza)
        {
            var webbinding = new WebHttpBinding
            {
                CloseTimeout = new TimeSpan(0, 10, 0),
                OpenTimeout = new TimeSpan(0, 10, 0),
                ReceiveTimeout = new TimeSpan(0, 10, 0),
                SendTimeout = new TimeSpan(0, 10, 0),
                UseDefaultWebProxy = false,
                AllowCookies = false,
                BypassProxyOnLocal = true,
                TransferMode = TransferMode.Buffered,
                MaxReceivedMessageSize = 15000000
            };

            EndpointAddress endpointAddress = new
                EndpointAddress(new Uri("http://localhost:8082/SKDehuWs"));

            EndpointDispatcher endpointDispatcher = new EndpointDispatcher(endpointAddress,
              "IPsoeComunicaciones", "http://localhost:8082/SKDehuWs");
            _ = endpointDispatcher.DispatchRuntime;

            DehuWsComunicaciones dehuWsComunicaciones = new DehuWsComunicaciones(webbinding, endpointAddress);

            WebHttpBehavior webBehavior = new WebHttpBehavior
            {
                AutomaticFormatSelectionEnabled = true,
                DefaultOutgoingRequestFormat = System.ServiceModel.Web.WebMessageFormat.Json,
                DefaultOutgoingResponseFormat = System.ServiceModel.Web.WebMessageFormat.Json,
                FaultExceptionEnabled = true,
                HelpEnabled = true,
                DefaultBodyStyle = System.ServiceModel.Web.WebMessageBodyStyle.Bare
            };

            dehuWsComunicaciones.Endpoint.Behaviors.Add(webBehavior);
            MyMessageInspector._switch = 2;
            dehuWsComunicaciones.Endpoint.EndpointBehaviors.Add(new InspectorBehavior());
            _ = webbinding.EnvelopeVersion;

            string nifTitular = ConfigurationManager.AppSettings["nifTitular"];
            string nifDestinatario = ConfigurationManager.AppSettings["nifDestinatario"];
            string SfechaDesde = "";
            string SfechaHasta = "";

            if (fechadesde != null && fechahasta != null)
            {
                SfechaDesde = fechadesde.ToString().Replace('/', '-');
                SfechaHasta = fechahasta.ToString().Replace('/', '-');
            }

            var parametros = "{'fechaDesde':'" + SfechaDesde + "','fechaHasta':'" + SfechaHasta + "','nifTitular':'" + nifTitular + "','nifDestinatario':'" + nifDestinatario + "'}";
            PeticionesLocaliza pl = new PeticionesLocaliza();
            pl.SetPeticionLocalizaXml(idPeticionLocaliza, parametros);

            return dehuWsComunicaciones.localizaComunicacionesSinAsignar(parametros);
        }
    }
}
