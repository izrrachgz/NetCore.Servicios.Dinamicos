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
  [Table(@"EntradaLogDetalle")]
  public class EntradaLogDetalle : EntidadBase
  {
    /// <summary>
    /// Llave foranea que hace referencia
    /// a la entrada de log
    /// </summary>
    [Required, ForeignKey(@"EntradaLog")]
    public int IdEntradaLog { get; set; }

    /// <summary>
    /// Relacion hacia la entrada de log
    /// </summary>
    [JsonIgnore]
    public EntradaLog EntradaLog { get; set; }

    /// <summary>
    /// Valor de detalle
    /// </summary>
    [Required]
    public string Valor { get; set; }
  }
}
