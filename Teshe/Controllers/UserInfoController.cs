using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Teshe.Models;
using System.Web.Security;
using System.Text;
using System.IO;


namespace Teshe.Controllers
{
    public class UserInfoController : BaseController
    {
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
        public ActionResult Create([Bind(Exclude = "RegisterOn")] UserInfo userinfo)
        {
            if (ModelState.IsValid)
            {
                userinfo.UserType = db.UserTypes.First<UserType>(u => u.Name == "客户");
                db.UserInfoes.Add(userinfo);
                db.SaveChanges();
                log.Info("用户" + userinfo.Name + "于" + DateTime.Now.ToString() + "申请注册");
                return View("Login");
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
        public ActionResult Login(string name, string password)
        {
            if (ValidateUser(name, password))
            {
                FormsAuthentication.SetAuthCookie(name, false);
                return RedirectToAction("Home", "Index");
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

        public ActionResult UploadPhoto(HttpPostedFileBase FileData)
        {
            byte[] buffer = Encoding.GetEncoding("utf-8").GetBytes(FileData.FileName);
            buffer = Encoding.Convert(Encoding.GetEncoding("utf-8"), Encoding.GetEncoding("gb2312"), buffer);
            string fn = Encoding.UTF8.GetString(buffer);
            string strUploadPath = Server.MapPath("/FileUpload/Photo/");

            if (FileData != null)
            {
                if (!Directory.Exists(strUploadPath))
                {
                    Directory.CreateDirectory(strUploadPath);
                }
                FileData.SaveAs(strUploadPath + fn);
                return Json(fn);
            }
            else
            {
                return Json(false);
            }
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


        [HttpPost]
        public ActionResult ValidateUserRepeat(string name)
        {
            var userInfo = db.UserInfoes.FirstOrDefault<UserInfo>(u => u.Name == name);
            if (userInfo != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true);
            }
        }
    }
}