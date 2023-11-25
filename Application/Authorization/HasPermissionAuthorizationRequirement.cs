using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Authorization
{
    public class HasPermissionAuthorizationRequirement : IAuthorizationRequirement
    {
    }

    public class HasPermissionsAuthorizationRequirement : IAuthorizationRequirement
    {
    }
}
