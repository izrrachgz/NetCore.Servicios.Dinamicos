using DocumentFormat.OpenXml.Packaging;
using Datos.Modelos;

namespace Datos.Extensiones
{
  /// <summary>
  /// Provee metodos de extensión para respuestas del tipo coleccion
  /// </summary>
  public static class ExtensionesDeRespuestaColeccion
  {
    /// <summary>
    /// Crea un documento de excel a partir de la coleccion
    /// de entidades contenidas en una respuesta.
    /// </summary>
    /// <param name="respuesta">Instancia valida de respuesta coleccion</param>
    /// <param name="configuracion">Configuracion para aplicar al documento</param>
    /// <returns>Documento Excel</returns>
    public static RespuestaModelo<SpreadsheetDocument> ReporteExcel<T>(this RespuestaColeccion<T> respuesta, ConfiguracionReporteExcel configuracion = null)
    {
      //Verificar que la respuesta de coleccion sea correcta
      if (!respuesta.Correcto)
      {
        return new RespuestaModelo<SpreadsheetDocument>()
        {
          Correcto = false,
          Mensaje = respuesta.Mensaje
        };
      }
      return respuesta.Coleccion.DocumentoExcel(configuracion);
    }
  }
}
