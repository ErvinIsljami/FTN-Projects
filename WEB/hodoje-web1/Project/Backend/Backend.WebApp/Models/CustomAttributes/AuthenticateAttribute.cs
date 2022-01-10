using Backend.AccessServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Threading;

namespace Backend.Models.CustomAttributes
{
    public class AuthenticationFilter : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            string encodedHash;
            var _accessService = actionContext.ControllerContext.Configuration.DependencyResolver.GetService(typeof(IAccessService)) as IAccessService;
            var authHeader = actionContext.Request.Headers.Authorization;
            if (authHeader == null || String.IsNullOrWhiteSpace(authHeader.Parameter))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                encodedHash = actionContext.Request.Headers.Authorization.Parameter;
                string hash = _accessService.ExtractHash(encodedHash);
                if (_accessService.IsLoggedIn(hash))
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(hash), null);
                    return;
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
        }
    }
}