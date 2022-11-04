using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SifreKasasi.Login.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString("JWToken");
            return View();
        }
    }
}
