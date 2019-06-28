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
    public class UserIdController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        public UserIdController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("v1/api/userId/userName")]
        public async Task<JsonResult> GetbyName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                return new JsonResult("用户不存在");
            else
                return new JsonResult(user.Id);
        }


        [HttpGet("v1/api/userId/email")]
        public async Task<JsonResult> GetbyEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return new JsonResult("用户不存在");
            else
                return new JsonResult(user.Id);

        }
    }
}