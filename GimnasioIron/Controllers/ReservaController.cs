using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using GimnasioIron.Context;
using GimnasioIron.Models;
using System.Linq;

namespace GimnasioIron.Controllers
{
    public class ReservaController : Controller
    {
        private readonly GimnasioIronContext _context;

        public ReservaController(GimnasioIronContext context)
        {
            _context = context;
        }

        
        public IActionResult MisReservas()
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null) return RedirectToAction("Login", "Usuario");

            var reservas = _context.Reservas
                .Include(r => r.Clase)
                .Where(r => r.UsuarioId == usuarioId)
                .OrderBy(r => r.Clase.FechaHora)
                .ToList();

            return View(reservas);
        }

        
        public IActionResult ReservarClase()
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null) return RedirectToAction("Login", "Usuario");

            var hoy = DateTime.Today;
            var clases = _context.Clases
                .Include(c => c.Reservas)
                .Where(c => c.FechaHora >= hoy)
                .OrderBy(c => c.FechaHora)
                .ToList();

           
            var reservasDelUsuario = _context.Reservas
                .Include(r => r.Clase)
                .Where(r => r.UsuarioId == usuarioId)
                .ToList();

            var reservasPorDia = reservasDelUsuario
                .GroupBy(r => r.Clase.FechaHora.Date)
                .ToDictionary(g => g.Key, g => g.Count());

            ViewBag.ReservasPorDia = reservasPorDia;

            return View(clases);
        }

      
        [HttpPost]
        public IActionResult ConfirmarReserva(int claseId)
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null) return RedirectToAction("Login", "Usuario");

            var clase = _context.Clases
                .Include(c => c.Reservas)
                .FirstOrDefault(c => c.Id == claseId);

            if (clase == null || clase.FechaHora < DateTime.Now)
            {
                TempData["Error"] = "Clase inválida.";
                return RedirectToAction("ReservarClase");
            }

            var yaReservado = clase.Reservas.Any(r => r.UsuarioId == usuarioId);
            if (yaReservado)
            {
                TempData["Error"] = "Ya tenés una reserva hecha para esta clase.";
                return RedirectToAction("ReservarClase");
            }

            var cupoDisponible = clase.CupoMaximo - clase.Reservas.Count;
            if (cupoDisponible <= 0)
            {
                TempData["Error"] = "Clase completa. No hay más disponibilidad.";
                return RedirectToAction("ReservarClase");
            }

       
            var reservasHoy = _context.Reservas
                .Include(r => r.Clase)
                .Where(r => r.UsuarioId == usuarioId && r.Clase.FechaHora.Date == clase.FechaHora.Date)
                .Count();

            if (reservasHoy >= 2)
            {
                TempData["Error"] = "Ya tenés 2 clases reservadas para ese día.";
                return RedirectToAction("ReservarClase");
            }

            var nuevaReserva = new Reserva
            {
                ClaseId = claseId,
                UsuarioId = usuarioId.Value
            };

            _context.Reservas.Add(nuevaReserva);
            _context.SaveChanges();

            TempData["Mensaje"] = "¡Reserva exitosa!";
            return RedirectToAction("MisReservas");
        }

   
        [HttpPost]
        public IActionResult Cancelar(int id)
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null) return RedirectToAction("Login", "Usuario");

            var reserva = _context.Reservas.FirstOrDefault(r => r.Id == id && r.UsuarioId == usuarioId);
            if (reserva != null)
            {
                _context.Reservas.Remove(reserva);
                _context.SaveChanges();
                TempData["Mensaje"] = "La reserva fue cancelada exitosamente.";
            }
            else
            {
                TempData["Error"] = "No se encontró la reserva o no tenés permiso para cancelarla.";
            }

            return RedirectToAction("MisReservas");
        }

        
        public IActionResult VerReservas()
        {
            var clasesConReservas = _context.Clases
                .Include(c => c.Reservas)
                .ThenInclude(r => r.Usuario)
                .OrderBy(c => c.FechaHora)
                .ToList();

            return View(clasesConReservas);
        }
    }
}
