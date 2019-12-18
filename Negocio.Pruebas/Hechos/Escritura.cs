using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using Negocio.Extensiones;
using Negocio.Modelos;
using Datos.Modelos;
using Xunit;

namespace Negocio.Pruebas.Hechos
{
  public class Escritura
  {
    [Fact]
    public void GuardarArchivoExcel()
    {
      List<int> numeros = Enumerable.Range(1, 10).ToList();
      RespuestaModelo<SpreadsheetDocument> guardado = numeros.DocumentoExcel(new ConfiguracionReporteExcel()
      {
        Titulo = @"Reporte Númerico",
        DirectorioDeSalida = AppDomain.CurrentDomain.BaseDirectory
      });
      Assert.True(guardado.Correcto);
    }
  }
}
