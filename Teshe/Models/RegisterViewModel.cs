using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Teshe.Models
{
    public class RegisterViewModel : UserInfo
    {
        [NotMapped]
        [DisplayName("确认密码")]
        [Compare("Password", ErrorMessage = "两次输入的密码不一致")]
        [DataType(DataType.Password)]
        public virtual string RepPassword { get; set; }
    }
}