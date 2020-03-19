namespace Utilidades.Modelos
{
  /// <summary>
  /// Provee un modelo de datos para representar una entidad a su nivel basico
  /// de lista, solo su identidad secuencial y un valor de referencia
  /// </summary>
  public class IndiceValor
  {
    /// <summary>
    /// Identificador primario
    /// </summary>
    public long Indice { get; set; }

    /// <summary>
    /// Valor Asociado
    /// </summary>
    public string Valor { get; set; }
  }
}
