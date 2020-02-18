using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Utilidades.Herramientas;
using Utilidades.Modelos;
using DocumentFormat.OpenXml.Packaging;
using Newtonsoft.Json;
using Utilidades.Mensajes;

namespace Utilidades.Extensiones
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

    /// <summary>
    /// Convierte una coleccion de entidades en un
    /// documento de excel
    /// </summary>
    /// <typeparam name="T">Tipo de entidad</typeparam>
    /// <param name="lista">Coleccion de entidades</param>
    /// <param name="configuracion">Configuracion para aplicar al documento</param>
    /// <returns>Directorio para acceder al archivo</returns>
    public static RespuestaModelo<SpreadsheetDocument> DocumentoExcel<T>(this List<T> lista, ConfiguracionReporteExcel configuracion = null)
    {
      //Verificar que la lista sea valida
      if (lista.NoEsValida())
      {
        return new RespuestaModelo<SpreadsheetDocument>()
        {
          Correcto = false,
          Mensaje = Error.ListaInvalida
        };
      }
      return Excel.GuardarContenidoDeLista(lista, configuracion);
    }

    /// <summary>
    /// Guarda un archivo en formato json utilizando
    /// la referencia de configuracion especificada
    /// </summary>
    /// <typeparam name="T">Tipo de elemento contenido en la coleccion</typeparam>
    /// <param name="lista">Coleccion de elementos</param>
    /// <param name="configuracion">Configuracion de archivo</param>
    /// <returns></returns>
    public static async Task<RespuestaBasica> GuardarArchivoComoJson<T>(this List<T> lista, ConfiguracionArchivo configuracion = null)
    {
      if (lista.NoEsValida())
        return new RespuestaBasica(false, Error.ListaInvalida);
      configuracion = configuracion ?? new ConfiguracionArchivo();
      if (!Directory.Exists(configuracion.DirectorioDeSalida))
      {
        try
        {
          Directory.CreateDirectory(configuracion.DirectorioDeSalida);
        }
        catch (Exception ex)
        {
          return new RespuestaBasica(ex);
        }
      }
      RespuestaBasica respuesta;
      try
      {
        string contenido = await Task.Run(() => JsonConvert.SerializeObject(lista));
        string directorio = $"{configuracion.DirectorioDeSalida}{configuracion.Nombre}.json";
        await File.WriteAllTextAsync(directorio, contenido, configuracion.Codificacion);
        FileInfo info = new FileInfo(directorio);
        respuesta = new RespuestaBasica(info.Exists, info.Exists ? @"Se ha guardado el archivo correctamente." : @"No se ha podido guardar el archivo.");
        if (configuracion.EliminarFuenteDeDatos)
        {
          contenido = @"";
          lista.Clear();
          lista.TrimExcess();
        }
      }
      catch (Exception ex)
      {
        respuesta = new RespuestaBasica(ex);
      }
      return respuesta;
    }

    /// <summary>
    /// Guarda un archivo en formato binario utilizando
    /// la referencia de configuracion especificada
    /// </summary>
    /// <typeparam name="T">Tipo de elemento contenido en la coleccion</typeparam>
    /// <param name="lista">Coleccion de elementos</param>
    /// <param name="configuracion">Configuracion de archivo</param>
    /// <returns></returns>
    public static async Task<RespuestaBasica> GuardarArchivoComoBytes<T>(this List<T> lista, ConfiguracionArchivo configuracion = null)
    {
      if (lista.NoEsValida())
        return new RespuestaBasica(false, Error.ListaInvalida);
      configuracion = configuracion ?? new ConfiguracionArchivo();
      if (!Directory.Exists(configuracion.DirectorioDeSalida))
      {
        try
        {
          Directory.CreateDirectory(configuracion.DirectorioDeSalida);
        }
        catch (Exception ex)
        {
          return new RespuestaBasica(ex);
        }
      }
      RespuestaBasica respuesta;
      try
      {
        configuracion.Extension = configuracion.Extension.NoEsValida() ? @"dat" : configuracion.Extension;
        string directorio = $"{configuracion.DirectorioDeSalida}{configuracion.Nombre}.{configuracion.Extension}";
        await File.WriteAllBytesAsync(directorio, lista.ObtenerBytes());
        FileInfo info = new FileInfo(directorio);
        respuesta = new RespuestaBasica(info.Exists, info.Exists ? @"Se ha guardado el archivo correctamente." : @"No se ha podido guardar el archivo.");
        if (configuracion.EliminarFuenteDeDatos)
        {
          lista.Clear();
          lista.TrimExcess();
        }
      }
      catch (Exception ex)
      {
        respuesta = new RespuestaBasica(ex);
      }
      return respuesta;
    }
  }
}