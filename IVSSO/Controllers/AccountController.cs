// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IVSSO.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using IdentityServer4.Stores;
/// <summary>
/// this page is especially for SSO Login
/// </summary>
namespace IdentityServer4.Quickstart.UI
{
    /// <summary>
    /// This sample controller implements a typical login/logout/provision workflow for local and external accounts.
    /// The login service encapsulates the interactions with the user data store. This data store is in-memory only and cannot be used for production!
    /// The interaction service provides a way for the UI to communicate with identityserver for validation and context retrieval
    /// </summary>
    [AllowAnonymous]
    
    public class AccountController : Controller
    {
 
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger _logger;
        private readonly IClientStore _clientStore;


        public AccountController(
            SignInManager<IdentityUser> signInManager,
            IIdentityServerInteractionService interaction,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            IClientStore clientStore,
            ILoggerFactory loggerFactory)
        {
            // if the TestUserStore is not in DI, then we'll just use the global users collection
            // this is where you would plug in your own custom identity management library (e.g. ASP.NET Identity

            _interaction = interaction;
            _schemeProvider = schemeProvider;
            _events = events;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _clientStore = clientStore;
            _signInManager = signInManager;
        }
        //testAPI
        [HttpPost("User/Log")]
        public async Task<IActionResult> Log(string Email, string Password)
        {
            //postman 里面的。。。会附带/n导致认证失败
            Password = "Gnm19980521!";
            var result = await _signInManager.PasswordSignInAsync(Email, Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return Ok("Log Successful!");
            }
            else
                return Ok("Bad");
        }

        /// <summary>
        /// Entry point into the login workflow
        /// GET: /Account/Login
        /// </summary>
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            // build a model so we know what to show on th_signInManager = signInManager;
            ViewData["returnUrl"] = returnUrl;

            return View();
        }

        /// <summary>
        /// Handle postback from username/password login
        /// POST: /Account/Login
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Login(LoginInputModel model, string returnUrl=null)
        {
            
            if (ModelState.IsValid)
            {
                ViewData["returnUrl"] = returnUrl;
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {

                    _logger.LogInformation(1, "User logged in.");
                    if (returnUrl != null)
                    {
                    // return RedirectToLocal(returnUrl);
                    return RedirectToLocal(returnUrl);
                    }
                    else {
                    ModelState.AddModelError(string.Empty, "Invalid returnUrl");
                    return View(model);
                    }
                    
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, "User account locked out.");
                    return View("Lockout");
                }
                else
                {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
                }
            }
           return View(model);
        }


        [HttpPost]
        
        public async Task<IActionResult> LogOut(LogoutInputModel model)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToPage("Home/Index"); ;
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(WebApplication1.Controllers.HomeController.Index), "Home");
            }
        }
        

    }
}