using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityManager.Controllers
{

    [Authorize(Policy = "Admin-policy")]
    public class EmailConfirmController : ControllerBase
    {

        private readonly UserManager<IdentityUser> _userManager;

        public EmailConfirmController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;

        }

        /// <summary>
        /// 查询邮件是否确认的标识符状态，已确认的话返回true，否则false
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("v1/api/emailConfirm/get/{id}")]
        //这个{id}是不是这里都一样，不影响是谁，只影响路由结果
        //v1/api/emailConfirm/不会路由到这里
        public async Task<JsonResult> Get(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                bool result = await _userManager.IsEmailConfirmedAsync(user);
                if (result)
                    return new JsonResult(true);
                else
                    return new JsonResult(false);
            }
            else
                return new JsonResult("Fail: User not exist。");
        }
        
        /// <summary>
        /// 设置邮件确认标识符,一旦设置之后不会再变成false，除非更换新的邮件地址
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPut("v1/api/emailConfirm/put/{id}")]
        public async Task<JsonResult> Put(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                if (token == null)
                    return new JsonResult("Fail: Unable to generate the token");
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                    return new JsonResult("Success: Email confirm set successfully");
                else
                    return new JsonResult("Fail: Email confirm set fail");
            }
            else
                return new JsonResult("Fail: User not exist。");
        }

    }
}