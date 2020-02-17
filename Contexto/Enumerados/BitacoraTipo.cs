namespace Contexto.Enumerados
{
  /// <summary>
  /// Tipos de entradas admitidas en
  /// el la bitacora
  /// </summary>
  public enum BitacoraTipo
  {
    /// <summary>
    /// Utilizada cuando la entrada
    /// indica un contexto informativo
    /// </summary>
    Informacion = 0,

    /// <summary>
    /// Utilizada cuando la entrada
    /// indica un contexto amenazante
    /// </summary>
    Advertencia = 1,

    /// <summary>
    /// Utilizada cuando la entrada
    /// indica un contexto erroneo
    /// </summary>
    Error = 2,
  }
}
