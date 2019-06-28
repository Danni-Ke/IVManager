using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace IdentityManager.RestfulControllers
{
    [Authorize(Policy = "Admin-policy")]
    public class TwoFactorController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        public TwoFactorController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("v1/api/twoFactor/get/{id}")]
        public async Task<JsonResult> Get(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                //到时候确认下这个的正确用法
                bool result = await _userManager.GetTwoFactorEnabledAsync(user);
                return new JsonResult(result);
            }
            else
                return new JsonResult("Fail:用户不存在");

        }

        //true=enable,false=disable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPut("v1/api/twoFactor/update/{id}")]
        public async Task<JsonResult> Put(string userId, bool enable)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.SetTwoFactorEnabledAsync(user, enable);
                if (result.Succeeded)
                    return new JsonResult("Success：双因子认证设置成功");
                else
                    return new JsonResult("Fail: 双因子设置失败");
            }
            else
                return new JsonResult("Fail:用户不存在");
        }
    }
}