using System.Net;
using System.Net.Http;
using Servicio.Extensiones;
using Xunit;

namespace Servicio.Pruebas.Hechos.Extensiones
{
  public class Http
  {
    [Fact]
    public void NoEsValida()
    {
      HttpResponseMessage http = new HttpResponseMessage(HttpStatusCode.InternalServerError);
      Assert.True(http.NoEsValida());
    }
  }
}
