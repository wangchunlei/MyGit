using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Domas.Web.Tools.Authorize
{
    public class RequirePermissionFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var cTypeName = filterContext.Controller.GetType().UnderlyingSystemType.FullName;
            var success = false;

            success |= AuthManager.HasPermission(cTypeName,
                                                 filterContext.ActionDescriptor.ActionName,
                                                 filterContext.HttpContext.User.Identity.Name);

            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
            || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            //|| filterContext.HttpContext.User.Identity.Name.ToLower() == "admin";

            if (!skipAuthorization)
            {
                if (filterContext.HttpContext.User.Identity.IsAuthenticated && (filterContext.ActionDescriptor.IsDefined(typeof(AllowLoginedAttribute), true)
            || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowLoginedAttribute), true)))
                {
                    skipAuthorization = true;
                }
            }

            if (skipAuthorization || success)
            {
                var cache = filterContext.HttpContext.Response.Cache;
                cache.SetProxyMaxAge(new TimeSpan(0));
                cache.AddValidationCallback((HttpContext context, object data, ref HttpValidationStatus validationStatus) =>
                                                {
                                                    validationStatus =
                                                        this.OnCacheAuthorization(new HttpContextWrapper(context));
                                                }, null);
            }
            else
            {
                this.HandleUnauthorizedRequest(filterContext);
            }
        }

        private HttpValidationStatus OnCacheAuthorization(HttpContextWrapper httpContextWrapper)
        {
            return HttpValidationStatus.Valid;
        }

        private void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new HttpStatusCodeResult(500);
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult("无权限执行操作", null);
            }
        }
    }
}
