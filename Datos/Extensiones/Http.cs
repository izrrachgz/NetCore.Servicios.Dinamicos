using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Datos.Modelos;
using DocumentFormat.OpenXml.Packaging;
using Newtonsoft.Json;

namespace Datos.Extensiones
{
  /// <summary>
  /// Provee metodos de extension para los mensajes http
  /// </summary>
  public static class ExtensionesHttp
  {
    /// <summary>
    /// Indica si el mensaje de respuesta http es nulo o
    /// no se encuentra dentro de un estado aceptable
    /// </summary>
    /// <param name="http">Referencia a la solicitud</param>
    /// <returns>Verdadero o falso</returns>
    public static bool NoEsValida(this HttpResponseMessage http)
    {
      return http == null || !http.IsSuccessStatusCode;
    }

    /// <summary>
    /// Adjunta un archivo a partir de un flujo de datos en memoria
    /// </summary>
    /// <param name="http">Referencia a la respuesta web</param>
    /// <param name="stream">Referencia al flujo de datos</param>
    /// <param name="tipoDeContenido">Nombre del tipo mime de contenido adjunto</param>
    /// <param name="nombre">Nombre del archivo adjunto</param>
    /// <returns>Respuesta web con archivo adjunto</returns>
    public static void AgregarAdjunto(this HttpResponseMessage http, Stream stream, string tipoDeContenido = null, string nombre = null)
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
    public static void AgregarAdjunto(this HttpResponseMessage http, byte[] bytes, string tipoDeContenido, string nombre = null)
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
    public static void AgregarAdjunto(this HttpResponseMessage http, FileInfo info, string tipoDeContenido, string nombre = null)
    {
      if (http.NoEsValida() || info.NoEsValido() || tipoDeContenido.NoEsValida()) return;
      AgregarAdjunto(http, new FileStream(info.FullName, FileMode.Open, FileAccess.Read), tipoDeContenido, nombre ?? info.Name);
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
    public static void AgregarAdjunto(this HttpResponseMessage http, string direccion, string tipoDeContenido, string nombre = null)
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
      AgregarAdjunto(http, new FileStream(info.FullName, FileMode.Open, FileAccess.Read), tipoDeContenido, nombre ?? info.Name);
    }

    /// <summary>
    /// Interpreta el contenido de la solicitud como una cadena json
    /// y lo deserializa hacia la instancia del tipo de objeto indicado
    /// </summary>
    /// <typeparam name="T">Tipo de objeto a deserializar</typeparam>
    /// <param name="http">Referencia de la solicitud</param>
    /// <returns>Respuesta modelo que contiene una instancia del tipo indicado</returns>
    public static async Task<RespuestaModelo<T>> ObtenerDeContenidoJson<T>(this HttpResponseMessage http)
    {
      if (http.NoEsValida() || !(http.Content is StringContent))
        return new RespuestaModelo<T>() { Correcto = false, Mensaje = @"El contenido de la solicitud no es valido." };
      RespuestaModelo<T> respuesta;
      try
      {
        T modelo = JsonConvert.DeserializeObject<T>(await http.Content.ReadAsStringAsync());
        respuesta = new RespuestaModelo<T>(modelo);
      }
      catch (Exception ex)
      {
        respuesta = new RespuestaModelo<T>(ex);
      }
      return respuesta;
    }

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
      http.AgregarAdjunto(resultado.Modelo.Stream(), nombre: $@"Reporte {DateTime.Now:s}.xlsx");
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
      http.AgregarAdjunto(resultado.Modelo.Stream(), nombre: $@"Reporte {DateTime.Now:s}.xlsx");
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
      http.AgregarAdjunto(documento.Stream());
    }

    /// <summary>
    /// Adjunta un objeto a un mensaje de respuesta
    /// en notacion de objetos de javascript
    /// </summary>
    /// <param name="http">Referencia a la respuesta</param>
    /// <param name="objeto">Referencia al objeto</param>
    /// <returns>Respuesta web con el objeto como contenido json</returns>
    public static void AdjuntarComoJson(this HttpResponseMessage http, object objeto)
    {
      if (http.NoEsValida() || objeto == null) return;
      http.Content = new StringContent(JsonConvert.SerializeObject(objeto), Encoding.UTF8, @"application/json");
    }
  }
}
