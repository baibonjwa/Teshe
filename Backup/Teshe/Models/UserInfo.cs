using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.IO;
using NPOI.HSSF.UserModel;

namespace Teshe.Models
{
    public class UserInfo
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("用户名")]
        [Required(ErrorMessage = "请输入用户名")]
        [MaxLength(40, ErrorMessage = "用户名不得超过40个字符")]
        [Remote("ValidateUserRepeat", "UserInfo", HttpMethod = "POST", ErrorMessage = "用户名已被注册")]
        public string Name { get; set; }

        [DisplayName("会员密码")]
        [Required(ErrorMessage = "请输入密码")]
        [MaxLength(40, ErrorMessage = "密码不得超过40个字符")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("用户注册时间")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime RegisterOn { get; set; }

        [DisplayName("通过审核")]
        public int IsVerify { get; set; }

        [DisplayName("负责人")]
        [Required(ErrorMessage = "请输入负责人")]
        public string ResponsiblePerson { get; set; }

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

        [DisplayName("手机")]
        [Required(ErrorMessage = "请输入手机")]
        public string Tel { get; set; }

        [DisplayName("邮箱")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("备注")]
        public string Remarks { get; set; }

        [DisplayName("头像")]
        public string PhotoUrl { get; set; }

        [DisplayName("用户类型")]
        public virtual UserType UserType { get; set; }

        [DisplayName("SIM卡号")]
        public String SIMCode { get; set; }

        public MemoryStream Export(List<UserInfo> list)
        {
            //创建流对象
            using (MemoryStream ms = new MemoryStream())
            {
                //将参数写入到一个临时集合中
                List<string> propertyNameList = new List<string>();
                HSSFWorkbook workbook = new HSSFWorkbook();
                HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet();
                HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);

                headerRow.CreateCell(0).SetCellValue("用户名");
                headerRow.CreateCell(1).SetCellValue("密码");
                headerRow.CreateCell(2).SetCellValue("负责人");
                headerRow.CreateCell(3).SetCellValue("所在单位");
                headerRow.CreateCell(4).SetCellValue("所在区（县）");
                headerRow.CreateCell(5).SetCellValue("所在城市");
                headerRow.CreateCell(6).SetCellValue("所在省份");

                if (list.Count > 0)
                {
                    int rowIndex = 1;
                    //遍历集合生成excel的行集数据
                    for (int i = 0; i < list.Count; i++)
                    {
                        HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);

                        dataRow.CreateCell(0).SetCellValue(list[i].Name);
                        dataRow.CreateCell(1).SetCellValue(list[i].Password);
                        dataRow.CreateCell(2).SetCellValue(list[i].ResponsiblePerson);
                        dataRow.CreateCell(3).SetCellValue(list[i].Company);
                        dataRow.CreateCell(4).SetCellValue(list[i].District);
                        dataRow.CreateCell(5).SetCellValue(list[i].City);
                        dataRow.CreateCell(6).SetCellValue(list[i].Province);
                        rowIndex++;
                    }
                }
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                return ms;
            }

        }
    }
}