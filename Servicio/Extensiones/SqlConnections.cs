using System.Data;
using System.Data.SqlClient;

namespace Servicio.Extensiones
{
  /// <summary>
  /// Provee metodos de extensión para conexines sql
  /// </summary>
  public static class SqlConnectionsExtensions
  {
    /// <summary>
    /// Indica si una conexión es nula o se encuentra abierta para su uso
    /// </summary>
    /// <param name="conexion">Conexión para comprobar</param>
    /// <returns>Verdadero o falso</returns>
    public static bool NoEsValida(this SqlConnection conexion)
    {
      return conexion == null || !conexion.State.Equals(ConnectionState.Open);
    }
  }
}