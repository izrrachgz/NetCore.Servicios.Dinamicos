using System.IO;

namespace Servicio.Extensiones
{
  /// <summary>
  /// Provee metodos de extension para archivos
  /// </summary>
  public static class ExtensionesDeArchivos
  {
    /// <summary>
    /// Indica si la informacion sobre un
    /// archivo es nula o no existe en la ruta
    /// proporcionada
    /// </summary>
    /// <param name="info">Referencia de informacion</param>
    /// <returns>Verdadero o falso</returns>
    public static bool NoEsValido(this FileInfo info)
    {
      return info == null || !info.Exists;
    }
  }
}
