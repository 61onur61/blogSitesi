using blogSitesi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace blogSitesi.Controllers
{
    public class HomeController : Controller
    {
        blogSitesiDB db = new blogSitesiDB();
        // GET: Home
        public ActionResult Index(int Page=1)
        {
            var makale = db.Makales.OrderByDescending(m => m.makaleId).ToPagedList(Page,4);
            return View(makale);
        }
        public ActionResult KategoriMakale(int id)
        {
            var makaleler = db.Makales.Where(m => m.Kategori.kategoriId == id).ToList();
            return View(makaleler);
        }
        public ActionResult ara(string ara = null)
        {
            var aranan = db.Makales.Where(m => m.makaleBaslik.Contains(ara)).ToList();
            return View(aranan.OrderByDescending(m=>m.makaleTarih));
        }
        public ActionResult SonYorumlar()
        {
            return View(db.Yorums.OrderByDescending(y => y.yorumId).Take(5));
        }
        public ActionResult SonMakaleler()
        {
            return View(db.Makales.OrderByDescending(m => m.makaleId).Take(5));
        }
        public ActionResult PopularMakaleler()
        {
            return View(db.Makales.OrderByDescending(m => m.makaleOkunma).Take(5));
        }
        public ActionResult MakaleDetay(int id)
        {
            var makale = db.Makales.Where(m => m.makaleId == id).SingleOrDefault();
            if(makale == null)
            {
                HttpNotFound();
            }
            return View(makale);
        }
        public ActionResult Hakkimizda()
        {
            return View();
        }
        public ActionResult iletisim()
        {
            return View();
        }
        public ActionResult KategoriListele()
        {
            return View(db.Kategoris.ToList().Take(10));
        }
        public ActionResult EtiketListele()
        {
            return View(db.Etikets.ToList());
        }

        public JsonResult YorumYap(string yorum,int makaleId)
        {
            var uyeId = Session["uyeId"];
            if(yorum != null)
            {
                db.Yorums.Add(new Yorum { UyeId = Convert.ToInt32(uyeId), MakaleId = makaleId, yorumIcerik = yorum, yorumTarih = DateTime.Now });
                db.SaveChanges();
            }
            return Json(false,JsonRequestBehavior.AllowGet);
        }
        public ActionResult YorumSil(int id)
        {
            var uyeId = Session["uyeId"];
            var yorum = db.Yorums.Where(y => y.yorumId == id).SingleOrDefault();
            var makale = db.Makales.Where(m => m.makaleId == yorum.MakaleId).SingleOrDefault();
            if(yorum.UyeId == Convert.ToInt32(uyeId))
            {
                db.Yorums.Remove(yorum);
                db.SaveChanges();
                return RedirectToAction("MakaleDetay", "Home", new { id = makale.makaleId });
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult OkunmaArttir(int makaleId)
        {
            var makale = db.Makales.Where(m => m.makaleId == makaleId).SingleOrDefault();
            makale.makaleOkunma += 1;
            db.SaveChanges();
            return View();
        }

        public ActionResult Chat()
        {
            return View();
        }
    }
}