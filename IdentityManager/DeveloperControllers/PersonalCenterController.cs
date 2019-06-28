using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace IdentityManager.DeveloperControllers
{
    /// <summary>
    /// 已经大体无漏洞
    /// </summary>
    [Produces("application/json")]
    [Authorize(Policy = "User-policy")]
    /// <summary>
    /// 默认一开始初始化的时候，用户名是邮箱，如果用户不提供用户名的时候
    /// </summary>
    public class PersonalCenterController : ControllerBase
    {
        
        private readonly UserManager<IdentityUser> _userManager;
     
        public PersonalCenterController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            
        }

        /// <summary>
        /// 更改用户信息，比如邮箱电话等等
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="oldEmail"></param>
        /// <param name="newEmail"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        [HttpPost("/v1/api/userInfo")]
        public async Task<JsonResult> UserInfoPost(string userId, string userName, string oldEmail, string newEmail, int mobile)
        {

           
            //由于用户名目前在测试中和邮箱一直，更新的时候bug要注意
            //set这些变量的同时对应的密码也会被更新
            //验证用户存在
            var user = await _userManager.FindByEmailAsync(oldEmail);
            //需要验证新邮箱是否已被占用
            var existuser = await _userManager.FindByEmailAsync(newEmail);
            if (existuser!=null)
            {
                return new JsonResult(new ReturnFail("New Email already be used "));
            }
            if (user!=null && user.Id == userId )
            {
                //不清楚normalizedemail 和normalizedUsername 是否自动更新,到时候看看
                var result = await _userManager.SetUserNameAsync(user, userName);
                var result1 = await _userManager.SetPhoneNumberAsync(user, mobile.ToString());
                
                
                var result2 = await _userManager.SetEmailAsync(user, newEmail);
                if(result.Succeeded && result1.Succeeded && result2.Succeeded)
                    return new JsonResult(new ReturnSuccess());
                else
                    return new JsonResult(new ReturnFail("Update user Information failed"));
            }
            else
                return new JsonResult(new ReturnFail("User not exists or incorrect old email"));
                
        }


        /// <summary>
        /// 提供用户id和老密码，新密码来更改密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <param name="confirmPassword"></param>
        /// <returns></returns>
        [HttpPost("v1/api/password")]
        public async Task<JsonResult> ResetPassword(string userId, string oldPassword, string newPassword, string confirmPassword) 
        {
            //验证用户是否存在
            var user = await _userManager.FindByIdAsync(userId);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (user != null)
            {
                //需要验证旧的密码是否正确

                if (await _userManager.CheckPasswordAsync(user, oldPassword))
                {
                    if(oldPassword==newPassword)
                        return new JsonResult(new ReturnFail("New password is same as old password, no need change"));
                    //验证重输入密码是否一致
                    if (newPassword == confirmPassword)
                    {
                        //验证旧的密码是否正确
                        //其实resetpassword需要验证旧密码
                        //但是changepassword貌似不需要
                        var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
                        if (result.Succeeded)
                            return new JsonResult(true);
                        else
                            return new JsonResult(new ReturnFail("Reset password fail"));

                    }
                    else
                    {
                        return new JsonResult(new ReturnFail("New password not match confirm password"));
                    }
                }
                else
                    return new JsonResult(new ReturnFail("Old password not correct"));
            }
            else
            { 

                return new JsonResult(new ReturnFail("User not exists"));

            }
        }
 
    }
}