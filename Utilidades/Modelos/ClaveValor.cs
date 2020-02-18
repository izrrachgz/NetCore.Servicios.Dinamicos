namespace Utilidades.Modelos
{
  /// <summary>
  /// Provee un modelo de datos para representar una entidad a su nivel basico
  /// de lista, solo su identidad clave y un valor de referencia
  /// </summary>
  public class ClaveValor
  {
    /// <summary>
    /// Identificador primario
    /// </summary>
    public string Clave { get; set; }

    /// <summary>
    /// Valor Asociado
    /// </summary>
    public object Valor { get; set; }
  }
}