using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Teshe.Models
{
    public class DeviceIndexViewModel
    {
        [DisplayName("设备名称")]
        public string Name { get; set; }

        [DisplayName("设备型号")]
        public string Model { get; set; }

        [DisplayName("条形码")]
        public string Barcode { get; set; }

        [DisplayName("所在公司")]
        public string Company { get; set; }

        [DisplayName("所在区（县）")]
        public string District { get; set; }

        [DisplayName("所在城市")]
        public string City { get; set; }

        [DisplayName("所在省份")]
        public string Province { get; set; }

        [DisplayName("安装时间")]
        public DateTime? SetupTime { get; set; }

        [DisplayName("检测状态")]
        public string CheckState { get; set; }
    }
}