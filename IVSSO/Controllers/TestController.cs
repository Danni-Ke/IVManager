using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace IVSSO.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return Json(new
            {
                User.Identity.IsAuthenticated,
                User.Identity.AuthenticationType,
                Claims = User.Claims.Select(c => new { c.Type, c.Value })
            });
        }
    }
}