using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Contexto.Entidades;
using Contexto.Enumerados;
using DocumentFormat.OpenXml.Packaging;
using Negocio.Extensiones;
using Negocio.Modelos;
using Datos.Modelos;
using Xunit;

namespace Negocio.Pruebas.Teorias
{
  /// <summary>
  /// Pruebas teoricas de rendimiento
  /// </summary>
  public class Rendimiento
  {
    private EntradaLog Modelo { get; }

    public Rendimiento()
    {
      Modelo = new EntradaLog()
      {
        Nombre = @"Death Note",
        Descripcion = @"6:40",
        Tipo = EntradaLogTipo.Advertencia
      };
    }

    /// <summary>
    /// Verifica la posibilidad de que la coleccion
    /// de objetos se guarde en un archivo de reporte
    /// excel en disco antes de que se agote el tiempo
    /// estimado
    /// </summary>
    /// <param name="estimado">Tiempo estimado en segundos para concluir la tarea</param>
    /// <param name="cantidad">Cantidad de elementos que conformaran el listado</param>
    [Theory, InlineData(1, 1000)]
    public void GuardarListaEnExcel(short estimado = 1, int cantidad = 1000)
    {
      Stopwatch temporizador = new Stopwatch();
      List<EntradaLog> lista = Enumerable.Repeat(Modelo, cantidad).ToList();
      temporizador.Start();
      RespuestaModelo<SpreadsheetDocument> documento = lista.DocumentoExcel(new ConfiguracionReporteExcel()
      {
        Titulo = @"Reporte de Entradas",
        DirectorioDeSalida = AppDomain.CurrentDomain.BaseDirectory,
      });
      temporizador.Stop();
      Assert.True(documento.Correcto && temporizador.Elapsed.TotalSeconds <= estimado);
    }
  }
}
