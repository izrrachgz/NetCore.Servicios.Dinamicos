using Datos.Contratos;
using Datos.ProveedoresDeDatos;

namespace Negocio.ProveedoresDeDatos
{
  /// <summary>
  /// Provee el mecanismo basico para acceder a los
  /// datos de una entidad
  /// </summary>
  /// <typeparam name="T">Entidad</typeparam>
  public class ProveedorBasico<T> where T : class, IEntidad, new()
  {
    /// <summary>
    /// Datos de la entidad asociada
    /// </summary>
    protected ProveedorDeDatos<T> Datos { get; }

    public ProveedorBasico()
    {
      Datos = new ProveedorDeDatos<T>();
    }

  }
}
