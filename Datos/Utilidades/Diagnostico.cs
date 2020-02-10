using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Datos.Modelos;
using ThreadState = System.Threading.ThreadState;

namespace Datos.Utilidades
{
  /// <summary>
  /// Proporciona el mecanismo para realizar una prueba de carga
  /// y estres sobre un listado de tareas
  /// </summary>
  public class DiagnosticarRendimiento<T>
  {
    /// <summary>
    /// Marcador utilizado para medir el tiempo
    /// de respuesta de la tarea de manera global
    /// </summary>
    public Stopwatch Cronometro { get; }

    /// <summary>
    /// Cantidad de iteraciones que se deben efectuar con la lista de tareas
    /// dada
    /// </summary>
    public int Ciclos { get; }

    /// <summary>
    /// Cantidad de hilos que se deben crear para efectuar cada ejecucion de lista
    /// de tareas dada de manera aislada
    /// </summary>
    public byte Hilos { get; }

    /// <summary>
    /// Lista de tareas que se deben realizar
    /// </summary>
    public List<Task<T>> Tareas { get; }

    public DiagnosticarRendimiento(List<Task<T>> tareas, int ciclos = 1, byte hilos = 1)
    {
      Cronometro = new Stopwatch();
      Tareas = tareas ?? new List<Task<T>>(0);
      Ciclos = ciclos;
      Hilos = hilos;
    }

    /// <summary>
    /// Inicia la ejecución de la prueba
    /// </summary>
    /// <returns>Resumen de prueba</returns>
    public async Task<ResumenDeDiagnostico<T>> Ejecutar()
    {
      return await Task.Run(() =>
      {
        List<MetricaDeTarea<T>> resultados = new List<MetricaDeTarea<T>>(Hilos * Ciclos * Tareas.Count);
        List<Thread> hilos = new List<Thread>(Hilos);
        Cronometro.Start();
        for (int i = 0; i < Hilos; i++)
        {
          Thread h = new Thread(() =>
          {
            for (int x = 0; x < Ciclos; x++)
            {
              MetricaDeTarea<T> metrica = new MetricaDeTarea<T>(Tareas.ElementAt(x));
              metrica.CalcularMetrica();
              resultados.Add(metrica);
            }
          })
          { IsBackground = true };
          h.Start();
          hilos.Add(h);
        }
        do
        {
          Console.Write(@".");
        } while (!hilos.TrueForAll(h => h.ThreadState.Equals(ThreadState.Stopped)));
        Cronometro.Stop();
        return new ResumenDeDiagnostico<T>(resultados);
      });
    }
  }
}
