using System.Threading.Tasks;
using Contexto.Entidades;
using Datos.Modelos;
using Microsoft.AspNetCore.Mvc;
using Negocio.ProveedoresDeDatos;

namespace Api.Controllers
{
  /// <summary>
  /// Provee un mecanismo de acceso a los datos de las entradas
  /// de log
  /// </summary>
  [ApiController, Route("Api/[controller]")]
  public class EntradasLogController : ControllerBase
  {
    /// <summary>
    /// Referencia de acceso al repositorio de datos
    /// de la entidad EntradaLog
    /// </summary>
    private ProveedorEntradaLog DatosEntradaLog { get; }

    public EntradasLogController()
    {
      DatosEntradaLog = new ProveedorEntradaLog();
    }

    /// <summary>
    /// Permite obtener todas las entradas de log utilizando
    /// un indice de pagina
    /// </summary>
    /// <param name="solicitud">Solicitud de pagina</param>
    /// <returns></returns>
    [HttpPost, Route(@"Paginado")]
    public async Task<RespuestaColeccion<EntradaLog>> Obtener(SolicitudPagina solicitud)
      => await DatosEntradaLog.Obtener(new Paginado(solicitud));
  }
}