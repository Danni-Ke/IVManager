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

    public class RoleController : ControllerBase
    {
       
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        /// <summary>
        /// 返回该角色名字的id，还是返回角色名字？
        /// </summary>
        [HttpGet("v1/api/role/get/{roleName}")]
        public async Task<JsonResult> Get(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
                return new JsonResult(role.Id);
            else
                return new JsonResult("角色不存在");
        }

        /// <summary>
        /// 新建一个角色
        /// </summary>
        [HttpPost("v1/api/role/create")]
        public async Task<JsonResult> Post(string roleName)
        {
            
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                if (result.Succeeded)
                    return new JsonResult("角色创建成功");
                else
                    return new JsonResult("角色创建失败");
            }
            else
            {
                return new JsonResult("角色已经存在");
            }

        }

        [HttpDelete("v1/api/role/delete/{RoleName}")]
        public async Task<JsonResult> Delete(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                    return new JsonResult("角色删除成功！");
                else
                    return new JsonResult("角色删除失败");
            }
            else return new JsonResult("角色不存在");
        }

        /// <summary>
        /// 到时候看看也可以改成新名字和旧的名字
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="newRoleName"></param>
        /// <returns></returns>
        [HttpPut("v1/api/role/put/{id}")]

        public async Task<JsonResult> Put(string roleId, string newRoleName)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                role.Name = newRoleName;
                //不确定是否Normalizedname也更新,如果不更新这边自己手动改一下，如果不用normalized更好
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                    return new JsonResult("角色名字更新成功");
                else
                    return new JsonResult("角色名字更新失败");
            }
            else return new JsonResult("角色不存在");
        }

        
    }
}
