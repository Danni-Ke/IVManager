using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IVSSO.Controllers
{
    [Produces("application/json")]
    //上面这个attribute会把输出变成json格式，倒是确认下用jsonResult还是这个属性
    //[Authorize(Policy = "All-policy")]
    [Route("api/demo")]
    public class UserController : Controller
    {
        
        [HttpGet]
        //返回当前用户是谁，测试用
        //sign out 之后应该是 返回这个 {"name":null,"isAuthenticated":false}
        //如果是signin的状况比对授权的时候会自动比对当前signin的人
        public object Get() {
            return new
            {
                User.Identity.Name,
                User.Identity.IsAuthenticated
            };
        }
    }
}