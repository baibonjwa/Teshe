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
using System.Data.Entity.Validation;
using EmitMapper;
using EmitMapper.MappingConfiguration;
using System.Reflection;
using Teshe.Common;

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
        [Authorize(Roles = "区（县）级管理员,市级管理员,省级管理员,系统管理员")]
        public ActionResult CreateAdmin()
        {
            ViewBag.CreateAdminIsSuccess = false;
            List<SelectListItem> items = new List<SelectListItem>();
            if (User.IsInRole("系统管理员"))
            {
                List<UserType> adminList = db.UserTypes.Where<UserType>(u => u.Name == "系统管理员" || u.Name == "省级管理员").ToList<UserType>();
                if (adminList.Count == 2)
                {
                    SelectListItem sli = new SelectListItem();
                    sli.Text = adminList[0].Name;
                    sli.Value = adminList[0].Id.ToString();
                    items.Add(sli);
                    sli = new SelectListItem();
                    sli.Text = adminList[1].Name;
                    sli.Value = adminList[1].Id.ToString();
                    items.Add(sli);
                    ViewBag.UserTypeList = items;
                }
            }
            else if (User.IsInRole("省级管理员"))
            {
                List<UserType> adminList = db.UserTypes.Where<UserType>(u => u.Name == "市级管理员").ToList<UserType>();
                if (adminList.Count > 0)
                {
                    SelectListItem sli = new SelectListItem();
                    sli.Text = adminList[0].Name;
                    sli.Value = adminList[0].Id.ToString();
                    items.Add(sli);
                    ViewBag.UserTypeList = items;
                }
            }
            else if (User.IsInRole("市级管理员"))
            {
                List<UserType> adminList = db.UserTypes.Where<UserType>(u => u.Name == "区（县）级管理员").ToList<UserType>();
                if (adminList.Count > 0)
                {
                    SelectListItem sli = new SelectListItem();
                    sli.Text = adminList[0].Name;
                    sli.Value = adminList[0].Id.ToString();
                    items.Add(sli);
                    ViewBag.UserTypeList = items;
                }
            }


            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "区（县）级管理员,市级管理员,省级管理员,系统管理员")]
        public ActionResult CreateAdmin(CreateAdminViewModel viewModel)
        {
            UserInfo userinfo = ObjectMapperManager.DefaultInstance.GetMapper<CreateAdminViewModel, UserInfo>().Map
(viewModel);
            userinfo.UserType = db.UserTypes.Find(Convert.ToInt16(viewModel.UserType.Name));
            userinfo.IsVerify = 1;
            if (ModelState.IsValid)
            {
                db.UserInfoes.Add(userinfo);
                db.SaveChanges();
                log.Info("用户" + userinfo.Name + "于" + DateTime.Now.ToString() + "建立了管理员");
                ViewBag.CreateAdminIsSuccess = false;
                return View();
            }
            return View();
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
        public ActionResult ModifyPassword()
        {
            ViewBag.ModifyPasswordIsSuccess = false;
            return View();
        }

        [HttpPost]
        public ActionResult ModifyPassword(ModifyPasswordViewModel viewModel)
        {
            UserInfo userinfo = GetUser();
            if (userinfo.Password == viewModel.OldPassword)
            {
                userinfo = Helper.ModifyMap<UserInfo, ModifyPasswordViewModel>(userinfo, viewModel);
                if (ModelState.IsValid)
                {
                    db.Entry(userinfo).State = EntityState.Modified;
                    db.SaveChanges();
                    log.Info("用户" + User.Identity.Name + "于" + DateTime.Now.ToString() + "修改密码");
                    ViewBag.ModifyPasswordIsSuccess = true;
                }
            }
            else
            {
                ModelState.AddModelError("", "原密码错误！");
            }
            return View();
        }

        public ActionResult ModifyUserInfo()
        {
            ViewBag.ModifyInfoIsSuccess = false;
            UserInfo userinfo = new UserInfo();
            userinfo = db.UserInfoes.FirstOrDefault<UserInfo>(u => u.Name == User.Identity.Name);

            ModifyUserInfoViewModel vm = ObjectMapperManager.DefaultInstance.GetMapper<UserInfo, ModifyUserInfoViewModel>().Map(userinfo);
            return View(vm);
        }
        [HttpPost]
        public ActionResult ModifyUserInfo(ModifyUserInfoViewModel viewModel)
        {
            UserInfo userinfo = db.UserInfoes.Find(viewModel.Id);
            userinfo = Helper.ModifyMap<UserInfo, ModifyUserInfoViewModel>(userinfo, viewModel);
            if (ModelState.IsValid)
            {
                db.Entry(userinfo).State = EntityState.Modified;
                db.SaveChanges();
                log.Info("用户" + User.Identity.Name + "于" + DateTime.Now.ToString() + "修改个人信息");
                ViewBag.ModifyInfoIsSuccess = true;
            }
            return View();
        }


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
        [Authorize(Roles = "区（县）级管理员,市级管理员,省级管理员,系统管理员")]
        public ActionResult Delete(int id = 0)
        {
            UserInfo userinfo = db.UserInfoes.Find(id);
            if (userinfo == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.UserInfoes.Remove(userinfo);
                db.SaveChanges();
                log.Info(User.Identity.Name + "于" + DateTime.Now.ToString() + "审核未通过" + userinfo.Name + "用户");
            }
            return View("Verify");
        }


        public ActionResult Cancel(String name)
        {
            UserInfo userinfo = db.UserInfoes.FirstOrDefault<UserInfo>(u => u.Name == name);
            if (userinfo == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.UserInfoes.Remove(userinfo);
                db.SaveChanges();
                log.Info(User.Identity.Name + "于" + DateTime.Now.ToString() + "注销");
                FormsAuthentication.SignOut();
                Session.Clear();
            }
            return Redirect("/");
        }
        //
        // POST: /UserInfo/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    UserInfo userinfo = db.UserInfoes.Find(id);
        //    db.UserInfoes.Remove(userinfo);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
        public ActionResult Login(UserInfoLoginViewModel userinfo)
        {
            if (ValidateUser(userinfo.Name, userinfo.Password))
            {
                FormsAuthentication.SetAuthCookie(userinfo.Name, false);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "您输入的账号或密码有错误");
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            Session.Clear();

            return RedirectToAction("Login", "UserInfo");
        }

        public ActionResult Verify()
        {
            return View();
        }

        [Authorize(Roles = "区（县）级管理员,市级管理员,省级管理员,系统管理员")]
        public ActionResult PassVerify(int id)
        {
            UserInfo userinfo = db.UserInfoes.Find(id);
            if (ModelState.IsValid)
            {
                userinfo.IsVerify = 1;
                db.Entry(userinfo).State = EntityState.Modified;
                db.SaveChanges();
                log.Info(User.Identity.Name + "于" + DateTime.Now.ToString() + "审核通过" + userinfo.Name + "用户");
            }
            return View("Verify");
        }

        public ActionResult GetNotVerifyUserInfos()
        {
            List<UserInfo> list = db.UserInfoes.Where<UserInfo>(u => u.IsVerify == 0).ToList<UserInfo>();
            return Json(list);
        }

        public ActionResult UploadPhoto(HttpPostedFileBase FileData)
        {
            //Response.HeaderEncoding = Encoding.UTF8; 
            string oldFileName = FileData.FileName;
            var sbFileName = new StringBuilder();
            sbFileName.Append(DateTime.Now.Year);
            sbFileName.Append(DateTime.Now.Month);
            sbFileName.Append(DateTime.Now.Day);
            sbFileName.Append(DateTime.Now.Hour);
            sbFileName.Append(DateTime.Now.Minute);
            sbFileName.Append(DateTime.Now.Second);
            sbFileName.Append(DateTime.Now.Millisecond);
            sbFileName.Append(Path.GetExtension(oldFileName));
            string newFileName = sbFileName.ToString();

            string strUploadPath = Server.MapPath("/FileUpload/Photo/");

            if (FileData != null)
            {
                if (!Directory.Exists(strUploadPath))
                {
                    Directory.CreateDirectory(strUploadPath);
                }
                FileData.SaveAs(strUploadPath + newFileName);
                return Json(oldFileName + "," + newFileName);
            }
            else
            {
                return Json(false);
            }
        }

        private bool ValidateUser(string name, string password)
        {
            UserInfo ui = db.UserInfoes.FirstOrDefault<UserInfo>(u => u.Name == name && u.Password == password && u.IsVerify == 1);
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

        public ActionResult PowerNotEnough()
        {
            return View();
        }
    }
}