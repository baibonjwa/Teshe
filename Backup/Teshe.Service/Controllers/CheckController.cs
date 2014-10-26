using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Teshe.Models;

namespace Teshe.Service.Controllers
{
    public class CheckController : BaseController
    {
        [HttpGet]
        public bool Check(String username, String simcode)
        {
            UserInfo user = db.UserInfoes.FirstOrDefault<UserInfo>(u => u.Name == username && u.SIMCode == simcode);
            if (user != null)
                return true;
            else
                return false;
        }
    }
}
