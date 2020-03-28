using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using blogSitesi.Models;
using PagedList;
using PagedList.Mvc;

namespace blogSitesi.Controllers
{
    public class AdminUyeController : Controller
    {
        private blogSitesiDB db = new blogSitesiDB();

        // GET: AdminUye
        public ActionResult Index(string a,int Page=1)
        {
            var uyes = from k in db.Uyes.Include(u => u.Yetki) select k;
            if (!string.IsNullOrEmpty(a))
            {
                uyes = uyes.Where(m => m.kullaniciEmail.Contains(a));
            }
            //var uyes = db.Uyes.Include(u => u.Yetki);
            return View(uyes.OrderByDescending(u => u.uyeId).ToPagedList(Page,3));
        }

        // GET: AdminUye/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uye uye = db.Uyes.Find(id);
            if (uye == null)
            {
                return HttpNotFound();
            }
            return View(uye);
        }

        // GET: AdminUye/Create
        public ActionResult Create()
        {
            ViewBag.YetkiId = new SelectList(db.Yetkis, "yetkiId", "yetki1");
            return View();
        }

        // POST: AdminUye/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "uyeId,kullaniciAd,kullaniciEmail,kullaniciSifre,kullaniciAdSoyad,kullaniciFoto,YetkiId")] Uye uye)
        {
            if (ModelState.IsValid)
            {
                db.Uyes.Add(uye);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.YetkiId = new SelectList(db.Yetkis, "yetkiId", "yetki1", uye.YetkiId);
            return View(uye);
        }

        // GET: AdminUye/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uye uye = db.Uyes.Find(id);
            if (uye == null)
            {
                return HttpNotFound();
            }
            ViewBag.YetkiId = new SelectList(db.Yetkis, "yetkiId", "yetki1", uye.YetkiId);
            return View(uye);
        }

        // POST: AdminUye/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "uyeId,kullaniciAd,kullaniciEmail,kullaniciSifre,kullaniciAdSoyad,kullaniciFoto,YetkiId")] Uye uye)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uye).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.YetkiId = new SelectList(db.Yetkis, "yetkiId", "yetki1", uye.YetkiId);
            return View(uye);
        }

        // GET: AdminUye/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uye uye = db.Uyes.Find(id);
            if (uye == null)
            {
                return HttpNotFound();
            }
            return View(uye);
        }

        // POST: AdminUye/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Uye uye = db.Uyes.Find(id);
            db.Uyes.Remove(uye);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
