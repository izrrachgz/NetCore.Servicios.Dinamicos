using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Datos.Modelos;

namespace Datos.Contratos
{
  /// <summary>
  /// Contrato de servicio
  /// </summary>
  /// <typeparam name="T">Entidad de Servicio</typeparam>
  public interface IServicio<T> where T : new()
  {
    /// <summary>
    /// Deberá indicar el nombre de la tabla asociada a la entidad su inicialización deberá ser a través del constructor
    /// </summary>
    string Tabla { get; }

    /// <summary>
    /// Tipo de la Entidad asociada al servicio
    /// </summary>
    Type Tipo { get; }

    /// <summary>
    /// Deberá crear la instrucción sql de inserción para la entidad
    /// e inicializar una lista con los parametros vacios
    /// </summary>
    /// <param name="parametros">Lista de parametros</param>
    /// <returns>Cadena SQL para insertar</returns>
    string CrearSqlInsertar(out List<SqlParameter> parametros);

    /// <summary>
    /// Deberá crear la instrucción sql de actualización para la entidad
    /// e inicializar una lista con los parametros vacios
    /// </summary>
    /// <param name="parametros">Lista de parametros</param>
    /// <returns>Cadena SQL para insertar</returns>
    string CrearSqlActualizar(out List<SqlParameter> parametros);

    /// <summary>
    /// Deberá regresar la entidad encontrada dentro del contexto de datos
    /// </summary>
    /// <param name="id">Identificador primario de la entidad</param>
    /// <returns>Entidad</returns>
    Task<RespuestaModelo<T>> Obtener(int id);

    /// <summary>
    /// Deberá regresar una entidades de entidades dentro de un modelo de datos paginado
    /// </summary>
    /// <param name="paginado">Solicitud de página</param>
    /// <param name="condicion">Condiciones para obtener las entidades</param>
    /// <returns>Lista de Entidades</returns>
    Task<RespuestaColeccion<T>> Obtener(Paginado paginado, List<Condicion> condicion = null);

    /// <summary>
    /// Deberá regresar una lista de clave/valor asociadas a la entidad
    /// </summary>
    /// <param name="columna">Nombre de la columna para seleccionar</param>
    /// <returns>Lista de clave/valor asociados a la Entidad</returns>
    Task<RespuestaColeccion<ClaveValor>> Obtener(string columna);

    /// <summary>
    /// Deberá regresar una lista de entidades asociadas al servicio conteniendo
    /// solo las columnas indicadas
    /// </summary>
    /// <param name="columnas">Nombre de las columnas para seleccionar</param>
    /// <param name="paginado">Solicitud de página</param>
    /// <param name="condicion">Condiciones para obtener las entidades</param>
    /// <returns>Lista de entidades asociadas al servicio</returns>
    Task<RespuestaColeccion<T>> Obtener(string[] columnas, Paginado paginado, List<Condicion> condicion = null);

    /// <summary>
    /// Deberá Insertar o actualizar el modelo en el contexto de datos
    /// </summary>
    /// <param name="modelo">Entidad para guardar o actualizar</param>
    /// <returns>Verdadero o falso</returns>
    Task<RespuestaBasica> Guardar(T modelo);

    /// <summary>
    /// Deberá Insertar o actualizar los modelos en el contexto de datos
    /// </summary>
    /// <param name="entidades">Entidades para guardar o actualizar</param>
    /// <returns>Verdadero o falso</returns>
    Task<RespuestaColeccion<int>> Guardar(List<T> entidades);

    /// <summary>
    /// Deberá actualizar la fecha de eliminado en el modelo encontrado
    /// </summary>
    /// <param name="id">Identificador primario de modelo</param>
    /// <returns>Verdadero o falso</returns>
    Task<RespuestaBasica> Eliminar(int id);

    /// <summary>
    /// Deberá actualizar la fecha de eliminado en los modelos encontrados
    /// </summary>
    /// <param name="ids">Identificador primario de modelo</param>
    /// <returns>Verdadero o falso</returns>
    Task<RespuestaBasica> Eliminar(List<int> ids);

    /// <summary>
    /// Deberá insertar el elemento en el contexto de datos y devolver su identificador primario
    /// </summary>
    /// <param name="modelo">Elemento a insertar</param>
    /// <returns>Identificador primario</returns>
    Task<RespuestaModelo<int>> Insertar(T modelo);

    /// <summary>
    /// Deberá insertar el elemento en el contexto de datos y devolver su identificador primario
    /// </summary>
    /// <param name="modelo">Elemento a insertar</param>
    /// <param name="transaccion">Transacción abierta asociada a la conexión vigente</param>
    /// <returns>Identificador primario</returns>
    Task<RespuestaModelo<int>> Insertar(T modelo, SqlTransaction transaccion);

    /// <summary>
    /// Deberá insertar todos los elementos en el Contexto de datos y devolver sus identificadores primarios
    /// </summary>
    /// <param name="entidades">Elementos a insertar</param>
    /// <returns>Arreglo de itentificadores primarios de modelo</returns>
    Task<RespuestaColeccion<int>> Insertar(List<T> entidades);

    /// <summary>
    /// Deberá Insertar todos los elementos en el Contexto de datos y devolver sus identificadores primarios
    /// </summary>
    /// <param name="entidades">Elementos a insertar</param>
    /// <param name="transaccion">Transacción abierta asociada a la conexión vigente</param>
    /// <returns>Arreglo de identificadores primarios de modelo</returns>
    Task<RespuestaColeccion<int>> Insertar(List<T> entidades, SqlTransaction transaccion);

    /// <summary>
    /// Deberá actualizar el elemento en el contexto de datos y devolver la cantidad de registros afectados
    /// </summary>
    /// <param name="modelo">Elemento a actualizar</param>
    /// <returns>Cantidad de filas afectadas</returns>
    Task<RespuestaModelo<int>> Actualizar(T modelo);

    /// <summary>
    /// Deberá actualizar el elemento en el contexto de datos y devolver la cantidad de registros afectados
    /// </summary>
    /// <param name="modelo">Elemento a actualizar</param>
    /// <param name="transaccion">Transacción abierta asociada a la conexión vigente</param>
    /// <returns>Cantidad de filas afectadas</returns>
    Task<RespuestaModelo<int>> Actualizar(T modelo, SqlTransaction transaccion);

    /// <summary>
    /// Deberá actualizar todos los elementos en el Contexto de datos y devolver la cantidad de registros afectados
    /// </summary>
    /// <param name="entidades">Elementos a actualizar</param>
    /// <returns>Cantidad de filas afectadas</returns>
    Task<RespuestaModelo<int>> Actualizar(List<T> entidades);

    /// <summary>
    /// Deberá actualizar todos los elementos en el Contexto de datos y devolver la cantidad de registros afectados
    /// </summary>
    /// <param name="entidades">Elementos a actualizar</param>
    /// <param name="transaccion">Transacción abierta asociada a la conexión vigente</param>
    /// <returns>Cantidad de filas afectadas</returns>
    Task<RespuestaModelo<int>> Actualizar(List<T> entidades, SqlTransaction transaccion);
  }
}
