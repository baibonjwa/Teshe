using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Teshe.Service.Controllers
{
    public class GetImageController : ApiController
    {
        public HttpResponseMessage Get(string imageName, int width, int height)
        {
            Image img = new Bitmap("/FileUpload/Photo/" + imageName);
            MemoryStream ms = new MemoryStream();
            img.Save(ms,System.Drawing.Imaging.ImageFormat.Png);
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(ms.ToArray());
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            return result;
        }
    }
}
