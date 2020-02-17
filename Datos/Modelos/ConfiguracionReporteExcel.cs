namespace Datos.Modelos
{
  /// <summary>
  /// Provee un modelo de datos para representar
  /// la configuracion aplicable a un documento de excel
  /// </summary>
  public class ConfiguracionReporteExcel : ConfiguracionArchivo
  {
    /// <summary>
    /// Nombre de la hoja
    /// </summary>
    public string Titulo { get; set; }

    /// <summary>
    /// Encabezados del contenido
    /// </summary>
    public string[] Encabezados { get; set; }

    /// <summary>
    /// Clave de proteccion
    /// </summary>
    public string Clave { get; set; }

    public ConfiguracionReporteExcel()
    {
      Titulo = @"Reporte";
      Encabezados = null;
      DirectorioDeSalida = null;
      Clave = null;
    }
  }
}
