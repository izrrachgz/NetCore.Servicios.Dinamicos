using Servicio.Extensiones;
using Xunit;

namespace Servicio.Pruebas.Hechos.Extensiones
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
