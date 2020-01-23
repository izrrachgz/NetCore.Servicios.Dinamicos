using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Datos.Modelos;

namespace Datos.Extensiones
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

    /// <summary>
    /// Convierte una lista en una respuesta de coleccion
    /// </summary>
    /// <typeparam name="T">Tipo de objeto interpretado</typeparam>
    /// <param name="lista">Coleccion de elementos</param>
    /// <returns>Respuesta coleccion</returns>
    public static RespuestaColeccion<T> Convertir<T>(this List<T> lista)
    {
      return new RespuestaColeccion<T>(lista);
    }

    /// <summary>
    /// Convierte una lista en un arreglo
    /// de bytes
    /// </summary>
    /// <typeparam name="T">Tipo de elemento contenido en la coleccion</typeparam>
    /// <param name="lista">Coleccion de elementos</param>
    /// <returns>Arreglo de bytes</returns>
    public static byte[] ObtenerBytes<T>(this List<T> lista)
    {
      if (lista.NoEsValida()) return new byte[0] { };
      BinaryFormatter bf = new BinaryFormatter();
      MemoryStream ms = new MemoryStream();
      bf.Serialize(ms, lista);
      ms.Position = 0;
      return ms.ToArray();
    }
  }
}