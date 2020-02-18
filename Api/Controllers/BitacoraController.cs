using System.Threading.Tasks;
using Contexto.Entidades;
using Utilidades.Modelos;
using Microsoft.AspNetCore.Mvc;
using Negocio.ProveedoresDeDatos;

namespace Api.Controllers
{
  /// <summary>
  /// Provee un mecanismo de acceso a los datos de las entradas
  /// de log
  /// </summary>
  [ApiController, Route("Api/[controller]")]
  public class BitacoraController : ControllerBase
  {
    /// <summary>
    /// Referencia de acceso al repositorio de datos
    /// de la entidad EntradaLog
    /// </summary>
    private ProveedorBitacora ProveedorBitacora { get; }

    public BitacoraController()
    {
      ProveedorBitacora = new ProveedorBitacora();
    }

    /// <summary>
    /// Permite obtener todas las entradas de log utilizando
    /// un indice de pagina
    /// </summary>
    /// <param name="solicitud">Solicitud de pagina</param>
    /// <returns></returns>
    [HttpPost, Route(@"Paginado")]
    public async Task<RespuestaColeccion<Bitacora>> Obtener(SolicitudPagina solicitud)
      => await ProveedorBitacora.Obtener(new Paginado(solicitud));
  }
}