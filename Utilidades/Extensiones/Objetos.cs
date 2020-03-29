using System.Collections.Generic;

namespace Utilidades.Extensiones
{
  /// <summary>
  /// Provee metodos de extension para objetos primitivos
  /// </summary>
  public static class ExtensionesDeObjetos
  {
    /// <summary>
    /// Indica si el objeto proporcionado corresponde a una lista
    /// que retiene datos del tipo cadena
    /// </summary>
    /// <param name="colecccion">Coleccion de valores</param>
    /// <returns>Verdadero o falso</returns>
    public static bool EsColeccionDeCaracteres(this object coleccion)
    {
      return coleccion is List<string> || coleccion is string[];
    }

    /// <summary>
    /// Indica si el objeto proporcionado corresponde a una lista
    /// que retiene datos del tipo numerico
    /// </summary>
    /// <param name="coleccion">Coleccion de valores</param>
    /// <returns>Verdadero o falso</returns>
    public static bool EsColeccionNumerica(this object coleccion)
    {
      bool es = coleccion is IEnumerable<sbyte> ||
        coleccion is IEnumerable<byte> ||
        coleccion is IEnumerable<ushort> ||
        coleccion is IEnumerable<short> ||
        coleccion is IEnumerable<uint> ||
        coleccion is IEnumerable<int> ||
        coleccion is IEnumerable<ulong> ||
        coleccion is IEnumerable<long> ||
        coleccion is IEnumerable<float> ||
        coleccion is IEnumerable<double> ||
        coleccion is IEnumerable<decimal>;
      return es;
    }
  }
}
