using blogSitesi.Models;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PagedList;
using blogSitesi.Models.ValidationRules;

namespace blogSitesi.Controllers
{
    public class AdminMakaleController : Controller
    {
        blogSitesiDB db = new blogSitesiDB();
        // GET: AdminMakale
        public ActionResult Index(string a,int page=1)
        {
            var makale = from k in db.Makales select k;
            if (!string.IsNullOrEmpty(a))
            {
                makale = makale.Where(m => m.makaleBaslik.Contains(a));
            }
            
            return View(makale.OrderByDescending(m => m.makaleId).ToPagedList(page, 3));
        }

        // GET: AdminMakale/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Makale makale = db.Makales.Find(id);
            if (makale == null)
            {
                return HttpNotFound();
            }
            return View(makale);
        }

        // GET: AdminMakale/Create
        public ActionResult Create()
        {
            ViewBag.KategoriId = new SelectList(db.Kategoris, "kategoriId", "kategoriAd");
            return View();
        }

        // POST: AdminMakale/Create
        [HttpPost]
        public ActionResult Create(Makale makale,string etiketler,HttpPostedFileBase makaleFoto)
        {
            var validator = new MakaleValidator();

            var result = validator.Validate(makale);

            if (result.Errors.Count > 0)
            {
                foreach (var item in result.Errors)
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);

                return View(makale);
            }

            if(ModelState.IsValid)
            {


                // TODO: Add insert logic here
                if (makaleFoto != null)
                {
                    WebImage img = new WebImage(makaleFoto.InputStream);
                    FileInfo FotoInfo = new FileInfo(makaleFoto.FileName);

                    string newfoto = Guid.NewGuid().ToString() + FotoInfo.Extension;
                    img.Resize(600, 250);
                    img.Save("~/Uploads/MakaleFoto/" + newfoto);
                    makale.makaleFoto = "/Uploads/MakaleFoto/" + newfoto;

                   

                }
                if (etiketler != null)
                {
                    string[] etiketDizi = etiketler.Split(',');
                    foreach (var i in etiketDizi)
                    {
                        Etiket yeniEtiket = new Etiket { etiketAd = i };
                        db.Etikets.Add(yeniEtiket);
                        makale.Etikets.Add(yeniEtiket);
                    }
                }
                makale.UyeId = Convert.ToInt32(Session["uyeId"]);
                db.Makales.Add(makale);
                db.SaveChanges();


                return RedirectToAction("Index");
            }
            
                return View(makale);
            
        }

        // GET: AdminMakale/Edit/5
        public ActionResult Edit(int id)
        {
            var makale = db.Makales.Where(m => m.makaleId == id).SingleOrDefault();
            if(makale == null)
            {
                return HttpNotFound();
            }
            ViewBag.KategoriId = new SelectList(db.Kategoris, "kategoriId", "kategoriAd", makale.KategoriId);
            return View(makale);
        }

        // POST: AdminMakale/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, HttpPostedFileBase makaleFoto, Makale makale)
        {
            try
            {
                // TODO: Add update logic here
                var makales = db.Makales.Where(m => m.makaleId == id).SingleOrDefault();
                if(makaleFoto != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(makales.makaleFoto)))
                    {
                        System.IO.File.Delete(Server.MapPath(makales.makaleFoto));
                    }
                    WebImage img = new WebImage(makaleFoto.InputStream);
                    FileInfo FotoInfo = new FileInfo(makaleFoto.FileName);

                    string newfoto = Guid.NewGuid().ToString() + FotoInfo.Extension;
                    img.Resize(600, 250);
                    img.Save("~/Uploads/MakaleFoto/" + newfoto);
                    makales.makaleFoto = "/Uploads/MakaleFoto/" + newfoto;

                    
                    
                }
                makales.makaleBaslik = makale.makaleBaslik;
                makales.makaleIcerik = makale.makaleIcerik;
                makales.KategoriId = makale.KategoriId;
                makales.makaleTarih = makale.makaleTarih;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminMakale/Delete/5
        public ActionResult Delete(int id)
        {
            var makale = db.Makales.Where(m => m.makaleId == id).SingleOrDefault();
            if(makale == null)
            {
                return HttpNotFound();
            }
            return View(makale);
        }

        // POST: AdminMakale/Delete/5
        [HttpPost]
        public ActionResult Delete(int id,Makale makale)
        {
            try
            {
                // TODO: Add delete logic here
                var makales = db.Makales.Where(m => m.makaleId == id).SingleOrDefault();
                if(makales == null)
                {
                    return HttpNotFound();
                }
                if (System.IO.File.Exists(Server.MapPath(makales.makaleFoto)))
                {
                    System.IO.File.Delete(Server.MapPath(makales.makaleFoto));
                }
                foreach (var yorum in makales.Yorums.ToList())
                {
                    db.Yorums.Remove(yorum);
                }
                foreach (var etiket in makales.Etikets.ToList())
                {
                    db.Etikets.Remove(etiket);
                }
                db.Makales.Remove(makales);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
