using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace IdentityManager.RestfulControllers
{
    [Authorize(Policy = "Admin-policy")]
    public class UserClaimController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        public UserClaimController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }


        [HttpPost("v1/api/userClaim/create/{id}")]
        public async Task<JsonResult> Post(string userId, string claimType, string claimValue)
        {
            var claim = new Claim(claimType, claimValue);
            var result = await _userManager.AddClaimAsync(await _userManager.FindByIdAsync(userId), claim);
            if (result.Succeeded)
                return new JsonResult("Success:添加用户声明成功");
            else
                return new JsonResult("Fail:添加用户声明失败");
        }

        //目前不知道json是否能返回list
        /// <summary>
        /// 获取一个用户的所有claim
        /// </summary>
        [HttpGet("v1/api/userClaim/get/{id}")]
        public async Task<JsonResult> Get(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                IList<Claim> claimlist = await _userManager.GetClaimsAsync(user);
               
                if (claimlist.Count > 0)
                    return new JsonResult(claimlist);
                else
                    return new JsonResult("Success: 当前用户没有声明");

            }
            else
                return new JsonResult("Fail: 用户不存在");
        }


        [HttpDelete("v1/api/userClaim/delete/{claimType}/{id}")]
        public async Task<JsonResult> Delete(string userId, string claimType)
        {
            string claimValue="";
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                IList<Claim> claimlist = await _userManager.GetClaimsAsync(user);
                foreach(var item in claimlist)
                {
                    if (item.Type == claimType)
                        claimValue = item.Value;
                }
                if (claimValue == null)
                    return new JsonResult("Fail:该用户声明不存在.");
                var result = await _userManager.RemoveClaimAsync(user,new Claim(claimType, claimValue));
                if (result.Succeeded)
                    return new JsonResult("Success:删除声明成功");
                else
                    return new JsonResult("Fail:删除声明失败");
            }
            else
                return new JsonResult("Fail:用户不存在");
        }
    }
}