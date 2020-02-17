using System.Threading.Tasks;
using Contexto.Entidades;
using Contexto.ProveedoresDeDatos;
using Datos.Modelos;

namespace Negocio.ProveedoresDeDatos
{
  /// <summary>
  /// Provee los mecanismos para acceder a los datos
  /// de la entidad Bitacora
  /// </summary>
  public class ProveedorBitacora : ProveedorDeDatosBase<Bitacora>
  {
    /// <summary>
    /// Devuelve las entradas de log dentro de un modelo de datos paginado
    /// </summary>
    /// <param name="pagina"></param>
    /// <returns></returns>
    public async Task<RespuestaColeccion<Bitacora>> Paginado(Paginado pagina)
      => await Obtener(pagina);
  }
}
