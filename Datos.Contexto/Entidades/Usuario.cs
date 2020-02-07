using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Datos.Entidades;

namespace Datos.Contexto.Entidades
{
  [Table("Usuario")]
  public class Usuario : EntidadBase
  {
    [Required, MaxLength(128)]
    public string Nombre { get; set; }

    [Required, MaxLength(64)]
    public string ApellidoPaterno { get; set; }

    [Required, MaxLength(64)]
    public string ApellidoMaterno { get; set; }

    [NotMapped]
    public string NombreCompleto => $@"{Nombre} {ApellidoPaterno} {ApellidoMaterno}";

    [/*Index(IsUnique = false, IsClustered = false),*/ Required, MaxLength(192)]
    public string Correo { get; set; }

    [Required, MaxLength(18)]
    public string NumeroContacto { get; set; }
  }
}
