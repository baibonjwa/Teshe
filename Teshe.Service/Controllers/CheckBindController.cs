using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Teshe.Models;

namespace Teshe.Service.Controllers
{
    public class CheckBindController : BaseController
    {
        public String GetCheckBind(String username)
        {
            UserInfo user = db.UserInfoes.FirstOrDefault<UserInfo>(u => u.Name == username);
            if (user != null)
            {
                if (String.IsNullOrEmpty(user.SIMCode))
                {
                    return "该用户未绑定";
                }
                else
                {
                    return "该用户已绑定";
                }
            }
            else
            {
                return "该用户不存在";
            }
        }
    }
}
