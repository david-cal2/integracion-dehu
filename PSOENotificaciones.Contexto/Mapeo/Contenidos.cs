using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PSOENotificaciones.Contexto
{
    //Contenido del documento
    [Table("Contenido")]
    public partial class Contenido : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private int detalleDocumentosFieldId;  // DetalleDocumentos detalleDocumentosField;
        private string hrefField;
        private string valueField;

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
                this.RaisePropertyChanged("detalleDocumentosFieldId");
            }
        }

        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
                this.RaisePropertyChanged("href");
            }
        }

        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
                this.RaisePropertyChanged("Value");
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

        public int InsertContenido(string href, string value, int detalleDocumentosId, GestNotifContext db = null)
        {
            Contenido contenido = new Contenido()
            {
                DetalleDocumentos_ID = detalleDocumentosId,
                Href = href,
                Value = value
            };

            db.Contenido.Add(contenido);
            db.SaveChanges();

            return contenido.ID;
        }

        public Contenido GetContenido(int idDetalleDecoumento)
        {
            Contenido contenido = null;

            using (var db = new GestNotifContext())
            {
                contenido = db.Contenido.Where(i => i.DetalleDocumentos_ID == idDetalleDecoumento).FirstOrDefault();
            }

            return contenido;
        }
    }

    //Contenido de los anexos
    [Table("Contenido1")]
    public partial class Contenido1 : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private int documentosAnexoFieldId; //DocumentosAnexo documentosAnexoField;
        private string hrefField;
        private string valueField;

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

        public int DocumentosAnexo_ID
        {
            get
            {
                return this.documentosAnexoFieldId;
            }
            set
            {
                this.documentosAnexoFieldId = value;
                this.RaisePropertyChanged("documentosAnexoFieldId");
            }
        }

        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
                this.RaisePropertyChanged("href");
            }
        }

        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
                this.RaisePropertyChanged("Value");
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

        public int InsertContenido1(string href, string value, int documentoAnexoId, GestNotifContext db = null)
        {
            Contenido1 contenido = new Contenido1()
            {
                DocumentosAnexo_ID = documentoAnexoId,
                Href = href,
                Value = value
            };

            db.Contenido1.Add(contenido);
            db.SaveChanges();

            return contenido.ID;
        }

        public Contenido1 GetContenido1(int idDocumentoAnexo)
        {
            Contenido1 contenido = null;

            using (var db = new GestNotifContext())
            {
                contenido = db.Contenido1.Where(i => i.DocumentosAnexo_ID == idDocumentoAnexo).FirstOrDefault();
            }

            return contenido;
        }
    }

    //Contenido del acuse
    [Table("Contenido2")]
    public partial class Contenido2 : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private int acusesPdfIdField;
        private string hrefField;
        private string valueField;

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

        public int AcusesPdf_ID
        {
            get
            {
                return this.acusesPdfIdField;
            }
            set
            {
                this.acusesPdfIdField = value;
                this.RaisePropertyChanged("acusesPdfIdField");
            }
        }

        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
                this.RaisePropertyChanged("href");
            }
        }

        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
                this.RaisePropertyChanged("Value");
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

        public int InsertContenidoAcusePdf(string href, string value, int acusePdfId, GestNotifContext db = null)
        {
            Contenido2 contenido = new Contenido2()
            {
                AcusesPdf_ID = acusePdfId,
                Href = href,
                Value = value
            };

            try
            {
                db.Contenido2.Add(contenido);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcionContext(ex.Message, ex.StackTrace, 0, db);
            }

            return contenido.ID;
        }

        public Contenido2 GetContenido2(int idAcusePdf)
        {
            Contenido2 contenido = null;

            using (var db = new GestNotifContext())
            {
                contenido = db.Contenido2.Where(i => i.AcusesPdf_ID == idAcusePdf).FirstOrDefault();
            }

            return contenido;
        }
    }
}