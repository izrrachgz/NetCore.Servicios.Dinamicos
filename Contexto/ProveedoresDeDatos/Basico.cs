using Datos.Contratos;
using Datos.ProveedoresDeDatos;

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
    internal Repositorio.Repositorio Repositorio { get; }

    public ProveedorDeDatosBase()
    {
      Repositorio = new Repositorio.Repositorio();
    }

    //Crea tus sobrecargas o nuevas funcionalidades
  }
}
