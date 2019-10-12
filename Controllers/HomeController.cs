using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SajaloZindagi.Models;

namespace SajaloZindagi.Controllers
{
    public class HomeController : Controller
    {
        private readonly sajaloZindagiContext db;
        public HomeController(sajaloZindagiContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            
            return View(db.Simages.Where(s => s.IsDeal == true).ToList());
        }

        public IActionResult About()
        {
            

            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
