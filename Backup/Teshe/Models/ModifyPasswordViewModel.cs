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
    public class ModifyPasswordViewModel
    {
        [DisplayName("原密码")]
        [Required(ErrorMessage = "请输入原密码")]
        [MaxLength(40, ErrorMessage = "密码不得超过40个字符")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [DisplayName("新密码")]
        [Required(ErrorMessage = "请输入新密码")]
        [MaxLength(40, ErrorMessage = "密码不得超过40个字符")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        [DisplayName("确认密码")]
        [Compare("Password", ErrorMessage = "两次输入的密码不一致")]
        [DataType(DataType.Password)]
        public string RepPassword { get; set; }
    }
}