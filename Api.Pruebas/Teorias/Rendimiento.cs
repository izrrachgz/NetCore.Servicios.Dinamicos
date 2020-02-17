using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Api.Pruebas.Configuraciones;
using Datos.Configuraciones;
using Datos.Extensiones;
using Datos.Utilidades;
using Datos.Modelos;
using Newtonsoft.Json;
using Xunit;

namespace Api.Pruebas.Teorias
{
  /// <summary>
  /// Pruebas teoricas de rendimiento del api
  /// </summary>
  public class Rendimiento
  {
    /// <summary>
    /// Referencia a los elementos de configuracion
    /// contenidos en la configuracion de las pruebas
    /// </summary>
    private List<ElementoConfiguracion> Config { get; }

    public Rendimiento()
    {
      Config = Configuracion<ConfiguracionApiPruebas>.Instancia.Configuraciones;
    }

    /// <summary>
    /// Permite realizar un muestreo metrico sobre el
    /// comportamiento del servicio durante una prueba
    /// de estres
    /// </summary>
    /// <param name="estimado">Tiempo estimado en segundos en el que deberia concluir la tarea como maximo</param>
    /// <param name="ciclos">Cantidad de veces que se debe realizar la prueba</param>
    /// <param name="hilos">Cantidad de instancias que deben realizar la prueba</param>
    [Theory, InlineData(10, 1, 1)]
    public async Task ObtenerBitacoraPorPagina(short estimado, int ciclos, byte hilos)
    {
      SolicitudHttp http = new SolicitudHttp();
      string url = Config.Obtener<string>(@"UrlObtenerBitacoraPorPagina");
      string metodo = Config.Obtener<string>(@"MetodoObtenerBitacoraPorPagina");
      string json = JsonConvert.SerializeObject(new SolicitudPagina());
      List<Task<HttpResponseMessage>> tareas = new List<Task<HttpResponseMessage>>(1)
      {
        http.Post(url, metodo, json)
      };
      DiagnosticarRendimiento<HttpResponseMessage> prueba = new DiagnosticarRendimiento<HttpResponseMessage>(tareas, ciclos, hilos);
      ResumenDeDiagnostico<HttpResponseMessage> resumen = await prueba.Ejecutar();
      Assert.True(resumen.PromedioDeRespuesta.Ticks > 0 && prueba.Cronometro.Elapsed.TotalSeconds <= estimado);
    }
  }
}
