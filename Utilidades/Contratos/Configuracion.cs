using System.Collections.Generic;
using Utilidades.Modelos;

namespace Utilidades.Contratos
{
  /// <summary>
  /// Contrato de configuracion
  /// </summary>
  public interface IConfiguracion
  {
    /// <summary>
    /// Debera retener una coleccion de elementos de configuracion
    /// </summary>
    List<ElementoConfiguracion> Configuraciones { get; set; }

    /// <summary>
    /// Debera verificar que el contenido de configuracion
    /// es valido para su uso
    /// </summary>
    /// <returns>Verdadero o falso</returns>
    bool EsValida();
  }
}
