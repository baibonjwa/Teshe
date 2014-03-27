using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Teshe.Models;
using log4net;
using System.Drawing;
using System.IO;
using Newtonsoft.Json.Converters;
namespace Teshe.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/
        protected TesheContext db = new TesheContext();
        protected ILog log = LogManager.GetLogger("Log");
        protected IsoDateTimeConverter dateTimeConverter = new IsoDateTimeConverter();

        public BaseController()
        {
            dateTimeConverter.DateTimeFormat = "yyyy-MM-dd";
        }

        protected UserInfo GetUser()
        {
            return db.UserInfoes.FirstOrDefault<UserInfo>(u => u.Name == User.Identity.Name);
        }
        protected byte[] BitmapToBytes(Bitmap Bitmap)
        {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                Bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                byte[] byteImage = new Byte[ms.Length];
                byteImage = ms.ToArray();
                return byteImage;
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            finally
            {
                ms.Close();
            }
        }
    }
}
