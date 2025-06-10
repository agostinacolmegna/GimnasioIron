using System.ComponentModel.DataAnnotations;

namespace GimnasioIron.Models
{
    public class Clase
    {
        public int Id { get; set; }

        [Required]
        public string? Nombre { get; set; }

        [Required]
        public string? Profesor { get; set; }

        [Required]
        public DateTime FechaHora { get; set; }

        [Range(1, 100)]
        public int CupoMaximo{ get; set; }

        public List<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}
