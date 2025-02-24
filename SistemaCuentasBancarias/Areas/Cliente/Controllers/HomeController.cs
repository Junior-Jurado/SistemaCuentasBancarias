using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaCuentasBancarias.AccesoDatos.Data.Repository.IRepository;
using SistemaCuentasBancarias.Models;
using SistemaCuentasBancarias.Models.ViewModels;

namespace SistemaCuentasBancarias.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class HomeController : Controller
    {
        private readonly IContenedorTrabajo _contedorTrabajo;

        public HomeController(IContenedorTrabajo contedorTrabajo)
        {
            _contedorTrabajo = contedorTrabajo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Sliders = _contedorTrabajo.Slider.GetAll(),
                Articulos = _contedorTrabajo.Articulo.GetAll()
            };

            // Esta línea es para poder saber si estamos en el Home o No
            ViewBag.IsHome = true;

            return View(homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
