using System.ComponentModel.DataAnnotations;

namespace MovimientoEstudiantil.DTO
{
    public class UsuarioDTO
    {
        [Required(ErrorMessage = "El campo sede es obligatorio.")]
        public int sede { get; set; }

        [Required(ErrorMessage = "El campo correo institucional es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debe ser un correo electrónico válido.")]
        [RegularExpression(@"^[^@\s]+@ucr\.ac\.cr$", ErrorMessage = "Solo se permiten correos con dominio @ucr.ac.cr")]
        public string correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres, incluyendo una mayúscula, una minúscula y un número.")]
        public string contrasena { get; set; }

        [Required(ErrorMessage = "El campo rol es obligatorio.")]
        public string rol { get; set; }
    }
}