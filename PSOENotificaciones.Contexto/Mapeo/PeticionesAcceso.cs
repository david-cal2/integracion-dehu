using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace PSOENotificaciones.Contexto
{
    [Table("PeticionesAcceso")]
    public partial class PeticionesAcceso : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private Envios enviosField;
        private Usuarios usuariosField;
        private Eventos eventosField;
        private DateTime fechaField;
        private int codigoOrigenField;
        private string nifReceptorField;
        private string nombreReceptorField;
        private string conceptoField;
        private List<Opcion2> opcionesPeticionAccesoField;
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

        public Envios Envios
        {
            get
            {
                return this.enviosField;
            }
            set
            {
                this.enviosField = value;
                this.RaisePropertyChanged("enviosField");
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
            }
        }

        public Eventos Eventos
        {
            get
            {
                return this.eventosField;
            }
            set
            {
                this.eventosField = value;
                this.RaisePropertyChanged("eventosField");
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

        public string NifReceptor
        {
            get
            {
                return this.nifReceptorField;
            }
            set
            {
                this.nifReceptorField = value;
                this.RaisePropertyChanged("nifReceptor");
            }
        }

        public string NombreReceptor
        {
            get
            {
                return this.nombreReceptorField;
            }
            set
            {
                this.nombreReceptorField = value;
                this.RaisePropertyChanged("nombreReceptor");
            }
        }

        public string Concepto
        {
            get
            {
                return this.conceptoField;
            }
            set
            {
                this.conceptoField = value;
                this.RaisePropertyChanged("concepto");
            }
        }

        public List<Opcion2> OpcionesPeticionAcceso
        {
            get
            {
                return this.opcionesPeticionAccesoField;
            }
            set
            {
                this.opcionesPeticionAccesoField = value;
                this.RaisePropertyChanged("opcionesPeticionAcceso");
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

        public int InsertPeticionAcceso(string identificador, int idUsuario, DateTime fecha, int codigoOrigen, string nifReceptor,
            string nombreReceptor, string concepto)
        {
            using (var db = new GestNotifContext())
            {
                PeticionesAcceso nuevaPeticion = new PeticionesAcceso
                {
                    Envios = db.Envios.Where(i => i.Identificador == identificador).FirstOrDefault(),
                    Usuarios = db.Usuarios.Where(i => i.ID == idUsuario).FirstOrDefault(),
                    Eventos = db.Eventos.Where(i => i.ID == (int)Evento.Aceptada).FirstOrDefault(),
                    Fecha = fecha,
                    CodigoOrigen = codigoOrigen,
                    NifReceptor = nifReceptor,
                    NombreReceptor = nombreReceptor,
                    Concepto = concepto,
                    NombreXml = string.Empty
                };

                db.PeticionesAcceso.Add(nuevaPeticion);
                db.SaveChanges();

                return nuevaPeticion.ID;
            }
        }

        public int InsertPeticionAccesoContext(string identificador, int idUsuario, DateTime fecha, int codigoOrigen, string nifReceptor,
            string nombreReceptor, string concepto, GestNotifContext db = null)
        {
            int idPeticion;

            PeticionesAcceso pa = new PeticionesAcceso
            {
                Envios = db.Envios.Where(i => i.Identificador == identificador).FirstOrDefault(),
                Usuarios = db.Usuarios.Where(i => i.ID == idUsuario).FirstOrDefault(),
                Eventos = db.Eventos.Where(i => i.ID == (int)Evento.Aceptada).FirstOrDefault(),
                Fecha = fecha,
                CodigoOrigen = codigoOrigen,
                NifReceptor = nifReceptor,
                NombreReceptor = nombreReceptor,
                Concepto = concepto,
                NombreXml = string.Empty
            };

            db.PeticionesAcceso.Add(pa);
            db.SaveChanges();

            idPeticion = pa.ID;

            return idPeticion;
        }

        public PeticionesAcceso GetPeticionAcceso(string identificador)
        {
            using (var db = new GestNotifContext())
            {
                return db.PeticionesAcceso.Include("Usuarios")
                    .Where(i => i.Envios.Identificador == identificador)
                    .OrderByDescending(i => i.Fecha).FirstOrDefault();
            }
        }
    }

    [Table("Eventos")]
    public partial class Eventos : object, System.ComponentModel.INotifyPropertyChanged
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

        [MaxLength(255)]
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
    }

    public enum Evento
    {
        Aceptada = 1
    }
}
