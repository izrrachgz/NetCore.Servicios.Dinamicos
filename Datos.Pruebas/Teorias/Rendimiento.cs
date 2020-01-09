using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Datos.Entidades;
using Datos.Modelos;
using Datos.ProveedoresDeDatos;
using Xunit;

namespace Datos.Pruebas.Teorias
{
  public class Rendimiento
  {
    private Usuario Usuario { get; }

    public Rendimiento()
    {
      Usuario = new Usuario()
      {
        Nombre = @"Pruebas",
        ApellidoPaterno = @"Pruebas",
        ApellidoMaterno = @"Pruebas",
        Correo = @"Pruebas@CSharp.com",
        NumeroContacto = @"6623559566"
      };
    }

    [Theory, InlineData(1, 1000)]
    public async Task GuardarUsuarios(short estimado = 1, int cantidad = 1000)
    {
      ProveedorDeDatos<Usuario> servicio = new ProveedorDeDatos<Usuario>();
      List<Usuario> usuarios = Enumerable.Repeat(Usuario, cantidad).ToList();
      Stopwatch temporizador = new Stopwatch();
      temporizador.Start();
      RespuestaColeccion<int> guardados = await servicio.Guardar(usuarios);
      temporizador.Stop();
      Assert.True(guardados.Correcto && temporizador.Elapsed.TotalSeconds <= estimado);
      usuarios.Clear();
      usuarios.TrimExcess();
      usuarios = null;
      guardados.Coleccion.Clear();
      guardados.Coleccion.TrimExcess();
      guardados.Coleccion = null;
    }

    [Theory, InlineData(1, 1000)]
    public async Task ObtenerUsuarios(short estimado = 1, int cantidad = 1000)
    {
      ProveedorDeDatos<Usuario> servicio = new ProveedorDeDatos<Usuario>();
      Paginado paginado = new Paginado() { Elementos = cantidad };
      Stopwatch temporizador = new Stopwatch();
      temporizador.Start();
      RespuestaColeccion<Usuario> usuarios = await servicio.Obtener(paginado);
      temporizador.Stop();
      Assert.True(usuarios.Correcto && temporizador.Elapsed.TotalSeconds <= estimado);
      usuarios.Coleccion.Clear();
      usuarios.Coleccion.TrimExcess();
      usuarios.Coleccion = null;
    }

    [Theory, InlineData(1)]
    public async Task ObtenerUsuariosClaveValor(short estimado)
    {
      ProveedorDeDatos<Usuario> servicio = new ProveedorDeDatos<Usuario>();
      Stopwatch temporizador = new Stopwatch();
      temporizador.Start();
      RespuestaColeccion<ClaveValor> usuarios = await servicio.Obtener("Correo");
      temporizador.Stop();
      Assert.True(usuarios.Correcto && temporizador.Elapsed.TotalSeconds <= estimado);
      usuarios.Coleccion.Clear();
      usuarios.Coleccion.TrimExcess();
      usuarios.Coleccion = null;
    }

    [Theory, InlineData(1)]
    public async Task ObtenerUsuariosColumnas(short estimado, int cantidad = 1000)
    {
      ProveedorDeDatos<Usuario> servicio = new ProveedorDeDatos<Usuario>();
      Paginado paginado = new Paginado() { Elementos = cantidad };
      string[] columnas = { "Id", "Nombre", "Correo" };
      Stopwatch temporizador = new Stopwatch();
      temporizador.Start();
      RespuestaColeccion<Usuario> usuarios = await servicio.Obtener(columnas, paginado);
      temporizador.Stop();
      Assert.True(usuarios.Correcto && temporizador.Elapsed.TotalSeconds <= estimado);
      usuarios.Coleccion.Clear();
      usuarios.Coleccion.TrimExcess();
      usuarios.Coleccion = null;
    }
  }
}