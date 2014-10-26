using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Teshe.Models;

namespace Teshe.Service.Controllers
{
    public class BaseController : ApiController
    {
        protected TesheContext db = new TesheContext();
        protected IsoDateTimeConverter dateTimeConverter = new IsoDateTimeConverter();
    }
}
