using System.ServiceModel;
using System.ServiceModel.Web;

namespace PSOENotificaciones.Interfaces
{
    [ServiceContract(Namespace = "http://localhost:8082/SKDehuWs/services/DehuWs")]
    public interface IDehuWsNotificationes
    {
        [OperationContract(Action = "http://localhost:8082/SKDehuWs/services/DehuWs/localizaNotificaciones")]
        [WebInvoke(Method = "POST",
            UriTemplate = "/services/DehuWs/localizaNotificaciones",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string localizaNotificacionesSinAsignar(string peticioneNotificaciones);
    }
}
