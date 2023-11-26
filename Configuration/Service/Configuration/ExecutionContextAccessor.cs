using Application.IService.Configuration;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Service.Configuration
{
    public class ExecutionContextAccessor : IExecutionContextAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExecutionContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId
        {
            get
            {
                try
                {
                    if (_httpContextAccessor
                    .HttpContext?
                    .User?
                    .Claims?
                    .SingleOrDefault(x => x.Type == "userId")?
                    .Value != null)
                    {
                        return _httpContextAccessor.HttpContext.User.Claims.Single(
                            x => x.Type == "userId").Value;
                    }

                    throw new ApplicationException("User context is not available");
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        public string UserType
        {
            get
            {
                try
                {
                    if (_httpContextAccessor
                    .HttpContext?
                    .User?
                    .Claims?
                    .SingleOrDefault(x => x.Type == "userType")?
                    .Value != null)
                    {
                        return _httpContextAccessor.HttpContext.User.Claims.Single(
                            x => x.Type == "userType").Value;
                    }

                    throw new ApplicationException("User context is not available");
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}
