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
    public void GuardarUsuario()
    {
      Servicio<Usuario> servicio = new Servicio<Usuario>();
      RespuestaBasica guardado = servicio.Guardar(Usuario);
      Assert.True(guardado.Correcto);
    }
  }
}
