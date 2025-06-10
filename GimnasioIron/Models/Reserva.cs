namespace GimnasioIron.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public int ClaseId { get; set; }
        public Clase? Clase { get; set; }

        public DateTime FechaReserva { get; set; }
    }
}
