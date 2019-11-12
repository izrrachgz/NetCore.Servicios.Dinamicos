using System.Data.SqlClient;
using Servicio.Extensiones;
using Xunit;

namespace Pruebas.Hechos.Extensiones
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
