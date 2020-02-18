using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using Utilidades.Extensiones;
using Utilidades.Modelos;
using Xunit;

namespace Negocio.Pruebas.Hechos.Extensiones
{
  /// <summary>
  /// Pruebas positivas de extensiones de lista
  /// </summary>
  public class Listas
  {
    /// <summary>
    /// Comprueba que el listado de nombres
    /// puede ser interpretado por un documento
    /// excel
    /// </summary>
    [Fact]
    public void DocumentoExcel()
    {
      List<string> nombres = Enumerable.Repeat(@"Israel Ch", 10).ToList();
      RespuestaModelo<SpreadsheetDocument> guardado = nombres.DocumentoExcel();
      Assert.True(guardado.Correcto);
    }
  }
}
