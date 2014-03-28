using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Teshe.Models
{
    public class DeviceModifyRecord
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("修改内容")]
        public String Content { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ModifyTime { get; set; }

        //[DisplayName("设备")]
        //public virtual Device Device { get; set; }

    }
}