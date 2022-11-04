using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SifreKasasi.Login.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SifreKasasi.Login.Controllers
{
    public class SiteController : Controller
    {
        public static string baseUrl = "https://localhost:44358/api/Sites/";

        public async Task<IActionResult> Index()
        {
            var site = await GetSite();
            return View(site);
        }


        [HttpGet]
        public async Task<List<Site>> GetSite()
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = baseUrl;
            HttpClient client=new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string jsonstr = await client.GetStringAsync(url);
            var res=JsonConvert.DeserializeObject<List<Site>>(jsonstr).ToList();
            return res;

        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SId,SiteAdi,SPassword")] Site site)
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = baseUrl;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var stringContent = new StringContent(JsonConvert.SerializeObject(site), Encoding.UTF8, "application/json");
            await client.PostAsync(url, stringContent);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = baseUrl+id;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string jsonstr = await client.GetStringAsync(url);
            var res = JsonConvert.DeserializeObject<Site>(jsonstr);
            if (res == null)
            {
                return NotFound();

            }
            return View(res);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SId,SiteAdi,SPassword")] Site site)
        {
            if(id != site.SId)
            {
                return NotFound();

            }
            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = baseUrl+id;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var stringContent = new StringContent(JsonConvert.SerializeObject(site), Encoding.UTF8, "application/json");
            await client.PutAsync(url, stringContent);
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = baseUrl + id;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string jsonstr = await client.GetStringAsync(url);
            var res = JsonConvert.DeserializeObject<Site>(jsonstr);
            if (res == null)
            {
                return NotFound();

            }
            return View(res);

        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = baseUrl + id;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            await client.DeleteAsync(url);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id == null)
            {
                return NotFound();

            }
            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = baseUrl + id;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string jsonstr = await client.GetStringAsync(url);
            var site = JsonConvert.DeserializeObject<Site>(jsonstr);
            if (site == null)
            {
                return NotFound();

            }
            return View(site);
        }

    }
}
