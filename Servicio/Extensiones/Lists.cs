using System.Collections.Generic;
using System.Linq;

namespace Servicio.Extensiones
{
  /// <summary>
  /// Provee metodos de extensión para listas
  /// </summary>
  public static class ListsExtensions
  {
    /// <summary>
    /// Indica si la lista es nula o no contiene ningun elemento
    /// </summary>
    /// <typeparam name="T">Objeto genérico admitido en la lista</typeparam>
    /// <param name="list">Lista para comprobar</param>
    /// <returns>Verdadero o falso</returns>
    public static bool NoEsValida<T>(this List<T> list)
    {
      return list == null || !list.Any();
    }
  }
}