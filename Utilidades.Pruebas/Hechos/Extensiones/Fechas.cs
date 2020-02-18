using System;
using Utilidades.Extensiones;
using Xunit;

namespace Utilidades.Pruebas.Hechos.Extensiones
{
  /// <summary>
  /// Pruebas positivas de fechas
  /// </summary>
  public class Fechas
  {
    /// <summary>
    /// Comprueba que la fecha no
    /// es valida para su uso
    /// </summary>
    [Fact]
    public void NoEsValida()
    {
      Assert.True(DateTime.MinValue.NoEsValida());
    }
  }
}
