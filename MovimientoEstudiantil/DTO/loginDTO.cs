using System.ComponentModel.DataAnnotations;

namespace MovimientoEstudiantil.DTO
{
    //MODIFICADO POR OSCAR SERRANO 26/05/2025
    public class loginDTO
    {
        [Required(ErrorMessage = "campo obligatorio")]
        [RegularExpression(@"^[^@\s]+@ucr\.ac\.cr$",
           ErrorMessage = "El correo debe usar el dominio @ucr.ac.cr")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "campo obligatorio")]
        //[RegularExpression(@"^(?=.[a-z])(?=.[A-Z])(?=.*\d).{6,}$",
        //ErrorMessage = "La contraseña debe tener al menos 6 caracteres, incluyendo una mayúscula, una minúscula y un número.")]
        public string Contrasena { get; set; }
    }
}