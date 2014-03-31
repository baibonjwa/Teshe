using EmitMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Teshe.Common;
using Teshe.Models;

namespace Teshe.Controllers
{
    [Authorize]
    public class ScrapController : BaseController
    {
        // GET: /Scrap/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Search(ScrapIndexViewModel viewModel)
        {
            Expression<Func<Scrap, bool>> where = PredicateExtensionses.True<Scrap>();
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
                    where = where.And(u => u.Device.District == user.District);
                    where = where.And(u => u.Device.City == user.City);
                    where = where.And(u => u.Device.Province == user.Province);
                }
                else if (User.IsInRole("市级管理员"))
                {
                    where = where.And(u => u.Device.City == user.City);
                    where = where.And(u => u.Device.Province == user.Province);
                }
                else if (User.IsInRole("省级管理员"))
                {
                    where = where.And(u => u.Device.Province == user.Province);
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(viewModel.Name)) where = where.And(u => u.Device.Name == viewModel.Name);
                if (!String.IsNullOrEmpty(viewModel.Model)) where = where.And(u => u.Device.Model == viewModel.Model);
                if (!String.IsNullOrEmpty(viewModel.Company)) where = where.And(u => u.Device.Company == viewModel.Company);
                if (!String.IsNullOrEmpty(viewModel.Barcode)) where = where.And(u => u.Device.Barcode == viewModel.Barcode);
                if (!String.IsNullOrEmpty(viewModel.District)) where = where.And(u => u.Device.District == viewModel.District);
                if (!String.IsNullOrEmpty(viewModel.City)) where = where.And(u => u.Device.City == viewModel.City);
                if (!String.IsNullOrEmpty(viewModel.Province)) where = where.And(u => u.Device.Province == viewModel.Province);
                if (viewModel.ScrapTime != null) where = where.And(u => u.ScrapTime == viewModel.ScrapTime);
            }
            List<Scrap> results = db.Scraps.Where<Scrap>(where).ToList();
            return Content(JsonConvert.SerializeObject(results, dateTimeConverter));
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
                log.Info("用户" + User.Identity.Name + "于" + DateTime.Now.ToString() + "添加报废信息" + scrap.Device.Name);
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
        public ActionResult Print(String data)
        {
            List<Scrap> list = JsonConvert.DeserializeObject<List<Scrap>>(data, dateTimeConverter);
            return View(list);
        }
        //
        // POST: /Scrap/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Scrap scrap)
        {
            if (ModelState.IsValid)
            {
                List<ScrapModifyRecord> recordList = new List<ScrapModifyRecord>();
                db.Entry(scrap).State = EntityState.Modified;
                DbPropertyValues proOld = db.Entry(scrap).GetDatabaseValues();
                DbPropertyValues proNew = db.Entry(scrap).CurrentValues;
                foreach (var p in proOld.PropertyNames)
                {
                    var pro = scrap.GetType().GetProperty(p);
                    object[] attr = pro.GetCustomAttributes(typeof(System.ComponentModel.DisplayNameAttribute), false);
                    String displayName = "";
                    if (attr.Count(u => u.GetType() == typeof(System.ComponentModel.DisplayNameAttribute)) > 0)
                    {
                        displayName = ((System.ComponentModel.DisplayNameAttribute)attr[0]).DisplayName;
                    }
                    if (proOld[p] == null && proNew[p] != null)
                    {
                        ScrapModifyRecord record = new ScrapModifyRecord();
                        record.Content = "用户\"" + User.Identity.Name + "\"于\"" + DateTime.Now.ToString() + "\"将故障信息\"" + scrap.Description + "\"的\"" + displayName + "\"字段由\"" + proOld[p] + "\"改为\"" + proNew[p] + "\"";
                        recordList.Add(record);
                    }
                    if (p == "InputTime")
                    {
                        continue;
                    }
                    else if (proOld[p] != null && proNew[p] != null && proOld[p].ToString() != proNew[p].ToString())
                    {
                        ScrapModifyRecord record = new ScrapModifyRecord();
                        record.Content = "用户\"" + User.Identity.Name + "\"于\"" + DateTime.Now.ToString() + "\"将故障信息\"" + scrap.Description + "\"的\"" + displayName + "\"字段由\"" + proOld[p] + "\"改为\"" + proNew[p] + "\"";
                        recordList.Add(record);
                    }
                }
                foreach (var r in recordList)
                {
                    db.ScrapModifyRecords.Add(r);
                }
                db.SaveChanges();
                log.Info("用户" + User.Identity.Name + "于" + DateTime.Now.ToString() + "修改报废信息");
                return RedirectToAction("Index");
            }
            return View(scrap);
        }

        //
        // GET: /Scrap/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Scrap scrap = db.Scraps.Find(id);
            db.Scraps.Remove(scrap);
            db.SaveChanges();
            log.Info("用户" + User.Identity.Name + "于" + DateTime.Now.ToString() + "删除报废信息" + scrap.Device.Name);
            return RedirectToAction("Index");
        }

        public ActionResult ExportExcel(String data)
        {
            Response.ContentType = "text/plain";
            List<Scrap> list = JsonConvert.DeserializeObject<List<Scrap>>(data, dateTimeConverter);
            Scrap scrap = new Scrap();
            return File(scrap.Export(list).GetBuffer(), "application/vnd.ms-excel;charset=UTF-8", "data.xls");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}