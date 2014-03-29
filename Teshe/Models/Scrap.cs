﻿using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;

namespace Teshe.Models
{
    public class Scrap
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("设备")]
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

        [DisplayName("修改记录")]
        public virtual List<Scrap> ModifyRecords { get; set; }

        public MemoryStream Export(List<Scrap> list)
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
                headerRow.CreateCell(7).SetCellValue("故障时间");
                headerRow.CreateCell(8).SetCellValue("故障描述");

                if (list.Count > 0)
                {
                    int rowIndex = 1;
                    //遍历集合生成excel的行集数据
                    for (int i = 0; i < list.Count; i++)
                    {
                        HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);

                        dataRow.CreateCell(0).SetCellValue(list[i].Device.Name);
                        dataRow.CreateCell(1).SetCellValue(list[i].Device.Model);
                        dataRow.CreateCell(2).SetCellValue(list[i].Device.Barcode);
                        dataRow.CreateCell(3).SetCellValue(list[i].Device.Company);
                        dataRow.CreateCell(4).SetCellValue(list[i].Device.District);
                        dataRow.CreateCell(5).SetCellValue(list[i].Device.City);
                        dataRow.CreateCell(6).SetCellValue(list[i].Device.Province);
                        dataRow.CreateCell(7).SetCellValue(list[i].ScrapTime);
                        dataRow.CreateCell(8).SetCellValue(list[i].Description);
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