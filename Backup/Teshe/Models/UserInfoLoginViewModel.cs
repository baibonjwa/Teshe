using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Teshe.Models
{
    public class UserInfoLoginViewModel
    {
        [DisplayName("用户")]
        [Required(ErrorMessage = "请输入{0}")]
        [MaxLength(40, ErrorMessage = "密码不得超过40个字符")]
        public string Name { get; set; }

        [DisplayName("密码")]
        [Required(ErrorMessage = "请输入{0}")]
        [MaxLength(40, ErrorMessage = "密码不得超过40个字符")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}