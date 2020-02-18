using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using Utilidades.Extensiones;
using Utilidades.Modelos;
using Xunit;

namespace Utilidades.Pruebas.Hechos
{
  /// <summary>
  /// Pruebas positivas de escritura de datos
  /// </summary>
  public class Escritura
  {
    /// <summary>
    /// Comprueba que la coleccion de datos
    /// se puede guardar como un archivo
    /// excel en disco
    /// </summary>
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
