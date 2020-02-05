using System;
using System.Threading.Tasks;
using Consola.Configuraciones;
using Datos.Configuraciones;

namespace Consola
{
  /// <summary>
  /// Provee el mecanismo para efectuar una tarea en concreto
  /// </summary>
  class Program
  {
    /// <summary>
    /// Punto de entrada del programa de aplicacion
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {
      try
      {
        Console.Write(@"Inicio del proceso");
        //Esperar a que termine el proceso
        EjecutarTareaPrincipal()
          .Wait();
        //Confirmar la salida si lo indica la configuracion
        if (Configuracion<ConfiguracionConsola>.Instancia.SolicitarConfirmacionDeSalida)
        {
          Console.Write(@"Ha concluido el proceso, presiona cualquier tecla para cerrar esta ventana.");
          Console.ReadKey();
        }
        else
        {
          Console.Write(@"Termina el proceso");
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
      }
    }

    /// <summary>
    /// Punto de entrada principal para la tarea
    /// </summary>
    /// <returns></returns>
    private static async Task EjecutarTareaPrincipal()
    {
      await Task.Run(() => Console.Write(@"Tarea terminada!"));
    }
  }
}
