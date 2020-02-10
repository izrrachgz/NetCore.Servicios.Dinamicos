using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Api.Pruebas.Configuraciones;
using Datos.Configuraciones;
using Datos.Extensiones;
using Datos.Modelos;
using Datos.Utilidades;
using Newtonsoft.Json;
using Xunit;

namespace Api.Pruebas.Hechos
{
  /// <summary>
  /// 
  /// </summary>
  public class Usuarios
  {
    /// <summary>
    /// Referencia a los elementos de configuracion
    /// contenidos en la configuracion de las pruebas
    /// </summary>
    private List<ElementoConfiguracion> Config { get; }

    public Usuarios()
    {
      Config = Configuracion<ConfiguracionApiPruebas>.Instancia.Configuraciones;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task ObtenerUsuariosPorPagina()
    {
      SolicitudHttp http = new SolicitudHttp();
      string url = Config.Obtener<string>(@"UrlObtenerUsuariosPorPagina");
      string metodo = Config.Obtener<string>(@"MetodoObtenerUsuariosPorPagina");
      string json = JsonConvert.SerializeObject(new SolicitudPagina());
      HttpResponseMessage respuesta = await http.Post(url, metodo, json);
      RespuestaModelo<RespuestaColeccion<Usuarios>> modelo = await respuesta.ObtenerDeContenidoJson<RespuestaColeccion<Usuarios>>();
      Assert.True(modelo.Correcto && modelo.Modelo.Correcto);
    }
  }
}
