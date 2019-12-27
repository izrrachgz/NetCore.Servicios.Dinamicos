using System.Threading.Tasks;
using Datos.Entidades;
using Datos.Servicios;
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
      Servicio<Usuario> servicio = new Servicio<Usuario>();
      RespuestaBasica guardado = await servicio.Guardar(Usuario);
      Assert.True(guardado.Correcto);
    }
  }
}
