using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teshe.Models
{
    public class Device
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("设备条形码")]
        [Required(ErrorMessage = "设备条形码")]
        public string Name { get; set; }

        [DisplayName("设备型号")]
        [Required(ErrorMessage = "设备型号不能为空")]
        public string Model { get; set;                 }

        [DisplayName("条形码")]
        [Required(ErrorMessage = "条形码不能为空")]
        public string Barcode { get; set; }

        [DisplayName("设备照片")]
        public string PhotoUrl { get; set; }

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

        [DisplayName("出厂日期")]
        [Required(ErrorMessage = "请输入出厂日期")]
        public DateTime ManufactureDate { get; set; }

        [DisplayName("生产厂家")]
        [Required(ErrorMessage = "请输入生产厂家")]
        public string Factory { get; set; }

        [DisplayName("安装时间")]
        [Required(ErrorMessage = "请输入安装时间")]
        public DateTime SetupTime { get; set; }

        [DisplayName("防爆否")]
        [Required(ErrorMessage = "请输入防爆否")]
        public string ExplosionProof { get; set; }

        [DisplayName("安检证号")]
        [Required(ErrorMessage = "请输入安检证号")]
        public string SecurityCertificateNo { get; set; }

        [DisplayName("检测状态")]
        [Required(ErrorMessage = "请输入检测状态")]
        public string CheckState { get; set; }

        [DisplayName("检测时间")]
        [Required(ErrorMessage = "请输入检测时间")]
        public DateTime CheckTime { get; set; }

        [DisplayName("检测周期")]
        [Required(ErrorMessage = "请输入检测周期")]
        public int CheckCycle { get; set; }

        [DisplayName("使用状态")]
        [Required(ErrorMessage = "请输入使用状态")]
        public string UseState { get; set; }

        [DisplayName("维修记录")]
        public string MaintenanceRecord { get; set; }

        [DisplayName("备注")]
        public string Remarks { get; set; }

        [DisplayName("录入人员")]
        public UserInfo UserInfo { get; set; }

        [DisplayName("录入时间")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime InputTime { get; set; }

        [DisplayName("属性")]
        public virtual List<Attribute> Attributes { get; set; }
    }
}