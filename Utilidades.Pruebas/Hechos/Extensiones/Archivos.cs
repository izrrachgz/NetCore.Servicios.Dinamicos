using System.IO;
using Utilidades.Extensiones;
using Xunit;

namespace Utilidades.Pruebas.Hechos.Extensiones
{
  /// <summary>
  /// Pruebas positivas de archivos
  /// </summary>
  public class Archivos
  {
    /// <summary>
    /// Comprueba que el archivo en la direccion no existe
    /// </summary>
    [Fact]
    public void NoEsValido()
    {
      FileInfo info = new FileInfo(@"c:\vicky.jpg");
      Assert.True(info.NoEsValido());
    }
  }
}
