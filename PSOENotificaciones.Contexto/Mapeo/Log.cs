using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;

namespace PSOENotificaciones.Contexto
{
    [Table("LogTareaLocaliza")]
    public partial class LogTareaLocaliza : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private DateTime fechaField;
        private string mensajeField;
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

        public string Mensaje
        {
            get
            {
                return this.mensajeField;
            }
            set
            {
                this.mensajeField = value;
                this.RaisePropertyChanged("mensajeField");
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

        public void InsertLog(string mensaje)
        {
            using (var db = new GestNotifContext())
            {
                try
                {
                    LogTareaLocaliza log = new LogTareaLocaliza
                    {
                        Fecha = DateTime.Now,
                        Mensaje = mensaje,
                        IP = GetIPAddress()
                    };

                    db.LogTareaLocaliza.Add(log);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    db.Dispose();
                    Console.Write(ex.Message);
                }
            }
        }

        public void InsertLogContext(string mensaje, GestNotifContext db)
        {
            try
            {
                LogTareaLocaliza log = new LogTareaLocaliza
                {
                    Fecha = DateTime.Now,
                    Mensaje = mensaje,
                    IP = GetIPAddress()
                };

                db.LogTareaLocaliza.Add(log);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                db.Dispose();
                Console.Write(ex.Message);
            }
        }

        public List<LogTareaLocaliza> GetLogs()
        {
            using (var db = new GestNotifContext())
            {
                return db.LogTareaLocaliza
                    .OrderByDescending(i => i.Fecha).ToList();
            }
        }

        public List<LogTareaLocaliza> GetLogsPorFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            using (var db = new GestNotifContext())
            {
                return db.LogTareaLocaliza
                    .Where(i => i.Fecha >= fechaInicio && i.Fecha <= fechaFin)
                    .OrderBy(i => i.Fecha).ToList();
            }
        }

        public LogTareaLocaliza GetLogHoy(DateTime fechaHoy)
        {
            using (var db = new GestNotifContext())
            {
                return db.LogTareaLocaliza
                    .Where(i => i.Mensaje == "INICIO 'Proceso Localiza' - AUTOMÁTICO" && i.Fecha >= fechaHoy).FirstOrDefault();
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

    [Table("LogTareaAlertasCaducadas")]
    public partial class LogTareaAlertasCaducadas : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private DateTime fechaField;
        private string mensajeField;
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

        public string Mensaje
        {
            get
            {
                return this.mensajeField;
            }
            set
            {
                this.mensajeField = value;
                this.RaisePropertyChanged("mensajeField");
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

        public void InsertLog(string mensaje)
        {
            using (var db = new GestNotifContext())
            {
                try
                {
                    LogTareaAlertasCaducadas log = new LogTareaAlertasCaducadas
                    {
                        Fecha = DateTime.Now,
                        Mensaje = mensaje,
                        IP = GetIPAddress()
                    };

                    db.LogTareaAlertasCaducadas.Add(log);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    db.Dispose();
                    Console.Write(ex.Message);
                }
            }
        }

        public void InsertLogContext(string mensaje, GestNotifContext db)
        {
            try
            {
                LogTareaAlertasCaducadas log = new LogTareaAlertasCaducadas
                {
                    Fecha = DateTime.Now,
                    Mensaje = mensaje,
                    IP = GetIPAddress()
                };

                db.LogTareaAlertasCaducadas.Add(log);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                db.Dispose();
                Console.Write(ex.Message);
            }
        }

        public List<LogTareaAlertasCaducadas> GetLogs()
        {
            using (var db = new GestNotifContext())
            {
                return db.LogTareaAlertasCaducadas
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

    [Table("LogLlamadasDehu")]
    public partial class LogLlamadasDehu : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private DateTime fechaField;
        private string mensajeField;

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
                return this.fechaField;
            }
            set
            {
                this.fechaField = value;
                this.RaisePropertyChanged("fechaField");
            }
        }

        public string Mensaje
        {
            get
            {
                return this.mensajeField;
            }
            set
            {
                this.mensajeField = value;
                this.RaisePropertyChanged("mensajeField");
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

        public void InsertLogLlamadaDehuContext(string mensaje, GestNotifContext db)
        {
            try
            {
                LogLlamadasDehu log = new LogLlamadasDehu
                {
                    Fecha = DateTime.Now,
                    Mensaje = mensaje
                };

                db.LogLlamadasDehu.Add(log);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                db.Dispose();
                Console.Write(ex.Message);
            }
        }

        public List<LogLlamadasDehu> GetLogsLlamadasDehu()
        {
            using (var db = new GestNotifContext())
            {
                return db.LogLlamadasDehu
                    .OrderByDescending(i => i.Fecha).ToList();
            }
        }
    }
}
