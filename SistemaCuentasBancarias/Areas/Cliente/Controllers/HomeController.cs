using System.Diagnostics;
using Microsoft.AspNetCore.JsonPatch.Internal;
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

        // Para buscador
        [HttpGet]
        public IActionResult ResultadoBusqueda(string searchString, int page = 1, int pageSize = 6)
        {
            var articulos = _contedorTrabajo.Articulo.AsQueryable();

            // Filtrar por título si hay un término de búsqueda
            if(!string.IsNullOrEmpty(searchString))
            {
                articulos = articulos.Where(a => a.Nombre.Contains(searchString));
            }

            // Paginar los resultados
            var paginatedEntries = articulos.Skip((page-1) * pageSize).Take(pageSize);

            // Crear el modelo para la vista
            var model = new ListaPaginada<Articulo>(paginatedEntries.ToList(), articulos.Count(), page, pageSize, searchString);

            return View(model);
        }
        public IActionResult Detalle(int id)
        {
            var articulo = _contedorTrabajo.Articulo.GetFirstOrDefault(a => a.Id == id);
            if (articulo == null)
            {
                return NotFound();
            }
            return View(articulo);
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
