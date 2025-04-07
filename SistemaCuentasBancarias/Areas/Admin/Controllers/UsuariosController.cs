using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaCuentasBancarias.AccesoDatos.Data.Repository.IRepository;

namespace SistemaCuentasBancarias.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Area("Admin")]
    public class UsuariosController : Controller
    {
        private readonly IContenedorTrabajo _contedorTrabajo;

        public UsuariosController(IContenedorTrabajo contenedorTrabajo)
        {
            _contedorTrabajo = contenedorTrabajo;
            
        }

        [HttpGet]
        public IActionResult Index()
        {
            //Opción 1: Obtener todos los usuarios
            //return View(_contedorTrabajo.Usuario.GetAll());

            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var usuarioActual = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return View(_contedorTrabajo.Usuario.GetAll(usuario => usuario.Id != usuarioActual.Value));
        }

        [HttpGet]
        public IActionResult Bloquear(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _contedorTrabajo.Usuario.BloquearUsuario(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Desbloquear(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            _contedorTrabajo.Usuario.DesbloquearUsuario(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
