using DocumentFormat.OpenXml.Packaging;
using Negocio.Extensiones;
using Datos.Extensiones;
using Xunit;

namespace Negocio.Pruebas.Hechos.Extensiones
{
  /// <summary>
  /// Pruebas positivas de extensiones de reportes
  /// </summary>
  public class Reportes
  {
    /// <summary>
    /// Comprueba que el documento de excel
    /// no es valido para su uso
    /// </summary>
    [Fact]
    public void NoEsValido()
    {
      SpreadsheetDocument documento = null;
      Assert.True(documento.NoEsValido());
    }

    /// <summary>
    /// Comprueba que el objeto de flujo
    /// no es valido para su uso
    /// </summary>
    [Fact]
    public void Stream()
    {
      SpreadsheetDocument documento = null;
      Assert.True(documento.Stream().NoEsValido());
    }

    /// <summary>
    /// Comprueba que el objeto de bytes
    /// no es valido para su uso
    /// </summary>
    [Fact]
    public void Bytes()
    {
      SpreadsheetDocument documento = null;
      Assert.True(documento.Bytes().NoEsValido());
    }
  }
}
