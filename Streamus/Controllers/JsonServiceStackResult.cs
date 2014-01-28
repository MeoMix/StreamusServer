using ServiceStack;
using Streamus.Domain.Interfaces;
using System;
using System.Web.Mvc;

namespace Streamus.Controllers
{
    /// <summary>
    /// A fast implementation of JSON serialization using Newtonsoft.
    /// http://stackoverflow.com/questions/7109967/using-json-net-as-default-json-serializer-in-asp-net-mvc-3-is-it-possible
    /// </summary>
    public class JsonServiceStackResult : JsonResult
    {
        public JsonServiceStackResult(object data)
        {
            //  Ensure that programmers are returning the proper entities.
            if (data is IAbstractDomainEntity<Guid> || data is IAbstractDomainEntity<string>)
            {
                throw new Exception("Attempted serialization of domain entity detected. Only DTOs should be serialized.");
            }

            Data = data;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;

            response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/json; charset=utf-8";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data != null)
            {
                response.Write(Data.ToJson());
            }
        }
    }
}