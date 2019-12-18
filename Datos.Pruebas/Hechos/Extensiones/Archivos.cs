using System.IO;
using Datos.Extensiones;
using Xunit;

namespace Datos.Pruebas.Hechos.Extensiones
{
  public class Archivos
  {
    [Fact]
    public void NoEsValido()
    {
      FileInfo info = new FileInfo(@"c:\vicky.jpg");
      Assert.True(info.NoEsValido());
    }
  }
}
