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
    public class PhoneNumberController : ControllerBase
    {
        /// <summary>
        /// 和邮件不一样，注册的时候是没有设定的
        /// </summary>
        private readonly UserManager<IdentityUser> _userManager;
        public PhoneNumberController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("v1/api/phoneNumber/get/{id}")]
        public async Task<JsonResult> Get(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                string mobile = await _userManager.GetPhoneNumberAsync(user);
                if (mobile != "" && mobile != null&& mobile!="0")
                {
                    return new JsonResult(mobile);
                }
                else
                    return new JsonResult("Fail:用户没有设置电话号码");
            }
            else
                return new JsonResult("Fail:用户不存在");
        }



        /// <summary>
        /// 和post一样，post不需要id！！！！
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newMobile"></param>
        /// <returns></returns>
        [HttpPut("api/v1/phoneNumber/update/{id}")]
        public async Task<JsonResult> Put(string userId, string mobile)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                if (mobile.Length == 11 && mobile.All(Char.IsDigit))
                {
                    //暂时不知道这个token的用途，测试的时候看看，如果没效果可以直接用setphonenumber
                    string token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, mobile);
                    if (await _userManager.VerifyChangePhoneNumberTokenAsync(user, token, mobile))
                    {
                        var result = await _userManager.SetPhoneNumberAsync(user, mobile);
                        if (result.Succeeded)
                            return new JsonResult("Success:更新电话号码成功");
                        else
                            return new JsonResult("Fail:更新电话号码失败");
                    }
                    else
                        return new JsonResult("Fail: 生成更新令牌失败");
                }

                else
                    return new JsonResult("Fail:电话号码不合法");

            }
            else
                return new JsonResult("Fail:用户不存在");


        }

        /// <summary>
        /// 删除电话号码,只是把数据库电话号码变成空字符串，不是null
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>

        [HttpDelete("v1/api/phoneNumber/delete/{id}")]
        public async Task<JsonResult> Delete(string userId)
        {
            //到时候看看空的能不能被测试
            string newMobile = "";
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                //暂时不知道这个token的用途，测试的时候看看，如果没效果可以直接用setphonenumber
                string token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, newMobile);
                if (await _userManager.VerifyChangePhoneNumberTokenAsync(user, token, newMobile))
                {
                    var result = await _userManager.SetPhoneNumberAsync(user, newMobile);
                    if (result.Succeeded)
                        return new JsonResult("Success:删除电话号码成功");
                    else
                        return new JsonResult("Fail:删除电话号码失败");
                }
                else
                    return new JsonResult("Fail:生成令牌失败");
            }

            else
                return new JsonResult("Fail:用户不存在");

        }
    }   
}
