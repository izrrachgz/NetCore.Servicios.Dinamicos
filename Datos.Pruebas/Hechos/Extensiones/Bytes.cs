using Datos.Extensiones;
using Xunit;

namespace Datos.Pruebas.Hechos.Extensiones
{
  /// <summary>
  /// Pruebas positivas de bytes
  /// </summary>
  public class Bytes
  {
    /// <summary>
    /// Comprueba que el arreglo de bytes
    /// no es valido para su uso
    /// </summary>
    [Fact]
    public void NoEsValido()
    {
      byte[] bytes = new byte[0];
      Assert.True(bytes.NoEsValido());
    }
  }
}
