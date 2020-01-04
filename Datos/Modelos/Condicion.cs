using Datos.Enumerados;

namespace Datos.Modelos
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
      Operador = Operador.NoEs;
      Valor = 0;
    }

    public Condicion(string columna, object valor, Operador operador = Operador.Igual)
    {
      Columna = columna;
      Operador = operador;
      Valor = valor;
    }
  }
}
