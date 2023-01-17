using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Validation;
using System.Linq;

namespace PSOENotificaciones.Contexto
{
    [Table("RespuestasAcceso")]
    public partial class RespuestasAcceso : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private string enviosIdentificadorField;
        private int peticionesAccesoIdField;
        private DateTime fechaField;
        private string codigoRespuestaField;
        private string descripcionRespuestaField;
        private int codigoOrigenField;
        private List<Opcion3> opcionesRespuestaAccesoField;
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

        public string Envios_Identificador
        {
            get
            {
                return this.enviosIdentificadorField;
            }
            set
            {
                this.enviosIdentificadorField = value;
                this.RaisePropertyChanged("enviosIdentificadorField");
            }
        }

        public int PeticionesAcceso_ID
        {
            get
            {
                return this.peticionesAccesoIdField;
            }
            set
            {
                this.peticionesAccesoIdField = value;
            }
        }

        public DateTime Fecha
        {
            get
            {
                return this.fechaField;
            }
            set
            {
                this.fechaField = value;
                this.RaisePropertyChanged("fecha");
            }
        }

        [MaxLength(4)]
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

        public int CodigoOrigen
        {
            get
            {
                return this.codigoOrigenField;
            }
            set
            {
                this.codigoOrigenField = value;
                this.RaisePropertyChanged("codigoOrigen");
            }
        }

