using Datos.Extensiones;
using Xunit;

namespace Datos.Pruebas.Hechos.Extensiones
{
  public class Bytes
  {
    [Fact]
    public void NoEsValido()
    {
      byte[] bytes = new byte[0];
      Assert.True(bytes.NoEsValido());
    }
  }
}
