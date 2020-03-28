using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using blogSitesi.Models;
using blogSitesi.Models.ValidationRules;
using PagedList;
using PagedList.Mvc;

namespace blogSitesi.Controllers
{
    public class AdminKategoriController : Controller
    {
        private blogSitesiDB db = new blogSitesiDB();

        // GET: AdminKategori
        public ActionResult Index(string a,int page = 1)
        {
            var kategori = from k in db.Kategoris select k;
            if (!string.IsNullOrEmpty(a))
            {
                kategori = kategori.Where(m => m.kategoriAd.Contains(a));
            }
            return View(kategori.ToList().ToPagedList(page, 3));
        }

        // GET: AdminKategori/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategori kategori = db.Kategoris.Find(id);
            if (kategori == null)
            {
                return HttpNotFound();
            }
            return View(kategori);
        }

        public ActionResult BagliMakaleler(int id)
        {
           
            var makaleler = db.Makales;
            ViewBag.makaleId = id;

            var sayi = makaleler.ToList().Count;

            return View(makaleler.ToList());
        }

        // GET: AdminKategori/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminKategori/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "kategoriId,kategoriAd")] Kategori kategori)
        {
            var validator = new KategoriValidator();

            var result = validator.Validate(kategori);

            if (result.Errors.Count > 0)
            {
                foreach (var item in result.Errors)
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);

                return View(kategori);
            }

            if (ModelState.IsValid)
            {
                db.Kategoris.Add(kategori);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kategori);
        }

        // GET: AdminKategori/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategori kategori = db.Kategoris.Find(id);
            if (kategori == null)
            {
                return HttpNotFound();
            }
            return View(kategori);
        }

        // POST: AdminKategori/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "kategoriId,kategoriAd")] Kategori kategori)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kategori).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kategori);
        }

        // GET: AdminKategori/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.uyari2 = "Silmek istediginize eminmisiniz?";
            ViewBag.icon = "question";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategori kategori = db.Kategoris.Find(id);
            if (kategori == null)
            {
                return HttpNotFound();
            }
            return View(kategori);
        }

        // POST: AdminKategori/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var makale = db.Makales.Where(k => k.KategoriId == id).ToList();
            if (makale.Count() > 0)
            {
                ViewBag.uyari2 = "Bu kategorinin icerdigi makaleler var ilk olarak bu kategorinin icerdiği makaleleri siliniz!!!";
                ViewBag.icon = "warning";
                Kategori kat = db.Kategoris.Find(id);
                return View(kat);
            }
            Kategori kategori = db.Kategoris.Find(id);
            db.Kategoris.Remove(kategori);
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
