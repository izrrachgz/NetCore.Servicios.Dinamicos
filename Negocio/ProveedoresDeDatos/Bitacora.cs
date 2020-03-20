using System.Threading.Tasks;
using Contexto.Entidades;
using Contexto.ProveedoresDeDatos;
using Utilidades.Modelos;

namespace Negocio.ProveedoresDeDatos
{
  /// <summary>
  /// Provee los mecanismos para acceder a los datos
  /// de la entidad Bitacora
  /// </summary>
  public class ProveedorBitacora : ProveedorDeDatosBase<Bitacora>
  {
    /// <summary>
    /// Devuelve todos los registros de Bitacora dentro de un modelo de datos paginado
    /// </summary>
    /// <param name="pagina">Solicitud de Pagina</param>
    /// <returns>Coleccion de elementos de Bitacora</returns>
    public async Task<RespuestaColeccion<Bitacora>> Paginado(Paginado pagina)
      => await base.Obtener(pagina);

    /// <summary>
    /// Devuelve un registro de Bitacora cuyo identificador primario
    /// es igual al proporcionado
    /// </summary>
    /// <param name="id">Identificador Primario de Bitacora</param>
    /// <returns>Elemento de Bitacora</returns>
    public async Task<RespuestaModelo<Bitacora>> ObtenerPorId(long id)
      => await base.Obtener(id);    

    /// <summary>
    /// Guarda los cambios efectuados de la entidad
    /// nuevos/actualizacion
    /// </summary>
    /// <param name="modelo">Entidad</param>
    /// <returns>Respuesta basica</returns>
    public async Task<RespuestaBasica> GuardarBitacora(Bitacora modelo)
      => await base.Guardar(modelo);    
  }
}
