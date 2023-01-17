using System.ServiceModel;
using System.ServiceModel.Web;

namespace PSOENotificaciones.Interfaces
{
    [ServiceContract(Namespace = "http://localhost:8082/SKDehuWs/services/DehuWs")]
    public interface IDehuWsComunicaciones
    {
        [OperationContract(Action = "http://localhost:8082/SKDehuWs/services/DehuWs/localizaComunicaciones")]
        [WebInvoke(Method = "POST",
            UriTemplate = "/services/DehuWs/localizaComunicaciones",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string localizaComunicacionesSinAsignar(string peticioneComunicaciones);
    }
}
