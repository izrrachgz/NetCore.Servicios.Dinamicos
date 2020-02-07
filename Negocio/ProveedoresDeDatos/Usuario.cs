using System.Collections.Generic;
using System.Threading.Tasks;
using Contexto.Entidades;
using Contexto.ProveedoresDeDatos;
using Datos.Enumerados;
using Datos.Modelos;

namespace Negocio.ProveedoresDeDatos
{
  public class ProveedorUsuario : ProveedorDeDatosBase<Usuario>
  {
    /// <summary>
    /// Devuelve todos los usuarios dentro de un modelo de datos
    /// paginado
    /// </summary>
    /// <param name="pagina"></param>
    /// <returns></returns>
    public async Task<RespuestaColeccion<Usuario>> Usuarios(Paginado pagina)
      => await Obtener(pagina);

    /// <summary>
    /// Devuelve todos los usuarios que no tiene un numero de celular
    /// asignado
    /// </summary>
    /// <param name="paginado">Pagina solicitada</param>
    /// <returns></returns>
    public async Task<RespuestaColeccion<Usuario>> UsuariosSinCelular(Paginado paginado)
    {
      List<Condicion> condiciones = new List<Condicion>(1)
      {
        new Condicion(@"Celular",null,Operador.Es)
      };
      return await Obtener(paginado ?? new Paginado(), condiciones);
    }
  }
}
