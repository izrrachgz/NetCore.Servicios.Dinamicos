using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using Datos.Extensiones;
using Datos.Modelos;
using Newtonsoft.Json;

namespace Negocio.Extensiones
{
  /// <summary>
  /// Provee metodos de extension para las respuestas tipo http
  /// </summary>
  public static class ExtensionesDeRespuestaHttp
  {
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
  }
}
