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
    /// <param name="tipoDeContenido">Nombre del tipo de contenido adjunto</param>
    /// <returns>Respuesta web con el documento adjunto</returns>
    private static void AgregarAdjunto(HttpResponseMessage http, Stream stream, string nombre = null, string tipoDeContenido = null)
    {
      if (http.NoEsValida() || stream.NoEsValido()) return;
      http.Content = new StreamContent(stream);
      http.Content.Headers.ContentType = new MediaTypeHeaderValue(tipoDeContenido ?? "application/octet-stream");
      http.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
      {
        FileName = nombre ?? $@"{DateTime.Now:s}"
      };
    }

    /// <summary>
    /// Agrega un arreglo de bytes a una respuesta web como adjunto
    /// </summary>
    /// <param name="http">Referencia a la respuesta</param>
    /// <param name="bytes">Arreglo de bytes del archivo</param>
    /// <param name="tipoDeContenido">Nombre del tipo mime de contenido adjunto</param>
    /// <param name="nombre">Nombre del documento adjunto</param>
    /// <returns>Respuesta web con el documento adjunto</returns>
    public static void AgregarAdjunto(HttpResponseMessage http, byte[] bytes, string tipoDeContenido, string nombre = null)
    {
      if (http.NoEsValida() || bytes.NoEsValido() || tipoDeContenido.NoEsValida()) return;
      http.Content = new ByteArrayContent(bytes);
      http.Content.Headers.ContentType = new MediaTypeHeaderValue(tipoDeContenido);
      http.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
      {
        FileName = nombre ?? $@"{DateTime.Now:s}"
      };
    }

    /// <summary>
    /// Agrega un stream a una respuesta web como adjunto
    /// a partir de la informacion del archivo
    /// </summary>
    /// <param name="http">Referencia a la respuesta</param>
    /// <param name="info">Informacion sobre el archivo</param>
    /// <param name="tipoDeContenido">Nombre del tipo mime de contenido adjunto</param>
    /// <param name="nombre">Nombre del documento adjunto</param>
    /// <returns>Respuesta web con el documento adjunto</returns>
    public static void AgregarAdjunto(HttpResponseMessage http, FileInfo info, string tipoDeContenido, string nombre = null)
    {
      if (http.NoEsValida() || info.NoEsValido() || tipoDeContenido.NoEsValida()) return;
      AgregarAdjunto(http, new FileStream(info.FullName, FileMode.Open, FileAccess.Read), nombre ?? info.Name, tipoDeContenido);
    }

    /// <summary>
    /// Agrega un stream a una respuesta web como adjunto
    /// a partir de una ruta absoluta a un archivo
    /// </summary>
    /// <param name="http">Referencia a la respuesta</param>
    /// <param name="direccion">Informacion sobre el archivo</param>
    /// <param name="tipoDeContenido">Nombre del tipo mime de contenido adjunto</param>
    /// <param name="nombre">Nombre del documento adjunto</param>
    /// <returns>Respuesta web con el documento adjunto</returns>
    public static void AgregarAdjunto(HttpResponseMessage http, string direccion, string tipoDeContenido, string nombre = null)
    {
      if (http.NoEsValida() || direccion.NoEsValida() || direccion.EsDireccionWeb() || tipoDeContenido.NoEsValida()) return;
      FileInfo info;
      try
      {
        info = new FileInfo(direccion);
      }
      catch (Exception)
      {
        info = null;
      }
      if (info.NoEsValido()) return;
      AgregarAdjunto(http, new FileStream(info.FullName, FileMode.Open, FileAccess.Read), nombre ?? info.Name, tipoDeContenido);
    }

    /// <summary>
    /// Adjunta un archivo a partir de un flujo de datos en memoria
    /// </summary>
    /// <param name="http">Referencia a la respuesta web</param>
    /// <param name="stream">Referencia al flujo de datos</param>
    /// <param name="nombre">Nombre del archivo adjunto</param>
    /// <param name="tipoDeContenido">Nombre del tipo mime de contenido adjunto</param>
    /// <returns>Respuesta web con archivo adjunto</returns>
    public static void AdjuntarArchivo(this HttpResponseMessage http, Stream stream, string nombre = null, string tipoDeContenido = null)
    => AgregarAdjunto(http, stream, nombre, tipoDeContenido);

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
      AgregarAdjunto(http, resultado.Modelo.Stream(), $@"Reporte {DateTime.Now:s}.xlsx");
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
      AgregarAdjunto(http, resultado.Modelo.Stream(), $@"Reporte {DateTime.Now:s}.xlsx");
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
