using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PSOENotificaciones.Contexto
{
    [Table("RespuestasLocaliza")]
    public partial class RespuestasLocaliza : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private DateTime fechaField;
        private string codigoRespuestaField;
        private string descripcionRespuestaField;
        private string nifPeticionField;
        private List<Envios> enviosField;
        private bool hayMasResultadosField;
        private PeticionesLocaliza peticionesLocalizaField;
        private List<Opcion1> opcionesRespuestaLocalizaField;
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
                return this.fechaField;
            }
            set
            {
                this.fechaField = value;
                this.RaisePropertyChanged("fecha");
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

        [MaxLength(255)]
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

        [MaxLength(9)]
        public string NifPeticion
        {
            get
            {
                return this.nifPeticionField;
            }
            set
            {
                this.nifPeticionField = value;
                this.RaisePropertyChanged("nifPeticion");
            }
        }

        public List<Envios> Envios
        {
            get
            {
                return this.enviosField;
            }
            set
            {
                this.enviosField = value;
                this.RaisePropertyChanged("envios");
            }
        }

        public bool HayMasResultados
        {
            get
            {
                return this.hayMasResultadosField;
            }
            set
            {
                this.hayMasResultadosField = value;
                this.RaisePropertyChanged("hayMasResultados");
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

        public List<Opcion1> OpcionesRespuestaLocaliza
        {
            get
            {
                return this.opcionesRespuestaLocalizaField;
            }
            set
            {
                this.opcionesRespuestaLocalizaField = value;
                this.RaisePropertyChanged("opcionesRespuestaLocaliza");
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

        public int InsertRespuestaLocalizaContext(DateTime fechaRespuesta, int idPeticionLocaliza, string codigoRespuesta,
            string descripcionResouesta, string nifPeticion, bool hayMasResultados, string respuestaXml, GestNotifContext db = null)
        {
            int idRespuesta;

            RespuestasLocaliza rl = new RespuestasLocaliza
            {
                PeticionesLocaliza = db.PeticionesLocaliza.Where(i => i.ID == idPeticionLocaliza).FirstOrDefault(),
                Fecha = fechaRespuesta,
                CodigoRespuesta = codigoRespuesta,
                DescripcionRespuesta = descripcionResouesta,
                NifPeticion = nifPeticion,
                HayMasResultados = hayMasResultados,
                NombreXml = respuestaXml
            };

            db.RespuestasLocaliza.Add(rl);
            db.SaveChanges();

            idRespuesta = rl.ID;

            return idRespuesta;
        }

        public RespuestasLocaliza GetRespuestaLocaliza(int idRespuestaLocaliza)
        {
            using (var db = new GestNotifContext())
            {
                return db.RespuestasLocaliza.Include("PeticionesLocaliza")
                    .Where(i => i.ID == idRespuestaLocaliza).FirstOrDefault();
            }
        }
    }

    [Table("Envios")]
    public partial class Envios : object, System.ComponentModel.INotifyPropertyChanged
    {
        private string identificadorField;
        private RespuestasLocaliza respuestasLocalizaField;
        private int codigoOrigenField;
        private string conceptoField;
        private string descripcionField;
        private DateTime fechaPuestaDisposicionField;
        private string metadatosPublicosField;
        private Vinculos vinculosField;
        private TiposEnvio tiposEnvioField;
        private OrganismosEmisores organismosEmisoresField;
        private Personas personasField;
        private EstadosEnvios estadosEnvioField;
        private string gruposAsignadosCadenaField;
        private string observacionesField;
        private string xmlComparecenciaField;
        private string errorCodigoField;
        private string errorDescripcionField;
        private string errorMetodoField;
        private DateTime? errorFechaField;
        private List<Opcion1> opcionesEnvioField;

        [Key]
        public string Identificador
        {
            get
            {
                return this.identificadorField;
            }
            set
            {
                this.identificadorField = value;
                this.RaisePropertyChanged("identificador");
            }
        }

        [Required]
        public RespuestasLocaliza RepuestasLocaliza
        {
            get
            {
                return this.respuestasLocalizaField;
            }
            set
            {
                this.respuestasLocalizaField = value;
                this.RaisePropertyChanged("respuestasLocalizaField");
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

        [MaxLength(255)]
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

        [MaxLength(1000)]
        public string Descripcion
        {
            get
            {
                return this.descripcionField;
            }
            set
            {
                this.descripcionField = value;
                this.RaisePropertyChanged("descripcion");
            }
        }

        public DateTime FechaPuestaDisposicion
        {
            get
            {
                return this.fechaPuestaDisposicionField;
            }
            set
            {
                this.fechaPuestaDisposicionField = value;
                this.RaisePropertyChanged("fechaPuestaDisposicion");
            }
        }

        [MaxLength(255)]
        public string MetadatosPublicos
        {
            get
            {
                return this.metadatosPublicosField;
            }
            set
            {
                this.metadatosPublicosField = value;
                this.RaisePropertyChanged("metadatosPublicos");
            }
        }

        public Vinculos Vinculos
        {
            get
            {
                return this.vinculosField;
            }
            set
            {
                this.vinculosField = value;
                this.RaisePropertyChanged("vinculosField");
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
                this.RaisePropertyChanged("tiposEnvioField");
            }
        }

        public OrganismosEmisores OrganismosEmisores
        {
            get
            {
                return this.organismosEmisoresField;
            }
            set
            {
                this.organismosEmisoresField = value;
                this.RaisePropertyChanged("organismosEmisoresField");
            }
        }

        public Personas Personas
        {
            get
            {
                return this.personasField;
            }
            set
            {
                this.personasField = value;
                this.RaisePropertyChanged("personasField");
            }
        }

        public EstadosEnvios EstadosEnvio
        {
            get
            {
                return this.estadosEnvioField;
            }
            set
            {
                this.estadosEnvioField = value;
                this.RaisePropertyChanged("estadosEnvioField");
            }
        }

        public string GruposAsignadosCadena
        {
            get
            {
                return this.gruposAsignadosCadenaField;
            }
            set
            {
                this.gruposAsignadosCadenaField = value;
                this.RaisePropertyChanged("gruposAsignadosCadenaField");
            }
        }

        public string Observaciones
        {
            get
            {
                return this.observacionesField;
            }
            set
            {
                this.observacionesField = value;
                this.RaisePropertyChanged("observacionesField");
            }
        }

        public string XmlComparecencia
        {
            get
            {
                return this.xmlComparecenciaField;
            }
            set
            {
                this.xmlComparecenciaField = value;
                this.RaisePropertyChanged("xmlComparecenciaField");
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

        public List<Opcion1> OpcionesEnvio
        {
            get
            {
                return this.opcionesEnvioField;
            }
            set
            {
                this.opcionesEnvioField = value;
                this.RaisePropertyChanged("opcionesEnvio");
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

        [NotMapped]
        public DateTime FechaCaducidad
        {
            get
            {
                //Alertas a = new Alertas();
                //Alertas alerta = a.GetAlertaById((int)Alerta.DiasCaducidad);
                return fechaPuestaDisposicionField.AddDays(Alertas.DiasCaducidad);
            }
        }

        [NotMapped]
        public string CodigoHtmlGruposAsignadosCadena
        {
            get
            {
                return "<span class='cadena-" + identificadorField + "'>" + gruposAsignadosCadenaField + "</span>";
            }
        }

        [NotMapped]
        public string CodigoHtmlCheckBox
        {
            get
            {
                return "<input class=\"styled-checkbox\" id=\"styled-checkbox-" + this.identificadorField + "\" type=\"checkbox\" value=\"value1\"><label for=\"styled-checkbox-" + this.identificadorField + "\"></label>";
            }
        }

        [NotMapped]
        public string CodigoHtmlEstado
        {
            get
            {
                switch (estadosEnvioField.ID)
                {
                    case (int)EstadoEnvio.SinAsignar:
                        return "<span class='status_icon status_icon__earring' title='Sin Asignar'></span>";
                    case (int)EstadoEnvio.EnAlerta:
                        return "<span class='status_icon status_icon__alert' title='Sin Alerta'></span>";
                    case (int)EstadoEnvio.Asignada:
                        return "<span class='status_icon status_icon__assigned' title='Asignada'></span>";
                    case (int)EstadoEnvio.Comparecida:
                        return "<span class='status_icon status_icon__appeared' title='Comparecida'></span>";
                    case (int)EstadoEnvio.Caducada:
                        return "<span class='status_icon status_icon__rejected' title='Caducada'></span>";
                    case (int)EstadoEnvio.Externas:
                        return "<span class='status_icon status_icon__extern' title='Externa'></span>";
                    case (int)EstadoEnvio.Leida:
                        return "<span class='status_icon status_icon__appeared' title='Leída'></span>";
                    default:
                        return "";
                }
            }
        }

        [NotMapped]
        public string EstadoDescripcion
        {
            get
            {
                return EstadosEnvio.Descripcion;
            }
        }

        [NotMapped]
        public string CodigoHtmlAdministracionPublica
        {
            get
            {
                return "<span data-bs-toggle='tooltip' data-bs-placement='top' title='" + OrganismosEmisores.AdministracionPublica + "'>" + OrganismosEmisores.AdministracionPublica + "</span>";
            }
        }

        [NotMapped]
        public string CodigoHtmlOrganismoEmisorRaiz
        {
            get
            {
                return "<span data-bs-toggle='tooltip' data-bs-placement='top' title='" + OrganismosEmisores.OrganismoRaiz + "'>" + OrganismosEmisores.OrganismoRaiz + "</span>";
            }
        }

        [NotMapped]
        public string CodigoHtmlOrganismoEmisor
        {
            get
            {
                return "<span data-bs-toggle='tooltip' data-bs-placement='top' title='" + OrganismosEmisores.OrganismoEmisor + "'>" + OrganismosEmisores.OrganismoEmisor + "</span>";
            }
        }

        [NotMapped]
        public string CodigoHtmlMenuNotificaciones
        {
            get
            {
                string enlaceNotificacion = (EstadosEnvio.ID == (int)EstadoEnvio.Comparecida ?
                    "<a href='../Notificaciones/NotificacionComparecida?identificador=" + identificadorField + "'><span class='action_button__icon action_button__icon__mail' title='Notificación Comparecida'></span></a>" :
                    "<a href='../Notificaciones/NotificacionDetalle?identificador=" + identificadorField + "&c=1' class='btn-comparecer'><span class='action_button__icon action_button__icon__mail_close' title='Comparecer Notificación'></span>");

                if (EstadosEnvio.ID == (int)EstadoEnvio.Caducada)
                    enlaceNotificacion = "<a href='../Notificaciones/NotificacionDetalle?identificador=" + identificadorField + "&c=0' class='btn-comparecer'><span class='action_button__icon action_button__icon__mail_close' title='Ver detalle'></span>";

                if (EstadosEnvio.ID == (int)EstadoEnvio.Externas)
                    enlaceNotificacion = "";

                return "<div class='action_button'>" +
                    enlaceNotificacion +
                    "<a href=\"javascript:abrirModalAsignarGrupo('" + identificadorField + "')\" class='btn-abrir-modal-asignar-grupo'><span class='action_button__icon action_button__icon__assigned-people' title='Asignar grupo'></span></a>" +
                    "</div>";
            }
        }

        [NotMapped]
        public string CodigoHtmlMenuComunicaciones
        {
            get
            {
                string enlaceComunicacion = (EstadosEnvio.ID == (int)EstadoEnvio.Leida ?
                    "<a href='../Comunicaciones/ComunicacionLeida?identificador=" + identificadorField + "'><span class='action_button__icon action_button__icon__mail' title='Notificación Leída'></span></a>" :
                    "<a href='../Comunicaciones/ComunicacionDetalle?identificador=" + identificadorField + "&c=1' class='btn-comparecer'><span class='action_button__icon action_button__icon__mail_close' title='Leer Comunicación'></span>");

                return "<div class='action_button'>" +
                    enlaceComunicacion +
                    "<a href=\"javascript:abrirModalAsignarGrupo('" + identificadorField + "')\" class='btn-abrir-modal-asignar-grupo'><span class='action_button__icon action_button__icon__assigned-people' title='Asignar grupo'></span></a>" +
                    "</div>";
            }
        }

        public Envios GetEnvioDetalleContext(string identificador, GestNotifContext db)
        {
            Envios envio = db.Envios.Include("EstadosEnvio")
                .Include("OrganismosEmisores").Include("Personas").Include("Vinculos")
                .Where(i => i.Identificador == identificador).FirstOrDefault();

            return envio;
        }

        public Envios GetEnvioDetalle(string identificador)
        {
            Envios envio = null;
            using (var db = new GestNotifContext())
            {
                envio = db.Envios.Include("EstadosEnvio").Include("OrganismosEmisores").Include("Personas").Include("Vinculos")
                    .Include("TiposEnvio").Where(i => i.Identificador == identificador).FirstOrDefault();
            }

            return envio;
        }

        public DateTime GetEnvioFechaCaducidad(string identificador)
        {
            using (var db = new GestNotifContext())
            {
                return db.Envios.Where(i => i.Identificador == identificador)
                    .Select(i => i.FechaPuestaDisposicion)
                    .FirstOrDefault()
                    .AddDays(Alertas.DiasCaducidad);
            }
        }

        public Envios GetEnvioAuditoria(string identificador)
        {
            Envios envio = null;
            using (var db = new GestNotifContext())
            {
                envio = db.Envios.Include("RepuestasLocaliza")
                    .Where(i => i.Identificador == identificador).FirstOrDefault();
            }

            return envio;
        }

        public int GetEnvioEstado(string identificador)
        {
            using (var db = new GestNotifContext())
            {
                return db.Envios.Include("EstadosEnvio")
                    .Where(i => i.Identificador == identificador)
                    .Select(i => i.EstadosEnvio.ID)
                    .FirstOrDefault();
            }
        }

        public object GetEnviosBuscadorComplejo(int idTipoEnvio, string[] idEstados, int idUsuario, int idPerfil,
            string identificador = null, string administracionPublica = null, string organismoRaiz = null,
            int? idOrganoEmisor = null, string asunto = null, int? idGrupo = null, DateTime? fechaDesde = null, DateTime? fechaHasta = null)
        {
            Alertas al = new Alertas();
            al.SetDiasCaducidad();

            List<Envios> listaEnvios = new List<Envios>();

            using (var db = new GestNotifContext())
            {
                bool tieneEstado = idEstados.Length > 0;

                if (idPerfil == (int)Perfil.Administrador || idPerfil == (int)Perfil.UsuarioConsulta)
                {
                    //Se obtienen una lista con los identificadores de los envios.
                    List<string> listaEnviosTemp = null;

                    if (idGrupo != null)
                    {
                        listaEnviosTemp = db.Envios
                            .Join(db.GruposEnvios,
                                e1 => e1.Identificador,
                                e2 => e2.Envios.Identificador,
                                (e1, e2) => new
                                {
                                    Envio = e1,
                                    Grupo = e2
                                })
                            .Where(i => i.Envio.TiposEnvio.ID == idTipoEnvio &&
                                ((tieneEstado && idEstados.Contains(i.Envio.EstadosEnvio.ID.ToString())) || !tieneEstado) &&
                                ((identificador != null && i.Envio.Identificador == identificador) || identificador == null) &&
                                ((administracionPublica != "Seleccionar" && i.Envio.OrganismosEmisores.AdministracionPublica == administracionPublica) || administracionPublica == "Seleccionar") &&
                                ((organismoRaiz != "Seleccionar" && i.Envio.OrganismosEmisores.OrganismoRaiz == organismoRaiz) || organismoRaiz == "Seleccionar") &&
                                ((idOrganoEmisor.HasValue && i.Envio.OrganismosEmisores.ID == idOrganoEmisor.Value) || !idOrganoEmisor.HasValue) &&
                                ((asunto != null && i.Envio.Concepto == asunto) || asunto == null) &&
                                ((idGrupo.HasValue && i.Grupo.Grupos.ID == idGrupo.Value) || !idGrupo.HasValue) &&
                                ((fechaDesde.HasValue && i.Envio.FechaPuestaDisposicion >= fechaDesde) || !fechaDesde.HasValue) &&
                                ((fechaHasta.HasValue && i.Envio.FechaPuestaDisposicion <= fechaHasta) || !fechaHasta.HasValue))
                            .Select(i => i.Envio.Identificador).Distinct().ToList();
                    }
                    else
                    {
                        listaEnviosTemp = db.Envios
                            .Where(i => i.TiposEnvio.ID == idTipoEnvio &&
                                ((tieneEstado && idEstados.Contains(i.EstadosEnvio.ID.ToString())) || !tieneEstado) &&
                                ((identificador != null && i.Identificador == identificador) || identificador == null) &&
                                ((administracionPublica != "Seleccionar" && i.OrganismosEmisores.AdministracionPublica == administracionPublica) || administracionPublica == "Seleccionar") &&
                                ((organismoRaiz != "Seleccionar" && i.OrganismosEmisores.OrganismoRaiz == organismoRaiz) || organismoRaiz == "Seleccionar") &&
                                ((idOrganoEmisor.HasValue && i.OrganismosEmisores.ID == idOrganoEmisor.Value) || !idOrganoEmisor.HasValue) &&
                                ((asunto != null && i.Concepto == asunto) || asunto == null) &&
                                ((fechaDesde.HasValue && i.FechaPuestaDisposicion >= fechaDesde) || !fechaDesde.HasValue) &&
                                ((fechaHasta.HasValue && i.FechaPuestaDisposicion <= fechaHasta) || !fechaHasta.HasValue))
                            .Select(i => i.Identificador).Distinct().ToList();
                    }

                    //Se obtienen la lista definitiva con todas las propiedades
                    listaEnvios = db.Envios.Include("OrganismosEmisores").Include("TiposEnvio").Include("EstadosEnvio")
                        .Where(i => listaEnviosTemp.Contains(i.Identificador))
                        .OrderByDescending(i => i.FechaPuestaDisposicion).ToList();
                }

                if (idPerfil == (int)Perfil.Asignador)
                {
                    //Se obtienen una lista con los identificadores de los envios.
                    List<string> listaEnviosTemp = null;

                    if (idGrupo != null)
                    {
                        listaEnviosTemp = db.Envios
                            .Join(db.GruposEnvios,
                                e1 => e1.Identificador,
                                e2 => e2.Envios.Identificador,
                                (e1, e2) => new
                                {
                                    Envio = e1,
                                    Grupo = e2
                                })
                            .Where(i => i.Envio.TiposEnvio.ID == idTipoEnvio &&
                                ((tieneEstado && idEstados.Contains(i.Envio.EstadosEnvio.ID.ToString())) || !tieneEstado) &&
                                ((identificador != null && i.Envio.Identificador == identificador) || identificador == null) &&
                                ((administracionPublica != "Seleccionar" && i.Envio.OrganismosEmisores.AdministracionPublica == administracionPublica) || administracionPublica == "Seleccionar") &&
                                ((organismoRaiz != "Seleccionar" && i.Envio.OrganismosEmisores.OrganismoRaiz == organismoRaiz) || organismoRaiz == "Seleccionar") &&
                                ((idOrganoEmisor.HasValue && i.Envio.OrganismosEmisores.ID == idOrganoEmisor.Value) || !idOrganoEmisor.HasValue) &&
                                ((asunto != null && i.Envio.Concepto == asunto) || asunto == null) &&
                                ((idGrupo.HasValue && i.Grupo.Grupos.ID == idGrupo.Value) || !idGrupo.HasValue) &&
                                ((fechaDesde.HasValue && i.Envio.FechaPuestaDisposicion >= fechaDesde) || !fechaDesde.HasValue) &&
                                ((fechaHasta.HasValue && i.Envio.FechaPuestaDisposicion <= fechaHasta) || !fechaHasta.HasValue))
                            .Select(i => i.Envio.Identificador).Distinct().ToList();
                    }
                    else
                    {
                        listaEnviosTemp = db.Envios
                            .Where(i => i.TiposEnvio.ID == idTipoEnvio &&
                                ((tieneEstado && idEstados.Contains(i.EstadosEnvio.ID.ToString())) || !tieneEstado) &&
                                ((identificador != null && i.Identificador == identificador) || identificador == null) &&
                                ((administracionPublica != "Seleccionar" && i.OrganismosEmisores.AdministracionPublica == administracionPublica) || administracionPublica == "Seleccionar") &&
                                ((organismoRaiz != "Seleccionar" && i.OrganismosEmisores.OrganismoRaiz == organismoRaiz) || organismoRaiz == "Seleccionar") &&
                                ((idOrganoEmisor.HasValue && i.OrganismosEmisores.ID == idOrganoEmisor.Value) || !idOrganoEmisor.HasValue) &&
                                ((asunto != null && i.Concepto == asunto) || asunto == null) &&
                                ((fechaDesde.HasValue && i.FechaPuestaDisposicion >= fechaDesde) || !fechaDesde.HasValue) &&
                                ((fechaHasta.HasValue && i.FechaPuestaDisposicion <= fechaHasta) || !fechaHasta.HasValue))
                            .Select(i => i.Identificador).Distinct().ToList();
                    }

                    //Se obtienen la lista definitiva con todas las propiedades
                    listaEnvios = db.Envios.Include("OrganismosEmisores").Include("TiposEnvio").Include("EstadosEnvio")
                        .Where(i => listaEnviosTemp.Contains(i.Identificador) &&
                            String.IsNullOrEmpty(i.GruposAsignadosCadena))
                        .OrderByDescending(i => i.FechaPuestaDisposicion).ToList();
                }

                //El perfil gestor solo tiene que ver las notificaciones que esten asignadas a los grupos a los que pertenece.
                if (idPerfil == (int)Perfil.Gestor)
                {
                    //Se obtienen una lista con los ids de los grupos a los que pertenece el usuario
                    GruposUsuarios gu = new GruposUsuarios();
                    List<string> listaIdGrupos = gu.GetListaIdsGrupoPorUsuario(idUsuario);

                    //Se obtienen una lista con los identificadores de los envios asociados a los grupos
                    List<string> listaEnviosTemp = db.Envios
                        .Join(db.GruposEnvios,
                            e1 => e1.Identificador,
                            e2 => e2.Envios.Identificador,
                            (e1, e2) => new
                            {
                                Envio = e1,
                                Grupo = e2
                            })
                        .Where(i => i.Envio.TiposEnvio.ID == idTipoEnvio &&
                            ((tieneEstado && idEstados.Contains(i.Envio.EstadosEnvio.ID.ToString())) || !tieneEstado) &&
                            ((identificador != null && i.Envio.Identificador == identificador) || identificador == null) &&
                            ((administracionPublica != "Seleccionar" && i.Envio.OrganismosEmisores.AdministracionPublica == administracionPublica) || administracionPublica == "Seleccionar") &&
                            ((organismoRaiz != "Seleccionar" && i.Envio.OrganismosEmisores.OrganismoRaiz == organismoRaiz) || organismoRaiz == "Seleccionar") &&
                            ((idOrganoEmisor.HasValue && i.Envio.OrganismosEmisores.ID == idOrganoEmisor.Value) || !idOrganoEmisor.HasValue) &&
                            ((asunto != null && i.Envio.Concepto == asunto) || asunto == null) &&
                            (listaIdGrupos.Contains(i.Grupo.Grupos.ID.ToString())) &&
                            ((fechaDesde.HasValue && i.Envio.FechaPuestaDisposicion >= fechaDesde) || !fechaDesde.HasValue) &&
                            ((fechaHasta.HasValue && i.Envio.FechaPuestaDisposicion <= fechaHasta) || !fechaHasta.HasValue))
                        .Select(i => i.Envio.Identificador).Distinct().ToList();

                    //Se obtienen la lista definitiva con todas las propiedades
                    listaEnvios = db.Envios.Include("OrganismosEmisores").Include("TiposEnvio").Include("EstadosEnvio")
                        .Where(i => listaEnviosTemp.Contains(i.Identificador))
                        .OrderByDescending(i => i.FechaPuestaDisposicion).ToList();
                }
            }

            ResultadoEnvios res = new ResultadoEnvios
            {
                ListaEnvios = listaEnvios
            };

            return res;
        }

        public List<Envios> GetEnviosTodosWinService()
        {
            Alertas al = new Alertas();
            al.SetDiasCaducidad();

            List<Envios> listaEnviosTodosLosEstados = new List<Envios>();

            using (var db = new GestNotifContext())
            {
                listaEnviosTodosLosEstados = db.Envios.ToList();
            }

            return listaEnviosTodosLosEstados;
        }

        public ResultadoEnvios GetEnvios(int idTipoEnvio, string[] idEstados, int idUsuario, int idPerfil)
        {
            Alertas al = new Alertas();
            al.SetDiasCaducidad();

            IEnumerable<Envios> listaEnviosTodosLosEstadosTotal = null;
            IEnumerable<Envios> listaEnviosTodosLosEstados = null;
            List<Envios> listaEnvios = new List<Envios>();

            using (var db = new GestNotifContext())
            {
                //Se limita por defecto a los envios de los últimos 30 días
                ParametrosInternos pi = new ParametrosInternos();
                DateTime fechaMaxima = DateTime.Now.AddDays(Convert.ToInt32(pi.GetParametroInternoPorId((int)ParametroInterno.LimiteCargaNotificaciones).Valor));

                bool tieneEstado = idEstados.Length > 0;

                if (idPerfil == (int)Perfil.Administrador || idPerfil == (int)Perfil.UsuarioConsulta)
                {
                    listaEnviosTodosLosEstadosTotal = db.Envios.Include("OrganismosEmisores").Include("TiposEnvio").Include("EstadosEnvio")
                        .AsNoTracking()
                        .Where(i => i.TiposEnvio.ID == idTipoEnvio)
                        .AsEnumerable();

                    //Se obtienen los envios de todos los estados para calcular su cantidad posteriormente (de los últimos 30 días)
                    listaEnviosTodosLosEstados = listaEnviosTodosLosEstadosTotal
                        .Where(i => i.TiposEnvio.ID == idTipoEnvio &&
                            (i.FechaPuestaDisposicion >= fechaMaxima))
                        .AsEnumerable();

                    listaEnvios = listaEnviosTodosLosEstados
                        .Where(i => (tieneEstado && idEstados.Contains(i.EstadosEnvio.ID.ToString())) || !tieneEstado)
                        .OrderByDescending(i => i.FechaPuestaDisposicion).ToList();
                }

                if (idPerfil == (int)Perfil.Asignador)
                {
                    /* ¡ATENCIÓN!
                        El asignador tiene que ver todas los envíos que esten sin asignar independiente de su estado
                    */

                    //Se obtienen los envios de todos los estados sin límite de fecha.
                    //Esta varible se usa para calcular las cantidades totales.
                    listaEnviosTodosLosEstadosTotal = db.Envios.Include("OrganismosEmisores").Include("TiposEnvio").Include("EstadosEnvio")
                        .AsNoTracking()
                        .Where(i => i.TiposEnvio.ID == idTipoEnvio)
                        .AsEnumerable();

                    //Se obtienen los envios de todos los estados de los últimos 30 días.
                    //Esta variable se usa para calcular las cantidades de los últimos 30 días.
                    listaEnviosTodosLosEstados = listaEnviosTodosLosEstadosTotal
                        .Where(i => i.TiposEnvio.ID == idTipoEnvio &&
                            (i.FechaPuestaDisposicion >= fechaMaxima))
                        .AsEnumerable();

                    //Se obtienen los envíos que no tengan ningún grupo asignado.
                    listaEnvios = listaEnviosTodosLosEstados
                        .Where(i => String.IsNullOrEmpty(i.GruposAsignadosCadena))
                        .OrderByDescending(i => i.FechaPuestaDisposicion).ToList();
                }

                if (idPerfil == (int)Perfil.Gestor)
                {
                    /* El perfil gestor solo tiene que ver las notificaciones que esten asignadas a los grupos a los que pertenece. */

                    //Se obtienen una lista con los ids de los grupos a los que pertenece el usuario
                    GruposUsuarios gu = new GruposUsuarios();
                    List<string> listaIdGrupos = gu.GetListaIdsGrupoPorUsuario(idUsuario);

                    //Se obtienen una lista con los identificadores de los envios asociados a los grupos
                    List<string> listaEnviosTemp = db.Envios.Include("OrganismosEmisores").Include("TiposEnvio").Include("EstadosEnvio")
                        .AsNoTracking()
                        .Join(db.GruposEnvios,
                            e1 => e1.Identificador,
                            e2 => e2.Envios.Identificador,
                            (e1, e2) => new
                            {
                                Envio = e1,
                                Grupo = e2
                            })
                        .Where(i =>
                            (i.Envio.TiposEnvio.ID == idTipoEnvio) &&
                            //((tieneEstado && idEstados.Contains(i.Envio.EstadosEnvio.ID.ToString())) || !tieneEstado) &&
                            (listaIdGrupos.Contains(i.Grupo.Grupos.ID.ToString())))
                        .Select(i => i.Envio.Identificador).Distinct().ToList();

                    listaEnviosTodosLosEstadosTotal = db.Envios.Include("OrganismosEmisores").Include("TiposEnvio").Include("EstadosEnvio")
                        .AsNoTracking()
                        .Where(i => (i.TiposEnvio.ID == idTipoEnvio) &&
                            (listaEnviosTemp.Contains(i.Identificador)))
                        .AsEnumerable();

                    //Se obtienen los envios de todos los estados para calcular su cantidad posteriormente (de los últimos 30 días)
                    listaEnviosTodosLosEstados = listaEnviosTodosLosEstadosTotal
                        .Where(i => (i.FechaPuestaDisposicion >= fechaMaxima))
                        .AsEnumerable();

                    //Se obtienen la lista definitiva con todas las propiedades
                    listaEnvios = listaEnviosTodosLosEstados
                        .Where(i => ((tieneEstado && idEstados.Contains(i.EstadosEnvio.ID.ToString())) || !tieneEstado))
                        .OrderByDescending(i => i.FechaPuestaDisposicion).ToList();
                }

                ResultadoEnvios res = new ResultadoEnvios
                {
                    ListaEnvios = listaEnvios,
                    //Cantidades = GetCantidadEnvios(idTipoEnvio, listaEnviosTodosLosEstados, idPerfil),
                    CantidadesTotales = GetCantidadEnvios(idTipoEnvio, listaEnviosTodosLosEstadosTotal, idPerfil),
                };

                return res;
            }
        }

        public ResultadoEnvios GetEnviosPorEstado(int idTipoEnvio, string[] idEstados, int idUsuario, int idPerfil, bool primeros)
        {
            Alertas al = new Alertas();
            al.SetDiasCaducidad();

            int limite = 10;

            List<Envios> listaEnviosTodosLosEstados = new List<Envios>();
            List<Envios> listaEnvios = new List<Envios>();

            using (var db = new GestNotifContext())
            {
                bool tieneEstado = idEstados.Length > 0;

                if (idPerfil == (int)Perfil.Administrador || idPerfil == (int)Perfil.UsuarioConsulta)
                {
                    //Se obtienen los envios de todos los estados para calcular su cantidad posteriormente
                    listaEnviosTodosLosEstados = db.Envios.Include("OrganismosEmisores").Include("TiposEnvio").Include("EstadosEnvio")
                        .AsNoTracking()
                        .Where(i => i.TiposEnvio.ID == idTipoEnvio)
                        .ToList();

                    if (primeros)
                    {
                        listaEnvios = listaEnviosTodosLosEstados
                            .Where(i => (tieneEstado && idEstados.Contains(i.EstadosEnvio.ID.ToString())) || !tieneEstado)
                            .OrderByDescending(i => i.FechaPuestaDisposicion)
                            //.Take(limite)
                            .ToList();
                    }
                    else
                    {
                        try
                        {
                            listaEnvios = listaEnviosTodosLosEstados
                                .Where(i => (tieneEstado && idEstados.Contains(i.EstadosEnvio.ID.ToString())) || !tieneEstado)
                                .OrderByDescending(i => i.FechaPuestaDisposicion)
                                .Skip(limite).ToList();
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }

                if (idPerfil == (int)Perfil.Asignador)
                {
                    /* El asignador tiene que ver todas los envíos que esten sin asignar independiente de su estado */

                    //Se obtienen los envios de todos los estados para calcular su cantidad posteriormente (los primeros 1000)
                    listaEnviosTodosLosEstados = db.Envios.Include("OrganismosEmisores").Include("TiposEnvio").Include("EstadosEnvio")
                        .AsNoTracking()
                        .Where(i => i.TiposEnvio.ID == idTipoEnvio)
                        .ToList();

                    if (primeros)
                    {
                        listaEnvios = listaEnviosTodosLosEstados
                            .Where(i => String.IsNullOrEmpty(i.GruposAsignadosCadena))
                            .OrderByDescending(i => i.FechaPuestaDisposicion)
                            //.Take(limite)
                            .ToList();
                    }
                    else
                    {
                        try
                        {
                            listaEnvios = listaEnviosTodosLosEstados
                                .Where(i => String.IsNullOrEmpty(i.GruposAsignadosCadena))
                                .OrderByDescending(i => i.FechaPuestaDisposicion)
                                .Skip(limite).ToList();
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }

                if (idPerfil == (int)Perfil.Gestor)
                {
                    /* El perfil gestor solo tiene que ver las notificaciones que esten asignadas a los grupos a los que pertenece. */

                    //Se obtienen una lista con los ids de los grupos a los que pertenece el usuario
                    GruposUsuarios gu = new GruposUsuarios();
                    List<string> listaIdGrupos = gu.GetListaIdsGrupoPorUsuario(idUsuario);

                    //Se obtienen una lista con los identificadores de los envios asociados a los grupos
                    List<string> listaEnviosTemp = db.Envios.Include("OrganismosEmisores").Include("TiposEnvio").Include("EstadosEnvio")
                        .AsNoTracking()
                        .Join(db.GruposEnvios,
                            e1 => e1.Identificador,
                            e2 => e2.Envios.Identificador,
                            (e1, e2) => new
                            {
                                Envio = e1,
                                Grupo = e2
                            })
                        .Where(i =>
                            (i.Envio.TiposEnvio.ID == idTipoEnvio) &&
                            //((tieneEstado && idEstados.Contains(i.Envio.EstadosEnvio.ID.ToString())) || !tieneEstado) &&
                            (listaIdGrupos.Contains(i.Grupo.Grupos.ID.ToString())))
                        .Select(i => i.Envio.Identificador).Distinct().ToList();

                    //Se obtienen los envios de todos los estados para calcular su cantidad posteriormente
                    listaEnviosTodosLosEstados = db.Envios.Include("OrganismosEmisores").Include("TiposEnvio").Include("EstadosEnvio")
                        .AsNoTracking()
                        .Where(i => (i.TiposEnvio.ID == idTipoEnvio) &&
                            (listaEnviosTemp.Contains(i.Identificador)))
                        .ToList();

                    if (primeros)
                    {
                        //Se obtienen la lista definitiva con todas las propiedades
                        listaEnvios = listaEnviosTodosLosEstados
                            .Where(i => ((tieneEstado && idEstados.Contains(i.EstadosEnvio.ID.ToString())) || !tieneEstado))
                            .OrderByDescending(i => i.FechaPuestaDisposicion)
                            //.Take(limite)
                            .ToList();
                    }
                    else
                    {
                        //Últimos 1000
                        try
                        {
                            //Se obtienen la lista definitiva con todas las propiedades
                            listaEnvios = listaEnviosTodosLosEstados
                                .Where(i => ((tieneEstado && idEstados.Contains(i.EstadosEnvio.ID.ToString())) || !tieneEstado))
                                .OrderByDescending(i => i.FechaPuestaDisposicion)
                                .Skip(limite).ToList();
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }

                ResultadoEnvios res = new ResultadoEnvios
                {
                    ListaEnvios = listaEnvios,
                    CantidadesTotales = GetCantidadEnvios(idTipoEnvio, listaEnviosTodosLosEstados, idPerfil)
                };

                return res;
            }
        }

        public ResultadoEnvios GetResultadoCantidadEnvios(int idTipoEnvio, int idUsuario, int idPerfil)
        {
            IEnumerable<Envios> listaEnviosTodosLosEstados = null;

            using (var db = new GestNotifContext())
            {
                //Se limita por defecto a los envios del último mes
                //ParametrosInternos pi = new ParametrosInternos();
                //DateTime fechaMaxima = DateTime.Now.AddDays(Convert.ToInt32(pi.GetParametroInternoPorId(1).Valor));

                if (idPerfil == (int)Perfil.Administrador || idPerfil == (int)Perfil.UsuarioConsulta)
                {
                    //Se obtienen los envios de todos los estados para calcular su cantidad posteriormente
                    listaEnviosTodosLosEstados = db.Envios.Include("OrganismosEmisores").Include("TiposEnvio").Include("EstadosEnvio")
                        .AsNoTracking()
                        .Where(i => i.TiposEnvio.ID == idTipoEnvio)
                        .AsEnumerable();
                }

                if (idPerfil == (int)Perfil.Asignador)
                {
                    listaEnviosTodosLosEstados = db.Envios.Include("OrganismosEmisores").Include("TiposEnvio").Include("EstadosEnvio")
                        .AsNoTracking()
                        .Where(i => i.TiposEnvio.ID == idTipoEnvio &&
                            (String.IsNullOrEmpty(i.GruposAsignadosCadena)))
                        .AsEnumerable();
                }

                //El perfil gestor solo tiene que ver las notificaciones que esten asignadas a los grupos a los que pertenece.
                if (idPerfil == (int)Perfil.Gestor)
                {
                    //Se obtienen una lista con los ids de los grupos a los que pertenece el usuario
                    GruposUsuarios gu = new GruposUsuarios();
                    List<string> listaIdGrupos = gu.GetListaIdsGrupoPorUsuario(idUsuario);

                    //Se obtienen una lista con los identificadores de los envios asociados a los grupos
                    List<string> listaEnviosTemp = db.Envios.Include("OrganismosEmisores").Include("TiposEnvio").Include("EstadosEnvio")
                        .AsNoTracking()
                        .Join(db.GruposEnvios,
                            e1 => e1.Identificador,
                            e2 => e2.Envios.Identificador,
                            (e1, e2) => new
                            {
                                Envio = e1,
                                Grupo = e2
                            })
                        .Where(i =>
                            (i.Envio.TiposEnvio.ID == idTipoEnvio) &&
                            //((tieneEstado && idEstados.Contains(i.Envio.EstadosEnvio.ID.ToString())) || !tieneEstado) &&
                            (listaIdGrupos.Contains(i.Grupo.Grupos.ID.ToString())))
                        .Select(i => i.Envio.Identificador).Distinct().ToList();

                    //Se obtienen los envios de todos los estados para calcular su cantidad posteriormente
                    listaEnviosTodosLosEstados = db.Envios.Include("OrganismosEmisores").Include("TiposEnvio").Include("EstadosEnvio")
                        .AsNoTracking()
                        .Where(i =>
                            (i.TiposEnvio.ID == idTipoEnvio) &&
                            (listaEnviosTemp.Contains(i.Identificador)))
                        .AsEnumerable();
                }

                ResultadoEnvios res = new ResultadoEnvios
                {
                    ListaEnvios = null,
                    CantidadesTotales = GetCantidadEnvios(idTipoEnvio, listaEnviosTodosLosEstados, idPerfil)
                };

                return res;
            }
        }

        public List<KeyValuePair<string, int>> GetCantidadEnvios(int idTipoEnvio, IEnumerable<Envios> listaEnvios, int idPerfil)
        {
            //using (var db = new GestNotifContext())
            //{
            if (idPerfil == (int)Perfil.Asignador)
            {
                var list = new List<KeyValuePair<string, int>>
                    {
                        new KeyValuePair<string, int>("Sin Asignar", listaEnvios.Where(i => String.IsNullOrEmpty(i.GruposAsignadosCadena) && i.TiposEnvio.ID == idTipoEnvio).Count()),
                        new KeyValuePair<string, int>("En Alerta", 0),
                        new KeyValuePair<string, int>("Asignadas", 0),
                        new KeyValuePair<string, int>("Comparecidas", 0),
                        new KeyValuePair<string, int>("Caducadas", 0),
                        new KeyValuePair<string, int>("Leídas", 0),
                        new KeyValuePair<string, int>("Externas", 0)
                    };
                return list;
            }
            else
            {
                var list = new List<KeyValuePair<string, int>>
                    {
                        new KeyValuePair<string, int>("Sin Asignar", listaEnvios.Where(i => i.EstadosEnvio.ID == (int)EstadoEnvio.SinAsignar && i.TiposEnvio.ID == idTipoEnvio).Count()),
                        new KeyValuePair<string, int>("En Alerta", listaEnvios.Where(i => i.EstadosEnvio.ID == (int)EstadoEnvio.EnAlerta && i.TiposEnvio.ID == idTipoEnvio).Count()),
                        new KeyValuePair<string, int>("Asignadas", listaEnvios.Where(i => i.EstadosEnvio.ID == (int)EstadoEnvio.Asignada && i.TiposEnvio.ID == idTipoEnvio).Count()),
                        new KeyValuePair<string, int>("Comparecidas", listaEnvios.Where(i => i.EstadosEnvio.ID == (int)EstadoEnvio.Comparecida && i.TiposEnvio.ID == idTipoEnvio).Count()),
                        new KeyValuePair<string, int>("Caducadas", listaEnvios.Where(i => i.EstadosEnvio.ID == (int)EstadoEnvio.Caducada && i.TiposEnvio.ID == idTipoEnvio).Count()),
                        new KeyValuePair<string, int>("Leídas", listaEnvios.Where(i => i.EstadosEnvio.ID == (int)EstadoEnvio.Leida && i.TiposEnvio.ID == idTipoEnvio).Count()),
                        new KeyValuePair<string, int>("Externas", listaEnvios.Where(i => i.EstadosEnvio.ID == (int)EstadoEnvio.Externas && i.TiposEnvio.ID == idTipoEnvio).Count())
                    };
                return list;
            }
            //}
        }

        public List<string> GetAsuntos(int idTipoEnvio, int? idEstado)
        {
            using (var db = new GestNotifContext())
            {
                if (idEstado != null)
                    return db.Envios.Where(i => i.TiposEnvio.ID == idTipoEnvio && i.EstadosEnvio.ID == idEstado).Select(i => i.Concepto).Distinct().ToList();
                else
                    return db.Envios.Where(i => i.TiposEnvio.ID == idTipoEnvio).Select(i => i.Concepto).Distinct().ToList();
            }
        }

        public void InsertEnvioContext(string identificador, int codigoOrigen, string concepto, string descripcion,
            DateTime fechaPuestaDisposicion, string metadatosPublicos, int idVinculo, int idTipoEnvio,
            int idOrganismoEmisor, int? idPersona, int idRespuestaLocaliza, int idEstadoEnvio, GestNotifContext db = null)
        {
            Envios en = new Envios
            {
                Identificador = identificador,
                CodigoOrigen = codigoOrigen,
                Concepto = concepto,
                Descripcion = descripcion,
                FechaPuestaDisposicion = fechaPuestaDisposicion,
                MetadatosPublicos = metadatosPublicos,
                Vinculos = db.Vinculos.Where(i => i.ID == idVinculo).FirstOrDefault(),
                TiposEnvio = db.TiposEnvio.Where(i => i.ID == idTipoEnvio).FirstOrDefault(),
                OrganismosEmisores = db.OrganismosEmisores.Where(i => i.ID == idOrganismoEmisor).FirstOrDefault(),
                Personas = db.Persona.Where(i => i.ID == idPersona).FirstOrDefault(),
                RepuestasLocaliza = db.RespuestasLocaliza.Where(i => i.ID == idRespuestaLocaliza).FirstOrDefault(),

                EstadosEnvio = db.EstadosEnvios.Where(i => i.ID == idEstadoEnvio).FirstOrDefault(),
            };

            db.Envios.Add(en);
            db.SaveChanges();

            //Se registra en el histórico
            HistorialCambiosEnvio hce = new HistorialCambiosEnvio
            {
                Envios_Identificador = identificador,
                Usuarios_ID = 0,
                TiposCambioEnvio_ID = (int)TipoCambioEnvio.CambioEstado,
                Fecha = DateTime.Now,
                Grupos_ID = 0,
                EstadosEnvio_ID = (int)EstadoEnvio.SinAsignar
            };

            db.HistorialCambiosEnvio.Add(hce);
            db.SaveChanges();
        }

        public void InsertErroresEnvio(string codigo, string descripcion, string metodo, string identificador)
        {
            using (var db = new GestNotifContext())
            {
                Envios envios = db.Envios.Where(l => l.Identificador == identificador).FirstOrDefault();
                envios.ErrorCodigo = codigo;
                envios.ErrorDescripcion = descripcion;
                envios.ErrorMetodo = metodo;
                envios.ErrorFecha = DateTime.Now;

                db.SaveChanges();
            }
        }

        public void InsertErroresEnvioContext(string message, string localizedMessage, string metodo, string _identificador, GestNotifContext db)
        {
            Envios envios = db.Envios.Where(l => l.Identificador == _identificador).FirstOrDefault();

            envios.ErrorCodigo = message;
            envios.ErrorDescripcion = localizedMessage;
            envios.ErrorMetodo = metodo;
            envios.ErrorFecha = DateTime.Now;

            db.SaveChanges();
        }

        public void SetEstadoEnvio(string identificador, EstadoEnvio estadoEnvio, int idUsuarioCambio)
        {
            using (var db = new GestNotifContext())
            {
                EstadosEnvios nuevoEstado = db.EstadosEnvios
                    .Where(i => i.ID == (int)estadoEnvio).FirstOrDefault();

                Envios envio = db.Envios.Include("EstadosEnvio")
                    .Where(i => i.Identificador == identificador).FirstOrDefault();

                if (nuevoEstado != envio.EstadosEnvio)
                {
                    //Se cambia el estado
                    envio.EstadosEnvio = nuevoEstado;
                    db.SaveChanges();

                    //Se registra en el histórico
                    HistorialCambiosEnvio hce = new HistorialCambiosEnvio
                    {
                        Envios_Identificador = identificador,
                        Usuarios_ID = idUsuarioCambio,
                        TiposCambioEnvio_ID = (int)TipoCambioEnvio.CambioEstado,
                        Fecha = DateTime.Now,
                        Grupos_ID = 0,
                        EstadosEnvio_ID = nuevoEstado.ID
                    };

                    db.HistorialCambiosEnvio.Add(hce);
                    db.SaveChanges();
                }
            }
        }

        public void SetEstadoEnvioContext(string identificador, EstadoEnvio estadoEnvio, int idUsuarioCambio, GestNotifContext db)
        {
            EstadosEnvios nuevoEstado = db.EstadosEnvios
                .Where(i => i.ID == (int)estadoEnvio).FirstOrDefault();

            Envios envio = db.Envios.Include("EstadosEnvio")
                .Where(i => i.Identificador == identificador).FirstOrDefault();

            if (nuevoEstado != envio.EstadosEnvio)
            {
                //Se cambia el estado
                envio.EstadosEnvio = nuevoEstado;
                db.SaveChanges();

                //Se registra en el histórico
                HistorialCambiosEnvio hce = new HistorialCambiosEnvio
                {
                    Envios_Identificador = identificador,
                    Usuarios_ID = idUsuarioCambio,
                    TiposCambioEnvio_ID = (int)TipoCambioEnvio.CambioEstado,
                    Fecha = DateTime.Now,
                    Grupos_ID = 0,
                    EstadosEnvio_ID = (int)estadoEnvio
                };

                db.HistorialCambiosEnvio.Add(hce);
                db.SaveChanges();
            }
        }

        public void SetGruposAsignadosCadena(string identificador, string cadena)
        {
            using (var db = new GestNotifContext())
            {
                db.Envios.Where(i => i.Identificador == identificador).FirstOrDefault().GruposAsignadosCadena = cadena;
                db.SaveChanges();
            }
        }

        public void SetGruposAsignadosCadenaContext(string identificador, string cadena, GestNotifContext db)
        {
            db.Envios.Where(i => i.Identificador == identificador).FirstOrDefault().GruposAsignadosCadena = cadena;
            db.SaveChanges();
        }

        public bool TieneUsuarioEnviosEnAlerta(int idUsuario)
        {
            bool tieneEnviosEnAlerta = false;

            using (var db = new GestNotifContext())
            {
                //Se limita por defecto a los envios del último mes
                //ParametrosInternos pi = new ParametrosInternos();
                //DateTime fechaMaxima = DateTime.Now.AddDays(Convert.ToInt32(pi.GetParametroInternoPorId(1).Valor));

                //Se obtienen una lista con los ids de los grupos a los que pertenece el usuario
                GruposUsuarios gu = new GruposUsuarios();
                List<string> listaIdGrupos = gu.GetListaIdsGrupoPorUsuario(idUsuario);

                //Se obtienen una lista con los identificadores de los envios asociados a los grupos
                int numEnviosEnAlerta = db.Envios
                    .Join(db.GruposEnvios,
                        e1 => e1.Identificador,
                        e2 => e2.Envios.Identificador,
                        (e1, e2) => new
                        {
                            Envio = e1,
                            Grupo = e2
                        })
                    .Where(i => listaIdGrupos.Contains(i.Grupo.Grupos.ID.ToString()) &&
                        (i.Envio.EstadosEnvio.ID == (int)EstadoEnvio.EnAlerta))
                    .Select(i => i.Envio.Identificador).Distinct().ToList().Count();

                tieneEnviosEnAlerta = (numEnviosEnAlerta > 0);
            }

            return tieneEnviosEnAlerta;
        }

        public void SetObservaciones(string identificador, string observaciones)
        {
            using (var db = new GestNotifContext())
            {
                db.Envios
                    .Where(i => i.Identificador == identificador)
                    .FirstOrDefault().Observaciones = observaciones;

                db.SaveChanges();
            }
        }

        public List<Envios> GetEnviosByIdOrganismo(int idOrganismo)
        {
            using (var db = new GestNotifContext())
            {
                return db.Envios.Where(i => i.OrganismosEmisores.ID == idOrganismo).ToList();
            }
        }
    }

    [Table("Personas")]
    public partial class Personas : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private string nombreTitularField;
        private string nifTitularField;
        private string codigoDIR3Field;
        private string codigoDIReField;
        private string descripcionEntidadField;

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
        public string NombreTitular
        {
            get
            {
                return this.nombreTitularField;
            }
            set
            {
                this.nombreTitularField = value;
                this.RaisePropertyChanged("nombreTitular");
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
        public string CodigoDIR3
        {
            get
            {
                return this.codigoDIR3Field;
            }
            set
            {
                this.codigoDIR3Field = value;
                this.RaisePropertyChanged("codigoDIR3");
            }
        }

        [MaxLength(255)]
        public string CodigoDIRe
        {
            get
            {
                return this.codigoDIReField;
            }
            set
            {
                this.codigoDIReField = value;
                this.RaisePropertyChanged("codigoDIRe");
            }
        }

        [MaxLength(255)]
        public string DescripcionEntidad
        {
            get
            {
                return this.descripcionEntidadField;
            }
            set
            {
                this.descripcionEntidadField = value;
                this.RaisePropertyChanged("descripcionEntidad");
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

        public int InsertPersonaContext(string nombreTitular, string nifTitular = null, string codigoDIR3 = null, string codigoDIRe = null,
            string descripcionEntidad = null, GestNotifContext db = null)
        {
            int idPersona;

            Personas personaExiste = db.Persona.Where(i => i.NombreTitular == nombreTitular.Trim()).FirstOrDefault();

            if (personaExiste == null)
            {
                Personas pe = new Personas
                {
                    NombreTitular = nombreTitular.Trim(),
                    NifTitular = nifTitular,
                    CodigoDIR3 = codigoDIR3,
                    CodigoDIRe = codigoDIRe,
                    DescripcionEntidad = descripcionEntidad,
                };

                db.Persona.Add(pe);
                db.SaveChanges();

                idPersona = pe.ID;
            }
            else
            {
                idPersona = personaExiste.ID;
            }

            return idPersona;
        }
    }

    [Table("TiposEnvio")]
    public partial class TiposEnvio : object, System.ComponentModel.INotifyPropertyChanged
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

        public List<TiposEnvio> GetTiposEnvio()
        {
            using (var db = new GestNotifContext())
            {
                return db.TiposEnvio.ToList();
            }
        }
    }

    [Table("Vinculos")]
    public partial class Vinculos : object, System.ComponentModel.INotifyPropertyChanged
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

    [Table("EstadosEnvios")]
    public partial class EstadosEnvios : object, System.ComponentModel.INotifyPropertyChanged
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

        public List<EstadosEnvios> GetEstadosEnvio(int idTipoEnvios)
        {
            using (var db = new GestNotifContext())
            {
                if (idTipoEnvios == (int)TipoEnvio.Comunicaciones)
                {
                    return db.EstadosEnvios.AsNoTracking().Where(i => i.ID == (int)EstadoEnvio.SinAsignar || i.ID == (int)EstadoEnvio.Asignada || i.ID == (int)EstadoEnvio.Leida).ToList();
                }
                else
                {
                    return db.EstadosEnvios.AsNoTracking().Where(i => i.ID != (int)EstadoEnvio.Leida).ToList();
                }
            }
        }
    }

    public partial class ResultadoEnvios
    {
        public List<Envios> ListaEnvios;

        //Cantidades de envíos de los últimos 30 días
        //public List<KeyValuePair<string, int>> Cantidades;

        //Cantidades de envíos totales
        public List<KeyValuePair<string, int>> CantidadesTotales;

        //Dias de límite de carga notificaciones
        public int NumeroUltimosDias;
    }

    public enum TipoEnvio
    {
        Comunicaciones = 1,
        Notificaciones = 2
    }

    public enum Vinculo
    {
        Titular = 1,
        Destinatario = 2,
        Apoderado = 3
    }

    public enum EstadoEnvio
    {
        SinAsignar = 1,
        EnAlerta = 2,
        Asignada = 3,
        Comparecida = 4,
        Caducada = 5,
        Leida = 6,
        Externas = 7
    }
}