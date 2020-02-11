using System.Threading.Tasks;
using Contexto.Entidades;
using Contexto.ProveedoresDeDatos;
using Datos.Modelos;

namespace Negocio.ProveedoresDeDatos
{
  /// <summary>
  /// Provee los mecanismos para acceder a los datos
  /// de la entidad EntradaLog
  /// </summary>
  public class ProveedorEntradaLog : ProveedorDeDatosBase<EntradaLog>
  {
    /// <summary>
    /// Devuelve las entradas de log dentro de un modelo de datos paginado
    /// </summary>
    /// <param name="pagina"></param>
    /// <returns></returns>
    public async Task<RespuestaColeccion<EntradaLog>> Paginado(Paginado pagina)
      => await Obtener(pagina);
  }
}
