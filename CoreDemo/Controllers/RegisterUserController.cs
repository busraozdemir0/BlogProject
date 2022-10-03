
using CoreDemo.Models;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using EntityLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    [AllowAnonymous]
    public class RegisterUserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterUserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            Context context = new Context();
            var roleList = context.Roles.Where(x => x.RolType != ((int)UserRoleTypeEnum.Admin)).ToList();
            List<SelectListItem> roller = (from x in roleList
                                           select new SelectListItem
                                           {
                                               Text = x.Name,
                                               Value = x.Id.ToString()
                                           }).ToList();

            ViewBag.roller = roller;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(UserSignUpViewModel p)
        {
            Context context = new Context();
            var roleList = context.Roles.Where(x => x.RolType != ((int)UserRoleTypeEnum.Admin)).ToList();
            List<SelectListItem> roller = (from x in roleList
                                           select new SelectListItem
                                           {
                                               Text = x.Name,
                                               Value = x.Id.ToString()
                                           }).ToList();

            ViewBag.roller = roller;

            if (!p.IsAcceptTheContract)
            {
                ModelState.AddModelError("IsAcceptTheContract", "Sayfamıza kayıt olabilmek için  gizlilik sözleşmesini kabul etmeniz gerekmektedir.");
                return View(p);
            }
            if (ModelState.IsValid)
            {
                var rolename = context.Roles.Find(p.RolId).ToString();

                AppUser user = new AppUser()
                {
                    Email = p.Mail,
                    UserName = p.UserName,
                    NameSurname = p.NameSurname
                };
                var result = await _userManager.CreateAsync(user, p.Password);
                if(result.Succeeded)
                {
                    var defaultRole = rolename;
                    if(defaultRole!=null)
                    {
                        IdentityResult roleresult = await _userManager.AddToRoleAsync(user, defaultRole);
                    }
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    foreach(var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(p);
        }
    }
}
