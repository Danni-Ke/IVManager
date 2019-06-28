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
    public class LockoutDateController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public LockoutDateController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("v1/api/lockoutDate/get/{id}")]
        public async Task<JsonResult> Get(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                //到时候确认下这个的正确用法
                var date = await _userManager.GetLockoutEndDateAsync(user);
                if (date == null)
                    return new JsonResult("Fail:尚未设置日期");
                return new JsonResult(date.ToString());
            }
            else
                return new JsonResult("Fail:用户不存在");

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPut("v1/api/lockoutDate/update/{id}")]
        public async Task<JsonResult> Put(string userId, string datetime)
        { 
            //格式必须为yyyy-MM-dd hh:mm:ss
            var date = Convert.ToDateTime(datetime);
            date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            //Console.WriteLine("Converted {0} {1} to a DateTimeOffset value of {2}",
                  //utcTime1,
                  //utcTime1.Kind.ToString(),
                  //utcTime2);
            // This example displays the following output to the console:
            //    Converted 6/19/2008 7:00:00 AM Utc to a DateTimeOffset value of 6/19/2008 7:00:00 AM +00:00
            DateTimeOffset newdate = date;
            //倒时候确定下date的具体格式
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {    if (!user.LockoutEnabled)
                    return new JsonResult("Fail：当前用户不允许被锁，请先上锁");
               // 设置过去的时间会解锁
                //没有await就不会回来继续这个任务，只返回线程等待的结果
                var result = await _userManager.SetLockoutEndDateAsync(user, newdate);
                if (result.Succeeded)
                    return new JsonResult("Success：更新成功");
                else
                    return new JsonResult("Fail: 设置日期失败，不合法日期格式或者日期");

            }
            else
                return new JsonResult("Fail:用户不存在");
        }
    }
}