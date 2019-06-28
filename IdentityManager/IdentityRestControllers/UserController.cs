using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityManager.RestfulControllers
{
    /// <summary>
    ///pineappleman520@gmail.com是管理员账号,
    ///目前这里面删除是靠id，可以改成email
    /// </summary>
    //返回json
    //[Produces("application/json")]

    [Authorize(Policy = "Admin-policy")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        
        
        /// <summary>
        /// 创建新用户
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("v1/api/user/create")]
        [AllowAnonymous]
        public async Task<JsonResult> Post(string email, string password)
        {   
            if (await _userManager.FindByEmailAsync(email) != null)
            {
                return new JsonResult("User already exists");
            }
           
            var user = new IdentityUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
                return new JsonResult("User created successfully！");
            else
            {
                AddErrors(result);
                //return this.NotFound();
                //return StatusCode(406, "Error from PostUser");
                //content是在游览器显示
                return new JsonResult("Create user failed");
            }
        }


        /// <summary>
        /// 更新某个用户,再根据要求改一下参数到时候，更新自己的另外在passwordstore那里面写
        /// </summary>
        [HttpPut("v1/api/user/update/{id}")]
        public async Task<JsonResult> Put(string userId, string userName,string email, string phoneNumber)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new JsonResult("用户不存在");
            //只更新基础项目
            if (phoneNumber.Length != 11 || phoneNumber.All(Char.IsDigit))
            {
                return new JsonResult("电话号码不合法");
            }
            user.Email = email;
            user.UserName = userName;
            user.PhoneNumber = phoneNumber;
            //下面这些应该单独更新
            //user.PasswordHash = password;
            //user.EmailConfirmed = model.EmailConfirmed;
            //user.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            //user.TwoFactorEnabled = user.TwoFactorEnabled;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return new JsonResult("更新用户资料成功");
            else
            {
                AddErrors(result);
                return new JsonResult("更新用户资料失败");
            }
        }

        /// <summary>
        /// 请求某个用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        //[Authorize (Roles ="Admin")]
        [HttpGet("v1/api/user/get/{id}")]
        public async Task<JsonResult> Get(string userId)
        {
            //判断错误交给别的函数
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
                return new JsonResult(user);
            else
                return new JsonResult("用户不存在，请求无效");
        }


        /// <summary>
        /// 删除某个用户,返回type的话,删除自己的另外写
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns> 
        
        [HttpDelete("v1/api/user/delete/{id}")]
        
        public async Task<JsonResult> Delete(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new JsonResult("用户不存在，请求无效");
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return new JsonResult("删除用户成功!");
            else
            {
                AddErrors(result);
                return new JsonResult("删除用户失败");
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