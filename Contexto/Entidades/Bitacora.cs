using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Contexto.Enumerados;
using Utilidades.Entidades;

namespace Contexto.Entidades
{
  /// <summary>
  /// Representa una entrada de log
  /// </summary>
  [Table(@"Bitacora")]
  public class Bitacora : EntidadBase
  {
    /// <summary>
    /// Nombre de la entrada
    /// </summary>
    [Required, MaxLength(128)]
    public string Nombre { get; set; }

    /// <summary>
    /// Breve descripcion de la anotacion
    /// </summary>
    [MaxLength(512), DefaultValue(null)]
    public string Descripcion { get; set; }

    /// <summary>
    /// Tipo de entrada
    /// </summary>
    [Required, DefaultValue(BitacoraTipo.Informacion)]
    public BitacoraTipo Tipo { get; set; }
  }
}
