using System;
using System.ComponentModel.DataAnnotations;

namespace GimnasioIron.Models
{
    public class Pago
    {
        public int Id { get; set; }
        public double Monto { get; set; }
        public MetodoPago Metodo { get; set; }
        public EstadoPago Estado { get; set; }
        public DateTime Fecha { get; set; }
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public string? NumeroTarjeta { get; set; }  // opcional para simular tarjeta
        public string? CVV { get; set; }            // opcional para simular tarjeta
        public string? MotivoRechazo { get; set; }  // para mostrar al socio
    }

    public enum MetodoPago
    {
        Transferencia = 0,
        Tarjeta = 1
    }

    public enum EstadoPago
    {
        Pendiente = 0,
        Aprobado = 1,
        Rechazado = 2
    }
}
