﻿using Shop.Models;
using System;
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
        public List<Venta> ObtenerVentas(int idEstado)
        {
            try
            {
                List<Venta> lstVentas;
                using (DB_A363ED_ShopEntities bd = new DB_A363ED_ShopEntities())
                {
                    lstVentas = bd.Venta.Where(x=> x.idEstado == idEstado).ToList();
                    foreach (Venta oVenta in lstVentas)
                    {
                        int i = 0;
                        foreach (DetalleVenta oDetalle in oVenta.DetalleVenta)
                        {
                            i = oDetalle.Producto.idProducto;
                            i = oDetalle.Producto.Imagen.Count();
                        }
                    }
                }
                return lstVentas;
            }
            catch (Exception)
            {
                return new List<Venta>();
            }

        }
    }
}