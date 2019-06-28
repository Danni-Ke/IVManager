using System.Threading.Tasks;
using IVSSO.SSOControllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentityModel.Client;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace IVSSO.AccountControllers
{

   
    //全局这个覆盖单个
    //[AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
       
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signinManager)
        {
            _userManager = userManager;
            _signInManager = signinManager;
        }

        /// <summary>
        /// 一个新的版本获取令牌，由于DiscoveryClient可能在新的版本的里面面临废弃
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [Authorize(Policy = "All-policy")]
        [HttpPost("/v1/api/token")]
        public async Task<JsonResult> GetToken(string email, string password)
        {
            var client = new HttpClient();
            //这里的uri如果以后部署的地址不是这个，需要更改
            client.BaseAddress = new System.Uri("http://localhost:5000");
            var request = new HttpRequestMessage(HttpMethod.Post, "/connect/token");
            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("client_id", "Baidu API"));
            keyValues.Add(new KeyValuePair<string, string>("client_secret", "BaiduSecret"));
            keyValues.Add(new KeyValuePair<string, string>("grant_type", "password"));
            keyValues.Add(new KeyValuePair<string, string>("username", email));
            keyValues.Add(new KeyValuePair<string, string>("password", password));

            request.Content = new FormUrlEncodedContent(keyValues);
            var response = await client.SendAsync(request);
            if (response == null)
                return new JsonResult("Not response from the end point of token");
            
            else{
                //对content做处理,取出其中的令牌，作去头尾处理
                string a = await response.Content.ReadAsStringAsync();
                string[] Allparts = a.Split(",");
                string tokenpart = Allparts[0];
                string[] Firstpart = tokenpart.Split(":");
                string token = Firstpart[1];
                token = token.Trim('"');
                return new JsonResult(new KeyValuePair<string, string>("access_token", token));
            }
        }



        //这里的token url需要更改在正式上线服务器的时候
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("/v1/api/login")]
        public async Task<JsonResult> Login(string email, string password)
        {
            //需要记住我的参数吗？
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                //记住我参数现在是false，到时候问问看
                var result = await _signInManager.PasswordSignInAsync(email, password, false, lockoutOnFailure: false);
                
                if (result.Succeeded)
                {
                    //获取用户名字，id和身份
                    var data = new LoginReturnModel();
                    data.userId = await _userManager.GetUserIdAsync(user);
                    data.userName = await _userManager.GetUserNameAsync(user);
                    bool userInAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                    bool userInUser = await _userManager.IsInRoleAsync(user, "User");
                    //不太严谨，只要不是管理者都是用户，对于分配到不是用户也不是管理员的情况无法覆盖
                    //2代表既不是用户也不是管理员，后续可以删除, 因为早期有些用户我加入的时候没有添加角色，
                    data.userType = userInAdmin ? 1 : 0;
                    if (data.userType == 0)
                    {
                        data.userType = userInUser ? 0 : 2;
                    }
                    //获取access_token
                    //----------------------------------------------------------------
                    //正式发布的时候这里的authority需要更改下以取得正确的token
                    //----------------------------------------------------------------
                    var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
                    if (disco.IsError)
                    {
                        return new JsonResult(new LoginReturnModel(false, "DescoveryClient fail"));  
                    }
                    //这边API暂时是hard coded，到时候问需不需要包含别的API，或者再看看接口怎么自定义
                    //scope要和数据库定义的客户端相对应，统一用baiduAPI签发
                    var tokenClient = new TokenClient(disco.TokenEndpoint, "Baidu API", "BaiduSecret");
                    var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(email, password, "Api1 Api2");
                    if (tokenResponse.IsError)
                    {
                        return new JsonResult(new LoginReturnModel(false, "令牌生成错误"));
                    }
                    if (tokenResponse.AccessToken == null)
                    {
                        return new JsonResult(new LoginReturnModel(false, "令牌生成失败"));
                    }
                    data.access_Token = tokenResponse.AccessToken;

                    data.result = true;
                    //这里data内不包括token的过期时间和refresh token
                    return new JsonResult(data);
                }
                if(result.IsLockedOut)
                    return new JsonResult(new LoginReturnModel(false, "用户账户已被锁定"));
                else
                    return new JsonResult(new LoginReturnModel(false, "登录失败"));
            }
            else
                return new JsonResult(new LoginReturnModel(false, "用户不存在"));
        }


        /// <summary>
        /// 查询邮箱是否已经被注册
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("/v1/api/accountExist")]
        public async Task<JsonResult> AccountExist(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return new JsonResult(new ReturnFailModel("该邮已经注册"));
            }
            else
                return new JsonResult(true);
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="mobile"></param>
        /// <param name="password"></param>
        /// <param name="confirmPassword"></param>
        /// <param name="UserType"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("/v1/api/register")]
        public async Task<JsonResult> Register(string userName, string email, int mobile, string password, string confirmPassword, int UserType)
        {
            //角色不能为空
            //1是管理员，0是用户
            var user = new IdentityUser
            {
                UserName = userName,
                Email = email,
                PhoneNumber = mobile.ToString(),
            };
            //验证两次输入的密码是否正确
            if (password != confirmPassword)
                return new JsonResult(new ReturnFailModel("两次输入密码不一致"));
            else
            {
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    //identitytserver里面是这么做的，问问看到时候用哪个，logout里面也是
                    // AuthenticationProperties props = null;
                    // await HttpContext.SignInAsync(userName, email, props);
                    //注册后改为登录状态
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    var roleresult = UserType == 1 ? AddUserRole(user, "Admin") : AddUserRole(user, "User");
                    if (roleresult.IsCompletedSuccessfully)
                        return new JsonResult(new ReturnSuccessModel());
                    else { return new JsonResult(new ReturnFailModel("注册用户角色失败")); }
                }
                else
                    return new JsonResult(new ReturnFailModel("注册失败"));
            }
            //其他限定条件可以再看看，再添加
        }

        /// <summary>
        /// identity和identityServer4 signin/out方式不一样到时候再研究下
        /// </summary>
        /// <returns></returns>
        /// 这里不需要发token自动检测当前上下文的用户登录
        [Authorize(Policy = "All-policy")]
        [HttpPost("/v1/api/logout")]
        public async Task<JsonResult> Logout()
        {
            //IS4里面的做法
            //await HttpContext.SignOutAsync();
            await _signInManager.SignOutAsync();
            return new JsonResult("用户已退出！");
        }

        
        

        /// <summary>
        ///添加角色，比如用户还是管理员 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="Role"></param>
        /// <returns></returns>
        public async Task<IdentityResult> AddUserRole(IdentityUser user, string Role)
        {
           
            var result = await _userManager.AddToRoleAsync(user, Role);
            return result;
            
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