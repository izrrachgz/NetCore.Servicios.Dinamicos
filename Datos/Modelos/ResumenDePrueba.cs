using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Datos.Modelos
{
  /// <summary>
  /// Proporciona el modelo de información detallada
  /// para interpretar una prueba realizada
  /// </summary>
  public sealed class ResumenDePrueba<T>
  {
    /// <summary>
    /// Total de tareas efectuadas
    /// </summary>
    public int Total { get; }

    /// <summary>
    /// Total de tareas correctas
    /// </summary>
    public int Correctas { get; }

    /// <summary>
    /// Total de tareas erroneas
    /// </summary>
    public int Erroneas { get; }

    /// <summary>
    /// La tarea que fué resuelta más rapido
    /// </summary>
    public Tuple<Task<T>, TimeSpan> MasRapida { get; }

    /// <summary>
    /// La tarea que tomó más tiempo en concluir
    /// </summary>
    public Tuple<Task<T>, TimeSpan> MasLenta { get; }

    /// <summary>
    /// Tiempo de respuesta promedio
    /// </summary>
    public TimeSpan PromedioDeRespuesta { get; }

    /// <summary>
    /// Metricas obtenidas al efectuar la tarea
    /// asociada
    /// </summary>
    public List<MetricaDeTarea<T>> Metricas { get; }

    public ResumenDePrueba(List<MetricaDeTarea<T>> metricas)
    {
      Metricas = metricas ?? new List<MetricaDeTarea<T>>(0);
      Total = Metricas.Count;
      Correctas = Metricas.Count(m => m.Correcto);
      Erroneas = Total - Correctas;
      PromedioDeRespuesta = Metricas.Count > 0 ? new TimeSpan(Metricas.Sum(m => m.Cronometro.Elapsed.Ticks) / Metricas.Count) : TimeSpan.MaxValue;
      MasRapida = Metricas.OrderBy(m => m.Cronometro.ElapsedTicks)
        .Select(m => new Tuple<Task<T>, TimeSpan>(m.Tarea, m.Cronometro.Elapsed))
        .FirstOrDefault();
      MasLenta = Metricas.OrderByDescending(m => m.Cronometro.ElapsedTicks)
        .Select(m => new Tuple<Task<T>, TimeSpan>(m.Tarea, m.Cronometro.Elapsed))
        .FirstOrDefault();
    }
  }
}
