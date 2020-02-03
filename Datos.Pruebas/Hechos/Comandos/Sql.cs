using System.Threading.Tasks;
using Datos.Comandos;
using Datos.Modelos;
using Xunit;

namespace Datos.Pruebas.Hechos.Comandos
{
  public class ComandosSql
  {
    /// <summary>
    /// Comprueba que el metodo Consulta de la utilidad
    /// de comandos puede ejecutar comandos sql
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Consulta()
    {
      string sql = @"SELECT 1 as Total";
      Comando comando = new Comando();
      RespuestaColeccion<FilaDeTabla> resultado = await comando.Consulta(sql);
      Assert.True(resultado.Correcto);
    }
  }
}
