using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
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
    /// Guarda un archivo excel simple en el directorio
    /// de ejecucion de la aplicacion, o en un directorio
    /// si es proporcionada una ruta y tiene permisos de escritura.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad</typeparam>
    /// <param name="lista">Coleccion de entidades</param>
    /// <param name="directorio">Directorio para guardar el archivo</param>
    /// <returns>Directorio para acceder al archivo</returns>
    public static RespuestaModelo<string> GuardarComoArchivoExcel<T>(this List<T> lista, string directorio = null)
    {
      //Verificar que la lista sea valida
      if (lista.NoEsValida())
      {
        return new RespuestaModelo<string>()
        {
          Correcto = false,
          Mensaje = @"La lista de contenido no es valida."
        };
      }
      string directorioBase = AppDomain.CurrentDomain.BaseDirectory;
      directorio = directorio ?? directorioBase;
      string direccionPlantilla = $@"{directorioBase}\Plantillas\Reportes\RespuestaColeccion.xlsx";
      RespuestaBasica existePlantilla = ReporteExcel.ExisteArchivo(direccionPlantilla);
      if (!existePlantilla.Correcto)
      {
        return new RespuestaModelo<string>()
        {
          Correcto = false,
          Mensaje = existePlantilla.Mensaje
        };
      }
      string directorioSalida = $@"{directorio}{Guid.NewGuid():N}.xlsx";
      using (SpreadsheetDocument documento = SpreadsheetDocument.CreateFromTemplate(direccionPlantilla))
      {
        RespuestaModelo<SpreadsheetDocument> guardado = ReporteExcel.GuardarContenidoEnExcel(documento, lista);
        if (guardado.Correcto)
        {
          documento.SaveAs(directorioSalida);
        }
      }
      return new RespuestaModelo<string>(directorioSalida);
    }

    /// <summary>
    /// Convierte una coleccion de entidades en un
    /// documento de excel
    /// </summary>
    /// <typeparam name="T">Tipo de entidad</typeparam>
    /// <param name="lista">Coleccion de entidades</param>
    /// <returns>Directorio para acceder al archivo</returns>
    public static RespuestaModelo<SpreadsheetDocument> ObtenerComoExcel<T>(this List<T> lista)
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
      string directorioBase = AppDomain.CurrentDomain.BaseDirectory;
      string direccionPlantilla = $@"{directorioBase}\Plantillas\Reportes\RespuestaColeccion.xlsx";
      RespuestaBasica existePlantilla = ReporteExcel.ExisteArchivo(direccionPlantilla);
      if (!existePlantilla.Correcto)
      {
        return new RespuestaModelo<SpreadsheetDocument>()
        {
          Correcto = false,
          Mensaje = existePlantilla.Mensaje
        };
      }
      SpreadsheetDocument documento;
      using (documento = SpreadsheetDocument.CreateFromTemplate(direccionPlantilla))
      {
        RespuestaModelo<SpreadsheetDocument> guardado = ReporteExcel.GuardarContenidoEnExcel(documento, lista);
        if (guardado.Correcto)
        {
          documento = guardado.Modelo;
        }
      }
      return new RespuestaModelo<SpreadsheetDocument>(documento);
    }
  }
}
