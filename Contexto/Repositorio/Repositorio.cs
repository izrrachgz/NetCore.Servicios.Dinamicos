using Datos.Configuraciones;
using Contexto.Configuraciones;
using Contexto.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Contexto.Repositorio
{
  /// <summary>
  /// Provee un modelo de datos que permite
  /// representar una instancia del repositorio de datos
  /// asociado
  /// </summary>
  internal class Repositorio : DbContext
  {
    #region Propiedades

    /// <summary>
    /// Cadena de conexion al repositorio de datos
    /// </summary>
    protected string CadenaDeConexion { get; }

    #endregion

    #region Entidades

    public DbSet<Usuario> Usuarios { get; set; }

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
      //Agrega tus configuraciones aquí
    }

    #endregion
  }
}
