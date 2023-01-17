using PSOENotificaciones.Contexto;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Web.Mvc;

namespace PSOENotificaciones.Controllers
{
    public class AuditoriaController : BaseController
    {
        // GET: EjecutarTareas
        public ActionResult EjecutarTareas()
        {
            return View();
        }

        // GET: LogTareaLocaliza
        public ActionResult LogTareaLocaliza()
        {
            return View();
        }

        // GET: AuditoriaEnvio
        public ActionResult AuditoriaEnvio()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return View();
            }
        }

        // GET: ListadoExcepciones
        public ActionResult ListadoExcepciones()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult GetExcepciones()
        {
            List<HistorialExcepciones> lista;

            try
            {
                HistorialExcepciones exc = new HistorialExcepciones();
                lista = exc.GetExcepciones();
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return new LargeJsonResult { Data = lista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult GetHistorialCambiosEnvio(string identificador)
        {
            List<HistorialCambiosEnvio> listaEstados;
            List<HistorialCambiosEnvio> listaGrupos;

            try
            {
                HistorialCambiosEnvio hce = new HistorialCambiosEnvio();
                listaEstados = hce.GetHistorialCambiosEnvio(identificador, TipoCambioEnvio.CambioEstado);
                listaGrupos = hce.GetHistorialCambiosEnvio(identificador, TipoCambioEnvio.GrupoAsignado);
            }
            catch (DbEntityValidationException ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { ListaEstados = listaEstados, ListaGrupos = listaGrupos }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPeticiones(string identificador)
        {
            AuditoriaEnvio objAuditoria = new AuditoriaEnvio();

            try
            {
                Envios env = new Envios();
                Envios objEnvio = env.GetEnvioAuditoria(identificador);

                if (objEnvio != null)
                {
                    //objAuditoria.Envio = objEnvio;

                    RespuestasLocaliza rl = new RespuestasLocaliza();
                    RespuestasLocaliza objRespuesta = rl.GetRespuestaLocaliza(objEnvio.RepuestasLocaliza.ID);

                    if (objRespuesta != null)
                    {
                        objAuditoria.RespuestaLocaliza = objRespuesta;

                        PeticionesLocaliza pl = new PeticionesLocaliza();
                        PeticionesLocaliza peticionLocaliza = pl.GetPeticionLocaliza(objRespuesta.PeticionesLocaliza.ID);

                        if (peticionLocaliza != null)
                        {
                            objAuditoria.PeticionLocaliza = peticionLocaliza;
                        }
                    }

                    PeticionesAcceso pa = new PeticionesAcceso();
                    PeticionesAcceso objPeticcionAcceso = pa.GetPeticionAcceso(identificador);

                    if (objPeticcionAcceso != null)
                    {
                        objAuditoria.PeticionAcceso = objPeticcionAcceso;

                        RespuestasAcceso ra = new RespuestasAcceso();
                        RespuestasAcceso objRepuestaAcceso = ra.GetRespuestaAccesoPorIdentificador(identificador);

                        if (objRepuestaAcceso != null)
                        {
                            objAuditoria.RespuestaAcceso = objRepuestaAcceso;

                            DetalleDocumentos dd = new DetalleDocumentos();
                            DetalleDocumentos detalleDocumento = dd.GetDetalleDocumento(objRepuestaAcceso.ID);

                            if (detalleDocumento != null)
                            {
                                objAuditoria.DetalleDocumento = detalleDocumento;

                                HashDocumento hd = new HashDocumento();
                                HashDocumento hashDocumento = hd.GetHashDocumento(detalleDocumento.ID);

                                if (hashDocumento != null)
                                {
                                    objAuditoria.HashDocumento = hashDocumento;
                                }
                            }
                        }
                    }

                    PeticionesAcusePdf pAcuse = new PeticionesAcusePdf();
                    PeticionesAcusePdf objPeticionAcuse = pAcuse.GetPeticionesAcusePdf(identificador);

                    if (objPeticionAcuse != null)
                    {
                        objAuditoria.PeticionAcusePdf = objPeticionAcuse;

                        RespuestasAcusePdf ra = new RespuestasAcusePdf();
                        RespuestasAcusePdf objRespuestaAcuse = ra.GetRespuestasAcusePdf(objPeticionAcuse.ID);

                        if (objRespuestaAcuse != null)
                        {
                            objAuditoria.RespuestaAcusePdf = objRespuestaAcuse;

                            AcusesPdf ac = new AcusesPdf();
                            AcusesPdf acusePdf = ac.GetAcusesPdfPorIdRespuesta(objRespuestaAcuse.ID);

                            if (acusePdf != null)
                            {
                                objAuditoria.AcusePdf = acusePdf;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = "True", auditoria = objAuditoria }, JsonRequestBehavior.AllowGet);
            //return new LargeJsonResult { Data = objAuditoria, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult GetLogTareaLocaliza()
        {
            List<LogTareaLocaliza> lista;
            try
            {
                LogTareaLocaliza log = new LogTareaLocaliza();
                DateTime fechaInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                DateTime fechaFin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                lista = log.GetLogsPorFecha(fechaInicio, fechaFin);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { ListaLogs = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ActualizaJsonOrganimos()
        {
            try
            {
                ActualizaJsonOrganimosTarea();
                return Json(new { Success = "True" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }

    public class AuditoriaEnvio
    {
        PeticionesLocaliza peticionLocaliza;
        RespuestasLocaliza respuestaLocaliza;

        PeticionesAcceso peticionAcceso;
        RespuestasAcceso respuestaAcceso;
        DetalleDocumentos detalleDocumento;
        HashDocumento hashDocumento;

        PeticionesAcusePdf peticionAcusePdf;
        RespuestasAcusePdf respuestaAcusePdf;
        AcusesPdf acusePdf;

        public PeticionesLocaliza PeticionLocaliza
        {
            get
            {
                return this.peticionLocaliza;
            }
            set
            {
                this.peticionLocaliza = value;
            }
        }

        public RespuestasLocaliza RespuestaLocaliza
        {
            get
            {
                return this.respuestaLocaliza;
            }
            set
            {
                this.respuestaLocaliza = value;
            }
        }

        public PeticionesAcceso PeticionAcceso
        {
            get
            {
                return this.peticionAcceso;
            }
            set
            {
                this.peticionAcceso = value;
            }
        }

        public RespuestasAcceso RespuestaAcceso
        {
            get
            {
                return this.respuestaAcceso;
            }
            set
            {
                this.respuestaAcceso = value;
            }
        }

        public DetalleDocumentos DetalleDocumento
        {
            get
            {
                return this.detalleDocumento;
            }
            set
            {
                this.detalleDocumento = value;
            }
        }

        public HashDocumento HashDocumento
        {
            get
            {
                return this.hashDocumento;
            }
            set
            {
                this.hashDocumento = value;
            }
        }

        public PeticionesAcusePdf PeticionAcusePdf
        {
            get
            {
                return this.peticionAcusePdf;
            }
            set
            {
                this.peticionAcusePdf = value;
            }
        }

        public RespuestasAcusePdf RespuestaAcusePdf
        {
            get
            {
                return this.respuestaAcusePdf;
            }
            set
            {
                this.respuestaAcusePdf = value;
            }
        }

        public AcusesPdf AcusePdf
        {
            get
            {
                return this.acusePdf;
            }
            set
            {
                this.acusePdf = value;
            }
        }
    }
}
