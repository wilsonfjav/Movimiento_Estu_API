using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MovimientoEstudiantil.Models
{
    public class Estudiante
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idEstudiante { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string correo { get; set; }

        [Required]
        public int provincia { get; set; }

        [Required]
        public int sede { get; set; }

        [Required]
        [StringLength(2)]
        public string satisfaccionCarrera { get; set; }

        [Required]
        public int anioIngreso { get; set; }

        [JsonIgnore]
        public virtual Provincia? Provincia_I { get; set; }

        [JsonIgnore]
        public virtual Sede? Sede_I { get; set; }
    }
}
