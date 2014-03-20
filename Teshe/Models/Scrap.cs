﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Teshe.Models
{
    public class Scrap
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("设备")]
        [Required(ErrorMessage = "设备不能为空")]
        public virtual Device Device { get; set; }

        [DisplayName("报废原因")]
        [Required(ErrorMessage = "故障描述不能为空")]
        public String Description { get; set; }

        [DisplayName("报废时间")]
        [Required(ErrorMessage = "报废时间不能为空")]
        public DateTime ScrapTime { get; set; }

        [DisplayName("备注")]
        public String Remarks { get; set; }

        [DisplayName("录入时间")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime InputTime { get; set; }

        [DisplayName("录入人员")]
        public virtual UserInfo UserInfo { get; set; }

        //TODO:此处需求不明确
    }
}