using Application.IService.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authorization
{
    internal class HasPermissionAuthorizationHandler : AttributeAuthorizationHandler<
        HasPermissionAuthorizationRequirement, HasPermissionAttribute>
    {
        private readonly IExecutionContextAccessor _executionContextAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HasPermissionAuthorizationHandler(
            IExecutionContextAccessor executionContextAccessor,
            IHttpContextAccessor httpContextAccessor)
        {
            _executionContextAccessor = executionContextAccessor;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            HasPermissionAuthorizationRequirement requirement,
            HasPermissionAttribute attribute)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            byte[] bytes;
            string msg = string.Empty;
            try
            {
                var userID = _executionContextAccessor.UserId;

                if (userID == null)
                {
                    var res = new { success = false, returnMess = "Unauthorized", returnMessCode = "" };
                    msg = JsonConvert.SerializeObject(res);
                    bytes = Encoding.UTF8.GetBytes(msg);
                    httpContext.Response.StatusCode = 401;
                    httpContext.Response.ContentType = "application/json";
                    _ = httpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);
                    context.Fail();
                    return;
                }

                

                context.Succeed(requirement);
            }
            catch (Exception ex)
            {
                httpContext.Response.StatusCode = 400;
                httpContext.Response.ContentType = "application/json";
                bytes = Encoding.UTF8.GetBytes(ex.ToString());
                _ = httpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);
                context.Fail();
                return;
            }
        }

       
    }

    internal class HasPermissionsAuthorizationHandler : AttributeAuthorizationHandler<
        HasPermissionsAuthorizationRequirement, HasPermissionsAttribute>
    {
        private readonly IExecutionContextAccessor _executionContextAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HasPermissionsAuthorizationHandler(
            IExecutionContextAccessor executionContextAccessor,
            IHttpContextAccessor httpContextAccessor)
        {
            _executionContextAccessor = executionContextAccessor;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            HasPermissionsAuthorizationRequirement requirement,
            HasPermissionsAttribute attribute)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            byte[] bytes;
            string msg = string.Empty;
            var userID = _executionContextAccessor.UserId;

            if (userID == null)
            {
                var res = new { success = false, returnMess = "Unauthorized", returnMessCode = "" };
                msg = JsonConvert.SerializeObject(res);
                bytes = Encoding.UTF8.GetBytes(msg);
                httpContext.Response.StatusCode = 401;
                httpContext.Response.ContentType = "application/json";
                _ = httpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);
                context.Fail();
                return;
            }

            

            context.Succeed(requirement);
        }

    }
}

