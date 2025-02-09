using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SistemaCuentasBancarias.AccesoDatos.Data.Repository.IRepository;

namespace SistemaCuentasBancarias.AccesoDatos.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext Context;
        internal DbSet<T> dbSet;

        public Repository(DbContext contex)
        {
            Context = contex;
            this.dbSet = contex.Set<T>();
        }


        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, Func<IOrderedQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? includeProperties = null)
        {
            // Se crea una consulta IQueryable a partir del DbSet del contexto
            IQueryable<T> query = dbSet;

            // Se aplica el filtro si se proporciona
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Se incluyen propiedades de navegación si se proporciona
            // es decir, datos relacionados ej: como un profesor todos los estudiantes que tiene 
            if (includeProperties != null)
            {
                // Se divide la cadena de propiedades por coma y se sobre ellas
                foreach (var includeProperty in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            // Se aplica el ordenamiento si se proporciona
            if(orderBy != null)
            {
                // Se ejecuta la función de ordenamiento y se convierte la consulta en una lista
                return orderBy((IOrderedQueryable<T>)query).ToList();
            }

            // Si no se proporciona ordenamiento, simplemente se convierte la consulta en una lista
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            // Se crea una consulta IQueryable a partir del DbSet del contexto
            IQueryable<T> query = dbSet;

            // Se aplica el filtro si se proporciona
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Se incluyen propiedades de navegación si se proporciona
            // es decir, datos relacionados ej: como un profesor todos los estudiantes que tiene 
            if (includeProperties != null)
            {
                // Se divide la cadena de propiedades por coma y se sobre ellas
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return query.FirstOrDefault();
        }

        public void Remove(int id)
        {
            T entityToRemove = dbSet.Find(id);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }
    }
}
