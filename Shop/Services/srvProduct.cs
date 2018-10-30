using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shop.Models;

namespace Shop.Services
{
    public class srvProduct
    {
        public List<Producto> obtenerProductos(int pageIndex, int pageSize, List<Producto> lstProductos = null)
        {
            using (DB_A363ED_ShopEntities bd = new DB_A363ED_ShopEntities())
            {
                if (lstProductos == null)
                {
                    lstProductos = bd.Producto.ToList();
                    
                }
                lstProductos = bd.Producto.ToList().Skip(pageIndex * pageSize).Take(pageSize).ToList();
                foreach (Producto opro in lstProductos)
                {
                    opro.Imagen.First();
                }
                return lstProductos;
            }
            
        }
    }
}