using PSOENotificaciones.Contexto;
using PSOENotificaciones.Helpers;
using PSOENotificaciones.Servicios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace PSOENotificaciones.Controllers
{
    public class TareasController : BaseController
    {
        #region Contadores 
        public static int totalNotificacionesProcesadas = 0;
        public static int totalNotificacionesAsignadas = 0;
        public static int totalNotificacionesPendientes = 0;
        public static int totalComunicacionesProcesadas = 0;
        public static int totalComunicacionesAsignadas = 0;
        public static int totalComunicacionesPendientes = 0;
        public static int totalEnviosExternas = 0;
        public static List<KeyValuePair<int, List<Envios>>> listaGruposEnviosAsignados = new List<KeyValuePair<int, List<Envios>>>();
        public static List<KeyValuePair<int, List<Envios>>> listaGruposEnviosExternas = new List<KeyValuePair<int, List<Envios>>>();

        private static void LimpiarContadores()
        {
            totalNotificacionesProcesadas = 0;
            totalNotificacionesAsignadas = 0;
            totalNotificacionesPendientes = 0;
            totalComunicacionesProcesadas = 0;
            totalComunicacionesAsignadas = 0;
            totalComunicacionesPendientes = 0;
            totalEnviosExternas = 0;
            listaGruposEnviosAsignados = new List<KeyValuePair<int, List<Envios>>>();
            listaGruposEnviosExternas = new List<KeyValuePair<int, List<Envios>>>();
        }
        #endregion

        #region Tarea programada Localiza Envíos
        [HttpGet]
        public ActionResult EjecutarTareaLocalizaManual(int idTipoEnvio, string fechaDesde, string fechaHasta)
        {
            int idUsuario = GetSessionUsuarioID();

            try
            {
                LogTareaLocaliza log = new LogTareaLocaliza();
                log.InsertLog("INICIO 'Proceso Localiza' - MANUAL");

                GestNotifContext dbContext = new GestNotifContext();

                LimpiarContadores();   

                if (idTipoEnvio == (int)TipoPeticionesLocaliza.Comunicaciones)
                {
                    log.InsertLog("Comunicaciones");
                    LlamadaPeticionLocaliza((int)TipoCarga.Manual, fechaDesde, fechaHasta, (int)TipoEnvio.Comunicaciones, idUsuario);
                }

                if (idTipoEnvio == (int)TipoPeticionesLocaliza.Notificaciones)
                {
                    log.InsertLog("Notificaciones");
                    LlamadaPeticionLocaliza((int)TipoCarga.Manual, fechaDesde, fechaHasta, (int)TipoEnvio.Notificaciones, idUsuario);
                }

                if (idTipoEnvio == (int)TipoPeticionesLocaliza.Todas)
                {
                    log.InsertLog("Comunicaciones");
                    LlamadaPeticionLocaliza((int)TipoCarga.Manual, fechaDesde, fechaHasta, (int)TipoEnvio.Comunicaciones, idUsuario);
                    log.InsertLog("Notificaciones");
                    LlamadaPeticionLocaliza((int)TipoCarga.Manual, fechaDesde, fechaHasta, (int)TipoEnvio.Notificaciones, idUsuario);
                }

                dbContext.Dispose();

                log.InsertLog("FIN 'Proceso Localiza' - MANUAL");

                return Json(new { Success = "True" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, idUsuario);
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult EjecutarTareaLocalizaManualEnvioEmails()
        {
            try
            {
                EnviarCorreoAsignacionAdmin((int)TipoCarga.Manual);
                EnviarCorreoAsignacionGrupos();

                return Json(new { Success = "True" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public static void EjecutarTareaLocalizaAutomatico()
        {
            try
            {
                LogTareaLocaliza log = new LogTareaLocaliza();
                log.InsertLog("INICIO 'Proceso Localiza' - AUTOMÁTICO");

                LimpiarContadores();

                string fechaDesde = null;
                string fechaHasta = null;
                int idUsuario = 0;

                LlamadaPeticionLocaliza((int)TipoCarga.Automatica, fechaDesde, fechaHasta, (int)TipoEnvio.Comunicaciones, idUsuario);
                LlamadaPeticionLocaliza((int)TipoCarga.Automatica, fechaDesde, fechaHasta, (int)TipoEnvio.Notificaciones, idUsuario);

                log.InsertLog("EnviarCorreoAsignacionAdmin");
                EnviarCorreoAsignacionAdmin((int)TipoCarga.Automatica);
                EnviarCorreoAsignacionGrupos();

                if (listaGruposEnviosExternas.Count > 0)
                {
                    EnviarCorreoExternasAdmin();
                    EnviarCorreoExternasGrupos();
                }

                log.InsertLog("FIN 'Proceso Localiza' - AUTOMÁTICO");
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.InnerException.Message, ex.InnerException.StackTrace, 0);
                EnviarCorreoCargaAutomaticaError();
            }
        }

        public static void LlamadaPeticionLocaliza(int tipoLlamada, string fechaDesde, string fechaHasta, int idTipoEnvioPeticion, int idUsuario)
        {
            //try
            //{
                LogTareaLocaliza log = new LogTareaLocaliza();
                GestNotifContext dbContext = new GestNotifContext();

                #region Insert PeticionLocaliza
                DateTime fechaActual = DateTime.Now;
                DateTime? fechaDesdeDT = null;
                DateTime? fechaHastaDT = null;
                string nifItutlar = ConfigurationManager.AppSettings["nifTitular"];
                string NifDestinatario = ConfigurationManager.AppSettings["nifDestinatario"];

                try
                {
                    if (!String.IsNullOrEmpty(fechaDesde))
                        fechaDesdeDT = DateTime.Parse(fechaDesde);

                    if (!String.IsNullOrEmpty(fechaHasta))
                        fechaHastaDT = DateTime.Parse(fechaHasta);
                }
                catch (Exception ex)
                {
                    HistorialExcepciones h = new HistorialExcepciones();
                    h.InsertExcepcion("Fecha desde: " + fechaDesde + "; Fecha hasta: " + fechaHasta + "; " + ex.Message, ex.StackTrace, 0);
                }

                PeticionesLocaliza pl = new PeticionesLocaliza();
                int idPeticionLocaliza = pl.InsertPeticionLocalizaContext(fechaActual, idTipoEnvioPeticion, idUsuario,
                    nifItutlar, NifDestinatario, String.Empty, fechaDesdeDT, fechaHastaDT, dbContext);
                #endregion

                PsoePeticionesLocalizaServicio psoePeticionesLocalizaServicio = new PsoePeticionesLocalizaServicio();
                string respuestaXml = psoePeticionesLocalizaServicio.EnviarPeticion(fechaDesdeDT, fechaHastaDT, idTipoEnvioPeticion, idPeticionLocaliza);

                XElement xElementRespuesta = XElement.Parse(respuestaXml);
                List<object> lista = FirstXTransform.XElementTransforms(xElementRespuesta);

                #region Insert RespuestaLocaliza
                var _type = FirstXTransform.ReturnTypeListNode(lista);
                var listName = (List<XName>)_type[0];
                var listAttribute = (List<XAttribute>)_type[1];
                var listInnerXml = (List<string>)_type[2];
                var NodeElement = (List<XElement>)_type[3];

                var XElementNodeEnvio = NodeElement.Where(l => l.Name.LocalName == "localiza").FirstOrDefault();
                var ListEnvios = FirstXTransform.XElementTransforms(XElementNodeEnvio);

                var NodeElementEnvios = (List<XElement>)_type[3];
                var XElementNodeItem = NodeElementEnvios.Where(l => l.Name.LocalName == "localiza").FirstOrDefault();

                //***************************************************
                var ListItem = FirstXTransform.XElementTransforms(XElementNodeItem);
                var typeItem = FirstXTransform.ReturnTypeListNode(ListItem);

                var listInnerXmlItem = (List<string>)typeItem[2];
                var NodeElementItem = (List<XElement>)typeItem[3];

                RespuestasLocaliza rl = new RespuestasLocaliza();

                DateTime fechaRespuesta = DateTime.Now;
                string codigoRespuesta = listInnerXmlItem[0];
                string descripcionRespuesta = listInnerXmlItem[1];
                string nifPeticion = listInnerXmlItem[2];
                string sample = listInnerXmlItem[4];
                Boolean.TryParse(sample, out bool myBool);
                bool hayMasResultados = myBool;

                int idRespuesta = rl.InsertRespuestaLocalizaContext(fechaRespuesta, idPeticionLocaliza, codigoRespuesta,
                    descripcionRespuesta, nifPeticion, hayMasResultados, respuestaXml, dbContext);
                #endregion

                var ListItemEnvios = FirstXTransform.XElementTransforms(XElementNodeItem);
                var typeItemEnvios = FirstXTransform.ReturnTypeListNode(ListItemEnvios);

                var NodeElementItemEnvios = (List<XElement>)typeItemEnvios[3];
                var NodeEnvios = NodeElementItemEnvios[3];

                var xmldocNodeEnvio = new XmlDocument();
                xmldocNodeEnvio.Load(NodeEnvios.CreateReader());

                List<Envios> ListEnvioVerification = new List<Envios>();

                foreach (var item in NodeElementItemEnvios)
                {
                    if (item.Name == "envios" && item.FirstNode != null)
                    {
                        var XElementNodeChildItem = item;

                        var ListChildItem = FirstXTransform.XElementTransforms(XElementNodeChildItem);
                        var typeChildItem = FirstXTransform.ReturnTypeListNode(ListChildItem);

                        var listNameChildItem = (List<XName>)typeChildItem[0];
                        var listAttributeChildItem = (List<XAttribute>)typeChildItem[1];
                        var listInnerXmlChildItem = (List<string>)typeChildItem[2];
                        var NodeElementChildItem = (List<XElement>)typeChildItem[3];

                        foreach (var itemChild in NodeElementChildItem)
                        {
                            #region Leer Xml
                            var ListOrganismEmisores = FirstXTransform.XElementTransforms(itemChild);
                            var typeOrganismEmisores = FirstXTransform.ReturnTypeListNode(ListOrganismEmisores);

                            //Recoger Item individual 
                            var listNameItemIndividual = (List<XName>)typeOrganismEmisores[0];
                            var listAttributeItemIndividual = (List<XAttribute>)typeOrganismEmisores[1];
                            var listInnerXmlItemIndividual = (List<string>)typeOrganismEmisores[2];
                            var NodeElementItemIndividual = (List<XElement>)typeOrganismEmisores[3];

                            //Identificator
                            var NodeElementIdentificador = NodeElementItemIndividual[0];

                            //Organismo Emisor
                            var XElementNodeOrganismEmisores = NodeElementItemIndividual[4];

                            var ListOrganismEmisores2 = FirstXTransform.XElementTransforms(XElementNodeOrganismEmisores);
                            var typeOrganismEmisores2 = FirstXTransform.ReturnTypeListNode(ListOrganismEmisores2);

                            var listNameOrganismEmisores = (List<XName>)typeOrganismEmisores2[0];
                            var listAttributeOrganismEmisores = (List<XAttribute>)typeOrganismEmisores2[1];
                            var listInnerXmlOrganismEmisores = (List<string>)typeOrganismEmisores2[2];
                            var NodeElementOrganismEmisores = (List<XElement>)typeOrganismEmisores2[3];

                            //Organismo Emisor Raiz 
                            var XElementNodeEmisoreRaiz = NodeElementItemIndividual[5];

                            var ListEmisoreRaiz = FirstXTransform.XElementTransforms(XElementNodeEmisoreRaiz);
                            var typeEmisoreRaiz = FirstXTransform.ReturnTypeListNode(ListEmisoreRaiz);

                            var listNameEmisoreRaiz = (List<XName>)typeEmisoreRaiz[0];
                            var listAttributeEmisoreRaiz = (List<XAttribute>)typeEmisoreRaiz[1];
                            var listInnerXmlEmisoreRaiz = (List<string>)typeEmisoreRaiz[2];
                            var NodeElementEmisoreRaiz = (List<XElement>)typeEmisoreRaiz[3];

                            //Titular 
                            var XElementNodetitular = NodeElementItemIndividual[9];//10

                            var Listtitular = FirstXTransform.XElementTransforms(XElementNodetitular);
                            var typetitular = FirstXTransform.ReturnTypeListNode(Listtitular);

                            var listNametitular = (List<XName>)typetitular[0];
                            var listAttributetitular = (List<XAttribute>)typetitular[1];
                            var listInnerXmltitular = (List<string>)typetitular[2];
                            var NodeElementtitular = (List<XElement>)typetitular[3];

                            Personas pe = new Personas();

                            string nombreTitular = listInnerXmltitular[0];
                            string nifTitularPersona = listInnerXmltitular[1];
                            string codigoDIR3 = NodeElementOrganismEmisores[0].Value;
                            string codigoDIRe = NodeElementEmisoreRaiz[0].Value;
                            string descripcionEntidad = listInnerXmltitular[4];
                            #endregion

                            var CompPersona = dbContext.Persona.Where(l => l.ID != pe.ID);

                            int idPersona = 0;
                            if (CompPersona != null)
                            {
                                dbContext = new GestNotifContext();
                                idPersona = pe.InsertPersonaContext(nombreTitular, nifTitularPersona, codigoDIR3, codigoDIRe,
                                    descripcionEntidad, dbContext);
                            }

                            dbContext.Dispose();
                            dbContext = new GestNotifContext();

                            OrganismosEmisores or = new OrganismosEmisores();

                            int idOrganismoEmisor = or.GetOrganismoEmisorContext(codigoDIR3, dbContext).ID;
                            int idOrganismoEmisorRaiz = idOrganismoEmisor;
                            dbContext.SaveChanges();

                            int idEstadoEnvio = (int)EstadoEnvio.SinAsignar;

                            string identificador = NodeElementIdentificador.Value;
                            int codigoOrigen = Convert.ToInt32(NodeElementItemIndividual[1].Value);
                            string concepto = NodeElementItemIndividual[2].Value;
                            string descripcionEnvio = NodeElementItemIndividual[3].Value;

                            var fechaPuestaDisposicion_ = NodeElementItemIndividual[6].Value;
                            string[] fechaArray = fechaPuestaDisposicion_.ToString().Split('T')[0].Split('-');
                            DateTime fechaPuestaDisposicion = new DateTime(Convert.ToInt32(fechaArray[0]), Convert.ToInt32(fechaArray[1]), Convert.ToInt32(fechaArray[2]));

                            string metadatosPublicos = NodeElementItemIndividual[10].Value;
                            int idVinculo = Convert.ToInt32(NodeElementItemIndividual[8].Value);
                            int idTipoEnvio = Convert.ToInt32(NodeElementItemIndividual[7].Value);

                            Envios en = new Envios
                            {
                                RepuestasLocaliza = rl,
                                Identificador = identificador
                            };
                            ListEnvioVerification.Add(en);

                            try
                            {
                                var CompENvio = dbContext.Envios.All(l => l.Identificador != identificador);

                                if (CompENvio && idTipoEnvio != 0 && idVinculo != 0 && idPersona != 0
                                    && idOrganismoEmisor != 0 && idRespuesta != 0
                                    && fechaPuestaDisposicion != null && concepto != "" && codigoOrigen != 0)
                                {
                                    dbContext.Dispose();
                                    dbContext = new GestNotifContext();

                                    try
                                    {
                                        en.InsertEnvioContext(identificador, codigoOrigen, concepto, descripcionEnvio,
                                            fechaPuestaDisposicion, metadatosPublicos, idVinculo, idTipoEnvio, idOrganismoEmisor,
                                            idPersona, idRespuesta, idEstadoEnvio, dbContext);
                                    }
                                    catch (Exception ex)
                                    {
                                        HistorialExcepciones his = new HistorialExcepciones();
                                        his.InsertExcepcionContext(ex.Message, ex.StackTrace, 0, dbContext);
                                    }

                                    AsignarGruposUsuarios(dbContext, identificador, idOrganismoEmisor, idTipoEnvio);
                                }
                            }
                            catch (UpdateException e)
                            {
                                Console.WriteLine(e.InnerException);
                            }
                        }
                    }
                }

                //Para las notificaciones en carga automática comprobamos si hay notifificaiones externas
                if (idTipoEnvioPeticion == (int)TipoEnvio.Notificaciones && tipoLlamada == (int)TipoCarga.Automatica)
                {
                    //log.InsertLogContext("Llamada a la función 'ExternaNotificationesPendientes'", dbContext);
                    ComprobarNotificacionesExternas(dbContext, ListEnvioVerification);
                }

                dbContext.SaveChanges();
                dbContext.Dispose();
            //}
            //catch (Exception ex)
            //{
            //    LogTareaLocaliza log = new LogTareaLocaliza();
            //    log.InsertLog("Se ha producido una excepción: " + ex.InnerException.Message);
            //    HistorialExcepciones his = new HistorialExcepciones();
            //    his.InsertExcepcion(ex.InnerException.Message, ex.InnerException.StackTrace, 0);
            //}
        }

        private static void AsignarGruposUsuarios(GestNotifContext dbContext, string identificador, int idOrganismoEmisor, int idTipoEnvio)
        {
            Envios env = new Envios();
            GruposEnvios ge = new GruposEnvios();

            //Se obtienen los grupos a los que pertenece el organismo.
            List<GruposOrganismosEmisores> gruposOrganismosEmisores = dbContext.GruposOrganismosEmisores
                .Include("OrganismosEmisores").Include("Grupos")
                .Where(p => p.OrganismosEmisores.ID == idOrganismoEmisor)
                .ToList();

            foreach (GruposOrganismosEmisores item in gruposOrganismosEmisores)
            {
                Envios envioAsignado = dbContext.Envios.Where(i => i.Identificador == identificador).FirstOrDefault();
                //GruposEnvios gruposEnvios = new GruposEnvios
                //{
                //    Grupos = item.Grupos,
                //    Envios = envioAsignado,
                //    Fecha = DateTime.Now
                //};

                //dbContext.GruposEnvios.Add(gruposEnvios);
                //dbContext.SaveChanges();

                ge.InsertGrupoEnvioAutomaticoContext(envioAsignado, item.Grupos, dbContext);

                List<KeyValuePair<int, List<Envios>>> lista = listaGruposEnviosAsignados.Where(i => i.Key == item.Grupos.ID).ToList();
                if (lista.Count == 1)
                {
                    lista[0].Value.Add(envioAsignado);
                    lista[0] = new KeyValuePair<int, List<Envios>>(item.Grupos.ID, lista[0].Value);
                }
                else
                {
                    List<Envios> nuevaLista = new List<Envios>
                    {
                        envioAsignado
                    };

                    KeyValuePair<int, List<Envios>> nuevoGrupo = new KeyValuePair<int, List<Envios>>(item.Grupos.ID, nuevaLista);
                    listaGruposEnviosAsignados.Add(nuevoGrupo);
                }
            }

            if (idTipoEnvio == (int)TipoEnvio.Notificaciones)
            {
                totalNotificacionesProcesadas += 1;
                if (gruposOrganismosEmisores.Count > 0)
                {
                    totalNotificacionesAsignadas += 1;
                    env.SetEstadoEnvioContext(identificador, EstadoEnvio.Asignada, 0, dbContext);
                }
                else
                    totalNotificacionesPendientes += 1;
            }

            if (idTipoEnvio == (int)TipoEnvio.Comunicaciones)
            {
                totalComunicacionesProcesadas += 1;
                if (gruposOrganismosEmisores.Count > 0)
                {
                    totalComunicacionesAsignadas += 1;
                    env.SetEstadoEnvioContext(identificador, EstadoEnvio.Asignada, 0, dbContext);
                }
                else
                    totalComunicacionesPendientes += 1;
            }

            /* Se guardan los nombres de los grupos en una cadena separados por coma */
            List<string> listaGrupos = ge.GetGruposPorEnvioContexto(identificador, dbContext);
            string cadena = "";
            if (listaGrupos.Count > 0)
                cadena = listaGrupos.Aggregate((i, j) => i + ", " + j);

            env.SetGruposAsignadosCadenaContext(identificador, cadena, dbContext);
        }

        private static void ComprobarNotificacionesExternas(GestNotifContext dbContext, List<Envios> enviosNuevos)
        {
            List<string> enviosYaExistentes = dbContext.Envios
                .Where(l => (l.EstadosEnvio.ID == (int)EstadoEnvio.SinAsignar
                        || l.EstadosEnvio.ID == (int)EstadoEnvio.EnAlerta
                        || l.EstadosEnvio.ID == (int)EstadoEnvio.Asignada)
                    && l.TiposEnvio.ID == (int)TipoEnvio.Notificaciones)
                .Select(l => l.Identificador).ToList();

            List<string> enviosDehu = enviosNuevos.Select(l => l.Identificador).ToList();

            foreach (string identificador in enviosYaExistentes)
            {
                if (!enviosDehu.Contains(identificador))
                {
                    Envios env = new Envios();
                    env.SetEstadoEnvioContext(identificador, EstadoEnvio.Externas, 0, dbContext);
                    totalEnviosExternas += 1;

                    Envios envioExterno = env.GetEnvioDetalle(identificador);

                    List<GruposOrganismosEmisores> gruposOrganismosEmisores = dbContext.GruposOrganismosEmisores
                        .Include("OrganismosEmisores").Include("Grupos")
                        .Where(p => p.OrganismosEmisores.ID == envioExterno.OrganismosEmisores.ID)
                        .ToList();

                    foreach (GruposOrganismosEmisores item in gruposOrganismosEmisores)
                    {
                        List<KeyValuePair<int, List<Envios>>> lista = listaGruposEnviosExternas.Where(i => i.Key == item.Grupos.ID).ToList();
                        if (lista.Count == 1)
                        {
                            lista[0].Value.Add(envioExterno);
                            lista[0] = new KeyValuePair<int, List<Envios>>(item.Grupos.ID, lista[0].Value);
                        }
                        else
                        {
                            List<Envios> nuevaLista = new List<Envios>
                            {
                                envioExterno
                            };

                            KeyValuePair<int, List<Envios>> nuevoGrupo = new KeyValuePair<int, List<Envios>>(item.Grupos.ID, nuevaLista);
                            listaGruposEnviosExternas.Add(nuevoGrupo);
                        }
                    }
                }
            }
        }

        public static void EnviarCorreoAsignacionAdmin(int tipoCarga)
        {
            try
            {
                string asunto = "Gestión de notificaciones: Proceso de carga automática de notificaciones y comunicaciones";
                string body = "";

                string rutaPlantillas = ConfigurationManager.AppSettings["rutaPlantillas"];
                using (StreamReader reader = new StreamReader(@rutaPlantillas + "email_AsignacionAutomatica.html"))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{fechaHora}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                body = body.Replace("{tipoCarga}", (tipoCarga == (int)TipoCarga.Automatica ? "Automática" : "Manual"));

                string gruposNotificacionesHtml = "";
                string gruposComunicacionesHtml = "";

                foreach (KeyValuePair<int, List<Envios>> item in listaGruposEnviosAsignados)
                {
                    Grupos gr = new Grupos();
                    Grupos grupo = gr.GetGrupoById(item.Key);

                    var cantNotificaciones = item.Value.Where(i => i.TiposEnvio.ID == (int)TipoEnvio.Notificaciones).Count();
                    gruposNotificacionesHtml += "<tr><td>" + grupo.Nombre + "</td><td style='float: right;'>" + cantNotificaciones + "</td></tr>";

                    var cantComunicaciones = item.Value.Where(i => i.TiposEnvio.ID == (int)TipoEnvio.Comunicaciones).Count();
                    gruposComunicacionesHtml += "<tr><td>" + grupo.Nombre + "</td><td style='float: right;'>" + cantComunicaciones + "</td></tr>";
                }

                /* NOTIFICACIONES */
                body = body.Replace("{notificacionesProcesadas}", totalNotificacionesProcesadas.ToString());
                body = body.Replace("{notificacionesAsignadas}", totalNotificacionesAsignadas.ToString());
                body = body.Replace("{notificacionesPendientes}", totalNotificacionesPendientes.ToString());
                body = body.Replace("{gruposNotificaciones}", gruposNotificacionesHtml);

                /* COMUNICACIONES */
                body = body.Replace("{comunicacionesProcesadas}", totalComunicacionesProcesadas.ToString());
                body = body.Replace("{comunicacionesAsignadas}", totalComunicacionesAsignadas.ToString());
                body = body.Replace("{comunicacionesPendientes}", totalComunicacionesPendientes.ToString());
                body = body.Replace("{gruposComunicaciones}", gruposComunicacionesHtml);

                AvisosNotificacion avi = new AvisosNotificacion();
                List<AvisosNotificacion> listaAvisos = avi.GetAvisos((int)TipoEmail.ProcesoCarga);
                foreach (AvisosNotificacion a in listaAvisos)
                {
                    Utils.SendMail(a.Email, asunto, body);
                }

                //Utils.SendMail("dcalero@serikat.es", asunto, body);
            }
            catch (Exception ex)
            {
                LogTareaLocaliza log = new LogTareaLocaliza();
                log.InsertLog("Error EnviarCorreoAsignacionAdmin" + ex.Message);
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, 0);
            }
        }

        public static void EnviarCorreoAsignacionGrupos()
        {
            try
            {
                foreach (KeyValuePair<int, List<Envios>> item in listaGruposEnviosAsignados)
                {
                    string asunto = "Gestión de notificaciones: Notificaciones asignadas automáticamente";
                    string body = "";

                    string rutaPlantillas = ConfigurationManager.AppSettings["rutaPlantillas"];
                    using (StreamReader reader = new StreamReader(@rutaPlantillas + "email_AsignacionPorGrupos.html"))
                    {
                        body = reader.ReadToEnd();
                    }

                    Grupos gr = new Grupos();
                    Grupos grupo = gr.GetGrupoById(item.Key);

                    body = body.Replace("{fechaHora}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    body = body.Replace("{grupo}", grupo.Nombre);

                    var cantNotificaciones = item.Value.Where(i => i.TiposEnvio.ID == (int)TipoEnvio.Notificaciones).Count();
                    var cantComunicaciones = item.Value.Where(i => i.TiposEnvio.ID == (int)TipoEnvio.Comunicaciones).Count();

                    body = body.Replace("{notificacionesAsignadas}", cantNotificaciones.ToString());
                    body = body.Replace("{comunicacionesAsignadas}", cantComunicaciones.ToString());

                    string detalleNotificacionesHTML = "";
                    string detalleComunicacionesHTML = "";

                    foreach (Envios itemEnvio in item.Value)
                    {
                        if (itemEnvio.TiposEnvio.ID == (int)TipoEnvio.Notificaciones)
                            detalleNotificacionesHTML += "<tr><td>" + itemEnvio.Identificador + "</td><td>" + itemEnvio.Concepto + "</td><td>" + itemEnvio.OrganismosEmisores.OrganismoEmisor + "</td></tr>";

                        if (itemEnvio.TiposEnvio.ID == (int)TipoEnvio.Comunicaciones)
                            detalleComunicacionesHTML += "<tr><td>" + itemEnvio.Identificador + "</td><td>" + itemEnvio.Concepto + "</td><td>" + itemEnvio.OrganismosEmisores.OrganismoEmisor + "</td></tr>";
                    }

                    body = body.Replace("{detalleNotificaciones}", detalleNotificacionesHTML);
                    body = body.Replace("{detalleComunicaciones}", detalleComunicacionesHTML);

                    GruposUsuarios usr = new GruposUsuarios();
                    List<GruposUsuarios> listaGruposUsarios = usr.GetUsuarioPorGrupo(item.Key);
                    foreach (GruposUsuarios gu in listaGruposUsarios)
                    {
                        Utils.SendMail(gu.Usuarios.Email, asunto, body);
                    }
                }
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, 0);
            }
        }

        public static void EnviarCorreoExternasAdmin()
        {
            try
            {
                string asunto = "Gestión de notificaciones: Notificaciones gestionadas por fuera de la aplicación";
                string body = "";

                string rutaPlantillas = ConfigurationManager.AppSettings["rutaPlantillas"];
                using (StreamReader reader = new StreamReader(@rutaPlantillas + "email_EnviosExternasAdmin.html"))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{fechaHora}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                body = body.Replace("{cantidad}", totalEnviosExternas.ToString());

                string gruposHtml = "";
                foreach (KeyValuePair<int, List<Envios>> item in listaGruposEnviosExternas)
                {
                    Grupos gr = new Grupos();
                    Grupos grupo = gr.GetGrupoById(item.Key);

                    var cantidad = item.Value.Count();
                    gruposHtml += "<tr><td>" + grupo.Nombre + "</td><td>" + cantidad + "</td></tr>";
                }
                body = body.Replace("{grupos}", gruposHtml);

                AvisosNotificacion avi = new AvisosNotificacion();
                List<AvisosNotificacion> listaAvisos = avi.GetAvisos((int)TipoEmail.Externas);
                foreach (AvisosNotificacion a in listaAvisos)
                {
                    Utils.SendMail(a.Email, asunto, body);
                }
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, 0);
            }
        }

        public static void EnviarCorreoExternasGrupos()
        {
            try
            {
                foreach (KeyValuePair<int, List<Envios>> item in listaGruposEnviosExternas)
                {
                    string asunto = "Gestión de notificaciones: Notificaciones gestionadas por fuera de la aplicación";
                    string body = "";

                    string rutaPlantillas = ConfigurationManager.AppSettings["rutaPlantillas"];
                    using (StreamReader reader = new StreamReader(@rutaPlantillas + "email_EnviosExternasGrupos.html"))
                    {
                        body = reader.ReadToEnd();
                    }

                    Grupos gr = new Grupos();
                    Grupos grupo = gr.GetGrupoById(item.Key);

                    body = body.Replace("{fechaHora}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    body = body.Replace("{grupo}", grupo.Nombre);

                    var cantidad = item.Value.Count();

                    body = body.Replace("{notificacionesExternas}", cantidad.ToString());

                    string detalleNotificacionesHTML = "";
                    foreach (Envios itemEnvio in item.Value)
                    {
                        if (itemEnvio.TiposEnvio.ID == (int)TipoEnvio.Notificaciones)
                            detalleNotificacionesHTML += "<tr><td>" + itemEnvio.Identificador + "</td><td>" + itemEnvio.Concepto + "</td><td>" + itemEnvio.OrganismosEmisores.OrganismoEmisor + "</td></tr>";
                    }
                    body = body.Replace("{detalleNotificaciones}", detalleNotificacionesHTML);

                    GruposUsuarios usr = new GruposUsuarios();
                    List<GruposUsuarios> listaGruposUsarios = usr.GetUsuarioPorGrupo(item.Key);
                    foreach (GruposUsuarios gu in listaGruposUsarios)
                    {
                        Utils.SendMail(gu.Usuarios.Email, asunto, body);
                    }
                }
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, 0);
            }
        }

        public static void EnviarCorreoCargaAutomaticaError()
        {
            try
            {
                string asunto = "Gestión de notificaciones: Error en el proceso de carga automática de notificaciones / comunicaciones";
                string body = "";

                string rutaPlantillas = ConfigurationManager.AppSettings["rutaPlantillas"];
                using (StreamReader reader = new StreamReader(@rutaPlantillas + "email_CargaAutomaticaError.html"))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{dia}", DateTime.Now.ToString("dd/MM/yyyy"));
                body = body.Replace("{hora}", DateTime.Now.ToString("HH:mm:ss"));

                AvisosNotificacion avi = new AvisosNotificacion();
                List<AvisosNotificacion> listaAvisos = avi.GetAvisos((int)TipoEmail.ProcesoCargaError);
                foreach (AvisosNotificacion a in listaAvisos)
                {
                    Utils.SendMail(a.Email, asunto, body);
                }
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, 0);
            }
        }
        #endregion

        #region Tarea programada Actualizar Estados
        [HttpGet]
        public ActionResult EjecutarTareaAlertasCaducadasManual()
        {
            try
            {
                EjecutarTareaAlertasCaducadas();
                return Json(new { Success = "True" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public static void EjecutarTareaAlertasCaducadas()
        {
            GestNotifContext dbContext = new GestNotifContext();

            LogTareaAlertasCaducadas log = new LogTareaAlertasCaducadas();
            log.InsertLogContext("INICIO Tarea Programada 'Caducadas y En Alerta'", dbContext);

            try
            {
                LimpiarContadores();

                PsoeRespuestasLocalizaServicio servicio = new PsoeRespuestasLocalizaServicio();
                DataTable dtIdentificadores = servicio.ActulizarEstadosEnvios();

                List<Envios> listaEnvios = new List<Envios>();
                foreach (DataRow row in dtIdentificadores.Rows)
                {
                    Envios env = new Envios();
                    Envios envio = env.GetEnvioDetalle(row[0].ToString());
                    listaEnvios.Add(envio);
                }

                #region En alerta
                List<Envios> listaEnviosAlerta = listaEnvios.Where(i => i.EstadosEnvio.ID == (int)EstadoEnvio.EnAlerta).ToList();
                List<KeyValuePair<int, List<Envios>>> listaGruposEnviosAlerta = new List<KeyValuePair<int, List<Envios>>>();

                foreach (Envios itemEnvio in listaEnviosAlerta)
                {
                    List<Grupos> grupos = dbContext.GruposEnvios.Include("Grupos").Include("Envios")
                        .Where(p => p.Envios.Identificador == itemEnvio.Identificador)
                        .Select(i => i.Grupos)
                        .ToList();

                    foreach (Grupos itemGrupo in grupos)
                    {
                        List<KeyValuePair<int, List<Envios>>> lista = listaGruposEnviosAlerta.Where(i => i.Key == itemGrupo.ID).ToList();
                        if (lista.Count == 1)
                        {
                            lista[0].Value.Add(itemEnvio);
                            lista[0] = new KeyValuePair<int, List<Envios>>(itemGrupo.ID, lista[0].Value);
                        }
                        else
                        {
                            List<Envios> nuevaLista = new List<Envios>
                            {
                                itemEnvio
                            };

                            KeyValuePair<int, List<Envios>> nuevoGrupo = new KeyValuePair<int, List<Envios>>(itemGrupo.ID, nuevaLista);
                            listaGruposEnviosAlerta.Add(nuevoGrupo);
                        }
                    }
                }

                EnviarCorreoEnviosAlertaAdmin(listaEnviosAlerta, listaGruposEnviosAlerta);
                EnviarCorreoEnviosAlertaGrupos(listaGruposEnviosAlerta);
                #endregion

                #region Caducadas
                List<Envios> listaEnviosCaducadas = listaEnvios.Where(i => i.EstadosEnvio.ID == (int)EstadoEnvio.Caducada).ToList();
                List<KeyValuePair<int, List<Envios>>> listaGruposEnviosCaducadas = new List<KeyValuePair<int, List<Envios>>>();

                foreach (Envios itemEnvio in listaEnviosCaducadas)
                {
                    List<Grupos> grupos = dbContext.GruposEnvios.Include("Grupos").Include("Envios")
                        .Where(p => p.Envios.Identificador == itemEnvio.Identificador)
                        .Select(i => i.Grupos)
                        .ToList();

                    foreach (Grupos itemGrupo in grupos)
                    {
                        List<KeyValuePair<int, List<Envios>>> lista = listaGruposEnviosCaducadas.Where(i => i.Key == itemGrupo.ID).ToList();
                        if (lista.Count == 1)
                        {
                            lista[0].Value.Add(itemEnvio);
                            lista[0] = new KeyValuePair<int, List<Envios>>(itemGrupo.ID, lista[0].Value);
                        }
                        else
                        {
                            List<Envios> nuevaLista = new List<Envios>
                            {
                                itemEnvio
                            };

                            KeyValuePair<int, List<Envios>> nuevoGrupo = new KeyValuePair<int, List<Envios>>(itemGrupo.ID, nuevaLista);
                            listaGruposEnviosCaducadas.Add(nuevoGrupo);
                        }
                    }
                }

                EnviarCorreoEnviosCaducadasAdmin(listaEnviosCaducadas, listaGruposEnviosCaducadas);
                EnviarCorreoEnviosCaducadasGrupos(listaGruposEnviosCaducadas);
                #endregion
            }
            catch (Exception ex)
            {
                log.InsertLogContext("Se ha producido una excepción: " + ex.Message + "; Consulta la tabal 'HistorialExcepciones' para más detalles", dbContext);

                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcionContext(ex.Message, ex.StackTrace, 0, dbContext);
            }

            log.InsertLogContext("FIN Tarea Programada 'Caducadas y En Alerta'", dbContext);

            dbContext.Dispose();
        }

        public static void EnviarCorreoEnviosAlertaAdmin(List<Envios> listaEnviosAlerta, List<KeyValuePair<int, List<Envios>>> listaGruposEnviosAlerta)
        {
            try
            {
                string asunto = "Gestión de notificaciones: Notificaciones en Alerta";
                string body = "";

                string rutaPlantillas = ConfigurationManager.AppSettings["rutaPlantillas"];
                using (StreamReader reader = new StreamReader(@rutaPlantillas + "email_EnviosAlertaAdmin.html"))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{fechaHora}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                body = body.Replace("{cantidad}", listaEnviosAlerta.Count.ToString());

                string gruposHtml = "";
                foreach (KeyValuePair<int, List<Envios>> item in listaGruposEnviosAlerta)
                {
                    Grupos gr = new Grupos();
                    Grupos grupo = gr.GetGrupoById(item.Key);

                    var cantEnviosAlerta = item.Value.Count();
                    gruposHtml += "<tr><td>" + grupo.Nombre + "</td><td>" + cantEnviosAlerta + "</td></tr>";
                }

                body = body.Replace("{grupos}", gruposHtml);

                AvisosNotificacion avi = new AvisosNotificacion();
                List<AvisosNotificacion> listaAvisos = avi.GetAvisos((int)TipoEmail.EnAlerta);
                foreach (AvisosNotificacion a in listaAvisos)
                {
                    Utils.SendMail(a.Email, asunto, body);
                }

                //Utils.SendMail("dcalero@serikat.es", asunto, body);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, 0);
            }
        }

        public static void EnviarCorreoEnviosAlertaGrupos(List<KeyValuePair<int, List<Envios>>> listaGruposEnviosAlerta)
        {
            try
            {
                foreach (KeyValuePair<int, List<Envios>> item in listaGruposEnviosAlerta)
                {
                    string asunto = "Gestión de notificaciones: Notificaciones en Alerta";
                    string body = "";

                    string rutaPlantillas = ConfigurationManager.AppSettings["rutaPlantillas"];
                    using (StreamReader reader = new StreamReader(@rutaPlantillas + "email_EnviosAlertaGrupos.html"))
                    {
                        body = reader.ReadToEnd();
                    }

                    Grupos gr = new Grupos();
                    Grupos grupo = gr.GetGrupoById(item.Key);

                    body = body.Replace("{fechaHora}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    body = body.Replace("{grupo}", grupo.Nombre);
                    body = body.Replace("{notificacionesAlerta}", item.Value.Count().ToString());

                    string detalleNotificacionesHTML = "";
                    foreach (Envios itemEnvio in item.Value)
                    {
                        if (itemEnvio.TiposEnvio.ID == (int)TipoEnvio.Notificaciones)
                            detalleNotificacionesHTML += "<tr><td>" + itemEnvio.Identificador + "</td><td>" + itemEnvio.Concepto + "</td><td>" + itemEnvio.OrganismosEmisores.OrganismoEmisor + "</td></tr>";
                    }

                    body = body.Replace("{detalleNotificaciones}", detalleNotificacionesHTML);

                    GruposUsuarios usr = new GruposUsuarios();
                    List<GruposUsuarios> listaGruposUsarios = usr.GetUsuarioPorGrupo(item.Key);
                    foreach (GruposUsuarios gu in listaGruposUsarios)
                    {
                        Utils.SendMail(gu.Usuarios.Email, asunto, body);
                    }
                }
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, 0);
            }
        }

        public static void EnviarCorreoEnviosCaducadasAdmin(List<Envios> listaEnviosCaducadas, List<KeyValuePair<int, List<Envios>>> listaGruposEnviosCaducadas)
        {
            try
            {
                string asunto = "Gestión de notificaciones: Notificaciones Caducadas";
                string body = "";

                string rutaPlantillas = ConfigurationManager.AppSettings["rutaPlantillas"];
                using (StreamReader reader = new StreamReader(@rutaPlantillas + "email_EnviosCaducadasAdmin.html"))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{fechaHora}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                body = body.Replace("{cantidad}", listaEnviosCaducadas.Count.ToString());

                string gruposHtml = "";
                foreach (KeyValuePair<int, List<Envios>> item in listaGruposEnviosCaducadas)
                {
                    Grupos gr = new Grupos();
                    Grupos grupo = gr.GetGrupoById(item.Key);

                    var cantEnviosComunicaciones = item.Value.Count();
                    gruposHtml += "<tr><td>" + grupo.Nombre + "</td><td>" + cantEnviosComunicaciones + "</td></tr>";
                }

                body = body.Replace("{grupos}", gruposHtml);

                AvisosNotificacion avi = new AvisosNotificacion();
                List<AvisosNotificacion> listaAvisos = avi.GetAvisos((int)TipoEmail.Caducadas);
                foreach (AvisosNotificacion a in listaAvisos)
                {
                    Utils.SendMail(a.Email, asunto, body);
                }

                //Utils.SendMail("dcalero@serikat.es", asunto, body);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, 0);
            }
        }

        public static void EnviarCorreoEnviosCaducadasGrupos(List<KeyValuePair<int, List<Envios>>> listaGruposEnviosCaducadas)
        {
            try
            {
                foreach (KeyValuePair<int, List<Envios>> item in listaGruposEnviosCaducadas)
                {
                    string asunto = "Gestión de notificaciones: Notificaciones caducadas";
                    string body = "";

                    string rutaPlantillas = ConfigurationManager.AppSettings["rutaPlantillas"];
                    using (StreamReader reader = new StreamReader(@rutaPlantillas + "email_EnviosCaducadasGrupos.html"))
                    {
                        body = reader.ReadToEnd();
                    }

                    Grupos gr = new Grupos();
                    Grupos grupo = gr.GetGrupoById(item.Key);

                    body = body.Replace("{fechaHora}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    body = body.Replace("{grupo}", grupo.Nombre);
                    body = body.Replace("{notificacionesCaducadas}", item.Value.Count().ToString());

                    string detalleNotificacionesHTML = "";
                    foreach (Envios itemEnvio in item.Value)
                    {
                        if (itemEnvio.TiposEnvio.ID == (int)TipoEnvio.Notificaciones)
                            detalleNotificacionesHTML += "<tr><td>" + itemEnvio.Identificador + "</td><td>" + itemEnvio.Concepto + "</td><td>" + itemEnvio.OrganismosEmisores.OrganismoEmisor + "</td></tr>";
                    }

                    body = body.Replace("{detalleNotificaciones}", detalleNotificacionesHTML);

                    GruposUsuarios usr = new GruposUsuarios();
                    List<GruposUsuarios> listaGruposUsarios = usr.GetUsuarioPorGrupo(item.Key);
                    foreach (GruposUsuarios gu in listaGruposUsarios)
                    {
                        Utils.SendMail(gu.Usuarios.Email, asunto, body);
                    }
                }
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, 0);
            }
        }
        #endregion

        public enum TipoCarga
        {
            Automatica = 1,
            Manual = 2
        }

        public enum TipoPeticionesLocaliza
        {
            Comunicaciones = 1,
            Notificaciones = 2,
            Todas = 3
        }
    }
}