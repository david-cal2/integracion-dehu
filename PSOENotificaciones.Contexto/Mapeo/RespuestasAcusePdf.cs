using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PSOENotificaciones.Contexto
{
    [Table("RespuestasAcusePdf")]
    public partial class RespuestasAcusePdf : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private DateTime fechaFiled;
        private int PeticionAccusePdfIdField;
        private string codigoRespuestaField;
        private string descripcionRespuestaField;
        private List<Opcion7> opcionesRespuestaConsultaAcusePdfField;
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

        public int PeticionesAcusePdf_ID
        {
            get
            {
                return this.PeticionAccusePdfIdField;
            }
            set
            {
                this.PeticionAccusePdfIdField = value;
                this.RaisePropertyChanged("PeticionAccusePdfIdField");
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

        [MaxLength(100)]
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

        [System.Xml.Serialization.XmlArrayItemAttribute("opcion", IsNullable = false)]
        public List<Opcion7> OpcionesRespuestaConsultaAcusePdf
        {
            get
            {
                return this.opcionesRespuestaConsultaAcusePdfField;
            }
            set
            {
                this.opcionesRespuestaConsultaAcusePdfField = value;
                this.RaisePropertyChanged("opcionesRespuestaConsultaAcusePdf");
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

        public int InsertRespuestasAcusePdf(DateTime Fecha, int PeticionAccusePdfId,
            string CodigoRespuesta, string DescripcionRespuesta, string NombreXml, GestNotifContext db = null)
        {
            RespuestasAcusePdf respuestasAcusePdf = new RespuestasAcusePdf
            {
                PeticionesAcusePdf_ID = PeticionAccusePdfId,
                CodigoRespuesta = CodigoRespuesta,
                DescripcionRespuesta = DescripcionRespuesta,
                Fecha = Fecha,
                NombreXml = NombreXml
            };

            db.RespuestasAcusePdf.Add(respuestasAcusePdf);

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return respuestasAcusePdf.ID;
        }

        public RespuestasAcusePdf GetRespuestasAcusePdf(int idPeticionAcusePdf)
        {
            RespuestasAcusePdf respuestasAcusePdf = null;

            using (var db = new GestNotifContext())
            {
                respuestasAcusePdf = db.RespuestasAcusePdf
                    .Where(i => i.PeticionesAcusePdf_ID == idPeticionAcusePdf).FirstOrDefault();
            }

            return respuestasAcusePdf;
        }
    }

    [Table("AcusesPdf")]
    public partial class AcusesPdf : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private int respuestasAcusePdfFieldID;
        private string nombreAcuseField;
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

        public int RespuestasAcusePdf_ID//respuestasAcusePdfID
        {
            get
            {
                return this.respuestasAcusePdfFieldID;
            }
            set
            {
                this.respuestasAcusePdfFieldID = value;
                this.RaisePropertyChanged("respuestasAcusePdfFieldID");
            }
        }

        [MaxLength(100)]
        public string NombreAcuse
        {
            get
            {
                return this.nombreAcuseField;
            }
            set
            {
                this.nombreAcuseField = value;
                this.RaisePropertyChanged("nombreAcuseField");
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
                this.RaisePropertyChanged("mimeTypeField");
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
                this.RaisePropertyChanged("metadatosField");
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

        public int InsertAcusesPdf(int respuestasAcusePdfId,
           string metadatos,
           string mimitype,
           string nombreaccuse,
           GestNotifContext db = null)

        {
            int idAcusesPdf;
            //db = new GestNotifContext();
            //using (var db1 = new GestNotifContext())
            {
                AcusesPdf acusesPdf = new AcusesPdf
                {
                    Metadatos = metadatos,
                    MimeType = mimitype,
                    NombreAcuse = nombreaccuse,
                    RespuestasAcusePdf_ID = respuestasAcusePdfId
                };

                db.AcusesPdf.Add(acusesPdf);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException);
                }

                idAcusesPdf = acusesPdf.ID;
            }

            return idAcusesPdf;
        }

        public AcusesPdf GetAcusesPdf(int id)
        {
            AcusesPdf acusesPdf = null;
            using (var db = new GestNotifContext())
            {
                acusesPdf = (from AcusesPdf in db.AcusesPdf
                             where AcusesPdf.idField == id
                             select AcusesPdf).FirstOrDefault();
            }

            return acusesPdf;
        }

        public AcusesPdf GetAcusesPdfPorIdRespuesta(int idRespuestaAcusePdf)
        {
            AcusesPdf acusePdf = null;

            using (var db = new GestNotifContext())
            {
                acusePdf = db.AcusesPdf.
                    Where(i => i.RespuestasAcusePdf_ID == idRespuestaAcusePdf).FirstOrDefault();
            }

            return acusePdf;
        }
    }
}
