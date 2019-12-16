using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using DocumentFormat.OpenXml.Packaging;
using Servicio.Extensiones;
using Servicio.Modelos;

namespace Negocio.Extensiones
{
  /// <summary>
  /// Provee metodos de extension para las respuestas tipo http
  /// </summary>
  public static class ExtensionesDeRespuestaHttp
  {
    /// <summary>
    /// Agrega un stream a una respuesta web como adjunto
    /// </summary>
    /// <param name="http">Referencia a la respuesta</param>
    /// <param name="stream">Flujo de datos en memoria</param>
    /// <param name="nombre">Nombre del documento adjunto</param>
    /// <returns>Respuesta web con el documento adjunto</returns>
    private static void AgregarAdjunto(HttpResponseMessage http, Stream stream, string nombre = null)
    {
      if (http == null || stream == null) return;
      http.Content = new StreamContent(stream);
      http.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
      http.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
      {
        FileName = nombre ?? $@"Reporte{DateTime.Now:s}.xlsx",
      };
    }

    /// <summary>
    /// Adjunta un archivo a partir de un flujo de datos en memoria
    /// </summary>
    /// <param name="http">Referencia a la respuesta web</param>
    /// <param name="stream">Referencia al flujo de datos</param>
    /// <param name="nombre">Nombre del archivo adjunto</param>
    /// <returns>Respuesta web con archivo adjunto</returns>
    public static void AdjuntarArchivo(this HttpResponseMessage http, Stream stream, string nombre = null)
    => AgregarAdjunto(http, stream, nombre);


    /// <summary>
    /// Adjunta un documento excel a partir de una coleccion
    /// a un mensaje de respuesta
    /// </summary>
    /// <typeparam name="T">Tipo de entidad en la respuesta coleccion</typeparam>
    /// <param name="http">Referencia a la respuesta</param>
    /// <param name="respueta">Referencia a la respuesta coleccion</param>
    /// <returns>Respuesta web con el documento como adjunto</returns>
    public static void AdjuntarComoExcel<T>(this HttpResponseMessage http, RespuestaColeccion<T> respueta)
    {
      if (!respueta.Correcto) return;
      RespuestaModelo<SpreadsheetDocument> resultado = respueta.Coleccion.DocumentoExcel();
      if (!resultado.Correcto) return;
      AgregarAdjunto(http, resultado.Modelo.Stream());
    }

    /// <summary>
    /// Adjunta un documento excel a partir de una coleccion
    /// a un mensaje de respuesta
    /// </summary>
    /// <typeparam name="T">Tipo de entidad en la coleccion</typeparam>
    /// <param name="http">Referencia a la respuesta</param>
    /// <param name="lista">Referencia a la coleccion</param>
    /// <returns>Respuesta web con el documento como adjunto</returns>
    public static void AdjuntarComoExcel<T>(this HttpResponseMessage http, List<T> lista)
    {
      if (lista.NoEsValida()) return;
      RespuestaModelo<SpreadsheetDocument> resultado = lista.DocumentoExcel();
      if (!resultado.Correcto) return;
      AgregarAdjunto(http, resultado.Modelo.Stream());
    }

    /// <summary>
    /// Adjunta un documento excel a un mensaje de respuesta
    /// </summary>
    /// <param name="http">Referencia a la respuesta</param>
    /// <param name="documento">Referencia al documento</param>
    /// <returns>Respuesta web con el documento como adjunto</returns>
    public static void AdjuntarExcel(this HttpResponseMessage http, SpreadsheetDocument documento)
    {
      if (documento.NoEsValido()) return;
      AgregarAdjunto(http, documento.Stream());
    }
  }
}
