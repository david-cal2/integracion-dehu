using System.ServiceModel;
using System.ServiceModel.Web;

namespace PSOENotificaciones.Interfaces
{
    [ServiceContract(Namespace = "http://localhost:8082/SKDehuWs/services/DehuWs")]
    public interface IPsoePeticionesAcceso
    {
        [OperationContract(Action =
                   "http://localhost:8082/SKDehuWs/services/DehuWs/comparecencia",
                   ReplyAction = "*"
                   )]
        [WebInvoke(Method = "POST",
                   UriTemplate = "/services/DehuWs/comparecencia",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.WrappedRequest
        )
        ]
        string comparecencia(string peticioneComparencia);

      
        [OperationContract(Action =
                  "http://localhost:8082/SKDehuWs/services/DehuWs/accesoComunicacion",
                  ReplyAction = "*"
        )]
        [WebInvoke(Method = "POST",
                  UriTemplate = "/services/DehuWs/accesoComunicacion",
                  RequestFormat = WebMessageFormat.Json,
                  ResponseFormat = WebMessageFormat.Json,
                  BodyStyle = WebMessageBodyStyle.WrappedRequest
        )
        ]
        string accesoComunicacion(string peticioneaccederComunicacion);
     

        [OperationContract(Action =
                  "http://localhost:8082/SKDehuWs/services/DehuWs/consultarAcuse",
                  ReplyAction = "*"
                  )]
        [WebInvoke(Method = "POST",
                  UriTemplate = "/services/DehuWs/consultarAcuse",
                  RequestFormat = WebMessageFormat.Json,
                  ResponseFormat = WebMessageFormat.Json,
                  BodyStyle = WebMessageBodyStyle.WrappedRequest
        )
        ]
        string consultaAcuse(string peticioneconsultaAcuse);


        [OperationContract(Action =
                  "http://localhost:8082/SKDehuWs/services/DehuWs/consultarAnexo",
                  ReplyAction = "*"
                  )]
        [WebInvoke(Method = "POST",
                  UriTemplate = "/services/DehuWs/consultarAnexo",
                  RequestFormat = WebMessageFormat.Json,
                  ResponseFormat = WebMessageFormat.Json,
                  BodyStyle = WebMessageBodyStyle.WrappedRequest
        )
        ]
        string consultaAnexo(string peticioneconsultaAnexo);
    }
}
