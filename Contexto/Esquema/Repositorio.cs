using Datos.Configuraciones;
using Contexto.Configuraciones;
using Contexto.Entidades;
using Contexto.Esquema.ConfiguracionDeEntidades;
using Microsoft.EntityFrameworkCore;

namespace Contexto.Esquema
{
  /// <summary>
  /// Provee un modelo de datos que permite
  /// representar una instancia del repositorio de datos
  /// asociado
  /// </summary>
  internal sealed class Repositorio : DbContext
  {
    #region Propiedades

    /// <summary>
    /// Cadena de conexion al repositorio de datos
    /// </summary>
    private string CadenaDeConexion { get; }

    #endregion

    #region Entidades

    /// <summary>
    /// Coleccion de entradas de log
    /// </summary>
    public DbSet<EntradaLog> EntradasDeLog { get; set; }

    /// <summary>
    /// Coleccion de detalles asociado a las entradas de log
    /// </summary>
    public DbSet<EntradaLogDetalle> DetallesDeEntradaLog { get; set; }

    //Agrega tus colecciones aquí

    #endregion

    public Repositorio()
    {
      CadenaDeConexion = Configuracion<ConfiguracionContexto>.Instancia.CadenaDeConexion;
    }

    #region Configuraciones

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      base.OnConfiguring(options);
      options.UseSqlServer(CadenaDeConexion);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      //Registra las configuraciones de tus entidades aquí

      EntradaLogDetalleConfiguracion.Registrar(modelBuilder);
    }

    #endregion
  }
}
