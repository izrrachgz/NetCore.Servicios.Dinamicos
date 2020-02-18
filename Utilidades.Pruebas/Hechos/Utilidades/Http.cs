using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Utilidades.Configuraciones;
using Utilidades.Herramientas;
using Utilidades.Modelos;
using Utilidades.Pruebas.Configuraciones;
using Xunit;

namespace Utilidades.Pruebas.Hechos.Utilidades
{
  /// <summary>
  /// Pruebas positivas sobre la utilidad http
  /// </summary>
  public class UtilidadHttp
  {
    /// <summary>
    /// Direccion base del recurso bin
    /// </summary>
    private string UrlBaseBin { get; }

    /// <summary>
    /// Direccion del recurso bin para identificar la solicitud
    /// </summary>
    private string MetodoBin { get; }

    public UtilidadHttp()
    {
      UrlBaseBin = Configuracion<ConfiguracionUtilidadesPruebas>.Instancia.UrlBaseBin;
      MetodoBin = Configuracion<ConfiguracionUtilidadesPruebas>.Instancia.MetodoBin;
    }

    /// <summary>
    /// Comprueba que el metodo POST de la utilidad http
    /// puede enviar informacion al recurso especificado
    /// codificada en un objeto json
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task PostJson()
    {
      SolicitudHttp http = new SolicitudHttp();
      HttpResponseMessage mensaje = await http.Post(UrlBaseBin, MetodoBin, @"{hola:'adios'}");
      Assert.True(mensaje.IsSuccessStatusCode);
    }

    /// <summary>
    /// Comprueba que el metodo POST de la utilidad http
    /// puede enviar un objeto buffer al recurso especificado
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task PostBuffer()
    {
      SolicitudHttp http = new SolicitudHttp();
      byte[] contenido = new byte[3] { 1, 2, 3 };
      HttpResponseMessage mensaje = await http.Post(UrlBaseBin, MetodoBin, contenido);
      Assert.True(mensaje.IsSuccessStatusCode);
    }

    /// <summary>
    /// Comprueba que el metodo POST de la utilidad http
    /// puede enviar un objeto Diccionario al recurso especificado
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task PostForm()
    {
      SolicitudHttp http = new SolicitudHttp();
      Dictionary<string, string> contenido = new Dictionary<string, string>(1)
      {
        {@"hola",@"adios"}
      };
      HttpResponseMessage mensaje = await http.Post(UrlBaseBin, MetodoBin, contenido);
      Assert.True(mensaje.IsSuccessStatusCode);
    }

    /// <summary>
    /// Comprueba que el metodo POST de la utilidad http
    /// puede enviar un objeto Stream al recurso especificado
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task PostStream()
    {
      SolicitudHttp http = new SolicitudHttp();
      Stream contenido = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + @"ConfiguracionNegocio.json");
      HttpResponseMessage mensaje = await http.Post(UrlBaseBin, MetodoBin, contenido);
      Assert.True(mensaje.IsSuccessStatusCode);
    }

    /// <summary>
    /// Comprueba que el metodo PUT de la utilidad http
    /// puede enviar informacion al recurso especificado
    /// codificado en un objeto json
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task PutJson()
    {
      SolicitudHttp http = new SolicitudHttp();
      HttpResponseMessage mensaje = await http.Put(UrlBaseBin, MetodoBin, @"{hola:'adios'}");
      Assert.True(mensaje.IsSuccessStatusCode);
    }

    /// <summary>
    /// Comprueba que el metodo PUT de la utilidad http
    /// puede enviar un objeto buffer al recurso especificado
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task PutBuffer()
    {
      SolicitudHttp http = new SolicitudHttp();
      byte[] contenido = new byte[3] { 1, 2, 3 };
      HttpResponseMessage mensaje = await http.Put(UrlBaseBin, MetodoBin, contenido);
      Assert.True(mensaje.IsSuccessStatusCode);
    }

    /// <summary>
    /// Comprueba que el metodo PUT de la utilidad http
    /// puede enviar un objeto Diccionario al recurso especificado
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task PutForm()
    {
      SolicitudHttp http = new SolicitudHttp();
      Dictionary<string, string> contenido = new Dictionary<string, string>(1)
      {
        {@"hola",@"adios"}
      };
      HttpResponseMessage mensaje = await http.Put(UrlBaseBin, MetodoBin, contenido);
      Assert.True(mensaje.IsSuccessStatusCode);
    }

