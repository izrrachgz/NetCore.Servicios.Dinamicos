namespace Datos.Enumerados
{
  /// <summary>
  /// Representa el tipos de operadores de comparación aplicado
  /// </summary>
  public enum Operador
  {
    /// <summary>
    /// Se utiliza cuando se requiere comparar igualdad con el valor
    /// proporcionado (=)
    /// </summary>
    Igual = 0,

    /// <summary>
    /// Se utiliza cuando se requiere comparar menor que el valor
    /// proporcionado (<)
    /// </summary>
    Menor = 1,

    /// <summary>
    /// Se utiliza cuando se requiere comparar menor o igual que el valor
    /// proporcionado (<=)
    /// </summary>
    MenorIgual = 2,

    /// <summary>
    /// Se utiliza cuando se requiere comparar distinto que el valor
    /// proporcionado (<>)
    /// </summary>
    Distinto = 3,

    /// <summary>
    /// Se utiliza cuando se requiere comparar mayor que el valor
    /// proporcionado (>)
    /// </summary>
    Mayor = 4,

    /// <summary>
    /// Se utiliza cuando se requiere comparar mayor o igual que el valor
    /// proporcionado (>=)
    /// </summary>
    MayorIgual = 5,

    /// <summary>
    /// Se utiliza cuando se requiere comparar diferente que el valor
    /// proporcionado (!=)
    /// </summary>
    Diferente = 6,

    /// <summary>
    /// Se utiliza cuando se requiere comparar diferente que el valor
    /// proporcionado (is)
    /// </summary>
    Es = 7,

    /// <summary>
    /// Se utiliza cuando se requiere comparar diferente que el valor
    /// proporcionado (is not)
    /// </summary>
    NoEs = 8,

    /// <summary>
    /// Se utiliza cuando se requiere comparar un valor de cadena parecido que al valor
    /// proporcionado (LIKE)
    /// </summary>
    Parecido = 9,
  }
}
