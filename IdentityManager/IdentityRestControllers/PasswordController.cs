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
    public class PasswordController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public PasswordController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;

        }



        /// <summary>
        /// 这个方法不会返回用户密码，返回用户是否设置密码
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("v1/api/password/get/{id}")]
        public async Task<JsonResult> Get(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new JsonResult("Fail: 用户不存在");
            }
            bool result = await _userManager.HasPasswordAsync(user);
            return new JsonResult(result);
        }

        /// <summary>
        /// 为没有设置密码的用户设置密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>

        [HttpPost("v1/api/password/create/{id}")]
        public async Task<JsonResult> Post(string userId, string password)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new JsonResult("Fail: 用户不存在");
            }
            var result = await _userManager.AddPasswordAsync(user, password);
            if (!result.Succeeded)
                return new JsonResult("Fail:设置用户密码失败");
            return new JsonResult("Success:设置用户密码成功");
        }
        /// <summary>
        /// 这个不需要旧的密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newpassword"></param>
        /// <returns></returns>
        //这个办法不需要旧的密码
        [HttpPut("v1/api/password/update/{id}")]
        public async Task<JsonResult> Put(string userId, string newpassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new JsonResult("Fail: 用户不存在");
            }
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (token == "" || token == null)
                return new JsonResult("Fail: 修改令牌生成失败");
            var result = await _userManager.ResetPasswordAsync(user, token, newpassword);
            if (!result.Succeeded)
                return new JsonResult("Fail:重置用户密码失败");
            return new JsonResult("Success:重置用户密码成功");
        }


        /// <summary>
        /// 删除用户密码
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>

        [HttpDelete("v1/api/password/delete/{id}")]
        public async Task<JsonResult> Delete(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new JsonResult("Fail: 用户不存在");
            }
            var result = await _userManager.RemovePasswordAsync(user);
            if (!result.Succeeded)
                return new JsonResult("Fail:删除用户密码失败");
            return new JsonResult("Success:删除用户密码成功");
        }
    }
}