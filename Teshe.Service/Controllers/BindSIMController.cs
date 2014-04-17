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
    public class BindSIMController : BaseController
    {
        [HttpGet]
        public String BindSIM(String username, String password, String simcode)
        {
            UserInfo user = db.UserInfoes.FirstOrDefault<UserInfo>(u => u.Name == username && u.Password == password);
            if (user == null)
                return "该用户不存在或用户名密码错误";
            if (!String.IsNullOrEmpty(user.SIMCode))
                return "该用户已绑定";
            user.SIMCode = simcode;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return "绑定成功";
        }
    }
}
