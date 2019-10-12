using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SajaloZindagi.CustomClasses;
using SajaloZindagi.Models;
using StackExchange.Redis;

namespace SajaloZindagi.Controllers
{
    
    public class AdminController : Controller
    {
        private readonly sajaloZindagiContext db;
        private readonly IHostingEnvironment hostingEnvironment;
        public AdminController(sajaloZindagiContext _db,IHostingEnvironment _hostingEnvironment) {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }
        [SessionCheck]
        public IActionResult CreateVolunteer()
        {
            return View("AddVolunteer");
        }
        [SessionCheck]
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        [HttpPost]
        [SessionCheck]
        public IActionResult CreateVolunteer([Bind("VName","VAddress","VContact")]Volunteer volunteer)
        {
            ViewBag.error = 0;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Volunteer.Add(volunteer);
                    int effRows = db.SaveChanges();
                    if (effRows < 1)
                    {
                        ViewBag.error = 2;
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

            return View("AddVolunteer");
        }
        [SessionCheck]
        public IActionResult Index()
        {
            
                var a = HttpContext.Session.GetString("Admin");


            if (a.Contains("true"))
            {
                ViewBag.access = true;
            } else
            {
                ViewBag.access = false;
            }
            
            var services = (from o in db.Orders
                            join s in db.Simages
                            on o.ImgId equals s.ImgId
                            where o.OReadStatus == false
                            group o by new { o.OPh } into g
                            select new CustomOrder { count = g.Count(), OPh = g.Key.OPh }).ToList<CustomOrder>();
            return View(services);  
        }

        public IActionResult login()
        {
            return View("AdminLogin");
        }
        [SessionCheck]
        public IActionResult detOrder(string id)
        {

            try
            {

                var services = (from s in db.Simages
                                join o in db.Orders
                                on s.ImgId equals o.ImgId
                                where o.OReadStatus == false && o.OPh.Equals(id)
                                select new Customdetails
                                {
                                    ImgName = s.ImgName,
                                    ImgPath = s.ImgPath,
                                    ImgPrice = s.ImgPrice,
                                    VId = o.VId,
                                    OId = o.OId,
                                    Oname = o.OName,
                                    date = o.ODate,
                                    description = o.ODescr,
                                    NoOfGuests = o.ONoOfGuests,
                                    FuncTime = o.OFuncTime,
                                    FuncType = o.OFuncType
                                })
                                .ToList<Customdetails>();

                var orders = db.Orders.Where(o => o.OPh.Equals(id) && o.OReadStatus == false);
                foreach (var order in orders)
                {
                    order.OReadStatus = true;
                    db.Attach(order);
                    db.Entry(order).Property(p => p.OReadStatus).IsModified = true;
                }

                db.SaveChanges();

                if (services[0].VId != null)
                {
                    var v = db.Volunteer.Find(services[0].VId);
                    ViewBag.vName = v.VName;
                }

                ViewBag.num = id;

                return View("details", services);
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }
        }


        [SessionCheck]
        public IActionResult detOrderP(string id)
        {

            try
            {

                var services = (from s in db.Simages
                                join o in db.Orders
                                on s.ImgId equals o.ImgId
                                where o.OReadStatus == true && o.OPh.Equals(id)
                                select new Customdetails {
                                    ImgName = s.ImgName,
                                    ImgPath = s.ImgPath,
                                    ImgPrice = s.ImgPrice,
                                    VId  = o.VId,
                                    OId = o.OId,
                                    Oname = o.OName,
                                    date = o.ODate,
                                    description = o.ODescr,
                                    NoOfGuests = o.ONoOfGuests,
                                    FuncTime = o.OFuncTime,
                                    FuncType = o.OFuncType
                                })
                                .ToList<Customdetails>();

                var orders = db.Orders.Where(o => o.OPh.Equals(id) && o.OReadStatus == false);
                foreach (var order in orders)
                {
                    order.OReadStatus = true;
                    db.Attach(order);
                    db.Entry(order).Property(p => p.OReadStatus).IsModified = true;
                }

                db.SaveChanges();

                if (services[0].VId != null)
                {
                    var v = db.Volunteer.Find(services[0].VId);
                    ViewBag.vName = v.VName;
                }

                ViewBag.num = id;

                return View("details", services);
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }
           
        }
        public IActionResult showPrev()
        {
            try
            {
                var servicesUnread = (from o in db.Orders
                                       join s in db.Simages
                                       on o.ImgId equals s.ImgId
                                       where o.OReadStatus == false
                                       group o by new { o.OPh } into g
                                       select new CustomOrder { count = g.Count(), OPh = g.Key.OPh }).ToList<CustomOrder>();

                var services = (from o in db.Orders
                                join s in db.Simages
                                on o.ImgId equals s.ImgId
                                where o.OReadStatus == true
                                group o by new { o.OPh } into g
                                select new CustomOrder { count = g.Count(), OPh = g.Key.OPh }).ToList<CustomOrder>();
                ViewBag.services = services;
                return View("Index", servicesUnread);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public IActionResult login(string AUname , string APassword)
        {
            try
            {
                if (AUname != null && APassword != null)
                {
                    var admin = db.Admin.Where(a => a.AUname.Equals(AUname) && a.APassword.Equals(APassword));
                    if (admin.Count() > 0)
                    {
                        HttpContext.Session.SetString("Admin",JsonConvert.SerializeObject(admin));
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.error = 2;   
                    }
                }
                else
                {
                    ViewBag.error = 3;
                }
            }
            catch (Exception ex)
            {

                ViewBag.error = 2;
                return View("AdminLogin");
            }
            return View("AdminLogin");
        }

        [SessionCheck]
        public IActionResult createEventServices()
        {
            ViewBag.subEvents = SubEvents();
            return View("AddEventServices",Services());
        }
        [SessionCheck]
        [HttpPost]
        public IActionResult createEventServices(int SeId,int[] SId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (int sid in SId) {
                        db.EventServices.Add(new EventServices { SeId=SeId,SId = sid});
                    }
                    
                    int effectedRows = db.SaveChanges();
                    if (effectedRows > 0)
                    {
                        ViewBag.error = 1;
                    }
                    else
                    {
                        ViewBag.error = 2;
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
                ViewBag.subEvents = SubEvents();
                return View("AddEventServices", Services());
            }
            ViewBag.subEvents = SubEvents();
            return View("AddEventServices", Services());
        }
        [SessionCheck]
        public IActionResult createEvent() {

            return View("AddEvents",Events());
        }
        [SessionCheck]
        public IActionResult createSubEvent() {
            ViewBag.Events = Events();
            return View("AddSubEvents",SubEvents());
        }
        
        [SessionCheck]
        
        public IActionResult deleteService(int id)
        {
            try
            {
                db.Services.Remove(new Models.Services { SId = id});
                db.SaveChanges();
            }
            catch (Exception)
            {
                return RedirectToAction("createService");
            }
            return RedirectToAction("createService");
        }
        
        [SessionCheck]
        public IActionResult updateService(int id)
        {
            var service = db.Services.Find(id);
            return View("EditService",service);
           
        }
        [SessionCheck]
        [HttpPost]
        public IActionResult updateService([Bind("SId","SName", "SPrice", "SIsAvailable", "SDetail")]Services services, IFormFile SDp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var s = db.Services.Find(services.SId);
                    s.SName = services.SName;
                    s.SPrice = services.SPrice;
                    s.SIsAvailable = services.SIsAvailable;
                    s.SDetail = services.SDetail;
                    if (SDp != null && SDp.Length < 180000)
                    {

                        s.SDp = uploadFile(SDp, s.SName, null);
                        db.Attach(s);
                        db.Entry(s).Property(p => p.SDp).IsModified = true;
                    }
                    if (SDp == null) { db.Attach(s); }
                    
                    db.Entry(s).Property(p => p.SName).IsModified = true;
                    db.Entry(s).Property(p => p.SPrice).IsModified = true;
                    db.Entry(s).Property(p => p.SIsAvailable).IsModified = true;
                    db.Entry(s).Property(p => p.SDetail).IsModified = true;
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.error = 2;
                    return RedirectToAction("createService");
                }
                return RedirectToAction("createService");
            }
            catch (Exception)
            {

                
            }
            return RedirectToAction("createService");
        }
        [SessionCheck]
        public IActionResult createService()
        {
            
            return View("AddServices",Services());
        }

        [SessionCheck]
        [HttpPost]
        public IActionResult createService([Bind("SName","SPrice", "SIsAvailable", "SDetail")]Services services,IFormFile SDp)
        {
            try
            {
                if (ModelState.IsValid && SDp.Length < 180000)
                {
                    services.SDp = uploadFile(SDp, services.SName, null);
                    services.SId = 0;
                    db.Services.Add(services);
                    int effectedRows = db.SaveChanges();
                    if (effectedRows  > 0)
                    {
                        ViewBag.error = 1;
                    }
                    else
                    {
                        ViewBag.error = 2;
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
            
            return View("AddServices",Services());

        }
        [SessionCheck]
        [HttpPost]
        public IActionResult createSubEvent([Bind("SeId","SeName","EId")]SubEvents subEvents,IFormFile SeDp)
        {
            try {
                if (ModelState.IsValid && SeDp.Length < 180000)
                {

                    subEvents.SeDp = uploadFile(SeDp,subEvents.SeName, null);
                    db.SubEvents.Add(subEvents);
                    int effectedRows = db.SaveChanges();
                    if (effectedRows > 0)
                    {
                        ViewBag.error = 1;//success
                    }
                    else
                    {
                        ViewBag.error = 2;//failed
                    }
                }
                else
                {
                    ViewBag.error = 3;
                    return RedirectToAction("createSubEvent");
                }
            }
            catch (Exception) {
                ViewBag.error = 2;
                return RedirectToAction("createSubEvent");
            }
            return RedirectToAction("createSubEvent");
        }


        [SessionCheck]
        [HttpPost]
        public IActionResult createEvent([Bind("EName", "EDescriiption")]Events events, IFormFile EDp)
        {
                try {
                    if (ModelState.IsValid && EDp.Length < 180000)
                    {

                        events.EDp = uploadFile(EDp,events.EName, null);
                        db.Events.Add(events);
                        int effectedRows = db.SaveChanges();
                        if (effectedRows > 0)
                        {
                            ViewBag.error = 1;//success
                        }
                        else
                        {
                            ViewBag.error = 2;//failed
                        }
                }
                else
                {
                    ViewBag.error = 3;//invalid
                }
            }
                catch (Exception) {
                    ViewBag.error = 2;//failed
                }
            
            
            return View("AddEvents",Events());
        }

        [SessionCheck]
        public IActionResult create() {
            ViewBag.error = 0;
            return View("createAdmin");
        }
        [SessionCheck]
        [HttpPost]
        public IActionResult create([Bind("AUname","APassword","AFullName","AEmail","AContact","AAccessType")]Admin admin,IFormFile ADp)
        {
            
                
                 try
                  {
                    if (ModelState.IsValid && ADp.Length < 180000)
                    {
                        admin.ADp = uploadFile(ADp,admin.AUname, null);
                        db.Admin.Add(admin);
                        int effectedRows = db.SaveChanges();
                        if (effectedRows > 0)
                        {
                            ViewBag.error = 1;//success
                        }
                        else
                        {
                            ViewBag.error = 2;//failed
                        }
                    }

                    else
                    {
                       ViewBag.error = 3;//failed
                    }
            }
                catch (Exception e)
                {
                    ViewBag.error = 2;//failed
                }
           
            
            return RedirectToAction("create");
        }
        [SessionCheck]
        
        [HttpGet]
        public IActionResult delete(int id) {
            try {
                var events = db.Events.Find(id);
                db.Events.Remove(events);
                db.SaveChanges();
                
            }
            catch (Exception) {
                ViewBag.error = 2;
            }
            
            
            return RedirectToAction("createEvent");
        }
        [SessionCheck]
        [HttpGet]
        public IActionResult update(int id) {
            try {
            Events eventModel = db.Events.Find(id);
                return View("EditEvents",eventModel);
           }
            catch (Exception) {
                ViewBag.error = 2;
               
                return RedirectToAction("createEvent");
            }
        }
        [SessionCheck]
        
        [HttpPost]
        public IActionResult updateSubEvent([Bind("SeId", "SeName", "EId")]SubEvents subEvents, IFormFile SeDp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var se = db.SubEvents.Find(subEvents.SeId);
                    se.EId = subEvents.EId;
                    se.SeName = subEvents.SeName;
                    if (SeDp != null && SeDp.Length < 180000)
                    {

                        subEvents.SeDp =uploadFile(SeDp,se.SeName, null);
                        db.Attach(se);
                        db.Entry(se).Property(p => p.SeDp).IsModified = true;
                    }
                    if (SeDp == null) { db.Attach(se); }
                    db.Entry(se).Property(p => p.EId).IsModified = true;
                    db.Entry(se).Property(p => p.SeName).IsModified = true;
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.error = 2;
                    return RedirectToAction("createSubEvent");
                }
                return RedirectToAction("createSubEvent");
            }
            catch (Exception)
            {
                ViewBag.error = 2;
                return RedirectToAction("createSubEvent");
            }
        }
        [SessionCheck]
        
        [HttpPost]
        public IActionResult update([Bind("EId","EName", "EDescriiption")]Events events, IFormFile EDp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var e = db.Events.Find(events.EId);
                    e.EName = events.EName;
                    e.EDescriiption = events.EDescriiption;
                    if (EDp != null && EDp.Length < 180000)
                    {

                        e.EDp = uploadFile(EDp,events.EName,null);
                        db.Attach(e);
                        db.Entry(e).Property(p => p.EDp).IsModified = true;
                    }
                    if (EDp == null) { db.Attach(e); }
                    db.Entry(e).Property(p => p.EName).IsModified = true;
                    db.Entry(e).Property(p => p.EDescriiption).IsModified = true;
                    db.SaveChanges();
                }
                else {
                    ViewBag.error = 2;
                }
                
                return RedirectToAction("createEvent");
            }
            catch (Exception)
            {
                ViewBag.error = 2;
               
                Events eventModel = db.Events.Find(events.EId);
                return View("EditEvents", eventModel);
            }
        }
        [SessionCheck]
       
