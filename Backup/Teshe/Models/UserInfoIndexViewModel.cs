using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Teshe.Models
{
    public class UserInfoIndexViewModel
    {
        [DisplayName("所在区（县）")]
        public string District { get; set; }

        [DisplayName("所在城市")]
        public string City { get; set; }

        [DisplayName("所在省份")]
        public string Province { get; set; }
    }
}