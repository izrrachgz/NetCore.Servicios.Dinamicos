using System.Threading.Tasks;
using Utilidades.Comandos;
using Utilidades.Modelos;
using Xunit;

namespace Utilidades.Pruebas.Hechos.Comandos
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
