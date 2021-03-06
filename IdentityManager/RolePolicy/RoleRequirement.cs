﻿using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityManager.RolePolicy
{
    public class RoleRequirement:IAuthorizationRequirement
    {
        //确定要包含的角色，来返回相应的要求政策
        public IEnumerable<string> Roles { get; }

        public RoleRequirement(params string[] roles)
        {
            Roles = roles?? throw new ArgumentNullException(nameof(roles));
        }
    }
}
