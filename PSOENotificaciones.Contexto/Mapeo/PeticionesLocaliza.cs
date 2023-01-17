using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace PSOENotificaciones.Contexto
{
    [Table("PeticionesLocaliza")]
    public partial class PeticionesLocaliza : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private Usuarios usuariosField = null;
        private DateTime fechaField;
        private string nifTitularField;
        private string nifDestinatarioField;
        private string codigoDestinoField;
        private DateTime? fechaDesdeField;
        private DateTime? fechaHastaField;
        private TiposEnvio tiposEnvioField = null;
        private List<Opcion> opcionesLocalizaField;
        private string errorCodigoField;
        private string errorDescripcionField;
        private string errorMetodoField;
        private DateTime? errorFechaField;
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

        public Usuarios Usuarios
        {
            get
            {
                return this.usuariosField;
            }
            set
            {
                this.usuariosField = value;
                this.RaisePropertyChanged("usuariosField");
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

        [MaxLength(9)]
        public string NifTitular
        {
            get
            {
                return this.nifTitularField;
            }
            set
            {
                this.nifTitularField = value;
                this.RaisePropertyChanged("nifTitular");
            }
        }

        [MaxLength(9)]
        public string NifDestinatario
        {
            get
            {
                return this.nifDestinatarioField;
            }
            set
            {
                this.nifDestinatarioField = value;
                this.RaisePropertyChanged("nifDestinatario");
            }
        }

        [MaxLength(255)]
        public string CodigoDestino
        {
            get
            {
                return this.codigoDestinoField;
            }
            set
            {
                this.codigoDestinoField = value;
                this.RaisePropertyChanged("codigoDestino");
            }
        }

        public DateTime? FechaDesde
        {
            get
            {
                return this.fechaDesdeField;
            }
            set
            {
                this.fechaDesdeField = value;
                this.RaisePropertyChanged("fechaDesde");
            }
        }

        public DateTime? FechaHasta
        {
            get
            {
                return this.fechaHastaField;
            }
            set
            {
                this.fechaHastaField = value;
                this.RaisePropertyChanged("fechaHasta");
            }
        }

        public TiposEnvio TiposEnvio
        {
            get
            {
                return this.tiposEnvioField;
            }
            set
            {
                this.tiposEnvioField = value;
                this.RaisePropertyChanged("tipoEnvio");
            }
        }

        public List<Opcion> OpcionesLocaliza
        {
            get
            {
                return this.opcionesLocalizaField;
            }
            set
            {
                this.opcionesLocalizaField = value;
                this.RaisePropertyChanged("opcionesLocaliza");
            }
        }

        public string ErrorCodigo
        {
            get
            {
                return this.errorCodigoField;
            }
            set
            {
                this.errorCodigoField = value;
                this.RaisePropertyChanged("errorCodigoField");
            }
        }

        public string ErrorDescripcion
        {
            get
            {
                return this.errorDescripcionField;
            }
            set
            {
                this.errorDescripcionField = value;
                this.RaisePropertyChanged("errorDescripcionField");
            }
        }

        public string ErrorMetodo
        {
            get
            {
                return this.errorMetodoField;
            }
            set
            {
                this.errorMetodoField = value;
                this.RaisePropertyChanged("errorMetodoField");
            }
        }

        public DateTime? ErrorFecha
        {
            get
            {
                return this.errorFechaField;
            }
            set
            {
                this.errorFechaField = value;
                this.RaisePropertyChanged("errorFechaField");
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

        public int InsertPeticionLocalizaContext(DateTime fechaPeticion, int idTipoEnvio, int idUsuario,
            string nifTitular = null, string nifDestinatario = null, string codigoDestino = null, DateTime? fechaDesde = null,
            DateTime? fechaHasta = null, GestNotifContext db = null)
        {
            int idPeticion = 0;

            try
            {
                PeticionesLocaliza pl = new PeticionesLocaliza
                {
                    Usuarios = db.Usuarios.Where(i => i.ID == idUsuario).FirstOrDefault(),
                    Fecha = fechaPeticion,
                    NifTitular = nifTitular,
                    NifDestinatario = nifDestinatario,
                    CodigoDestino = codigoDestino,
                    FechaDesde = fechaDesde,
                    FechaHasta = fechaHasta,
                    TiposEnvio = db.TiposEnvio.Where(i => i.ID == idTipoEnvio).FirstOrDefault(),
                    NombreXml = String.Empty
                };

                db.PeticionesLocaliza.Add(pl);
                db.SaveChanges();

                idPeticion = pl.ID;
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message + " " + e.InnerException);
                throw;
            }

            return idPeticion;
        }

        public void SetPeticionLocalizaXml(int idPeticionLocaliza, string peticionXml)
        {
            using (var db = new GestNotifContext())
            {
                PeticionesLocaliza peticion = db.PeticionesLocaliza.Where(i => i.ID == idPeticionLocaliza).FirstOrDefault();
                peticion.NombreXml = peticionXml;
                db.SaveChanges();
            }
        }

        public void InsertErroresPeticionLocaliza(string message, string localizedMessage, string metodo, string identificador, GestNotifContext db)
        {
            Envios envios = db.Envios.Where(l => l.Identificador == identificador).FirstOrDefault();
            envios.ErrorCodigo = message;
            envios.ErrorDescripcion = localizedMessage;
            envios.ErrorMetodo = metodo;
            envios.ErrorFecha = DateTime.Now;
            db.SaveChanges();
        }

        public PeticionesLocaliza GetPeticionLocaliza(int idPeticionLocaliza)
        {
            using (var db = new GestNotifContext())
            {
                return db.PeticionesLocaliza.Include("Usuarios")
                    .Where(i => i.ID == idPeticionLocaliza).FirstOrDefault();
            }
        }
    }
}
