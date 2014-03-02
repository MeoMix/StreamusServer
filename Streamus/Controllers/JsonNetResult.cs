using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Streamus.Controllers
{
    //  http://wingkaiwan.com/2012/12/28/replacing-mvc-javascriptserializer-with-json-net-jsonserializer/
    public class JsonNetResult : JsonResult
    {
        public JsonNetResult()
        {
            Settings = new JsonSerializerSettings
                {
                    //  TODO: Maybe in the future I'd support circular references? Not sure.
                    ReferenceLoopHandling = ReferenceLoopHandling.Error
                };
        }

        public JsonSerializerSettings Settings { get; private set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(ContentType) ? "application/json" : ContentType;

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;
            if (Data == null)
                return;

            JsonSerializer scriptSerializer = JsonSerializer.Create(Settings);

            using (var sw = new StringWriter())
            {
                scriptSerializer.Serialize(sw, Data);
                response.Write(sw.ToString());
            }
        }
    }
}