using Microsoft.EntityFrameworkCore;
using Servicio.Entidades;
using Servicio.Extensiones;
using Servicio.Utilidades;

namespace Servicio.Contexto
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
      CadenaDeConexion = Configuracion.Instancia.CadenaDeConexion;
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