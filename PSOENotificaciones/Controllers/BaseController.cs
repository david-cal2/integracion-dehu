using PSOENotificaciones.Servicios;
using PSOENotificaciones.Contexto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using PSOENotificaciones.Helpers;

namespace PSOENotificaciones.Controllers
{
    public class BaseController : Controller
    {
        [HttpGet]
        public ActionResult UpdateGrupos()
        {
            try
            {
                Envios env = new Envios();
                List<Envios> listaEnvios = env.GetEnviosTodosWinService();

                foreach (Envios e in listaEnvios)
                {
                    GruposEnvios ge = new GruposEnvios();
                    List<Grupos> listaGrupos = ge.GetGruposPorEnvio(e.Identificador);
                    List<string> listaGruposNombre = listaGrupos.Select(i => i.Nombre).ToList();
                    string cadena = "";
                    if (listaGruposNombre.Count > 0)
                        cadena = listaGruposNombre.Aggregate((i, j) => i + ", " + j);

                    env.SetGruposAsignadosCadena(e.Identificador, cadena);
                }
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = "True" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UpdateEmailAdministradores()
        {
            try
            {
                Usuarios usr = new Usuarios();
                List<Usuarios> listaUsuariosAdmin = usr.GetUsuariosPorPerfil((int)Perfil.Administrador);

                AvisosNotificacion an = new AvisosNotificacion();

                foreach (Usuarios u in listaUsuariosAdmin)
                {
                    an.InsertAvisoNotificacion(u.Nombre, u.Email, true, (int)TipoEmail.BloqueoUsuario);
                    an.InsertAvisoNotificacion(u.Nombre, u.Email, true, (int)TipoEmail.ProcesoCarga);
                    an.InsertAvisoNotificacion(u.Nombre, u.Email, true, (int)TipoEmail.ProcesoCargaError);
                    an.InsertAvisoNotificacion(u.Nombre, u.Email, true, (int)TipoEmail.AsignacionManual);
                    an.InsertAvisoNotificacion(u.Nombre, u.Email, true, (int)TipoEmail.EnAlerta);
                    an.InsertAvisoNotificacion(u.Nombre, u.Email, true, (int)TipoEmail.Caducadas);
                    an.InsertAvisoNotificacion(u.Nombre, u.Email, true, (int)TipoEmail.Externas);
                }
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = "True" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UpdateGruposOrganismos()
        {
            try
            {
                OrganismosEmisores org = new OrganismosEmisores();
                List<OrganismosEmisores> listaOrg = org.GetOrganismosEmisoresCadenaGruposVacia();

                foreach (OrganismosEmisores o in listaOrg)
                {
                    GruposOrganismosEmisores goe = new GruposOrganismosEmisores();
                    List<GruposOrganismosEmisores> listaGrupos = goe.GetGruposPorIdOrganismoEmisor(o.ID);

                    List<string> listaGruposNombre = listaGrupos.Select(i => i.Grupos.Nombre).ToList();
                    string cadena = "";
                    if (listaGruposNombre.Count > 0)
                        cadena = listaGruposNombre.Aggregate((i, j) => i + ", " + j);

                    org.SetGruposAsignadosCadena(o.ID, cadena);
                }
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = "True" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetEstados(int idTipoEnvios)
        {
            List<EstadosEnvios> listaEstados;
            try
            {
                EstadosEnvios ee = new EstadosEnvios();
                listaEstados = ee.GetEstadosEnvio(idTipoEnvios);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(listaEstados, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAsuntos(int idTipoEnvios, int? idEstado)
        {
            List<string> listaAsuntos;
            try
            {
                Envios en = new Envios();
                listaAsuntos = en.GetAsuntos(idTipoEnvios, idEstado);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(listaAsuntos, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetGrupoById(string idGrupo)
        {
            Grupos grupo;

            try
            {
                Grupos g = new Grupos();
                grupo = g.GetGrupoById(Convert.ToInt32(idGrupo));
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(grupo, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetGruposPorEnvio(string identificador)
        {
            List<Grupos> listaGrupos;
            try
            {
                GruposEnvios ge = new GruposEnvios();
                listaGrupos = ge.GetGruposPorEnvio(identificador);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(listaGrupos, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetRespuestaAcceso(string identificador)
        {
            RespuestasAcceso respuesta;

            try
            {
                RespuestasAcceso ra = new RespuestasAcceso();
                respuesta = ra.GetRespuestaAccesoPorIdentificador(identificador);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        public Usuarios GetSessionUsuario()
        {
            return (Session["usuario"] == null ? null : (Usuarios)Session["usuario"]);
        }

        public int GetSessionUsuarioID()
        {
            int id = 0;
            try
            {
                id = (Session["usuario"] == null ? 0 : Convert.ToInt32(((Usuarios)Session["usuario"]).ID));
            }
            catch (Exception ex)
            {
                PsoeHistorialesServicio servicio = new PsoeHistorialesServicio();
                servicio.InsertExcepcion(ex.Message, ex.StackTrace, 0);
            }

            return id;
        }

        private string GetIPAddress()
        {
            string IPAddress = "";
            string Hostname = System.Environment.MachineName;
            IPHostEntry Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = Convert.ToString(IP);
                }
            }
            return IPAddress;
        }

        public void ActualizaJsonOrganimosTarea()
        {
            OrganismosEmisores oe = new OrganismosEmisores();
            List<OrganismosEmisores> listaOrganismosEmisores = oe.GetOrganismosEmisoresTablaPosicion(true);

            //LargeJsonResult dataLarge = new LargeJsonResult { Data = listaOrganismosEmisores, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            string json = JsonConvert.SerializeObject(listaOrganismosEmisores);
            System.IO.File.WriteAllText(Server.MapPath("~") + @"json\organismos.json", json);
        }

        public void ActualizaJsonNotificacionesAdminComparecidas()
        {
            string[] listaIdEstado = { "4" };
            Usuarios usrSession = GetSessionUsuario();

            Envios en = new Envios();
            ResultadoEnvios listaNotif = en.GetEnviosPorEstado((int)TipoEnvio.Notificaciones, listaIdEstado, usrSession.ID, usrSession.Perfiles.ID, true);

            //LargeJsonResult dataLarge = new LargeJsonResult { Data = listaOrganismosEmisores, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            string json = JsonConvert.SerializeObject(listaNotif.ListaEnvios);
            System.IO.File.WriteAllText(Server.MapPath("~") + @"json\notificacionesAdminComp.json", json);
        }

        /**
         * Determina la llamada a cada uno de los métodos de JAVA
         **/
        private void DeterminarLLamadaJava(EjecuteClientAcceso ejecuteClientAcceso, TipoEjecucionJava tipoEjecucionJava,
            string identificador, AnexoReferencia anexoReferencia)
        {
            if (tipoEjecucionJava == TipoEjecucionJava.Comparecer)
            {
                ejecuteClientAcceso.LlamarServicioJava((int)TipoEjecucionJava.Comparecer, identificador);
            }
            else if (tipoEjecucionJava == TipoEjecucionJava.LeerComunicacion)
            {
                ejecuteClientAcceso.LlamarServicioJava((int)TipoEjecucionJava.LeerComunicacion, identificador);
            }
            else if (tipoEjecucionJava == TipoEjecucionJava.AccederComunicacion)
            {
                ejecuteClientAcceso.LlamarServicioJava((int)TipoEjecucionJava.AccederComunicacion, identificador);
            }
            else if (tipoEjecucionJava == TipoEjecucionJava.Acuse)
            {
                ejecuteClientAcceso.LlamarServicioJava((int)TipoEjecucionJava.Acuse, identificador);
            }
            else if (tipoEjecucionJava == TipoEjecucionJava.Anexos)
            {
                ejecuteClientAcceso.LlamarServicioJava((int)TipoEjecucionJava.Anexos, identificador, anexoReferencia);
            }
        }

        /**
         * Caso de error entre .NET con la comunicación de JAVA
         **/
        public bool ExisteErrorNetConJava(string RespuestaComparencia4, string identificador)
        {
            if (RespuestaComparencia4.Contains("CData") &&
                RespuestaComparencia4.Contains("Error") &&
                RespuestaComparencia4.Contains("cuerpo") &&
                RespuestaComparencia4.Contains("nivel") &&
                RespuestaComparencia4.Contains("XML"))
            {

                Envios envios = new Envios();
                GestNotifContext db = new GestNotifContext();
                envios.InsertErroresEnvioContext("Erro Connection Java", "Erro Connection Java", "Erro Connection Java", identificador, db);
                //dbContext = new GestNotifContext();
                //ErroresLlamadasDehu erroresLlamadasDehu = new ErroresLlamadasDehu()
                //{
                //    //Descripcion = (string)text4
                //    Descripcion = RespuestaComparencia4
                //}
                //;
                //erroresLlamadasDehu.InsertExcepcionesLlamadasDehu(erroresLlamadasDehu.Descripcion);
                //break;
                return true;
            }

            return false;
        }

        public string InsertRespuestaJava(EjecuteClientAcceso ejecuteClientAcceso, string identificador, int idPeticionAcceso,
            TipoEjecucionJava tipoEjecucionJava, AnexoReferencia anexoReferencia = null)
        {
            GestNotifContext dbContext = new GestNotifContext();
            string ErrorCodigo = "";

            try
            {
                LogLlamadasDehu log = new LogLlamadasDehu();
                log.InsertLogLlamadaDehuContext("Respuesta Java; tipoEjecucionJava: " + tipoEjecucionJava.ToString() + "; identificador:" + identificador + "; peticionAccesoID: " + idPeticionAcceso + ";", dbContext);

                List<XElement> NodeElementComparencia = new List<XElement>();
                int idRespuestasAcceso = 0;
                List<XElement> NodeElementComparenciaRoot = new List<XElement>();
                XmlDocument xmldocNodeHashDocument = new XmlDocument();

                DeterminarLLamadaJava(ejecuteClientAcceso, tipoEjecucionJava, identificador, anexoReferencia);
                string respuestaXml = EjecuteClientAcceso.respuesta;

                log.InsertLogLlamadaDehuContext("Respuesta xml: " + respuestaXml, dbContext);

                if (!String.IsNullOrEmpty(respuestaXml))
                {
                    XElement xElementComparencia4 = XElement.Parse((string)respuestaXml);

                    if (tipoEjecucionJava == TipoEjecucionJava.Comparecer || tipoEjecucionJava == TipoEjecucionJava.LeerComunicacion)
                    {
                        //Errores No Controlada para Dehu Malo format Jsop o Xml
                        //Error al leer el cuerpo: System.Xml.XmlException: Los elementos
                        //CData no son válidos en el nivel superior de un documento XML. Línea 1, posición 3. ...
                        if (respuestaXml != null)
                        {
                            if (ExisteErrorNetConJava(respuestaXml, identificador))
                            {
                                return null;
                            }
                            else
                            {
                                //Si no hay errores
                                XElement xElementComparencia44 = XElement.Parse((string)respuestaXml);

                                var ErroresDehuComparecnecia = new XmlDocument();
                                ErroresDehuComparecnecia.Load(xElementComparencia44.CreateReader());

                                var CountNodeElement = ErroresDehuComparecnecia.ChildNodes.Item(0).ChildNodes.Count;
                                int counterNodeError = 0;

                                if (CountNodeElement != 0)
                                {
                                    int[] counterNotErrores = new int[CountNodeElement - 1];
                                    for (int i = 0; i < CountNodeElement; i++)
                                    {
                                        if (ErroresDehuComparecnecia.ChildNodes.Item(0).ChildNodes.Item(i).Name == "error")
                                        {
                                            counterNodeError = i;
                                        }
                                        else
                                        {
                                            counterNotErrores[i] = i;
                                        }

                                    }

                                    //Errores controlados de DEHú - JAVA
                                    try
                                    {
                                        if (ErroresDehuComparecnecia.ChildNodes.Item(0).ChildNodes.Item(counterNodeError).InnerText != "")
                                        {
                                            #region Insert ErrorEnvio
                                            Envios enviosError = new Envios()
                                            {
                                                ErrorCodigo = ErroresDehuComparecnecia.ChildNodes.Item(0).ChildNodes.Item(counterNodeError).ChildNodes.Item(0).InnerText,
                                                ErrorDescripcion = ErroresDehuComparecnecia.ChildNodes.Item(0).ChildNodes.Item(counterNodeError).ChildNodes.Item(1).InnerText,
                                                ErrorMetodo = ErroresDehuComparecnecia.ChildNodes.Item(0).ChildNodes.Item(counterNodeError).ChildNodes.Item(2).InnerText,
                                            };

                                            log.InsertLogLlamadaDehuContext("ErrorCodigo: " + enviosError.ErrorCodigo + "; ErrorDescripcion: " + enviosError.ErrorDescripcion + "; ErrorMetodo: " + enviosError.ErrorMetodo, dbContext);

                                            if (enviosError.ErrorCodigo == "4209 No se puede acceder a una notificación ya comparecida")
                                            {
                                                Envios envios = new Envios();
                                                envios.SetEstadoEnvioContext(identificador, EstadoEnvio.Externas, 0, dbContext);
                                            }

                                            ErrorCodigo = enviosError.ErrorCodigo;

                                            enviosError.InsertErroresEnvioContext(enviosError.ErrorCodigo, enviosError.ErrorDescripcion,
                                                enviosError.ErrorMetodo, identificador, dbContext);
                                            #endregion

                                            bool ValideNotErro = false;

                                            for (int i = 0; i < CountNodeElement - 1; i++)
                                            {
                                                if (ErroresDehuComparecnecia.ChildNodes.Item(0).ChildNodes.Item(counterNotErrores[i]).InnerText != "")
                                                {
                                                    ValideNotErro = true;
                                                }
                                            }

                                            if (ValideNotErro == true)
                                            {
                                                xElementComparencia4 = XElement.Parse((string)respuestaXml);
                                            }
                                            else
                                            {
                                                log.InsertLogLlamadaDehuContext("ErrorCodigo: " + ErrorCodigo, dbContext);

                                                RespuestasAcceso ra1 = new RespuestasAcceso();
                                                idRespuestasAcceso = ra1.InsertRespuestaAccesoContext(identificador,
                                                    idPeticionAcceso, DateTime.Now, enviosError.ErrorCodigo.Substring(0, 4),
                                                    enviosError.ErrorDescripcion, 0, respuestaXml, dbContext);

                                                return ErrorCodigo;
                                            }
                                        }
                                        else
                                        {
                                            xElementComparencia4 = XElement.Parse((string)respuestaXml);
                                        }
                                    }
                                    catch (XmlException ex)
                                    {
                                        log.InsertLogLlamadaDehuContext("Excepción xml: " + ex.Message, dbContext);

                                        HistorialExcepciones he = new HistorialExcepciones();
                                        he.InsertExcepcionContext(ex.Message, ex.StackTrace, GetSessionUsuarioID(), dbContext);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.InsertLogLlamadaDehuContext("Excepción general: " + ex.Message, dbContext);

                                        HistorialExcepciones he = new HistorialExcepciones();
                                        he.InsertExcepcionContext(ex.Message, ex.StackTrace, GetSessionUsuarioID(), dbContext);
                                    }
                                }
                            }
                        }

                        var AllListComparenciaRoot = FirstXTransform.XElementTransforms(xElementComparencia4);
                        var ListAllComparenciaRoot = FirstXTransform.ReturnTypeListNode(AllListComparenciaRoot);

                        NodeElementComparenciaRoot = (List<XElement>)ListAllComparenciaRoot[ListAllComparenciaRoot.Length - 1];

                        var XElementNodeDocumentoError = NodeElementComparenciaRoot.Where(l => l.Name == "error").FirstOrDefault();

                        var AllListDocumentoError = FirstXTransform.XElementTransforms(XElementNodeDocumentoError);
                        var ListAllDocumentoError = FirstXTransform.ReturnTypeListNode(AllListDocumentoError);

                        var NodeElementDocumentoError = (List<XElement>)ListAllDocumentoError[ListAllComparenciaRoot.Length - 1];

                        var XElementNodeDocumentComparencia = NodeElementComparenciaRoot[0];

                        var AllListComparencia = FirstXTransform.XElementTransforms(XElementNodeDocumentComparencia);
                        var ListAllComparencia = FirstXTransform.ReturnTypeListNode(AllListComparencia);

                        NodeElementComparencia = (List<XElement>)ListAllComparencia[3];

                        var codigorespuesta = NodeElementComparencia[0];
                        var descriptionrespuesta = NodeElementComparencia[1];

                        var codigoOrigen2 = NodeElementComparencia[3];

                        //************************************************************************
                        //Comparecencia ChildNode
                        //Documento
                        var XElementNodeDocumentDocument = NodeElementComparencia[5];
                        var AllListDocument = FirstXTransform.XElementTransforms(XElementNodeDocumentDocument);
                        var ListAllDocument = FirstXTransform.ReturnTypeListNode(AllListDocument);

                        var NodeElementDocument = (List<XElement>)ListAllDocument[3];

                        //Nombre ==0
                        var XElementNodeNombre = NodeElementDocument[0];
                        var xmldocNodeNombre = new XmlDocument();
                        xmldocNodeNombre.Load(XElementNodeNombre.CreateReader());

                        //Contenido ==1
                        var XElementNodeContenido = NodeElementDocument[1];
                        var xmldocNodeContenido = new XmlDocument();
                        xmldocNodeContenido.Load(XElementNodeContenido.CreateReader());

                        //HashDocument ==2
                        var XElementNodeHashDocument = NodeElementDocument[2];
                        xmldocNodeHashDocument = new XmlDocument();
                        xmldocNodeHashDocument.Load(XElementNodeHashDocument.CreateReader());

                        //MIme Type ==3;
                        var XElementNodeMimeType = NodeElementDocument[3];
                        var xmldocNodeMimeType = new XmlDocument();
                        xmldocNodeMimeType.Load(XElementNodeMimeType.CreateReader());

                        //MetaDatosType ==4;
                        var XElementNodeMetaDatosType = NodeElementDocument[4];
                        var xmldocNodeMetaDatosType = new XmlDocument();
                        xmldocNodeMetaDatosType.Load(XElementNodeMetaDatosType.CreateReader());

                        //EnlaceDocument ==5;
                        var XElementNodeEnlaceDocument = NodeElementDocument[5];
                        var xmldocNodeEnlaceDocument = new XmlDocument();
                        xmldocNodeEnlaceDocument.Load(XElementNodeEnlaceDocument.CreateReader());

                        //ReferenciaDocumento ==6;
                        var XElementNodeReferenciaDocumento = NodeElementDocument[6];
                        var xmldocNodeReferenciaDocumento = new XmlDocument();
                        xmldocNodeReferenciaDocumento.Load(XElementNodeReferenciaDocumento.CreateReader());
                        var ReferenciaDocumentoByte = xmldocNodeReferenciaDocumento.InnerText;

                        //ReferenciaPdfAccuse ==7;
                        var XElementNodeReferenciaPdfAccuse = NodeElementDocument[7];
                        var xmldocNodeReferenciaPdfAccuse = new XmlDocument();
                        xmldocNodeReferenciaPdfAccuse.Load(XElementNodeReferenciaPdfAccuse.CreateReader());
                        var ReferenciaPdfAccuseByte = xmldocNodeReferenciaPdfAccuse.InnerText;

                        //CsvResguardo ==8 
                        var XElementNodeCsvResguardo = NodeElementDocument[8];
                        var xmldocNodeCsvResguardoe = new XmlDocument();
                        xmldocNodeCsvResguardoe.Load(XElementNodeCsvResguardo.CreateReader());

                        #region Insert RespuestaAcceso
                        //log.InsertLogLlamadaDehuContext("Insert RespuestaAcceso", dbContext);

                        RespuestasAcceso ra = new RespuestasAcceso();
                        idRespuestasAcceso = ra.InsertRespuestaAccesoContext(identificador,
                            idPeticionAcceso, DateTime.Now, codigorespuesta.Value, descriptionrespuesta.Value,
                            Convert.ToInt32(codigoOrigen2.Value), respuestaXml, dbContext);

                        ViewBag.IdRespuestaAcceso = idRespuestasAcceso;
                        #endregion

                        #region Insert DetalleDocumento y Contenido
                        //log.InsertLogLlamadaDehuContext("Insert DetalleDocumento", dbContext);

                        DetalleDocumentos dd = new DetalleDocumentos();
                        int idDetalleDocumento = dd.InsertDetalleDocumentoContext(dbContext,
                            idRespuestasAcceso, xmldocNodeNombre.InnerText, xmldocNodeMimeType.InnerText,
                            xmldocNodeMetaDatosType.InnerText, xmldocNodeEnlaceDocument.InnerText,
                            ReferenciaDocumentoByte, ReferenciaPdfAccuseByte, xmldocNodeCsvResguardoe.InnerText);

                        if (xmldocNodeContenido.ChildNodes.Item(0).OuterXml != null)
                        {
                            if (xmldocNodeContenido.ChildNodes.Item(0).ChildNodes.Item(0) != null)
                            {
                                var _contenidoValue = xmldocNodeContenido.ChildNodes.Item(0).ChildNodes.Item(0).InnerText;
                                var _contenidoHref = xmldocNodeContenido.ChildNodes.Item(0).ChildNodes.Item(1).InnerText;
                                Contenido con = new Contenido();
                                con.InsertContenido(_contenidoHref, _contenidoValue, idDetalleDocumento, dbContext);
                            }
                        }
                        #endregion

                        #region Insert HashDocumentos
                        //log.InsertLogLlamadaDehuContext("Insert HashDocumentos", dbContext);
                        LeerXml_HasDocumento(dbContext, xmldocNodeHashDocument, idDetalleDocumento);
                        #endregion
                    }

                    //Anexos Referencia
                    if (tipoEjecucionJava == TipoEjecucionJava.Comparecer || tipoEjecucionJava == TipoEjecucionJava.LeerComunicacion)
                    {
                        XmlDocument NodeNotificationDocumentoAnexo = new XmlDocument();
                        NodeNotificationDocumentoAnexo.Load(xElementComparencia4.CreateReader());

                        if (NodeNotificationDocumentoAnexo.ChildNodes.Item(0).ChildNodes.Item(0).ChildNodes != null)
                        {
                            if (NodeNotificationDocumentoAnexo.ChildNodes.Item(0).ChildNodes.Item(0).ChildNodes != null)
                            {
                                for (int k = 0; k < NodeNotificationDocumentoAnexo.ChildNodes.Item(0).ChildNodes.Item(0).ChildNodes.Count; k++)
                                {
                                    if (NodeNotificationDocumentoAnexo.ChildNodes.Item(0).ChildNodes.Item(0).ChildNodes.Item(k).InnerXml != "")
                                    {
                                        if (NodeNotificationDocumentoAnexo.ChildNodes.Item(0).ChildNodes.Item(0).ChildNodes.Item(k).Name == "anexos")
                                        {
                                            //Anexos Referencia
                                            var AnexosReferenciaXml = new XmlDocument();
                                            AnexosReferenciaXml.Load(XElement.Parse(NodeNotificationDocumentoAnexo.ChildNodes.Item(0).ChildNodes.Item(0).ChildNodes.Item(k).OuterXml).CreateReader());

                                            if (AnexosReferenciaXml.ChildNodes.Item(0).ChildNodes != null)
                                            {
                                                for (int ii = 0; ii < AnexosReferenciaXml.ChildNodes.Item(0).ChildNodes.Count; ii++)
                                                {
                                                    if (AnexosReferenciaXml.ChildNodes.Item(0).ChildNodes.Item(ii) != null)
                                                    {
                                                        if (AnexosReferenciaXml.ChildNodes.Item(0).ChildNodes.Item(ii).InnerXml != "")
                                                        {
                                                            if (AnexosReferenciaXml.ChildNodes.Item(0).ChildNodes.Item(ii).Name == "anexosReferencia")
                                                            {
                                                                if (AnexosReferenciaXml.ChildNodes.Item(0).ChildNodes.Item(ii).ChildNodes != null)
                                                                {
                                                                    for (int jj = 0; jj < AnexosReferenciaXml.ChildNodes.Item(0).ChildNodes.Item(ii).ChildNodes.Count; jj++)
                                                                    {
                                                                        if (AnexosReferenciaXml.ChildNodes.Item(0).ChildNodes.Item(ii).ChildNodes.Item(jj).InnerXml != "")
                                                                        {
                                                                            if (AnexosReferenciaXml.ChildNodes.Item(ii).ChildNodes.Count != 0)
                                                                            {
                                                                                var itemAnexoReferenciayUrl = XElement.Parse(AnexosReferenciaXml.ChildNodes.Item(ii).ChildNodes.Item(jj).OuterXml).Descendants();

                                                                                XmlDocument itemAnexoReferenciayUrlCountXMl = new XmlDocument();

                                                                                var anexoRefernciaItemParsing = AnexosReferenciaXml.ChildNodes.Item(ii).ChildNodes.Item(jj).OuterXml;
                                                                                itemAnexoReferenciayUrlCountXMl.LoadXml(anexoRefernciaItemParsing);

                                                                                if (jj == 0)
                                                                                {
                                                                                    if (itemAnexoReferenciayUrlCountXMl.ChildNodes.Item(0).ChildNodes != null)
                                                                                    {
                                                                                        for (int i = 0; i < itemAnexoReferenciayUrlCountXMl.ChildNodes.Item(0).ChildNodes.Count/*n*5*/; i++)
                                                                                        {
                                                                                            if (itemAnexoReferenciayUrlCountXMl.ChildNodes.Item(0).ChildNodes.Item(i).InnerXml != "")
                                                                                            {
                                                                                                string referenciaDocumento = itemAnexoReferenciayUrl.ToList().Where(L => L.Name == "referenciaDocumento").ElementAt(i).Value;
                                                                                                string nombre = itemAnexoReferenciayUrl.ToList().Where(L => L.Name == "nombre").ElementAt(i).Value;
                                                                                                string mimeType = itemAnexoReferenciayUrl.ToList().Where(L => L.Name == "mimeType").ElementAt(i).Value;

                                                                                                dbContext = new GestNotifContext();

                                                                                                AnexoReferencia ar = new AnexoReferencia();
                                                                                                int anexoReferenciaId = ar.InsertAnexoReferencia(idRespuestasAcceso, nombre, mimeType, referenciaDocumento, dbContext);
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                                break;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (tipoEjecucionJava == TipoEjecucionJava.Acuse || tipoEjecucionJava == TipoEjecucionJava.Comparecer || tipoEjecucionJava == TipoEjecucionJava.AccederComunicacion)
                    {
                        //log.InsertLogLlamadaDehuContext("Acuse", dbContext);

                        respuestaXml = EjecuteClientAcceso.respuesta;

                        #region Acuse
                        if (respuestaXml != null)
                        {
                            var XElementNodeAcuseComparencia = XElement.Parse((string)respuestaXml);
                            var xmldocCodigoRespuesta = new XmlDocument();
                            xmldocCodigoRespuesta.Load(XElementNodeAcuseComparencia.CreateReader());
                            XmlNode XmlNodeConsulta = null;
                            XmlNode XmlNodeError = null;
                            XmlNode XmlNodeAcuse = null;

                            foreach (var item in xmldocCodigoRespuesta.ChildNodes.Item(0))
                            {
                                var itemNode = (XmlNode)item;
                                if (itemNode.Name == "consulta")
                                {
                                    XmlNodeConsulta = itemNode;
                                }
                                if (itemNode.Name == "error")
                                {
                                    XmlNodeError = itemNode;
                                }
                                if (itemNode.Name == "acuse")
                                {
                                    XmlNodeAcuse = itemNode;
                                }

                            }
                            //Error
                            var xmldocError = new XmlDocument();

                            if (XmlNodeError != null)
                            {
                                XmlNode[] xmlNode = null;

                                int counterNOde = 0;
                                foreach (var item in xmldocError.ChildNodes)
                                {
                                    var itemNOde = (XmlNode)item;
                                    xmlNode[counterNOde] = itemNOde;
                                    counterNOde++;
                                }

                                for (int i = 0; i < xmldocError.ChildNodes.Count; i++)
                                {
                                    if (xmldocError.ChildNodes.Item(i).InnerText != "")
                                    {
                                        if (xmlNode[i].Name == "error")
                                        {
                                            if (xmlNode.Where(l => l.Name == "message").Any() &&
                                                xmlNode.Where(l => l.Name == "metodo").Any() &&
                                                xmlNode.Where(l => l.Name == "localizedMessage").Any())
                                            {
                                                var message = xmlNode.Where(l => l.Name == "message").FirstOrDefault();
                                                var metodo = xmlNode.Where(l => l.Name == "metodo").FirstOrDefault();
                                                var localizedMessage = xmlNode.Where(l => l.Name == "localizedMessage").FirstOrDefault();
                                                var error = dbContext.Envios.Where(l => l.Identificador == identificador).FirstOrDefault();
                                                error.InsertErroresEnvioContext(message.ToString(), localizedMessage.ToString(), message.ToString(), identificador, dbContext);
                                            }
                                        }
                                    }
                                }
                            }

                            if (XmlNodeConsulta != null)
                            {
                                LeerXml_AcusePdf(dbContext, XmlNodeConsulta, identificador);
                            }

                            if (XmlNodeAcuse != null)
                            {
                                LeerXml_AcusePdf(dbContext, XmlNodeAcuse, identificador);
                            }
                        }
                        #endregion
                    }

                    if (tipoEjecucionJava == TipoEjecucionJava.Anexos)
                    {
                        dbContext = new GestNotifContext();

                        var XmlNodeList = XElement.Parse(respuestaXml);

                        XmlDocument AnexosXml = new XmlDocument();
                        AnexosXml.Load(XmlNodeList.CreateReader());
                        XmlNodeList = XElement.Parse(respuestaXml);

                        AnexosXml = new XmlDocument();
                        AnexosXml.Load(XmlNodeList.CreateReader());

                        var CodigoOrigen_ = dbContext.Envios.Where(l => l.Identificador == identificador).FirstOrDefault().CodigoOrigen;
                        var usuariosId = GetSessionUsuarioID();

                        string nifReceptor = ConfigurationManager.AppSettings["nifReceptor"];
                        var CodigoOrigen__ = CodigoOrigen_.ToString();

                        if (AnexosXml != null)
                        {
                            if (AnexosXml.ChildNodes.Item(0) != null)
                            {
                                XmlNode error = null;
                                XmlNode XmlNodeConsulta = null;

                                foreach (var item in AnexosXml.ChildNodes.Item(0).ChildNodes)
                                {
                                    var itemXmlNode = (XmlNode)item;

                                    if (itemXmlNode.Name == "error")
                                    {
                                        error = itemXmlNode;
                                        if (error.ChildNodes != null)
                                        {
                                            XmlNode[] allInner = null;
                                            int counterInneXml = 0;
                                            foreach (var itemLastChild in error.ChildNodes)
                                            {
                                                var xx = (XmlNode)itemLastChild;
                                                allInner[counterInneXml] = xx;
                                                counterInneXml++;
                                            }

                                            for (int i = 0; i < error.ChildNodes.Count; i++)
                                            {
                                                if (error.ChildNodes.Item(i).InnerText != "")
                                                {
                                                    if (allInner[i] != null)
                                                    {
                                                        if (allInner.Where(l => l.Name == "message").Any()
                                                            &&
                                                            allInner.Where(l => l.Name == "metodo").Any()
                                                            &&
                                                            allInner.Where(l => l.Name == "localizedMessage").Any()
                                                            )
                                                        {
                                                            var message = allInner.Where(l => l.Name == "message").FirstOrDefault().ToString() != null ? allInner.Where(l => l.Name == "message").FirstOrDefault().ToString() : "";
                                                            var metodo = allInner.Where(l => l.Name == "metodo").FirstOrDefault().ToString() != null ? allInner.Where(l => l.Name == "metodo").FirstOrDefault().ToString() : ""; ;
                                                            var localizedMessage = allInner.Where(l => l.Name == "localizedMessage").FirstOrDefault().ToString() != null ? allInner.Where(l => l.Name == "localizedMessage").FirstOrDefault().ToString() : ""; ;

                                                            Envios envios3 = new Envios()
                                                            {
                                                                ErrorCodigo = message,
                                                                ErrorDescripcion = localizedMessage,
                                                                ErrorMetodo = metodo
                                                            };

                                                            envios3.InsertErroresEnvioContext(
                                                                envios3.ErrorCodigo,
                                                                envios3.ErrorDescripcion,
                                                                envios3.ErrorMetodo,
                                                                identificador,
                                                                dbContext
                                                                );
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (itemXmlNode.Name == "consulta")
                                    {
                                        //log.InsertLogLlamadaDehuContext("Anexos itemXmlNode consulta", dbContext);
                                        //log.InsertLogLlamadaDehuContext(anexoReferencia.ReferenciaDocumento, dbContext);

                                        DateTime fechaActual = DateTime.Now;

                                        var List4 = new List<Opcion4>();

                                        PeticionesAnexo pa = new PeticionesAnexo();
                                        int peticionAnexoId = pa.InsertPeticionesAnexo(CodigoOrigen_.ToString(),
                                            identificador, fechaActual, nifReceptor, String.Empty, List4,
                                            anexoReferencia.ReferenciaDocumento, usuariosId, dbContext);

                                        XmlNodeConsulta = itemXmlNode;
                                        var xmldoccodigoRespuesta = XmlNodeConsulta.ChildNodes.Item(0).InnerXml;

                                        //descripcionRespuesta
                                        var xmldocdescripcionRespuesta = XmlNodeConsulta.ChildNodes.Item(1).InnerXml;

                                        //acusePdf
                                        var xmldocacusePdf = XmlNodeConsulta.ChildNodes.Item(2).OuterXml;

                                        var xmldocAcuse = new XmlDocument();
                                        xmldocAcuse.LoadXml(xmldocacusePdf);
                                        //nombreAcuse
                                        var xmldocnombreAcuse = xmldocAcuse.ChildNodes.Item(0).ChildNodes.Item(0).InnerXml;
                                        //contenido
                                        var xmldoccontenido = xmldocAcuse.ChildNodes.Item(0).ChildNodes.Item(1).OuterXml;

                                        var xmlContenido = new XmlDocument();
                                        xmlContenido.LoadXml(xmldoccontenido);
                                        //href
                                        var xmldochref = xmlContenido.ChildNodes.Item(0).ChildNodes.Item(0).InnerXml;
                                        //value
                                        var xmldovalue = xmlContenido.ChildNodes.Item(0).ChildNodes.Item(1).InnerXml;

                                        //mimeType
                                        var xmldocmimeType = xmldocAcuse.ChildNodes.Item(0).ChildNodes.Item(2).InnerXml;
                                        // metadatos
                                        var xmldocmetadatos = xmldocAcuse.ChildNodes.Item(0).ChildNodes.Item(3).InnerXml;

                                        RespuestasAnexo respuestasAnexo1 = new RespuestasAnexo()
                                        {
                                            CodigoRespuesta = xmldoccodigoRespuesta,
                                            DescripcionRespuesta = xmldocdescripcionRespuesta,
                                            Fecha = fechaActual,
                                            NombreXml = xmldocnombreAcuse,
                                            PeticionesAnexo_ID = peticionAnexoId,
                                            OpcionesRespuestaConsultaAnexo = new List<Opcion5>()
                                        };

                                        int respuestasAnexo1ID = respuestasAnexo1.InsertRespuestasAnexo(
                                            respuestasAnexo1.CodigoRespuesta,
                                            respuestasAnexo1.DescripcionRespuesta,
                                            respuestasAnexo1.Fecha,
                                            peticionAnexoId.ToString(),
                                            respuestasAnexo1.NombreXml,
                                            respuestasAnexo1.OpcionesRespuestaConsultaAnexo,
                                            dbContext
                                            );

                                        DocumentosAnexo documentosAnexo = new DocumentosAnexo()
                                        {
                                            Metadatos = xmldocmetadatos,
                                            MimeType = xmldocmimeType,
                                            Nombre = xmldocnombreAcuse,
                                            RespuestasAnexo_ID = respuestasAnexo1ID
                                        };

                                        int documentosAnexoID = documentosAnexo.InsertDocumentoAnexo(
                                            documentosAnexo.Metadatos,
                                            documentosAnexo.MimeType,
                                            documentosAnexo.Nombre,
                                            documentosAnexo.RespuestasAnexo_ID,
                                            dbContext);

                                        Contenido1 contenido1 = new Contenido1()
                                        {
                                            Href = xmldochref,
                                            Value = xmldovalue,
                                            DocumentosAnexo_ID = documentosAnexoID

                                        };
                                        contenido1.InsertContenido1(
                                                contenido1.Href,
                                                contenido1.Value,
                                                contenido1.DocumentosAnexo_ID,
                                                dbContext
                                            );
                                    }

                                    //if (itemXmlNode.Name == "anexosReferencia")
                                    //{
                                    //    log.InsertLogLlamadaDehuContext("Anexos itemXmlNode anexosReferencia", dbContext);

                                    //    foreach (var itemXmlNodeReferencia in itemXmlNode)
                                    //    {
                                    //        itemXmlNode = (XmlNode)itemXmlNodeReferencia;
                                    //        if (itemXmlNode.ChildNodes != null)
                                    //        {
                                    //            foreach (var itemReferenciaAnexo in itemXmlNode.ChildNodes.Item(0).ChildNodes)
                                    //            {
                                    //                log.InsertLogLlamadaDehuContext(annexoReferenciaID_, dbContext);

                                    //                PeticionesAnexo peticionesAnexo = new PeticionesAnexo()
                                    //                {
                                    //                    CodigoOrigen = CodigoOrigen__,
                                    //                    Usuarios_ID = usuariosId,
                                    //                    Fecha = DateTime.Now,
                                    //                    NifReceptor = nifReceptor,
                                    //                    Referencia = annexoReferenciaID_,
                                    //                    NombreXml = nombreXml_,
                                    //                    Envios_Identificador = identificador
                                    //                };
                                    //                //peticionesAnexof = peticionesAnexo;
                                    //                var List4 = new List<Opcion4>();

                                    //                int peticionAnexoId = peticionesAnexo.InsertPeticionesAnexo(
                                    //                    peticionesAnexo.CodigoOrigen,
                                    //                    identificador,
                                    //                    peticionesAnexo.Fecha,
                                    //                    peticionesAnexo.NifReceptor,
                                    //                    peticionesAnexo.NombreXml,
                                    //                    List4,
                                    //                    peticionesAnexo.Referencia,
                                    //                    peticionesAnexo.Usuarios_ID,
                                    //                    dbContext
                                    //                    );
                                    //                XmlNodeConsulta = itemXmlNode;
                                    //                var xmldoccodigoRespuesta = XmlNodeConsulta.ChildNodes.Item(0).InnerXml;

                                    //                //descripcionRespuesta
                                    //                var xmldocdescripcionRespuesta = XmlNodeConsulta.ChildNodes.Item(1).InnerXml;

                                    //                //documento
                                    //                var xmldocacusePdf = XmlNodeConsulta.ChildNodes.Item(2).OuterXml;

                                    //                var xmldocAcuse = new XmlDocument();
                                    //                xmldocAcuse.LoadXml(xmldocacusePdf);
                                    //                //nombreAcuse
                                    //                var xmldocnombreAcuse = xmldocAcuse.ChildNodes.Item(0).ChildNodes.Item(0).InnerXml;
                                    //                //contenido
                                    //                var xmldoccontenido = xmldocAcuse.ChildNodes.Item(0).ChildNodes.Item(1).OuterXml;

                                    //                var xmlContenido = new XmlDocument();
                                    //                xmlContenido.LoadXml(xmldoccontenido);
                                    //                //href
                                    //                var xmldochref = xmlContenido.ChildNodes.Item(0).ChildNodes.Item(0).InnerXml;
                                    //                //value
                                    //                var xmldovalue = xmlContenido.ChildNodes.Item(0).ChildNodes.Item(1).InnerXml;

                                    //                //mimeType
                                    //                var xmldocmimeType = xmldocAcuse.ChildNodes.Item(0).ChildNodes.Item(2).InnerXml;
                                    //                // metadatos
                                    //                var xmldocmetadatos = xmldocAcuse.ChildNodes.Item(0).ChildNodes.Item(3).InnerXml;


                                    //                RespuestasAnexo respuestasAnexo1 = new RespuestasAnexo()
                                    //                {
                                    //                    CodigoRespuesta = xmldoccodigoRespuesta,
                                    //                    DescripcionRespuesta = xmldocdescripcionRespuesta,
                                    //                    Fecha = peticionesAnexo.Fecha,
                                    //                    NombreXml = xmldocnombreAcuse,
                                    //                    PeticionesAnexo_ID = peticionAnexoId,
                                    //                    OpcionesRespuestaConsultaAnexo = new List<Opcion5>()
                                    //                };

                                    //                int respuestasAnexo1ID = respuestasAnexo1.InsertRespuestasAnexo(
                                    //                    respuestasAnexo1.CodigoRespuesta,
                                    //                    respuestasAnexo1.DescripcionRespuesta,
                                    //                    respuestasAnexo1.Fecha,
                                    //                    respuestasAnexo1.PeticionesAnexo_ID.ToString(),
                                    //                    respuestasAnexo1.NombreXml,
                                    //                    respuestasAnexo1.OpcionesRespuestaConsultaAnexo,
                                    //                    dbContext
                                    //                    );

                                    //                DocumentosAnexo documentosAnexo = new DocumentosAnexo()
                                    //                {
                                    //                    Metadatos = xmldocmetadatos,
                                    //                    MimeType = xmldocmimeType,
                                    //                    Nombre = xmldocnombreAcuse,
                                    //                    RespuestasAnexo_ID = respuestasAnexo1ID
                                    //                }
                                    //                ;
                                    //                int documentosAnexoID = documentosAnexo.InsertDocumentoAnexo(
                                    //                                documentosAnexo.Metadatos,
                                    //                                documentosAnexo.MimeType,
                                    //                                documentosAnexo.Nombre,
                                    //                                documentosAnexo.RespuestasAnexo_ID,
                                    //                                dbContext
                                    //                            );

                                    //                Contenido1 contenido1 = new Contenido1()
                                    //                {
                                    //                    Href = xmldochref,
                                    //                    Value = xmldovalue,
                                    //                    DocumentosAnexo_ID = documentosAnexoID

                                    //                };
                                    //                contenido1.InsertContenido1(
                                    //                        contenido1.Href,
                                    //                        contenido1.Value,
                                    //                        contenido1.DocumentosAnexo_ID,
                                    //                        dbContext
                                    //                    );
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                }
                            }
                        }
                    }
                }

                dbContext.Dispose();
                return ErrorCodigo;
            }
            catch (Exception ex)
            {
                dbContext.Dispose();
                HistorialExcepciones he = new HistorialExcepciones();
                he.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return "Excepcion";
            }
        }

        private void LeerXml_AcusePdf(GestNotifContext dbContext, XmlNode XmlNodeConsulta, string identificador)
        {
            LogLlamadasDehu log = new LogLlamadasDehu();

            try
            {
                var xmldoccodigoRespuesta = XmlNodeConsulta.ChildNodes.Item(0).InnerXml;

                //descripcionRespuesta
                var xmldocdescripcionRespuesta = XmlNodeConsulta.ChildNodes.Item(1).InnerXml;

                //acusePdf
                var xmldocacusePdf = XmlNodeConsulta.ChildNodes.Item(2).OuterXml;

                var xmldocAcuse = new XmlDocument();
                xmldocAcuse.LoadXml(xmldocacusePdf);
                //nombreAcuse
                var xmldocnombreAcuse = xmldocAcuse.ChildNodes.Item(0).ChildNodes.Item(0).InnerXml;
                //contenido
                var xmldoccontenido = xmldocAcuse.ChildNodes.Item(0).ChildNodes.Item(1).OuterXml;

                var xmlContenido = new XmlDocument();
                xmlContenido.LoadXml(xmldoccontenido);
                //href
                var xmldochref = xmlContenido.ChildNodes.Item(0).ChildNodes.Item(0).InnerXml;
                //value
                var xmldovalue = xmlContenido.ChildNodes.Item(0).ChildNodes.Item(1).InnerXml;

                //mimeType
                var xmldocmimeType = xmldocAcuse.ChildNodes.Item(0).ChildNodes.Item(2).InnerXml;
                // metadatos
                var xmldocmetadatos = xmldocAcuse.ChildNodes.Item(0).ChildNodes.Item(3).InnerXml;

                Envios envios2 = new Envios();
                var identificador_ = envios2.GetEnvioDetalleContext(identificador, dbContext);

                #region Insert PeticionAcusePdf
                //log.InsertLogLlamadaDehuContext("Insert PeticionAcusePdf", dbContext);

                PeticionesAcusePdf pap = new PeticionesAcusePdf();
                int idPeticionAcusePdf = pap.InsertPeticionesAcusePdf(identificador_.CodigoOrigen,
                    identificador, DateTime.Now, ConfigurationManager.AppSettings["nifReceptor"],
                    String.Empty, GetSessionUsuarioID(), dbContext);
                #endregion

                #region Insert RespuestaAcusePdf
                //log.InsertLogLlamadaDehuContext("Insert RespuestaAcusePdf", dbContext);

                RespuestasAcusePdf rap = new RespuestasAcusePdf();
                int idRespuestaAcusePdf = rap.InsertRespuestasAcusePdf(DateTime.Now, idPeticionAcusePdf, xmldoccodigoRespuesta,
                    xmldocdescripcionRespuesta, String.Empty, dbContext);
                #endregion

                #region Insert AcusePdf
                //log.InsertLogLlamadaDehuContext("Insert AcusePdf", dbContext);

                AcusesPdf ap = new AcusesPdf();
                int idAcusePdf = ap.InsertAcusesPdf(idRespuestaAcusePdf, xmldocmetadatos,
                    xmldocmimeType, xmldocnombreAcuse, dbContext);
                #endregion

                #region Insert Contenido2 AcusePdf
                //log.InsertLogLlamadaDehuContext("Insert Contenido2 AcusePdf", dbContext);

                InsertContenidoAcusePDF(dbContext, xmlContenido, idAcusePdf);
                #endregion
            }
            catch (Exception ex)
            {
                HistorialExcepciones he = new HistorialExcepciones();
                he.InsertExcepcionContext(ex.Message, ex.StackTrace, GetSessionUsuarioID(), dbContext);
            }
        }

        private void LeerXml_HasDocumento(GestNotifContext dbContext, XmlDocument xmldocNodeHashDocument, int idDetalleDocumento)
        {
            try
            {
                //Tabla HashDocument 
                xmldocNodeHashDocument.ChildNodes.Item(0);
                xmldocNodeHashDocument.ChildNodes.Item(1);

                if (xmldocNodeHashDocument.ChildNodes.Item(0).ChildNodes.Item(0) != null)
                {
                    HashDocumento hashDocumento = new HashDocumento()
                    {
                        DetalleDocumentos_ID = idDetalleDocumento,
                        AlgoritmoHash = xmldocNodeHashDocument.ChildNodes.Item(0).ChildNodes.Item(1).InnerText,
                        Hash = xmldocNodeHashDocument.ChildNodes.Item(0).ChildNodes.Item(0).InnerText,
                    };

                    hashDocumento.InsertHashDocumento(hashDocumento.DetalleDocumentos_ID, hashDocumento.Hash, hashDocumento.AlgoritmoHash, dbContext);
                }
            }
            catch (Exception ex)
            {
                HistorialExcepciones he = new HistorialExcepciones();
                he.InsertExcepcionContext(ex.Message, ex.StackTrace, GetSessionUsuarioID(), dbContext);
            }
        }

        public void InsertContenidoAcusePDF(GestNotifContext dbContext, XmlDocument xmlContenido, int accusepdfId)
        {
            try
            {
                Contenido2 contenidoAcusePdf = new Contenido2();
                contenidoAcusePdf.InsertContenidoAcusePdf(xmlContenido.ChildNodes.Item(0).ChildNodes.Item(1).InnerXml, xmlContenido.ChildNodes.Item(0).ChildNodes.Item(0).InnerXml, accusepdfId, dbContext);
            }
            catch (Exception ex)
            {
                HistorialExcepciones he = new HistorialExcepciones();
                he.InsertExcepcionContext(ex.Message, ex.StackTrace, GetSessionUsuarioID(), dbContext);
            }
        }

        public void RecogerAnexos(string identificador)
        {
            try
            {
                EjecuteClientAcceso ejecuteClientAcceso = new EjecuteClientAcceso();

                PeticionesAcceso pa = new PeticionesAcceso();
                PeticionesAcceso peticionesAcceso = pa.GetPeticionAcceso(identificador);

                if (peticionesAcceso != null)
                {
                    RespuestasAcceso ra = new RespuestasAcceso();
                    RespuestasAcceso respuestaAcceso = ra.GetRespuestaAccesoPorIdPeticion(peticionesAcceso.ID);

                    if (respuestaAcceso != null)
                    {
                        AnexoReferencia ar = new AnexoReferencia();
                        List<AnexoReferencia> listaReferencias = ar.GetAnexosReferencia(respuestaAcceso.ID);

                        if (listaReferencias != null)
                        {
                            string errorCodigo = "";
                            foreach (AnexoReferencia anexoRef in listaReferencias)
                            {
                                //Llamada a Dehú solo de los anexos
                                errorCodigo += InsertRespuestaJava(ejecuteClientAcceso, identificador, peticionesAcceso.ID, TipoEjecucionJava.Anexos, anexoRef);
                            }

                            //Si no ha habido ningún error se acualzian los campos de error a vacío.
                            if (String.IsNullOrEmpty(errorCodigo))
                            {
                                Envios env = new Envios();
                                env.InsertErroresEnvio(String.Empty, String.Empty, String.Empty, identificador);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
            }
        }

        public string AsignarGrupoEnvioDesdeAdmin(string identificador, int idGrupo, bool agregar)
        {
            string cadena = "";
            int idUsuarioQueAsigna = GetSessionUsuarioID();

            Envios env = new Envios();
            GruposEnvios ge = new GruposEnvios();

            if (agregar)
            {
                ge.InsertGrupoEnvio(idGrupo, identificador, idUsuarioQueAsigna);
                EnviarCorreoAsignacionManual(identificador, idGrupo);
            }
            else
            {
                ge.DeleteGrupoEnvio(idGrupo, identificador);
            }

            //Se crea una cadena con los nombres de los grupos
            List<Grupos> listaGrupos = ge.GetGruposPorEnvio(identificador);
            List<string> listaGruposNombre = listaGrupos.Select(i => i.Nombre).ToList();
            //string cadena = "";
            if (listaGruposNombre.Count > 0)
                cadena = listaGruposNombre.Aggregate((i, j) => i + ", " + j);
            env.SetGruposAsignadosCadena(identificador, cadena);

            /* El estado solo se cambia si el actual es 'Sin asignar'.
             * El asignador puede asignar notificaciones que esten en alerta, comparecidas, caducadas o externas
             */
            int idEstado = env.GetEnvioEstado(identificador);
            if (cadena != "" && idEstado == (int)EstadoEnvio.SinAsignar)
            {
                env.SetEstadoEnvio(identificador, EstadoEnvio.Asignada, idUsuarioQueAsigna);
            }

            if (cadena == "" && idEstado == (int)EstadoEnvio.Asignada)
            {
                env.SetEstadoEnvio(identificador, EstadoEnvio.SinAsignar, idUsuarioQueAsigna);
            }

            return cadena;
        }

        public string AsignarGrupoEnvioManual(string identificador, string idGrupos)
        {
            string cadena = "";
            int idUsuarioQueAsigna = GetSessionUsuarioID();

            Envios env = new Envios();
            GruposEnvios ge = new GruposEnvios();

            List<int> listaIdsGrupoBBDD = ge.GetGruposPorEnvio(identificador).Select(i => i.ID).ToList();

            List<int> listaIdsGrupo = new List<int>();

            if (idGrupos != "")
            {
                string[] arrayIdsGrupo = idGrupos.TrimEnd(',').Split(',');

                foreach (string id in arrayIdsGrupo)
                {
                    int idGrupo = Convert.ToInt32(id);
                    listaIdsGrupo.Add(idGrupo);
                }
            }

            foreach (int idGrupo in listaIdsGrupoBBDD)
            {
                if (!listaIdsGrupo.Contains(idGrupo))
                {
                    //Eliminar registro
                    ge.DeleteGrupoEnvio(idGrupo, identificador);
                }
            }

            foreach (int idGrupo in listaIdsGrupo)
            {
                if (!listaIdsGrupoBBDD.Contains(idGrupo))
                {
                    //Se inserta el registro
                    ge.InsertGrupoEnvio(idGrupo, identificador, idUsuarioQueAsigna);
                    EnviarCorreoAsignacionManual(identificador, idGrupo);
                }
            }

            //Se crea una cadena con los nombres de los grupos
            List<Grupos> listaGrupos = ge.GetGruposPorEnvio(identificador);
            List<string> listaGruposNombre = listaGrupos.Select(i => i.Nombre).ToList();
            //string cadena = "";
            if (listaGruposNombre.Count > 0)
                cadena = listaGruposNombre.Aggregate((i, j) => i + ", " + j);
            env.SetGruposAsignadosCadena(identificador, cadena);

            /* El estado solo se cambia si el actual es 'Sin asignar'.
             * El asignador puede asignar notificaciones que esten en alerta, comparecidas, caducadas o externas
             */
            int idEstado = env.GetEnvioEstado(identificador);
            if (cadena != "" && idEstado == (int)EstadoEnvio.SinAsignar)
            {
                env.SetEstadoEnvio(identificador, EstadoEnvio.Asignada, idUsuarioQueAsigna);
            }

            if (cadena == "" && idEstado == (int)EstadoEnvio.Asignada)
            {
                env.SetEstadoEnvio(identificador, EstadoEnvio.SinAsignar, idUsuarioQueAsigna);
            }

            return cadena;
        }

        public void EnviarCorreoAsignacionManual(string identificador, int idGrupo)
        {
            try
            {
                Grupos gr = new Grupos();
                Grupos grupo = gr.GetGrupoById(idGrupo);

                Envios en = new Envios();
                Envios envio = en.GetEnvioDetalle(identificador);

                string asunto = "Gestión de notificaciones: Asignación manual de notificaciones y comunicaciones";
                string body = "";

                using (StreamReader reader = new StreamReader(Server.MapPath("~/Plantillas/email_AsignacionManual.html")))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{grupo}", grupo.Nombre);
                body = body.Replace("{fecha}", DateTime.Now.ToString("dd/MM/yyyy"));
                body = body.Replace("{identificador}", envio.Identificador);
                body = body.Replace("{concepto}", envio.Concepto);
                body = body.Replace("{organismoEmisor}", envio.OrganismosEmisores.OrganismoEmisor);

                GruposUsuarios usr = new GruposUsuarios();
                List<GruposUsuarios> listaGruposUsarios = usr.GetUsuarioPorGrupo(idGrupo);
                foreach (GruposUsuarios gu in listaGruposUsarios)
                {
                    Utils.SendMail(gu.Usuarios.Email, asunto, body);
                }
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
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

        public class LargeJsonResult : JsonResult
        {
            const string JsonRequest_GetNotAllowed = "This request has been blocked because sensitive information could be disclosed to third party web sites when this is used in a GET request. To allow GET requests, set JsonRequestBehavior to AllowGet.";
            public LargeJsonResult()
            {
                MaxJsonLength = 10024000;
                RecursionLimit = 10000;
            }

            public new int MaxJsonLength { get; set; }
            public new int RecursionLimit { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                if (context == null)
                {
                    throw new ArgumentNullException("context");
                }
                if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                    String.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException(JsonRequest_GetNotAllowed);
                }

                HttpResponseBase response = context.HttpContext.Response;

                if (!String.IsNullOrEmpty(ContentType))
                {
                    response.ContentType = ContentType;
                }
                else
                {
                    response.ContentType = "application/json";
                }
                if (ContentEncoding != null)
                {
                    response.ContentEncoding = ContentEncoding;
                }
                if (Data != null)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer() { MaxJsonLength = MaxJsonLength, RecursionLimit = RecursionLimit };
                    response.Write(serializer.Serialize(Data));
                }
            }
        }
    }
}