using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PSOENotificaciones.Contexto
{
    [Table("AuditoriaAplicaciones")]
    public partial class AuditoriaAplicaciones : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int appIdField;
        private string appNombreField;
        private string appDescripcionField;
        private string appUrlField;

        [Key]
        public int AppID
        {
            get
            {
                return this.appIdField;
            }
            set
            {
                this.appIdField = value;
                this.RaisePropertyChanged("appIdField");
            }
        }

        [Required]
        [MaxLength(255)]
        public string AppNombre
        {
            get
            {
                return this.appNombreField;
            }
            set
            {
                this.appNombreField = value;
                this.RaisePropertyChanged("appNombreField");
            }
        }

        [Required]
        [MaxLength(255)]
        public string AppDescripcion
        {
            get
            {
                return this.appDescripcionField;
            }
            set
            {
                this.appDescripcionField = value;
                this.RaisePropertyChanged("appDescripcionField");
            }
        }

        [Required]
        [MaxLength(255)]
        public string AppUrl
        {
            get
            {
                return this.appUrlField;
            }
            set
            {
                this.appUrlField = value;
                this.RaisePropertyChanged("appUrlField");
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

    [Table("AuditoriaLogin")]
    public partial class AuditoriaLogin : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int auditIdField;
        private AuditoriaAplicaciones auditoriaAplicacionesField;
        private Usuarios usuariosField;
        private DateTime fechaAccesoField;
        private bool autorizadoField;
        private string hostNameField;
        private string clientIpField;

        [Key]
        public int AuditID
        {
            get
            {
                return this.auditIdField;
            }
            set
            {
                this.auditIdField = value;
                this.RaisePropertyChanged("auditIdField");
            }
        }

        [Required]
        public AuditoriaAplicaciones AuditoriaAplicaciones
        {
            get
            {
                return this.auditoriaAplicacionesField;
            }
            set
            {
                this.auditoriaAplicacionesField = value;
                this.RaisePropertyChanged("auditoriaAplicacionesField");
            }
        }

        [Required]
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

        public DateTime FechaAcceso
        {
            get
            {
                return this.fechaAccesoField;
            }
            set
            {
                this.fechaAccesoField = value;
                this.RaisePropertyChanged("fechaAccesoField");
            }
        }

        public bool Autorizado
        {
            get
            {
                return this.autorizadoField;
            }
            set
            {
                this.autorizadoField = value;
                this.RaisePropertyChanged("autorizadoField");
            }
        }

        public string HostName
        {
            get
            {
                return this.hostNameField;
            }
            set
            {
                this.hostNameField = value;
                this.RaisePropertyChanged("hostNameField");
            }
        }

        public string ClientIP
        {
            get
            {
                return this.clientIpField;
            }
            set
            {
                this.clientIpField = value;
                this.RaisePropertyChanged("clientIpField");
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

        public void InsertAuditoriaLogin(bool autorizado, string hostName, string clientIP, int? idUsuario)
        {
            using (var db = new GestNotifContext())
            {
                AuditoriaLogin al = new AuditoriaLogin
                {
                    Autorizado = autorizado,
                    HostName = hostName,
                    ClientIP = clientIP,
                    Usuarios = (idUsuario.HasValue ? db.Usuarios.Where(i => i.ID == idUsuario).ToList()[0] : null)
                };
                db.AuditoriaLogin.Add(al);

                db.SaveChanges();
            }
        }
    }
}
