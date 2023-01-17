using PSOENotificaciones.Contexto;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using static PSOENotificaciones.Controllers.TareasController;

namespace PSOENotificaciones.Controllers
{
    public class ConfiguracionController : BaseController
    {
        // GET: Avisos
        public ActionResult Avisos()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                Usuarios usr = (Usuarios)Session["usuario"];
                if (usr.Perfiles.ID == (int)Perfil.Administrador)
                {
                    ViewBag.IdPerfil = usr.Perfiles.ID;
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
        }

        // GET: Horario
        public ActionResult Horario()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                Usuarios usr = (Usuarios)Session["usuario"];
                if (usr.Perfiles.ID == (int)Perfil.Administrador)
                {
                    ViewBag.IdPerfil = usr.Perfiles.ID;
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
        }

        // GET: Parametros
        public ActionResult Parametros()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                Usuarios usr = (Usuarios)Session["usuario"];
                if (usr.Perfiles.ID == (int)Perfil.Administrador)
                {
                    ViewBag.IdPerfil = usr.Perfiles.ID;
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
        }

        [HttpGet]
        public ActionResult CargarAvisos(int idTipoEmail)
        {
            List<AvisosNotificacion> listaAvisosCompleta = new List<AvisosNotificacion>();

            try
            {
                //Usuarios usr = new Usuarios();
                //List<Usuarios> listaUsuariosAdmin = usr.GetUsuariosPorPerfil((int)Perfil.Administrador);
                //foreach (Usuarios u in listaUsuariosAdmin)
                //{
                //    AvisosNotificacion emailAdmin = new AvisosNotificacion
                //    {
                //        ID = 0,
                //        Nombre = u.Nombre,
                //        Email = u.Email,
                //        Activo = true,
                //        TipoEmail = idTipoEmail
                //    };

                //    listaAvisosCompleta.Add(emailAdmin);
                //}

                AvisosNotificacion avisos = new AvisosNotificacion();
                List<AvisosNotificacion> listaAvisos = avisos.GetAvisos(idTipoEmail);

                listaAvisosCompleta.AddRange(listaAvisos);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(listaAvisosCompleta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CargarEmail(int idEmail)
        {
            AvisosNotificacion email;

            try
            {
                AvisosNotificacion avisos = new AvisosNotificacion();
                email = avisos.GetAviso(idEmail);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(email, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult InsertarMailAviso(int id, string nombre, string mail, bool activo, int idTipoEmail)
        {
            try
            {
                AvisosNotificacion avisos = new AvisosNotificacion();
                if (id == -1)
                {
                    bool ok = avisos.InsertAvisoNotificacion(nombre.Trim(), mail.Trim(), activo, idTipoEmail);
                    if (ok)
                        return Json(new { Success = "True" }, JsonRequestBehavior.AllowGet);
                    else
                        return Json(new { Success = "False", mensaje = "Ya existe" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    avisos.UpdateAvisoNotificacion(id, nombre.Trim(), mail.Trim(), activo, idTipoEmail);
                    return Json(new { Success = "True" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult BorrarMailAviso(int id)
        {
            try
            {
                AvisosNotificacion avisos = new AvisosNotificacion();
                avisos.DeleteAvisoNotificacion(id);
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
        public ActionResult GetHorario()
        {
            List<ConfiguracionPeticiones> listaHorario;

            try
            {
                ConfiguracionPeticiones horario = new ConfiguracionPeticiones();
                listaHorario = horario.GetHorarios();
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(listaHorario, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetHorario(DiaSemana diaSemana, TimeSpan horario, bool activo)
        {
            try
            {
                ConfiguracionPeticiones configPeticiones = new ConfiguracionPeticiones();
                configPeticiones.UpdateConfiguracionPeticiones(diaSemana, horario, activo);
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
        public ActionResult GetAlertas()
        {
            List<Alertas> listaAlertas;

            try
            {
                Alertas alerta = new Alertas();
                listaAlertas = alerta.GetAlertas();
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(listaAlertas, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SetAlertas(int id, string valor)
        {
            try
            {
                Alertas alerta = new Alertas();
                alerta.UpdateAlertas(id, valor);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = "True", responseText = "Inserción Correcta" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetParametros()
        {
            List<Parametros> listaParametros;

            try
            {
                Parametros parametro = new Parametros();
                listaParametros = parametro.GetParametros();
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(listaParametros, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SetParametros(int id, string valor)
        {
            try
            {
                Parametros parametro = new Parametros();
                parametro.UpdateParametros(id, valor);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = "True", responseText = "Inserción Correcta" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ComprobarServicioPlanificador()
        {
            string estado;

            try
            {
                estado = "1";
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(estado, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ComprobarServicioDehu()
        {
            string estado;

            try
            {
                estado = "2";
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(estado, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EnviarCorreoTest()
        {
            try
            {
                TareasController.EnviarCorreoAsignacionAdmin((int)TipoCarga.Automatica);
                //TareasController.EnviarCorreoAsignacionGrupos();
                //TareasController.EnviarCorreoExternasAdmin();
                //TareasController.EnviarCorreoExternasGrupos();
                //TareasController.EnviarCorreoCargaAutomaticaError();
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = "True" }, JsonRequestBehavior.AllowGet);
        }
    }
}