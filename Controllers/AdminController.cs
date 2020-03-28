using blogSitesi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace blogSitesi.Controllers
{
    public class AdminController : Controller
    {
        blogSitesiDB db = new blogSitesiDB();
        // GET: Admin
        public ActionResult Index()
        {
            var UyeSayisi = db.Uyes.Count();
            ViewBag.UyeSayisi = UyeSayisi;
            var MakaleSayisi = db.Makales.Count();
            ViewBag.MakaleSayisi = MakaleSayisi;
            var YorumSayisi = db.Yorums.Count();
            ViewBag.YorumSayisi = YorumSayisi;
            var KategoriSayisi = db.Kategoris.Count();
            ViewBag.KategoriSayisi = KategoriSayisi;
            var PopulerMakele = db.EnPopulerMakale().Select(y => y.makaleBaslik).FirstOrDefault();
            ViewBag.PopulerMakele = PopulerMakele;
            var SonMakale = db.Makales.OrderByDescending(m => m.makaleTarih).Select(y => y.makaleBaslik).FirstOrDefault();
            ViewBag.SonMakale = SonMakale;
            var EnAktifUye = db.Uyes.OrderByDescending(u => u.Yorums.Count()).Select(y => y.kullaniciAd).FirstOrDefault();
            ViewBag.EnAktifUye = EnAktifUye;
            var EnPopulerKategori = db.Kategoris.OrderByDescending(k => k.Makales.Count()).Select(y => y.kategoriAd).FirstOrDefault();
            ViewBag.EnPopulerKategori = EnPopulerKategori;
            return View();
        }
    }
}