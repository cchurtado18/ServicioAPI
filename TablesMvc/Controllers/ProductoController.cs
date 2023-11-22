using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using TablesMvc.Models;
using Newtonsoft.Json;
using System.Text;

namespace TablesMvc.Controllers
{
    public class ProductoController : Controller
    {
        private string baseURL = "http://localhost:60895/";
        // GET: Carrera
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Lista()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.GetAsync("/api/Producto").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            List<ProductoCLS> productos = JsonConvert.DeserializeObject<List<ProductoCLS>>(data);

            return Json(
                new
                {
                    success = true,
                    data = productos,
                    message = "done"
                },
                JsonRequestBehavior.AllowGet
                );
        }

        public JsonResult Guardar(string Idproducto, string Nombre, string Color, string Marca)
        {

            try
            {
                ProductoCLS productos = new ProductoCLS();
                productos.Idproducto = Idproducto;
                productos.Nombre = Nombre;
                productos.Color = Color;
                productos.Marca = Marca;

                Console.WriteLine("id " + productos.Idproducto + " Nombre " + productos.Nombre + " Color " + productos.Color +   " Marca " + productos.Marca);

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string productosJson = JsonConvert.SerializeObject(productos);
                HttpContent body = new StringContent(productosJson, Encoding.UTF8, "application/json");

                HttpResponseMessage findIdResponse = httpClient.GetAsync($"/api/producto/{Idproducto}").Result;

                if (!findIdResponse.IsSuccessStatusCode)
                {
                    HttpResponseMessage response = httpClient.PostAsync("/api/producto", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                            new
                            {
                                success = true,
                                message = "Producto creado satisfactoriamente"
                            }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    HttpResponseMessage response = httpClient.PutAsync($"/api/producto/{Idproducto}", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                            new
                            {
                                success = true,
                                message = "Producto modificado satisfactoriamente"
                            }, JsonRequestBehavior.AllowGet);
                    }
                }
                throw new Exception("Error al guardar");
            }

            catch (Exception ex)
            {
                return Json(
                    new
                    {
                        success = false,
                        message = ex.InnerException
                    }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult Eliminar(string Idproducto)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.DeleteAsync($"/api/producto/{Idproducto}").Result;

            if (response.IsSuccessStatusCode)
            {
                return Json(
                    new
                    {
                        success = true,
                        message = "producto eliminado satisfactoriamente"
                    }, JsonRequestBehavior.AllowGet);
            }

            throw new Exception("Error al eliminar");
        }
    }
}