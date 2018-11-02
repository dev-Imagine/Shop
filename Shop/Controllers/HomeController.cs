using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shop.Services;
using Shop.Models;

namespace Shop.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            srvCategories sCat = new srvCategories();
            ViewBag.lstCategories = sCat.ObtenerCategorias();
            return View();
            
        }
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
      
        public ActionResult Services()
        {
            return View();
        }
    }
}