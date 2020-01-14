using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RD.Lib;

namespace TTIS.WebUi.Controllers
{
    public class AuthController : RDController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Logout()
        {
            ClearCookie();
            return new SignOutResult(new[] { "oidc", "Cookies" });
        }
    }
}