using Contexto.Esquema;
using Utilidades.Contratos;
using Utilidades.ProveedoresDeDatos;

namespace Contexto.ProveedoresDeDatos
{
  /// <summary>
  /// Provee el mecanismo basico para acceder a los
  /// datos de una entidad
  /// </summary>
  /// <typeparam name="T">Entidad</typeparam>
  public class ProveedorDeDatosBase<T> : ProveedorDeDatos<T> where T : class, IEntidad, new()
  {
    /// <summary>
    /// Referencia al repositorio de datos
    /// de las entidades asociadas
    /// </summary>
    internal Repositorio Repositorio { get; }

    public ProveedorDeDatosBase()
    {
      Repositorio = new Repositorio();
    }

    //Agrega sobrecargas o nuevas funcionalidades globales
  }
}
