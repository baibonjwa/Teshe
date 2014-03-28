using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Teshe.Models
{
    public class DeviceModifyRecord
    {
        [Key]
        public int Id { get; set; }

        public String Content { get; set; }

        public DateTime ModifyTime { get; set; }

        public virtual Device Device { get; set; }

    }
}