using Shop.Models;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Services
{
    public class srvVentas
    {
        public Venta GuardarVenta(Venta oVenta)
        {
            using (DB_A363ED_ShopEntities bd = new DB_A363ED_ShopEntities())
            {
                bd.Entry(oVenta).State = System.Data.Entity.EntityState.Added;
                bd.SaveChanges();
                return oVenta;
            }

        }
    }
}