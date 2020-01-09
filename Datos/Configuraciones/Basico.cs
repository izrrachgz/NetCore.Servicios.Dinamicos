using System.Collections.Generic;
using Datos.Contratos;
using Datos.Modelos;

namespace Datos.Configuraciones
{
  /// <summary>
  /// Provee un modelo de datos y funcionalidad
  /// para representar la operacion de un
  /// archivo de configuracion
  /// </summary>
  public class ConfiguracionBase : IConfiguracion
  {
    /// <summary>
    /// Coleccion de elementos de configuracion
    /// </summary>
    public List<ElementoConfiguracion> Configuraciones { get; set; }

    /// <summary>
    /// Verifica que el contenido de la configuracion
    /// sea valido para su uso
    /// </summary>
    /// <returns>Verdadero o falso</returns>
    public virtual bool EsValida() => true;
  }
}
