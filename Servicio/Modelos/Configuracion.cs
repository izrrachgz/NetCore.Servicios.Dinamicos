using System;
using System.IO;
using Newtonsoft.Json;

namespace Servicio.Modelos
{
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
  }
}
