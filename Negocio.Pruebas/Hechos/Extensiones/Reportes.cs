using DocumentFormat.OpenXml.Packaging;
using Negocio.Extensiones;
using Servicio.Extensiones;
using Xunit;

namespace Negocio.Pruebas.Hechos.Extensiones
{
  public class Reportes
  {
    [Fact]
    public void NoEsValido()
    {
      SpreadsheetDocument documento = null;
      Assert.True(documento.NoEsValido());
    }

    [Fact]
    public void Stream()
    {
      SpreadsheetDocument documento = null;
      Assert.True(documento.Stream().NoEsValido());
    }

    [Fact]
    public void Bytes()
    {
      SpreadsheetDocument documento = null;
      Assert.True(documento.Bytes().NoEsValido());
    }
  }
}
