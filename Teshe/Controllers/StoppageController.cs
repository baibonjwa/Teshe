using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Teshe.Models;
using EmitMapper;

namespace Teshe.Controllers
{
    public class StoppageController : Controller
    {
        private TesheContext db = new TesheContext();

        //
        // GET: /Stoppage/

        public ActionResult Index()
        {
            return View(db.Stoppages.ToList());
        }

        //
        // GET: /Stoppage/Details/5

        public ActionResult Details(int id = 0)
        {
            Stoppage stoppage = db.Stoppages.Find(id);
            if (stoppage == null)
            {
                return HttpNotFound();
            }
            return View(stoppage);
        }

        //
        // GET: /Stoppage/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Stoppage/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateStoppageViewModel model)
        {
            if (ModelState.IsValid)
            {
                Stoppage stoppage = ObjectMapperManager.DefaultInstance.GetMapper<CreateStoppageViewModel, Stoppage>().Map
(model);
                stoppage.Device = db.Devices.FirstOrDefault<Device>(u => u.Barcode == model.DeviceBarcode);
                stoppage.UserInfo = db.UserInfoes.FirstOrDefault<UserInfo>(u => u.Name == User.Identity.Name);
                db.Stoppages.Add(stoppage);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(model);
        }

        //
        // GET: /Stoppage/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Stoppage stoppage = db.Stoppages.Find(id);
            if (stoppage == null)
            {
                return HttpNotFound();
            }
            return View(stoppage);
        }

        //
        // POST: /Stoppage/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Stoppage stoppage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stoppage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stoppage);
        }

        //
        // GET: /Stoppage/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Stoppage stoppage = db.Stoppages.Find(id);
            if (stoppage == null)
            {
                return HttpNotFound();
            }
            return View(stoppage);
        }

        //
        // POST: /Stoppage/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Stoppage stoppage = db.Stoppages.Find(id);
            db.Stoppages.Remove(stoppage);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}