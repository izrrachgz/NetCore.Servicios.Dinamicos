using System.Threading.Tasks;
using Contexto.Entidades;
using Datos.Modelos;
using Microsoft.AspNetCore.Mvc;
using Negocio.ProveedoresDeDatos;

namespace Api.Controllers
{
  /// <summary>
  /// Provee un mecanismo de acceso a los datos de los usuarios
  /// </summary>
  [ApiController, Route("Api/[controller]")]
  public class UsuariosController : ControllerBase
  {
    /// <summary>
    /// Referencia de acceso al repositorio de datos
    /// de la entidad de usuario
    /// </summary>
    private ProveedorUsuario DatosUsuario { get; }

    public UsuariosController()
    {
      DatosUsuario = new ProveedorUsuario();
    }

    /// <summary>
    /// Permite obtener todos los usuarios utilizando
    /// un indice de pagina
    /// </summary>
    /// <param name="solicitud">Solicitud de pagina</param>
    /// <returns></returns>
    [HttpPost, Route(@"Paginado")]
    public async Task<RespuestaColeccion<Usuario>> Obtener(SolicitudPagina solicitud)
      => await DatosUsuario.Usuarios(new Paginado(solicitud ?? new SolicitudPagina()));

    /// <summary>
    /// Permite obtener todos los usuarios que no tienen un numero
    /// de celular
    /// </summary>
    /// <param name="solicitud"></param>
    /// <returns></returns>
    [HttpPost, Route(@"SinCelular")]
    public async Task<RespuestaColeccion<Usuario>> UsuariosSinCelular(SolicitudPagina solicitud)
      => await DatosUsuario.UsuariosSinCelular(new Paginado(solicitud ?? new SolicitudPagina()));
  }
}