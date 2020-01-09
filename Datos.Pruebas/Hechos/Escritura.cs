using System.Threading.Tasks;
using Datos.Entidades;
using Datos.ProveedoresDeDatos;
using Datos.Modelos;
using Xunit;

namespace Datos.Pruebas.Hechos
{
  public class Escritura
  {
    public Usuario Usuario { get; }

    public Escritura()
    {
      Usuario = new Usuario()
      {
        Nombre = @"Pruebas",
        ApellidoPaterno = @"Pruebas",
        ApellidoMaterno = @"Pruebas",
        Correo = @"Pruebas@CSharp.com",
        NumeroContacto = @"6623559566"
      };
    }

    [Fact]
    public async Task GuardarUsuario()
    {
      ProveedorDeDatos<Usuario> servicio = new ProveedorDeDatos<Usuario>();
      RespuestaBasica guardado = await servicio.Guardar(Usuario);
      Assert.True(guardado.Correcto);
    }
  }
}
