using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GimnasioIron.Context;
using GimnasioIron.Models;
using System.Linq;

namespace GimnasioIron.Controllers
{
    public class ClaseController : Controller
    {
        private readonly GimnasioIronContext _context;

        public ClaseController(GimnasioIronContext context)
        {
            _context = context;
        }

        // 🔹 Muestra todas las clases para el admin
        public IActionResult PanelClases()
        {
            var clases = _context.Clases.OrderBy(c => c.FechaHora).ToList();
            return View(clases);
        }

        // 🔹 Muestra el formulario para crear una clase
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        // 🔹 Procesa la creación de clase
        [HttpPost]
        public IActionResult Crear([Bind("Nombre,Profesor,FechaHora,CupoMaximo")] Clase clase)
        {
            if (ModelState.IsValid)
            {
                _context.Clases.Add(clase);
                _context.SaveChanges();
                return RedirectToAction("PanelAdmin", "Usuario");
            }

            return View(clase);
        }

        // 🔹 Muestra el formulario para editar
        public IActionResult Editar(int id)
        {
            var clase = _context.Clases.FirstOrDefault(c => c.Id == id);
            if (clase == null) return NotFound();
            return View(clase);
        }

        // 🔹 Guarda la clase editada
        [HttpPost]
        public IActionResult Editar(Clase claseEditada)
        {
            if (ModelState.IsValid)
            {
                _context.Clases.Update(claseEditada);
                _context.SaveChanges();
                TempData["Mensaje"] = "Clase actualizada correctamente.";
                return RedirectToAction("PanelClases");
            }

            return View(claseEditada);
        }

        // 🔹 Confirmar antes de eliminar
        public IActionResult ConfirmarEliminar(int id)
        {
            var clase = _context.Clases.FirstOrDefault(c => c.Id == id);
            if (clase == null)
            {
                return NotFound();
            }

            return View(clase);
        }

        // 🔹 Elimina la clase solo si no tiene reservas
        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            var clase = _context.Clases
                .Include(c => c.Reservas)
                .FirstOrDefault(c => c.Id == id);

            if (clase == null)
            {
                return NotFound();
            }

            if (clase.Reservas.Any())
            {
                TempData["Error"] = "No se puede eliminar una clase que ya tiene reservas.";
                return RedirectToAction("PanelClases");
            }

            _context.Clases.Remove(clase);
            _context.SaveChanges();
            TempData["Mensaje"] = "Clase eliminada exitosamente.";
            return RedirectToAction("PanelClases");
        }
    }
}
