using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Teshe.Models
{
    public class Scrap
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("设备名称")]
        [Required(ErrorMessage = "设备名称不能为空")]
        public string Name { get; set; }

        //TODO:此处需求不明确
    }
}