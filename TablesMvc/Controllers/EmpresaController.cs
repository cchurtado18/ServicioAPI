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
    public class EmpresaController : Controller
    {
        private string baseURL = "http://localhost:60895/";

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Lista()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.GetAsync("/api/Empresa").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            List<EmpresaCLS> Empresa = JsonConvert.DeserializeObject<List<EmpresaCLS>>(data);

            return Json(
                new
                {
                    success = true,
                    data = Empresa,
                    message = "done"
                },
                JsonRequestBehavior.AllowGet
            );
        }

        public JsonResult Guardar(int IdEmpresa, string NombreEmpresa ,string Nacionalidad)
        {
            try
            {
                EmpresaCLS Empresa = new EmpresaCLS();
                Empresa.IdEmpresa = IdEmpresa;
                Empresa.NombreEmpresa = NombreEmpresa;
                Empresa.Nacionalidad = Nacionalidad;

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string regionJson = JsonConvert.SerializeObject(Empresa);
                HttpContent body = new StringContent(regionJson, Encoding.UTF8, "application/json");

                if (IdEmpresa == 0)
                {
                    HttpResponseMessage response = httpClient.PostAsync("/api/Empresa", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                            new
                            {
                                success = true,
                                message = "Empresa creada satisfactoriamente"
                            }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    HttpResponseMessage response = httpClient.PutAsync($"/api/Empresa/{IdEmpresa}", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                            new
                            {
                                success = true,
                                message = "Empresa modificada satisfactoriamente"
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

        public JsonResult Eliminar(int IdEmpresa)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.DeleteAsync($"/api/Empresa/{IdEmpresa}").Result;

            if (response.IsSuccessStatusCode)
            {
                return Json(
                    new
                    {
                        success = true,
                        message = "Empresa eliminada satisfactoriamente"
                    }, JsonRequestBehavior.AllowGet);
            }

            throw new Exception("Error al eliminar");
        }
    }
}