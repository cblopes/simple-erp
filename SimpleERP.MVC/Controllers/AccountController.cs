﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SimpleERP.MVC.Models;
using SimpleERP.MVC.Services;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace SimpleERP.MVC.Controllers
{
    public class AccountController : MainController
    {
        private readonly IAccountService _authService;

        public AccountController(IAccountService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUser userRegister)
        {
            if (!ModelState.IsValid) return View(userRegister);

            var response = await _authService.Register(userRegister);

            if (HasErrorsResponse(response.ResponseResult)) return View(userRegister);

            await LoginRelease(response);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUser loginUser, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid) return View(loginUser);

            var response = await _authService.Login(loginUser);

            if (HasErrorsResponse(response.ResponseResult)) return View(loginUser);

            await LoginRelease(response);

            if (returnUrl == null) return RedirectToAction("Index", "Home");

            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private async Task LoginRelease(ResponseUserLogin response)
        {
            var token = GetFormmatedToken(response.AccessToken);

            var claims = new List<Claim>();
            claims.Add(new Claim("JWT", response.AccessToken));
            claims.AddRange(token.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddSeconds(response.ExpiresIn),
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        private static JwtSecurityToken GetFormmatedToken(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        }
    }
}
