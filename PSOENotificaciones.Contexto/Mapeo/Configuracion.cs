using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Collections.Generic;

namespace PSOENotificaciones.Contexto
{
    [Table("Alertas")]
    public partial class Alertas : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private string descripcionField;
        private string valorField;
        private bool activoField;
        public static int DiasCaducidad;

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

        [MaxLength(50)]
        public string Valor
        {
            get
            {
                return this.valorField;
            }
            set
            {
                this.valorField = value;
                this.RaisePropertyChanged("valorField");
            }
        }

        public bool Activo
        {
            get
            {
                return this.activoField;
            }
            set
            {
                this.activoField = value;
                this.RaisePropertyChanged("activoField");
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

        public List<Alertas> GetAlertas()
        {
            using (var db = new GestNotifContext())
            {
                return db.Alertas.ToList();
            }
        }

        public Alertas GetAlertaById(int idAlerta)
        {
            using (var db = new GestNotifContext())
            {
                return db.Alertas.Where(i => i.ID == idAlerta).FirstOrDefault();
            }
        }

        public void UpdateAlertas(int id, string valor)
        {
            using (var db = new GestNotifContext())
            {
                var alerta = db.Alertas.SingleOrDefault(b => b.ID == id);
                if (alerta != null)
                {
                    alerta.Valor = valor;
                    db.SaveChanges();
                }
            }
        }

        public void SetDiasCaducidad()
        {
            using (var db = new GestNotifContext())
            {
                Alertas alerta = db.Alertas.Where(i => i.ID == (int)Alerta.DiasCaducidad).FirstOrDefault();
                Alertas.DiasCaducidad = Convert.ToInt32(alerta.Valor);
            }
        }
    }

    [Table("Parametros")]
    public partial class Parametros : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private string descripcionField;
        private string valorField;
        private bool? activoField;

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

        [MaxLength(50)]
        public string Valor
        {
            get
            {
                return this.valorField;
            }
            set
            {
                this.valorField = value;
                this.RaisePropertyChanged("valorField");
            }
        }

        public bool? Activo
        {
            get
            {
                return this.activoField;
            }
            set
            {
                this.activoField = value;
                this.RaisePropertyChanged("activoField");
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

        public List<Parametros> GetParametros()
        {
            using (var db = new GestNotifContext())
            {
                return db.Parametros.ToList();
            }
        }

        public string GetParametroById(int idParametro)
        {
            using (var db = new GestNotifContext())
            {
                return db.Parametros.Where(i => i.ID == idParametro).Select(i => i.Valor).FirstOrDefault();
            }
        }

        public void UpdateParametros(int id, string valor)
        {
            using (var db = new GestNotifContext())
            {
                var parametro = db.Parametros.SingleOrDefault(b => b.ID == id);
                if (parametro != null)
                {
                    parametro.Valor = valor;
                    db.SaveChanges();
                }
            }
        }
    }

    [Table("ParametrosInternos")]
    public partial class ParametrosInternos : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private string descripcionField;
        private string valorField;

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

        [MaxLength(50)]
        public string Valor
        {
            get
            {
                return this.valorField;
            }
            set
            {
                this.valorField = value;
                this.RaisePropertyChanged("valorField");
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

        public ParametrosInternos GetParametroInternoPorId(int idParametroInterno)
        {
            using (var db = new GestNotifContext())
            {
                return db.ParametrosInternos.Where(i => i.ID == idParametroInterno).FirstOrDefault();
            }
        }
    }

    [Table("AvisosNotificacion")]
    public partial class AvisosNotificacion : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private string nombreField;
        private string emailField;
        private bool activoField;
        private int tipoEmailField;

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

        [MaxLength(50)]
        public string Email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
                this.RaisePropertyChanged("valorField");
            }
        }

        public bool Activo
        {
            get
            {
                return this.activoField;
            }
            set
            {
                this.activoField = value;
                this.RaisePropertyChanged("activoField");
            }
        }

        public int TipoEmail
        {
            get
            {
                return this.tipoEmailField;
            }
            set
            {
                this.tipoEmailField = value;
                this.RaisePropertyChanged("activoField");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        [NotMapped]
        public string CodigoHtmlEditar
        {
            get
            {
                //if (ID == 0)
                //    return "";
                //else
                    return "<div class='action_button'>" +
                        "<span class='action_button__icon action_button__icon__edit editar_email' id-mail='" + idField + "' id='spanEdit' title='Editar email'></span>" +
                        "</div>";
            }
        }

        [NotMapped]
        public string CodigoHtmlEliminar
        {
            get
            {
                //if (ID == 0)
                //    return "";
                //else
                    return "<div class='action_button'>" +
                        "<span class='action_button__icon action_button__icon__delete' id='spanDelete' title='Eliminar email'></span>" +
                        "</div>";
            }
        }

        [NotMapped]
        public string CodigoHtmlActivo
        {
            get
            {
                if (activoField)
                    return "<span class='status_icon status_icon__appeared'></span>";
                else
                    return "<span class='status_icon status_icon__rejected'></span>";
            }
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        public List<AvisosNotificacion> GetAvisos(int idTipoEmail)
        {
            using (var db = new GestNotifContext())
            {
                return db.AvisosNotificacion.Where(i => i.TipoEmail == idTipoEmail).ToList();
            }
        }

        public AvisosNotificacion GetAviso(int idEmail)
        {
            using (var db = new GestNotifContext())
            {
                return db.AvisosNotificacion.Where(i => i.ID == idEmail).FirstOrDefault();
            }
        }

        public bool InsertAvisoNotificacion(string nombre, string email, bool activo, int idTipoEmail)
        {
            using (var db = new GestNotifContext())
            {
                AvisosNotificacion objEmail = db.AvisosNotificacion.Where(i => i.Email == email && i.TipoEmail == idTipoEmail).FirstOrDefault();

                if (objEmail == null)
                {
                    AvisosNotificacion an = new AvisosNotificacion
                    {
                        Nombre = nombre,
                        Email = email,
                        Activo = activo,
                        TipoEmail = idTipoEmail
                    };
                    db.AvisosNotificacion.Add(an);

                    db.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        public void UpdateAvisoNotificacion(int id, string nombre, string email, bool activo, int idTipoEmail)
        {
            using (var db = new GestNotifContext())
            {
                var aviso = db.AvisosNotificacion.SingleOrDefault(b => b.ID == id);
                if (aviso != null)
                {
                    aviso.Nombre = nombre;
                    aviso.Email = email;
                    aviso.Activo = activo;
                    aviso.TipoEmail = idTipoEmail;
                    db.SaveChanges();
                }
            }
        }

        public void DeleteAvisoNotificacion(int idAviso)
        {
            using (var db = new GestNotifContext())
            {
                AvisosNotificacion aviso = db.AvisosNotificacion.SingleOrDefault(b => b.ID == idAviso);
                db.AvisosNotificacion.Remove(aviso);
                db.SaveChanges();
            }
        }
    }

    [Table("ConfiguracionPeticiones")]
    public partial class ConfiguracionPeticiones : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private string diaSemanaField;
        private TimeSpan horarioField;
        private bool activoField;

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

        [MaxLength(20)]
        public string DiaSemana
        {
            get
            {
                return this.diaSemanaField;
            }
            set
            {
                this.diaSemanaField = value;
                this.RaisePropertyChanged("diaSemanaField");
            }
        }

        public TimeSpan Horario
        {
            get
            {
                return this.horarioField;
            }
            set
            {
                this.horarioField = value;
                this.RaisePropertyChanged("horarioField");
            }
        }

        public bool Activo
        {
            get
            {
                return this.activoField;
            }
            set
            {
                this.activoField = value;
                this.RaisePropertyChanged("activoField");
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

        public void UpdateConfiguracionPeticiones(DiaSemana diaSemana, TimeSpan horario, bool activo)
        {
            using (var db = new GestNotifContext())
            {
                ConfiguracionPeticiones cp = db.ConfiguracionPeticiones.First(i => i.ID == (int)diaSemana);
                cp.Horario = horario;
                cp.Activo = activo;

                db.SaveChanges();
            }
        }

        public List<ConfiguracionPeticiones> GetHorarios()
        {
            using (var db = new GestNotifContext())
            {
                return db.ConfiguracionPeticiones.ToList();
            }
        }

        public ConfiguracionPeticiones GetHorario(int diaSemana)
        {
            using (var db = new GestNotifContext())
            {
                return db.ConfiguracionPeticiones.Where(i => i.ID == diaSemana && i.Activo == true).FirstOrDefault();
            }
        }
    }

    public enum DiaSemana
    {
        Lunes = 1,
        Martes = 2,
        Miércoles = 3,
        Jueves = 4,
        Viernes = 5,
        Sábado = 6,
        Domingo = 7
    }

    public enum Parametro
    {
        NumeroIntentosLogin = 1,
        TiempoEsperaLogin = 2,
        ConsentimientoLegal = 3
    }

    public enum ParametroInterno
    {
        LimiteCargaNotificaciones = 1,
        EnvioEmails = 2,
    }

    public enum Alerta
    {
        DiasAntelacion = 1,
        DiasCaducidad = 2
    }

    public enum TipoEmail
    {
        AltaUsuario = 1,
        BloqueoUsuario = 2,
        DesbloqueoUsuario = 3,
        ProcesoCarga = 4,
        ProcesoCargaError = 5,
        AsignacionManual = 6,
        EnAlerta = 7,
        Caducadas = 8,
        Externas = 9,
    }
}
