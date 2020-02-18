using Utilidades.Configuraciones;

namespace Consola.Configuraciones
{
  /// <summary>
  /// Provee un modelo de datos para representar
  /// la configuracion que se asocia al contexto
  /// de la aplicacion de consola
  /// </summary>
  internal sealed class ConfiguracionConsola : ConfiguracionBase
  {
    /// <summary>
    /// Indica si hay que esperar a que el usuario
    /// confirme la salida del programa al concluir
    /// la tarea
    /// </summary>
    public bool SolicitarConfirmacionDeSalida { get; set; }
  }
}
