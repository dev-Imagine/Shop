using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shop.Models;

namespace Shop.Services
{
    public class srvProduct
    {
        public List<Producto> obtenerProductos(int pageIndex, int pageSize, string stSearch)
        {
            if (stSearch.Length <= 3){
                return new List<Producto>();
            }
            using (DB_A363ED_ShopEntities bd = new DB_A363ED_ShopEntities())
            {
                List<Producto> lstProductos = bd.Producto.Where(x=> (x.SubCategoria.nombre + x.SubCategoria.Categoria.nombre + x.nombre).ToUpper().Contains(stSearch.ToUpper())).ToList().Skip(pageIndex * pageSize).Take(pageSize).ToList();
                foreach (Producto opro in lstProductos)
                {
                    opro.Imagen.Where(x => x.principal = true).FirstOrDefault();
                }
                return lstProductos;
            }
            
        }
        public int TotalProductosBusqueda(string stSearch)
        {
            using (DB_A363ED_ShopEntities bd = new DB_A363ED_ShopEntities())
            {
                return bd.Producto.Where(x => (x.SubCategoria.nombre + x.SubCategoria.Categoria.nombre + x.nombre).ToUpper().Contains(stSearch.ToUpper())).Count();
            }

        }
        public Producto obtenerProducto(int idProducto)
        {
            using (DB_A363ED_ShopEntities bd = new DB_A363ED_ShopEntities())
            {
                try
                {
                    Producto oPro = bd.Producto.Where(x => x.idProducto == idProducto).FirstOrDefault();
                    oPro.Imagen.ToList();
                    int i = oPro.SubCategoria.Categoria.idCategoria;
                    return oPro;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private Producto GuardarProducto(Producto oProducto)
        {
            try
            {
                using (DB_A363ED_ShopEntities bd = new DB_A363ED_ShopEntities())
                {
                    oProducto.nombre = oProducto.nombre.ToUpper();
                    bd.Entry(oProducto).State = System.Data.Entity.EntityState.Added;
                    bd.SaveChanges();
                    return oProducto;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private Producto ModificarProducto(Producto oProducto)
        {
            try
            {
                using (DB_A363ED_ShopEntities bd = new DB_A363ED_ShopEntities())
                {
                    oProducto.nombre = oProducto.nombre.ToUpper();
                    bd.Entry(oProducto).State = System.Data.Entity.EntityState.Modified;
                    bd.SaveChanges();
                    return oProducto;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Producto GuardarModificarProducto(Producto oProducto)
        {
            try
            {
                oProducto.nombre = oProducto.nombre.ToUpper();
                
                if (oProducto.idProducto == 0)
                {
                    GuardarProducto(oProducto);
                }
                else
                {
                    ModificarProducto(oProducto);
                }
                return oProducto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void guardarImagen(int idProducto, string archivo)
        {
            try
            {
                using (DB_A363ED_ShopEntities bd = new DB_A363ED_ShopEntities())
                {
                    try
                    {
                        Imagen oImagen = new Imagen();
                        oImagen.idProducto = idProducto;
                        oImagen.archivo = archivo;
                        bd.Entry(oImagen).State = System.Data.Entity.EntityState.Added;
                        bd.SaveChanges();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}