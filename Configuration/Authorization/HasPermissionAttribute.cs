using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    internal class HasPermissionAttribute : AuthorizeAttribute
    {
        internal const string HasPermissionPolicyName = "HasPermission";

        public HasPermissionAttribute(string name)
            : base(HasPermissionPolicyName)
        {
            Name = name;
        }

        public string Name { get; }
    }
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    internal class HasPermissionsAttribute : AuthorizeAttribute
    {
        internal const string HasPermissionsPolicyName = "HasPermissions";

        public HasPermissionsAttribute(string[] names)
            : base(HasPermissionsPolicyName)
        {
            Names = names;
        }

        public string[] Names { get; }
    }
}
