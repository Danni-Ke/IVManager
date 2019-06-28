using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVSSO.RolePolicy
{
    public class MyAuthorizationHandler : AuthorizationHandler<RoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            //由于这里面的context.user 比对的是目前登录的人，如果是本身服务器上的API，无需比对token里面的，
            //直接当前登录用户即可比对即可，如果不允许访问的资源返回accessdenied，得先去服务器登录
            foreach (var item in requirement.Roles)
            {
                if (context.User.IsInRole(item))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }
            context.Fail();
            return Task.CompletedTask;
        }
    }
}
