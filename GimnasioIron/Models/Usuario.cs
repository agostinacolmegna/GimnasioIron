using System.ComponentModel.DataAnnotations;

namespace GimnasioIron.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Contraseña { get; set; }

        [Required]
        public string? Telefono { get; set; }
       
        [Required]
        public DateTime FechaNacimiento { get; set; }

        public bool EsAdmin { get; set; }

        public List<Reserva> Reservas { get; set; } = new List<Reserva>();


    }
}