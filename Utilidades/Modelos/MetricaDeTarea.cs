using System.Diagnostics;
using System.Threading.Tasks;

namespace Utilidades.Modelos
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

    public MetricaDeTarea(Task<T> tarea)
    {
      Cronometro = new Stopwatch();
      Tarea = tarea;
    }

    /// <summary>
    /// Permite calcular la metrica de la tarea asociada
    /// </summary>
    /// <returns></returns>
    public void CalcularMetrica()
    {
      Cronometro.Reset();
      Cronometro.Start();
      //Esperar a que la tarea termine en el hilo asignado
      Tarea.Wait();
      Cronometro.Stop();
    }
  }
}
