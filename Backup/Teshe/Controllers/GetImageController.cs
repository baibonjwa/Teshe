using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Channels;
using System.Web;
using System.Web.Http;
using Teshe.Common;

namespace Teshe.Controllers
{
    public class GetImageController : ApiController
    {
        public HttpResponseMessage GetImage(string imageName)
        {
            string path = HttpContext.Current.Server.MapPath("/FileUpload/Photo/" + imageName);
            Image img = new Bitmap(path);
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(ms.ToArray());
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            return result;
        }

        //等比缩放
        public HttpResponseMessage GetThumbNailImage(string imageName, int width, int height)
        {
            string path = HttpContext.Current.Server.MapPath("/FileUpload/Photo/" + imageName);
            Image img = new Bitmap(path);
            img = Helper.GetThumbNailImage(img, width, height);
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(ms.ToArray());
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            return result;
        }
    }
}
