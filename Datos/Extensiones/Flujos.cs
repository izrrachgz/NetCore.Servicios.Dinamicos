using System.IO;

namespace Datos.Extensiones
{
  /// <summary>
  /// Provee metodos de extension para los flujos de datos
  /// </summary>
  public static class ExtensionesDeFlujos
  {
    /// <summary>
    /// Indica si el flujo de datos es nulo o
    /// no tiene espacio definido
    /// </summary>
    /// <param name="stream">Referencia al flujo de datos</param>
    /// <returns>Verdadero o falso</returns>
    public static bool NoEsValido(this Stream stream)
    {
      return stream == null || stream.Length.Equals(0) || stream.Position.Equals(0);
    }
  }
}
