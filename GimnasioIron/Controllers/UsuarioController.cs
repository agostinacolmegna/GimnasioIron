using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using GimnasioIron.Context;
using GimnasioIron.Models;
using Microsoft.EntityFrameworkCore;

namespace GimnasioIron.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly GimnasioIronContext _context;

        public UsuarioController(GimnasioIronContext context)
        {
            _context = context;
        }

       

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registro(Usuario usuario)
        {
            if (_context.Usuarios.Any(u => u.Email == usuario.Email))
                ModelState.AddModelError("Email", "Este email ya está registrado.");

            int edad = DateTime.Today.Year - usuario.FechaNacimiento.Year;
            if (usuario.FechaNacimiento > DateTime.Today.AddYears(-edad)) edad--;

            if (edad < 16)
                ModelState.AddModelError("FechaNacimiento", "Debés tener al menos 16 años.");

            if (ModelState.IsValid)
            {
                usuario.EsAdmin = false;
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();
                return RedirectToAction("Confirmacion");
            }

            return View(usuario);
        }

        public IActionResult Confirmacion()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string contraseña)
        {
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Email.ToLower() == email.ToLower() && u.Contraseña == contraseña);

            if (usuario == null)
            {
                ViewBag.Error = "Email o contraseña incorrectos.";
                return View();
            }

            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
            HttpContext.Session.SetString("Nombre", usuario.Nombre);
            HttpContext.Session.SetString("EsAdmin", usuario.EsAdmin ? "true" : "false");

            if (usuario.EsAdmin)
            {
                TempData["Mensaje"] = $"Bienvenido, administrador {usuario.Nombre}.";
                return RedirectToAction("PanelAdmin");
            }

            var hoy = DateTime.Today;

            bool cuotaPaga = _context.Pagos.Any(p =>
                p.UsuarioId == usuario.Id &&
                p.Estado == EstadoPago.Aprobado &&
                p.Fecha.Month == hoy.Month &&
                p.Fecha.Year == hoy.Year
            );

            if (!cuotaPaga)
                TempData["CuotaMensaje"] = "Tu cuota está vencida. Para continuar reservando clases, aboná la mensualidad.";

            return RedirectToAction("PanelSocio");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

      

        public IActionResult PanelSocio()
        {
            return View();
        }



        public IActionResult PanelAdmin(string filtroNombre = "")
        {
           
            return View();
        }

        public IActionResult VerPagos(string filtroNombre = "")
        {
            var pagos = _context.Pagos
                .Include(p => p.Usuario)
                .Where(p => string.IsNullOrEmpty(filtroNombre) || p.Usuario.Nombre.Contains(filtroNombre))
                .OrderByDescending(p => p.Fecha)
                .ToList();

            ViewBag.FiltroNombre = filtroNombre;
            return View(pagos);
        }

        [HttpPost]
        public IActionResult Aprobar(int id)
        {
            var pago = _context.Pagos.FirstOrDefault(p => p.Id == id);
            if (pago != null)
            {
                pago.Estado = EstadoPago.Aprobado;
                _context.SaveChanges();
            }
            return RedirectToAction("VerPagos");
        }

        [HttpPost]
        public IActionResult Rechazar(int id, string motivo)
        {
            if (string.IsNullOrWhiteSpace(motivo))
            {
                TempData["Mensaje"] = "Debés seleccionar un motivo para rechazar el pago.";
                TempData["TipoMensaje"] = "warning";
                return RedirectToAction("VerPagos");
            }

            var pago = _context.Pagos.FirstOrDefault(p => p.Id == id);
            if (pago != null)
            {
                pago.Estado = EstadoPago.Rechazado;
                pago.MotivoRechazo = motivo;
                _context.SaveChanges();
            }

            TempData["Mensaje"] = "Pago rechazado correctamente.";
            TempData["TipoMensaje"] = "danger";
            return RedirectToAction("VerPagos");
        }
    }
}


//cambios2
//cambio3
//cambio5