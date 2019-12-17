using System.Net.Http;

namespace Servicio.Extensiones
{
  /// <summary>
  /// Provee metodos de extension para los mensajes http
  /// </summary>
  public static class ExtensionesDeMensajesHttp
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
  }
}
