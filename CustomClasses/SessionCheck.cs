using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SajaloZindagi.CustomClasses
{
    public class SessionCheck:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetString("Admin") == null)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                        { "Controller","Admin"},
                        { "Action","login"}

                    }
                    
                    );
            }
        }
    }
}
