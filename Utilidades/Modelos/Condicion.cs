using Utilidades.Enumerados;

namespace Utilidades.Modelos
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

    /// <summary>
    /// Crea una nueva instancia de condicion
    /// </summary>
    /// <param name="columna">Propiedad referente de comparacion</param>    
    /// <param name="operador">Operador de comparacion</param>
    /// <param name="valor">Valor referente de comparacion contra el valor de propiedad</param>
    public Condicion(string columna, Operador operador, object valor)
    {
      Columna = columna;
      Operador = operador;
      Valor = valor;
    }
  }
}
