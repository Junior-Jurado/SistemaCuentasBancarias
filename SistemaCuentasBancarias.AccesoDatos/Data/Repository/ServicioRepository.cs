using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void Update(Servicio servicio)
        {
            var objDesdeDb = _db.Servicio.FirstOrDefault(s => s.Id == servicio.Id);
            objDesdeDb.Nombre = servicio.Nombre;
            objDesdeDb.Orden = servicio.Orden;

            _db.SaveChanges();
        }
    }
}
