using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Streamus.Controllers.Attributes
{
    public class BulkSessionBatchSizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            //Session.SetBatchSize(playlistsList.Count / 3);

            //if (playlistsList.Count > 1000)
            //{
            //    Session.SetBatchSize(playlistsList.Count / 10);
            //}
            //else if (playlistsList.Count > 3)
            //{
            //    
            //}
        }

        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
        }
    }
}