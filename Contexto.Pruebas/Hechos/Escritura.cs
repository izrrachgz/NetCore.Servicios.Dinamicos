using System.Threading.Tasks;
using Contexto.Entidades;
using Contexto.Enumerados;
using Datos.ProveedoresDeDatos;
using Datos.Modelos;
using Xunit;

namespace Contexto.Pruebas.Hechos
{
  /// <summary>
  /// Pruebas positivas de escritura de datos
  /// </summary>
  public class Escritura
  {
    public EntradaLog Entrada { get; }

    public Escritura()
    {
      Entrada = new EntradaLog()
      {
        Nombre = @"Death Note",
        Descripcion = @"6:40",
        Tipo = EntradaLogTipo.Advertencia
      };
    }

    /// <summary>
    /// Comprueba que se guarde una entrada de log
    /// en el repositorio de datos utilizando
    /// el mecanismo de proveedor de datos
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GuardarEntrada()
    {
      ProveedorDeDatos<EntradaLog> servicio = new ProveedorDeDatos<EntradaLog>();
      RespuestaBasica guardado = await servicio.Guardar(Entrada);
      Assert.True(guardado.Correcto);
    }
  }
}
