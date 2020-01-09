using Datos.Extensiones;

namespace Datos.Configuraciones
{
  /// <summary>
  /// Provee un modelo de datos para representar
  /// la configuracion que se asocia al contexto
  /// de datos
  /// </summary>
  internal sealed class ConfiguracionDatos : ConfiguracionBase
  {
    /// <summary>
    /// Cadena de conexion al repositorio de datos
    /// </summary>
    public string CadenaDeConexion { get; set; }

    /// <summary>
    /// Verifica que el contenido de la configuracion
    /// de datos contenga la definicion de la cadena
    /// de conexion hacia el repositorio de los datos
    /// </summary>
    /// <returns>Verdadero o falso</returns>
    public override bool EsValida()
    {
      return !CadenaDeConexion.NoEsValida();
    }
  }
}
