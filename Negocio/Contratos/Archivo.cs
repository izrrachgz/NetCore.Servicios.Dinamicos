using System.IO;
using System.Threading.Tasks;
using Datos.Modelos;

namespace Negocio.Contratos
{
  /// <summary>
  /// Contrato de Archivo
  /// </summary>
  public interface IArchivo
  {
    /// <summary>
    /// Nombre del archivo sin extension
    /// </summary>
    string Nombre { get; set; }

    /// <summary>
    /// Persona, aplicacion o referencia de creador
    /// </summary>
    string Autor { get; set; }

    /// <summary>
    /// Extension del archivo
    /// </summary>
    string Extension { get; set; }

    /// <summary>
    /// Breve descripcion del contenido o proposito del archivo
    /// </summary>
    string Descripcion { get; set; }

    /// <summary>
    /// Directorio de salida
    /// </summary>
    string DirectorioDeSalida { get; }

    /// <summary>
    /// Debera guardar el contenido del
    /// archivo en el directorio de salida
    /// indicado
    /// </summary>
    /// <returns></returns>
    Task<RespuestaBasica> Guardar();

    /// <summary>
    /// Debera proporcionar la informacion del archivo
    /// en disco
    /// </summary>
    Task<FileInfo> Informacion { get; }
  }
}
