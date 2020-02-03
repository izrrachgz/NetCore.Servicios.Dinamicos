using System.Net.Http;
using System.Threading.Tasks;
using Datos.Modelos;
using Negocio.Utilidades;
using Xunit;

namespace Negocio.Pruebas.Hechos.Utilidades
{
  /// <summary>
  /// Pruebas positivas sobre la utilidad http
  /// </summary>
  public class UtilidadHttp
  {
    /// <summary>
    /// Comprueba que el metodo POST de la utilidad http
    /// puede enviar informacion al recurso especificado
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Post()
    {
      string url = @"http://requestbin.net/";
      string metodo = @"r/ukxpn7uk";
      SolicitudHttp http = new SolicitudHttp();
      HttpResponseMessage mensaje = await http.Post(url, metodo, @"{hola:'adios'}");
      Assert.True(mensaje.IsSuccessStatusCode);
    }

    /// <summary>
    /// Comprueba que el metodo GET de la utilidad http
    /// puede acceder al recurso especificado
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Get()
    {
      string url = @"https://www.google.com";
      string metodo = @"/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png";
      SolicitudHttp http = new SolicitudHttp();
      HttpResponseMessage contenido = await http.Get(url, metodo);
      Assert.True(contenido.IsSuccessStatusCode);
    }

    /// <summary>
    /// Comprueba que el metodo Descargar de la utilidad http
    /// puede descargar un archivo dada una direccion
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Descargar()
    {
      string url = @"https://www.google.com";
      string metodo = @"/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png";
      SolicitudHttp http = new SolicitudHttp();
      RespuestaModelo<byte[]> contenido = await http.Descargar(url, metodo);
      Assert.True(contenido.Correcto);
    }
  }
}
