﻿using EmitMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Teshe.Models;

namespace Teshe.Controllers
{
    public class ScrapController : Controller
    {
        private TesheContext db = new TesheContext();

        //
        // GET: /Scrap/

        public ActionResult Index()
        {
            return View(db.Scraps.ToList());
        }

        //
        // GET: /Scrap/Details/5

        public ActionResult Details(int id = 0)
        {
            Scrap scrap = db.Scraps.Find(id);
            if (scrap == null)
            {
                return HttpNotFound();
            }
            return View(scrap);
        }

        //
        // GET: /Scrap/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Scrap/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateScrapViewModel model)
        {
            if (ModelState.IsValid)
            {
                Scrap scrap = ObjectMapperManager.DefaultInstance.GetMapper<CreateScrapViewModel, Scrap>().Map
(model);
                scrap.Device = db.Devices.FirstOrDefault<Device>(u => u.Barcode == model.DeviceBarcode);
                scrap.UserInfo = db.UserInfoes.FirstOrDefault<UserInfo>(u => u.Name == User.Identity.Name);
                db.Scraps.Add(scrap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        //
        // GET: /Scrap/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Scrap scrap = db.Scraps.Find(id);
            if (scrap == null)
            {
                return HttpNotFound();
            }
            return View(scrap);
        }

        //
        // POST: /Scrap/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Scrap scrap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scrap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(scrap);
        }

        //
        // GET: /Scrap/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Scrap scrap = db.Scraps.Find(id);
            if (scrap == null)
            {
                return HttpNotFound();
            }
            return View(scrap);
        }

        //
        // POST: /Scrap/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Scrap scrap = db.Scraps.Find(id);
            db.Scraps.Remove(scrap);
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