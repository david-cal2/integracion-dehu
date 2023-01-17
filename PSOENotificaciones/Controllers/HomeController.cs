using PSOENotificaciones.Servicios;
using PSOENotificaciones.Contexto;
using PSOENotificaciones.Helpers;
using System;
using System.Data;
using System.Net;
using System.Web.Mvc;
using System.IO;
using System.Collections.Generic;
using System.Web;
using System.Configuration;

namespace PSOENotificaciones.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Login()
        {
            Parametros par = new Parametros();
            string valor = par.GetParametroById((int)Parametro.TiempoEsperaLogin);

            ViewBag.TiempoEsperaLogin = valor;

            return View();
        }

        public ActionResult LoginNuevaPass(string d)
        {
            if (!String.IsNullOrEmpty(d))
            {
                Usuarios usr = new Usuarios();
                Usuarios u = usr.GetUsuarioPorClaveRecuperaPassword(d);
                if (u == null)
                    return RedirectToAction("Login", "Home");
                else
                {
                    ViewBag.IdUsuario = u.ID;
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult LoginRecupera()
        {
            return View();
        }

        public ActionResult AvisoLegal()
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
        public ActionResult ValidarLogin(string usuario, string password)
        {
            JsonResult resultado;
            try
            {
                string hostName = Dns.GetHostName();
                string clientIP = GetIPAddress();

                PsoeAdministracionServicio objAdmin = new PsoeAdministracionServicio();
                DataTable dt = objAdmin.LoginUsuario(usuario, password, hostName, clientIP);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Columns.Count > 3)
                    {
                        //Login correcto
                        Usuarios usr = new Usuarios();
                        DataRow row = dt.Rows[0];

                        Usuarios usuario2 = new Usuarios();
                        Usuarios usr2 = usuario2.GetUsuarioPorId(Convert.ToInt32(row[0].ToString()));

                        try
                        {
                            usr.ID = Convert.ToInt32(row[0].ToString());
                            usr.LoginUsuario = row[1].ToString();
                            usr.Email = row[2].ToString();
                            usr.Nombre = usr2.Nombre;
                            usr.Apellidos = (String.IsNullOrEmpty(usr2.Apellidos) ? "" : usr2.Apellidos);
                            usr.Perfiles = new Perfiles()
                            {
                                ID = Convert.ToInt32(row[3].ToString()),
                                PantallasInicio = new PantallasInicio()
                                {
                                    ID = Convert.ToInt32(row[4].ToString())
                                }
                            };
                            usr.ConsentimientoLegal = row[5] != null && Convert.ToBoolean(row[5]);
                        }
                        catch (Exception ex)
                        {
                            Session["usuario"] = null;
                            HistorialExcepciones his = new HistorialExcepciones();
                            his.InsertExcepcion("0: " + row[0].ToString() + "; 1: " + row[1].ToString() + "; 2: " + row[2].ToString() + "; 3: " + row[3].ToString() + "; 4: " + row[4].ToString() + "; 5: " + row[5].ToString() + "; " + ex.Message, ex.StackTrace, GetSessionUsuarioID());
                        }

                        Envios en = new Envios();
                        bool tieneEnviosEnAlerta = en.TieneUsuarioEnviosEnAlerta(usr.ID);

                        string direccionPantallaInicio = "Notificaciones/Notificaciones";
                        if (tieneEnviosEnAlerta && usr.Perfiles.ID != (int)Perfil.Asignador)
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

                        Session["usuario"] = usr;

                        resultado = Json(new
                        {
                            id = usr.ID.ToString(),
                            pantallaInicioPerfil = direccionPantallaInicio,
                            consentimientoLegal = usr.ConsentimientoLegal
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Session["usuario"] = null;

                        DataRow row = dt.Rows[0];
                        int idMensajeError = int.Parse(row[0].ToString());
                        string mensajeError = row[1].ToString();
                        int idUsuarioError = (row[2] != null ? int.Parse(row[2].ToString()) : 0);

                        resultado = Json(new
                        {
                            idMensajeLoginIncorrecto = idMensajeError,
                            mensajeLoginIncorrecto = mensajeError,
                            idUsuario = idUsuarioError
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    //HistorialExcepciones his = new HistorialExcepciones();
                    //his.InsertExcepcion(mensaje, "Ejecución del procedimiento 'getLoginUsuario'", GetSessionUsuarioID());
                    resultado = Json(new
                    {
                        Success = "False",
                        mensajeLogin = "Login Erroneo"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Session["usuario"] = null;
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                resultado = Json(new
                {
                    Success = "False",
                    mensajeExcepcion = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }

            return resultado;
        }

        [HttpGet]
        public ActionResult UpdateUsuarioPassword(int idUsuario, string password)
        {
            JsonResult resultado;
            try
            {
                PsoeAdministracionServicio objAdmin = new PsoeAdministracionServicio();
                objAdmin.UpdateUsuarioPassword(idUsuario, password);
                resultado = Json(new
                {
                    Success = "True",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Session["usuario"] = null;
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                resultado = Json(new
                {
                    Success = "False",
                    mensajeExcepcion = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }

            return resultado;
        }

        [HttpGet]
        public ActionResult UpdatePasswordPerfil(string passwordActual, string passwordNueva)
        {
            JsonResult resultado;
            try
            {
                Usuarios usr = GetSessionUsuario();

                PsoeAdministracionServicio objAdmin = new PsoeAdministracionServicio();
                string esIgualPassActual = objAdmin.GetComprobarPassActual(usr.ID, passwordActual);

                if (esIgualPassActual == "1")
                {
                    bool ok = objAdmin.UpdateUsuarioPassword(usr.ID, passwordNueva);
                    if (ok)
                        resultado = Json(new { Success = "True" }, JsonRequestBehavior.AllowGet);
                    else
                        resultado = Json(new { Success = "False", mensajeExcepcion = "Error en la llamada al procedimiento 'updateUsuarioPassword'" }, JsonRequestBehavior.AllowGet);
                }
                else
                    resultado = Json(new { Success = "False" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                resultado = Json(new { Success = "False", mensajeExcepcion = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return resultado;
        }

        [HttpGet]
        public ActionResult ComprobarDni(int idUsuario, string dni)
        {
            JsonResult resultado;
            try
            {
                Usuarios usr = new Usuarios();
                int res = usr.ComprobarDni(idUsuario, dni);

                if (res == -1)
                {
                    EnviarCorreoDniIncorrecto(idUsuario);
                }

                resultado = Json(new
                {
                    Success = "True",
                    ResultadoValidacion = res
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Session["usuario"] = null;
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                resultado = Json(new
                {
                    Success = "False",
                    mensajeExcepcion = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }

            return resultado;
        }

        #region envio emails
        [HttpGet]
        public ActionResult EnviarCorreoUsuarioNuevo(int idNuevoUsuario)
        {
            try
            {
                Usuarios usr = new Usuarios();
                Usuarios usuarioNuevo = usr.GetUsuarioPorId(idNuevoUsuario);

                string asunto = "Gestión de notificaciones: Alta de usuarios";
                string body = "";

                using (StreamReader reader = new StreamReader(Server.MapPath("~/Plantillas/email_nuevoUsuario.html")))
                {
                    body = reader.ReadToEnd();
                }

                string cadenaAleatoria = UpdateClaveRecuperarPassword(Convert.ToInt32(idNuevoUsuario), null);

                body = body.Replace("{nombre}", usuarioNuevo.Nombre + " "
                    + (String.IsNullOrEmpty(usuarioNuevo.Apellidos) ? "" : usuarioNuevo.Apellidos));
                body = body.Replace("{usuario}", usuarioNuevo.LoginUsuario);
                //body = body.Replace("{fecha}", DateTime.Now.ToString("dd/MM/yyyy"));

                string ip = GetIPAddress();
                string host = Request.Url.Host;
                int puerto = Request.Url.Port;
                //string direccion = "http://" + host + ":" + puerto;
                string direccion = "http://" + ip + ":" + puerto;
                body = body.Replace("{enlace}", direccion + "/Home/LoginNuevaPass?d=" + cadenaAleatoria);

                Utils.SendMail(usuarioNuevo.Email, asunto, body);
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
        public ActionResult EnviarCorreoRecuperarPassword(string email)
        {
            JsonResult resultado;
            try
            {
                string cadenaAleatoria = UpdateClaveRecuperarPassword(0, email);

                if (cadenaAleatoria != string.Empty)
                {
                    string asunto = "Gestión de notificaciones: Recuperar contraseña";
                    string body = "";

                    using (StreamReader reader = new StreamReader(Server.MapPath("~/Plantillas/email_recuperarPassword.html")))
                    {
                        body = reader.ReadToEnd();
                    }

                    string ip = GetIPAddress();
                    string host = Request.Url.Host;
                    int puerto = Request.Url.Port;
                    //string direccion = "http://" + host + ":" + puerto;
                    string direccion = "http://" + ip + ":" + puerto;
                    body = body.Replace("{url}", direccion + "/Home/LoginNuevaPass?d=" + cadenaAleatoria);

                    Utils.SendMail(email, asunto, body);
                    resultado = Json(new { Success = "True" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    resultado = Json(new { Success = "False" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                resultado = Json(new { Success = "False", mensajeExcepcion = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return resultado;
        }

        [HttpGet]
        public ActionResult EnviarCorreoUsuarioBloqueado(int idUsuario)
        {
            try
            {
                Usuarios usr = new Usuarios();
                Usuarios usuarioBloqueado = usr.GetUsuarioPorId(idUsuario);

                string asunto = "Gestión de notificaciones: Bloqueo de usuarios";
                string body = "";

                using (StreamReader reader = new StreamReader(Server.MapPath("~/Plantillas/email_usuarioBloqueado.html")))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{usuario}", usuarioBloqueado.LoginUsuario);
                body = body.Replace("{correo}", usuarioBloqueado.Email);
                body = body.Replace("{telefono}", usuarioBloqueado.Telefono);
                body = body.Replace("{fecha}", DateTime.Now.ToString("dd/MM/yyyy"));
                body = body.Replace("{nombreCompleto}", usuarioBloqueado.Nombre + " " + usuarioBloqueado.Apellidos);

                AvisosNotificacion avi = new AvisosNotificacion();
                List<AvisosNotificacion> listaAvisos = avi.GetAvisos((int)TipoEmail.BloqueoUsuario);
                foreach (AvisosNotificacion a in listaAvisos)
                {
                    Utils.SendMail(a.Email, asunto, body);
                }

                Utils.SendMail(usuarioBloqueado.Email, asunto, body);
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
        public ActionResult EnviarCorreoDesbloqueoUsuario(int idUsuario)
        {
            JsonResult resultado;
            try
            {
                string cadenaAleatoria = UpdateClaveRecuperarPassword(idUsuario, null);

                if (cadenaAleatoria != string.Empty)
                {
                    Usuarios usr = new Usuarios();
                    Usuarios usuarioDesbloqueado = usr.GetUsuarioPorId(idUsuario);

                    string asunto = "Gestión de notificaciones: Desbloqueo de usuarios";
                    string body = "";

                    using (StreamReader reader = new StreamReader(Server.MapPath("~/Plantillas/email_desbloqueoUsuario.html")))
                    {
                        body = reader.ReadToEnd();
                    }

                    body = body.Replace("{usuario}", usuarioDesbloqueado.LoginUsuario);
                    body = body.Replace("{fecha}", DateTime.Now.ToString("dd/MM/yyyy"));

                    string ip = GetIPAddress();
                    string host = Request.Url.Host;
                    int puerto = Request.Url.Port;
                    //string direccion = "http://" + host + ":" + puerto;
                    string direccion = "http://" + ip + ":" + puerto;
                    body = body.Replace("{enlace}", direccion + "/Home/LoginNuevaPass?d=" + cadenaAleatoria);

                    Utils.SendMail(usuarioDesbloqueado.Email, asunto, body);
                    resultado = Json(new { Success = "True" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    resultado = Json(new { Success = "False" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                resultado = Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return resultado;
        }

        [HttpGet]
        public ActionResult EnviarCorreoDniIncorrecto(int idUsuario)
        {
            JsonResult resultado;
            try
            {
                Usuarios usr = new Usuarios();
                Usuarios usuarioBloqueado = usr.GetUsuarioPorId(idUsuario);

                string asunto = "Gestión de notificaciones: Validación DNI incorrecta";
                string body = "";

                using (StreamReader reader = new StreamReader(Server.MapPath("~/Plantillas/email_dniIncorrecto.html")))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{usuario}", usuarioBloqueado.LoginUsuario);
                body = body.Replace("{dni}", usuarioBloqueado.NIF);

                Utils.SendMail(ConfigurationManager.AppSettings["emailAreaJuridica"], asunto, body);
                resultado = Json(new { Success = "True" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                resultado = Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return resultado;
        }
        #endregion

        [HttpGet]
        [OutputCache(Duration = 1000, VaryByParam = "none")]
        public ActionResult GetInfoUsuario()
        {
            JsonResult resultado;
            Usuarios usr = GetSessionUsuario();

            if (usr != null)
            {
                resultado = Json(new
                {
                    Success = "True",
                    login = usr.LoginUsuario,
                    email = usr.Email,
                    siglas = (String.IsNullOrEmpty(usr.Apellidos) ? usr.Nombre.Substring(0, 2) : usr.Nombre.Substring(0, 1) + usr.Apellidos.Substring(0, 1))
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                resultado = Json(new
                {
                    Success = "False"
                }, JsonRequestBehavior.AllowGet);
            }

            return resultado;
        }

        [HttpGet]
        public ActionResult CerrarSesion()
        {
            Session["ListaCantidadesNotificaciones"] = null;
            Session["ListaCantidadesComunicaciones"] = null;
            Session["usuario"] = null;

            var urlToRemove = Url.Action("GetInfoUsuario", "Home");
            HttpResponse.RemoveOutputCacheItem(urlToRemove);

            JsonResult resultado = Json(new
            {
                Success = "True",
            }, JsonRequestBehavior.AllowGet);

            return resultado;
        }

        [HttpGet]
        public ActionResult GetUsuarioDetalle()
        {

            Usuarios usu = (Usuarios)Session["usuario"];
            try
            {
                Usuarios usr = new Usuarios();
                usu = usr.GetUsuarioPorId(usu.ID);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(usu, JsonRequestBehavior.AllowGet);
        }

        private string GetCadenaAleatoria()
        {
            var characters = "abcdefghijklmnopqrstuvwxyz0123456789";
            var Charsarr = new char[20];
            var random = new Random();

            for (int i = 0; i < Charsarr.Length; i++)
            {
                Charsarr[i] = characters[random.Next(characters.Length)];
            }

            var resultString = new String(Charsarr);
            return resultString;
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

        private string UpdateClaveRecuperarPassword(int idUser, string email)
        {
            string cadenaAleatoria = GetCadenaAleatoria();
            Usuarios usr = new Usuarios();
            bool existe;

            if (email != null)
            {
                existe = usr.UpdateClaveRecuperarPassword(email.Trim(), cadenaAleatoria);
            }
            else
            {
                existe = usr.UpdateClaveRecuperarPassword(idUser, cadenaAleatoria);
            }

            if (!existe)
            {
                cadenaAleatoria = string.Empty;
            }
            return cadenaAleatoria;
        }
    }
}