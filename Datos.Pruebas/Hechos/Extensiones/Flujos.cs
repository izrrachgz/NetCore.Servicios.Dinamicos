using System.IO;
using Datos.Extensiones;
using Xunit;

namespace Datos.Pruebas.Hechos.Extensiones
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
