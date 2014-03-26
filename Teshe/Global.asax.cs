using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Teshe.Models;
using AutoMapper;

namespace Teshe
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public MvcApplication()
        {
            AuthorizeRequest += new EventHandler(MvcApplication_AuthorizeRequest);
        }

        void MvcApplication_AuthorizeRequest(object sender, EventArgs e)
        {
            IIdentity id = Context.User.Identity;
            if (id.IsAuthenticated)
            {
                string[] roles = new string[1];
                roles[0] = new TesheContext().UserInfoes.FirstOrDefault<UserInfo>(u => u.Name == id.Name).UserType.Name;
                Context.User = new GenericPrincipal(id, roles);
            }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            System.Data.Entity.Database.SetInitializer(
                new System.Data.Entity.MigrateDatabaseToLatestVersion<Models.TesheContext, Migrations.Configuration>());
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //
            log4net.Config.XmlConfigurator.Configure();
            log4net.ILog log = log4net.LogManager.GetLogger("MyLogger");
            log.Warn("应用程序启动");
            // 在应用程序启动时运行的代码  
            //Time_Task.Instance().ExecuteTask += new System.Timers.ElapsedEventHandler(Global_ExecuteTask);
            //Time_Task.Instance().Interval = 1000 * 86400;//表示间隔
            //Time_Task.Instance().Start();
            System.Timers.Timer myTimer = new System.Timers.Timer();
            myTimer.Elapsed += new System.Timers.ElapsedEventHandler(Global_ExecuteTask);
            myTimer.Interval = 1000 * 60 * 24;
            myTimer.Enabled = true;
        }
        void Global_ExecuteTask(object sender, System.Timers.ElapsedEventArgs e)
        {
            TesheContext db = new TesheContext();
            string strweek = e.SignalTime.DayOfWeek.ToString();
            DateTime date = e.SignalTime.Date;

            int inthour = e.SignalTime.Hour;
            int intminute = e.SignalTime.Minute;
            //int intSecond = e.SignalTime.Second;
            //int isecond = 00;
            log4net.ILog log = log4net.LogManager.GetLogger("MyLogger");
            List<Device> devicelist = db.Devices.ToList();
            List<UserInfo> userlist = db.UserInfoes.Where<UserInfo>(u => u.UserType.Name != "客户").ToList();

            foreach (var i in devicelist)
            {
                TimeSpan ts = i.CheckTime.AddDays(i.CheckCycle) - DateTime.Now;
                List<UserInfo> userDistrict = userlist.Where<UserInfo>(u => u.District == i.District && u.UserType.Name == "区（县）级管理员").ToList();
                List<UserInfo> userCity = userlist.Where<UserInfo>(u => u.City == i.City && u.UserType.Name == "市级管理员").ToList();
                List<UserInfo> userProvince = userlist.Where<UserInfo>(u => u.Province == i.Province && u.UserType.Name == "省级管理员").ToList();
                List<UserInfo> userSystem = userlist.Where<UserInfo>(u => u.UserType.Name == "系统管理员").ToList();
                if (ts.Days == 7 || ts.Days == 15)
                //if (true)
                {
                    foreach (var j in userDistrict)
                    {
                        Mail mail = new Mail();
                        mail.Contents = i.Company + "的" + i.Name + "(" + i.Model + ")" + "设备需要在" + i.CheckTime.AddDays(i.CheckCycle) + "前检查";
                        mail.IsRead = 0;
                        mail.ReceivedUser = j;
                        db.Mails.Add(mail);
                        db.SaveChanges();
                    }
                    foreach (var j in userCity)
                    {
                        Mail mail = new Mail();
                        mail.Contents = i.Company + "的" + i.Name + "(" + i.Model + ")" + "设备需要在" + i.CheckTime.AddDays(i.CheckCycle) + "前检查";
                        mail.IsRead = 0;
                        mail.ReceivedUser = j;
                        db.Mails.Add(mail);
                        db.SaveChanges();
                    }
                    foreach (var j in userProvince)
                    {
                        Mail mail = new Mail();
                        mail.Contents = i.City + i.District + i.Company + "的" + i.Name + "(" + i.Model + ")" + "设备需要在" + i.CheckTime.AddDays(i.CheckCycle) + "前检查";
                        mail.IsRead = 0;
                        mail.ReceivedUser = j;
                        db.Mails.Add(mail);
                        db.SaveChanges();
                    }
                    foreach (var j in userSystem)
                    {
                        Mail mail = new Mail();
                        mail.Contents = i.Company + "的" + i.Name + "(" + i.Model + ")" + "设备需要在" + i.CheckTime.AddDays(i.CheckCycle) + "前检查";
                        mail.IsRead = 0;
                        mail.ReceivedUser = j;
                        db.Mails.Add(mail);
                        db.SaveChanges();
                    }
                }
            }
            //if (inthour == ihour && intminute == iminute)
            //{

            //}
        }
    }
}