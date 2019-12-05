using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Servicio.Modelos;
using Servicio.Extensiones;

namespace Servicio.Utilidades
{
  /// <summary>
  /// Provee la funcionalidad para obtener la informacion
  /// sobre configuraciones de la aplicacion
  /// </summary>
  internal sealed class Configuracion
  {
    /// <summary>
    /// Carga de configuracion a traves del archivo
    /// </summary>
    private static Configuracion Cargar
    {
      get
      {
        FileInfo info = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "Configuracion.json");
        if (!info.Exists) throw new Exception("No se ha encontrado el archivo de configuracion.");
        Configuracion configuracion;
        using (FileStream fs = info.OpenRead())
        {
          using (StreamReader sr = new StreamReader(fs))
          {
            configuracion = JsonConvert.DeserializeObject<Configuracion>(sr.ReadToEnd());
            sr.Dispose();
          }
          fs.Dispose();
        }
        return configuracion;
      }
    }

    /// <summary>
    /// Acceso a la configuracion
    /// </summary>
    public static Configuracion Instancia => Cargar;

    /// <summary>
    /// Cadena de conexion al repositorio de datos
    /// </summary>
    public string CadenaDeConexion { get; set; }

    /// <summary>
    /// Lista de configuraciones
    /// </summary>
    private List<ElementoConfiguracion> Configuraciones { get; set; }

    /// <summary>
    /// Permite obtener un valor de configuracion
    /// utilizando como busqueda la clave unica
    /// del elemento
    /// </summary>
    /// <typeparam name="T">Tipo de objeto interpretado retenido en el valor asociado</typeparam>
    /// <param name="clave">Clave de identificacion unica</param>
    /// <returns></returns>
    public T Obtener<T>(string clave)
    {
      if (Configuraciones.NoEsValida() || !Configuraciones.Any(c => c.Clave.Equals(clave)))
        return default(T);
      return (T)Configuraciones.First(c => c.Clave.Equals(clave)).Valor;
    }
  }
}
