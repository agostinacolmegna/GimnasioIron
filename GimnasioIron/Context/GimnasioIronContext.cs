using Microsoft.EntityFrameworkCore;
using GimnasioIron.Models;
using System;

namespace GimnasioIron.Context
{
    public class GimnasioIronContext : DbContext
    {
        public GimnasioIronContext(DbContextOptions<GimnasioIronContext> options)
            : base(options)
        {
        }




        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Clase> Clases { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Pago> Pagos { get; set; }

        
    }
}
