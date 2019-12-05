using System.Collections.Generic;
using System.Linq;
using Servicio.Modelos;

namespace Servicio.Extensiones
{
  /// <summary>
  /// Provee metodos de extensión para listas
  /// </summary>
  public static class ExtensionesDeListas
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

    /// <summary>
    /// Permite obtener un valor de configuracion
    /// utilizando como busqueda la clave unica
    /// del elemento
    /// </summary>
    /// <param name="lista">Coleccion de elementos de configuracion</param>
    /// <typeparam name="T">Tipo de objeto interpretado retenido en el valor asociado</typeparam>
    /// <param name="clave">Clave de identificacion unica</param>
    /// <returns></returns>
    public static T Obtener<T>(this List<ElementoConfiguracion> lista, string clave)
    {
      if (lista.NoEsValida() || !lista.Any(c => c.Clave.Equals(clave)))
        return default(T);
      return (T)lista.First(c => c.Clave.Equals(clave)).Valor;
    }
  }
}