using IdentityManager.RolePolicy;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

namespace IdentityManager.Rolepolicy
{
    public class MyAuthorizationHandler : AuthorizationHandler<RoleRequirement>
    {
        //获取上下文信息，主要作用是活的客户端像服务端请求提交的相关信息，生存周期是从客户端点击并且产生了一
        //个向服务器发送请求的开始知道完成请求返回，这里是从postman测试点击开始到送回postman所要求的api结果为止。
        //每一个请求都会创立一个新的HttpContext实例，直到请求结束，服务器销毁这个实例，用于获取当前请求的临时信息。
        IHttpContextAccessor _httpContextAccessor = null;
        public MyAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 验证传入的要求是否符合，这里改为自己的办法验证
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {

            string jwtToken = GetToken().Result;
            if (jwtToken == null)
            {
                context.Fail();
                return Task.CompletedTask;
            }
            var jw = new JwtSecurityToken(jwtToken);
            var claims = jw.Claims;
            string userRole = "";
            foreach (var claim in claims)
            {
                if (claim.Type == "role")
                {
                    userRole = claim.Value;
                }
            }
            //假定只有用户和管理员两种角色，不会重叠，也不会有第三种
            foreach (var item in requirement.Roles)
            {
                //假如有多种要求，只要其中一种符合就返回
                if (item == userRole)
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }
            context.Fail();
            return Task.CompletedTask;

        }

        /// <summary>
        /// 获取当前上下文的access_token
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetToken()
        {
            //只要无法decoded被jwthandler，这里就会有空值
            HttpContext httpContext = _httpContextAccessor.HttpContext;
            return await AuthenticationHttpContextExtensions.GetTokenAsync(httpContext, "access_token");
            
        }
    }
}


