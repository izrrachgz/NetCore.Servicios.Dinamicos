using System.Diagnostics;
using System.Threading.Tasks;

namespace Api.Pruebas.Modelos
{
  /// <summary>
  /// Proporciona el modelo de datos para almacenar
  /// la informacion transferida de una tarea
  /// y obtiene la metrica al concluirla
  /// </summary>
  public sealed class MetricaDeTarea<T>
  {
    /// <summary>
    /// Marcador utilizado para medir el tiempo
    /// de respuesta de la tarea
    /// </summary>
    public Stopwatch Cronometro { get; set; }

    /// <summary>
    /// Referencia de la solicitud efectuada
    /// </summary>
    public Task<T> Tarea { get; }

    /// <summary>
    /// Referencia de la respuesta obtenida
    /// asociada a la tarea
    /// </summary>
    public T Respuesta { get; set; }

    /// <summary>
    /// Indica si la tarea ha concluido correctamente
    /// </summary>
    public bool Correcto { get; set; }

    public MetricaDeTarea(Task<T> tarea)
    {
      Cronometro = new Stopwatch();
      Tarea = tarea;
    }
  }
}
