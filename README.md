# Proveedores de Datos Dinamicos
	Permite crear proveedores de datos genéricos de manera dinámica
	de las entidades registradas

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
```

### Agrega una Migración desde la consola administradora de paquetes
	EntityFrameworkCore\Add-Migration NOMBRE
	
### Actualiza la Base de datos desde la consola administradora de paquetes
	EntityFrameworkCore\Update-Database

### Crea una instancia de proveedor de datos asociado a la entidad

```sh
//Crear una referencia al proveedor de datos asociado a la entidad de Usuario
ProveedorDeDatos<Usuario> datosUsuario = new ProveedorDeDatos<Usuario>();

//Obtener un usuario por llave primaria
datosUsuario.Obtener(1);

//Obtener una coleccion de usuarios utilizando una indice de paginado
datosUsuario.Obtener(new Paginado());

//Guardar un usuario (nuevo o edicion)
datosUsuario.Guardar(new Usuario());

//Guardar una coleccion de usuarios (nuevos y/o editados)
datosUsuario.Guardar(new List<Usuario>());

//Insertar una coleccion de usuarios nuevos
datosUsuario.Insertar(new List<Usuario>());

//Actualizar una coleccion de usuarios
datosUsuario.Actualizar(new List<Usuario>());

//Eliminar de manera logica un usuario por su llave primaria
datosUsuario.Eliminar(1);

//Eliminar una coleccion de usuarios utilizando un listado de llaves primarias
datosUsuario.Eliminar(new List<int>());

```
  
