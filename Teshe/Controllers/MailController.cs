using Newtonsoft.Json;
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
    public class MailController : BaseController
    {
        //
        // GET: /Mail/
        public ActionResult Index()
        {
            return View(db.Mails.ToList());
        }

        //
        // GET: /Mail/Details/5

        public ActionResult Details(int id = 0)
        {
            Mail mail = db.Mails.Find(id);
            if (mail == null)
            {
                return HttpNotFound();
            }
            return View(mail);
        }

        //
        // GET: /Mail/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Mail/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Mail mail)
        {
            if (ModelState.IsValid)
            {
                db.Mails.Add(mail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mail);
        }

        //
        // GET: /Mail/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Mail mail = db.Mails.Find(id);
            if (mail == null)
            {
                return HttpNotFound();
            }
            return View(mail);
        }

        //
        // POST: /Mail/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Mail mail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mail);
        }

        //
        // GET: /Mail/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Mail mail = db.Mails.Find(id);
            db.Mails.Remove(mail);
            db.SaveChanges();
            return Content("删除成功");
        }

        //
        // POST: /Mail/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mail mail = db.Mails.Find(id);
            db.Mails.Remove(mail);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Search()
        {
            List<Mail> list = db.Mails.Where<Mail>(u => u.ReceivedUser.Name == User.Identity.Name).ToList();
            return Content(JsonConvert.SerializeObject(list, dateTimeConverter));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}