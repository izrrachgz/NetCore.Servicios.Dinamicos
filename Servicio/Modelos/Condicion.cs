namespace Servicio.Modelos
{
  /// <summary>
  /// Provee un modelo de datos para representar
  /// una condicion de comparación entre valores de columna
  /// </summary>
  public class Condicion
  {
    /// <summary>
    /// Columna que se va a evaluar
    /// </summary>
    public string Columna { get; }

    /// <summary>
    /// Operador de comparación
    /// </summary>
    public Operador Operador { get; }

    /// <summary>
    /// Valor referente a la comparación
    /// </summary>
    public object Valor { get; }

    public Condicion()
    {
      Columna = "Id";
      Operador = Operador.Mayor;
      Valor = 0;
    }

    public Condicion(string columna, object valor, Operador operador = Operador.Igual)
    {
      Columna = columna;
      Operador = operador;
      Valor = valor;
    }
  }

  /// <summary>
  /// Tipos de operadores de comparación
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
  }
}
