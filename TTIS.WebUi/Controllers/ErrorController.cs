using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace TTIS.WebUi.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult NoAccess()
        {
            return View();
        }
    }
}