using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Teshe.Models;
using EmitMapper;
using Teshe.Common;
using System.Linq.Expressions;
using Newtonsoft.Json;
using System.Data.Entity.Infrastructure;
using System.Reflection;

namespace Teshe.Controllers
{
    [Authorize]
    public class StoppageController : BaseController
    {
        //
        // GET: /Stoppage/

        public ActionResult Index()
        {
            return View();
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
                log.Info("用户" + User.Identity.Name + "于" + DateTime.Now.ToString() + "添加故障信息" + stoppage.Device.Name);
                return RedirectToAction("Index");

            }
            return View(model);
        }


        public ActionResult Search(StoppageIndexViewModel viewModel)
        {
            Expression<Func<Stoppage, bool>> where = PredicateExtensionses.True<Stoppage>();
            bool isfirst = true;
            PropertyInfo[] pro = viewModel.GetType().GetProperties();
            foreach (var p in pro)
            {
                if (p.GetValue(viewModel, null) != null)
                {
                    isfirst = false;
                    break;
                }
            }
            if (isfirst)
            {
                UserInfo user = GetUser();
                if (User.IsInRole("区（县）级管理员"))
                {
                    where = where.And(u => u.Device.District.Contains(user.District));
                    where = where.And(u => u.Device.City.Contains(user.City));
                    where = where.And(u => u.Device.Province.Contains(user.Province));
                }
                else if (User.IsInRole("市级管理员"))
                {
                    where = where.And(u => u.Device.City.Contains(user.City));
                    where = where.And(u => u.Device.Province.Contains(user.Province));
                }
                else if (User.IsInRole("省级管理员"))
                {
                    where = where.And(u => u.Device.Province.Contains(user.Province));
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(viewModel.Name)) where = where.And(u => u.Device.Name.Contains(viewModel.Name));
                if (!String.IsNullOrEmpty(viewModel.Model)) where = where.And(u => u.Device.Model.Contains(viewModel.Model));
                if (!String.IsNullOrEmpty(viewModel.Company)) where = where.And(u => u.Device.Company.Contains(viewModel.Company));
                if (!String.IsNullOrEmpty(viewModel.Barcode)) where = where.And(u => u.Device.Barcode.Contains(viewModel.Barcode));
                if (!String.IsNullOrEmpty(viewModel.District)) where = where.And(u => u.Device.District.Contains(viewModel.District));
                if (!String.IsNullOrEmpty(viewModel.City)) where = where.And(u => u.Device.City.Contains(viewModel.City));
                if (!String.IsNullOrEmpty(viewModel.Province)) where = where.And(u => u.Device.Province.Contains(viewModel.Province));
                if (viewModel.StoppageTime != null) where = where.And(u => u.StoppageTime == viewModel.StoppageTime);
            }
            List<Stoppage> results = db.Stoppages.Where<Stoppage>(where).ToList();
            return Content(JsonConvert.SerializeObject(results, dateTimeConverter));
        }
        //
        // GET: /Stoppage/Edit/5
        public ActionResult Print(String data)
        {
            List<Stoppage> list = JsonConvert.DeserializeObject<List<Stoppage>>(data, dateTimeConverter);
            return View(list);
        }
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
                List<StoppageModifyRecord> recordList = new List<StoppageModifyRecord>();
                db.Entry(stoppage).State = EntityState.Modified;
                DbPropertyValues proOld = db.Entry(stoppage).GetDatabaseValues();
                DbPropertyValues proNew = db.Entry(stoppage).CurrentValues;
                foreach (var p in proOld.PropertyNames)
                {
                    var pro = stoppage.GetType().GetProperty(p);
                    object[] attr = pro.GetCustomAttributes(typeof(System.ComponentModel.DisplayNameAttribute), false);
                    String displayName = "";
                    if (attr.Count(u => u.GetType() == typeof(System.ComponentModel.DisplayNameAttribute)) > 0)
                    {
                        displayName = ((System.ComponentModel.DisplayNameAttribute)attr[0]).DisplayName;
                    }
                    if (proOld[p] == null && proNew[p] != null)
                    {
                        StoppageModifyRecord record = new StoppageModifyRecord();
                        record.Stoppage = stoppage;
                        record.Content = "用户\"" + User.Identity.Name + "\"于\"" + DateTime.Now.ToString() + "\"将故障信息\"" + stoppage.Description + "\"的\"" + displayName + "\"字段由\"" + proOld[p] + "\"改为\"" + proNew[p] + "\"";
                        recordList.Add(record);
                    }
                    if (p == "InputTime")
                    {
                        continue;
                    }
                    else if (proOld[p] != null && proNew[p] != null && proOld[p].ToString() != proNew[p].ToString())
                    {
                        StoppageModifyRecord record = new StoppageModifyRecord();
                        record.Stoppage = stoppage;
                        record.Content = "用户\"" + User.Identity.Name + "\"于\"" + DateTime.Now.ToString() + "\"将故障信息\"" + stoppage.Description + "\"的\"" + displayName + "\"字段由\"" + proOld[p] + "\"改为\"" + proNew[p] + "\"";
                        recordList.Add(record);
                    }
                }
                foreach (var r in recordList)
                {
                    db.StoppageModifyRecords.Add(r);
                }

                db.SaveChanges();
                log.Info("用户" + User.Identity.Name + "于" + DateTime.Now.ToString() + "修改故障信息");
                return RedirectToAction("Index");
            }
            return View(stoppage);
        }

        //
        // GET: /Stoppage/Delete/5
        public ActionResult ModifyRecord(int id)
        {
            List<StoppageModifyRecord> list = db.Stoppages.FirstOrDefault<Stoppage>(u => u.Id == id).ModifyRecords;
            return View(list);
        }
        public ActionResult Delete(int id = 0)
        {
            Stoppage stoppage = db.Stoppages.Find(id);
            db.Stoppages.Remove(stoppage);
            db.SaveChanges();
            log.Info("用户" + User.Identity.Name + "于" + DateTime.Now.ToString() + "删除故障信息");
            return RedirectToAction("Index");
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

        public ActionResult ExportExcel(String data)
        {
            Response.ContentType = "text/plain";
            List<Stoppage> list = JsonConvert.DeserializeObject<List<Stoppage>>(data, dateTimeConverter);
            Stoppage stoppage = new Stoppage();
            return File(stoppage.Export(list).GetBuffer(), "application/vnd.ms-excel;charset=UTF-8", "data.xls");
        }
    }
}