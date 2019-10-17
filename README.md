# NetCore Servicios-Dinamicos
	Permite crear servicios genéricos de manera dinámica

### Agrega tu entidad dentro del nombre de espacio de "Entidades"
	
	¡Deberá extender la clase EntidadBase!

```sh
namespace Servicio.Entidades
{
  [Table("Usuario")]
  public class Usuario : EntidadBase
  {
    [Required, MaxLength(128)]
    public string Nombre { get; set; }

    [Required, MaxLength(64)]
    public string ApellidoPaterno { get; set; }

    [Required, MaxLength(64)]
    public string ApellidoMaterno { get; set; }

    [NotMapped]
    public string NombreCompleto => $@"{Nombre} {ApellidoPaterno} {ApellidoMaterno}";

    [Required, MaxLength(192)]
    public string Correo { get; set; }

    [Required, MaxLength(18)]
    public string NumeroContacto { get; set; }
  }
}
```

### Agrega tu referencia dentro del nombre de espacio "Contexto"

```sh
namespace Servicio.Contexto
{
  internal class Repositorio : DbContext
  {
    public const string CadenaDeConexion = @"Data Source =.\SQLEXPRESS;Initial Catalog = Servicios.Dinamicos;Integrated Security = true;";

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
```

### Agrega una Migración desde la consola administradora de paquetes
	EntityFrameworkCore\Add-Migration NOMBRE
	
### Actualiza la Base de datos desde la consola administradora de paquetes
	EntityFrameworkCore\Update-Database

### Crea una instancia de tu nueva entidad

```sh
Servicio<Usuario> servicio = new Servicio<Usuario>();

servicio.Obtener(1);
servicio.Obtener(new Paginado());
servicio.Guardar(new Usuario());
servicio.Guardar(new List<Usuario>());
servicio.Insertar(new List<Usuario>());
servicio.Actualizar(new List<Usuario>());
servicio.Eliminar(1);
servicio.Eliminar(new List<int>());
```
  
