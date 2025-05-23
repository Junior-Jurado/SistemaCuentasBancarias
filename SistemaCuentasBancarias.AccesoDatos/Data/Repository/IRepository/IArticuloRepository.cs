﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaCuentasBancarias.Models;

namespace SistemaCuentasBancarias.AccesoDatos.Data.Repository.IRepository
{
    public interface IArticuloRepository : IRepository<Articulo>
    {
        void Update(Articulo articulo);

        // Método para el buscador
        IQueryable<Articulo> AsQueryable();
    }
}
