using blogSitesi.Models;
using blogSitesi.Models.ValidationRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace blogSitesi.Controllers
{
    public class iletisimController : Controller
    {
        blogSitesiDB db = new blogSitesiDB();
        // GET: iletisim
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Mesaj m)
        {
            var validator = new iletisimValidator();

            var result = validator.Validate(m);

            if (result.Errors.Count > 0)
            {
                foreach (var item in result.Errors)
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);

                return View(m);
            }
            m.Alici = "admin@email.com";
            m.Tarih = DateTime.Parse(DateTime.Now.ToShortDateString());
            db.Mesaj.Add(m);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}