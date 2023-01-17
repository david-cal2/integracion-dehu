using PSOENotificaciones.Contexto;
using PSOENotificaciones.Servicios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace PSOENotificaciones.Controllers
{
    public class ComunicacionesController : BaseController
    {
        // GET: Comunicaciones
        public ActionResult Comunicaciones()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                Usuarios usr = (Usuarios)Session["usuario"];
                ViewBag.IdPerfil = usr.Perfiles.ID;
                ViewBag.IdTipoEnvios = (int)TipoEnvio.Comunicaciones;
                return View();
            }
        }

        // GET: ComunicacionesEstado
        public ActionResult ComunicacionesEstado(int idEstado)
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
                        if (idEstado == (int)EstadoEnvio.SinAsignar)
                            okAcceso = true;
                        break;
                    case (int)Perfil.Gestor:
                        if (idEstado != (int)EstadoEnvio.SinAsignar)
                            okAcceso = true;
                        break;
                    case (int)Perfil.UsuarioConsulta:
                        okAcceso = true;
                        break;
                }

                if (okAcceso)
                {
                    ViewBag.IdPerfil = usr.Perfiles.ID;
                    ViewBag.IdEstado = idEstado;
                    ViewBag.IdTipoEnvios = (int)TipoEnvio.Comunicaciones;

                    switch (idEstado)
                    {
                        case (int)EstadoEnvio.SinAsignar:
                            ViewBag.Titulo = "Comunicaciones Sin asignar";
                            break;
                        case (int)EstadoEnvio.Asignada:
                            ViewBag.Titulo = "Comunicaciones Asignadas";
                            break;
                        case (int)EstadoEnvio.Leida:
                            ViewBag.Titulo = "Comunicaciones Leídas";
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

        // GET: ComunicacionDetalle
        public ActionResult ComunicacionDetalle(string identificador, string c)
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
                    ViewBag.IdTipoEnvios = (int)TipoEnvio.Comunicaciones;
                    ViewBag.VerComparecer = c;
                    ViewBag.ListaCantidadesComunicaciones = ((JsonResult)Session["ListaCantidadesComunicaciones"]).Data;
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
        }

        // GET: ComunicacionLeida
        public ActionResult ComunicacionLeida(string identificador)
        {
            if (identificador == null)
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
                    ViewBag.IdTipoEnvios = (int)TipoEnvio.Comunicaciones;
                    ViewBag.ListaCantidadesComunicaciones = ((JsonResult)Session["ListaCantidadesComunicaciones"]).Data;

                    RespuestasAcceso ra = new RespuestasAcceso();
                    RespuestasAcceso respuestaAccesoOK = ra.GetRespuestaAccesoOK(identificador);

                    Envios en1 = new Envios();
                    Envios envioComprobar = en1.GetEnvioDetalle(identificador);

                    if (respuestaAccesoOK == null && envioComprobar.EstadosEnvio.ID != (int)EstadoEnvio.Leida)
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
                            string errorCodigo = InsertRespuestaJava(ejecuteClientAcceso, identificador, idPeticionAcceso, TipoEjecucionJava.LeerComunicacion);

                            //if (errorCodigo != "")
                                RecogerAnexos(identificador);

                            if (errorCodigo == "3009 No se ha podido recuperar el documento")
                            {
                                Envios en = new Envios();
                                en.SetEstadoEnvio(identificador, EstadoEnvio.Leida, usuarioConectado.ID);
                            }

                            /*
                             * Existen tres posibles errores que se puenden dar al leer una comunicación
                             * ViewBag.ErrorNet: Excepción producida en el codigo .NET
                             * ViewBag.ErrorConectarDehu: Error que se produce en la conexión con Dehú
                             * ViewBag.ErrorCodigo: Error que se produce al comparecer la notificación (ha conectado bien con Dehú)
                             */
                            if (errorCodigo == "Excepcion")
                                ViewBag.ErrorNet = "1";
                            else
                                ViewBag.ErrorNet = "0";

                            RespuestasAcceso respOK = ra.GetRespuestaAccesoPorIdentificador(identificador);

                            bool leerOK = false;
                            if (respOK != null)
                            {
                                ViewBag.Status = respOK.CodigoRespuesta;
                                if (respOK.CodigoRespuesta == "200")
                                    leerOK = true;
                            }
                            else
                                ViewBag.Status = "0";

                            if (leerOK)
                            {
                                Envios en = new Envios();
                                en.SetEstadoEnvio(identificador, EstadoEnvio.Leida, usuarioConectado.ID);

                                ViewBag.Leer = "1";
                                ViewBag.ErrorConectarDehu = "0";
                                ViewBag.IdRespuestaAcceso = respOK.ID;
                            }
                            else
                            {
                                ViewBag.Leer = "1";
                                ViewBag.ErrorConectarDehu = String.IsNullOrEmpty(errorCodigo) ? "1" : "0";
                                ViewBag.IdRespuestaAcceso = "0";
                            }
                        }
                        catch (Exception ex)
                        {
                            HistorialExcepciones his = new HistorialExcepciones();
                            his.InsertExcepcion(ex.Message, ex.StackTrace, usuarioConectado.ID);

                            ViewBag.Leer = "1";
                            ViewBag.ErrorConectarDehu = "0";
                            ViewBag.ErrorNet = "1";

                            RespuestasAcceso respOK = ra.GetRespuestaAccesoPorIdentificador(identificador);

                            bool leerOK = false;
                            if (respOK != null)
                            {
                                ViewBag.Status = respOK.CodigoRespuesta;
                                if (respOK.CodigoRespuesta == "200")
                                    leerOK = true;
                            }
                            else
                                ViewBag.Status = "0";

                            if (leerOK)
                            {
                                Envios en = new Envios();
                                en.SetEstadoEnvio(identificador, EstadoEnvio.Leida, usuarioConectado.ID);
                                ViewBag.IdRespuestaAcceso = respOK.ID;
                                ViewBag.Status = respOK.CodigoRespuesta;
                            }
                            else
                            {
                                ViewBag.IdRespuestaAcceso = "0";
                                ViewBag.Status = "0";
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        ViewBag.Leer = "0";
                        ViewBag.ErrorConectarDehu = "0";
                        ViewBag.ErrorNet = "0";
                        ViewBag.IdRespuestaAcceso = (respuestaAccesoOK == null ? "0" : respuestaAccesoOK.ID.ToString());
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
        public ActionResult GetComunicacionesBuscadorComplejo(string idEstados, string identificador = null,
            string administracionPublica = null, string organismoRaiz = null, int? idOrganoEmisor = null,
            string asunto = null, int? idGrupo = null, string fechaDesde = null, string fechaHasta = null)
        {
            object listaComuni;

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

                string[] listaIdEstado = idEstados.Replace("[", "").Replace("]", "").Split(',');
                Usuarios usrSession = GetSessionUsuario();

                Envios en = new Envios();
                listaComuni = en.GetEnviosBuscadorComplejo((int)TipoEnvio.Comunicaciones, listaIdEstado, usrSession.ID, usrSession.Perfiles.ID,
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

            return new LargeJsonResult { Data = listaComuni, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult GetComunicaciones(string idEstados)
        {
            ResultadoEnvios listaComuni;

            try
            {
                string[] listaIdEstado = idEstados.Replace("[", "").Replace("]", "").Split(',');
                Usuarios usrSession = GetSessionUsuario();

                Envios en = new Envios();
                listaComuni = en.GetEnvios((int)TipoEnvio.Comunicaciones, listaIdEstado, usrSession.ID, usrSession.Perfiles.ID);

                ParametrosInternos pi = new ParametrosInternos();
                string num = pi.GetParametroInternoPorId((int)ParametroInterno.LimiteCargaNotificaciones).Valor;
                listaComuni.NumeroUltimosDias = Convert.ToInt32(num.Replace("-", ""));

                Session["ListaCantidadesComunicaciones"] = Json(listaComuni.CantidadesTotales, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return new LargeJsonResult { Data = listaComuni, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult GetComunicacionesPorEstado(string idEstados, bool primeros)
        {
            ResultadoEnvios listaComuni;

            try
            {
                string[] listaIdEstado = idEstados.Replace("[", "").Replace("]", "").Split(',');
                Usuarios usrSession = GetSessionUsuario();

                Envios en = new Envios();
                listaComuni = en.GetEnviosPorEstado((int)TipoEnvio.Comunicaciones, listaIdEstado, usrSession.ID, usrSession.Perfiles.ID, primeros);

                Session["ListaCantidadesComunicaciones"] = Json(listaComuni.CantidadesTotales, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return new LargeJsonResult { Data = listaComuni, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult GetComunicacionDetalle(string identificador)
        {
            Envios comunicacionDetalle;

            try
            {
                Envios en = new Envios();
                comunicacionDetalle = en.GetEnvioDetalle(identificador);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(comunicacionDetalle, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SetEstadoLeida(string identificador)
        {
            try
            {
                Envios env = new Envios();
                int idUsuarioQueLee = GetSessionUsuarioID();
                int idEstado = env.GetEnvioEstado(identificador);

                if (idEstado != (int)EstadoEnvio.Leida)
                    env.SetEstadoEnvio(identificador, EstadoEnvio.Leida, idUsuarioQueLee);
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
        public ActionResult GetCantidadEnviosLeida()
        {
            ResultadoEnvios listaComunicaciones;

            try
            {
                Usuarios usrSession = GetSessionUsuario();

                Envios en = new Envios();
                listaComunicaciones = en.GetResultadoCantidadEnvios((int)TipoEnvio.Comunicaciones, usrSession.ID, usrSession.Perfiles.ID);

                Session["ListaCantidadesComunicaciones"] = Json(listaComunicaciones.CantidadesTotales, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return new LargeJsonResult { Data = listaComunicaciones, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult GetContenidosDocumentos(string identificador, int idRespuestaAcceso)
        {
            Contenido contenido = null;
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

                JsonResult resultado = Json(new
                {
                    DetalleDocumento = detalleDocumento,
                    Contenido = contenido,
                    AnexosRefAcceso = listaAnexosReferencia,
                    ListaAnexosUrl = listaAnexosUrl,
                    ListaAnexosReferencia = listaDocContenidoAnexos,
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