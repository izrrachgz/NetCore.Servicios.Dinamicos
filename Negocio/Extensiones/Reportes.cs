using System.Linq;
using DocumentFormat.OpenXml.Packaging;

namespace Negocio.Extensiones
{
  /// <summary>
  /// Provee metodos de extensión para reportes del tipo excel
  /// </summary>
  public static class ExtensionesDeReportesExcel
  {
    /// <summary>
    /// Indica si un documento de reporte es nulo
    /// </summary>
    /// <param name="documento">Referencia al documento de reporte</param>
    /// <returns>Verdadero o falso</returns>
    public static bool NoEsValido(this SpreadsheetDocument documento)
    {
      return documento == null || !documento.WorkbookPart.WorksheetParts.Any();
    }
  }
}
