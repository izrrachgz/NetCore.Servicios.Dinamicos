using System.Collections.Generic;
using System.Linq;
using Utilidades.Herramientas;
using Xunit;

namespace Negocio.Pruebas.Hechos.Utilidades
{
  /// <summary>
  /// Pruebas positivas de la utilidad Excel
  /// </summary>
  public class UtilidadExcel
  {
    /// <summary>
    /// Comprueba que la utilidad Excel
    /// puede guarder el contenido de una lista
    /// en un archivo local en disco
    /// </summary>
    [Fact]
    public void GuardarContenidoDeLista()
    {
      List<string> nombres = Enumerable.Repeat(@"Israel", 10).ToList();
      Assert.True(Excel.GuardarContenidoDeLista(nombres).Correcto);
    }
  }
}
