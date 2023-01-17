using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Validation;
using System.Linq;

namespace PSOENotificaciones.Contexto
{
    [Table("Usuarios")]
    public partial class Usuarios : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private string nombreField;
        private string apellidosField;
        private string nifField;
        private string emailField;
        private string telefonoField;
        private string loginUsuarioField;
        private byte[] loginPasswordField;
        private int? loginNumeroIntentosField;
        private DateTime? fechaUltimoIntentoLoginField;
        private bool activoField;
        private DateTime fechaAltaField;
        private DateTime? fechaBajaField;
        private bool bloqueadoField;
        private DateTime? fechaBloqueadoField;
        private Perfiles perfilesField;
        private string claveRecuperaPasswordField;
        private bool? consentimientoLegalField;
        private DateTime? fechaConsentimientoField;
        private int? dniNumeroIntentosField;

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

        [Required]
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
        public string Apellidos
        {
            get
            {
                return this.apellidosField;
            }
            set
            {
                this.apellidosField = value;
                this.RaisePropertyChanged("apellidosField");
            }
        }

        [Required]
        [MaxLength(9)]
        public string NIF
        {
            get
            {
                return this.nifField;
            }
            set
            {
                this.nifField = value;
                this.RaisePropertyChanged("nifField");
            }
        }

