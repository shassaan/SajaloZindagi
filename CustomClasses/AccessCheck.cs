using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using SajaloZindagi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SajaloZindagi.CustomClasses
{
    public class AccessCheck : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var admin = JsonConvert.DeserializeObject<Admin>(context.HttpContext.Session.GetString("Admin"));
            if (admin.AAccessType == false)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                        { "Controller","Admin"},
                        { "Action","Index"}
                    }

                    );
            }
        }
    }
}
