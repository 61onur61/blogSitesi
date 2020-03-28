using blogSitesi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using blogSitesi.Models.ValidationRules;
using System.Net;

namespace blogSitesi.Controllers
{
    public class MesajKullaniciController : Controller
    {
        blogSitesiDB db = new blogSitesiDB();
        // GET: MesajKullanici
        public ActionResult GelenMesaj(string a, int page = 1)
        {
            if (Session["kullaniciEmail"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var uyeMail = Session["kullaniciEmail"].ToString();
            var mesajlar = from k in db.Mesaj.Where(x => x.Alici == uyeMail).ToList() select k;
            if (!string.IsNullOrEmpty(a))
            {
                mesajlar = mesajlar.Where(m => m.Gonderen.Contains(a));
            }
            //var mesajlar = db.Mesaj.Where(x => x.Alici == uyeMail).ToList();
            return View(mesajlar.ToList().ToPagedList(page, 6));
        }

        public ActionResult GidenMesaj(string a, int page = 1)
        {
            if (Session["kullaniciEmail"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var uyeMail = Session["kullaniciEmail"].ToString();
            var mesajlar = from k in db.Mesaj.Where(x => x.Gonderen == uyeMail).ToList() select k;
            if (!string.IsNullOrEmpty(a))
            {
                mesajlar = mesajlar.Where(m => m.Konu.Contains(a));
            }
            return View(mesajlar.ToList().ToPagedList(page, 6));
        }
        // GET: AdminYorum/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mesaj mesaj = db.Mesaj.Find(id);
            if (mesaj == null)
            {
                return HttpNotFound();
            }
            return View(mesaj);
        }

        // POST: AdminYorum/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mesaj mesaj = db.Mesaj.Find(id);
            db.Mesaj.Remove(mesaj);
            db.SaveChanges();
            return RedirectToAction("GelenMesaj");
        }
        [HttpGet]
        public ActionResult YeniMesaj()
        {
            return View();
        }
        [HttpPost]
        public ActionResult YeniMesaj(Mesaj m)
        {
            var validator = new MesajValidator();

            var result = validator.Validate(m);

            if (result.Errors.Count > 0)
            {
                foreach (var item in result.Errors)
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);

                return View(m);
            }
            if (Session["kullaniciEmail"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var uyeMail = Session["kullaniciEmail"].ToString();
            m.Gonderen = uyeMail.ToString();
            m.Tarih = DateTime.Parse(DateTime.Now.ToShortDateString());
            db.Mesaj.Add(m);
            db.SaveChanges();
            return RedirectToAction("GidenMesaj", "MesajKullanici");
        }
    }
}