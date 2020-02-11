using System.Collections.Generic;
using Contexto.Entidades;
using DocumentFormat.OpenXml.Packaging;
using Negocio.Extensiones;
using Datos.Modelos;
using Xunit;

namespace Negocio.Pruebas.Hechos.Extensiones
{
  /// <summary>
  /// Pruebas positivas de extensiones de RespuestaColeccion
  /// </summary>
  public class RespuestaColeccion
  {
    /// <summary>
    /// Comprueba que el objeto de respuesta
    /// se puede interpretar como un documento
    /// de excel
    /// </summary>
    [Fact]
    public void ReporteExcel()
    {
      List<EntradaLog> coleccion = new List<EntradaLog>(1)
      {
        new EntradaLog()
        {
          Nombre = @"Prueba",
          Descripcion = @"Descripcion de prueba",
        }
      };
      RespuestaColeccion<EntradaLog> respuesta = new RespuestaColeccion<EntradaLog>(coleccion);
      RespuestaModelo<SpreadsheetDocument> documento = respuesta.ReporteExcel();
      Assert.True(documento.Correcto);
    }
  }
}
