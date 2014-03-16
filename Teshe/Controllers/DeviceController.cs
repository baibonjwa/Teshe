using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Teshe.Models;
using ZXing;
using ZXing.Common;

namespace Teshe.Controllers
{
    public class DeviceController : BaseController
    {
        private TesheContext db = new TesheContext();
        EncodingOptions options = null;
        BarcodeWriter writer = null;
        //
        // GET: /Device/

        public ActionResult Index()
        {
            return View(db.Devices.ToList());
        }

        //
        // GET: /Device/Details/5

        public ActionResult Details(int id = 0)
        {
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }

        //
        // GET: /Device/Create

        public ActionResult Create()
        {
            List<SelectListItem> explosionProofList = new List<SelectListItem>();
            explosionProofList.Add(new SelectListItem() { Text = "否", Value = "否" });
            explosionProofList.Add(new SelectListItem() { Text = "是", Value = "是" });
            ViewBag.ExplosionProofList = explosionProofList;

            List<SelectListItem> checkStateList = new List<SelectListItem>();
            checkStateList.Add(new SelectListItem() { Text = "待检", Value = "待检" });
            checkStateList.Add(new SelectListItem() { Text = "监测有效期内", Value = "监测有效期内" });
            checkStateList.Add(new SelectListItem() { Text = "超期", Value = "超期" });
            ViewBag.CheckStateList = checkStateList;

            List<SelectListItem> useStateList = new List<SelectListItem>();
            useStateList.Add(new SelectListItem() { Text = "正常", Value = "正常" });
            useStateList.Add(new SelectListItem() { Text = "故障", Value = "故障" });
            useStateList.Add(new SelectListItem() { Text = "报废", Value = "报废" });
            ViewBag.UseStateList = useStateList;

            return View();
        }

        public ActionResult CreateBarcode(String code)
        {
            options = new EncodingOptions
            {
                //DisableECI = true,  
                //CharacterSet = "UTF-8",  
                Width = 373,
                Height = 263
            };
            writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.ITF;
            writer.Options = options;
            Bitmap bitmap = writer.Write(code);
            return File(BitmapToBytes(bitmap), "application/x-MS-bmp", "barcode.bmp");

        }
        //
        // POST: /Device/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Device device)
        {
            //此处不用能GetUser()，因为在EF中db是有状态的，并且不能修改，GetUser里的db是另一个状态的db
            //会提示“一个实体对象不能由多个 IEntityChangeTracker 实例引用。”的悲催错误
            device.UserInfo = db.UserInfoes.FirstOrDefault(u => u.Name == User.Identity.Name);
            if (ModelState.IsValid)
            {
                db.Devices.Add(device);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(device);
        }

        //
        // GET: /Device/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }

        //
        // POST: /Device/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Device device)
        {
            if (ModelState.IsValid)
            {
                db.Entry(device).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(device);
        }

        //
        // GET: /Device/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }

        //
        // POST: /Device/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Device device = db.Devices.Find(id);
            db.Devices.Remove(device);
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