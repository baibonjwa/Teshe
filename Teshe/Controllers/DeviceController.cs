using System.IO;
using System.Text;
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
using Attribute = Teshe.Models.Attribute;

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
            checkStateList.Add(new SelectListItem() { Text = "检测有效期内", Value = "检测有效期内" });
            checkStateList.Add(new SelectListItem() { Text = "超期", Value = "超期" });
            ViewBag.CheckStateList = checkStateList;

            List<SelectListItem> checkStateListForSearch = new List<SelectListItem>();
            checkStateListForSearch.Add(new SelectListItem() { Text = "全部", Value = "" });
            checkStateListForSearch.Add(new SelectListItem() { Text = "待检", Value = "待检" });
            checkStateListForSearch.Add(new SelectListItem() { Text = "检测有效期内", Value = "检测有效期内" });
            checkStateListForSearch.Add(new SelectListItem() { Text = "超期", Value = "超期" });
            ViewBag.CheckStateListForSearch = checkStateListForSearch;

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

        public ActionResult OutDate()
        {
            return View();
        }

        public ActionResult Barcode(String barcode)
        {
            ViewBag.Barcode = barcode;
            return View();
        }

        public ActionResult Verify()
        {
            return View();
        }

        public ActionResult CreateLiftTechnique()
        {
            return View();
        }

        public ActionResult CreateDeliveryEquipment()
        {
            return View();
        }

        public ActionResult CreateSewerage()
        {
            return View();
        }

        public ActionResult CreateAirmovingDevice()
        {
            return View();
        }

        public ActionResult CreatePressureAir()
        {
            return View();
        }

        public ActionResult Search(DeviceIndexViewModel viewModel)
        {
            Expression<Func<Device, bool>> where = PredicateExtensionses.True<Device>();
            where = where.And(u => u.IsVerify == 1);
            bool isfirst = true;
            //PropertyInfo[] pro = viewModel.GetType().GetProperties();
            //foreach (var p in pro)
            //{
            //    if (p.GetValue(viewModel, null) != null)
            //    {
            //        isfirst = false;
            //        break;
            //    }
            //}
            if (isfirst)
            {
                UserInfo user = GetUser();
                if (User.IsInRole("区（县）级管理员"))
                {
                    where = where.And(u => u.District.Contains(user.District));
                    where = where.And(u => u.City.Contains(user.City));
                    where = where.And(u => u.Province.Contains(user.Province));
                }
                else if (User.IsInRole("市级管理员"))
                {
                    where = where.And(u => u.City.Contains(user.City));
                    where = where.And(u => u.Province.Contains(user.Province));
                }
                else if (User.IsInRole("省级管理员"))
                {
                    where = where.And(u => u.Province.Contains(user.Province));
                }
            }
            //else
            //{
                if (!String.IsNullOrEmpty(viewModel.Name)) where = where.And(u => u.Name.Contains(viewModel.Name));
                if (!String.IsNullOrEmpty(viewModel.Model)) where = where.And(u => u.Model.Contains(viewModel.Model));
                if (viewModel.SetupTime != null) where = where.And(u => u.SetupTime == viewModel.SetupTime);
                if (!String.IsNullOrEmpty(viewModel.Company)) where = where.And(u => u.Company.Contains(viewModel.Company));
                if (!String.IsNullOrEmpty(viewModel.Barcode)) where = where.And(u => u.Barcode.Contains(viewModel.Barcode));
                if (!String.IsNullOrEmpty(viewModel.CheckState)) where = where.And(u => u.CheckState.Contains(viewModel.CheckState));
                if (!String.IsNullOrEmpty(viewModel.District)) where = where.And(u => u.District.Contains(viewModel.District));
                if (!String.IsNullOrEmpty(viewModel.City)) where = where.And(u => u.City.Contains(viewModel.City));
                if (!String.IsNullOrEmpty(viewModel.Province)) where = where.And(u => u.Province.Contains(viewModel.Province));
            //}
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
            if (GetUser().UserType.Name == "系统管理员")
                device.IsVerify = 1;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateLiftTechnique(Device device, String attributesJson, String attr1, String attr2, String attr3, String attr4, String attr5, String attr6,
            String attr7, String attr8, String attr9, String attr10, String attr11, String attr12, String attr13, String attr14, String attr15,
            String attr16, String attr17, String attr18, String attr19, String attr20, String attr21, String attr22, String attr23, String attr24, String attr25)
        {
            device.UserInfo = db.UserInfoes.FirstOrDefault(u => u.Name == User.Identity.Name);
            if (GetUser().UserType.Name == "系统管理员")
                device.IsVerify = 1;
            var attrList = new List<Attribute>
            {
                new Attribute() { Name = "滚筒直径/m",Content=attr1,Device = device },
                new Attribute() { Name = "用途",Content=attr2,Device = device },
                new Attribute() { Name = "最大静张力/KN",Content=attr3,Device = device },
                new Attribute() { Name = "最大静张力差/KN",Content=attr4,Device = device },
                new Attribute() { Name = "运行井深/m",Content=attr5,Device = device },
                new Attribute() { Name = "坡度/°",Content=attr6,Device = device },
                new Attribute() { Name = "提升容积名称",Content=attr7,Device = device },
                new Attribute() { Name = "提升容积自重/kg",Content=attr8,Device = device },
                new Attribute() { Name = "载人数",Content=attr9,Device = device },
                new Attribute() { Name = "矿车重量/kg",Content=attr10,Device = device },
                new Attribute() { Name = "提升重量/kg",Content=attr11,Device = device },
                new Attribute() { Name = "电机型号",Content=attr12,Device = device },
                new Attribute() { Name = "额定功率/kw",Content=attr13,Device = device },
                new Attribute() { Name = "额定电压/V",Content=attr14,Device = device },
                new Attribute() { Name = "额定电流/A",Content=attr15,Device = device },
                new Attribute() { Name = "减速器型号",Content=attr16,Device = device },
                new Attribute() { Name = "减速比",Content=attr17,Device = device },
                new Attribute() { Name = "围抱角/°",Content=attr18,Device = device },
                new Attribute() { Name = "摩擦轮直径/m",Content=attr19,Device = device },
                new Attribute() { Name = "提升钢丝绳直径/m",Content=attr20,Device = device },
                new Attribute() { Name = "制动绳直径/mm",Content=attr21,Device = device },
                new Attribute() { Name = "最大终端载荷/kg",Content=attr22,Device = device },
                new Attribute() { Name = "弹簧最大工作负荷/KN",Content=attr23,Device = device },
                new Attribute() { Name = "最大计算制动力/KN",Content=attr24,Device = device },
                new Attribute() { Name = "实际最大载重量/kg",Content=attr25,Device = device },
            };
            var otherAttr = JsonConvert.DeserializeObject<List<Attribute>>(attributesJson);
            if (otherAttr != null && otherAttr.Count > 0)
            {
                attrList.AddRange(otherAttr);
            }
            device.Attributes = attrList;
            if (ModelState.IsValid)
            {
                db.Devices.Add(device);
                db.SaveChanges();
                log.Info("用户" + User.Identity.Name + "于" + DateTime.Now + "添加提升设备" + device.Name);
                return RedirectToAction("Index");
            }
            return View(device);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDeliveryEquipment(Device device, String attributesJson, String attr1, String attr2, String attr3, String attr4, String attr5)
        {
            device.UserInfo = db.UserInfoes.FirstOrDefault(u => u.Name == User.Identity.Name);
            if (GetUser().UserType.Name == "系统管理员")
                device.IsVerify = 1;
            var attrList = new List<Attribute>
            {
                new Attribute() { Name = "最大牵引力/KN",Content=attr1,Device = device },
                new Attribute() { Name = "组车数量",Content=attr2,Device = device },
                new Attribute() { Name = "巷道倾角/°",Content=attr3,Device = device },
                new Attribute() { Name = "人车空载重量/kg",Content=attr4,Device = device },
                new Attribute() { Name = "人车满载重量/kg",Content=attr5,Device = device }
            };
            var otherAttr = JsonConvert.DeserializeObject<List<Attribute>>(attributesJson);
            if (otherAttr != null && otherAttr.Count > 0)
            {
                attrList.AddRange(otherAttr);
            }
            device.Attributes = attrList;
            if (ModelState.IsValid)
            {
                db.Devices.Add(device);
                db.SaveChanges();
                log.Info("用户" + User.Identity.Name + "于" + DateTime.Now + "添加运输设备" + device.Name);
                return RedirectToAction("Index");
            }
            return View(device);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSewerage(Device device, String attributesJson, String attr1, String attr2, String attr3, String attr4, String attr5,
            String attr6, String attr7, String attr8, String attr9)
        {
            device.UserInfo = db.UserInfoes.FirstOrDefault(u => u.Name == User.Identity.Name);
            if (GetUser().UserType.Name == "系统管理员")
                device.IsVerify = 1;
            var attrList = new List<Attribute>
            {
                new Attribute() { Name = "额定扬程/m",Content=attr1,Device = device },
                new Attribute() { Name = "额定转速/（r/min）",Content=attr2,Device = device },
                new Attribute() { Name = "轴功率/KW",Content=attr3,Device = device },
                new Attribute() { Name = "额定效率/%",Content=attr4,Device = device },
                new Attribute() { Name = "额定流量/（m³/h）",Content=attr5,Device = device },
                new Attribute() { Name = "电动机型号",Content=attr6,Device = device },
                new Attribute() { Name = "额定功率/kw",Content=attr7,Device = device },
                new Attribute() { Name = "额定电压/V",Content=attr8,Device = device },
                new Attribute() { Name = "额定电流/A",Content=attr9,Device = device }
            };
            var otherAttr = JsonConvert.DeserializeObject<List<Attribute>>(attributesJson);
            if (otherAttr != null && otherAttr.Count > 0)
            {
                attrList.AddRange(otherAttr);
            }
            device.Attributes = attrList;
            if (ModelState.IsValid)
            {
                db.Devices.Add(device);
                db.SaveChanges();
                log.Info("用户" + User.Identity.Name + "于" + DateTime.Now + "添加排水设备" + device.Name);
                return RedirectToAction("Index");
            }
            return View(device);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAirmovingDevice(Device device, String attributesJson, String attr1, String attr2, String attr3, String attr4, String attr5,
            String attr6, String attr7, String attr8, String attr9, String attr10)
        {
            device.UserInfo = db.UserInfoes.FirstOrDefault(u => u.Name == User.Identity.Name);
            if (GetUser().UserType.Name == "系统管理员")
                device.IsVerify = 1;
            var attrList = new List<Attribute>
            {
                new Attribute() { Name = "通风方式",Content=attr1,Device = device },
                new Attribute() { Name = "通风机风压/pa",Content=attr2,Device = device },
                new Attribute() { Name = "额定风量/（m³/min）",Content=attr3,Device = device },
                new Attribute() { Name = "轴功率/KW",Content=attr4,Device = device },
                new Attribute() { Name = "额定转速/（r/min）",Content=attr5,Device = device },
                new Attribute() { Name = "传动方式",Content=attr6,Device = device },
                new Attribute() { Name = "电动机型号",Content=attr7,Device = device },
                new Attribute() { Name = "额定功率/kw",Content=attr8,Device = device },
                new Attribute() { Name = "额定电压/V",Content=attr9,Device = device },
                new Attribute() { Name = "额定电流/A",Content=attr10,Device = device }
            };
            var otherAttr = JsonConvert.DeserializeObject<List<Attribute>>(attributesJson);
            if (otherAttr != null && otherAttr.Count > 0)
            {
                attrList.AddRange(otherAttr);
            }
            device.Attributes = attrList;
            if (ModelState.IsValid)
            {
                db.Devices.Add(device);
                db.SaveChanges();
                log.Info("用户" + User.Identity.Name + "于" + DateTime.Now + "添加通风设备" + device.Name);
                return RedirectToAction("Index");
            }
            return View(device);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePressureAir(Device device, String attributesJson, String attr1, String attr2, String attr3, String attr4, String attr5,
            String attr6, String attr7, String attr8, String attr9, String attr10, String attr11, String attr12)
        {
            device.UserInfo = db.UserInfoes.FirstOrDefault(u => u.Name == User.Identity.Name);
            if (GetUser().UserType.Name == "系统管理员")
                device.IsVerify = 1;
            var attrList = new List<Attribute>
            {
                new Attribute() { Name = "额定流量/（m³/min）",Content=attr1,Device = device },
                new Attribute() { Name = "额定压力/MPa",Content=attr2,Device = device },
                new Attribute() { Name = "额定转速/（r/min）",Content=attr3,Device = device },
                new Attribute() { Name = "轴功率/KW",Content=attr4,Device = device },
                new Attribute() { Name = "冷却方式",Content=attr5,Device = device },
                new Attribute() { Name = "传动方式",Content=attr6,Device = device },
                new Attribute() { Name = "电动机型号",Content=attr7,Device = device },
                new Attribute() { Name = "额定功率/kw",Content=attr8,Device = device },
                new Attribute() { Name = "额定电压/V",Content=attr9,Device = device },
                new Attribute() { Name = "额定电流/A",Content=attr10,Device = device },
                new Attribute() { Name = "风包容积/m³",Content=attr11,Device = device },
                new Attribute() { Name = "工作压力/MPa",Content=attr12,Device = device }
            };
            var otherAttr = JsonConvert.DeserializeObject<List<Attribute>>(attributesJson);
            if (otherAttr != null && otherAttr.Count > 0)
            {
                attrList.AddRange(otherAttr);
            }
            device.Attributes = attrList;
            if (ModelState.IsValid)
            {
                db.Devices.Add(device);
                db.SaveChanges();
                log.Info("用户" + User.Identity.Name + "于" + DateTime.Now + "添加压风设备" + device.Name);
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
            List<DeviceModifyRecord> recordList = new List<DeviceModifyRecord>();
            if (!String.IsNullOrEmpty(attributesJson))
            {
                List<Teshe.Models.Attribute> attrListOld = db.Attributes.Where<Teshe.Models.Attribute>(u => u.Device.Id == device.Id).ToList();
                List<Teshe.Models.Attribute> attrList = new List<Models.Attribute>();
                foreach (var i in attrListOld)
                {
                    db.Attributes.Remove(i);
                }
                if (attributesJson.Length > 5)
                {
                    attrList = new List<Models.Attribute>(JsonConvert.DeserializeObject<List<Teshe.Models.Attribute>>(attributesJson));
                }
                foreach (var i in attrList)
                {
                    db.Attributes.Add(i);
                }

                int oldc = attrListOld.Count;
                int newc = attrList.Count;
                if (oldc - newc > 0)
                {
                    int sign = 0;
                    foreach (var i in attrListOld)
                    {
                        DeviceModifyRecord record = new DeviceModifyRecord();
                        record.Device = device;
                        if (attrList.Count > sign)
                        {
                            if (i.Name != attrList[sign].Name || i.Content != attrList[sign].Content)
                            {
                                record.Content = "用户\"" + User.Identity.Name + "\"于\"" + DateTime.Now.ToString() + "\"将设备项目\"" + i.Name + "\"：\"" + i.Content + "\"改为\"" + attrList[sign].Name + "\"：\"" + attrList[sign].Content + "\"";
                                recordList.Add(record);
                            }
                        }
                        else
                        {
                            record.Content = "用户\"" + User.Identity.Name + "\"于\"" + DateTime.Now.ToString() + "\"删除设备项目\"" + i.Name + "\"：\"" + i.Content + "\"";
                            recordList.Add(record);
                        }
                        sign++;
                    }

                }
                else
                {
                    int sign = 0;
                    foreach (var i in attrList)
                    {
                        DeviceModifyRecord record = new DeviceModifyRecord();
                        record.Device = device;
                        if (attrListOld.Count > sign)
                        {
                            if (attrListOld[sign].Name != i.Name || attrListOld[sign].Content != i.Content)
                            {
                                record.Content = "用户\"" + User.Identity.Name + "\"于\"" + DateTime.Now.ToString() + "\"将设备项目\"" + attrListOld[sign].Name + "\"：\"" + attrListOld[sign].Content + "\"改为\"" + i.Name + "\"：\"" + i.Content + "\"";
                                recordList.Add(record);
                            }
                        }
                        else
                        {
                            record.Content = "用户\"" + User.Identity.Name + "\"于\"" + DateTime.Now.ToString() + "\"添加设备项目\"" + i.Name + "\"：\"" + i.Content + "\"";
                            recordList.Add(record);
                        }
                        sign++;

                    }
                }
                //for (int i = 0; i < Math.Abs(attrListOld.Count - attrList.Count) - 1; i++)
                //{

                //}


                device.Attributes = attrList;
            }

            //防止转换错误
            if (ModelState.IsValid)
            {

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
            var list = new List<Models.Attribute>(device.Attributes);
            foreach (var i in list)
            {
                db.Attributes.Remove(i);
            }
            db.Devices.Remove(device);
            var modify = new List<DeviceModifyRecord>();
            modify = db.DeviceModifyRecords.Where(u => u.Device.Id == id).ToList();
            foreach (var i in modify)
            {
                db.DeviceModifyRecords.Remove(i);
            }

            var stoppageModify = db.StoppageModifyRecords.Where(u => u.Stoppage.Device.Id == id).ToList();
            foreach (var i in stoppageModify)
            {
                db.StoppageModifyRecords.Remove(i);
            }

            var stoppage = db.Stoppages.Where(u => u.Device.Id == id).ToList();
            foreach (var i in stoppage)
            {
                db.Stoppages.Remove(i);
            }

            var scrapModify = db.ScrapModifyRecords.Where(u => u.Scrap.Device.Id == id).ToList();
            foreach (var i in scrapModify)
            {
                db.ScrapModifyRecords.Remove(i);
            }

            var scrap = db.Scraps.Where(u => u.Device.Id == id).ToList();
            foreach (var i in scrap)
            {
                db.Scraps.Remove(i);
            }

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
            return Content(JsonConvert.SerializeObject(db.Devices.FirstOrDefault<Device>(u => u.Barcode == barcode && u.IsVerify == 1)));
        }

        public ActionResult ExportExcel(String data)
        {
            Response.ContentType = "text/plain";
            var list = JsonConvert.DeserializeObject<List<Device>>(data, dateTimeConverter);
            var device = new Device();
            //Response.ContentType = "application/vnd.ms-excel;charset=UTF-8";
            //Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "temp.xls"));
            //Response.Clear();
            return File(device.Export(list).GetBuffer(), "application/vnd.ms-excel;charset=UTF-8", "data.xls");
        }

        public ActionResult GetNotVerifyDevices()
        {
            UserInfo user = GetUser();
            List<Device> list = null;
            if (user.UserType.Name == "系统管理员")
            {
                list = db.Devices.Where<Device>(u => u.IsVerify == 0).ToList<Device>();
            }
            else if (user.UserType.Name == "省级管理员")
            {
                list = db.Devices.Where<Device>(u => u.IsVerify == 0 && u.Province == user.Province).ToList<Device>();
            }
            else if (user.UserType.Name == "市级管理员")
            {
                list = db.Devices.Where<Device>(u => u.IsVerify == 0 && u.City == user.City).ToList<Device>();
            }
            else if (user.UserType.Name == "区（县）级管理员")
            {
                list = db.Devices.Where<Device>(u => u.IsVerify == 0 && u.District == user.District).ToList<Device>();
            }
            return Content(JsonConvert.SerializeObject(list, dateTimeConverter));
        }

        public ActionResult PassVerify(int id)
        {
            Device device = db.Devices.Find(id);
            if (ModelState.IsValid)
            {
                device.IsVerify = 1;
                db.Entry(device).State = EntityState.Modified;
                db.SaveChanges();
                log.Info(User.Identity.Name + "于" + DateTime.Now + "审核通过" + device.Name + "设备");
            }
            return View("Verify");
        }

        [AllowAnonymous]
        public ActionResult UploadReport(HttpPostedFileBase FileData)
        {
            //Response.HeaderEncoding = Encoding.UTF8; 
            string oldFileName = HttpUtility.UrlDecode(FileData.FileName);
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

            string strUploadPath = Server.MapPath("/FileUpload/Report/");


            if (!Directory.Exists(strUploadPath))
            {
                Directory.CreateDirectory(strUploadPath);
            }
            FileData.SaveAs(strUploadPath + newFileName);
            return Json(oldFileName + "," + newFileName);

        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}