using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shop.Models;
using Shop.Services;
namespace Shop.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult ViewProduct()
        {
            srvCategories sCategoria = new srvCategories();
            ViewBag.lstCategoria = sCategoria.ObtenerCategorias();
            return View();
            
        }

        //vistas parciales
        
    }
}