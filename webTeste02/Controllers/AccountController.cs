using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using webTeste02.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace webTeste02.Controllers
{
    public class AccountController : Controller
    {

        public IActionResult Login()
        {


            return View();

        }
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }


        public async Task<ActionResult> ValidateAsync(User admin)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("https://localhost:44353/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(String.Format("api/usuario/getByEmail/?email={0}", admin.email));
                if (response.IsSuccessStatusCode)
                {  //GET
                    User usuario = await response.Content.ReadAsAsync<User>();
                    if (usuario!= null)
                    {
                        if (usuario.Senha == admin.Senha)
                        {
                            HttpContext.Session.SetString("email", usuario.email);
                            HttpContext.Session.SetString("id", usuario.id);
                            HttpContext.Session.SetString("Nivel", usuario.Nivel);
                            HttpContext.Session.SetString("Nome", usuario.Nome);

                            return Json(new { status = true, nivel = usuario.Nivel, message = "Login Successfull!" });
                        }
                        else
                        {
                            return Json(new { status = false, message = "Invalid Password!" });
                        }
                    }
                    else
                    {
                        return Json(new { status = false, message = "Invalid Email!" });
                    }

                }
                else
                {
                    return Json(new { status = false, message = "Error StatusCode!" });
                }
            }
        }
    }


}
