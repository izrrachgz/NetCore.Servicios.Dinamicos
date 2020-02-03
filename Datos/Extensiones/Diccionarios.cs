using System.Collections.Generic;

namespace Datos.Extensiones
{
  /// <summary>
  /// Provee metodos de extension para diccionarios
  /// </summary>
  public static class ExtensionesDeDiccionarios
  {
    /// <summary>
    /// Indica si los datos de un diccionario
    /// son nulos o vacios
    /// </summary>
    /// <param name="diccionario">Referencia al diccionario</param>
    /// <returns>Verdadero o falso</returns>
    public static bool NoEsValido<T>(this Dictionary<T, T> diccionario)
    {
      return diccionario == null || diccionario.Count.Equals(0);
    }
  }
}
