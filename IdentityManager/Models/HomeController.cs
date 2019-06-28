using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Client.Controllers
{
    //test controller
    
    public class HomeController : Controller
    {
        //token错误的话返回的是401， 拒绝返回403
        //401->认证不通过
        //403->认证通过但是无授权
        //[Authorize(Roles ="User")]
        [Authorize(Policy = "Admin-policy")]
        [HttpGet]
        public IActionResult Index()
        {
            return new JsonResult("Hello World");
        }
        //test
        [Authorize]
        public IActionResult About()
        {
            //ViewData["Message"] = "Your application description page.";

            //return View();
            return Ok("Here it is");
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //[HttpPost]
        //http://localhost:5002/Home/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
            return View();
            //return new SignOutResult(new[] { "Cookies", "oidc" });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
