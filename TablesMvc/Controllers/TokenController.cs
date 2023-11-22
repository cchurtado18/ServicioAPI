﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TablesMvc.Models;

namespace WebCRUDapi.Controllers
{
    public class TokenController : Controller
    {
        private string baseUrl = "http://localhost:60895/";
        public ActionResult Index()
        {
            if (HttpContext.Session["token"] == null)
            {
                ViewBag.Message = "Presione login para autenticarse";
            }
            else
            {
                ViewBag.Message = "Presione logout para cerrar la sesión";
            }

            return View();
        }

        public ActionResult Login()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            User user = new User();

            user.Username = "admin";
            user.Password = "123456";

            string stringData = JsonConvert.SerializeObject(user);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync("/api/Token", contentData).Result;
            string stringJWT = response.Content.ReadAsStringAsync().Result;
            TokenCLS token = JsonConvert.DeserializeObject<TokenCLS>(stringJWT);

            HttpContext.Session.Add("token", token.AccessToken);

            ViewBag.Message = "Usuario Autenticado";

            return View("Index");
        }

        public ActionResult LogOut()
        {
            HttpContext.Session.Remove("token");
            ViewBag.Message = "Usuario ha salido de la sesión";

            return View("Index");
        }
    }
}