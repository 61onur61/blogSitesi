using blogSitesi.Models;
using blogSitesi.Models.ValidationRules;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace blogSitesi.Controllers
{
    public class UyeController : Controller
    {
        private blogSitesiDB db = new blogSitesiDB();
        // GET: Uye
        public ActionResult Index(int id)
        {
            var uye = db.Uyes.Where(u => u.uyeId == id).SingleOrDefault();
            if(Convert.ToInt32(Session["uyeId"]) != uye.uyeId)
            {
                return HttpNotFound();
            }
            return View(uye);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Uye uye)
        {
            
                var validator = new UyeValidator();

                var result = validator.Validate(uye);

                if (result.Errors.Count > 0)
                {
                    foreach (var item in result.Errors)
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);

                    return View(uye);
                }

            if (ModelState.IsValid)
            {
                var login = db.Uyes.Where(u => u.kullaniciAd == uye.kullaniciAd).SingleOrDefault();
                if (login.kullaniciAd == uye.kullaniciAd && login.kullaniciEmail == uye.kullaniciEmail && login.kullaniciSifre == uye.kullaniciSifre)
                {
                    Session["uyeid"] = login.uyeId;
                    Session["kullaniciAdi"] = login.kullaniciAd;
                    Session["yetkiid"] = login.YetkiId;
                    Session["kullaniciEmail"] = login.kullaniciEmail;
                    Session["uyeFoto"] = login.kullaniciFoto;


                }
                else
                {

                    ViewBag.Uyari = "Kullanıcı Adı , Email ya da Şifreniz yanlış!!!";
                    return View();
                }
               
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Logout()
        {
            Session["uyeid"] = null;
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Uye uye, HttpPostedFileBase kullaniciFoto)
        {
            var validator = new UyeValidator();

            var result = validator.Validate(uye);

            if (result.Errors.Count > 0)
            {
                foreach (var item in result.Errors)
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);

                return View(uye);
            }

            if (ModelState.IsValid)
            {
                if(kullaniciFoto != null)
                {
                    WebImage img = new WebImage(kullaniciFoto.InputStream);
                    FileInfo FotoInfo = new FileInfo(kullaniciFoto.FileName);

                    string newfoto = Guid.NewGuid().ToString() + FotoInfo.Extension;
                    img.Resize(150, 150);
                    img.Save("~/Uploads/UyeFoto/" + newfoto);
                    uye.kullaniciFoto = "/Uploads/UyeFoto/" + newfoto;
                    uye.YetkiId = 2;
                    db.Uyes.Add(uye);
                    db.SaveChanges();
                    Session["uyeid"] = uye.uyeId;
                    Session["kullaniciAdi"] = uye.kullaniciAd;
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError("Fotograf", "Fotograf Seciniz");
                }
            }
            return View(uye);
        }

        public ActionResult Edit(int id)
        {
            var uye = db.Uyes.Where(u => u.uyeId == id).SingleOrDefault();
            if(Convert.ToInt32(Session["uyeId"]) != uye.uyeId)
            {
                return HttpNotFound();
            }
            return View(uye);
        }

        [HttpPost]
        public ActionResult Edit(Uye uye,int id,HttpPostedFileBase KullaniciFoto)
        {
            if(ModelState.IsValid)
            {
                var uyes = db.Uyes.Where(u => u.uyeId == id).SingleOrDefault();
                if(KullaniciFoto != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(uyes.kullaniciFoto)))
                    {
                        System.IO.File.Delete(Server.MapPath(uyes.kullaniciFoto));
                    }
                    WebImage img = new WebImage(KullaniciFoto.InputStream);
                    FileInfo FotoInfo = new FileInfo(KullaniciFoto.FileName);

                    string newfoto = Guid.NewGuid().ToString() + FotoInfo.Extension;
                    img.Resize(150, 150);
                    img.Save("~/Uploads/UyeFoto/" + newfoto);
                    uyes.kullaniciFoto = "/Uploads/UyeFoto/" + newfoto;
                }
                uyes.kullaniciAdSoyad = uye.kullaniciAdSoyad;
                uyes.kullaniciAd = uye.kullaniciAd;
                uyes.kullaniciEmail = uye.kullaniciEmail;
                uyes.kullaniciSifre = uye.kullaniciSifre;
                db.SaveChanges();
                Session["kullaniciAd"] = uye.kullaniciAd;
                return RedirectToAction("Index", "Uye", new { id = uyes.uyeId });
            }
            
            
                return View();
            
            
        }

        public ActionResult UyeProfil(int id)
        {
            var uye = db.Uyes.Where(u => u.uyeId == id).SingleOrDefault();
            return View(uye);
        }

    }
}