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
            if (Session["Usuario"] != null)
            {
                srvCategories sCategoria = new srvCategories();
                ViewBag.lstCategoria = sCategoria.ObtenerCategorias();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            
            
            
        }
        public ActionResult SalesList()
        {
            srvVentas sVenta = new srvVentas();
            List<Venta> st = sVenta.ObtenerVentas(1);
            ViewBag.ventas = st;
            return View();

        }

        public ActionResult LogIn()
        {
            return View();

        }
        [HttpPost]
        public ActionResult LogIn(Usuario oUser)
        {
            srvUsers sUser = new srvUsers();
            Usuario oUs = new Usuario();
            oUs = sUser.logIn(oUser);
            if (oUs.idUsuario != 0)
            {
                Session["Usuario"] = oUs;
            }
            else
            {
                Session["Usuario"] = null;
            }
            return View();

        }
        //vistas parciales

    }
}