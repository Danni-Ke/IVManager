using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityManager.RestfulControllers
{
   
    [Authorize(Policy = "Admin-policy")]
    /// <summary>
    /// 由于邮箱地址在创建用户或者注册时候已经有了，所以没有post方法，也没有
    /// delete，删除用户就会自动删除邮箱，删除最好不要传送空的邮箱string，容易造成漏洞崩溃
    /// 而且有很多搜索以email作为基础，别删除。
    /// </summary>
    public class EmailController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public EmailController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }


        /// <summary>
        /// 获取指定用户的email，一般不会用到，因为注册的时候就有了
        /// 不存在email为空的状态，只要注册必须要有
        /// </summary>
        [HttpGet("v1/api/email/get/{id}")]
        public async Task<JsonResult> Get(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                string email = await _userManager.GetEmailAsync(user);
                return new JsonResult(email);
            }
            else
                return new JsonResult("Fail: 用户不存在");

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPut("v1/api/email/update/{id}")]
        public async Task<JsonResult> Put(string userId, string newEmail)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var existuser = await _userManager.FindByEmailAsync(newEmail);
                if (existuser != null)
                    return new JsonResult("Fail: 更新邮件地址失败，新邮件地址已被占用");
                var result  = await _userManager.SetEmailAsync(user, newEmail);
                if (result.Succeeded)
                    return new JsonResult("Success: 成功更新邮件地址!");
                else
                    return new JsonResult("Fail: 更新邮件地址失败");
            }
            else
                return new JsonResult("Fail: 用户不存在");
        }
    }
}