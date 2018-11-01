using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shop.Models;
using Shop.Services;

namespace Shop.Controllers
{
    public class ProductController : Controller
    {
        // VISTAS
        [HttpPost]
        public ActionResult ProductList(string stSearch)
        {
            srvProduct sPro = new srvProduct();
            ViewBag.cantProductos = sPro.TotalProductosBusqueda(stSearch);
            ViewBag.search = stSearch;
            srvCategories sCategoria = new srvCategories();
            ViewBag.lstCategorias = sCategoria.ObtenerCategorias();
            return View();
        }
        public ActionResult ProductDetail(int idProducto)
        {
            srvProduct sProd = new srvProduct();
            ViewBag.oProducto = sProd.obtenerProducto(idProducto);
            return View();
        }
        
        // VISTAS PARCIALES

        public ActionResult _ProductList(int pageIndex, int pageSize, string stSearch)
        {
            srvProduct sPro = new srvProduct();
            int cantProductos = sPro.TotalProductosBusqueda(stSearch);
            if (pageIndex * pageSize >= cantProductos)
            {
                ViewBag.complete = true;
                ViewBag.lstProductos = new List<Producto>();
            }
            else
            {
                ViewBag.complete = false;
                ViewBag.lstProductos = sPro.obtenerProductos(pageIndex, pageSize, stSearch);
            }
            return PartialView();
        }
    }
}