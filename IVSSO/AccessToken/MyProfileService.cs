using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using IdentityModel;

namespace IVSSO.AccessToken
{
    /// <summary>
    /// 自定义令牌附加的内容
    /// </summary>
    public class MyProfileService : IProfileService
    {

        private UserManager<IdentityUser> _userManager;

        public MyProfileService(UserManager<IdentityUser> userManager)
        {
            this._userManager = userManager;
        }

        //这个接口在token生成的时候会被用到,先测试看看是不是这个接口

        private async Task<List<Claim>> GetClaimsFromUser(IdentityUser user)
        {
            var claims = new List<Claim>();
            //claims.Add(new Claim("UserId", user.Id));
            //claims.Add(new Claim(JwtClaimTypes.Id, user.Id));
            //sub就是id
            claims.Add(new Claim(JwtClaimTypes.Name, user.UserName));
            claims.Add(new Claim(JwtClaimTypes.Email, user.Email));
            //把电话放在token里面可以删掉
            if (await _userManager.GetPhoneNumberAsync(user) != null)
            {
                claims.Add(new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber));
            }
            
            //添加角色自带服务
            var roleslist = await _userManager.GetRolesAsync(user);
            foreach (var role in roleslist)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, role));
            }
            return claims;

        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {

            var subjectId = context.Subject.Claims.FirstOrDefault(c => { return c.Type == "sub"; }).Value;
            var user = await _userManager.FindByIdAsync(subjectId);

            context.IssuedClaims = await GetClaimsFromUser(user);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {

            context.IsActive = true;

            var subjectId = context.Subject.Claims.FirstOrDefault(c => c.Type == "sub").Value;
            var user = await _userManager.FindByIdAsync(subjectId);
            context.IsActive = user != null;
        }

       
    }
}
