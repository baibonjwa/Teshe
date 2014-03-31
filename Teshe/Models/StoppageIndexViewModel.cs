using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Teshe.Models
{
    public class StoppageIndexViewModel
    {
        [DisplayName("设备名称")]
        [Required(ErrorMessage = "设备不能为空")]
        public string Name { get; set; }

        [DisplayName("设备型号")]
        [Required(ErrorMessage = "设备型号不能为空")]
        public string Model { get; set; }

        [DisplayName("条形码")]
        [Required(ErrorMessage = "条形码不能为空")]
        public string Barcode { get; set; }

        [DisplayName("所在公司")]
        [Required(ErrorMessage = "请输入所在公司")]
        public string Company { get; set; }

        [DisplayName("所在区（县）")]
        public string District { get; set; }

        [DisplayName("所在城市")]
        public string City { get; set; }

        [DisplayName("所在省份")]
        public string Province { get; set; }

        [DisplayName("故障时间")]
        [Required(ErrorMessage = "故障时间不能为空")]
        public DateTime? StoppageTime { get; set; }
    }
}