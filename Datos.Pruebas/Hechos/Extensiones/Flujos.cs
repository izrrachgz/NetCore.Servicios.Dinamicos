using System.IO;
using Datos.Extensiones;
using Xunit;

namespace Datos.Pruebas.Hechos.Extensiones
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
