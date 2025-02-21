using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaCuentasBancarias.AccesoDatos.Data.Repository.IRepository;
using SistemaCuentasBancarias.Data;
using SistemaCuentasBancarias.Models;

namespace SistemaCuentasBancarias.AccesoDatos.Data.Repository
{
    public class ServicioRepository : Repository<Servicio>, IServicioRepository
    {
        private readonly ApplicationDbContext _db;

        public ServicioRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
            
        }

        public IEnumerable<SelectListItem> GetListaServicios()
        {
            return _db.Servicio.Select(i => new SelectListItem() 
            {
                Text = i.Nombre,
                Value = i.Id.ToString()
            });
        }

        public void Update(Servicio servicio)
        {
            var objDesdeDb = _db.Servicio.FirstOrDefault(s => s.Id == servicio.Id);
            objDesdeDb.Nombre = servicio.Nombre;
            objDesdeDb.Orden = servicio.Orden;

            //_db.SaveChanges();
        }
    }
}
