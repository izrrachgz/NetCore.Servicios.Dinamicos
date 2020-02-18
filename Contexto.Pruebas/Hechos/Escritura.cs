using System.Threading.Tasks;
using Contexto.Entidades;
using Contexto.Enumerados;
using Utilidades.ProveedoresDeDatos;
using Utilidades.Modelos;
using Xunit;

namespace Contexto.Pruebas.Hechos
{
  /// <summary>
  /// Pruebas positivas de escritura de datos
  /// </summary>
  public class Escritura
  {
    public Bitacora Entrada { get; }

    public Escritura()
    {
      Entrada = new Bitacora()
      {
        Nombre = @"Death Note",
        Descripcion = @"6:40",
        Tipo = BitacoraTipo.Advertencia
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
      ProveedorDeDatos<Bitacora> servicio = new ProveedorDeDatos<Bitacora>();
      RespuestaBasica guardado = await servicio.Guardar(Entrada);
      Assert.True(guardado.Correcto);
    }
  }
}
