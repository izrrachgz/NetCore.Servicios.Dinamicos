using Servicio.Extensiones;
using Xunit;

namespace Pruebas.Hechos
{
  public class Extensiones
  {
    [Fact]
    public void NoEsValida()
    {
      Assert.True(@"".NoEsValida());
    }

    [Fact]
    public void EsNumero()
    {
      Assert.True(@"1".EsNumero());
    }

    [Fact]
    public void EsCelular()
    {
      Assert.True(@"66235595665".EsCelular());
    }

    [Fact]
    public void EsCorreo()
    {
      Assert.True(@"izrra.ch@icloud.com".EsCorreo());
    }
  }
}
