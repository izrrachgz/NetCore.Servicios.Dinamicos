using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using Negocio.Extensiones;
using Datos.Entidades;
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
      List<Usuario> usuarios = new List<Usuario>(1)
      {
        new Usuario()
        {
          Nombre = @"Pruebas",
          ApellidoPaterno = @"Pruebas",
          ApellidoMaterno = @"Pruebas",
          Correo = @"Pruebas@CSharp.com",
          NumeroContacto = @"6623559566"
        }
      };
      RespuestaColeccion<Usuario> respuesta = new RespuestaColeccion<Usuario>(usuarios);
      RespuestaModelo<SpreadsheetDocument> documento = respuesta.ReporteExcel();
      Assert.True(documento.Correcto);
    }
  }
}
