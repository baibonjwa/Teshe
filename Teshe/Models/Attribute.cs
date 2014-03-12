using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Teshe.Models
{
    public class Attribute
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("属性名称")]
        [Required(ErrorMessage = "设备名称不能为空")]
        public string Name { get; set; }

        [DisplayName("属性内容")]
        [Required(ErrorMessage = "设备型号不能为空")]
        public string Content { get; set; }

    }
}