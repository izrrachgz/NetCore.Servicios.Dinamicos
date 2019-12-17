using System.IO;
using Servicio.Extensiones;
using Xunit;

namespace Servicio.Pruebas.Hechos.Extensiones
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
