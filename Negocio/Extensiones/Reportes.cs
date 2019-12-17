using System.IO;
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

    /// <summary>
    /// Crea un flujo de memoria a partir de un clon
    /// del documento
    /// </summary>
    /// <param name="documento">Referencia al documento</param>
    /// <returns>Flujo de memoria</returns>
    public static Stream Stream(this SpreadsheetDocument documento)
    {
      Stream stream = new MemoryStream();
      if (!documento.NoEsValido()) documento.Clone(stream);
      return stream;
    }

    /// <summary>
    /// Crea un arreglo de bytes a partir de un clon del documento
    /// </summary>
    /// <param name="documento">Referencia al documento</param>
    /// <returns>Arreglo de bytes</returns>
    public static byte[] Bytes(this SpreadsheetDocument documento)
    {
      byte[] bytes = new byte[0];
      if (documento.NoEsValido()) return bytes;
      Stream stream = documento.Stream();
      bytes = new byte[stream.Length];
      using (MemoryStream ms = new MemoryStream(bytes)) stream.CopyTo(ms);
      return bytes;
    }
  }
}
