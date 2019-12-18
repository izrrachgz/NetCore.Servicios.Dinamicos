using System.IO;
using Servicio.Extensiones;
using Xunit;

namespace Servicio.Pruebas.Hechos.Extensiones
{
  public class Flujos
  {
    [Fact]
    public void NoEsValido()
    {
      Stream stream = Stream.Null;
      Assert.True(stream.NoEsValido());
    }
  }
}
