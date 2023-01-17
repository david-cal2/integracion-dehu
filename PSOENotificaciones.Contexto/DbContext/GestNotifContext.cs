using PSOENotificaciones.Contexto;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

public partial class GestNotifContext : DbContext
{
    public GestNotifContext() : base("PSOE_GestNotif")
    {
        //PARA HACER MIGRACIÓN COMENTAR ESTAS LÍNEAS

        string cadenaConexion = null;
        ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["PSOE_GestNotif"];
        if (settings != null)
            cadenaConexion = settings.ConnectionString;

        Database.Connection.ConnectionString = cadenaConexion;
        Database.SetInitializer(new CreateDatabaseIfNotExists<GestNotifContext>());
        Database.Initialize(true);
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
    }

    #region auditoria

    public DbSet<AuditoriaAplicaciones> AuditoriaAplicaciones { get; set; }

    public DbSet<AuditoriaLogin> AuditoriaLogin { get; set; }

    #endregion

    #region historiales

    public DbSet<HistorialExcepciones> HistorialExcepciones { get; set; }

    public DbSet<HistorialCambiosEnvio> HistorialCambiosEnvio { get; set; }

    public DbSet<TiposCambioEnvio> TiposCambioEnvio { get; set; }
    #endregion

    #region localiza
    /* DEFINICIÓN DE OPERACIÓN – localiza() y respuestaLocaliza() */

    public DbSet<PeticionesLocaliza> PeticionesLocaliza { get; set; }

    public DbSet<Opcion> Opcion { get; set; }

    public DbSet<RespuestasLocaliza> RespuestasLocaliza { get; set; }

    public DbSet<Envios> Envios { get; set; }

    public DbSet<Opcion1> Opcion1 { get; set; }

    public DbSet<Personas> Persona { get; set; }

    public DbSet<TiposEnvio> TiposEnvio { get; set; }

    public DbSet<Vinculos> Vinculos { get; set; }

    public DbSet<EstadosEnvios> EstadosEnvios { get; set; }

    /* FIN DEFINICIÓN DE OPERACIÓN – localiza() y respuestaLocaliza() */
    #endregion

    #region acceso
    /* DEFINICIÓN DE OPERACIÓN – peticionAcceso() y respuestaPeticionAcceso() */

    public DbSet<PeticionesAcceso> PeticionesAcceso { get; set; }

    public DbSet<Eventos> Eventos { get; set; }

    public DbSet<Opcion2> Opcion2 { get; set; }

    public DbSet<RespuestasAcceso> RespuestasAcceso { get; set; }

    public DbSet<Opcion3> Opcion3 { get; set; }

    public DbSet<DetalleDocumentos> DetalleDocumentos { get; set; }

    public DbSet<Contenido> Contenido { get; set; }

    public DbSet<HashDocumento> HashDocumento { get; set; }


    public DbSet<AnexoReferencia> AnexoReferencia { get; set; }



    public DbSet<AnexoUrl> AnexoUrl { get; set; }

    /* FIN DEFINICIÓN DE OPERACIÓN – peticionAcceso() y respuestaPeticionAcceso() */
    #endregion

    #region anexo
    /* DEFINICIÓN DE OPERACIÓN – peticionAcceso() y respuestaPeticionAcceso() */

    public DbSet<PeticionesAnexo> PeticionesAnexo { get; set; }

    public DbSet<Opcion4> Opcion4 { get; set; }

    public DbSet<RespuestasAnexo> RespuestasAnexo { get; set; }

    public DbSet<Opcion5> Opcion5 { get; set; }

    public DbSet<DocumentosAnexo> DocumentosAnexo { get; set; }

    public DbSet<Contenido1> Contenido1 { get; set; }

    /* FIN DEFINICIÓN DE OPERACIÓN – peticionAcceso() y respuestaPeticionAcceso() */
    #endregion

    #region acuse pdf
    /* DEFINICIÓN DE OPERACIÓN – consultaAcusePdf() y respuestaConsultaAcusePdf() */
    public DbSet<PeticionesAcusePdf> PeticionesAcusePdf { get; set; }

    public DbSet<Opcion6> Opcion6 { get; set; }

    public DbSet<IdentificadoresAcusePdf> IdentificadoresAcusePdf { get; set; }

    public DbSet<RespuestasAcusePdf> RespuestasAcusePdf { get; set; }

    public DbSet<Opcion7> Opcion7 { get; set; }

    public DbSet<AcusesPdf> AcusesPdf { get; set; }

    public DbSet<Contenido2> Contenido2 { get; set; }

    /* FIN DEFINICIÓN DE OPERACIÓN – consultaAcusePdf() y respuestaConsultaAcusePdf() */
    #endregion

    #region administración

    public DbSet<Usuarios> Usuarios { get; set; }

    public DbSet<ComunidadesAutonomas> ComunidadesAutonomas { get; set; }

    public DbSet<Provincias> Provincias { get; set; }

    public DbSet<OrganismosEmisores> OrganismosEmisores { get; set; }

    public DbSet<Grupos> Grupos { get; set; }

    public DbSet<Perfiles> Perfiles { get; set; }

    public DbSet<GruposUsuarios> GruposUsuarios { get; set; }

    public DbSet<GruposOrganismosEmisores> GruposOrganismosEmisores { get; set; }

    public DbSet<GruposEnvios> GruposEnvios { get; set; }

    public DbSet<PantallasInicio> PantallasInicio { get; set; }

    #endregion

    #region configuración

    public DbSet<Alertas> Alertas { get; set; }

    public DbSet<Parametros> Parametros { get; set; }

    public DbSet<ParametrosInternos> ParametrosInternos { get; set; }

    public DbSet<AvisosNotificacion> AvisosNotificacion { get; set; }

    public DbSet<ConfiguracionPeticiones> ConfiguracionPeticiones { get; set; }

    #endregion

    #region Logs

    public DbSet<LogTareaLocaliza> LogTareaLocaliza { get; set; }

    public DbSet<LogTareaAlertasCaducadas> LogTareaAlertasCaducadas { get; set; }

    public DbSet<LogLlamadasDehu> LogLlamadasDehu { get; set; }
    #endregion

    #region Documentos Externos
    public DbSet<TiposDocumentosExternos> TiposDocumentosExternos { get; set; }

    public DbSet<DocumentosExternos> DocumentosExternos { get; set; }
    #endregion
}