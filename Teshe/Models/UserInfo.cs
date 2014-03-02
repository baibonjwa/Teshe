using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teshe.Models
{
    [DisplayName("用户信息")]
    [DisplayColumn("Name")]
    public class UserInfo
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("用户名")]
        [Required(ErrorMessage = "请输入用户名")]
        [MaxLength(40, ErrorMessage = "用户名不得超过40个字符")]
        public string Name { get; set; }

        [DisplayName("会员密码")]
        [Required(ErrorMessage = "请输入密码")]
        [MaxLength(40, ErrorMessage = "密码不得超过40个字符")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("用户注册时间")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime RegisterOn { get; set; }

        [DisplayName("通过审核")]
        public int IsVerify { get; set; }

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

        [DisplayName("备注")]
        public string Remarks { get; set; }

        [DisplayName("头像")]
        public string PhotoUrl { get; set; }

        [DisplayName("用户类型")]
        public UserType UserType { get; set; }

    }
}