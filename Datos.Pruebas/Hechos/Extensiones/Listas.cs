using System.Collections.Generic;
using Datos.Extensiones;
using Xunit;

namespace Datos.Pruebas.Hechos.Extensiones
{
  /// <summary>
  /// Pruebas positivas de listas
  /// </summary>
  public class Listas
  {
    /// <summary>
    /// Comprueba que la lista no es valida
    /// para su uso
    /// </summary>
    [Fact]
    public void NoEsValida()
    {
      List<string> lista = new List<string>(0);
      Assert.True(lista.NoEsValida());
    }
  }
}
