using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityManager.RestfulControllers
{
    [Authorize(Policy = "Admin-policy")]

    public class UserRoleController : ControllerBase
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        
        public UserRoleController(UserManager<IdentityUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;

        }


        /// <summary>
        /// 指定用户角色是否存在
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="Role"></param>
        /// <returns></returns>
        [HttpGet("v1/api/userRole/{roleName}/{id}")]
        public async Task<JsonResult> Get(string userId, string RoleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            bool result = await _userManager.IsInRoleAsync(user, RoleName);
            if (result)
                return new JsonResult("用户角色存在");
            else
            {
                return new JsonResult("用户角色不存在");
            }
        }


        /// <summary>
        /// 用户添加角色，和Put
        /// 类似，所以put不多写
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="Role"></param>
        /// <returns></returns>
        [HttpPost("v1/api/userRole/add/{roleName}/{id}")]
        
        public async Task<JsonResult> Post(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                return new JsonResult("该角色不存在，请添加角色之后，再讲该用户添加到角色");
            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
                return new JsonResult("添加用户角色成功");
            else
            {
                AddErrors(result);
                return new JsonResult("添加用户角失败");
            }
        }

        /// <summary>
        /// 返回用户全部角色的字符串
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("v1/api/userRole/get/{id}")]
        public async Task<JsonResult> Get(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            IList<string> Role = await _userManager.GetRolesAsync(user);
            if (Role.Count == 0)
                return new JsonResult("该用户没有角色");
            return new JsonResult(Role);
        }

        /// <summary>
        /// 删除某个用户的角色
        /// </summary>
        /// <param name="result"></param>
        [HttpDelete("v1/api/userRole/delete/{roleName}/{id}")]
        public async Task<JsonResult> Delete(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            bool Isinrole = await _userManager.IsInRoleAsync(user, roleName);
            if (!Isinrole)
                return new JsonResult("该用户不在角色中，删除无效");
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (result.Succeeded)
                return new JsonResult("用户角色删除成功");
            else
            {
                AddErrors(result);
                return new JsonResult("用户角色删除失败");
            }
        }

       

         
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }


    }
}
