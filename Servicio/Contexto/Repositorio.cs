using Microsoft.EntityFrameworkCore;
using Servicio.Entidades;

namespace Servicio.Contexto
{
  internal class Repositorio : DbContext
  {
    public const string CadenaDeConexion = @"Data Source =.\SQLEXPRESS;Initial Catalog =NetCore.Servicios.Dinamicos;Integrated Security = true;";

    public DbSet<Usuario> Usuarios { get; set; }

    //Agrega tus colecciones aquí

    public Repositorio() { }

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
  }
}