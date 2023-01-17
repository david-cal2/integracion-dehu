using PSOENotificaciones.Contexto;
using PSOENotificaciones.Interfaces;
using System;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace PSOENotificaciones.Servicios
{
    [DataContract(Namespace = "http://example.com/eg2")]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class PsoePeticionesAccesoServicio : System.ServiceModel.ClientBase<IPsoePeticionesAcceso>, IPsoePeticionesAcceso
    {
        public PsoePeticionesAccesoServicio(System.ServiceModel.Channels.Binding binding,
            System.ServiceModel.EndpointAddress remoteAddress) :
                base(binding, remoteAddress)
        {
        }
        
        public string comparecencia(string peticioneComparencia)
        {
            return base.Channel.comparecencia(peticioneComparencia);
        }

        public string accesoComunicacion(string peticioneaccederComunicacion)
        {
            return base.Channel.accesoComunicacion(peticioneaccederComunicacion);
        }

        public string consultaAcuse(string peticioneconsultaAcuse)
        {
            return base.Channel.consultaAcuse(peticioneconsultaAcuse);
        }

        public string consultaAnexo(string peticioneconsultaAnexo)
        {
            return base.Channel.consultaAnexo(peticioneconsultaAnexo);
        }
    }

    public class EjecuteClientAcceso
    {
        public EjecuteClientAcceso()
        {
        }
  
        public static string respuesta = null;

        public object LlamarServicioJava(int tipoEjecucionJava, string identificador, AnexoReferencia anexoReferencia = null)
        {
            if (tipoEjecucionJava == (int)TipoEjecucionJava.AccederComunicacion || tipoEjecucionJava == (int)TipoEjecucionJava.LeerComunicacion)
            {    
                try
                {
                    ClientNetparaServiciosJavaAcceso.LlamarJava("AccederComunication", identificador);
                }
                catch
                {

                }
            }

            if (tipoEjecucionJava == (int)TipoEjecucionJava.Acuse)
            {
                try
                {
                    ClientNetparaServiciosJavaAcceso.LlamarJava("Acuse", identificador);
                }
                catch 
                { 

                }
            }

            if (tipoEjecucionJava == (int)TipoEjecucionJava.Anexos)
            {
                try
                {
                    ClientNetparaServiciosJavaAcceso.LlamarJava("Anexos", identificador,  anexoReferencia);
                }
                catch
                {

                }
            }
            
            if (tipoEjecucionJava == (int)TipoEjecucionJava.Comparecer)
            {
                try
                {
                    ClientNetparaServiciosJavaAcceso.LlamarJava("Comparecida", identificador);
                }
                catch
                {
                   
                }
            }

            return null;
        }
    }

    public class ClientNetparaServiciosJavaAcceso
    {
        public static string LlamarJava(string typeservice, string identificador, AnexoReferencia anexoReferencia_ =null)
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
              "IPsoeNotificaciones", "http://localhost:8082/SKDehuWs");

            var dispatchRuntime = endpointDispatcher.DispatchRuntime;

            PsoePeticionesAccesoServicio psoePeticionesAccesoServicio = new PsoePeticionesAccesoServicio(webbinding, endpointAddress);

            WebHttpBehavior webBehavior = new WebHttpBehavior
            {
                AutomaticFormatSelectionEnabled = true,
                DefaultOutgoingRequestFormat = System.ServiceModel.Web.WebMessageFormat.Json,
                DefaultOutgoingResponseFormat = System.ServiceModel.Web.WebMessageFormat.Json,
                FaultExceptionEnabled = true,
                HelpEnabled = true,
                DefaultBodyStyle = System.ServiceModel.Web.WebMessageBodyStyle.Bare
            };

            psoePeticionesAccesoServicio.Endpoint.Behaviors.Add(webBehavior);

            MyMessageInspector._switch = 3;

            psoePeticionesAccesoServicio.Endpoint.EndpointBehaviors.Add(new InspectorBehavior());

            Envios envios = new Envios();
            var envios_ = envios.GetEnvioDetalle(identificador);
            var env = webbinding.EnvelopeVersion;

            string nifReceptor = ConfigurationManager.AppSettings["nifReceptor"];
            string nombreReceptor = ConfigurationManager.AppSettings["nombreReceptor"];

            if (typeservice == "LeerComunicacion" || typeservice == "AccederComunication")//AccederComunication
            {
                var parametros =
                    "{'identificador':'" + identificador + "','concepto':'" + envios_.Concepto + "','codigoOrigen':'" + envios_.CodigoOrigen + "','nifReceptor':'" + nifReceptor + "', 'nombreReceptor':'" + nombreReceptor + "'}";

                return psoePeticionesAccesoServicio.accesoComunicacion(parametros);
            }
            
            if (typeservice == "Acuse")
            {
                GestNotifContext db = new GestNotifContext();

                var respuestaAccesoId = db.RespuestasAcceso.Where(l => l.Envios_Identificador == identificador).FirstOrDefault();
                var csvresguardo = db.DetalleDocumentos.Where(l => l.RespuestasAcceso_ID == respuestaAccesoId.ID).FirstOrDefault().CsvResguardo;

                var parametros =
                    "{'identificador':'" + identificador + "','codigoOrigen':'" + envios_.CodigoOrigen + "','csvResguardo':'" + csvresguardo + "','nifReceptor':'" + nifReceptor + "'}";

                return psoePeticionesAccesoServicio.consultaAcuse(parametros);
            }

            if (typeservice == "Anexos")
            {
                GestNotifContext db = new GestNotifContext();

                var petiaccesoId = db.PeticionesAcceso.Include("Usuarios").Where(l => l.Envios.Identificador == identificador).FirstOrDefault();
                var respuestaAccesoId = db.RespuestasAcceso.Where(l => l.Envios_Identificador== identificador).FirstOrDefault();
                var annexoReferencia = db.AnexoReferencia.Where(l => l.ID ==  anexoReferencia_.ID).FirstOrDefault();

                if (annexoReferencia != null)
                {
                    var parametros =
                        "{'identificador':'" + identificador + "','codigoOrigen':'" + envios_.CodigoOrigen + "','referenciaDocumento':'" + annexoReferencia.ReferenciaDocumento + "','nifReceptor':'" + nifReceptor + "'}";

                    return psoePeticionesAccesoServicio.consultaAnexo(parametros);
                }
            }

            if (typeservice == "Comparecida")
            {
                var parametros =
                    "{'identificador':'" + identificador + "','concepto':'" + envios_.Concepto + "','codigoOrigen':'" + envios_.CodigoOrigen + "','nifReceptor':'" + nifReceptor + "', 'nombreReceptor':'" + nombreReceptor + "'}";

                return psoePeticionesAccesoServicio.comparecencia(parametros);
            }
            else
            {
                return "";
            }
        }
    }

    public enum TipoEjecucionJava
    {
        Comparecer = 1,
        LeerComunicacion = 2,
        AccederComunicacion = 3,
        Acuse = 4,
        Anexos = 5
    }
}
