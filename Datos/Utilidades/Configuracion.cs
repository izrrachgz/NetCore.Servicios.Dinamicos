using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Datos.Extensiones;
using Datos.Modelos;

namespace Datos.Utilidades
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
        FileInfo info = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "ConfiguracionDatos.json");
        if (!info.Exists)
        {
          //El archivo de configuracion es obligatorio
          throw new Exception(@"No se ha encontrado el archivo de configuracion.");
        }
        Configuracion configuracion;
        using (FileStream fs = info.OpenRead())
        {
          using (StreamReader sr = new StreamReader(fs))
          {
            StringBuilder sb = new StringBuilder();
            while (!sr.EndOfStream)
              sb.Append(sr.ReadLine());
            configuracion = JsonConvert.DeserializeObject<Configuracion>(sb.ToString());
            sb.Clear();
            sr.Dispose();
          }
          fs.Dispose();
        }
        if (configuracion.CadenaDeConexion.NoEsValida())
        {
          //La cadena de conexion es obligatoria
          throw new Exception(@"Es necesario definir la cadena de conexion para acceder al repositorio de datos.");
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
    /// Coleccion de elementos de configuracion
    /// </summary>
    public List<ElementoConfiguracion> Configuraciones { get; set; }
  }
}
