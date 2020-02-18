using System.Data.SqlClient;
using Utilidades.Extensiones;
using Xunit;

namespace Utilidades.Pruebas.Hechos.Extensiones
{
  /// <summary>
  /// Pruebas positivas de conexiones sql
  /// </summary>
  public class ConexionesSql
  {
    /// <summary>
    /// Comprueba que la conexion sql
    /// no es valida para su uso
    /// </summary>
    [Fact]
    public void NoEsValida()
    {
      using (SqlConnection conexion = new SqlConnection())
      {
        conexion.Dispose();
        Assert.True(conexion.NoEsValida());
      }
    }
  }
}
