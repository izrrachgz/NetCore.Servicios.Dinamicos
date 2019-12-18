using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using Negocio.Extensiones;
using Servicio.Modelos;
using Xunit;

namespace Negocio.Pruebas.Hechos.Extensiones
{
  public class Listas
  {
    [Fact]
    public void DocumentoExcel()
    {
      List<string> nombres = Enumerable.Repeat(@"Israel Ch", 10).ToList();
      RespuestaModelo<SpreadsheetDocument> guardado = nombres.DocumentoExcel();
      Assert.True(guardado.Correcto);
    }
  }
}
