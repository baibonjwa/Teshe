using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Teshe.Models
{
    public class UserType
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("用户类型")]
        [Required(ErrorMessage = "用户类型名称不能为空")]
        public string Name { get; set; }

    }
}