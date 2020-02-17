using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Datos.Entidades;
using Newtonsoft.Json;

namespace Contexto.Entidades
{
  /// <summary>
  /// Representa los detalles de una entrada
  /// de log
  /// </summary>
  [Table(@"BitacoraDetalle")]
  public class BitacoraDetalle : EntidadBase
  {
    /// <summary>
    /// Llave foranea que hace referencia
    /// a la entrada de log
    /// </summary>
    [Required, ForeignKey(@"Bitacora")]
    public int IdBitacora { get; set; }

    /// <summary>
    /// Relacion hacia la entrada de log
    /// </summary>
    [JsonIgnore]
    public Bitacora Bitacora { get; set; }

    /// <summary>
    /// Valor de detalle
    /// </summary>
    [Required]
    public string Valor { get; set; }
  }
}
