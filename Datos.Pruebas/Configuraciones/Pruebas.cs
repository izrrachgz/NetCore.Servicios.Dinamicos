using Datos.Configuraciones;
using Datos.Extensiones;

namespace Datos.Pruebas.Configuraciones
{
  /// <summary>
  /// Provee un modelo de datos para representar
  /// la configuracion que se asocia al contexto
  /// de pruebas de negocio 
  /// </summary>
  internal sealed class ConfiguracionDatosPruebas : ConfiguracionBase
  {
    /// <summary>
    /// Direccion base del sitio para efectuar
    /// solicitudes bin
    /// </summary>
    public string UrlBaseBin { get; set; }

    /// <summary>
    /// Metodo o parte del recurso bin
    /// </summary>
    public string MetodoBin { get; set; }

    /// <summary>
    /// Se asegura que al cargar los valores de configuracion
    /// la url y metodo para efectuar solicitudes bin sean
    /// validos para su uso
    /// </summary>
    /// <returns>Verdadero o falso</returns>
    public override bool EsValida()
    {
      return !UrlBaseBin.NoEsValida() && !MetodoBin.NoEsValida();
    }
  }
}
