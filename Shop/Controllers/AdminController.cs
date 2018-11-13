using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using mercadopago;
using Newtonsoft.Json.Linq;
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
            if (Session["Usuario"] != null)
            {
                srvVentas sVenta = new srvVentas();
                List<Venta> st = sVenta.ObtenerVentas(1);
                ViewBag.ventas = st;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
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
            if (oUs != null)
            {
                Session["Usuario"] = oUs;
            }
            else
            {
                Session["Usuario"] = null;
            }
            return View();

        }
        // METODOS
        [HttpPost]
        public JsonResult CancelSale(string order_id)
        {
            try
            {

                MP mp = new MP(srvConfig.MP_client_id(), srvConfig.MP_client_secret());
                // get info order --> "Order"
                string baseUrl = "https://api.mercadopago.com";
                string url = "merchant_orders/" + order_id + "?access_token=" + srvConfig.MP_client_access_token();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    var responseTask = client.GetAsync(url);
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsStringAsync();
                        readTask.Wait();
                        JObject Order = JObject.Parse(readTask.Result);
                        JToken payments = Order["payments"];
                        foreach (JToken payment in payments)
                        {
                            if (payment["status"].ToString() == "approved")
                            {
                                mp.cancelPayment(payment["id"].ToString());
                            }
                        }
                        srvVentas sVentas = new srvVentas();
                        sVentas.CancelarVenta(order_id);
                    }
                }
                return Json("true");
            }
            catch (Exception)
            {
                return Json("false");
            }
            
        }
    }
}