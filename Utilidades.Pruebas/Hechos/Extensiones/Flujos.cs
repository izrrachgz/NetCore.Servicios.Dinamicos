using System.IO;
using Utilidades.Extensiones;
using Xunit;

namespace Utilidades.Pruebas.Hechos.Extensiones
{
  /// <summary>
  /// Pruebas positivas de flujos de datos
  /// </summary>
  public class Flujos
  {
    /// <summary>
    /// Comprueba que el flujo de datos
    /// no es valido para su uso
    /// </summary>
    [Fact]
    public void NoEsValido()
    {
      Stream stream = Stream.Null;
      Assert.True(stream.NoEsValido());
    }
  }
}
