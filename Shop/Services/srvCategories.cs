using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shop.Models;

namespace Shop.Services
{
    public class srvCategories
    {
        public List<Categoria> ObtenerCategorias()
        {
            using (DB_A363ED_ShopEntities bd = new DB_A363ED_ShopEntities())
            {
                try
                {
                    List<Categoria> lstCategoria = bd.Categoria.ToList();
                    foreach (Categoria oCategoria in lstCategoria)
                    {
                        oCategoria.SubCategoria.Count();
                    }
                    return lstCategoria;
                }
                catch (Exception)
                {
                    return new List<Categoria>();
                }
            }

        }
    }
}