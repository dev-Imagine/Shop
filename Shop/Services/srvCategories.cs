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

        public Categoria ObtenerCategoria(int idCategoria)
        {
            using (DB_A363ED_ShopEntities bd = new DB_A363ED_ShopEntities())
            {
                try
                {
                    Categoria oCategoria = bd.Categoria.Where(x=> x.idCategoria == idCategoria).FirstOrDefault();
                    foreach (SubCategoria oSub in oCategoria.SubCategoria)
                    {
                        oCategoria.SubCategoria.Count();
                    }
                    return oCategoria;
                }
                catch (Exception)
                {
                    return new Categoria();
                }
            }

        }

        public List<SubCategoria> ObtenerSubCategorias(int idCategoria)
        {
            using (DB_A363ED_ShopEntities bd = new DB_A363ED_ShopEntities())
            {
                try
                {
                    List<SubCategoria> lstSubCategoria = new List<SubCategoria>();
                    if (idCategoria == 0)
                    {
                        lstSubCategoria = new List<SubCategoria>();
                    }
                    else
                    {
                        lstSubCategoria = bd.SubCategoria.Where(x => x.idCategoria == idCategoria).ToList();
                    }
                    
                    
                    return lstSubCategoria;
                }
                catch (Exception)
                {
                    return new List<SubCategoria>();
                }
            }

        }

        public Categoria GuardarModificarCategoria(Categoria oCategoria)
        {
            if (oCategoria.idCategoria == 0)
            {
                oCategoria = GuardarCategoria(oCategoria);
            }
            else
            {
                oCategoria = ModificarCategoria(oCategoria); //falta actualizar detalle
            }
            return oCategoria;
        }
        private Categoria GuardarCategoria(Categoria oCategoria)
        {
            try
            {
                using (DB_A363ED_ShopEntities bd = new DB_A363ED_ShopEntities())
                {
                    bd.Entry(oCategoria).State = System.Data.Entity.EntityState.Added;
                    bd.SaveChanges();
                    return oCategoria;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private Categoria ModificarCategoria(Categoria oCategoria)
        {
            try
            {
                using (DB_A363ED_ShopEntities bd = new DB_A363ED_ShopEntities())
                {
                    List<SubCategoria> lstSubcategoria = oCategoria.SubCategoria.ToList();
                    oCategoria.SubCategoria.Clear();
                    bd.Entry(oCategoria).State = System.Data.Entity.EntityState.Modified;
                    foreach (SubCategoria oSubCategoria in lstSubcategoria)
                    {
                        if (oSubCategoria.idSubCategoria == 0)
                        {
                            bd.Entry(oSubCategoria).State = System.Data.Entity.EntityState.Added;
                        }
                        else
                        {
                            bd.Entry(oSubCategoria).State = System.Data.Entity.EntityState.Modified;
                        }
                    }
                    bd.SaveChanges();
                    return oCategoria;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void EliminarSubcategoria(int idSubCategoria)
        {
            try
            {
                using (DB_A363ED_ShopEntities bd = new DB_A363ED_ShopEntities())
                {
                    SubCategoria oSubcategoria = bd.SubCategoria.Where(x => x.idSubCategoria == idSubCategoria).FirstOrDefault();
                    if (oSubcategoria.Producto.Count == 0)
                    {
                        bd.SubCategoria.Remove(oSubcategoria);
                        bd.SaveChanges();
                    }
                    else
                    {
                        throw new Exception();
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}