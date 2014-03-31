using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Teshe.Models;
using Teshe.Common;
using ZXing;
using ZXing.Common;
using Newtonsoft.Json.Converters;
using System.Reflection;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;

namespace Teshe.Controllers
{
	[Authorize]
	public class DeviceController : BaseController
	{
		public DeviceController()
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
		}

		EncodingOptions options = null;
		BarcodeWriter writer = null;
		//
		// GET: /Device/

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Barcode(String barcode)
		{
			ViewBag.Barcode = barcode;
			return View();
		}

		public ActionResult Search(DeviceIndexViewModel viewModel)
		{
			Expression<Func<Device, bool>> where = PredicateExtensionses.True<Device>();
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
					where = where.And(u => u.District == user.District);
					where = where.And(u => u.City == user.City);
					where = where.And(u => u.Province == user.Province);
				}
				else if (User.IsInRole("市级管理员"))
				{
					where = where.And(u => u.City == user.City);
					where = where.And(u => u.Province == user.Province);
				}
				else if (User.IsInRole("省级管理员"))
				{
					where = where.And(u => u.Province == user.Province);
				}
			}
			else
			{
				if (!String.IsNullOrEmpty(viewModel.Name)) where = where.And(u => u.Name == viewModel.Name);
				if (!String.IsNullOrEmpty(viewModel.Model)) where = where.And(u => u.Model == viewModel.Model);
				if (viewModel.SetupTime != null) where = where.And(u => u.SetupTime == viewModel.SetupTime);
				if (!String.IsNullOrEmpty(viewModel.Company)) where = where.And(u => u.Company == viewModel.Company);
				if (!String.IsNullOrEmpty(viewModel.Barcode)) where = where.And(u => u.Barcode == viewModel.Barcode);
				if (!String.IsNullOrEmpty(viewModel.CheckState)) where = where.And(u => u.CheckState == viewModel.CheckState);
				if (!String.IsNullOrEmpty(viewModel.District)) where = where.And(u => u.District == viewModel.District);
				if (!String.IsNullOrEmpty(viewModel.City)) where = where.And(u => u.City == viewModel.City);
				if (!String.IsNullOrEmpty(viewModel.Province)) where = where.And(u => u.Province == viewModel.Province);
			}
			List<Device> results = db.Devices.Where<Device>(where).ToList();
			return Content(JsonConvert.SerializeObject(results, dateTimeConverter));
		}

		public ActionResult Print(String data)
		{
			List<Device> list = JsonConvert.DeserializeObject<List<Device>>(data, dateTimeConverter);
			return View(list);
		}

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
			return View();
		}

		public ActionResult ModifyRecord(int id)
		{
			List<DeviceModifyRecord> list = db.Devices.FirstOrDefault<Device>(u => u.Id == id).ModifyRecords;
			return View(list);
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
		public ActionResult Create(Device device, String attributesJson)
		{
			//此处不用能GetUser()，因为在EF中db是有状态的，并且不能修改，GetUser里的db是另一个状态的db
			//会提示“一个实体对象不能由多个 IEntityChangeTracker 实例引用。”的悲催错误
			device.UserInfo = db.UserInfoes.FirstOrDefault(u => u.Name == User.Identity.Name);
			List<Teshe.Models.Attribute> attrList = JsonConvert.DeserializeObject<List<Teshe.Models.Attribute>>(attributesJson);
			device.Attributes = attrList;
			if (ModelState.IsValid)
			{
				db.Devices.Add(device);
				db.SaveChanges();
				log.Info("用户" + User.Identity.Name + "于" + DateTime.Now.ToString() + "添加设备" + device.Name);
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
		public ActionResult Edit(Device device, String attributesJson)
		{
			if (!String.IsNullOrEmpty(attributesJson))
			{
				List<Teshe.Models.Attribute> attrList = new List<Models.Attribute>(JsonConvert.DeserializeObject<List<Teshe.Models.Attribute>>(attributesJson));
				device.Attributes = attrList;
			}

			//防止转换错误
			if (ModelState.IsValid)
			{
				List<DeviceModifyRecord> recordList = new List<DeviceModifyRecord>();
				db.Entry(device).State = EntityState.Modified;
				DbPropertyValues proOld = db.Entry(device).GetDatabaseValues();
				DbPropertyValues proNew = db.Entry(device).CurrentValues;
				foreach (var p in proOld.PropertyNames)
				{
					var pro = device.GetType().GetProperty(p);
					object[] attr = pro.GetCustomAttributes(typeof(System.ComponentModel.DisplayNameAttribute), false);
					String displayName = "";
					if (attr.Count(u => u.GetType() == typeof(System.ComponentModel.DisplayNameAttribute)) > 0)
					{
						displayName = ((System.ComponentModel.DisplayNameAttribute)attr[0]).DisplayName;
					}
					if (proOld[p] == null && proNew[p] != null)
					{
						DeviceModifyRecord record = new DeviceModifyRecord();
						record.Device = device;
						record.Content = "用户\"" + User.Identity.Name + "\"于\"" + DateTime.Now.ToString() + "\"将设备\"" + device.Name + "\"的\"" + displayName + "\"字段由\"" + proOld[p] + "\"改为\"" + proNew[p] + "\"";
						recordList.Add(record);
					}
					if (p == "InputTime")
					{
						continue;
					}
					else if (proOld[p] != null && proNew[p] != null && proOld[p].ToString() != proNew[p].ToString())
					{
						DeviceModifyRecord record = new DeviceModifyRecord();
						record.Device = device;
						record.Content = "用户\"" + User.Identity.Name + "\"于\"" + DateTime.Now.ToString() + "\"将设备\"" + device.Name + "\"的\"" + displayName + "\"字段由\"" + proOld[p] + "\"改为\"" + proNew[p] + "\"";
						recordList.Add(record);
					}
				}
				foreach (var r in recordList)
				{
					db.DeviceModifyRecords.Add(r);
				}
				db.SaveChanges();

				//PropertyInfo[] pro = device.GetType().GetProperties();
				//foreach (var p in pro)
				//{
				//    if (db.Entry(device).Property(p.Name).IsModified)
				//    {

				//    }
				//    else
				//    {
				//        continue;
				//    }
				//}
				log.Info("用户" + User.Identity.Name + "于" + DateTime.Now.ToString() + "修改设备" + device.Name);
				return RedirectToAction("Index");
			}
			return View(device);
		}

		//
		// GET: /Device/Delete/5

		public ActionResult Delete(int id = 0)
		{
			Device device = db.Devices.Find(id);
			//级联删除
			List<Teshe.Models.Attribute> list = new List<Models.Attribute>(device.Attributes);
			foreach (var i in list)
			{
				db.Attributes.Remove(i);
			}
			db.Devices.Remove(device);
			db.SaveChanges();
			log.Info("用户" + User.Identity.Name + "于" + DateTime.Now.ToString() + "删除设备" + device.Name);
			return RedirectToAction("Index");
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

		public ActionResult GetDeviceByBarcode(String barcode)
		{
			return Content(JsonConvert.SerializeObject(db.Devices.FirstOrDefault<Device>(u => u.Barcode == barcode)));
		}

		public ActionResult ExportExcel(String data)
		{
			Response.ContentType = "text/plain";
			List<Device> list = JsonConvert.DeserializeObject<List<Device>>(data, dateTimeConverter);
			Device device = new Device();
			//Response.ContentType = "application/vnd.ms-excel;charset=UTF-8";
			//Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "temp.xls"));
			//Response.Clear();
			return File(device.Export(list).GetBuffer(), "application/vnd.ms-excel;charset=UTF-8", "data.xls");
		}
		protected override void Dispose(bool disposing)
		{
			db.Dispose();
			base.Dispose(disposing);
		}

	}
}