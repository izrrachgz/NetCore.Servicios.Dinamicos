using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using Negocio.Modelos;
using Negocio.Utilidades;
using Servicio.Extensiones;
using Servicio.Modelos;

namespace Negocio.Extensiones
{
  /// <summary>
  /// Provee metodos de extensión para listas
  /// </summary>
  public static class ExtensionesDeListas
  {
    /// <summary>
    /// Convierte una coleccion de entidades en un
    /// documento de excel
    /// </summary>
    /// <typeparam name="T">Tipo de entidad</typeparam>
    /// <param name="lista">Coleccion de entidades</param>
    /// <param name="configuracion">Configuracion para aplicar al documento</param>
    /// <returns>Directorio para acceder al archivo</returns>
    public static RespuestaModelo<SpreadsheetDocument> DocumentoExcel<T>(this List<T> lista, ConfiguracionReporteExcel configuracion = null)
    {
      //Verificar que la lista sea valida
      if (lista.NoEsValida())
      {
        return new RespuestaModelo<SpreadsheetDocument>()
        {
          Correcto = false,
          Mensaje = @"La lista de contenido no es valida."
        };
      }
      return Excel.GuardarContenidoDeLista(lista, configuracion);
    }
  }
}
