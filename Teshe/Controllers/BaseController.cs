using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Teshe.Models;
using log4net;
namespace Teshe.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/
        protected TesheContext db = new TesheContext();
        protected ILog log = LogManager.GetLogger("Log");

        protected UserInfo GetUser()
        {
            return db.UserInfoes.FirstOrDefault<UserInfo>(u => u.Name == User.Identity.Name);
        }
    }
}
