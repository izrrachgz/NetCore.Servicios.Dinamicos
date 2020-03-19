using System.Collections.Generic;
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
    /// Devuelve todos los registros de Bitacora dentro de un modelo de datos paginado
    /// </summary>
    /// <param name="paginado">Solicitud de Pagina</param>
    /// <param name="condicion">Condiciones de busqueda</param>
    /// <returns>Coleccion de elementos de Bitacora</returns>
    public async Task<RespuestaColeccion<Bitacora>> Paginado(Paginado paginado, List<Condicion> condicion)
      => await base.Obtener(paginado, condicion);

    /// <summary>
    /// Devuelve todos los registros de Bitacora incluyendo solo la columna
    /// indicada
    /// </summary>
    /// <param name="columna">Nombre de la columna</param>
    /// <returns>Coleccion de elementos</returns>
    public async Task<RespuestaColeccion<IndiceValor>> ObtenerSoloUnaColumna(string columna)
      => await base.Obtener(columna);

    /// <summary>
    /// Devuelve todos los registros de Bitacora dentro de un modelo de datos paginado
    /// incluyrendo solo las columnas indicadas
    /// </summary>
    /// <param name="columnas">Nombres de columnas</param>
    /// <param name="paginado">Solicitud de pagina</param>
    /// <param name="condicion">Condiciones de busqueda</param>
    /// <returns>Coleccion de elementos de Bitacora</returns>
    public async Task<RespuestaColeccion<Bitacora>> PaginadoPorColumnas(string[] columnas, Paginado paginado, List<Condicion> condicion = null)
      => await base.Obtener(columnas, paginado, condicion);

    /// <summary>
    /// Guarda los cambios efectuados de la entidad
    /// nuevos/actualizacion
    /// </summary>
    /// <param name="modelo">Entidad</param>
    /// <returns>Respuesta basica</returns>
    public async Task<RespuestaBasica> GuardarUno(Bitacora modelo)
      => await base.Guardar(modelo);

    /// <summary>
    /// Guarda los cambios efectuados de las entidades
    /// nuevos/actualizacion
    /// </summary>
    /// <param name="entidades">Entidades</param>
    /// <returns>Coleccion con identificadores primarios</returns>
    public async Task<RespuestaColeccion<long>> GuardarListado(List<Bitacora> entidades)
      => await base.Guardar(entidades);

    /// <summary>
    /// Guarda los valores de una entidad asumiendo
    /// que es un registro nuevo
    /// </summary>
    /// <param name="modelo">Entidad</param>
    /// <returns>Identificador primario de la entidad insertada</returns>
    public async Task<RespuestaModelo<long>> InsertarUno(Bitacora modelo)
      => await base.Insertar(modelo);

    /// <summary>
    /// Guarda los valores de las entidades asumiendo
    /// que son registros nuevos
    /// </summary>
    /// <param name="entidades">Entidades</param>
    /// <returns>Coleccion de identificadores primarios de las entidades insertadas</returns>
    public async Task<RespuestaColeccion<long>> InsertarListado(List<Bitacora> entidades)
      => await base.Insertar(entidades);

    /// <summary>
    /// Guarda los valores de una entidad asumiendo
    /// que es un registro existente
    /// </summary>
    /// <param name="modelo">Entidad</param>
    /// <returns>Cantidad de filas afectadas</returns>
    public async Task<RespuestaModelo<long>> ActualizarUno(Bitacora modelo)
      => await base.Actualizar(modelo);

    /// <summary>
    /// Guarda los valores de las entidades asumiendo
    /// que son registros existentes
    /// </summary>
    /// <param name="entidades">Entidades</param>
    /// <returns>Cantidad de filas afectadas</returns>
    public async Task<RespuestaModelo<long>> ActualizarListado(List<Bitacora> entidades)
      => await base.Actualizar(entidades);
  }
}
