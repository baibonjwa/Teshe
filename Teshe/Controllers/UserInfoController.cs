using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Teshe.Models;
using System.Web.Security;

namespace Teshe.Controllers
{
    public class UserInfoController : Controller
    {
        private TesheContext db = new TesheContext();

        //
        // GET: /UserInfo/

        public ActionResult Index()
        {
            return View(db.UserInfoes.ToList());
        }

        //
        // GET: /UserInfo/Details/5

        public ActionResult Details(int id = 0)
        {
            UserInfo userinfo = db.UserInfoes.Find(id);
            if (userinfo == null)
            {
                return HttpNotFound();
            }
            return View(userinfo);
        }

        //
        // GET: /UserInfo/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /UserInfo/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserInfo userinfo)
        {
            if (ModelState.IsValid)
            {
                db.UserInfoes.Add(userinfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userinfo);
        }

        //
        // GET: /UserInfo/Edit/5

        public ActionResult Edit(int id = 0)
        {
            UserInfo userinfo = db.UserInfoes.Find(id);
            if (userinfo == null)
            {
                return HttpNotFound();
            }
            return View(userinfo);
        }

        //
        // POST: /UserInfo/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserInfo userinfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userinfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userinfo);
        }

        //
        // GET: /UserInfo/Delete/5

        public ActionResult Delete(int id = 0)
        {
            UserInfo userinfo = db.UserInfoes.Find(id);
            if (userinfo == null)
            {
                return HttpNotFound();
            }
            return View(userinfo);
        }

        //
        // POST: /UserInfo/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserInfo userinfo = db.UserInfoes.Find(id);
            db.UserInfoes.Remove(userinfo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Login()
        {
            //ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(string name, string password, string returnUrl)
        {
            if (ValidateUser(name, password))
            {
                FormsAuthentication.SetAuthCookie(name, false);
                if (String.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Redirect(returnUrl);
                }
            }

            ModelState.AddModelError("", "您输入的账号或密码有错误");
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult PassVerify(UserInfo userinfo)
        {
            if (ModelState.IsValid)
            {
                userinfo.IsVerify = 1;
                db.Entry(userinfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userinfo);
        }

        private bool ValidateUser(string name, string password)
        {
            UserInfo ui = db.UserInfoes.FirstOrDefault<UserInfo>(u => u.Name == name && u.Password == password);
            if (ui != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ValidateUserRepeat(string name)
        {
            throw new NotImplementedException();
        }
    }
}