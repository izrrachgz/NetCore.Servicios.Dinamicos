using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using Datos.Extensiones;
using Datos.Modelos;
using Xunit;

namespace Negocio.Pruebas.Hechos
{
  /// <summary>
  /// Pruebas positivas de extensiones de escritura
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