        public List<Opcion3> OpcionesRespuestaAcceso
        {
            get
            {
                return this.opcionesRespuestaAccesoField;
            }
            set
            {
                this.opcionesRespuestaAccesoField = value;
                this.RaisePropertyChanged("opcionesRespuestaAccesoField");
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

        public int InsertRespuestaAccesoContext(string identificador, int idPeticionAcceso, DateTime fecha, string codigoRespuesta, string descripcionRespuesta,
            int codigoOrigen, string xmlRespuesta, GestNotifContext db)
        {
            int idRespuesta = 0;

            try
            {
                RespuestasAcceso ra = new RespuestasAcceso
                {
                    Envios_Identificador = identificador,
                    PeticionesAcceso_ID = idPeticionAcceso,
                    Fecha = fecha,
                    CodigoRespuesta = codigoRespuesta,
                    DescripcionRespuesta = descripcionRespuesta,
                    CodigoOrigen = codigoOrigen,
                    NombreXml = xmlRespuesta
                };

                db.RespuestasAcceso.Add(ra);
                db.SaveChanges();

                idRespuesta = ra.ID;
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcionContext("1" + ex.Message, ex.StackTrace, 0, db);
            }

            return idRespuesta;
        }

        public RespuestasAcceso GetRespuestaAccesoPorIdentificador(string identificador)
        {
            using (var db = new GestNotifContext())
            {
                return db.RespuestasAcceso
                    .Where(i => i.Envios_Identificador == identificador)
                    .OrderByDescending(i => i.Fecha).FirstOrDefault();
            }
        }

        public RespuestasAcceso GetRespuestaAccesoPorIdPeticion(int idPeticion)
        {
            using (var db = new GestNotifContext())
            {
                return db.RespuestasAcceso
                    .Where(i => i.PeticionesAcceso_ID == idPeticion)
                    .OrderByDescending(i => i.Fecha).FirstOrDefault();
            }
        }

        public RespuestasAcceso GetRespuestaAccesoContext(string identificador, GestNotifContext db)
        {
            return db.RespuestasAcceso
                .Where(i => i.Envios_Identificador == identificador)
                .FirstOrDefault();
        }

        public RespuestasAcceso GetRespuestaAccesoOK(string identificador)
        {
            using (var db = new GestNotifContext())
            {
                return db.RespuestasAcceso
                    .Where(i => i.Envios_Identificador == identificador && i.CodigoRespuesta == "200")
                    .FirstOrDefault();
            }
        }

        public RespuestasAcceso GetRespuestaAccesoOKContext(string identificador, GestNotifContext db)
        {
            RespuestasAcceso respuestaAcceso = null;

            respuestaAcceso = db.RespuestasAcceso
                .Where(i => i.Envios_Identificador == identificador && i.CodigoRespuesta == "200")
                .FirstOrDefault();

            return respuestaAcceso;
        }
    }

    [Table("DetalleDocumentos")]
    public partial class DetalleDocumentos : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private int respuestasAccesoFieldId;
        private string nombreField;
        private string mimeTypeField;
        private string metadatosField;
        private string enlaceDocumentoField;
        private string referenciaDocumentoField;
        private string referenciaPdfAcuseField;
        private string csvResguardoField;

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

        public int RespuestasAcceso_ID
        {
            get
            {
                return this.respuestasAccesoFieldId;
            }
            set
            {
                this.respuestasAccesoFieldId = value;
                this.RaisePropertyChanged("idField");
            }
        }

        [MaxLength(100)]
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

        [MaxLength(255)]
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

        [MaxLength(255)]
        public string EnlaceDocumento
        {
            get
            {
                return this.enlaceDocumentoField;
            }
            set
            {
                this.enlaceDocumentoField = value;
                this.RaisePropertyChanged("enlaceDocumento");
            }
        }

        public string ReferenciaDocumento
        {
            get
            {
                return this.referenciaDocumentoField;
            }
            set
            {
                this.referenciaDocumentoField = value;
                this.RaisePropertyChanged("referenciaDocumento");
            }
        }

        public string ReferenciaPdfAcuse
        {
            get
            {
                return this.referenciaPdfAcuseField;
            }
            set
            {
                this.referenciaPdfAcuseField = value;
                this.RaisePropertyChanged("referenciaPdfAcuse");
            }
        }

        public string CsvResguardo
        {
            get
            {
                return this.csvResguardoField;
            }
            set
            {
                this.csvResguardoField = value;
                this.RaisePropertyChanged("csvResguardo");
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

        public int InsertDetalleDocumentoContext(GestNotifContext db, int idRespuestaAcceso, string nombre = null, string mimeType = null, string metadatos = null,
            string enlaceDocumento = null, string referenciaDocumento = null, string referenciaPdfAcuse = null, string csvResguardo = null)
        {
            int idDetalleDocumento = 0;

            try
            {
                DetalleDocumentos nuevoDetalle = new DetalleDocumentos
                {
                    RespuestasAcceso_ID = idRespuestaAcceso,
                    Nombre = nombre,
                    MimeType = mimeType,
                    Metadatos = metadatos,
                    EnlaceDocumento = enlaceDocumento,
                    ReferenciaDocumento = referenciaDocumento,
                    ReferenciaPdfAcuse = referenciaPdfAcuse,
                    CsvResguardo = csvResguardo
                };

                db.DetalleDocumentos.Add(nuevoDetalle);
                db.SaveChanges();

                idDetalleDocumento = nuevoDetalle.ID;
            }
            catch (DbEntityValidationException ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcionContext(ex.Message, ex.StackTrace, 0, db);
            }

            return idDetalleDocumento;
        }

        public DetalleDocumentos GetDetalleDocumento(int idRespuestaAcceso)
        {
            DetalleDocumentos detalleDocumento = null;

            using (var db = new GestNotifContext())
            {
                detalleDocumento = db.DetalleDocumentos.Where(i => i.RespuestasAcceso_ID == idRespuestaAcceso).FirstOrDefault();
            }

            return detalleDocumento;
        }
    }

    [Table("HashDocumentos")]
    public partial class HashDocumento : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private int detalleDocumentosFieldId;
        private string hashField;
        private string algoritmoHashField;

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

        public int DetalleDocumentos_ID
        {
            get
            {
                return this.detalleDocumentosFieldId;
            }
            set
            {
                this.detalleDocumentosFieldId = value;
            }
        }

        public string Hash
        {
            get
            {
                return this.hashField;
            }
            set
            {
                this.hashField = value;
                this.RaisePropertyChanged("hash");
            }
        }

        public string AlgoritmoHash
        {
            get
            {
                return this.algoritmoHashField;
            }
            set
            {
                this.algoritmoHashField = value;
                this.RaisePropertyChanged("algoritmoHash");
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

        public int InsertHashDocumento(int idDetalleDocumento, string hash, string algoritmoHash, GestNotifContext db = null)
        {
            int idHashDocumento;

            HashDocumento hd = new HashDocumento
            {
                DetalleDocumentos_ID = idDetalleDocumento,
                Hash = hash,
                AlgoritmoHash = algoritmoHash
            };

            db.HashDocumento.Add(hd);
            db.SaveChanges();

            idHashDocumento = hd.ID;

            return idHashDocumento;
        }

        public HashDocumento GetHashDocumento(int idDocumento)
        {
            using (var db = new GestNotifContext())
            {
                return db.HashDocumento.Where(i => i.DetalleDocumentos_ID == idDocumento).FirstOrDefault();
            }
        }
    }

    [Table("AnexoReferencia")]
    public partial class AnexoReferencia
    {
        private int idField;
        private int respuestasAccesoIdField;
        private string nombreField;
        private string mimeTypeField;
        private string referenciaDocumentoField;

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

        public int RespuestasAcceso_ID
        {
            get
            {
                return this.respuestasAccesoIdField;
            }
            set
            {
                this.respuestasAccesoIdField = value;
                this.RaisePropertyChanged("respuestasAccesoIdField");
            }
        }

        [MaxLength(100)]
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

        [MaxLength(10)]
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

        public string ReferenciaDocumento
        {
            get
            {
                return this.referenciaDocumentoField;
            }
            set
            {
                this.referenciaDocumentoField = value;
                this.RaisePropertyChanged("referenciaDocumento");
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

        public int InsertAnexoReferencia(int RespuestasAccesoId, string nombre, string mimeType, string referenciaDocumento, GestNotifContext db)
        {
            int idAnexoReferencia = 0;

            try
            {
                AnexoReferencia ar = new AnexoReferencia
                {
                    RespuestasAcceso_ID = RespuestasAccesoId,
                    ReferenciaDocumento = referenciaDocumento,
                    Nombre = nombre,
                    MimeType = mimeType
                };

                db.AnexoReferencia.Add(ar);
                db.SaveChanges();

                idAnexoReferencia = ar.ID;
            }
            catch (DbEntityValidationException ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcionContext(ex.Message, ex.StackTrace, 0, db);
            }

            return idAnexoReferencia;
        }

        public List<AnexoReferencia> GetAnexosReferencia(int idRespuestaAcceso)
        {
            List<AnexoReferencia> lista = null;

            using (var db = new GestNotifContext())
            {
                lista = db.AnexoReferencia.Where(i => i.RespuestasAcceso_ID == idRespuestaAcceso).ToList();
            }

            return lista;
        }

        public List<AnexoReferencia> GetAnexosReferenciaContext(int idRespuestaAcceso, GestNotifContext db)
        {
            List<AnexoReferencia> lista = db.AnexoReferencia
                .Where(i => i.RespuestasAcceso_ID == idRespuestaAcceso)
                .ToList();

            return lista;
        }
    }

    [Table("AnexoUrl")]
    public partial class AnexoUrl
    {
        private int idField;
        private int RespuestasAcceso_IDField;
        private string nombreField;
        private string mimeTypeField;
        private string enlaceDocumentoField;

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

        public int RespuestasAcceso_ID
        {
            get
            {
                return this.RespuestasAcceso_IDField;
            }
            set
            {
                this.RespuestasAcceso_IDField = value;
                this.RaisePropertyChanged("RespuestasAcceso_ID");
            }
        }

        [MaxLength(255)]
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

        [MaxLength(255)]
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

        public string EnlaceDocumento
        {
            get
            {
                return this.enlaceDocumentoField;
            }
            set
            {
                this.enlaceDocumentoField = value;
                this.RaisePropertyChanged("enlaceDocumento");
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

        public int InsertAnexoUrl(int RespuestasAccesoId, string nombre, string mimeType, string enlaceDocumento, GestNotifContext db = null)
        {
            int idAnexoUrl = 0;

            try
            {
                AnexoUrl au = new AnexoUrl
                {
                    RespuestasAcceso_ID = RespuestasAccesoId,
                    Nombre = nombre,
                    MimeType = mimeType,
                    EnlaceDocumento = enlaceDocumento
                };

                db.AnexoUrl.Add(au);
                db.SaveChanges();

                idAnexoUrl = au.ID;
            }
            catch (DbEntityValidationException ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcionContext(ex.Message, ex.StackTrace, 0, db);
            }

            return idAnexoUrl;
        }

        public List<AnexoUrl> GetAnexosUrl(int idRespuestaAcceso)
        {
            List<AnexoUrl> lista = null;

            using (var db = new GestNotifContext())
            {
                lista = db.AnexoUrl
                    .Where(i => i.RespuestasAcceso_ID == idRespuestaAcceso)
                    .ToList();
            }

            return lista;
        }
    }
}