        [Required]
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
                this.RaisePropertyChanged("emailField");
            }
        }

        [MaxLength(50)]
        public string Telefono
        {
            get
            {
                return this.telefonoField;
            }
            set
            {
                this.telefonoField = value;
                this.RaisePropertyChanged("telefonoField");
            }
        }

        [Required]
        [MaxLength(50)]
        public string LoginUsuario
        {
            get
            {
                return this.loginUsuarioField;
            }
            set
            {
                this.loginUsuarioField = value;
                this.RaisePropertyChanged("loginUsuarioField");
            }
        }

        [Required]
        public byte[] LoginPassword
        {
            get
            {
                return this.loginPasswordField;
            }
            set
            {
                this.loginPasswordField = value;
                this.RaisePropertyChanged("loginPasswordField");
            }
        }

        public int? LoginNumeroIntentos
        {
            get
            {
                return this.loginNumeroIntentosField;
            }
            set
            {
                this.loginNumeroIntentosField = value;
                this.RaisePropertyChanged("loginNumeroIntentosField");
            }
        }

        public DateTime? FechaUltimoIntentoLogin
        {
            get
            {
                return this.fechaUltimoIntentoLoginField;
            }
            set
            {
                this.fechaUltimoIntentoLoginField = value;
                this.RaisePropertyChanged("fechaUltimoIntentoLoginField");
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

        public DateTime FechaAlta
        {
            get
            {
                return this.fechaAltaField;
            }
            set
            {
                this.fechaAltaField = value;
                this.RaisePropertyChanged("fechaAltaField");
            }
        }

        public DateTime? FechaBaja
        {
            get
            {
                return this.fechaBajaField;
            }
            set
            {
                this.fechaBajaField = value;
                this.RaisePropertyChanged("fechaBajaField");
            }
        }

        public bool Bloqueado
        {
            get
            {
                return this.bloqueadoField;
            }
            set
            {
                this.bloqueadoField = value;
                this.RaisePropertyChanged("bloqueadoField");
            }
        }

        public DateTime? FechaBloqueado
        {
            get
            {
                return this.fechaBloqueadoField;
            }
            set
            {
                this.fechaBloqueadoField = value;
                this.RaisePropertyChanged("fechaBloqueadoField");
            }
        }

        public Perfiles Perfiles
        {
            get
            {
                return this.perfilesField;
            }
            set
            {
                this.perfilesField = value;
            }
        }

        public string ClaveRecuperaPassword
        {
            get
            {
                return this.claveRecuperaPasswordField;
            }
            set
            {
                this.claveRecuperaPasswordField = value;
                this.RaisePropertyChanged("claveRecuperaPasswordField");
            }
        }

        public bool? ConsentimientoLegal
        {
            get
            {
                return this.consentimientoLegalField;
            }
            set
            {
                this.consentimientoLegalField = value;
                this.RaisePropertyChanged("consentimientoLegalField");
            }
        }

        public DateTime? FechaConsentimiento
        {
            get
            {
                return this.fechaConsentimientoField;
            }
            set
            {
                this.fechaConsentimientoField = value;
                this.RaisePropertyChanged("fechaConsentimientoField");
            }
        }

        public int? DniNumeroIntentos
        {
            get
            {
                return this.dniNumeroIntentosField;
            }
            set
            {
                this.dniNumeroIntentosField = value;
                this.RaisePropertyChanged("dniNumeroIntentosField");
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
        public string CodigoHtmlMenu
        {
            get
            {
                return "<div class='action_button'>" +
                    "<a href='../UsuarioDetalle/UsuarioDetalle?id=" + idField + "'><span class='action_button__icon action_button__icon__person'></span></a>" +
                    "</div>";
            }
        }

        [NotMapped]
        public string CodigoHtmlEditar
        {
            get
            {
                return "<div class='action_button'>" +
                    "   <a href='../Administracion/UsuarioEditar?id=" + idField + "'><span class='action_button__icon action_button__icon__edit' title='Editar usuario'></span></a>" +
                    "</div>";
            }
        }

        [NotMapped]
        public string CodigoHtmlEliminar
        {
            get
            {
                return "<div class='action_button'>" +
                    "<span class='action_button__icon action_button__icon__person_delete' id='spanEliminar'></span>" +
                    "</div>";
            }
        }

        [NotMapped]
        public string CodigoHtmlActivo
        {
            get
            {
                if (activoField)
                    return "<div class='switch-container d-flex'><input type='checkbox' class='checkbox-grupo' id='" + idField + "' checked /><label for='" + idField + "'>Toggle</label></div>";
                else
                    return "<div class='switch-container d-flex'><input type='checkbox' class='checkbox-grupo' id='" + idField + "' /><label for='" + idField + "'>Toggle</label></div>";
            }
        }

        [NotMapped]
        public string CodigoHtmlBloqueado
        {
            get
            {
                if (bloqueadoField)
                    return "<span class='status_icon status_icon__appeared'></span>";
                else
                    return "<span class='status_icon status_icon__rejected'></span>";
            }
        }

        [NotMapped]
        public string CodigoHtmlAsignar
        {
            get
            {
                return "<div class='action_button'>" +
                    "<span class='action_button__icon action_button__icon__assigned-people' id='spanAsignar'></span>" +
                    "</div>";
            }
        }

        [NotMapped]
        public string CodigoHtmlPerfil
        {
            get
            {
                string perfil = string.Empty;
                if (perfilesField != null)
                {
                    if (perfilesField.ID == (int)Perfil.Administrador)
                        perfil = "<span class='profileuser-description profile_icon__admin'></span><strong>" + perfilesField.Nombre + "</strong>";

                    if (perfilesField.ID == (int)Perfil.Asignador)
                        perfil = "<span class='profileuser-description profile_icon__asign'></span><strong>" + perfilesField.Nombre + "</strong>";

                    if (perfilesField.ID == (int)Perfil.Gestor)
                        perfil = "<span class='profileuser-description profile_icon__tramit'></span><strong>" + perfilesField.Nombre + "</strong>";

                    if (perfilesField.ID == (int)Perfil.UsuarioConsulta)
                        perfil = "<span class='profileuser-description profile_icon__consult'></span><strong>" + perfilesField.Nombre + "</strong>";
                }
                return perfil;
            }
        }

        [NotMapped]
        public int IdPerfil
        {
            get
            {
                int perfil = int.MinValue;
                if (perfilesField != null)
                {
                    perfil = perfilesField.ID;
                }
                return perfil;
            }
        }

        public List<Usuarios> GetUsuarios()
        {
            using (var db = new GestNotifContext())
            {
                return db.Usuarios.Include("Perfiles").ToList();
            }
        }

        public List<Usuarios> GetUsuariosPorPerfil(int idPerfil)
        {
            using (var db = new GestNotifContext())
            {
                return db.Usuarios.Include("Perfiles").Where(i => i.Perfiles.ID == idPerfil).ToList();
            }
        }

        public bool UpdateUsuario(int idUsuarioEditado, string nombre, string apellidos, string nif, string email, string telefono, bool activo, int idPerfil, 
            bool bloqueado, bool estaActivo, bool estaBloqueado)
        {
            using (var db = new GestNotifContext())
            {
                List<Usuarios> listaUsuariosConMismoEmail = db.Usuarios.Where(i => i.Email == email && i.ID != idUsuarioEditado).ToList();
                
                if (listaUsuariosConMismoEmail.Count == 0)
                {
                    Usuarios usuarioEditar = db.Usuarios.SingleOrDefault(i => i.ID == idUsuarioEditado);
                    Perfiles perfil = db.Perfiles.SingleOrDefault(b => b.ID == idPerfil);

                    usuarioEditar.Nombre = nombre;
                    usuarioEditar.Apellidos = apellidos;
                    usuarioEditar.NIF = nif;
                    usuarioEditar.Email = email;
                    usuarioEditar.Activo = activo;
                    usuarioEditar.Telefono = telefono;
                    usuarioEditar.Perfiles = perfil;
                    usuarioEditar.Bloqueado = bloqueado;

                    if (usuarioEditar.Activo == false && estaActivo == true)
                    {
                        //Es baja
                        usuarioEditar.FechaBaja = DateTime.Now;
                    }

                    if (usuarioEditar.Activo == true && estaActivo == false)
                    {
                        //Si se da de alta a un usuario que se habia dado de baja se borra su fecha de baja
                        usuarioEditar.FechaBaja = null;
                    }

                    if (usuarioEditar.Bloqueado == false && estaBloqueado == true)
                    {
                        //Si se desbloquea un usuario que estaba bloqueado se reinician los contadores
                        usuarioEditar.DniNumeroIntentos = 0;
                        usuarioEditar.FechaBloqueado = null;
                        usuarioEditar.LoginNumeroIntentos = 0;
                        usuarioEditar.FechaUltimoIntentoLogin = null;
                    }

                    db.SaveChanges();

                    return true;
                }
                else
                    return false;
            }
        }

        public bool UpdateUsuario(int idUsuarioEditado, string nombre, string apellidos, string nif, string email, string tlf, string login)
        {
            using (var db = new GestNotifContext())
            {
                List<Usuarios> listaUsuariosConMismoEmail = db.Usuarios.Where(i => i.Email == email && i.ID != idUsuarioEditado).ToList();

                if (listaUsuariosConMismoEmail.Count == 0)
                {
                    Usuarios usuario = db.Usuarios.SingleOrDefault(b => b.ID == idUsuarioEditado);

                    if (usuario != null)
                    {
                        usuario.Nombre = nombre;
                        usuario.Apellidos = apellidos;
                        usuario.NIF = nif;
                        usuario.Email = email;
                        usuario.Telefono = tlf;
                        usuario.LoginUsuario = login;
                        db.SaveChanges();
                    }

                    return true;
                }
                else
                    return false;
            }
        }

        public bool UpdateClaveRecuperarPassword(string email, string cadenaAleatoria)
        {
            using (var db = new GestNotifContext())
            {
                var usuario = db.Usuarios.Where(i => i.Email == email).SingleOrDefault();
                if (usuario != null)
                {
                    usuario.ClaveRecuperaPassword = cadenaAleatoria;
                    db.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        public bool UpdateClaveRecuperarPassword(int idUsuario, string cadenaAleatoria)
        {
            using (var db = new GestNotifContext())
            {
                var usuario = db.Usuarios.Where(u => u.ID == idUsuario).SingleOrDefault();
                if (usuario != null)
                {
                    usuario.ClaveRecuperaPassword = cadenaAleatoria;
                    db.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        public Usuarios GetUsuarioPorClaveRecuperaPassword(string clave)
        {
            using (var db = new GestNotifContext())
            {
                return db.Usuarios.Where(i => i.ClaveRecuperaPassword == clave).FirstOrDefault();
            }
        }

        public Usuarios GetUsuarioPorId(int idUsuario)
        {
            using (var db = new GestNotifContext())
            {
                return db.Usuarios.Include("Perfiles")
                    .Where(u => u.ID == idUsuario).FirstOrDefault();
            }
        }

        public bool AceptarConsentimiento(int idUsuario)
        {
            using (var db = new GestNotifContext())
            {
                Usuarios objUsuario = db.Usuarios.Where(u => u.ID == idUsuario).SingleOrDefault();
                if (objUsuario != null)
                {
                    objUsuario.ConsentimientoLegal = true;
                    objUsuario.FechaConsentimiento = DateTime.Now;
                    db.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        public void SetActivo(int id, bool activo)
        {
            using (var db = new GestNotifContext())
            {
                var usr = db.Usuarios.SingleOrDefault(b => b.ID == id);
                if (usr != null)
                {
                    usr.Activo = activo;
                    db.SaveChanges();
                }
            }
        }

        public int ComprobarDni(int idUsuario, string dni)
        {
            //Valores de salida: 1 => DNI correcto; 0 => DNI incorrecto; -1 => 3 intentos fallidos y se bloquea el usuario.
            using (var db = new GestNotifContext())
            {
                int dniCorrecto = 0;
                Usuarios usr = db.Usuarios.SingleOrDefault(i => i.ID == idUsuario);
                if (usr != null)
                {
                    if (usr.NIF == dni)
                    {
                        dniCorrecto = 1;
                        usr.DniNumeroIntentos = 0;
                    }
                    else
                    {
                        int? numeroIntentos = (usr.DniNumeroIntentos == null ? 1 : usr.DniNumeroIntentos + 1);
                        usr.DniNumeroIntentos = numeroIntentos;

                        if (numeroIntentos == 3)
                        {
                            usr.Bloqueado = true;
                            usr.FechaBloqueado = DateTime.Now;
                            dniCorrecto = -1;
                        }   
                    }

                    db.SaveChanges();
                }

                return dniCorrecto;
            }
        }
    }

    [Table("ComunidadesAutonomas")]
    public partial class ComunidadesAutonomas : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private string comunidadAutonomaField;

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

        [Required]
        [MaxLength(50)]
        public string ComunidadAutonoma
        {
            get
            {
                return this.comunidadAutonomaField;
            }
            set
            {
                this.comunidadAutonomaField = value;
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
        public List<ComunidadesAutonomas> GetCCAA()
        {
            using (var db = new GestNotifContext())
            {
                return db.ComunidadesAutonomas.ToList();
            }
        }
    }

    [Table("Provincias")]
    public partial class Provincias : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private string provinciaField;
        private ComunidadesAutonomas comunidadesAutonomasField;

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

        [Required]
        [MaxLength(50)]
        public string Provincia
        {
            get
            {
                return this.provinciaField;
            }
            set
            {
                this.provinciaField = value;
                this.RaisePropertyChanged("provinciaField");
            }
        }

        public ComunidadesAutonomas ComunidadesAutonomas
        {
            get
            {
                return this.comunidadesAutonomasField;
            }
            set
            {
                this.comunidadesAutonomasField = value;
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

        public List<Provincias> GetProvincias()
        {
            using (var db = new GestNotifContext())
            {
                return db.Provincias.ToList();
            }
        }
    }

    [Table("OrganismosEmisores")]
    public partial class OrganismosEmisores : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private string dir3OrganismoEmisorField;
        private string nifField;
        private string organismoEmisorField;
        private string dir3OrganismoRaizField;
        private string organismoRaizField;
        private string administracionPublicaField;
        private int numeroOrganismosField;
        private Provincias provinciasField;
        private ComunidadesAutonomas comunidadesAutonomasField;
        private string gruposAsignadosCadenaField;

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

        [Required]
        public string Dir3OrganismoEmisor
        {
            get
            {
                return this.dir3OrganismoEmisorField;
            }
            set
            {
                this.dir3OrganismoEmisorField = value;
                this.RaisePropertyChanged("dir3OrganismoEmisorField");
            }
        }

        [MaxLength(9)]
        public string NIF
        {
            get
            {
                return this.nifField;
            }
            set
            {
                this.nifField = value;
                this.RaisePropertyChanged("nifField");
            }
        }

        [Required]
        [MaxLength(255)]
        public string OrganismoEmisor
        {
            get
            {
                return this.organismoEmisorField;
            }
            set
            {
                this.organismoEmisorField = value;
                this.RaisePropertyChanged("organismoEmisorField");
            }
        }

        [Required]
        [MaxLength(9)]
        public string Dir3OrganismoRaiz
        {
            get
            {
                return this.dir3OrganismoRaizField;
            }
            set
            {
                this.dir3OrganismoRaizField = value;
                this.RaisePropertyChanged("dir3OrganismoRaizField");
            }
        }

        [Required]
        [MaxLength(255)]
        public string OrganismoRaiz
        {
            get
            {
                return this.organismoRaizField;
            }
            set
            {
                this.organismoRaizField = value;
                this.RaisePropertyChanged("organismoRaizField");
            }
        }

        [Required]
        [MaxLength(255)]
        public string AdministracionPublica
        {
            get
            {
                return this.administracionPublicaField;
            }
            set
            {
                this.administracionPublicaField = value;
                this.RaisePropertyChanged("administracionPublicaField");
            }
        }

        public int NumeroOrganismos
        {
            get
            {
                return this.numeroOrganismosField;
            }
            set
            {
                this.numeroOrganismosField = value;
                this.RaisePropertyChanged("numeroOrganismosField");
            }
        }

        public Provincias Provincias
        {
            get
            {
                return this.provinciasField;
            }
            set
            {
                this.provinciasField = value;
            }
        }

        public ComunidadesAutonomas ComunidadesAutonomas
        {
            get
            {
                return this.comunidadesAutonomasField;
            }
            set
            {
                this.comunidadesAutonomasField = value;
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

        [NotMapped]
        public string CodigoHtmlEliminar
        {
            get
            {
                return "<div class='action_button'>" +
                    "<span class='action_button__icon action_button__icon__person_delete' id='spanEliminarOrg' title='Eliminar'></span>" +
                    "</div>";
            }
        }

        [NotMapped]
        public string CodigoHtmlAsignar
        {
            get
            {
                return "<div class='action_button'>" +
                    "<span class='action_button__icon action_button__icon__assigned-people' id='spanAsignarOrg'></span>" +
                    "</div>";
            }
        }

        [NotMapped]
        public string CodigoHtmlGruposAsignadosCadena
        {
            get
            {
                return "<span class='cadena-" + idField + "'>" + gruposAsignadosCadenaField + "</span>";
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

        public object GetOrganismosEmisoresSelect(string organismoRaiz)
        {
            using (var db = new GestNotifContext())
            {
                return db.OrganismosEmisores
                    .AsNoTracking()
                    .Where(i => i.OrganismoRaiz == organismoRaiz)
                    .Select(i => new { i.ID, i.OrganismoEmisor })
                    .Distinct().ToList();
            }
        }

        public List<OrganismosEmisores> GetOrganismosEmisoresTablaPosicion(bool primeros)
        {
            using (var db = new GestNotifContext())
            {
                int limite = 2000;
                if (primeros)
                {
                    return db.OrganismosEmisores.Include("Provincias").Include("ComunidadesAutonomas")
                        .AsNoTracking()
                        .OrderBy(i => i.ID)
                        //.Take(limite)
                        .Distinct().ToList();
                }
                else
                {
                    return db.OrganismosEmisores.Include("Provincias").Include("ComunidadesAutonomas")
                        .AsNoTracking()
                        .OrderBy(i => i.ID)
                        .Skip(limite).Distinct().ToList();
                }
            }
        }

        public List<OrganismosEmisores> GetOrganismosEmisoresTablaPaginado(int pagina, int maximo)
        {
            using (var db = new GestNotifContext())
            {
                int skip = (maximo * (pagina - 1)) + 1;

                return db.OrganismosEmisores.Include("Provincias").Include("ComunidadesAutonomas")
                    .AsNoTracking()
                    .OrderBy(i => i.ID)
                    .Skip(skip)
                    .Take(maximo)
                    .Distinct().ToList();
            }
        }

        public List<OrganismosEmisores> GetOrganismosEmisoresTabla()
        {
            using (var db = new GestNotifContext())
            {
                return db.OrganismosEmisores.Include("Provincias").Include("ComunidadesAutonomas")
                    .AsNoTracking()
                    .Distinct().ToList();
            }
        }

        public List<OrganismosEmisores> GetOrganismosEmisoresCadenaGruposVacia()
        {
            using (var db = new GestNotifContext())
            {
                return db.OrganismosEmisores.Include("Provincias").Include("ComunidadesAutonomas")
                    .AsNoTracking()
                    .Where(i => i.GruposAsignadosCadena == null || i.GruposAsignadosCadena == "")
                    .Distinct().ToList();
            }
        }

        public List<OrganismosEmisores> GetOrganismosEmisoresTablaFiltro(string administracionPublica = null, string organismoRaiz = null,
            int? idOrganoEmisor = null, int? idProvincia = null, int? idCCAA = null)
        {
            using (var db = new GestNotifContext())
            {
                return db.OrganismosEmisores.Include("Provincias").Include("ComunidadesAutonomas")
                    .AsNoTracking()
                    .Where(i => ((administracionPublica != "Seleccionar" && i.AdministracionPublica == administracionPublica) || administracionPublica == "Seleccionar") &&
                        ((organismoRaiz != "Seleccionar" && i.OrganismoRaiz == organismoRaiz) || organismoRaiz == "Seleccionar") &&
                        ((idOrganoEmisor.HasValue && i.ID == idOrganoEmisor.Value) || !idOrganoEmisor.HasValue) &&
                        ((idProvincia.HasValue && i.Provincias.ID == idProvincia.Value) || !idProvincia.HasValue) &&
                        ((idCCAA.HasValue && i.ComunidadesAutonomas.ID == idCCAA.Value) || !idCCAA.HasValue))
                    .Distinct().ToList();
            }
        }

        public OrganismosEmisores GetOrganismoEmisorContext(string codigoDIR3, GestNotifContext db)
        {
            OrganismosEmisores organismoEmisor = db.OrganismosEmisores
                .AsNoTracking()
                .Where(i => i.Dir3OrganismoEmisor == codigoDIR3)
                .FirstOrDefault();

            return organismoEmisor;
        }

        public List<string> GetAdministracionPublica()
        {
            using (var db = new GestNotifContext())
            {
                return db.OrganismosEmisores
                    .AsNoTracking()
                    .Select(i => i.AdministracionPublica).Distinct().ToList();
            }
        }

        public List<string> GetOrganismosRaiz(string administracionPublica)
        {
            using (var db = new GestNotifContext())
            {
                return db.OrganismosEmisores
                    .AsNoTracking()
                    .Where(i => i.AdministracionPublica == administracionPublica)
                    .Select(i => i.OrganismoRaiz)
                    .Distinct().ToList();
            }
        }

        public void SetGruposAsignadosCadena(int idOrganismo, string cadena)
        {
            using (var db = new GestNotifContext())
            {
                db.OrganismosEmisores.Where(i => i.ID == idOrganismo).FirstOrDefault().GruposAsignadosCadena = cadena;
                db.SaveChanges();
            }
        }
    }

    [Table("Grupos")]
    public partial class Grupos : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private string nombreField;
        private bool activoField;
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

        [Required]
        [MaxLength(255)]
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

        [NotMapped]
        public string CodigoHtmlMenu
        {
            get
            {
                return "<div class='action_button' title='Editar'><a href='../Administracion/GrupoDetalle?id=" + idField + "'>" +
                    "<span class='action_button__icon action_button__icon__edit'></span>" +
                    "</a></div>";
            }
        }

        [NotMapped]
        public string CodigoHtmlActivo
        {
            get
            {
                if (activoField)
                    return "<div class='switch-container d-flex'><input type='checkbox' class='checkbox-grupo' id='" + idField + "' checked /><label for='" + idField + "'>Toggle</label></div>";
                else
                    return "<div class='switch-container d-flex'><input type='checkbox' class='checkbox-grupo' id='" + idField + "' /><label for='" + idField + "'>Toggle</label></div>";
            }
        }

        [NotMapped]
        public string CodigoHtmlCheckBox
        {
            get
            {
                return "<input class=\"styled-checkbox\" id=\"styled-checkbox-" + this.idField + "\" type=\"checkbox\" value=\"value1\"><label for=\"styled-checkbox-" + this.idField + "\"></label>";
            }
        }

        [NotMapped]
        public string CodigoHtmlEliminar
        {
            get
            {
                return "<div class='action_button'>" +
                    "<span class='action_button__icon action_button__icon__person_delete' id='spanEliminar'></span>" +
                    "</div>";
            }
        }

        [NotMapped]
        public string CodigoHtmlAsignar
        {
            get
            {
                return "<div class='action_button'>" +
                    "<span class='action_button__icon action_button__icon__assigned-people' id='spanAsignar'></span>" +
                    "</div>";
            }
        }

        public List<Grupos> GetGrupos()
        {
            using (var db = new GestNotifContext())
            {
                return db.Grupos.AsNoTracking().ToList();
            }
        }

        public Grupos GetGrupoById(int id)
        {
            using (var db = new GestNotifContext())
            {
                return db.Grupos.AsNoTracking().Where(i => i.ID == id).FirstOrDefault();
            }
        }

        public List<Grupos> GetGruposAcitvos()
        {
            using (var db = new GestNotifContext())
            {
                return db.Grupos.AsNoTracking().Where(i => i.Activo == true).ToList();
            }
        }

        public void UpdateGrupos(int id, string nombre, bool activo, string descripcion)
        {
            using (var db = new GestNotifContext())
            {
                var grupos = db.Grupos.AsNoTracking().SingleOrDefault(b => b.ID == id);
                if (grupos != null)
                {
                    grupos.Nombre = nombre;
                    grupos.Activo = activo;
                    grupos.Descripcion = descripcion;
                    db.SaveChanges();
                }
            }
        }

        public void InsertGrupos(string nombre, bool activo, string descripcion)
        {
            using (var db = new GestNotifContext())
            {
                Grupos gr = new Grupos
                {
                    Nombre = nombre,
                    Activo = activo,
                    Descripcion = descripcion
                };
                db.Grupos.Add(gr);
                db.SaveChanges();
            }
        }

        public void SetActivo(int id, bool activo)
        {
            using (var db = new GestNotifContext())
            {
                var grupos = db.Grupos.AsNoTracking().SingleOrDefault(b => b.ID == id);
                if (grupos != null)
                {
                    grupos.Activo = activo;
                    db.SaveChanges();
                }
            }
        }
    }

    [Table("Perfiles")]
    public partial class Perfiles : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private string nombreField;
        private DateTime fechaCreacionField;
        private bool activoField;
        private bool notificacionesField;
        private bool administracionField;
        private bool configuracionField;
        private DateTime? fechaBajaField;
        private string descripcionField;
        private PantallasInicio pantallasInicioField;

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
                this.RaisePropertyChanged("nombreField");
            }
        }

        [Required]
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

        public DateTime FechaCreacion
        {
            get
            {
                return this.fechaCreacionField;
            }
            set
            {
                this.fechaCreacionField = value;
                this.RaisePropertyChanged("fechaCreacionField");
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

        public bool Notificaciones
        {
            get
            {
                return this.notificacionesField;
            }
            set
            {
                this.notificacionesField = value;
                this.RaisePropertyChanged("notificacionesField");
            }
        }

        public bool Administracion
        {
            get
            {
                return this.administracionField;
            }
            set
            {
                this.administracionField = value;
                this.RaisePropertyChanged("administracionField");
            }
        }

        public bool Configuracion
        {
            get
            {
                return this.configuracionField;
            }
            set
            {
                this.configuracionField = value;
                this.RaisePropertyChanged("configuracionField");
            }
        }

        public DateTime? FechaBaja
        {
            get
            {
                return this.fechaBajaField;
            }
            set
            {
                this.fechaBajaField = value;
                this.RaisePropertyChanged("fechaBajaField");
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

        public PantallasInicio PantallasInicio
        {
            get
            {
                return this.pantallasInicioField;
            }
            set
            {
                this.pantallasInicioField = value;
                this.RaisePropertyChanged("pantallasInicioField");
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
        public string CodigoHtmlNotificaciones
        {
            get
            {
                if (notificacionesField)
                    return "<span class='status status__check'></span>";
                else
                    return "<span class='status status__delete'></span>";
            }
        }

        [NotMapped]
        public string CodigoHtmlIconoPerfil
        {
            get
            {
                string cadena = "";
                switch (idField)
                {
                    case (int)Perfil.Administrador:
                        cadena = "<span type='button' class='usertable_icon profile_icon__admin'></span>";
                        break;
                    case (int)Perfil.Asignador:
                        cadena = "<span type='button' class='usertable_icon profile_icon__asign'></span>";
                        break;
                    case (int)Perfil.Gestor:
                        cadena = "<span type='button' class='usertable_icon profile_icon__tramit'></span>";
                        break;
                    case (int)Perfil.UsuarioConsulta:
                        cadena = "<span type='button' class='usertable_icon profile_icon__consult'></span>";
                        break;

                }
                return cadena;
            }
        }

        [NotMapped]
        public string CodigoHtmlAdministracion
        {
            get
            {
                if (administracionField)
                    return "<span class='status status__check'></span>";
                else
                    return "<span class='status status__delete'></span>";
            }
        }

        [NotMapped]
        public string CodigoHtmlConfiguracion
        {
            get
            {
                if (configuracionField)
                    return "<span class='status status__check'></span>";
                else
                    return "<span class='status status__delete'></span>";
            }
        }

        [NotMapped]
        public string CodigoHtmlMenu
        {
            get
            {
                return "<div class='action_button'>" +
                    "<a href='../Administracion/PerfilDetalle?id=" + idField + "'>" +
                    "<span class='action_button__icon action_button__icon__edit'></span></a></div>";
            }
        }

        public List<Perfiles> GetPerfiles()
        {
            using (var db = new GestNotifContext())
            {
                return db.Perfiles.Include("PantallasInicio").ToList();
            }
        }

        public Perfiles GetPerfilById(int id)
        {
            using (var db = new GestNotifContext())
            {
                return db.Perfiles.Include("PantallasInicio")
                    .Where(i => i.ID == id)
                    .FirstOrDefault();
            }
        }

        public void UpdatePerfiles(int id, string nombre, string desc, bool noti, bool admi, bool confi, int pantIni)
        {
            using (var db = new GestNotifContext())
            {
                PantallasInicio pantalla = db.PantallasInicio.SingleOrDefault(b => b.ID == pantIni);

                var perfiles = db.Perfiles.SingleOrDefault(b => b.ID == id);
                if (perfiles != null)
                {
                    perfiles.Nombre = nombre;
                    perfiles.Descripcion = desc;
                    perfiles.Notificaciones = noti;
                    perfiles.Administracion = admi;
                    perfiles.Configuracion = confi;
                    perfiles.PantallasInicio = pantalla;
                    db.SaveChanges();
                }
            }
        }
    }

    [Table("GruposUsuarios")]
    public partial class GruposUsuarios : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private Grupos gruposField;
        private Usuarios usuariosField;

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

        [Required]
        public Grupos Grupos
        {
            get
            {
                return this.gruposField;
            }
            set
            {
                this.gruposField = value;
                this.RaisePropertyChanged("gruposField");
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

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        public List<GruposUsuarios> GetUsuarioPorGrupo(int id)
        {
            using (var db = new GestNotifContext())
            {
                return db.GruposUsuarios.Include("Usuarios").Include("Usuarios.Perfiles").Where(b => b.Grupos.ID == id).ToList();
            }
        }

        public List<GruposUsuarios> GetGrupoPorUsuario(int id)
        {
            using (var db = new GestNotifContext())
            {
                return db.GruposUsuarios.Include("Usuarios").Include("Grupos").Where(b => b.Usuarios.ID == id).ToList();
            }
        }

        public List<string> GetListaIdsGrupoPorUsuario(int idUsuario)
        {
            using (var db = new GestNotifContext())
            {
                return db.GruposUsuarios.Include("Usuarios").Include("Grupos")
                    .Where(b => b.Usuarios.ID == idUsuario && b.Grupos.Activo == true)
                    .Select(i => i.Grupos.ID.ToString()).Distinct().ToList();
            }
        }

        public bool InsertUsuarioGrupo(int idUsu, int idGrupo)
        {
            using (var db = new GestNotifContext())
            {
                GruposUsuarios grusu = db.GruposUsuarios.SingleOrDefault(b => b.Grupos.ID == idGrupo && b.Usuarios.ID == idUsu);
                if (grusu != null)
                {
                    return false;
                }
                Usuarios usu = db.Usuarios.SingleOrDefault(b => b.ID == idUsu);
                Grupos gr = db.Grupos.SingleOrDefault(b => b.ID == idGrupo);
                GruposUsuarios grUsu = new GruposUsuarios
                {
                    Usuarios = usu,
                    Grupos = gr
                };
                db.GruposUsuarios.Add(grUsu);
                db.SaveChanges();
                return true;
            }
        }

        public void DeleteUsuarioGrupo(int idUsuGrupo)
        {
            using (var db = new GestNotifContext())
            {
                GruposUsuarios grUsu = db.GruposUsuarios.Include("Usuarios").Include("Grupos").SingleOrDefault(b => b.ID == idUsuGrupo);
                db.GruposUsuarios.Remove(grUsu);
                db.SaveChanges();
            }
        }
    }

    [Table("GruposOrganismosEmisores")]
    public partial class GruposOrganismosEmisores : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private Grupos gruposField;
        private OrganismosEmisores organismosEmisoresField;

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

        [Required]
        public Grupos Grupos
        {
            get
            {
                return this.gruposField;
            }
            set
            {
                this.gruposField = value;
                this.RaisePropertyChanged("gruposField");
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

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        public GruposOrganismosEmisores GetGrupoOrganismoEmisor(int id)
        {
            using (var db = new GestNotifContext())
            {
                return db.GruposOrganismosEmisores
                    .Include("OrganismosEmisores").Include("Grupos")
                    .Where(b => b.ID == id).FirstOrDefault();
            }
        }

        public List<GruposOrganismosEmisores> GetOrganismosPorIdGrupo(int idGrupo)
        {
            using (var db = new GestNotifContext())
            {
                return db.GruposOrganismosEmisores.Include("OrganismosEmisores")
                    .Include("OrganismosEmisores.ComunidadesAutonomas")
                    .Include("OrganismosEmisores.Provincias")
                    .Where(b => b.Grupos.ID == idGrupo).ToList();
            }
        }

        public List<GruposOrganismosEmisores> GetGruposPorIdOrganismoEmisor(int idOrganismo)
        {
            using (var db = new GestNotifContext())
            {
                return db.GruposOrganismosEmisores
                    .Include("Grupos")
                    .Where(b => b.OrganismosEmisores.ID == idOrganismo).ToList();
            }
        }

        public void DeleteOrganismoGrupo(int idOrgGrupo)
        {
            using (var db = new GestNotifContext())
            {
                GruposOrganismosEmisores grOrg = db.GruposOrganismosEmisores.Include("OrganismosEmisores").Include("Grupos")
                    .SingleOrDefault(b => b.ID == idOrgGrupo);

                db.GruposOrganismosEmisores.Remove(grOrg);
                db.SaveChanges();
            }
        }

        public bool InsertOrgGrupo(int idOrganismo, int idGrupo)
        {
            using (var db = new GestNotifContext())
            {
                OrganismosEmisores org = db.OrganismosEmisores.SingleOrDefault(b => b.ID == idOrganismo);
                Grupos gr = db.Grupos.SingleOrDefault(b => b.ID == idGrupo);
                GruposOrganismosEmisores grOrgEm = db.GruposOrganismosEmisores
                    .SingleOrDefault(b => b.Grupos.ID == idGrupo && b.OrganismosEmisores.ID == idOrganismo);

                if (grOrgEm != null)
                {
                    return false;
                }
                else
                {
                    GruposOrganismosEmisores grOrg = new GruposOrganismosEmisores
                    {
                        OrganismosEmisores = org,
                        Grupos = gr
                    };
                    db.GruposOrganismosEmisores.Add(grOrg);
                    db.SaveChanges();

                    return true;
                }
            }
        }

        public void SetGruposAsignadosCadena(int idOrganismo)
        {
            List<GruposOrganismosEmisores> listaGrupos = GetGruposPorIdOrganismoEmisor(idOrganismo);

            List<string> listaGruposNombre = listaGrupos.Select(i => i.Grupos.Nombre).ToList();
            string cadena = "";
            if (listaGruposNombre.Count > 0)
                cadena = listaGruposNombre.Aggregate((i, j) => i + ", " + j);

            OrganismosEmisores oe = new OrganismosEmisores();
            oe.SetGruposAsignadosCadena(idOrganismo, cadena);
        }
    }

    [Table("GruposEnvios")]
    public partial class GruposEnvios : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private Grupos gruposField;
        private Envios enviosField;
        private DateTime fechaField;

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

        public Grupos Grupos
        {
            get
            {
                return this.gruposField;
            }
            set
            {
                this.gruposField = value;
                this.RaisePropertyChanged("gruposField");
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

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        public void InsertGrupoEnvio(int idGrupo, string identificador, int idUsuarioQueAsigna)
        {
            using (var db = new GestNotifContext())
            {
                try
                {
                    var obj1 = db.Grupos.SingleOrDefault(i => i.ID == idGrupo);
                    var obj2 = db.Envios.SingleOrDefault(i => i.Identificador == identificador);
                    GruposEnvios ge = new GruposEnvios
                    {
                        Grupos = obj1,
                        Envios = obj2,
                        Fecha = DateTime.Now
                    };

                    db.GruposEnvios.Add(ge);

                    //Se registra en el histórico
                    HistorialCambiosEnvio hce = new HistorialCambiosEnvio
                    {
                        Envios_Identificador = identificador,
                        Usuarios_ID = idUsuarioQueAsigna,
                        TiposCambioEnvio_ID = (int)TipoCambioEnvio.GrupoAsignado,
                        Fecha = DateTime.Now,
                        Grupos_ID = idGrupo,
                        EstadosEnvio_ID = 0,
                    };

                    db.HistorialCambiosEnvio.Add(hce);
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    db.Dispose();
                    throw new Exception("Error en 'InsertGrupoEnvio'", ex);
                }
            }
        }

        public void InsertGrupoEnvioAutomaticoContext(Envios envioAsignado, Grupos grupo, GestNotifContext db)
        {
            try
            {
                GruposEnvios gruposEnvios = new GruposEnvios
                {
                    Grupos = grupo,
                    Envios = envioAsignado,
                    Fecha = DateTime.Now
                };

                db.GruposEnvios.Add(gruposEnvios);
                db.SaveChanges();

                //Se registra en el histórico
                HistorialCambiosEnvio hce = new HistorialCambiosEnvio
                {
                    Envios_Identificador = envioAsignado.Identificador,
                    Usuarios_ID = 0,
                    TiposCambioEnvio_ID = (int)TipoCambioEnvio.GrupoAsignado,
                    Fecha = DateTime.Now,
                    Grupos_ID = grupo.ID,
                    EstadosEnvio_ID = 0,
                };

                db.HistorialCambiosEnvio.Add(hce);
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                db.Dispose();
                throw new Exception("Error en 'InsertGrupoEnvio'", ex);
            }
        }

        public void DeleteGrupoEnvio(int idGrupo, string identificador)
        {
            using (var db = new GestNotifContext())
            {
                try
                {
                    GruposEnvios objGrupoEnvio = db.GruposEnvios
                        .Where(i => i.Grupos.ID == idGrupo && i.Envios.Identificador == identificador).FirstOrDefault();

                    db.GruposEnvios.Remove(objGrupoEnvio);
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    db.Dispose();
                    throw new Exception("Error en 'DeleteGrupoEnvio'", ex);
                }
            }
        }

        public List<Grupos> GetGruposPorEnvio(string identificador)
        {
            using (var db = new GestNotifContext())
            {
                return db.GruposEnvios.Include("Grupos")
                    .Where(i => i.Envios.Identificador == identificador && i.Grupos.Activo == true)
                    .Select(i => i.Grupos)
                    .ToList();
            }
        }

        public List<string> GetGruposPorEnvioContexto(string identificador, GestNotifContext db)
        {
            return db.GruposEnvios.Include("Grupos")
                .Where(i => i.Envios.Identificador == identificador && i.Grupos.Activo == true)
                .Select(i => i.Grupos.Nombre)
                .ToList();
        }
    }

    [Table("PantallasInicio")]
    public partial class PantallasInicio : object, System.ComponentModel.INotifyPropertyChanged
    {
        private int idField;
        private string nombreField;
        private string direccionField;
        private int ordenField;

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

        public string Direccion
        {
            get
            {
                return this.direccionField;
            }
            set
            {
                this.direccionField = value;
                this.RaisePropertyChanged("direccionField");
            }
        }

        public int Orden
        {
            get
            {
                return this.ordenField;
            }
            set
            {
                this.ordenField = value;
                this.RaisePropertyChanged("ordenField");
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

        public List<PantallasInicio> GetPantallasInicio()
        {
            using (var db = new GestNotifContext())
            {
                return db.PantallasInicio.
                    OrderBy(i => i.Orden).ToList();
            }
        }

        public PantallasInicio GetPantallaInicioPorID(int idPantallaInicio)
        {
            using (var db = new GestNotifContext())
            {
                return db.PantallasInicio.Where(i => i.ID == idPantallaInicio).FirstOrDefault();
            }
        }

        public PantallasInicio GetPantallaInicioPorPerfil(int idPerfil)
        {
            using (var db = new GestNotifContext())
            {
                Perfiles p = db.Perfiles.Include("PantallasInicio")
                    .Where(j => j.ID == idPerfil).FirstOrDefault();

                if (p != null)
                    return db.PantallasInicio.Where(i => i.ID == p.PantallasInicio.ID).FirstOrDefault();
                else
                    return null;
            }
        }
    }

    public enum Perfil
    {
        Administrador = 1,
        Asignador = 2,
        Gestor = 3,
        UsuarioConsulta = 4
    }
}
