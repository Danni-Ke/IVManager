using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityManager.RestfulControllers
{
    [Authorize(Policy = "Admin-policy")]
    public class LockedoutController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public LockedoutController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }


        /// <summary>
        /// 确认是否被lock
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>

        [HttpGet("v1/api/lockout/get/{id}")]
        public async Task<JsonResult> Get(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                bool result = await _userManager.IsLockedOutAsync(user);
                return new JsonResult(result);
            }
            else
                return new JsonResult("Fail:用户不存在");

        }
    }
}