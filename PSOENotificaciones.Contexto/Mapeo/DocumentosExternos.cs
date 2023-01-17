using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PSOENotificaciones.Contexto
{
    [Table("TiposDocumentosExternos")]
    public partial class TiposDocumentosExternos : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private string descripcionField;

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

        [MaxLength(50)]
        public string Descripcion
        {
            get
            {
                return this.descripcionField;
            }
            set
            {
                this.descripcionField = value;
                this.RaisePropertyChanged("descripcionField");
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

        public List<TiposDocumentosExternos> GetTiposDocumentosExternos()
        {
            using (var db = new GestNotifContext())
            {
                return db.TiposDocumentosExternos.ToList();
            }
        }
    }

    [Table("DocumentosExternos")]
    public partial class DocumentosExternos : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private string identificadorField;
        private DateTime fechaField;
        private int idUsuarioField;
        private string documentoField;
        private string typeMimeField;
        private string nombreField;
        private string descripcionField;
        private int idTipoDocumentoExternoField;

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

        public string Identificador
        {
            get
            {
                return this.identificadorField;
            }
            set
            {
                this.identificadorField = value;
                this.RaisePropertyChanged("identificadorField");
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
                this.RaisePropertyChanged("fechaField");
            }
        }

        public int Usuarios_ID
        {
            get
            {
                return this.idUsuarioField;
            }
            set
            {
                this.idUsuarioField = value;
                this.RaisePropertyChanged("idUsuarioField");
            }
        }

        public string Documento
        {
            get
            {
                return this.documentoField;
            }
            set
            {
                this.documentoField = value;
                this.RaisePropertyChanged("documentoField");
            }
        }

        public string TypeMime
        {
            get
            {
                return this.typeMimeField;
            }
            set
            {
                this.typeMimeField = value;
                this.RaisePropertyChanged("typeMimeField");
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
                this.RaisePropertyChanged("nombreField");
            }
        }

        public string Descripcion
        {
            get
            {
                return this.descripcionField;
            }
            set
            {
                this.descripcionField = value;
                this.RaisePropertyChanged("descripcionField");
            }
        }

        public int TiposDocumentosExternos_ID
        {
            get
            {
                return this.idTipoDocumentoExternoField;
            }
            set
            {
                this.idTipoDocumentoExternoField = value;
                this.RaisePropertyChanged("idTipoDocumentoExternoField");
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

        public void InsertDocumentoExterno(string identificador, DateTime fecha, int idUsuario, string documento,
            string typeMime, string nombre, string descripcion, TipoDocumentoExterno tipo)
        {
            using (var db = new GestNotifContext())
            {
                DocumentosExternos docExt = new DocumentosExternos
                {
                    Identificador = identificador,
                    Fecha = fecha,
                    Usuarios_ID = idUsuario,
                    Documento = documento,
                    TypeMime = typeMime,
                    Nombre = nombre,
                    Descripcion = descripcion,
                    TiposDocumentosExternos_ID = (int)tipo
                };
                db.DocumentosExternos.Add(docExt);
                db.SaveChanges();
            }
        }

        public List<DocumentosExternos> GetDocumentosExternosByIdentificador(string identificador)
        {
            using (var db = new GestNotifContext())
            {
                List <DocumentosExternos> lista = db.DocumentosExternos
                    .AsNoTracking()
                    .Where(i => i.Identificador == identificador)
                    .ToList();

                db.Dispose();

                return lista;
            }
        }
    }

    public enum TipoDocumentoExterno
    {
        DocumentoPrincipal = 1,
        Anexo = 2,
        Acuse = 3,
        Otros = 4
    }
}
