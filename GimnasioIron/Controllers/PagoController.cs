using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using GimnasioIron.Context;
using GimnasioIron.Models;

namespace GimnasioIron.Controllers
{
    public class PagoController : Controller
    {
        private readonly GimnasioIronContext _context;

        public PagoController(GimnasioIronContext context)
        {
            _context = context;
        }

        // ✅ Muestra los pagos del usuario logueado
        public IActionResult MisPagos()
        {
            int? idUsuario = HttpContext.Session.GetInt32("UsuarioId");

            if (idUsuario == null)
                return RedirectToAction("Login", "Usuario");

            var pagos = _context.Pagos
                .Where(p => p.UsuarioId == idUsuario)
                .OrderByDescending(p => p.Fecha)
                .ToList();

            ViewBag.CuotaYaPagaEsteMes = pagos.Any(p =>
                p.Estado == EstadoPago.Aprobado &&
                p.Fecha.Month == DateTime.Today.Month &&
                p.Fecha.Year == DateTime.Today.Year
            );

            return View(pagos);
        }

        // ✅ Formulario GET para elegir método de pago
        [HttpGet]
        public IActionResult Abonar()
        {
            return View();
        }

        // ✅ POST para registrar el pago
        [HttpPost]
        public IActionResult Abonar(MetodoPago metodo, string? numeroTarjeta, string? cvv)
        {
            int? idUsuario = HttpContext.Session.GetInt32("UsuarioId");

            if (idUsuario == null)
                return RedirectToAction("Login", "Usuario");

            var hoy = DateTime.Today;

            bool yaPagoEsteMes = _context.Pagos.Any(p =>
                p.UsuarioId == idUsuario &&
                p.Fecha.Month == hoy.Month &&
                p.Fecha.Year == hoy.Year &&
                p.Estado != EstadoPago.Rechazado
            );

            if (yaPagoEsteMes)
            {
                TempData["Mensaje"] = "Ya abonaste la cuota este mes.";
                TempData["TipoMensaje"] = "warning";
                return RedirectToAction("MisPagos");
            }

            if (metodo == MetodoPago.Tarjeta && (string.IsNullOrWhiteSpace(numeroTarjeta) || string.IsNullOrWhiteSpace(cvv)))
            {
                TempData["Mensaje"] = "Debés ingresar los datos de la tarjeta.";
                TempData["TipoMensaje"] = "danger";
                return RedirectToAction("Abonar");
            }

            var nuevoPago = new Pago
            {
                UsuarioId = idUsuario.Value,
                Fecha = hoy,
                Monto = 35000,
                Metodo = metodo,
                Estado = EstadoPago.Pendiente,
                NumeroTarjeta = metodo == MetodoPago.Tarjeta ? numeroTarjeta : null,
                CVV = metodo == MetodoPago.Tarjeta ? cvv : null
            };

            _context.Pagos.Add(nuevoPago);
            _context.SaveChanges();

            TempData["Mensaje"] = "Tu pago fue registrado como pendiente. El administrador lo aprobará manualmente.";
            TempData["TipoMensaje"] = "success";
            return RedirectToAction("MisPagos");
        }
    }




}
