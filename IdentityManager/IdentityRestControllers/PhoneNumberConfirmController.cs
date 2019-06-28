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
    //[Authorize(Policy = "Admin-policy")]
    public class PhoneNumberConfirmController : ControllerBase
    {
        
        private readonly UserManager<IdentityUser> _userManager;

        public PhoneNumberConfirmController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// 得到指定用户的电话是否被确认
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("v1/api/phoneNumberConfirm/get/{id}")]
        public async Task<JsonResult> Get(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                bool result = await _userManager.IsPhoneNumberConfirmedAsync(user);
                if (result)
                    return new JsonResult(true);
                else
                    return new JsonResult(false);
            }
            else
                return new JsonResult("Fail:用户不存在");
        }

    }
}