using System.Collections.Generic;
using System.Threading.Tasks;
using Datos.Entidades;
using Datos.Enumerados;
using Datos.Modelos;

namespace Negocio.ProveedoresDeDatos
{
  public class ProveedorUsuario : ProveedorBasico<Usuario>
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pagina"></param>
    /// <returns></returns>
    public async Task<RespuestaColeccion<Usuario>> Usuarios(Paginado pagina)
      => await Datos.Obtener(pagina);

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
      return await Datos.Obtener(paginado ?? new Paginado(), condiciones);
    }
  }
}
