using PSOENotificaciones.Contexto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace PSOENotificaciones.Controllers
{
    public class DocumentosExternosController : BaseController
    {
        [HttpPost]
        public void UploadFile()
        {
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var httpPostedFile = System.Web.HttpContext.Current.Request.Files["UploadedFile"];

                if (httpPostedFile != null)
                {
                    byte[] thePictureAsBytes = new byte[httpPostedFile.ContentLength];
                    using (BinaryReader theReader = new BinaryReader(httpPostedFile.InputStream))
                    {
                        thePictureAsBytes = theReader.ReadBytes(httpPostedFile.ContentLength);
                    }
                    string codeBase64 = Convert.ToBase64String(thePictureAsBytes);
                    System.IO.File.WriteAllText(Server.MapPath("~") + @"txt\codeBase64.txt", codeBase64);
                }
            }
        }

        [HttpGet]
        public ActionResult InsertDocumentoExterno(string identificador, string typeMime, string nombre, string descripcion, int tipo)
        {
            try
            {
                DateTime fecha = DateTime.Now;
                int idUsuario = GetSessionUsuarioID();
                string codeBase64 = System.IO.File.ReadAllText(Server.MapPath("~") + @"txt\codeBase64.txt");
                TipoDocumentoExterno tipoDoc = (TipoDocumentoExterno)tipo;

                DocumentosExternos docExt = new DocumentosExternos();
                docExt.InsertDocumentoExterno(identificador, fecha, idUsuario, codeBase64, typeMime, nombre, descripcion, tipoDoc);

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
        public ActionResult GetTiposDocumentosExternos()
        {
            try
            {
                TiposDocumentosExternos docExt = new TiposDocumentosExternos();
                List<TiposDocumentosExternos>  lista = docExt.GetTiposDocumentosExternos();

                return Json(lista, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetDocumentosExternos(string identificador)
        {
            try
            {
                DocumentosExternos docExt = new DocumentosExternos();
                List<DocumentosExternos> lista = docExt.GetDocumentosExternosByIdentificador(identificador);

                //return Json(lista, JsonRequestBehavior.AllowGet);
                return new LargeJsonResult { Data = lista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, GetSessionUsuarioID());
                return Json(new { Success = "False", responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}