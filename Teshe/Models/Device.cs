using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using NPOI.HSSF.UserModel;

namespace Teshe.Models
{
    public class Device
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("设备名称")]
        [Required(ErrorMessage = "设备名称不能为空")]
        public string Name { get; set; }

        [DisplayName("设备型号")]
        [Required(ErrorMessage = "设备型号不能为空")]
        public string Model { get; set; }

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

        public MemoryStream Export(List<Device> list)
        {
            //创建流对象

            using (MemoryStream ms = new MemoryStream())
            {
                //将参数写入到一个临时集合中
                List<string> propertyNameList = new List<string>();
                HSSFWorkbook workbook = new HSSFWorkbook();
                HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet();
                HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);

                headerRow.CreateCell(0).SetCellValue("设备名称");
                headerRow.CreateCell(1).SetCellValue("设备型号");
                headerRow.CreateCell(2).SetCellValue("条形码");
                headerRow.CreateCell(3).SetCellValue("所在公司");
                headerRow.CreateCell(4).SetCellValue("所在区（县）");
                headerRow.CreateCell(5).SetCellValue("所在城市");
                headerRow.CreateCell(6).SetCellValue("所在省份");
                headerRow.CreateCell(7).SetCellValue("出厂日期");
                headerRow.CreateCell(8).SetCellValue("生产厂家");
                headerRow.CreateCell(9).SetCellValue("安装时间");
                headerRow.CreateCell(10).SetCellValue("防爆否");
                headerRow.CreateCell(11).SetCellValue("安检证号");
                headerRow.CreateCell(12).SetCellValue("检测状态");
                headerRow.CreateCell(13).SetCellValue("检测时间");
                headerRow.CreateCell(14).SetCellValue("使用状态");
                headerRow.CreateCell(15).SetCellValue("维修记录");

                if (list.Count > 0)
                {
                    //通过反射得到对象的属性集合
                    //PropertyInfo[] propertys = list[0].GetType().GetProperties();
                    ////遍历属性集合生成excel的表头标题
                    //for (int i = 0; i < propertys.Count(); i++)
                    //{
                    //    //判断此属性是否是用户定义属性
                    //    if (propertyNameList.Count == 0)
                    //    {
                    //        headerRow.CreateCell(i).SetCellValue(propertys[i].Name);
                    //    }
                    //    else
                    //    {
                    //        if (propertyNameList.Contains(propertys[i].Name))
                    //            headerRow.CreateCell(i).SetCellValue(propertys[i].Name);
                    //    }
                    //}
                    int rowIndex = 1;
                    //遍历集合生成excel的行集数据
                    for (int i = 0; i < list.Count; i++)
                    {
                        HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);

                        dataRow.CreateCell(0).SetCellValue(list[i].Name);
                        dataRow.CreateCell(1).SetCellValue(list[i].Model);
                        dataRow.CreateCell(2).SetCellValue(list[i].Barcode);
                        dataRow.CreateCell(3).SetCellValue(list[i].Company);
                        dataRow.CreateCell(4).SetCellValue(list[i].District);
                        dataRow.CreateCell(5).SetCellValue(list[i].City);
                        dataRow.CreateCell(6).SetCellValue(list[i].Province);
                        dataRow.CreateCell(7).SetCellValue(list[i].ManufactureDate);
                        dataRow.CreateCell(8).SetCellValue(list[i].Factory);
                        dataRow.CreateCell(9).SetCellValue(list[i].SetupTime);
                        dataRow.CreateCell(10).SetCellValue(list[i].ExplosionProof);
                        dataRow.CreateCell(11).SetCellValue(list[i].SecurityCertificateNo);
                        dataRow.CreateCell(12).SetCellValue(list[i].CheckState);
                        dataRow.CreateCell(13).SetCellValue(list[i].CheckTime);
                        dataRow.CreateCell(14).SetCellValue(list[i].UseState);
                        dataRow.CreateCell(15).SetCellValue(list[i].MaintenanceRecord);
                        //dataRow.CreateCell(16).SetCellValue(list[i].Review.ToString() == "0" ? "否" : "是");
                        //dataRow.CreateCell(17).SetCellValue(list[i].Irrgularity == null ? "数据错误" : list[i].Irrgularity.Name);
                        //if (propertyNameList.Count == 0)
                        //{
                        //    object obj = propertys[j].GetValue(list[i], null);
                        //    if (obj == null)
                        //    {
                        //        dataRow.CreateCell(j).SetCellValue("无值");
                        //    }
                        //    else
                        //    {
                        //        dataRow.CreateCell(j).SetCellValue(obj.ToString());
                        //    }

                        //}
                        //else
                        //{
                        //    if (propertyNameList.Contains(propertys[j].Name))
                        //    {
                        //        object obj = propertys[j].GetValue(list[i], null);
                        //        dataRow.CreateCell(j).SetCellValue(obj.ToString());
                        //    }
                        //}
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