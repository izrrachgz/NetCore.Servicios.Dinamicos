using System.Data.SqlClient;
using Datos.Extensiones;
using Xunit;

namespace Datos.Pruebas.Hechos.Extensiones
{
  public class ConexionesSql
  {
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
