using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PSOENotificaciones.Contexto
{
    [Table("RespuestasAnexo")]
    public partial class RespuestasAnexo : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private int peticionAnexoIdField;
        private DateTime fechaFiled;
        private string codigoRespuestaField;
        private string descripcionRespuestaField;
        private List<Opcion5> opcionesRespuestaConsultaAnexoField;
        private string nombreXml;

        [Key]
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
                this.RaisePropertyChanged("idField");
            }
        }

        public int PeticionesAnexo_ID
        {
            get
            {
                return this.peticionAnexoIdField;
            }
            set
            {
                this.peticionAnexoIdField = value;
                this.RaisePropertyChanged("peticionAnexoIdField");
            }
        }

        public DateTime Fecha
        {
            get
            {
                return this.fechaFiled;
            }
            set
            {
                this.fechaFiled = value;
                this.RaisePropertyChanged("fecha");
            }
        }

        public string CodigoRespuesta
        {
            get
            {
                return this.codigoRespuestaField;
            }
            set
            {
                this.codigoRespuestaField = value;
                this.RaisePropertyChanged("codigoRespuesta");
            }
        }

        public string DescripcionRespuesta
        {
            get
            {
                return this.descripcionRespuestaField;
            }
            set
            {
                this.descripcionRespuestaField = value;
                this.RaisePropertyChanged("descripcionRespuesta");
            }
        }

        public List<Opcion5> OpcionesRespuestaConsultaAnexo
        {
            get
            {
                return this.opcionesRespuestaConsultaAnexoField;
            }
            set
            {
                this.opcionesRespuestaConsultaAnexoField = value;
                this.RaisePropertyChanged("opcionesRespuestaConsultaAnexo");
            }
        }

        public string NombreXml
        {
            get
            {
                return this.nombreXml;
            }
            set
            {
                this.nombreXml = value;
                this.RaisePropertyChanged("nombreXml");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        public RespuestasAnexo GetRespuestaAnexo(int idPeticionAnexo)
        {
            RespuestasAnexo respuestasAnexo = null;

            using (var db = new GestNotifContext())
            {
                respuestasAnexo = db.RespuestasAnexo
                    .Where(i => i.PeticionesAnexo_ID == idPeticionAnexo).FirstOrDefault();
            }

            return respuestasAnexo;
        }
       

        public int InsertRespuestasAnexo(
           string CodigoRespuesta,
           string DescripcionRespuesta,
           DateTime fecha,
           string PeticionAnexoId,
           string NombreXml,
           List<Opcion5> OpcionesRespuestaConsultaAnexo,
           GestNotifContext db =null
           )
        {
            int idRespuestasAnexo;

            var PeticionAnexoId_ = Convert.ToInt32(PeticionAnexoId);
            //using (var db = new GestNotifContext())
            {
                RespuestasAnexo respuestasAnexo =new RespuestasAnexo()
                {
                   CodigoRespuesta = CodigoRespuesta,
                   DescripcionRespuesta  = DescripcionRespuesta,
                   Fecha  = fecha,
                    PeticionesAnexo_ID = PeticionAnexoId_,
                   NombreXml  = NombreXml,
                   OpcionesRespuestaConsultaAnexo  = OpcionesRespuestaConsultaAnexo
                };

                db.RespuestasAnexo.Add(respuestasAnexo);

                db.SaveChanges();

                idRespuestasAnexo = respuestasAnexo.ID;
            }

            return idRespuestasAnexo;
        }

    }

    [Table("DocumentosAnexo")]
    public partial class DocumentosAnexo : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private int respuestasAnexoIdField;
        private string nombreField;
        private string mimeTypeField;
        private string metadatosField;

        [Key]
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
                this.RaisePropertyChanged("idField");
            }
        }

        public int RespuestasAnexo_ID
        {
            get
            {
                return this.respuestasAnexoIdField;
            }
            set
            {
                this.respuestasAnexoIdField = value;
                this.RaisePropertyChanged("respuestasAnexoIdField");
            }
        }

        public string Nombre
        {
            get
            {
                return this.nombreField;
            }
            set
            {
                this.nombreField = value;
                this.RaisePropertyChanged("nombre");
            }
        }

        public string MimeType
        {
            get
            {
                return this.mimeTypeField;
            }
            set
            {
                this.mimeTypeField = value;
                this.RaisePropertyChanged("mimeType");
            }
        }

        public string Metadatos
        {
            get
            {
                return this.metadatosField;
            }
            set
            {
                this.metadatosField = value;
                this.RaisePropertyChanged("metadatos");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        public DocumentosAnexo GetDocumentoAnexo(int idRespuestaAnexo)
        {
            DocumentosAnexo docAnexo = null;

            using (var db = new GestNotifContext())
            {
                docAnexo = db.DocumentosAnexo
                    .Where(i => i.RespuestasAnexo_ID == idRespuestaAnexo).FirstOrDefault();
            }

            return docAnexo;
        }


        public int InsertDocumentoAnexo(
                string xmldocmetadatos,
                string xmldocmimeType,
                string xmldocnombreAcuse,
                int respuestasAnexo1ID,
                GestNotifContext db = null
           )
        {
            int idDocumentosAnexo;

            //using (var db = new GestNotifContext())
            {
                DocumentosAnexo documentoAnexo = new DocumentosAnexo()
                {
                    Metadatos = xmldocmetadatos,
                    MimeType = xmldocmimeType,
                    Nombre = xmldocnombreAcuse,
                    RespuestasAnexo_ID = respuestasAnexo1ID
                };

                db.DocumentosAnexo.Add(documentoAnexo);

                db.SaveChanges();

                idDocumentosAnexo = documentoAnexo.ID;
            }

            return idDocumentosAnexo;
        }


    }
}
