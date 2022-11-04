using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SifreKasasi.Login.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SifreKasasi.Login.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public static string baseUrl = "https://localhost:44358/api/Kayits/";
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> LoginUser(Kayit kayit)
        {
            using (var httpClient =new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(kayit), Encoding.UTF8, "application/json");
                using (var response =await httpClient.PostAsync("https://localhost:44358/api/token/", content))
                {
                    string token=await response.Content.ReadAsStringAsync();
                    if(token=="Invalid Credentials")
                    {
                        // ViewBag.Message = "KullanıcıAdı veya Password geçerli değil! ";
                        
                        return Redirect("~/Home/Index");
                    }
                    
                    HttpContext.Session.SetString("JWToken", token);
                    
                }
                return Redirect("~/Dashboard/Index");
            }
        }

        public IActionResult LogOff()
        {
            HttpContext.Session.Clear();
            return Redirect("~/Home/Index");
        }


        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KullaniciAdi,Email,Ad,Soyad,Password")] Kayit kayit)
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = baseUrl;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var stringContent = new StringContent(JsonConvert.SerializeObject(kayit), Encoding.UTF8, "application/json");
            await client.PostAsync(url, stringContent);
            return RedirectToAction(nameof(Index));
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
