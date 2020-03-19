using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Contexto.Entidades;
using Contexto.Enumerados;
using Utilidades.Modelos;
using Utilidades.ProveedoresDeDatos;
using Xunit;

namespace Contexto.Pruebas.Teorias
{
  /// <summary>
  /// Pruebas teoricas de rendimiento en proveedores
  /// de datos
  /// </summary>
  public class Rendimiento
  {
    private Bitacora Entrada { get; }

    public Rendimiento()
    {
      Entrada = new Bitacora()
      {
        Nombre = @"Death Note",
        Descripcion = @"6:40",
        Tipo = BitacoraTipo.Advertencia
      };
    }

    /// <summary>
    /// Evalua la posibilidad de guardar n
    /// entradas antes de que termine el
    /// intervalo de tiempo en segundos
    /// estimado
    /// </summary>
    /// <param name="estimado">Segundos dados para terminar la tarea</param>
    /// <param name="cantidad">Cantidad de entidades para guardar</param>
    /// <returns>Tarea indicando el estado</returns>
    [Theory, InlineData(1, 1000)]
    public async Task GuardarEntradasDeLog(short estimado = 1, int cantidad = 1000)
    {
      ProveedorDeDatos<Bitacora> servicio = new ProveedorDeDatos<Bitacora>();
      List<Bitacora> entradas = Enumerable.Repeat(Entrada, cantidad).ToList();
      Stopwatch temporizador = new Stopwatch();
      temporizador.Start();
      RespuestaColeccion<long> guardados = await servicio.Guardar(entradas);
      temporizador.Stop();
      Assert.True(guardados.Correcto && temporizador.Elapsed.TotalSeconds <= estimado);
      entradas.Clear();
      entradas.TrimExcess();
      entradas = null;
      guardados.Coleccion.Clear();
      guardados.Coleccion.TrimExcess();
      guardados.Coleccion = null;
    }

    /// <summary>
    /// Evalua la posibilidad de obtener n
    /// entradas antes de que termine el
    /// intervalo de tiempo en segundos
    /// estimado
    /// </summary>
    /// <param name="estimado">Segundos dados para terminar la tarea</param>
    /// <param name="cantidad">Cantidad de entidades para obtener</param>
    /// <returns>Tarea indicando el estado</returns>
    [Theory, InlineData(1, 1000)]
    public async Task ObtenerEntradasDeLog(short estimado = 1, int cantidad = 1000)
    {
      ProveedorDeDatos<Bitacora> servicio = new ProveedorDeDatos<Bitacora>();
      Paginado paginado = new Paginado() { Elementos = cantidad };
      Stopwatch temporizador = new Stopwatch();
      temporizador.Start();
      RespuestaColeccion<Bitacora> entradas = await servicio.Obtener(paginado);
      temporizador.Stop();
      Assert.True(entradas.Correcto && temporizador.Elapsed.TotalSeconds <= estimado);
      entradas.Coleccion.Clear();
      entradas.Coleccion.TrimExcess();
      entradas.Coleccion = null;
    }

    /// <summary>
    /// Evalua la posibilidad de obtener
    /// todas las entradas de log antes de que
    /// termine el intervalo de tiempo
    /// en segundos estimado
    /// </summary>
    /// <param name="estimado">Segundos dados para terminar la tarea</param>
    /// <returns>Tarea indicando el estado</returns>
    [Theory, InlineData(1)]
    public async Task ObtenerEntradasDeLogPorIndiceValor(short estimado)
    {
      ProveedorDeDatos<Bitacora> servicio = new ProveedorDeDatos<Bitacora>();
      Stopwatch temporizador = new Stopwatch();
      temporizador.Start();
      RespuestaColeccion<IndiceValor> entradas = await servicio.Obtener("Nombre");
      temporizador.Stop();
      Assert.True(entradas.Correcto && temporizador.Elapsed.TotalSeconds <= estimado);
      entradas.Coleccion.Clear();
      entradas.Coleccion.TrimExcess();
      entradas.Coleccion = null;
    }

    /// <summary>
    /// Evalua la posibilidad de obtener
    /// todas las entradas de log con columnas
    /// especificas antes de que termine el
    /// intervalo de tiempo en segundos
    /// estimado
    /// </summary>
    /// <param name="estimado">Segundos dados para terminar la tarea</param>
    /// <param name="cantidad">Cantidad de entidades para obtener</param>
    /// <returns>Tarea indicando el estado</returns>
    [Theory, InlineData(1)]
    public async Task ObtenerEntradasDeLogPorColumnas(short estimado, int cantidad = 1000)
    {
      ProveedorDeDatos<Bitacora> servicio = new ProveedorDeDatos<Bitacora>();
      Paginado paginado = new Paginado() { Elementos = cantidad };
      string[] columnas = { "Id", "Nombre", "Descripcion" };
      Stopwatch temporizador = new Stopwatch();
      temporizador.Start();
      RespuestaColeccion<Bitacora> coleccion = await servicio.Obtener(columnas, paginado);
      temporizador.Stop();
      Assert.True(coleccion.Correcto && temporizador.Elapsed.TotalSeconds <= estimado);
      coleccion.Coleccion.Clear();
      coleccion.Coleccion.TrimExcess();
      coleccion.Coleccion = null;
    }
  }
}