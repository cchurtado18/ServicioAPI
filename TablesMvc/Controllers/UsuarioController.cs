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
    public class UsuarioController : Controller
    {
        private string baseURL = "http://localhost:60895/";
        // GET: usuario
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Lista()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.GetAsync("/api/Usuario").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            List<UsuarioCLS> usuario = JsonConvert.DeserializeObject<List<UsuarioCLS>>(data);

            return Json(
                new
                {
                    success = true,
                    data = usuario,
                    message = "done"
                },
                JsonRequestBehavior.AllowGet
                );
        }

        public JsonResult Guardar(string IdUsuario, string Nombre, string Apellido, string Ciudad)
        {

            try
            {
                UsuarioCLS usuario = new UsuarioCLS();
                usuario.IdUsuario = IdUsuario;
                usuario.Nombre = Nombre;
                usuario.Apellido = Apellido;
                usuario.Ciudad= Ciudad;
                Console.WriteLine("id " + usuario.IdUsuario + " Nombre " + usuario.Nombre + " Apellido " + usuario.Apellido + " Ciudad " + usuario.Ciudad);

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string productosJson = JsonConvert.SerializeObject(usuario);
                HttpContent body = new StringContent(productosJson, Encoding.UTF8, "application/json");

                HttpResponseMessage findIdResponse = httpClient.GetAsync($"/api/usuario/{IdUsuario}").Result;

                if (!findIdResponse.IsSuccessStatusCode)
                {
                    HttpResponseMessage response = httpClient.PostAsync("/api/usuario", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                            new
                            {
                                success = true,
                                message = "Usuario creado satisfactoriamente"
                            }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    HttpResponseMessage response = httpClient.PutAsync($"/api/usuario/{IdUsuario}", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                            new
                            {
                                success = true,
                                message = "Usuario modificado satisfactoriamente"
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

        public JsonResult Eliminar(string IdUsuario)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.DeleteAsync($"/api/usuario/{IdUsuario}").Result;

            if (response.IsSuccessStatusCode)
            {
                return Json(
                    new
                    {
                        success = true,
                        message = "Usuario eliminado satisfactoriamente"
                    }, JsonRequestBehavior.AllowGet);
            }

            throw new Exception("Error al eliminar");
        }
    }
}