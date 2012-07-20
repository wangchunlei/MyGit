using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Domas.Web.Models;
using Domas.Web.Tools.Authorize.Models;

namespace Domas.Web.Tools.Authorize
{
    public class ActionLogFilterAttribute : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            using (var context = new UIComponentContext())
            {
                var log = new UIActionLogging()
                              {
                                  User = filterContext.HttpContext.User.Identity.Name,
                                  Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                                  Action = filterContext.ActionDescriptor.ActionName,
                                  IP = filterContext.HttpContext.Request.UserHostAddress,
                                  DateTime = filterContext.HttpContext.Timestamp
                              };
                context.UiActionLoggings.Add(log);
                context.SaveChanges();
            }
        }
    }
}
