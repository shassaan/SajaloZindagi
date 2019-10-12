using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SajaloZindagi.Models;

namespace SajaloZindagi.Controllers
{
    public class UserController : Controller
    {

        private readonly sajaloZindagiContext db;
        private readonly IHostingEnvironment hostingEnvironment;
        public UserController(sajaloZindagiContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }
        public IActionResult Index()
        {
            AdminController adminController = new AdminController(db,hostingEnvironment);
            ViewBag.Cart = getCountOfCart();
            return View("MainEvents", adminController.Events());
        }


        public IActionResult imgDetails(int id)
        {
            try
            {
                var image =  db.Simages.Find(id);
                return View("ImageDetails",image);
            }
            catch (Exception) {
                return View("Index");
            }
        }

        public IActionResult checkout()
        {
            if (HttpContext.Session.GetString("Cart") != null)
            {
                ViewBag.Cart = getCountOfCart();
                if (ViewBag.Cart == 0)
                {
                    ViewBag.Cart = 0;
                    return View("checkout",null);
                }
                return View("checkout", JsonConvert.DeserializeObject<List<Simages>>(HttpContext.Session.GetString("Cart")));
            }
            ViewBag.Cart = getCountOfCart();
            return View("checkout");
        }

        public IActionResult AllServices()
        {
            try
            {
                var list = db.Services.Where(s => s.SIsAvailable == true).ToList();
                    ViewBag.Cart = getCountOfCart();
                    return View("Services",list);
            }
            catch (Exception)
            {
                return Forbid();
            }
        
        }

        public IActionResult showImages(int id)
        {
            try
            {
                var listOfImages = db.Simages.Where(i => i.SId == id && i.IsDeal == false).ToList();
                return View("Images",listOfImages);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }

        public IActionResult AllPakages()
        {
            try
            {
                var listOfImages = db.Simages.Where(i => i.IsDeal == true).ToList();
                return View("AllPakages", listOfImages);
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public IActionResult checkout([Bind("OEmail", "OPh", "VId", "OName", "ODescr","OFuncTime","ONoOfGuests","OFuncType")]Orders orders, int[] images)
        {
            ViewBag.error = 0;
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var image in images)
                    {
                        db.Orders.Add(new Orders
                        {
                            OEmail = orders.OEmail,
                            OPh = orders.OPh,
                            VId = orders.VId,
                            ImgId = image,
                            ODate = DateTime.Now,
                            OReadStatus = false,
                            OName = orders.OName,
                            ODescr = orders.ODescr,
                            OFuncTime = orders.OFuncTime,
                            ONoOfGuests = orders.ONoOfGuests,
                            OFuncType = orders.OFuncType
                        });

                        int effRows = db.SaveChanges();
                        if (effRows < 1)
                        {
                            ViewBag.error = 2;
                        }
                    }
                }
                else
                {
                    ViewBag.error = 3;
                }
            }
            catch (Exception)
            {
                ViewBag.error = 2;
            }

            if (ViewBag.error == 0)
            {
                HttpContext.Session.Remove("Carts");
            }
            return View("Checkout");
        }

        public IActionResult removeFromCart(int id)
        {
            if (HttpContext.Session.GetString("Cart") != null)
            {
                var items = JsonConvert.DeserializeObject<List<Simages>>(HttpContext.Session.GetString("Cart"));

                HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(items.Where(i => i.ImgId != id).ToList()));
                return RedirectToAction("checkout");
            }

            return RedirectToAction("checkout");
        }

        public int getCountOfCart()
        {
            if (HttpContext.Session.GetString("Cart") != null)
            {
                return JsonConvert.DeserializeObject<List<Simages>>(HttpContext.Session.GetString("Cart")).Count;
            }
            return 0;
        }


        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            try
            {
                if (HttpContext.Session.GetString("Cart") != null)
                {
                    var list = JsonConvert.DeserializeObject<List<Simages>>(HttpContext.Session.GetString("Cart"));
                    if (list != null)
                    {
                        
                        Simages simages = db.Simages.Find(id);
                        foreach (var item in list)
                        {
                            if (item.ImgId == id)
                            {
                                return NoContent();
                            }
                        }

                        list.Add(db.Simages.Find(id));
                        HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(list));
                        return Json(1);
                    }
                }
                else
                {
                    List<Simages> list = new List<Simages>();
                    list.Add(db.Simages.Find(id));
                    HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(list));
                    return Json(1);
                }

                return NoContent();
            }
            catch (Exception)
            {
                return Forbid();
            }
        }

        public IActionResult SubEvents(int id)
        {
            try
            {
                ViewBag.Cart = getCountOfCart();
                return View("SubEvents", db.SubEvents.Where(s => s.EId == id).ToList());
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }



        [HttpPost]
        public IActionResult Services(int[] subevents)
        {
            try
            {
                if (subevents != null)
                {
                    List<Services> list = new List<Services>();
                    foreach (var id in subevents)
                    {
                        var query =  (from s in db.Services
                                    join es in db.EventServices on s.SId equals es.SId
                                    join se in db.SubEvents on es.SeId equals se.SeId
                                    where se.SeId == id && s.SIsAvailable == true
                                    select  new { s });
                        foreach (var q in query)
                        {
                            Services s = new Models.Services
                            {
                                SId = q.s.SId,
                                SName = q.s.SName.TrimEnd(),
                                SPrice = q.s.SPrice,
                                SDp = q.s.SDp.TrimEnd(),
                                SIsAvailable = q.s.SIsAvailable,
                                SDetail = q.s.SDetail.TrimEnd()
                            };
                            
                                list.Add(s);
                        }
                    }
                    ViewBag.Cart = getCountOfCart();
                    var newList = RemoveDuplicates(list);
                    return View("Services",newList);
                    
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception)
            {
                return Forbid();
            }
        }

        public List<Services> RemoveDuplicates(List<Services> services)
        {
            List<Services> result = new List<Services>();
            foreach (var item in services)
            {
                if (!result.Exists(s => s.SId == item.SId))
                {
                    result.Add(item);
                }
            }

            return result;
        }
    }
}