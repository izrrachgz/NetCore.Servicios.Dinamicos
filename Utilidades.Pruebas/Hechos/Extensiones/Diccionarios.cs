using System.Collections.Generic;
using Utilidades.Extensiones;
using Xunit;

namespace Utilidades.Pruebas.Hechos.Extensiones
{
  /// <summary>
  /// Pruebas positivas de diccionarios
  /// </summary>
  public class Diccionarios
  {
    /// <summary>
    /// Comprueba que el diccionario en referencia
    /// no es valido para su uso
    /// </summary>
    [Fact]
    public void NoEsValido()
    {
      Dictionary<string, string> diccionario = new Dictionary<string, string>();
      Assert.True(diccionario.NoEsValido());
    }
  }
}
