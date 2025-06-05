using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MovimientoEstudiantil.DTO
{
    public class EstudianteDTO
    {
        /*Esta clase DTO es una intermediaria entre la clase principal 
         * ESTUDIANTE el unico dato que no se ingresa es idEstudiante 
         * ya que en progra no nos sirve ingresar el idEstudiante porque 
         * se genera automaticamente
         */
        [Required(ErrorMessage = "El campo correo es obligatorio")]
        [RegularExpression(@"^[^@\s]+@ucr\.ac\.cr$", ErrorMessage = "El correo debe usar el dominio @ucr.ac.cr")]
        [StringLength(100, ErrorMessage = "El correo no debe superar los 100 caracteres")]
        public string correo { get; set; }

        [Required(ErrorMessage = "El campo provincia es obligatorio")]
        public int provincia { get; set; }

        [Required(ErrorMessage = "El campo sede es obligatorio")]
        public int sede { get; set; }

        [Required(ErrorMessage = "El campo de satisfaccion es obligatorio")]
        [StringLength(2)]
        public string satisfaccionCarrera { get; set; }

        [Required(ErrorMessage = "El campo de año de ingreso es obligatorio")]
        public int anioIngreso { get; set; }

    }
}