using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Teshe.Models;

namespace Teshe.Controllers
{
    public class SharedFileController : BaseController
    {
        //
        // GET: /SharedFile/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search()
        {
            return Content(JsonConvert.SerializeObject(db.SharedFiles.ToList()));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateSharedFileViewModel model)
        {

            var sharedFile = new SharedFile()
            {
                Title = model.Title,
                Description = model.Description,
                OldFileName = model.OldFileName,
                NewFileName = model.NewFileName,
                Time = DateTime.Now,
                People = GetUser()
            };

            db.SharedFiles.Add(sharedFile);
            db.SaveChanges();

            return View("Index");
        }

        public ActionResult Delete(int id = 0)
        {
            SharedFile file = db.SharedFiles.Find(id);
            //级联删除
            db.SharedFiles.Remove(file);

            db.SaveChanges();
            log.Info("用户" + User.Identity.Name + "于" + DateTime.Now + "删除共享文件" + file.Title);
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult UploadSharedFile(HttpPostedFileBase FileData)
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
            string strUploadPath = Server.MapPath("/FileUpload/SharedFile/");

            if (!Directory.Exists(strUploadPath))
            {
                Directory.CreateDirectory(strUploadPath);
            }
            FileData.SaveAs(strUploadPath + newFileName);
            return Json(oldFileName + "," + newFileName);
        }
    }
}
