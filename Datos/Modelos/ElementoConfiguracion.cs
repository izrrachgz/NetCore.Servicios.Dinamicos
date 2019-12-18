namespace Datos.Modelos
{
  /// <summary>
  /// Provee un modelo de datos para representar un valor
  /// de configuracion a su nivel basico con su identificador
  /// unico y un valor asignado
  /// </summary>
  public class ElementoConfiguracion
  {
    /// <summary>
    /// Identificador
    /// </summary>
    public string Clave { get; set; }

    /// <summary>
    /// Valor Asociado
    /// </summary>
    public object Valor { get; set; }
  }
}