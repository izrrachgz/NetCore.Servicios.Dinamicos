using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Servicio.Entidades;
using Servicio.Modelos;
using Servicio.Servicios;
using Xunit;

namespace Pruebas.Teorias
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

    [Theory, InlineData(2, 2000)]
    public void GuardarUsuarios(short estimado = 1, int cantidad = 1000)
    {
      Servicio<Usuario> servicio = new Servicio<Usuario>();
      List<Usuario> usuarios = Enumerable.Repeat(Usuario, cantidad).ToList();
      Stopwatch temporizador = new Stopwatch();
      temporizador.Start();
      RespuestaColeccion<int> guardados = servicio.Guardar(usuarios);
      temporizador.Stop();
      Assert.True(guardados.Correcto && temporizador.Elapsed.TotalSeconds <= estimado);
      usuarios.Clear();
      usuarios.TrimExcess();
      usuarios = null;
      guardados.Coleccion.Clear();
      guardados.Coleccion.TrimExcess();
      guardados.Coleccion = null;
    }

    [Theory, InlineData(2, 2000)]
    public void ObtenerUsuarios(short estimado = 1, int cantidad = 1000)
    {
      Servicio<Usuario> servicio = new Servicio<Usuario>();
      Paginado paginado = new Paginado();
      paginado.Elementos = cantidad;
      Stopwatch temporizador = new Stopwatch();
      temporizador.Start();
      RespuestaColeccion<Usuario> usuarios = servicio.Obtener(paginado);
      temporizador.Stop();
      Assert.True(usuarios.Correcto && temporizador.Elapsed.TotalSeconds <= estimado);
      usuarios.Coleccion.Clear();
      usuarios.Coleccion.TrimExcess();
      usuarios.Coleccion = null;
    }
  }
}