        [HttpGet]
        public IActionResult deleteSubEvents(int id) {
            try {
                db.SubEvents.Remove(new SubEvents() { SeId = id});
                db.SaveChanges();
            }
            catch (Exception) {
                ViewBag.error = 2;
                
            }
            
            return RedirectToAction("createSubEvent");
        }
        [SessionCheck]
       
        [HttpGet]
        public IActionResult updateSubEvents(int id)
        {
            try
            {
                SubEvents eventModel = db.SubEvents.Find(id);
                ViewBag.Events = Events();
                return View("EditSubEvents", eventModel);
            }
            catch (Exception)
            {
                ViewBag.error = 2;

                return View("createEvent");
            }
        }

        public List<Events> Events() {
            try {
                return db.Events.ToList();
            }
            catch (Exception) {
                return null;
            }
        }


        public string uploadFile(IFormFile file,string pname,string folderName)
        {
            
                var name = pname.TrimEnd() + "." + Path.GetFileName(file.FileName).Split(".")[1];
                var fileName = Path.Combine(hostingEnvironment.WebRootPath,
                "Uploads", name);
                using (var stream = System.IO.File.OpenWrite(fileName))
                {
                    file.CopyTo(stream);
                }
                return $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "\\" + "Uploads\\" + name;
        }

