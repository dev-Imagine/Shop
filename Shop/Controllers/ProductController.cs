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
        // GET: Product
        public ActionResult ProductList()
        {
            ViewBag.lstProductos = "";
            return View();
        }
        public ActionResult ProductDetail()
        {
            return View();
        }
        public PartialViewResult _ProductList(int pageIndex, int pageSize)
        {
            srvProduct sPro = new srvProduct();
            if (Session["lstProductos"] == null)
            {
                Session["lstProductos"] = sPro.obtenerProductos(pageIndex, pageSize);
            }
            else
            {
                Session["lstProductos"] = sPro.obtenerProductos(pageIndex, pageSize, (List<Producto>)Session["lstProductos"]);
            }
           
            return PartialView();
        }
    }
}