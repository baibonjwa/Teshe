using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Teshe.Models;
using System.Data;

namespace Teshe.Service.Controllers
{
    public class GetDeviceInfoController : BaseController
    {
        public String GetDeviceInfo(String barcode)
        {
            Device device = db.Devices.FirstOrDefault<Device>(u => u.Barcode == barcode);
            return JsonConvert.SerializeObject(device, dateTimeConverter);
        }

    }
}
