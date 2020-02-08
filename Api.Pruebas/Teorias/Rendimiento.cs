using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Datos.Utilidades;
using Datos.Modelos;
using Negocio.Utilidades;
using Newtonsoft.Json;
using Xunit;

namespace Api.Pruebas.Teorias
{
  public class Rendimiento
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="estimado"></param>
    /// <param name="ciclos"></param>
    /// <param name="hilos"></param>
    [Theory, InlineData(2, 1, 1)]
    public async Task Estres(short estimado, int ciclos, byte hilos)
    {
      SolicitudHttp http = new SolicitudHttp();
      string url = @"https://localhost:44330/";
      string metodo = @"api/Usuarios/Paginado";
      string json = JsonConvert.SerializeObject(new SolicitudPagina());
      List<Task<HttpResponseMessage>> tareas = new List<Task<HttpResponseMessage>>(1)
      {
        http.Post(url, metodo, json)
      };
      DiagnosticarRendimiento<HttpResponseMessage> prueba = new DiagnosticarRendimiento<HttpResponseMessage>(tareas, ciclos, hilos);
      await prueba.Ejecutar();
      Assert.True(prueba.Cronometro.Elapsed.TotalSeconds <= estimado);
    }
  }
}
