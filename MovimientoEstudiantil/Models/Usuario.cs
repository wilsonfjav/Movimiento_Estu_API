using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovimientoEstudiantil.Models
{
    public class Usuario
    {
        [Key]
        public int idUsuario { get; set; }

        [Required]
        public int sede { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string correo { get; set; }

        [Required]
        [StringLength(255)]
        public string contrasena { get; set; }

        [Required]
        [StringLength(20)]
        public string rol { get; set; }

        [Required]
        public DateTime fechaRegistro { get; set; }

        public virtual Sede Sede { get; set; } // Si tienes una clase Sede definida
    }
}
