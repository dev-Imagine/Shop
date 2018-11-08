using System;
using System.Collections;
using System.Linq;
using System.Web.Mvc;
using Shop.Models;
using Shop.Services;
using mercadopago;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Shop.Controllers
{
    public class SellController : Controller
    {
        public ActionResult Cart()
        {
            Venta oVenta = (Venta)Session["Cart"];
            if (oVenta != null && oVenta.DetalleVenta.Count() >= 1)
            {
                ViewBag.Venta = oVenta;
            }
            return View();
        }
        public ActionResult EndPurchase()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PaymentNotification(string topic, string id)
        {
            // URL Notificacion
            // http://localhost:64615/Sell/PaymentNotification/?topic=payment&id=4205580523

            try
            {
                if (topic == "payment")
                {
                    MP mp = new MP(srvConfig.MP_client_id(), srvConfig.MP_client_secret());
                    // get info pago --> "Payment"
                    Hashtable Payment = mp.getPaymentInfo(id);
                    Payment = ((Hashtable)Payment["response"]);
                    string orderID = ((Hashtable)Payment["collection"])["order_id"].ToString();
                    if (((Hashtable)Payment["collection"])["status"].ToString() == "approved")
                    {
                        // get info order --> "Order"
                        string baseUrl = "https://api.mercadopago.com";
                        string url = "merchant_orders/" + orderID + "?access_token=" + srvConfig.MP_client_access_token();
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

                                if (Order["status"].ToString() == "closed")
                                {
                                    Venta oVenta = new Venta();
                                    //oVenta.emailComprador
                                    oVenta.MP_state_shipments = "";
                                    oVenta.MP_order_id = Order["id"].ToString();
                                    oVenta.MP_state_order = Order["status"].ToString();
                                    oVenta.fecha = DateTime.Now;
                                    oVenta.montoTotal = Convert.ToInt32(Order["total_amount"].ToString());

                                    if (Order["shipments"].ToString() != null)
                                    {
                                        JToken Shipment = Order["shipments"].First;
                                        JToken receiver_address = Shipment["receiver_address"];
                                        JToken city = receiver_address["city"];
                                        JToken state = receiver_address["state"];
                                        oVenta.direccionShipments = receiver_address["address_line"].ToString() + ", " + city["name"].ToString() + ", " + state["name"].ToString() + ", CP:" + receiver_address["zip_code"].ToString();
                                        oVenta.MP_state_shipments = Shipment["status"].ToString();
                                        // oVenta.idLocalidad = city["name"].ToString();
                                    }

                                    DetalleVenta oDetalleVenta;
                                    JToken items = Order["items"];
                                    foreach (JToken item in items)
                                    {
                                        oDetalleVenta = new DetalleVenta();
                                        oDetalleVenta.idProducto = Convert.ToInt32(item["id"]);
                                        oDetalleVenta.cantidad = Convert.ToInt32(item["quantity"]);
                                        oDetalleVenta.precioUnitario = Convert.ToInt32(item["unit_price"]);
                                        oDetalleVenta.subTotal = oDetalleVenta.cantidad * oDetalleVenta.precioUnitario;
                                        oDetalleVenta.descuento = Convert.ToInt32(item["category_id"]);
                                        oVenta.DetalleVenta.Add(oDetalleVenta);
                                    }
                                    // Guardar
                                    srvVentas sVenta = new srvVentas();
                                    sVenta.GuardarVenta(oVenta);
                                }
                            }
                        }
                    }
                }
                return View();
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
        // METODOS
        [HttpPost]
        public JsonResult AddProduct(int idProducto, int cantidad)
        {
            try
            {
                Venta oVenta = (Venta)Session["Cart"];
                if (oVenta == null) {
                    oVenta = new Venta();
                }

                if (oVenta.DetalleVenta.Where(x=> x.idProducto == idProducto).Count() == 1)
                {
                    if (oVenta.DetalleVenta.Where(x => x.idProducto == idProducto).FirstOrDefault().Producto.stockActual < (cantidad + oVenta.DetalleVenta.Where(x => x.idProducto == idProducto).FirstOrDefault().cantidad))
                    {
                        return Json("Stock insuficiente.");
                    }

                    oVenta.DetalleVenta.Where(x => x.idProducto == idProducto).FirstOrDefault().cantidad += cantidad;
                    oVenta.DetalleVenta.Where(x => x.idProducto == idProducto).FirstOrDefault().subTotal += (oVenta.DetalleVenta.Where(x => x.idProducto == idProducto).FirstOrDefault().Producto.precio * cantidad) - Convert.ToDecimal(((oVenta.DetalleVenta.Where(x => x.idProducto == idProducto).FirstOrDefault().Producto.precio * cantidad) * oVenta.DetalleVenta.Where(x => x.idProducto == idProducto).FirstOrDefault().Producto.descuento) / 100);
                   
                    oVenta.montoTotal = oVenta.DetalleVenta.Sum(x => x.subTotal);
                }
                else
                {
                    srvProduct sProduct = new srvProduct();
                    Producto oProducto = sProduct.obtenerProducto(idProducto);

                    if (oProducto.stockActual < cantidad)
                    {
                        return Json("Stock insuficiente.");
                    }
                    DetalleVenta oDetalle = new DetalleVenta();
                    oDetalle.Producto = oProducto; // <---------------- Solo para mostrar
                    oDetalle.idProducto = oProducto.idProducto;
                    oDetalle.precioUnitario = oProducto.precio;
                    oDetalle.descuento = oProducto.descuento;
                    oDetalle.subTotal = (oProducto.precio * cantidad) - Convert.ToDecimal(((oProducto.precio * cantidad) * oDetalle.descuento) / 100);
                    oDetalle.cantidad = cantidad;
                    oVenta.DetalleVenta.Add(oDetalle);
                    oVenta.montoTotal += oDetalle.subTotal;
                    Session["Cart"] = oVenta;
                }


                return Json("true");
            }
            catch (Exception)
            {
                return Json("Ocurrió un erro al agregar el producto.");
            }
        }
        public JsonResult RemoveProduct (int idProducto)
        {
            try
            {
                Venta oVenta = (Venta)Session["Cart"];
                oVenta.DetalleVenta.Remove(oVenta.DetalleVenta.Where(x=> x.idProducto== idProducto).FirstOrDefault());
                Session["Cart"] = oVenta;
                return Json(oVenta.DetalleVenta.Sum(x=>x.cantidad));
            }
            catch (Exception)
            {
                return Json("Ocurrió un erro al quitar el producto.");
            }
        }
        [HttpPost]
        public ActionResult PayMercadoPago()
        {
            try
            {
                Venta oVenta = (Venta)Session["Cart"];
                if (oVenta == null || oVenta.DetalleVenta.Count == 0)
                {
                    return RedirectToAction("Cart", "Sell");
                }
                string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                MP mp = new MP(srvConfig.MP_client_id(), srvConfig.MP_client_secret());
                String preferenceData = "{\"items\":[";
                foreach (DetalleVenta oDetalleVenta in oVenta.DetalleVenta)
                {
                    // Item
                    preferenceData += "{" +
                            "\"title\":\"" + oDetalleVenta.Producto.nombre + "\"," +
                            "\"quantity\":" + oDetalleVenta.cantidad + "," +
                            "\"currency_id\":\"ARS\"," +
                            "\"category_id\":" + oDetalleVenta.descuento + "," +
                            "\"unit_price\":" + Convert.ToUInt32((oDetalleVenta.precioUnitario) - ((oDetalleVenta.precioUnitario * oDetalleVenta.Producto.descuento) / 100)) + "," +
                            "\"id\":" + oDetalleVenta.idProducto + "," +
                            "\"picture_url\":\"" + baseUrl + "Images/Product/"+ oDetalleVenta.Producto.Imagen.Where(x=> x.principal == true).FirstOrDefault().archivo + "\"," +
                            "},";
                }
                string stDimensions = srvMetodosGenericos.GetDimensionsProducts(oVenta.DetalleVenta.ToList());
                preferenceData +=  "]," +
                    "\"shipments\":{" +
                        "\"mode\":\"me2\"," +
                        "\"dimensions\":\"" + stDimensions + "\"," +
                        "\"local_pickup\":true," +
                        //"\"free_methods\":[" +
                        //    "{\"id\":73328}" +
                        //"]," +
                    "\"default_shipping_method\":73328," +
                    "\"zip_code\":\"5900\"" +
                    "}," +
                    "\"back_urls\":{" +
                            "\"success\": \"" + baseUrl + "Sell/EndPurchase" + "\"" +
                    "}," +
                    "\"auto_return\":\"approved\"," +
                    "}";

                Hashtable preference = mp.createPreference(preferenceData);
                string linkMP = ((Hashtable)preference["response"])["init_point"].ToString();
                return Redirect(linkMP);
            }
            catch (Exception)
            {
                Session["Cart"] = null;
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public JsonResult ShippingCost(string zip_code)
        {
            try
            {
                string stShippingCosts = "";
                Venta oVenta = (Venta)Session["Cart"];
                string stDimensions = srvMetodosGenericos.GetDimensionsProducts(oVenta.DetalleVenta.ToList());

                string baseUrl = "https://api.mercadolibre.com";
                string url = "sites/MLA/shipping_options?zip_code_from=5900&zip_code_to=" + zip_code + "&dimensions=" + stDimensions;

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
                        JObject ShippingCost = JObject.Parse(readTask.Result);
                        stShippingCosts = "<hr />";
                        foreach (JToken optionShipping in ShippingCost["options"])
                        {
                            stShippingCosts += "<p class=\"text-muted\" style=\"padding-left: 10%; padding-right: 10%;\">" + optionShipping["name"].ToString() + "<span class=\"pull-right\">$"+ optionShipping["cost"].ToString() + "</span></p>";                           
                            stShippingCosts += "<hr />";
                        }
                    }
                }
                return Json(stShippingCosts);
            }
            catch (Exception)
            {
                return Json("<hr /><br /> ");
            }
        }
    }
}