        public IActionResult createImages()
        {
            try
            {
                return View("AddImages",Services());
            }
            catch (Exception)
            {
                return View("AddImages",null);
            }
        }
        [HttpPost]
        public IActionResult createImages([Bind("SId","ImgPrice","ImgName", "ImgDescription", "IsDeal")]Simages images,IFormFile ImgPath,string img)
        {
            ViewBag.error = 0;
            try
            {
                if (ModelState.IsValid)
                {

                    if (img != null)
                    {
                        db.Simages.Add
                            (
                               new Simages
                               {
                                   SId = images.SId,
                                   IsDeal = images.IsDeal,
                                   ImgPath = img,
                                   ImgName = images.ImgName,
                                   ImgPrice = images.ImgPrice,
                                   ImgDescription = images.ImgDescription
                               }

                            );

                    }
                    else
                    {
                        db.Simages.Add
                            (
                               new Simages
                               {
                                   SId = images.SId,
                                   IsDeal = images.IsDeal,
                                   ImgPath = uploadFile(ImgPath, (DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year).ToString(), null),
                                   ImgName = images.ImgName,
                                   ImgPrice = images.ImgPrice,
                                   ImgDescription = images.ImgDescription
                               }

                            );

                    }

                    db.SaveChanges();
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
            return View("AddImages", Services());
        }

        public List<SubEvents> SubEvents() {
            try {
                return db.SubEvents.ToList();
            }
            catch (Exception) {
                return null;
            }
        }

        public List<Services> Services()
        {
            try {
                return db.Services.ToList();
            }
            catch (Exception) {
                return db.Services.ToList();
            }
            
        }
    }
}