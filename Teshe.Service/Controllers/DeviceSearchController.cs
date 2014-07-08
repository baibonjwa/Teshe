using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using Teshe.Common;
using Teshe.Models;

namespace Teshe.Service.Controllers
{
    public class DeviceSearchController : BaseController
    {
        [HttpGet]
        public List<Device> Search(int userId, string name = "", string model = "", string barcode = "", string company = "", string district = "", string city = "", string province = "", DateTime? setupTime = null, string checkState = "")
        {
            Expression<Func<Device, bool>> where = PredicateExtensionses.True<Device>();
            where = where.And(u => u.IsVerify == 1);
            bool isfirst = true;
            var viewModel = new DeviceIndexViewModel()
            {
                Name = name,
                Model = model,
                Barcode = barcode,
                Company = company,
                District = district,
                City = city,
                Province = province,
                SetupTime = setupTime,
                CheckState = checkState
            };
            PropertyInfo[] pro = viewModel.GetType().GetProperties();
            foreach (var p in pro)
            {
                if (p.GetValue(viewModel, null) != null)
                {
                    isfirst = false;
                    break;
                }
            }
            if (isfirst)
            {
                UserInfo user = db.UserInfoes.Find(userId);
                if (user.UserType.Name == "区（县）级管理员")
                {
                    where = where.And(u => u.District == user.District);
                    where = where.And(u => u.City == user.City);
                    where = where.And(u => u.Province == user.Province);
                }
                else if (user.UserType.Name == "市级管理员")
                {
                    where = where.And(u => u.City == user.City);
                    where = where.And(u => u.Province == user.Province);
                }
                else if (user.UserType.Name == "省级管理员")
                {
                    where = where.And(u => u.Province == user.Province);
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(viewModel.Name)) where = where.And(u => u.Name == viewModel.Name);
                if (!String.IsNullOrEmpty(viewModel.Model)) where = where.And(u => u.Model == viewModel.Model);
                if (viewModel.SetupTime != null) where = where.And(u => u.SetupTime == viewModel.SetupTime);
                if (!String.IsNullOrEmpty(viewModel.Company)) where = where.And(u => u.Company == viewModel.Company);
                if (!String.IsNullOrEmpty(viewModel.Barcode)) where = where.And(u => u.Barcode == viewModel.Barcode);
                if (!String.IsNullOrEmpty(viewModel.CheckState)) where = where.And(u => u.CheckState == viewModel.CheckState);
                if (!String.IsNullOrEmpty(viewModel.District)) where = where.And(u => u.District == viewModel.District);
                if (!String.IsNullOrEmpty(viewModel.City)) where = where.And(u => u.City == viewModel.City);
                if (!String.IsNullOrEmpty(viewModel.Province)) where = where.And(u => u.Province == viewModel.Province);
            }
            List<Device> results = db.Devices.Where<Device>(where).ToList();
            return results;
        }

    }
}
