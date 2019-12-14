using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using Negocio.Extensiones;
using Negocio.Modelos;
using Servicio.Modelos;
using Xunit;

namespace Negocio.Pruebas.Teorias
{
  public class Rendimiento
  {

    [Theory, InlineData(1, 1000)]
    public void GuardarListaEnExcel(short estimado = 1, int cantidad = 1000)
    {
      Stopwatch temporizador = new Stopwatch();
      List<string> lista = Enumerable.Repeat(@"Israel", cantidad).ToList();
      temporizador.Start();
      RespuestaModelo<SpreadsheetDocument> documento = lista.DocumentoExcel(new ConfiguracionReporteExcel()
      {
        Titulo = @"Reporte de Prueba",
        DirectorioDeSalida = AppDomain.CurrentDomain.BaseDirectory,
        Encabezados = new[] { @"Nombre" }
      });
      temporizador.Stop();
      Assert.True(documento.Correcto && temporizador.Elapsed.TotalSeconds <= estimado);
    }
  }
}
