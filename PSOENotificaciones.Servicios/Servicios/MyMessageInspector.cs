using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;

namespace PSOENotificaciones.Servicios
{
    public class MyMessageInspector : IClientMessageInspector
    {
        public static int? _switch = null;

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            switch (_switch)
            {
                case (int)TipoPeticionesCliente.Notificaciones:
                    EjecuteClientNotificaciones.respuesta = reply.ToString();
                    //File.WriteAllText(@"C:\d\workarea\proyectos\PSOENotificaciones.ConexionBD\EnviosPruebasNotif.txt", reply.ToString());
                    break;
                case (int)TipoPeticionesCliente.Comunicaciones:
                    EjecuteClientComunicaciones.respuesta = reply.ToString();
                    //File.WriteAllText(@"C:\d\workarea\proyectos\PSOENotificaciones.ConexionBD\EnviosPruebasCom.txt", reply.ToString());
                    break;
                case (int)TipoPeticionesCliente.Comparecencia:
                    //ComparenciaNotificationes Totale 
                    EjecuteClientAcceso.respuesta = reply.ToString();
                    //File.WriteAllText(@"C:\d\workarea\proyectos\PSOENotificaciones.ConexionBD\EnviosPruebasCom.txt", reply.ToString());
                    break;

                default:
                    break;
            }
            _switch = null;


            string MessageTemplate = @"<root type=""object""></root>";
            byte[] BinaryMessage = Encoding.ASCII.GetBytes(MessageTemplate);

            Stream stream = new MemoryStream(BinaryMessage);

            XmlDictionaryReader xdr =
                   XmlDictionaryReader.CreateTextReader(stream,
                               new XmlDictionaryReaderQuotas());
            MessageVersion ver = reply.Version;
            reply = Message.CreateMessage(ver, "GetDataResponse", xdr);
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            Console.WriteLine("request : " + request.ToString());
            return request;
        }

        public string LastRequestXML { get; private set; }

        public string LastResponseXML { get; private set; }

        public enum TipoPeticionesCliente
        {
            Notificaciones = 1,
            Comunicaciones = 2,
            Comparecencia = 3
        }
    }

    public class InspectorBehavior : IEndpointBehavior
    {
        public string LastRequestXML
        {
            get
            {
                return myMessageInspector.LastRequestXML;
            }
        }

        public string LastResponseXML
        {
            get
            {
                return myMessageInspector.LastResponseXML;
            }
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection
            bindingParameters)
        {

        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {

        }

        public void Validate(ServiceEndpoint endpoint)
        {

        }

        private readonly MyMessageInspector myMessageInspector = new MyMessageInspector();

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(myMessageInspector);
        }
    }
}