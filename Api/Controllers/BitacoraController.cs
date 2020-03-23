using System.Threading.Tasks;
using Contexto.Entidades;
using Utilidades.Modelos;
using Microsoft.AspNetCore.Mvc;
using Negocio.ProveedoresDeDatos;

namespace Api.Controllers
{
  /// <summary>
  /// Provee un mecanismo para exponer los puntos de entrada
  /// de negocio de Bitacora
  /// </summary>
  [ApiController, Route("Api/[controller]")]
  public sealed class BitacoraController : ControllerBase
  {
    /// <summary>
    /// Referencia de acceso hacia los datos de Bitacora via Negocio
    /// </summary>
    private ProveedorBitacora ProveedorBitacora { get; }

    /// <summary>
    /// Crea una nueva instancia del servicio de Bitacora
    /// </summary>
    public BitacoraController()
    {
      ProveedorBitacora = new ProveedorBitacora();
    }

    /// <summary>
    /// Permite obtener todos los registros de Bitacora utilizando
    /// un indice de pagina
    /// </summary>
    /// <param name="solicitud">Solicitud de pagina</param>
    /// <returns>RespuestaColeccion de Bitacora</returns>
    [HttpPost, Route(@"ObtenerPorPagina")]
    public async Task<RespuestaColeccion<Bitacora>> Obtener(SolicitudPagina solicitud)
      => await ProveedorBitacora.Obtener(new Paginado(solicitud));

    /// <summary>
    /// Permite obtener un registro de Bitacora mediante su identificador primario
    /// </summary>
    /// <param name="id">Identificador primario</param>
    /// <returns>RespuestaModelo de Bitacora</returns>
    [HttpPost, Route(@"ObtenerPorId/{id}")]
    public async Task<RespuestaModelo<Bitacora>> ObtenerPorId([FromQuery] long id)
      => await ProveedorBitacora.ObtenerPorId(id);

    /// <summary>
    /// Permite guardar un registro de bitacora ya sea nuevo o modificacion
    /// de los valores del mismo
    /// </summary>
    /// <param name="modelo">Bitacora</param>
    /// <returns>RespuestaBasica</returns>
    [HttpPost, Route(@"GuardarBitacora")]
    public async Task<RespuestaBasica> GuardarBitacora([FromBody] Bitacora modelo)
      => await ProveedorBitacora.GuardarBitacora(modelo);
  }
}