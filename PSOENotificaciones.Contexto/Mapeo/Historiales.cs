using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;

namespace PSOENotificaciones.Contexto
{
    [Table("HistorialExcepciones")]
    public partial class HistorialExcepciones : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private Usuarios usuariosField;
        private DateTime fechaField;
        private string excepcionField;
        private string trazaField;
        private string ipField;

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
                this.RaisePropertyChanged("fechaField");
            }
        }

        public string Excepcion
        {
            get
            {
                return this.excepcionField;
            }
            set
            {
                this.excepcionField = value;
                this.RaisePropertyChanged("excepcionField");
            }
        }

        public string Traza
        {
            get
            {
                return this.trazaField;
            }
            set
            {
                this.trazaField = value;
                this.RaisePropertyChanged("trazaField");
            }
        }

        public string IP
        {
            get
            {
                return this.ipField;
            }
            set
            {
                this.ipField = value;
                this.RaisePropertyChanged("ipField");
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

        public void InsertExcepcion(string mensaje, string traza, int idUsuario)
        {
            using (var db = new GestNotifContext())
            {
                try
                {
                    HistorialExcepciones err = new HistorialExcepciones
                    {
                        Usuarios = (idUsuario == 0 ? null : db.Usuarios.Where(i => i.ID == idUsuario).ToList()[0]),
                        Fecha = DateTime.Now,
                        Excepcion = mensaje,
                        Traza = traza,
                        IP = GetIPAddress()
                    };

                    db.HistorialExcepciones.Add(err);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    db.Dispose();
                    Console.Write(ex.Message);
                }
            }
        }

        public void InsertExcepcionContext(string mensaje, string traza, int idUsuario, GestNotifContext db)
        {
            try
            {
                HistorialExcepciones err = new HistorialExcepciones
                {
                    Usuarios = (idUsuario == 0 ? null : db.Usuarios.Where(i => i.ID == idUsuario).ToList()[0]),
                    Fecha = DateTime.Now,
                    Excepcion = mensaje,
                    Traza = traza,
                    IP = GetIPAddress()
                };

                db.HistorialExcepciones.Add(err);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                db.Dispose();
                Console.Write(ex.Message);
            }
        }

        public List<HistorialExcepciones> GetExcepciones()
        {
            using (var db = new GestNotifContext())
            {
                return db.HistorialExcepciones.Include("Usuarios")
                    .OrderByDescending(i => i.Fecha).ToList();
            }
        }

        private string GetIPAddress()
        {
            string IPAddress = "";
            string Hostname = System.Environment.MachineName;
            IPHostEntry Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = Convert.ToString(IP);
                }
            }
            return IPAddress;
        }
    }

    [Table("HistorialCambiosEnvio")]
    public partial class HistorialCambiosEnvio : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private string envioIdField;
        private int usuarioIdFieldId;
        private int tiposCambioEnvioIdField;
        private DateTime fechaField;
        private int grupoIdField;
        private int estadoEnvioIdField;

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
                return this.envioIdField;
            }
            set
            {
                this.envioIdField = value;
                this.RaisePropertyChanged("envioIdField");
            }
        }

        public int Usuarios_ID
        {
            get
            {
                return this.usuarioIdFieldId;
            }
            set
            {
                this.usuarioIdFieldId = value;
                this.RaisePropertyChanged("usuarioIdFieldId");
            }
        }

        public int TiposCambioEnvio_ID
        {
            get
            {
                return this.tiposCambioEnvioIdField;
            }
            set
            {
                this.tiposCambioEnvioIdField = value;
                this.RaisePropertyChanged("tiposCambioEnvioIdField");
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

        public int Grupos_ID
        {
            get
            {
                return this.grupoIdField;
            }
            set
            {
                this.grupoIdField = value;
                this.RaisePropertyChanged("grupoIdField");
            }
        }

        public int EstadosEnvio_ID
        {
            get
            {
                return this.estadoEnvioIdField;
            }
            set
            {
                this.estadoEnvioIdField = value;
                this.RaisePropertyChanged("estadoEnvioIdField");
            }
        }

        [NotMapped]
        public string CodigoHtmlUsuario
        {
            get
            {
                if (usuarioIdFieldId != 0)
                {
                    var db = new GestNotifContext();
                    return db.Usuarios.Where(i => i.ID == usuarioIdFieldId).FirstOrDefault().Nombre;
                }
                else
                    return "";
            }
        }

        [NotMapped]
        public string CodigoHtmlGrupo
        {
            get
            {
                if (grupoIdField != 0)
                {
                    Grupos gr = new Grupos();
                    return gr.GetGrupoById(grupoIdField).Nombre;
                }
                else
                    return "";
            }
        }

        [NotMapped]
        public string CodigoHtmlEstadoEnvio
        {
            get
            {
                if (estadoEnvioIdField != 0)
                {
                    switch (estadoEnvioIdField)
                    {
                        case (int)EstadoEnvio.SinAsignar:
                            return "<span class='status_icon status_icon__earring'></span>" + EstadoEnvio.SinAsignar;
                        case (int)EstadoEnvio.EnAlerta:
                            return "<span class='status_icon status_icon__alert'></span>" + EstadoEnvio.EnAlerta;
                        case (int)EstadoEnvio.Asignada:
                            return "<span class='status_icon status_icon__assigned'></span>" + EstadoEnvio.Asignada;
                        case (int)EstadoEnvio.Comparecida:
                            return "<span class='status_icon status_icon__appeared'></span>" + EstadoEnvio.Comparecida;
                        case (int)EstadoEnvio.Caducada:
                            return "<span class='status_icon status_icon__rejected'></span>" + EstadoEnvio.Caducada;
                        case (int)EstadoEnvio.Leida:
                            return "<span class='status_icon status_icon__appeared'></span>" + EstadoEnvio.Leida;
                        case (int)EstadoEnvio.Externas:
                            return "<span class='status_icon status_icon__extern'></span>" + EstadoEnvio.Externas;

                        default:
                            return "";
                    }
                }
                else
                    return "";
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

        public List<HistorialCambiosEnvio> GetHistorialCambiosEnvio(string identificador, TipoCambioEnvio tipoCambio)
        {
            using (var db = new GestNotifContext())
            {
                return db.HistorialCambiosEnvio
                    .Where(i => i.Envios_Identificador == identificador && i.TiposCambioEnvio_ID == (int)tipoCambio)
                    .OrderByDescending(i => i.Fecha).ToList();
            }
        }

        public HistorialCambiosEnvio GetEnvioCaducado(string identificador)
        {
            using (var db = new GestNotifContext())
            {
                return db.HistorialCambiosEnvio
                    .Where(i => i.Envios_Identificador == identificador && 
                        i.TiposCambioEnvio_ID == (int)TipoCambioEnvio.CambioEstado &&
                        i.EstadosEnvio_ID == (int)EstadoEnvio.Caducada)
                    .OrderByDescending(i => i.Fecha).FirstOrDefault();
            }
        }
    }

    [Table("TiposCambioEnvio")]
    public partial class TiposCambioEnvio : object, System.ComponentModel.INotifyPropertyChanged
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

    public enum TipoCambioEnvio
    {
        CambioEstado = 1,
        GrupoAsignado = 2,
    }
}
