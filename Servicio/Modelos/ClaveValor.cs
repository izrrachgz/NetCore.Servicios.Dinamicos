namespace Servicio.Modelos
{
  /// <summary>
  /// Permite representar una entidad a su nivel basico
  /// de lista, solo su identidad secuencial y un valor
  /// de referencia
  /// </summary>
  public class ClaveValor
  {
    /// <summary>
    /// Identificador primario
    /// </summary>
    public int Clave { get; set; }

    /// <summary>
    /// Valor Asociado
    /// </summary>
    public string Valor { get; set; }
  }
}
