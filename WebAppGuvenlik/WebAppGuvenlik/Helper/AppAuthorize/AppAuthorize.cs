using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppGuvenlik.Helper.SessionHelper;

namespace WebAppGuvenlik.Helper.AppAuthorize
{
    public class AppAuthorize : AuthorizeAttribute
    {
        private readonly int[] allowedRoleIds;
        public AppAuthorize(params int[] roleIds)
        {
            this.allowedRoleIds = roleIds;

        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            bool result = false;
            if (SessionHelper.SessionHelper.IsAuthenticated && allowedRoleIds.Contains(SessionHelper.SessionHelper.CurrentUser.RoleId))
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {


            if (!SessionHelper.SessionHelper.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Account", action = "Login" }));
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Account", action = "NotAuthorized" }));
            }     
        }
    }
}