using System;
using System.IO;
using System.Text;
using Datos.Contratos;
using Newtonsoft.Json;

namespace Datos.Configuraciones
{
  /// <summary>
  /// Provee la funcionalidad para obtener la informacion
  /// sobre configuraciones de la aplicacion
  /// </summary>
  public class Configuracion<T> where T : IConfiguracion
  {
    /// <summary>
    /// Carga de configuracion a traves del archivo
    /// </summary>
    [JsonIgnore]
    private static T Cargar
    {
      get
      {
        string nombre = typeof(T).Name;
        //El archivo de configuracion debe estar en la carpeta de ambito actual.
        FileInfo info = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + $@"{nombre}.json");
        if (!info.Exists)
        {
          //El archivo de configuracion es obligatorio
          throw new Exception(@"No se ha encontrado el archivo de configuracion.");
        }
        T configuracion;
        using (FileStream fs = info.OpenRead())
        {
          using (StreamReader sr = new StreamReader(fs))
          {
            StringBuilder sb = new StringBuilder();
            while (!sr.EndOfStream)
              sb.Append(sr.ReadLine());
            configuracion = JsonConvert.DeserializeObject<T>(sb.ToString());
            sb.Clear();
            sr.Dispose();
          }
          fs.Dispose();
        }
        //Si la configuracion definida no es valida, se detendra la ejecucion
        if (!configuracion.EsValida())
        {
          throw new Exception($@"El contenido del archivo de configuracion '{nombre}.json' no es valido.");
        }
        return configuracion;
      }
    }

    /// <summary>
    /// Acceso a la configuracion
    /// </summary>
    [JsonIgnore]
    public static T Instancia => Cargar;
  }
}
