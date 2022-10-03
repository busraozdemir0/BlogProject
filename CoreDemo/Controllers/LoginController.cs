using CoreDemo.Models;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using EntityLayer.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;

        public LoginController(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(UserSignInViewModel p)
        {
            Context context = new Context();
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(p.username, p.password, false, true);
                if (result.Succeeded)
                {
                    var name = context.Users.Where(x => x.UserName == p.username).Select(y => y.NameSurname).FirstOrDefault();
                    var userId = context.Users.Where(x => x.NameSurname == name).Select(y => y.Id).FirstOrDefault();

                    var userRoleId = context.UserRoles.Where(x => x.UserId == userId).Select(y => y.RoleId).FirstOrDefault();
                    var roleType = context.Roles.Where(x => x.Id == userRoleId).Select(y => y.RolType).FirstOrDefault();

                    if (roleType == (int)UserRoleTypeEnum.Admin)
                    {
                        return RedirectToRoute(new { action = "Index", controller = "Widget", area = "Admin" });
                    }
                    else if(roleType == (int)UserRoleTypeEnum.Yazar)
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else if (roleType == (int)UserRoleTypeEnum.Üye)
                    {
                        return RedirectToAction("Index", "Blog");
                    }

                }
                else
                {
                    return RedirectToAction("Index", "Dashboard");
                    //return RedirectToAction("Index", "Login");
                }
            }
            return View();
        }    
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Login");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
