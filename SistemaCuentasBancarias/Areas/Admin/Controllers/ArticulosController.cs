using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaCuentasBancarias.AccesoDatos.Data.Repository.IRepository;
using SistemaCuentasBancarias.Models;
using SistemaCuentasBancarias.Models.ViewModels;

namespace SistemaCuentasBancarias.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Area("Admin")]
    public class ArticulosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _hostingEnviroment;

        public ArticulosController(IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment hostingEnviroment)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _hostingEnviroment = hostingEnviroment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            ArticuloVM articuloVM = new ArticuloVM() 
            {
                Articulo = new SistemaCuentasBancarias.Models.Articulo(),
                ListaServicios = _contenedorTrabajo.Servicio.GetListaServicios()
            };
            return View(articuloVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ArticuloVM articuloVM)
        {
            string rutaPrincipal = _hostingEnviroment.WebRootPath;
            var archivos = HttpContext.Request.Form.Files;

            if (articuloVM.Articulo.Id == 0 && archivos.Count() > 0)
            {
                // Nuevo Artículo
                string nombreArchivo = Guid.NewGuid().ToString();
                var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos");
                var extension = Path.GetExtension(archivos[0].FileName);
                using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                {
                    archivos[0].CopyTo(fileStreams);
                }

                articuloVM.Articulo.UrlImagen = @"\imagenes\articulos\" + nombreArchivo + extension;
                articuloVM.Articulo.FechaCreacion = DateTime.Now.ToString();

                _contenedorTrabajo.Articulo.Add(articuloVM.Articulo);
                _contenedorTrabajo.Save();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("Imagen", "Debes selecionar una imagen");
            }

            articuloVM.ListaServicios = _contenedorTrabajo.Servicio.GetListaServicios();
            return View(articuloVM);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ArticuloVM artiVM = new ArticuloVM() 
            {
                Articulo = new SistemaCuentasBancarias.Models.Articulo(),
                ListaServicios = _contenedorTrabajo.Servicio.GetListaServicios()
            };

            if (id != null)
            {
                artiVM.Articulo = _contenedorTrabajo.Articulo.Get(id.GetValueOrDefault());
            }

            return View(artiVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ArticuloVM articuloVM)
        {
            string rutaPrincipal = _hostingEnviroment.WebRootPath;
            var archivos = HttpContext.Request.Form.Files;

            var articuloDesdeDb = _contenedorTrabajo.Articulo.Get(articuloVM.Articulo.Id); 

            if (archivos.Count() > 0)
            {
                // Nueva imagen para el Artículo
                string nombreArchivo = Guid.NewGuid().ToString();
                var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos");
                var extension = Path.GetExtension(archivos[0].FileName);
                var nuevaExtension = Path.GetExtension(archivos[0].FileName);

                var rutaImagen = Path.Combine(rutaPrincipal, articuloDesdeDb.UrlImagen.TrimStart('\\'));

                if (System.IO.File.Exists(rutaImagen))
                {
                    System.IO.File.Delete(rutaImagen);
                }

                // Nuevamente subir la nueva imagen
                using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                {
                    archivos[0].CopyTo(fileStreams);
                }

                articuloVM.Articulo.UrlImagen = @"\imagenes\articulos\" + nombreArchivo + extension;
                articuloVM.Articulo.FechaCreacion = DateTime.Now.ToString();

                _contenedorTrabajo.Articulo.Update(articuloVM.Articulo);
                _contenedorTrabajo.Save();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Aquí sería cuando la imagen ya existe y se conserva
                articuloVM.Articulo.UrlImagen = articuloDesdeDb.UrlImagen;
                _contenedorTrabajo.Articulo.Update(articuloVM.Articulo);
                _contenedorTrabajo.Save();

                return RedirectToAction(nameof(Index));
            }

        }

        #region Llamadas a la API
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _contenedorTrabajo.Articulo.GetAll(includeProperties: "Servicio") });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objDesdeDb = _contenedorTrabajo.Articulo.Get(id);
            string rutaImagen = Path.Combine(_hostingEnviroment.WebRootPath, objDesdeDb.UrlImagen.TrimStart('\\'));
            if (System.IO.File.Exists(rutaImagen))
            {
                System.IO.File.Delete(rutaImagen);
            }
            if (objDesdeDb == null)
            {
                return Json(new { success = false, message = "Error al borrar el artículo" });
            }
            _contenedorTrabajo.Articulo.Remove(objDesdeDb);
            _contenedorTrabajo.Save();
            return Json(new { success = true, message = "Artículo borrado exitosamente" });
        }
        #endregion
    }
}
