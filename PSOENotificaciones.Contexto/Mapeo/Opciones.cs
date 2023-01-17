using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PSOENotificaciones.Contexto
{
    [Table("Opciones")]
    public partial class Opcion : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private string tipoField;
        private string valueField;
        private PeticionesLocaliza peticionesLocalizaField;

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

        public string Tipo
        {
            get
            {
                return this.tipoField;
            }
            set
            {
                this.tipoField = value;
                this.RaisePropertyChanged("tipo");
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

        public PeticionesLocaliza PeticionesLocaliza
        {
            get
            {
                return this.peticionesLocalizaField;
            }
            set
            {
                this.peticionesLocalizaField = value;
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

        public void InsertOpcion(string tipo, string valor, int idPeticionLocaliza)
        {
            using (var db = new GestNotifContext())
            {
                Opcion op = new Opcion
                {
                    PeticionesLocaliza = db.PeticionesLocaliza.Where(i => i.ID == idPeticionLocaliza).FirstOrDefault(),
                    Tipo = tipo,
                    Value = valor
                };

                db.Opcion.Add(op);

                db.SaveChanges();
            }
        }
    }

    [Table("Opciones1")]
    public partial class Opcion1 : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private string tipoField;
        private string valueField;
        private RespuestasLocaliza respuestasLocalizaField = null;
        private Envios enviosField = null;

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

        public string Tipo
        {
            get
            {
                return this.tipoField;
            }
            set
            {
                this.tipoField = value;
                this.RaisePropertyChanged("tipo");
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

        public RespuestasLocaliza RespuestasLocaliza
        {
            get
            {
                return this.respuestasLocalizaField;
            }
            set
            {
                this.respuestasLocalizaField = value;
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

        public void InsertOpcion1(string tipo, string valor, int? idRespuestaLocaliza = null, string identificadorEnvio = null)
        {
            using (var db = new GestNotifContext())
            {
                Opcion1 op = new Opcion1
                {
                    RespuestasLocaliza = idRespuestaLocaliza.HasValue ? db.RespuestasLocaliza.Where(i => i.ID == idRespuestaLocaliza).FirstOrDefault() : RespuestasLocaliza,
                    Envios = idRespuestaLocaliza.HasValue ? db.Envios.Where(i => i.Identificador == identificadorEnvio).FirstOrDefault() : Envios,
                    Tipo = tipo,
                    Value = valor
                };

                db.Opcion1.Add(op);

                db.SaveChanges();
            }
        }
    }

    [Table("Opciones2")]
    public partial class Opcion2 : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private string tipoField;
        private string valueField;
        private PeticionesAcceso peticionesAccesoFiled;

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

        public string Tipo
        {
            get
            {
                return this.tipoField;
            }
            set
            {
                this.tipoField = value;
                this.RaisePropertyChanged("tipo");
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

        public PeticionesAcceso PeticionesAcceso
        {
            get
            {
                return this.peticionesAccesoFiled;
            }
            set
            {
                this.peticionesAccesoFiled = value;
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

        public void InsertOpcion2(string tipo, string valor, int idPeticionAcceso)
        {
            using (var db = new GestNotifContext())
            {
                Opcion2 op = new Opcion2
                {
                    PeticionesAcceso = db.PeticionesAcceso.Where(i => i.ID == idPeticionAcceso).FirstOrDefault(),
                    Tipo = tipo,
                    Value = valor
                };

                db.Opcion2.Add(op);

                db.SaveChanges();
            }
        }
    }

    [Table("Opciones3")]
    public partial class Opcion3 : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private string tipoField;
        private string valueField;
        private RespuestasAcceso respuestasAccesoField;

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

        public string Tipo
        {
            get
            {
                return this.tipoField;
            }
            set
            {
                this.tipoField = value;
                this.RaisePropertyChanged("tipo");
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

        public RespuestasAcceso RespuestasAcceso
        {
            get
            {
                return this.respuestasAccesoField;
            }
            set
            {
                this.respuestasAccesoField = value;
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

        public void InsertOpcion3(string tipo, string valor, int idRespuestaAcceso)
        {
            using (var db = new GestNotifContext())
            {
                Opcion3 op = new Opcion3
                {
                    RespuestasAcceso = db.RespuestasAcceso.Where(i => i.ID == idRespuestaAcceso).FirstOrDefault(),
                    Tipo = tipo,
                    Value = valor
                };

                db.Opcion3.Add(op);

                db.SaveChanges();
            }
        }
    }

    [Table("Opciones4")]
    public partial class Opcion4 : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private string tipoField;
        private string valueField;
        private PeticionesAnexo peticionesAnexoField;

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

        public string Tipo
        {
            get
            {
                return this.tipoField;
            }
            set
            {
                this.tipoField = value;
                this.RaisePropertyChanged("tipo");
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

        public PeticionesAnexo PeticionesAnexo
        {
            get
            {
                return this.peticionesAnexoField;
            }
            set
            {
                this.peticionesAnexoField = value;
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

    [Table("Opciones5")]
    public partial class Opcion5 : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private string tipoField;
        private string valueField;
        private RespuestasAnexo respuestasAnexoField;

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

        public string Tipo
        {
            get
            {
                return this.tipoField;
            }
            set
            {
                this.tipoField = value;
                this.RaisePropertyChanged("tipo");
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

        public RespuestasAnexo RespuestasAnexo
        {
            get
            {
                return this.respuestasAnexoField;
            }
            set
            {
                this.respuestasAnexoField = value;
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

    [Table("Opciones6")]
    public partial class Opcion6 : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private string tipoField;
        private string valueField;
        private PeticionesAcusePdf peticionesAcusePdfField;

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

        public string Tipo
        {
            get
            {
                return this.tipoField;
            }
            set
            {
                this.tipoField = value;
                this.RaisePropertyChanged("tipo");
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

        public PeticionesAcusePdf PeticionesAcusePdf
        {
            get
            {
                return this.peticionesAcusePdfField;
            }
            set
            {
                this.peticionesAcusePdfField = value;
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

    [Table("Opciones7")]
    public partial class Opcion7 : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private string tipoField;
        private string valueField;
        private RespuestasAcusePdf respuestasAcusePdfField;

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

        public string Tipo
        {
            get
            {
                return this.tipoField;
            }
            set
            {
                this.tipoField = value;
                this.RaisePropertyChanged("tipo");
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

        public RespuestasAcusePdf RespuestasAcusePdf
        {
            get
            {
                return this.respuestasAcusePdfField;
            }
            set
            {
                this.respuestasAcusePdfField = value;
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
}
