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
    //这个暂时不使用
    [Authorize(Policy = "Admin-policy")]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        public LoginController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("v1/api/login/Create/{id}")]
        public async Task<JsonResult> Post(string userId, UserLoginInfo login)
        {
            await _userManager.AddLoginAsync(await _userManager.FindByIdAsync(userId), login);
            return new JsonResult("OK");
        }


        [HttpGet("v1/api/login/Get/{id}")]
        public async Task<JsonResult> Get(string userId)
        {
            return new JsonResult
            (
                await _userManager.GetLoginsAsync(await _userManager.FindByIdAsync(userId))
            );

        }

        [HttpDelete("v1/api/login/Delete/{id}")]
        public async Task<JsonResult> Delete(string userId, string loginProvider, string providerKey)
        {
            var user = await _userManager.FindByIdAsync(userId);
            UserLoginInfo login = (await _userManager.GetLoginsAsync(user))
                                                    .SingleOrDefault(l => l.ProviderKey == providerKey);

            if (login == null)
                return new JsonResult(NotFound());

            await _userManager.RemoveLoginAsync(await _userManager.FindByIdAsync(userId), loginProvider, providerKey);

            return new JsonResult("OK");
        }
    }
}