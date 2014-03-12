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
    public class CreateAdminViewModel
    {
        [DisplayName("用户名")]
        [Required(ErrorMessage = "请输入用户名")]
        [MaxLength(40, ErrorMessage = "用户名不得超过40个字符")]
        [Remote("ValidateUserRepeat", "UserInfo", HttpMethod = "POST", ErrorMessage = "用户名已被注册")]
        public string Name { get; set; }

        [NotMapped]
        [DisplayName("确认密码")]
        [Compare("Password", ErrorMessage = "两次输入的密码不一致")]
        [DataType(DataType.Password)]
        public virtual string RepPassword { get; set; }

        [DisplayName("密码")]
        [Required(ErrorMessage = "请输入密码")]
        [MaxLength(40, ErrorMessage = "密码不得超过40个字符")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("负责人")]
        [Required(ErrorMessage = "请输入负责人")]
        public string ResponsiblePerson { get; set; }

        [DisplayName("所在公司")]
        [Required(ErrorMessage = "请输入所在公司")]
        public string Company { get; set; }

        [DisplayName("所在区（县）")]
        [Required(ErrorMessage = "请输入所在区（县）")]
        public string District { get; set; }

        [DisplayName("所在城市")]
        [Required(ErrorMessage = "请输入所在城市")]
        public string City { get; set; }

        [DisplayName("所在省份")]
        [Required(ErrorMessage = "请输入所在省份")]
        public string Province { get; set; }

        [DisplayName("手机")]
        [Required(ErrorMessage = "请输入手机")]
        public string Tel { get; set; }

        [DisplayName("邮箱")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("用户类型")]
        public virtual UserType UserType { get; set; }
    }
}