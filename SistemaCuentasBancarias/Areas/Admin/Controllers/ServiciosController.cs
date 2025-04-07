using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaCuentasBancarias.AccesoDatos.Data.Repository.IRepository;
using SistemaCuentasBancarias.Data;
using SistemaCuentasBancarias.Models;

namespace SistemaCuentasBancarias.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Area("Admin")]
    public class ServiciosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;

        public ServiciosController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }
        //[AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        #region Llamadas a la API
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _contenedorTrabajo.Servicio.GetAll() });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _contenedorTrabajo.Servicio.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error borrando servicio" });
            }

            _contenedorTrabajo.Servicio.Remove(objFromDb);
            _contenedorTrabajo.Save();

            return Json(new { success = true, message = "Servicio borrado correctamente" });


        }
        #endregion

        [HttpGet]
        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Servicio servicio)
        {
            if (ModelState.IsValid) {
                _contenedorTrabajo.Servicio.Add(servicio);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(servicio);
        }

        [HttpGet]
        public IActionResult Edit(int id) 
        {
            Servicio servicio = new Servicio();
            servicio = _contenedorTrabajo.Servicio.Get(id);

            if (servicio == null) 
            {
                return NotFound();
            }
            return View(servicio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Servicio servicio) 
        {
            if (ModelState.IsValid) 
            {
                _contenedorTrabajo.Servicio.Update(servicio);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index)); 
            }
            return View(servicio);
        }
    }
}
