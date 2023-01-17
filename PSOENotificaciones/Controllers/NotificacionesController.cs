using PSOENotificaciones.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using PSOENotificaciones.Contexto;
using PSOENotificaciones.Servicios;

namespace PSOENotificaciones.Controllers
{
    public class NotificacionesController : BaseController
    {
        // GET: Notificaciones
        public ActionResult Notificaciones()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                Usuarios usr = (Usuarios)Session["usuario"];
                ViewBag.IdPerfil = usr.Perfiles.ID;
                ViewBag.IdTipoEnvios = (int)TipoEnvio.Notificaciones;
                return View();
            }
        }

        // GET: NotificacionesEstado
        public ActionResult NotificacionesEstado(int? idEstado)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                bool okAcceso = false;
                Usuarios usr = (Usuarios)Session["usuario"];

                switch (usr.Perfiles.ID)
                {
                    case (int)Perfil.Administrador:
                        okAcceso = true;
                        break;
                    case (int)Perfil.Asignador:
                        if (idEstado.Value == (int)EstadoEnvio.SinAsignar)
                            okAcceso = true;
                        break;
                    case (int)Perfil.Gestor:
                        if (idEstado.Value != (int)EstadoEnvio.SinAsignar)
                            okAcceso = true;
                        break;
                    case (int)Perfil.UsuarioConsulta:
                        okAcceso = true;
                        break;
                }

                if (okAcceso)
                {
                    ViewBag.IdPerfil = usr.Perfiles.ID;
                    ViewBag.IdEstado = idEstado.Value;
                    ViewBag.IdTipoEnvios = (int)TipoEnvio.Notificaciones;

                    if (idEstado.Value == (int)EstadoEnvio.Comparecida && 
                        usr.Perfiles.ID == (int)Perfil.Administrador)
                    {
                        return RedirectToAction("NotificacionesAdminComp", "Notificaciones");
                    }

                    switch (idEstado.Value)
                    {
                        case (int)EstadoEnvio.SinAsignar:
                            ViewBag.Titulo = "Notificaciones Sin asignar";
                            break;
                        case (int)EstadoEnvio.EnAlerta:
                            ViewBag.Titulo = "Notificaciones En alerta";
                            break;
                        case (int)EstadoEnvio.Asignada:
                            ViewBag.Titulo = "Notificaciones Asignadas";
                            break;
                        case (int)EstadoEnvio.Comparecida:
                            ViewBag.Titulo = "Notificaciones Comparecidas";
                            break;
                        case (int)EstadoEnvio.Caducada:
                            ViewBag.Titulo = "Notificaciones Caducadas";
                            break;
                        case (int)EstadoEnvio.Externas:
                            ViewBag.Titulo = "Notificaciones Externas";
                            break;
                        default:
                            return RedirectToAction("Login", "Home");
                            //break;
                    }

                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
        }

        // GET: NotificacionesAdminComp
        public ActionResult NotificacionesAdminComp()
        {
            ActualizaJsonNotificacionesAdminComparecidas();

            return View();
        }

        // GET: NotificacionDetalle
        public ActionResult NotificacionDetalle(string identificador, string c)
        {
            if (identificador == null || c == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                bool okAcceso = false;
                Usuarios usr = (Usuarios)Session["usuario"];

                switch (usr.Perfiles.ID)
                {
                    case (int)Perfil.Administrador:
                        okAcceso = true;
                        break;
                    case (int)Perfil.Asignador:
                        okAcceso = true;
                        break;
                    case (int)Perfil.Gestor:
                        okAcceso = true;
                        break;
                    case (int)Perfil.UsuarioConsulta:
                        okAcceso = true;
                        break;
                }

                if (okAcceso)
                {
                    ViewBag.IdPerfil = usr.Perfiles.ID;
                    ViewBag.Identificador = identificador;
                    ViewBag.IdTipoEnvios = (int)TipoEnvio.Notificaciones;
                    ViewBag.VerComparecer = c;
                    ViewBag.ListaCantidadesNotificaciones = ((JsonResult)Session["ListaCantidadesNotificaciones"]).Data;
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
        }

        // GET: NotificacionComparecida
        public ActionResult NotificacionComparecida(string identificador)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                bool okAcceso = false;
                Usuarios usuarioConectado = GetSessionUsuario();

                switch (usuarioConectado.Perfiles.ID)
                {
                    case (int)Perfil.Administrador:
                        okAcceso = true;
                        break;
                    case (int)Perfil.Asignador:
                        okAcceso = false;
                        break;
                    case (int)Perfil.Gestor:
                        okAcceso = true;
                        break;
                    case (int)Perfil.UsuarioConsulta:
                        okAcceso = true;
                        break;
                }

                if (okAcceso)
                {
                    ViewBag.IdPerfil = usuarioConectado.Perfiles.ID;
                    ViewBag.Identificador = identificador;
                    ViewBag.IdTipoEnvios = (int)TipoEnvio.Notificaciones;
                    ViewBag.ListaCantidadesNotificaciones = ((JsonResult)Session["ListaCantidadesNotificaciones"]).Data;

                    RespuestasAcceso ra = new RespuestasAcceso();
                    RespuestasAcceso respuestaAccesoOK = ra.GetRespuestaAccesoOK(identificador);

                    if (respuestaAccesoOK == null)
                    {
                        #region Llamar PeticionAcceso
                        try
                        {
                            #region Insert PeticionAcceso
                            Envios env = new Envios();
                            Envios envio = env.GetEnvioDetalle(identificador);
                            DateTime fecha = DateTime.Now;
                            int codigoOrigen = envio.CodigoOrigen;
                            string nifReceptor = ConfigurationManager.AppSettings["nifReceptor"];
                            string nombreReceptor = ConfigurationManager.AppSettings["nombreReceptor"];
                            string concepto = envio.Concepto;

                            PeticionesAcceso pa = new PeticionesAcceso();
                            int idPeticionAcceso = pa.InsertPeticionAcceso(identificador, usuarioConectado.ID, fecha,
                                codigoOrigen, nifReceptor, nombreReceptor, concepto);
                            #endregion

                            EjecuteClientAcceso ejecuteClientAcceso = new EjecuteClientAcceso();
                            string errorCodigo = InsertRespuestaJava(ejecuteClientAcceso, identificador, idPeticionAcceso, TipoEjecucionJava.Comparecer);

                            //if (errorCodigo != "")
                                RecogerAnexos(identificador);

                            /*
                             * Existen tres posibles errores que se puenden dar al comparecer una notificación
                             * ViewBag.ErrorNet: Excepción producida en el codigo .NET
                             * ViewBag.ErrorConectarDehu: Error que se produce en la conexión con Dehú
                             * ViewBag.ErrorCodigo: Error que se produce al comparecer la notificación (ha conectado bien con Dehú)
                             */
                            if (errorCodigo == "Excepcion")
                                ViewBag.ErrorNet = "1";
                            else
                                ViewBag.ErrorNet = "0";

                            RespuestasAcceso respOK = ra.GetRespuestaAccesoPorIdentificador(identificador);

                            bool comparecerOK = false;
                            if (respOK != null)
                            {
                                ViewBag.Status = respOK.CodigoRespuesta;
                                if (respOK.CodigoRespuesta == "200")
                                    comparecerOK = true;
                            }
                            else
                                ViewBag.Status = "0";

                            if (comparecerOK)
                            {
                                Envios en = new Envios();
                                en.SetEstadoEnvio(identificador, EstadoEnvio.Comparecida, usuarioConectado.ID);

                                ViewBag.Comparecer = "1";
                                ViewBag.ErrorConectarDehu = "0";
                                ViewBag.IdRespuestaAcceso = respOK.ID;
                            }
                            else
                            {
                                ViewBag.Comparecer = "1";
                                ViewBag.ErrorConectarDehu = String.IsNullOrEmpty(errorCodigo) ? "1" : "0";
                                ViewBag.IdRespuestaAcceso = "0";
                            }
                        }
                        catch (Exception ex)
                        {
                            HistorialExcepciones his = new HistorialExcepciones();
                            his.InsertExcepcion(ex.Message, ex.StackTrace, usuarioConectado.ID);

                            ViewBag.Comparecer = "1";
                            ViewBag.ErrorConectarDehu = "0";
                            ViewBag.ErrorNet = "1";

                            RespuestasAcceso respOK = ra.GetRespuestaAccesoPorIdentificador(identificador);

                            bool comparecerOK = false;
                            if (respOK != null)
                            {
                                ViewBag.Status = respOK.CodigoRespuesta;
                                if (respOK.CodigoRespuesta == "200")
                                    comparecerOK = true;
                            }
                            else
                                ViewBag.Status = "0";

                            if (comparecerOK)
                            {
                                Envios en = new Envios();
                                en.SetEstadoEnvio(identificador, EstadoEnvio.Comparecida, usuarioConectado.ID);
                                ViewBag.IdRespuestaAcceso = respOK.ID;
                            }
                            else
                            {
                                ViewBag.IdRespuestaAcceso = "0";
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        ViewBag.Comparecer = "0";
                        ViewBag.ErrorConectarDehu = "0";
                        ViewBag.ErrorNet = "0";
                        ViewBag.IdRespuestaAcceso = respuestaAccesoOK.ID;
                        ViewBag.Status = "0";
                    }

                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
        }

        public ActionResult GetNotificacionesBuscadorComplejo(string idEstados, string identificador = null,
            string administracionPublica = null, string organismoRaiz = null, int? idOrganoEmisor = null,
            string asunto = null, int? idGrupo = null, string fechaDesde = null, string fechaHasta = null)
        {
            object listaNotif;

            try
            {
                DateTime? f1 = null;
                if (!String.IsNullOrEmpty(fechaDesde))
                {
                    string[] c = fechaDesde.Split('/');
                    f1 = new DateTime(Convert.ToInt32(c[2]), Convert.ToInt32(c[1]), Convert.ToInt32(c[0]));
                }
                DateTime? f2 = null;
                if (!String.IsNullOrEmpty(fechaHasta))
                {
                    string[] c = fechaHasta.Split('/');
                    f2 = new DateTime(Convert.ToInt32(c[2]), Convert.ToInt32(c[1]), Convert.ToInt32(c[0]), 23, 59, 59);
                }

                Usuarios usrSession = GetSessionUsuario();

                if (usrSession.Perfiles.ID == (int)Perfil.Asignador)
                {
                    idEstados = "[1,2,4,5,7]";
                }

                string[] listaIdEstado = idEstados.Replace("[", "").Replace("]", "").Split(',');

                Envios en = new Envios();
                listaNotif = en.GetEnviosBuscadorComplejo((int)TipoEnvio.Notificaciones, listaIdEstado, usrSession.ID, usrSession.Perfiles.ID,
                    (identificador == "" ? null : identificador),
                    (administracionPublica == "" ? "Seleccionar" : administracionPublica),
                    (organismoRaiz == "" ? "Seleccionar" : organismoRaiz),
                    idOrganoEmisor, (asunto == "" ? null : asunto), idGrupo, f1, f2);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return new LargeJsonResult { Data = listaNotif, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult GetNotificaciones(string idEstados)
        {
            ResultadoEnvios listaNotif;

            try
            {
                string[] listaIdEstado = idEstados.Replace("[", "").Replace("]", "").Split(',');
                Usuarios usrSession = GetSessionUsuario();

                Envios en = new Envios();
                listaNotif = en.GetEnvios((int)TipoEnvio.Notificaciones, listaIdEstado, usrSession.ID, usrSession.Perfiles.ID);
                
                ParametrosInternos pi = new ParametrosInternos();
                string num = pi.GetParametroInternoPorId((int)ParametroInterno.LimiteCargaNotificaciones).Valor;
                listaNotif.NumeroUltimosDias = Convert.ToInt32(num.Replace("-", ""));

                Session["ListaCantidadesNotificaciones"] = Json(listaNotif.CantidadesTotales, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return new LargeJsonResult { Data = listaNotif, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult GetNotificacionesPorEstado(string idEstados, bool primeros)
        {
            ResultadoEnvios listaNotif;

            try
            {
                string[] listaIdEstado = idEstados.Replace("[", "").Replace("]", "").Split(',');
                Usuarios usrSession = GetSessionUsuario();

                Envios en = new Envios();
                listaNotif = en.GetEnviosPorEstado((int)TipoEnvio.Notificaciones, listaIdEstado, usrSession.ID, usrSession.Perfiles.ID, primeros);

                Session["ListaCantidadesNotificaciones"] = Json(listaNotif.CantidadesTotales, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return new LargeJsonResult { Data = listaNotif, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult GetCantidadEnviosComparecida()
        {
            ResultadoEnvios listaNotif;

            try
            {
                Usuarios usrSession = GetSessionUsuario();

                Envios en = new Envios();
                listaNotif = en.GetResultadoCantidadEnvios((int)TipoEnvio.Notificaciones, usrSession.ID, usrSession.Perfiles.ID);

                Session["ListaCantidadesNotificaciones"] = Json(listaNotif.CantidadesTotales, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return new LargeJsonResult { Data = listaNotif, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult GetNotificacionDetalle(string identificador)
        {
            Envios notificacionDetalle;

            try
            {
                Envios en = new Envios();
                notificacionDetalle = en.GetEnvioDetalle(identificador);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(notificacionDetalle, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetContenidosDocumentos(string identificador, int idRespuestaAcceso)
        {
            Contenido contenido = null;
            AcusesPdf acusePdf = null;
            Contenido2 contenidoAcuse = null;

            try
            {
                #region Documento Principal Contenido
                DetalleDocumentos dd = new DetalleDocumentos();
                DetalleDocumentos detalleDocumento = dd.GetDetalleDocumento(idRespuestaAcceso);

                if (detalleDocumento != null)
                {
                    Contenido con = new Contenido();
                    contenido = con.GetContenido(detalleDocumento.ID);
                }
                #endregion

                #region Referencias de Anexo
                AnexoReferencia ar = new AnexoReferencia();
                List<AnexoReferencia> listaAnexosReferencia = ar.GetAnexosReferencia(idRespuestaAcceso);
                #endregion

                #region Anexos Url
                AnexoUrl au = new AnexoUrl();
                List<AnexoUrl> listaAnexosUrl = au.GetAnexosUrl(idRespuestaAcceso);
                #endregion

                #region Anexos Refrencia Contenido1
                List<DocumentosContenidoAnexos> listaDocContenidoAnexos = new List<DocumentosContenidoAnexos>();

                PeticionesAnexo pan = new PeticionesAnexo();
                List<PeticionesAnexo> listaPeticionesAnexos = pan.GetPeticionesAnexos(identificador);

                foreach (PeticionesAnexo p in listaPeticionesAnexos)
                {
                    RespuestasAnexo ran = new RespuestasAnexo();
                    RespuestasAnexo respuestaAnexo = ran.GetRespuestaAnexo(p.ID);

                    if (respuestaAnexo != null)
                    {
                        DocumentosAnexo dan = new DocumentosAnexo();
                        DocumentosAnexo docAnexo = dan.GetDocumentoAnexo(respuestaAnexo.ID);

                        if (docAnexo != null)
                        {
                            Contenido1 con1 = new Contenido1();
                            Contenido1 contenidoAnexo = con1.GetContenido1(docAnexo.ID);

                            if (contenidoAnexo != null)
                            {
                                DocumentosContenidoAnexos dca = new DocumentosContenidoAnexos
                                {
                                    DocAnexo = docAnexo,
                                    ContenidoAnexo = contenidoAnexo
                                };
                                listaDocContenidoAnexos.Add(dca);
                            }
                        }
                    }

                }
                #endregion

                #region Acuse Pdf Contenido2
                PeticionesAcusePdf pap = new PeticionesAcusePdf();
                PeticionesAcusePdf peticionAcusePdf = pap.GetPeticionesAcusePdf(identificador);

                if (peticionAcusePdf != null)
                {
                    RespuestasAcusePdf rap = new RespuestasAcusePdf();
                    RespuestasAcusePdf respuestaAcusePdf = rap.GetRespuestasAcusePdf(peticionAcusePdf.ID);

                    if (respuestaAcusePdf != null)
                    {
                        AcusesPdf acu = new AcusesPdf();
                        acusePdf = acu.GetAcusesPdfPorIdRespuesta(respuestaAcusePdf.ID);

                        if (acusePdf != null)
                        {
                            Contenido2 con2 = new Contenido2();
                            contenidoAcuse = con2.GetContenido2(acusePdf.ID);
                        }

                    }
                }
                #endregion

                JsonResult resultado = Json(new
                {
                    DetalleDocumento = detalleDocumento,
                    Contenido = contenido,
                    AnexosRefAcceso = listaAnexosReferencia,
                    ListaAnexosUrl = listaAnexosUrl,
                    ListaAnexosReferencia = listaDocContenidoAnexos,
                    AcusePdf = acusePdf,
                    ContenidoAcuse = contenidoAcuse
                }, JsonRequestBehavior.AllowGet);

                return resultado;
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AsignarGrupoEnvio(string identificador, string idGrupos)
        {
            try
            {
                string cadena = AsignarGrupoEnvioManual(identificador, idGrupos);

                return Json(new { Success = "True", cadenaGrupos = cadena }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult SetObservaciones(string identificador, string observaciones)
        {
            try
            {
                Envios env = new Envios();
                env.SetObservaciones(identificador, observaciones);
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
        public ActionResult ComprobarCaducidad(string identificador)
        {
            try
            {
                HistorialCambiosEnvio hce = new HistorialCambiosEnvio();
                HistorialCambiosEnvio obj = hce.GetEnvioCaducado(identificador);

                if (obj != null)
                {
                    Envios env = new Envios();
                    DateTime fechaCaducidad = env.GetEnvioFechaCaducidad(identificador);

                    return Json(new { Success = "True", Fecha = fechaCaducidad }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { Success = "False" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult LlamarAnexos(string identificador)
        {
            try
            {
                RecogerAnexos(identificador);

                JsonResult resultado = Json(new { Success = "True" }, JsonRequestBehavior.AllowGet);

                return resultado;
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult LlamarAcusePdf(string identificador)
        {
            try
            {
                RecogerAcusePdf(identificador);

                JsonResult resultado = Json(new { Success = "True" }, JsonRequestBehavior.AllowGet);

                return resultado;
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public void RecogerAcusePdf(string identificador)
        {
            try
            {
                EjecuteClientAcceso ejecuteClientAcceso = new EjecuteClientAcceso();

                PeticionesAcceso pa = new PeticionesAcceso();
                PeticionesAcceso peticionesAcceso = pa.GetPeticionAcceso(identificador);

                //Llamada a Dehú solo del Acuse Pdf
                string errorCodigo = InsertRespuestaJava(ejecuteClientAcceso, identificador, peticionesAcceso.ID, TipoEjecucionJava.Acuse, null);

                //Si no ha habido ningún error se acualzian los campos de error a vacío.
                if (String.IsNullOrEmpty(errorCodigo))
                {
                    Envios env = new Envios();
                    env.InsertErroresEnvio(String.Empty, String.Empty, String.Empty, identificador);
                }
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
            }
        }

        public class DocumentosContenidoAnexos
        {
            DocumentosAnexo docAnexo;
            Contenido1 contenidoAnexo;

            public DocumentosAnexo DocAnexo
            {
                get
                {
                    return this.docAnexo;
                }
                set
                {
                    this.docAnexo = value;
                }
            }

            public Contenido1 ContenidoAnexo
            {
                get
                {
                    return this.contenidoAnexo;
                }
                set
                {
                    this.contenidoAnexo = value;
                }
            }
        }
    }
}