    /// <summary>
    /// Comprueba que el metodo PUT de la utilidad http
    /// puede enviar un objeto Stream al recurso especificado
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task PutStream()
    {
      SolicitudHttp http = new SolicitudHttp();
      Stream contenido = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + @"ConfiguracionNegocio.json");
      HttpResponseMessage mensaje = await http.Put(UrlBaseBin, MetodoBin, contenido);
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
      SolicitudHttp http = new SolicitudHttp();
      HttpResponseMessage contenido = await http.Get(UrlBaseBin, MetodoBin);
      Assert.True(contenido.IsSuccessStatusCode);
    }

    /// <summary>
    /// Comprueba que el metodo GET de la utilidad http
    /// puede acceder al recurso especificado utilizando
    /// condiciones via url
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetQuery()
    {
      SolicitudHttp http = new SolicitudHttp();
      string parametros = @"hola=adios";
      HttpResponseMessage contenido = await http.Get(UrlBaseBin, MetodoBin, parametros);
      Assert.True(contenido.IsSuccessStatusCode);
    }

    /// <summary>
    /// Comprueba que el metodo GET de la utilidad http
    /// puede acceder al recurso especificado utilizando
    /// condiciones via url
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetKeys()
    {
      SolicitudHttp http = new SolicitudHttp();
      KeyValuePair<string, string> parametros = new KeyValuePair<string, string>(@"hola", @"adios");
      HttpResponseMessage contenido = await http.Get(UrlBaseBin, MetodoBin, parametros);
      Assert.True(contenido.IsSuccessStatusCode);
    }

    /// <summary>
    /// Comprueba que el metodo GET de la utilidad http
    /// puede acceder al recurso especificado utilizando
    /// condiciones via url
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetDictionary()
    {
      SolicitudHttp http = new SolicitudHttp();
      Dictionary<string, dynamic> parametros = new Dictionary<string, dynamic>(1)
      {
        {@"hola",@"adios"}
      };
      HttpResponseMessage contenido = await http.Get(UrlBaseBin, MetodoBin, parametros);
      Assert.True(contenido.IsSuccessStatusCode);
    }

    /// <summary>
    /// Comprueba que el metodo GET de la utilidad http
    /// puede acceder al recurso especificado utilizando
    /// condiciones via url
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetKeyList()
    {
      SolicitudHttp http = new SolicitudHttp();
      List<KeyValuePair<string, string>> parametros = new List<KeyValuePair<string, string>>(1)
      {
        new KeyValuePair<string, string>(@"hola",@"adios")
      };
      HttpResponseMessage contenido = await http.Get(UrlBaseBin, MetodoBin, parametros);
      Assert.True(contenido.IsSuccessStatusCode);
    }

    /// <summary>
    /// Comprueba que el metodo DELETE de la utilidad http
    /// puede acceder al recurso especificado y eliminarlo
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Delete()
    {
      SolicitudHttp http = new SolicitudHttp();
      HttpResponseMessage contenido = await http.Delete(UrlBaseBin, MetodoBin);
      Assert.True(contenido.IsSuccessStatusCode);
    }

    /// <summary>
    /// Comprueba que el metodo DELETE de la utilidad http
    /// puede acceder al recurso especificado y eliminarlo
    /// utilizando condiciones via url
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task DeleteQuery()
    {
      SolicitudHttp http = new SolicitudHttp();
      string parametros = @"hola=adios";
      HttpResponseMessage contenido = await http.Delete(UrlBaseBin, MetodoBin, parametros);
      Assert.True(contenido.IsSuccessStatusCode);
    }

    /// <summary>
    /// Comprueba que el metodo DELETE de la utilidad http
    /// puede acceder al recurso especificado y eliminarlo
    /// utilizando condiciones via url
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task DeleteKeys()
    {
      SolicitudHttp http = new SolicitudHttp();
      KeyValuePair<string, string> parametros = new KeyValuePair<string, string>(@"hola", @"adios");
      HttpResponseMessage contenido = await http.Delete(UrlBaseBin, MetodoBin, parametros);
      Assert.True(contenido.IsSuccessStatusCode);
    }

    /// <summary>
    /// Comprueba que el metodo DELETE de la utilidad http
    /// puede acceder al recurso especificado y eliminarlo
    /// utilizando condiciones via url
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task DeleteDictionary()
    {
      SolicitudHttp http = new SolicitudHttp();
      Dictionary<string, dynamic> parametros = new Dictionary<string, dynamic>(1)
      {
        {@"hola",@"adios"}
      };
      HttpResponseMessage contenido = await http.Delete(UrlBaseBin, MetodoBin, parametros);
      Assert.True(contenido.IsSuccessStatusCode);
    }

    /// <summary>
    /// Comprueba que el metodo DELETE de la utilidad http
    /// puede acceder al recurso especificado y eliminarlo
    /// utilizando condiciones via url
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task DeleteKeyList()
    {
      SolicitudHttp http = new SolicitudHttp();
      List<KeyValuePair<string, string>> parametros = new List<KeyValuePair<string, string>>(1)
      {
        new KeyValuePair<string, string>(@"hola",@"adios")
      };
      HttpResponseMessage contenido = await http.Delete(UrlBaseBin, MetodoBin, parametros);
      Assert.True(contenido.IsSuccessStatusCode);
    }

    /// <summary>
    /// Comprueba que el metodo Descargar de la utilidad http
    /// puede descargar un archivo codificado en un buffer
    /// dada una direccion de recurso
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

    /// <summary>
    /// Comprueba que el metodo Descargar en directorio de la utilidad http
    /// puede descargar un archivo y guardarlos en una direccion indicada
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task DescargarEnDirectorio()
    {
      string url = @"https://www.google.com";
      string metodo = @"/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png";
      SolicitudHttp http = new SolicitudHttp();
      RespuestaBasica contenido = await http.DescargarEnDirectorio(url, metodo, AppDomain.CurrentDomain.BaseDirectory, @"LogoGoogle", @"png");
      Assert.True(contenido.Correcto);
    }
  }
}
