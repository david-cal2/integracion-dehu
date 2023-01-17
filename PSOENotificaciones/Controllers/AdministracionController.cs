using PSOENotificaciones.Servicios;
using PSOENotificaciones.Contexto;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.IO;
using PSOENotificaciones.Helpers;
using System.Configuration;

namespace PSOENotificaciones.Controllers
{
    public class AdministracionController : BaseController
    {
        // GET: Grupos
        public ActionResult Grupos()
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

        // GET: GrupoDetalle
        public ActionResult GrupoDetalle(int id)
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
                    ViewBag.IdGrupoEditado = id;

                    ActualizaJsonOrganimosTarea();

                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
        }

        // GET: GrupoNuevo
        public ActionResult GrupoNuevo()
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

        // GET: Perfiles
        public ActionResult Perfiles()
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

        // GET: PerfilDetalle
        public ActionResult PerfilDetalle(int id)
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
                    ViewBag.IdPerfilEditar = id;
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
        }

        // GET: Organismos
        [OutputCache(Duration = 10, VaryByParam = "none")]
        public ActionResult Organismos()
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

                    ActualizaJsonOrganimosTarea();

                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
        }

        // GET: OrganismosPag
        public ActionResult OrganismosPag()
        {
            return View();
        }

        // GET: Usuarios
        public ActionResult Usuarios()
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

        // GET: UsuarioEditar
        public ActionResult UsuarioEditar(int id)
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
                    ViewBag.IdUsuarioEditado = id;
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
        }

        // GET: UsuarioNuevo
        public ActionResult UsuarioNuevo()
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

        // GET: UsuarioDetalle
        public ActionResult UsuarioDetalle()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                Usuarios usr = (Usuarios)Session["usuario"];
                ViewBag.IdPerfil = usr.Perfiles.ID;
                return View();
            }
        }

        // GET: UsuarioPass
        public ActionResult UsuarioPass()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                Usuarios usr = (Usuarios)Session["usuario"];
                ViewBag.IdPerfil = usr.Perfiles.ID;
                return View();
            }
        }

        // GET: UsuarioGrupos
        public ActionResult UsuarioGrupos()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                Usuarios usr = (Usuarios)Session["usuario"];
                ViewBag.IdPerfil = usr.Perfiles.ID;
                return View();
            }
        }

        [HttpGet]
        public ActionResult GetPantallasInicio()
        {
            List<PantallasInicio> listaPantallas;

            try
            {
                PantallasInicio p = new PantallasInicio();
                listaPantallas = p.GetPantallasInicio();
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(listaPantallas, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [OutputCache(Duration = 1000, VaryByParam = "none")]
        public ActionResult CargarPerfiles()
        {
            List<Perfiles> listaPerfiles;

            try
            {
                Perfiles perf = new Perfiles();
                listaPerfiles = perf.GetPerfiles();
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(listaPerfiles, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPerfilDetalle(int id)
        {
            Perfiles perfil;

            try
            {
                Perfiles p = new Perfiles();
                perfil = p.GetPerfilById(id);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(perfil, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetOrganismosEmisores(string organismoRaiz)
        {
            LargeJsonResult resultado;
            object listaOrganismosEmisores;

            try
            {
                OrganismosEmisores oe = new OrganismosEmisores();
                listaOrganismosEmisores = oe.GetOrganismosEmisoresSelect(organismoRaiz);
                resultado = new LargeJsonResult { Data = listaOrganismosEmisores, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return resultado;
        }

        [HttpGet]
        public ActionResult GetOrganismosEmisoresTablaLenght()
        {
            List<OrganismosEmisores> listaOrganismosEmisores;
            try
            {
                OrganismosEmisores oe = new OrganismosEmisores();
                listaOrganismosEmisores = oe.GetOrganismosEmisoresTabla();
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return new LargeJsonResult { Data = listaOrganismosEmisores, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult GetOrganismosEmisoresTablaPosicion(bool primeros)
        {
            List<OrganismosEmisores> listaOrganismosEmisores;
            try
            {
                OrganismosEmisores oe = new OrganismosEmisores();
                listaOrganismosEmisores = oe.GetOrganismosEmisoresTablaPosicion(primeros);
                return new LargeJsonResult { Data = listaOrganismosEmisores, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }  
        }

        [HttpGet]
        public ActionResult GetOrganismosEmisoresJson()
        {
            try
            {
                string json = System.IO.File.ReadAllText(Server.MapPath("~") + @"json\organismos.json");
                return new LargeJsonResult { Data = json, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetOrganismosEmisoresTablaPaginado(int pagina, int maximo)
        {
            List<OrganismosEmisores> listaOrganismosEmisores;
            try
            {
                OrganismosEmisores oe = new OrganismosEmisores();
                listaOrganismosEmisores = oe.GetOrganismosEmisoresTablaPaginado(pagina, maximo);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return new LargeJsonResult { Data = listaOrganismosEmisores, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult GetOrganismosEmisoresTabla()
        {
            List<OrganismosEmisores> listaOrganismosEmisores;
            try
            {
                OrganismosEmisores oe = new OrganismosEmisores();
                listaOrganismosEmisores = oe.GetOrganismosEmisoresTabla();
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return new LargeJsonResult { Data = listaOrganismosEmisores, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult GetOrganismosEmisoresTablaFiltro(string administracionPublica = null, string organismoRaiz = null,
            int? idOrganoEmisor = null, int? idProvincia = null, int? idCCAA = null)
        {
            List<OrganismosEmisores> listaOrganismosEmisores;
            try
            {
                OrganismosEmisores oe = new OrganismosEmisores();
                listaOrganismosEmisores = oe.GetOrganismosEmisoresTablaFiltro(
                    (administracionPublica == "" ? null : administracionPublica),
                    (organismoRaiz == "" ? null : organismoRaiz),
                    idOrganoEmisor, idProvincia, idCCAA);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return new LargeJsonResult { Data = listaOrganismosEmisores, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult GetUsuarios()
        {
            List<Usuarios> listaUsuarios;

            try
            {
                Usuarios us = new Usuarios();
                listaUsuarios = us.GetUsuarios();
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(listaUsuarios, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetUsuarioDetalle(int idUsuario)
        {
            Usuarios usuario;

            try
            {
                Usuarios us = new Usuarios();
                usuario = us.GetUsuarioPorId(idUsuario);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(usuario, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetGrupos()
        {
            List<Grupos> listaGrupos;

            try
            {
                Grupos gr = new Grupos();
                listaGrupos = gr.GetGrupos();
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
        public ActionResult GetGruposAcitvos()
        {
            List<Grupos> listaGrupos;

            try
            {
                Grupos gr = new Grupos();
                listaGrupos = gr.GetGruposAcitvos();
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
        public ActionResult ActualizarPerfil(int idPerfilEditado, string nombre, string desc, bool noti, bool admi, bool confi, int pantInicio)
        {
            try
            {
                Perfiles perfil = new Perfiles();
                if (idPerfilEditado != -1)
                {
                    perfil.UpdatePerfiles(idPerfilEditado, nombre, desc, noti, admi, confi, pantInicio);
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
        public ActionResult InsertarEditarUsuario(int id, string nombre, string ape, string nif, string mail, string tlf, string login, bool activo, int perId, bool bloqueado, string pass, bool estaActivo, bool estaBloqueado)
        {
            try
            {
                if (id == -1)
                {
                    PsoeAdministracionServicio objAdmin = new PsoeAdministracionServicio();
                    string resultado = objAdmin.InsertUsuario(nombre, ape, nif, mail, tlf, login, perId, pass);

                    if (resultado == "-1")
                        return Json(new { Success = "False", responseText = "El email ya está registrado" }, JsonRequestBehavior.AllowGet);
                    else if (resultado == "-2")
                        return Json(new { Success = "False", responseText = "El login de usuario ya está registrado" }, JsonRequestBehavior.AllowGet);
                    else
                        return Json(new { Success = "True", idNuevoUsuario = resultado }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Usuarios usr = new Usuarios();
                    bool resultado = usr.UpdateUsuario(id, nombre, ape, nif, mail.Trim(), tlf, activo, perId, bloqueado, estaActivo, estaBloqueado);

                    if (!resultado)
                        return Json(new { Success = "False", responseText = "El email ya está registrado" }, JsonRequestBehavior.AllowGet);
                    else
                        return Json(new { Success = "True", responseText = "Actualización correcta" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", mensajeExcepcion = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult EditarUsuarioConectado(string nombre, string apellidos, string nif, string mail, string tlf, string login)
        {
            try
            {
                Usuarios usr = new Usuarios();
                bool resultado = usr.UpdateUsuario(GetSessionUsuarioID(), nombre, apellidos, nif, mail, tlf, login);

                if (!resultado)
                    return Json(new { Success = "False", }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { Success = "True" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AceptarAvisoLegal()
        {
            try
            {
                Usuarios usr = GetSessionUsuario();

                Usuarios usuario = new Usuarios();
                usuario.AceptarConsentimiento(usr.ID);

                Envios en = new Envios();
                bool tieneEnviosEnAlerta = en.TieneUsuarioEnviosEnAlerta(usr.ID);

                string direccionPantallaInicio = "Notificaciones/Notificaciones";
                if (tieneEnviosEnAlerta)
                {
                    direccionPantallaInicio = "Notificaciones/NotificacionesEstado?idEstado=2";
                }
                else
                {
                    PantallasInicio pip = new PantallasInicio();
                    PantallasInicio pantalla = pip.GetPantallaInicioPorID(usr.Perfiles.PantallasInicio.ID);
                    if (pantalla != null)
                        direccionPantallaInicio = pantalla.Direccion;
                }

                EnviarCorreoAceptarTerminosLegales(usr.ID);

                JsonResult resultado = Json(new
                {
                    Success = "True",
                    pantallaInicioPerfil = direccionPantallaInicio,
                }, JsonRequestBehavior.AllowGet);

                return resultado;
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return RedirectToAction("Login", "Home");
            }
        }

        private void EnviarCorreoAceptarTerminosLegales(int idUsuario)
        {
            JsonResult resultado;
            try
            {
                Usuarios usr = new Usuarios();
                Usuarios usuarioAceptado = usr.GetUsuarioPorId(idUsuario);

                string path = Server.MapPath("~") + @"pdf/TerminosLegales.pdf";

                string asunto = "Gestión de notificaciones: Aceptación Terminos Legales";
                string body = "";

                using (StreamReader reader = new StreamReader(Server.MapPath("~/Plantillas/email_AceptarTerminosLegalesUsuario.html")))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{nombre}", usuarioAceptado.Nombre + " "
                    + (String.IsNullOrEmpty(usuarioAceptado.Apellidos) ? "" : usuarioAceptado.Apellidos));
                body = body.Replace("{fecha}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));

                Utils.SendMailAdjunto(usuarioAceptado.Email, asunto, body, path);

                /* Email a la asesoria jurídica */
                using (StreamReader reader = new StreamReader(Server.MapPath("~/Plantillas/email_AceptarTerminosLegalesAJ.html")))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{nombre}", usuarioAceptado.Nombre);
                body = body.Replace("{dni}", usuarioAceptado.NIF);
                body = body.Replace("{usuario}", usuarioAceptado.LoginUsuario);
                body = body.Replace("{fecha}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));

                Utils.SendMailAdjunto(ConfigurationManager.AppSettings["emailAreaJuridica"], asunto, body, path);

                resultado = Json(new { Success = "True" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
            }
        }

        [HttpGet]
        public ActionResult GetDetalleGrupo(int id)
        {
            Grupos grupo;

            try
            {
                Grupos g = new Grupos();
                grupo = g.GetGrupoById(id);
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
        public ActionResult CargarUsuarioPorGrupos(int id)
        {
            List<GruposUsuarios> listaUsuarios;

            try
            {
                GruposUsuarios us = new GruposUsuarios();
                listaUsuarios = us.GetUsuarioPorGrupo(id);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(listaUsuarios, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CargarGruposPorUsuario(int id)
        {
            List<GruposUsuarios> listaUsuarios;

            try
            {
                GruposUsuarios us = new GruposUsuarios();
                listaUsuarios = us.GetGrupoPorUsuario(id);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(listaUsuarios, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CargarGruposPorUsuarioPerfil()
        {
            List<GruposUsuarios> listaUsuarios;

            try
            {
                GruposUsuarios us = new GruposUsuarios();
                listaUsuarios = us.GetGrupoPorUsuario(GetSessionUsuarioID());
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(listaUsuarios, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CargarOrganismoPorId(int id)
        {
            List<GruposOrganismosEmisores> listaOrganismos;

            try
            {
                GruposOrganismosEmisores org = new GruposOrganismosEmisores();
                listaOrganismos = org.GetOrganismosPorIdGrupo(id);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(listaOrganismos, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult InsertarEditarGrupo(int id, string nombre, string descripcion)
        {
            Grupos grupo = new Grupos();
            try
            {
                if (id != -1)
                {
                    grupo.UpdateGrupos(id, nombre, true, descripcion);
                }
                else
                {
                    grupo.InsertGrupos(nombre, true, descripcion);
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
        public ActionResult SetActivoGrupo(int id, bool activo)
        {
            Grupos grupo = new Grupos();
            try
            {
                grupo.SetActivo(id, activo);
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
        public ActionResult SetActivoUsuario(int id, bool activo)
        {
            Usuarios usr = new Usuarios();
            try
            {
                usr.SetActivo(id, activo);
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
        public ActionResult AsignarUsuarioGrupo(int idUsu, int idGrupo)
        {
            try
            {
                GruposUsuarios us = new GruposUsuarios();
                bool resultado = us.InsertUsuarioGrupo(idUsu, idGrupo);
                if (resultado == false)
                {
                    return Json(new { Success = "False", responseText = "Ya existe" }, JsonRequestBehavior.AllowGet);
                }
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
        public ActionResult EliminarUsuarioGrupo(int idUsuGrupo)
        {
            try
            {
                GruposUsuarios us = new GruposUsuarios();
                us.DeleteUsuarioGrupo(idUsuGrupo);
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
        public ActionResult EliminarOrganismoGrupo(int idOrgGrupo)
        {
            try
            {
                GruposOrganismosEmisores org = new GruposOrganismosEmisores();

                GruposOrganismosEmisores grOrg = org.GetGrupoOrganismoEmisor(idOrgGrupo);
                int idOrganismo = grOrg.OrganismosEmisores.ID;
                int idGrupo = grOrg.Grupos.ID;

                org.DeleteOrganismoGrupo(idOrgGrupo);

                Envios env = new Envios();
                List<Envios> listaEnvios = env.GetEnviosByIdOrganismo(idOrganismo);
                foreach (Envios e in listaEnvios)
                {
                    string cadena = AsignarGrupoEnvioDesdeAdmin(e.Identificador, idGrupo, false);
                }

                org.SetGruposAsignadosCadena(idOrganismo);
                //ActualizaJsonOrganimosTarea();
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
        public ActionResult AsignarOrgGrupo(int idOrg, int idGrupo)
        {
            try
            {
                GruposOrganismosEmisores goe = new GruposOrganismosEmisores();
                bool resultado = goe.InsertOrgGrupo(idOrg, idGrupo);

                Envios env = new Envios();
                List<Envios> listaEnvios = env.GetEnviosByIdOrganismo(idOrg);
                foreach (Envios e in listaEnvios)
                {
                    string cadena = AsignarGrupoEnvioDesdeAdmin(e.Identificador, idGrupo, true);
                }

                goe.SetGruposAsignadosCadena(idOrg);
                //ActualizaJsonOrganimosTarea();

                if (resultado == false)
                {
                    return Json(new { Success = "False", responseText = "Ya existe" }, JsonRequestBehavior.AllowGet);
                }
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
        [OutputCache(Duration = 1000, VaryByParam = "none")]
        public ActionResult GetAdministracionPublica()
        {
            List<string> listaTiposAdministracion;
            try
            {
                OrganismosEmisores oe = new OrganismosEmisores();
                listaTiposAdministracion = oe.GetAdministracionPublica();
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(listaTiposAdministracion, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetOrganismosRaiz(string administracionPublica)
        {
            List<string> listaOrganismosRaiz;
            try
            {
                OrganismosEmisores oe = new OrganismosEmisores();
                listaOrganismosRaiz = oe.GetOrganismosRaiz(administracionPublica);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(listaOrganismosRaiz, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [OutputCache(Duration = 1000, VaryByParam = "none")]
        public ActionResult GetComunidadesAutonomas()
        {
            List<ComunidadesAutonomas> listaCCAA;

            try
            {
                ComunidadesAutonomas CCAA = new ComunidadesAutonomas();
                listaCCAA = CCAA.GetCCAA();
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(listaCCAA, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [OutputCache(Duration = 1000, VaryByParam = "none")]
        public ActionResult GetProvincias()
        {
            List<Provincias> listaProvincias;

            try
            {
                Provincias provincias = new Provincias();
                listaProvincias = provincias.GetProvincias();
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(listaProvincias, JsonRequestBehavior.AllowGet);
        }
    }
}