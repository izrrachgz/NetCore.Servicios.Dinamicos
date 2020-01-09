using Datos.Configuraciones;
using Microsoft.EntityFrameworkCore;
using Datos.Entidades;

namespace Datos.Contexto
{
  internal class Repositorio : DbContext
  {
    #region Propiedades

    /// <summary>
    /// Cadena de conexion al repositorio de datos
    /// </summary>
    public string CadenaDeConexion { get; }

    #endregion

    #region Entidades

    public DbSet<Usuario> Usuarios { get; set; }

    //Agrega tus colecciones aquí

    #endregion

    public Repositorio()
    {
      CadenaDeConexion = Configuracion<ConfiguracionDatos>.Instancia.CadenaDeConexion;
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