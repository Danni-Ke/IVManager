using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace IdentityManager.IdentityRestControllers
{
    [Authorize(Policy = "Admin-policy")]
    public class UserNameController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserNameController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;

        }

        [HttpGet("v1/api/userName/get/{id}")]
        public async Task<JsonResult> Get(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                string userName = await _userManager.GetUserNameAsync(user);
                if (userName != "")
                    return new JsonResult(userName);
                else
                    return new JsonResult("用户名没设定");
            }

            else
                return new JsonResult("Fail:用户不存在");
        }



        [HttpPut("v1/api/userName/update/{id}")]
        public async Task<JsonResult> Put(string userId, string newuserName)
        {
            if (newuserName == "")
                return new JsonResult("User name cannot be empty");
            var existuser = await _userManager.FindByNameAsync(newuserName);
            //保证新的用户名不是已存在用户的邮箱除了自己,暂时没添加这个条件
            //var existuser2 = await _userManager.FindByEmailAsync(newuserName);
            if (existuser != null)
                return new JsonResult("用户名已被占用");
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.SetUserNameAsync(user,newuserName);
                if (result.Succeeded)
                    return new JsonResult("用户名更新成功");
                else
                    return new JsonResult("用户名更新失败");
            }

            else
                return new JsonResult("Fail: 用户不存在");

        }
    }
}