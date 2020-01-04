namespace Datos.Enumerados
{
  /// <summary>
  /// Representa el tipo de union aplicado
  /// </summary>
  public enum TipoUnion
  {
    /// <summary>
    /// Unión interna (inner join)
    /// </summary>
    Interna = 0,

    /// <summary>
    /// Unión a la izqueirda (left join)
    /// </summary>
    Izquierda = 1,

    /// <summary>
    /// Unión a la derecha (right join)
    /// </summary>
    Derecha = 2,
  }
}
