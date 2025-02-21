using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemaCuentasBancarias.Models;

namespace SistemaCuentasBancarias.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Poner aquí todos los modelos
        public DbSet<Servicio> Servicio { get; set; }
        public DbSet<Articulo> Articulo { get; set; }
    }
}
