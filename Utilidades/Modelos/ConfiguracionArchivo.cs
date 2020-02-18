using System;
using System.Text;

namespace Utilidades.Modelos
{
  /// <summary>
  /// Provee un modelo de datos para
  /// representar la configuracion
  /// general de un archivo
  /// </summary>
  public class ConfiguracionArchivo
  {
    /// <summary>
    /// Nombre completo sin directorio y extension
    /// </summary>
    public string Nombre { get; set; }

    /// <summary>
    /// Extension del archivo sin punto
    /// </summary>
    public string Extension { get; set; }

    /// <summary>
    /// Direccion hacia la carpeta de salida
    /// </summary>
    public string DirectorioDeSalida { get; set; }

    /// <summary>
    /// Codificacion del contenido del archivo
    /// </summary>
    public Encoding Codificacion { get; set; }

    /// <summary>
    /// Indica si se deben eliminar los recursos
    /// de origen utilizados durante operaciones
    /// </summary>
    public bool EliminarFuenteDeDatos { get; set; }

    public ConfiguracionArchivo()
    {
      Nombre = Guid.NewGuid().ToString(@"D");
      Extension = @"json";
      DirectorioDeSalida = AppDomain.CurrentDomain.BaseDirectory;
      Codificacion = Encoding.UTF8;
      EliminarFuenteDeDatos = true;
    }
  }
}
