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
        }
    }
}