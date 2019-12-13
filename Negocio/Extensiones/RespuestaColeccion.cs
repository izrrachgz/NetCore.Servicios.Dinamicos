using DocumentFormat.OpenXml.Packaging;
using Servicio.Contratos;
using Servicio.Modelos;

namespace Negocio.Extensiones
{
  public static class ExtensionesDeRespuestaColeccion
  {
    /// <summary>
    /// Crea un documento de excel a partir de la coleccion
    /// de entidades contenidas en una respuesta.
    /// </summary>
    /// <param name="respuesta">Instancia valida de respuesta coleccion</param>
    /// <param name="columnas">Columnas opcionales que deberan aparecer como encabezados</param>
    /// <returns>Documento Excel</returns>
    public static RespuestaModelo<SpreadsheetDocument> ReporteExcel(this RespuestaColeccion<IEntidad> respuesta)
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
      return respuesta.Coleccion.ObtenerComoExcel();
    }
  }
}
