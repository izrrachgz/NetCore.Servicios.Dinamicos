namespace Datos.Mensajes
{
  /// <summary>
  /// Provee mensajes de acciones y operaciones efectuadas
  /// incorrectamente
  /// </summary>
  public static class Error
  {
    /// <summary>
    /// Usado cuando el id/clave no es válido
    /// </summary>
    public const string IdentificadorInvalido = @"El identificador no es válido.";

    /// <summary>
    /// Usado cuando la solicitud de pagina no es válida
    /// </summary>
    public const string PaginadoNoEsValido = @"El modelo de paginado no es válido.";

    /// <summary>
    /// Usado cuando las columnas no son válidas para la entidad de servicio
    /// </summary>
    public const string ColumnasDeBusquedaNoCoinciden = @"Las columnas de búsqueda porporcionadas no coinciden con las de la entidad.";

    /// <summary>
    /// Usado cuando ha ocurrido un error desconocido.
    /// </summary>
    public const string ErrorInterno = @"Ha ocurrido un error interno.";

    /// <summary>
    /// Usado cuando el modelo no es válido
    /// </summary>
    public const string ModeloInvalido = @"La Entidad proporcionada o su contenido no es válido para realizar la operación.";

    /// <summary>
    /// Usado cuando la lista no es válida
    /// </summary>
    public const string ListaInvalida = @"El listado proporcionado o su contenido no es válido para realizar la operación.";

    /// <summary>
    /// Usado cuando la diferencia de elementos afectados y la cantidad final
    /// no son iguales a la esperada
    /// </summary>
    public const string DiferenciaDeElementosAfectados = @"El número de elementos esperado no coincide con el devuelto, se han revertido los cambios.";
  }
}
