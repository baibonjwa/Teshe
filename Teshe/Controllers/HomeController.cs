using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Teshe.Common;

namespace Teshe.App_Start
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SystemInfo()
        {
            return View();
        }

        public ActionResult DataBackup()
        {
            try
            {
                ViewBag.BackupSuccess = true;
                var dname = DateTime.Now.Ticks;
                string filename = Server.MapPath("~/Data/" + dname + ".bak");
                //if (!System.IO.File.Exists(filename))
                //{
                //    System.IO.File.Create(filename);
                //}
                DatabaseMaintenance.Backup(filename);
                return File("/Data/" + dname + ".bak", "application/x-msdownload", dname + ".bak");
            }
            catch
            {
                ViewBag.BackupSuccess = false;
                return View();
            }
        }
        //public string DelDataBase(string id)
        //{
        //    try
        //    {
        //        string filepath = Server.MapPath("~/Data/" + id);
        //        System.IO.File.Delete(filepath);
        //        return "删除成功";
        //    }
        //    catch
        //    {
        //        return "删除失败";
        //    }
        //}
    }
}
