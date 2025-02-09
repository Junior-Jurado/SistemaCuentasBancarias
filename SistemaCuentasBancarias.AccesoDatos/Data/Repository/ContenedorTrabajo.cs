using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaCuentasBancarias.AccesoDatos.Data.Repository.IRepository;
using SistemaCuentasBancarias.Data;

namespace SistemaCuentasBancarias.AccesoDatos.Data.Repository
{
    public class ContenedorTrabajo : IContenedorTrabajo
    {
        private readonly ApplicationDbContext _db;

        public ContenedorTrabajo(ApplicationDbContext db)
        {  
            _db = db;
            Servicio = new ServicioRepository(_db);
        }

        public IServicioRepository Servicio {  get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
