using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using webTeste02.Models;

namespace webTeste02.Controllers
{
    public class UserController : Controller
    {
        public async Task<ActionResult> IndexAsync()
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return Redirect("/Account/Login");
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("https://localhost:44353/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string getUrl = "api/usuario";
                if (HttpContext.Session.GetString("Nivel") != "Loja")
                {
                    getUrl = getUrl + "/" + HttpContext.Session.GetString("id");
                    HttpResponseMessage responseuser= await client.GetAsync(getUrl);
                    if (responseuser.IsSuccessStatusCode)
                    {  //GET
                        List<User> usuarios = new List<User>();
                        User usuario = await responseuser.Content.ReadAsAsync<User>();
                        usuarios.Add(usuario);
                        return View(usuarios);

                    }
                    else
                    {
                        return Json(new { status = false, message = "Invalid!" });
                    }

                }

                HttpResponseMessage response = await client.GetAsync(getUrl);
                if (response.IsSuccessStatusCode)
                {  //GET
                    List<User> usuarios = await response.Content.ReadAsAsync<List<User>>();
                    return View(usuarios);

                }
                else
                {
                    return Json(new { status = false, message = "Invalid!" });
                }
            }

        }
            public ActionResult Create()
            {
                if (HttpContext.Session.GetString("email") == null && HttpContext.Session.GetString("Nilve") !="Loja"  )
                {
                    return Redirect("/Account/Login");
                }

                return View();
            }
        [HttpPost]
        public async Task<ActionResult> CreateUserAsync(User user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("https://localhost:44353/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PostAsJsonAsync("api/Usuario", user);
            }
            return RedirectToAction("Index", "User");
        }
            
            [HttpPost]
            public async Task<bool> DeleteAsync(string id)
            {
                try
                {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri("https://localhost:44353/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.DeleteAsync(String.Format("api/Usuario/{0}", id));
                }

                }
                catch (System.Exception)
                {
                    return false;
                }
                return true;


        }
        public async Task<ActionResult> UpdateAsync(string id)
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return Redirect("/Account/Login");
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("https://localhost:44353/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(String.Format("api/usuario/{0}", id));
                if (response.IsSuccessStatusCode)
                {  //GET
                    User usuario = await response.Content.ReadAsAsync<User>();
                    if (usuario != null)
                    {
                        return View(usuario);

                    }
                    else
                    {
                        return Json(new { status = false, message = "Invalid!" });
                    }

                }
                else
                {
                    return Json(new { status = false, message = "Error StatusCode!" });
                }
            }

        }
        [HttpPost]
            public async Task<ActionResult> UpdateUserAsync(User user)
            {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("https://localhost:44353/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PutAsJsonAsync(String.Format("api/Usuario/{0}",user.id) , user);
            }
            
            return RedirectToAction("Index", "User");
            }
        
    }
}
